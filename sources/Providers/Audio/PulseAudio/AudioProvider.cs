// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Interop;
using TerraFX.Utilities;

using static TerraFX.Interop.Pulse;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Audio.Providers.PulseAudio
{
    /// <summary>Provides access to a PulseAudio-based audio subsystem.</summary>
    [Export(typeof(IAudioProvider))]
    [Shared]
    public sealed class AudioProvider : IDisposable, IAsyncDisposable, IAudioProvider
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Stopping = 4;

        private readonly Lazy<IntPtr> _mainloop;
        private readonly Lazy<IntPtr> _context;
        private readonly SemaphoreSlim _mainLoopMutex;

        private Thread? _mainLoopThread;
        private State _state;

        /// <summary>Gets the underlying native pointer for the PulseAudio main loop.</summary>
        public unsafe pa_mainloop* MainLoop => (pa_mainloop*)_mainloop.Value;
        /// <summary>Gets the underlying native pointer for the PulseAudio context.</summary>
        public unsafe pa_context* Context => (pa_context*)_context.Value;

        /// <summary>Initializes a new instance of the <see cref="AudioProvider" /> class.</summary>
        [ImportingConstructor]
        public AudioProvider()
        {
            _mainloop = new Lazy<IntPtr>(CreateMainLoop, isThreadSafe: true);
            _context = new Lazy<IntPtr>(CreateContext, isThreadSafe: true);
            _mainLoopMutex = new SemaphoreSlim(initialCount: 1);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="AudioProvider" /> class.</summary>
        ~AudioProvider()
        {
            DisposeAsync(false).Wait();
        }

        private unsafe IntPtr CreateMainLoop()
        {
            var mainloop = pa_mainloop_new();

            if (mainloop == null)
            {
                ThrowExternalException(nameof(CreateMainLoop), errorCode: -1);
            }

            return (IntPtr)mainloop;
        }

        private unsafe IntPtr CreateContext()
        {
            var api = pa_mainloop_get_api(MainLoop);

            var context = pa_context_new(api, null);

            if (context == null)
            {
                ThrowExternalException(nameof(CreateContext), errorCode: -1);
            }

            return (IntPtr)context;
        }


        /// <inheritdoc/>
        public async ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            if (_state.TryTransition(from: Initialized, to: Starting) != Initialized)
            {
                ThrowInvalidOperationException(Resources.ProviderAlreadyStartedMessage);
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

            await WaitForStateAsync(pa_context_state.PA_CONTEXT_READY);
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
                    catch (Exception)
                    {
                        // Disconnect in case of an unhandled exception, so we're in a safe state to reconnect if the user wishes
                        unsafe
                        {
                            pa_context_disconnect(Context);
                        }
                        _ = _state.Transition(to: Initialized);

                        throw;
                    }
                    finally
                    {
                        _ = _mainLoopMutex.Release();
                    }
                }
            }
        }

        // Runs an iteration of the main loop. Returns true to indicate exit request.
        private unsafe bool RunMainLoopIteration()
        {
            _state.AssertNotDisposedOrDisposing();

            var retval = 0;
            var status = pa_mainloop_iterate(MainLoop, 1, &retval);

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

        private async Task WaitForStateAsync(pa_context_state desired)
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
                    _ = _mainLoopMutex.Release();
                }
            }
        }

        /// <inheritdoc/>
        public async ValueTask StopAsync(CancellationToken cancellationToken = default)
        {
            _state.ThrowIfDisposedOrDisposing();

            if (_state.TryTransition(from: Running, to: Stopping) != Running)
            {
                ThrowInvalidOperationException(string.Format(Resources.ProviderCannotBeStoppedMessage, _state));
            }

            await StopAsyncInternalAsync(cancellationToken);

            _ = _state.Transition(to: Initialized);
        }

        private async Task StopAsyncInternalAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // TODO: make WaitAsync() timeout configurable
                do
                {
                    unsafe
                    {
                        pa_mainloop_wakeup(MainLoop);
                    }
                }
                while (!await _mainLoopMutex.WaitAsync(millisecondsTimeout: 8, cancellationToken));

                unsafe
                {
                    pa_mainloop_quit(MainLoop, retval: 1);
                }
            }
            finally
            {
                _ = _mainLoopMutex.Release();
            }

            if (_mainLoopThread != null)
            {
                _mainLoopThread.Join();
            }
        }

        // Small helper struct to explicitly capture state for EnumerateAudioDevices
        private unsafe struct AudioDeviceEnumeratorHelper
        {
            public PulseAudioAdapterEnumerable Enumerable;
            public pa_operation* SourceOp;
            public pa_operation* SinkOp;
            public int Completed;
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
            _state.ThrowIfDisposedOrDisposing();
            if (_state != Running)
            {
                ThrowInvalidOperationException(Resources.CannotEnumerateAudioDevicesWhenNotRunningMessage);
            }

            Assert(_mainLoopThread != null, "Mainloop should not be null");

            var handle = GCHandle.Alloc(new AudioDeviceEnumeratorHelper());
            var userdata = (void*)GCHandle.ToIntPtr(handle);
            ref var helper = ref Unsafe.Unbox<AudioDeviceEnumeratorHelper>(handle.Target!);

            helper.Enumerable = new PulseAudioAdapterEnumerable(_mainLoopThread!, AddSourceDevice, AddSinkDevice);
            helper.SourceOp = pa_context_get_source_info_list(Context, helper.Enumerable.SourceCallback, userdata);
            helper.SinkOp = pa_context_get_sink_info_list(Context, helper.Enumerable.SinkCallback, userdata);

            return helper.Enumerable;

            static void AddSourceDevice(pa_context* c, pa_source_info* i, int eol, void* userdata)
            {
                var handle = GCHandle.FromIntPtr((IntPtr)userdata);
                IAudioAdapter? adapter = null;

                if (i != null)
                {
                    adapter = new PulseSourceAdapter(i);
                }

                AddAdapter(adapter, eol, handle);
            }

            static void AddSinkDevice(pa_context* c, pa_sink_info* i, int eol, void* userdata)
            {
                var handle = GCHandle.FromIntPtr((IntPtr)userdata);
                IAudioAdapter? adapter = null;

                if (i != null)
                {
                    adapter = new PulseSinkAdapter(i);
                }

                AddAdapter(adapter, eol, handle);
            }

            static void AddAdapter(IAudioAdapter? adapter, int eol, GCHandle handle)
            {
                ref var helper = ref Unsafe.Unbox<AudioDeviceEnumeratorHelper>(handle.Target!);

                if (adapter != null)
                {
                    helper.Enumerable.Add(adapter);
                }

                if (eol != 0)
                {
                    if (Interlocked.Increment(ref helper.Completed) == 2)
                    {
                        pa_operation_unref(helper.SourceOp);
                        pa_operation_unref(helper.SinkOp);
                        helper.Enumerable.Complete();
                        handle.Free();
                    }
                }
            }
        }

        /// <inheritdoc/>
        IAudioAdapterEnumerable IAudioProvider.EnumerateAudioDevices() => EnumerateAudioDevices();

        /// <inheritdoc/>
        public unsafe ValueTask<IAudioPlaybackDevice> RequestAudioPlaybackDeviceAsync(IAudioAdapter adapter) => new ValueTask<IAudioPlaybackDevice>(new PulseAudioPlaybackDevice(adapter, Context));

        /// <inheritdoc/>
        public ValueTask<IAudioRecordingDevice> RequestAudioRecordingDeviceAsync(IAudioAdapter adapter) => throw new NotImplementedException();

        /// <inheritdoc/>
        public void Dispose()
        {
            DisposeAsync(true).Wait();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        private async Task DisposeAsync(bool disposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState > Initialized && priorState < Stopping)
            {
                await StopAsyncInternalAsync();
            }

            if (priorState < Disposing)
            {
                if (disposing)
                {
                    _mainLoopMutex.Dispose();
                }

                unsafe
                {
                    if (_context.IsValueCreated)
                    {
                        pa_context_unref((pa_context*)_context.Value);
                    }

                    if (_mainloop.IsValueCreated)
                    {
                        pa_mainloop_free((pa_mainloop*)_mainloop.Value);
                    }
                }
            }

            _state.EndDispose();
        }
    }
}
