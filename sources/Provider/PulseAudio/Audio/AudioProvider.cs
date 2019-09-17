// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Composition;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TerraFX.Audio;
using TerraFX.Interop;
using TerraFX.Utilities;

using static TerraFX.Interop.Pulse;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.PulseAudio.Audio
{
    /// <summary>Provides access to a PulseAudio-based audio subsystem.</summary>
    [Export(typeof(IAudioProvider))]
    [Shared]
    public sealed class AudioProvider : IDisposable, IAudioProvider
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Stopping = 4;

        private readonly Lazy<IntPtr> _mainloop;
        private readonly Lazy<IntPtr> _context;
        private readonly SemaphoreSlim _mainLoopMutex;

        private Thread? _mainLoopThread;
        private State _state;

        private unsafe pa_mainloop* MainLoop => (pa_mainloop*)_mainloop.Value;
        private unsafe pa_context* Context => (pa_context*)_context.Value;

        /// <summary>Initializes a new instance of the <see cref="AudioProvider" /> class.</summary>
        [ImportingConstructor]
        public AudioProvider()
        {
            _mainloop = new Lazy<IntPtr>(CreateMainLoop, isThreadSafe: true);
            _context = new Lazy<IntPtr>(CreateContext, isThreadSafe: true);
            _mainLoopMutex = new SemaphoreSlim(1);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="AudioProvider" /> class.</summary>
        ~AudioProvider()
        {
            Dispose(false);
        }

        private unsafe IntPtr CreateMainLoop()
        {
            var mainloop = pa_mainloop_new();

            if (mainloop == null)
            {
                ThrowExternalException(nameof(CreateMainLoop), -1);
            }

            return (IntPtr)mainloop;
        }

        private unsafe IntPtr CreateContext()
        {
            var api = pa_mainloop_get_api(MainLoop);

            var context = pa_context_new(api, null);

            if (context == null)
            {
                ThrowExternalException(nameof(CreateContext), -1);
            }

            return (IntPtr)context;
        }


        /// <inheritdoc/>
        public async ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            if (_state.TryTransition(from: Initialized, to: Starting) != Initialized)
            {
                throw new InvalidOperationException("Provider is already starting");
            }

            unsafe
            {
                if (pa_context_connect(Context, null, pa_context_flags.PA_CONTEXT_NOFLAGS, null) < 0)
                {
                    ThrowExternalException(nameof(StartAsync), pa_context_errno(Context));
                }
            }

            _mainLoopThread = new Thread(ThreadMain)
            {
                Name = "TerraFX PulseAudio main loop thread"
            };
            _mainLoopThread.Start();

            await WaitForState(pa_context_state.PA_CONTEXT_READY);
            _ = _state.Transition(to: Running);

            void ThreadMain()
            {
                var exitRequested = false;

                while (!exitRequested)
                {
                    try
                    {
                        _mainLoopMutex.Wait();

                        exitRequested = RunMainLoopIteration();
                    }
                    finally
                    {
                        _mainLoopMutex.Release();
                    }
                }
            }
        }

        // Runs an iteration of the main loop. Returns true to indicate exit request.
        private unsafe bool RunMainLoopIteration()
        {
            int retval = 0;
            int status = pa_mainloop_iterate(MainLoop, 1, &retval);

            if (status < 0)
            {
                if (retval == 0)
                {
                    ThrowExternalException(nameof(RunMainLoopIteration), status);
                }

                return true;
            }

            return false;
        }

        private async Task WaitForState(pa_context_state desired)
        {
            pa_context_state current = default;

            while (current != desired)
            {
                try
                {
                    await _mainLoopMutex.WaitAsync();

                    unsafe
                    {
                        current = pa_context_get_state(Context);
                    }
                }
                finally
                {
                    _mainLoopMutex.Release();
                }
            }
        }

        /// <inheritdoc/>
        public async ValueTask StopAsync(CancellationToken cancellationToken = default)
        {
            if (_state.TryTransition(from: Running, to: Stopping) != Running)
            {
                throw new InvalidOperationException("Provider cannot be stopped in this state");
            }

            try
            {
                // TODO: possible deadlock if mainloop re-enters the mutex before us?
                unsafe
                {
                    pa_mainloop_wakeup(MainLoop);
                }

                await _mainLoopMutex.WaitAsync();

                unsafe
                {
                    pa_mainloop_quit(MainLoop, 1);
                }
            }
            finally
            {
                _mainLoopMutex.Release();
            }

            if (_mainLoopThread != null)
            {
                _mainLoopThread.Join();
            }

            _state.Transition(to: Initialized);
        }

        // Small helper class to explicitly capture state for EnumerateAudioDevices
        // Using a class to avoid boxing/unboxing a value type and potentially mutating a copied instance
        private unsafe class AudioDeviceEnumeratorHelper
        {
            public AudioDeviceEnumeratorHelper(PulseAudioAdapterEnumerable enumerable)
            {
                this.enumerable = enumerable;
            }

            public PulseAudioAdapterEnumerable enumerable;
            public pa_operation* source_op;
            public pa_operation* sink_op;
            public int completed;
        }

        /// <summary>
        /// Returns an enumerable which can be used to discover the input and output devices supported by PulseAudio
        /// </summary>
        /// <remarks>
        /// Synchronous iteration of the returned enumerable is implemented via async-over-sync code,
        /// meaning deadlocks may occur if extra care is not taken.
        /// </remarks>
        /// <returns>
        /// An enumerable supporting asynchronous iteration.
        /// </returns>
        public unsafe PulseAudioAdapterEnumerable EnumerateAudioDevices()
        {
            AudioDeviceEnumeratorHelper helper = new AudioDeviceEnumeratorHelper(
                new PulseAudioAdapterEnumerable(AddSourceDevice, AddSinkDevice));
            var handle = GCHandle.Alloc(helper);
            var userdata = (void*)GCHandle.ToIntPtr(handle);

            helper.source_op = pa_context_get_source_info_list(Context, helper.enumerable.SourceCallback, userdata);
            helper.sink_op = pa_context_get_sink_info_list(Context, helper.enumerable.SinkCallback, userdata);

            static void AddSourceDevice(pa_context* c, pa_source_info* i, int eol, void* userdata)
            {
                var handle = GCHandle.FromIntPtr((IntPtr)userdata);
                var helper = (AudioDeviceEnumeratorHelper)handle.Target!;

                if (i != null)
                {
                    helper.enumerable.Add(i);
                }

                if (eol != 0)
                {
                    pa_operation_unref(helper.source_op);

                    if (Interlocked.Increment(ref helper.completed) == 2)
                    {
                        helper.enumerable.Complete();
                        handle.Free();
                    }
                }
            }

            static void AddSinkDevice(pa_context* c, pa_sink_info* i, int eol, void* userdata)
            {
                var handle = GCHandle.FromIntPtr((IntPtr)userdata);
                var helper = (AudioDeviceEnumeratorHelper)handle.Target!;

                if (i != null)
                {
                    helper.enumerable.Add(i);
                }

                if (eol != 0)
                {
                    pa_operation_unref(helper.sink_op);

                    if (Interlocked.Increment(ref helper.completed) == 2)
                    {
                        helper.enumerable.Complete();
                        handle.Free();
                    }
                }
            }

            return helper.enumerable;
        }

        /// <inheritdoc/>
        IAudioAdapterEnumerable IAudioProvider.EnumerateAudioDevices()
        {
            return EnumerateAudioDevices();
        }

        /// <inheritdoc/>
        public ValueTask<IAudioPlaybackDevice> RequestAudioPlaybackDeviceAsync(IAudioAdapter adapter)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ValueTask<IAudioRecordingDevice> RequestAudioRecordingDeviceAsync(IAudioAdapter adapter)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private unsafe void Dispose(bool disposing)
        {
            // TODO: check if running and stop if necessary

            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                if (disposing)
                {
                    _mainLoopMutex.Dispose();
                }

                if (_context.IsValueCreated)
                {
                    pa_context_unref((pa_context*)_context.Value);
                }

                if (_mainloop.IsValueCreated)
                {
                    pa_mainloop_free((pa_mainloop*)_mainloop.Value);
                }
            }

            _state.EndDispose();
        }
    }
}
