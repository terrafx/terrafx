// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Interop.PulseAudio;
using TerraFX.Threading;
using static TerraFX.Interop.PulseAudio.pa_context_flags_t;
using static TerraFX.Interop.PulseAudio.pa_context_state_t;
using static TerraFX.Interop.PulseAudio.PulseAudio;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Audio
{
    /// <summary>Provides access to a PulseAudio-based audio subsystem.</summary>
    public sealed class PulseAudioService : IAudioService, IDisposable, IAsyncDisposable
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Stopping = 4;

        private readonly Lazy<Pointer<pa_mainloop>> _mainloop;
        private readonly Lazy<Pointer<pa_context>> _context;
        private readonly SemaphoreSlim _mainLoopMutex;

        private Thread? _mainLoopThread;
        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="PulseAudioService" /> class.</summary>
        public PulseAudioService()
        {
            _mainloop = new Lazy<Pointer<pa_mainloop>>(CreateMainLoop, isThreadSafe: true);
            _context = new Lazy<Pointer<pa_context>>(CreateContext, isThreadSafe: true);
            _mainLoopMutex = new SemaphoreSlim(initialCount: 1);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="PulseAudioService" /> class.</summary>
        ~PulseAudioService() => DisposeAsync(false).Wait();

        /// <summary>Gets the underlying native pointer for the PulseAudio context.</summary>
        public unsafe pa_context* Context => _context.Value;

        /// <summary>Gets the underlying native pointer for the PulseAudio main loop.</summary>
        public unsafe pa_mainloop* MainLoop => _mainloop.Value;

        private unsafe Pointer<pa_context> CreateContext()
        {
            var api = pa_mainloop_get_api(MainLoop);
            var context = pa_context_new(api, null);

            if (context == null)
            {
                ThrowExternalException(errorCode: -1, methodName: nameof(CreateContext));
            }
            return context;
        }

        private unsafe Pointer<pa_mainloop> CreateMainLoop()
        {
            var mainloop = pa_mainloop_new();

            if (mainloop == null)
            {
                ThrowExternalException(errorCode: -1, methodName: nameof(CreateMainLoop));
            }
            return mainloop;
        }

        /// <inheritdoc/>
        public async ValueTask StartAsync(CancellationToken cancellationToken = default)
        {
            _state.Transition(from: Initialized, to: Starting);

            unsafe
            {
                if (pa_context_connect(Context, null, PA_CONTEXT_NOFLAGS, null) < 0)
                {
                    ThrowExternalException(nameof(StartAsync), pa_context_errno(Context));
                }
            }

            _mainLoopThread = new Thread(ThreadMain) {
                Name = "TerraFX PulseAudio main loop thread"
            };
            _mainLoopThread.Start();

            await WaitForStateAsync(PA_CONTEXT_READY);
            _ = _state.Transition(to: Running);

            void ThreadMain()
            {
                var exitRequested = false;

                while (!exitRequested)
                {
                    try
                    {
                        _mainLoopMutex.Wait(cancellationToken);
                        exitRequested = RunMainLoopIteration();
                    }
                    catch (Exception)
                    {
                        unsafe
                        {
                            // Disconnect in case of an unhandled exception, so we're in a safe state to reconnect if the user wishes
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
            AssertNotDisposedOrDisposing(_state);

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

        private async Task WaitForStateAsync(pa_context_state_t desired)
        {
            pa_context_state_t current = default;

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
            ThrowIfDisposedOrDisposing(_state, nameof(PulseAudioService));

            _state.Transition(from: Running, to: Stopping);
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

        /// <inheritdoc cref="IAudioService.EnumerateAudioDevices()"/>
        public unsafe PulseAudioAdapterEnumerable EnumerateAudioDevices()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(PulseAudioService));

            if (_state != Running)
            {
                ThrowForInvalidState(nameof(Running));
            }

            AssertNotNull(_mainLoopThread);

            var handle = GCHandle.Alloc(new AudioDeviceEnumeratorHelper());
            var userdata = (void*)GCHandle.ToIntPtr(handle);
            ref var helper = ref Unsafe.Unbox<AudioDeviceEnumeratorHelper>(handle.Target!);

            helper.Enumerable = new PulseAudioAdapterEnumerable(_mainLoopThread!, &AddSourceDevice, &AddSinkDevice);
            helper.SourceOp = pa_context_get_source_info_list(Context, helper.Enumerable.SourceCallback, userdata);
            helper.SinkOp = pa_context_get_sink_info_list(Context, helper.Enumerable.SinkCallback, userdata);

            return helper.Enumerable;

            [UnmanagedCallersOnly]
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

            [UnmanagedCallersOnly]
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
        IAudioAdapterEnumerable IAudioService.EnumerateAudioDevices() => EnumerateAudioDevices();

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

            if (priorState is > Initialized and < Stopping)
            {
                await StopAsyncInternalAsync();
            }

            if (priorState < Disposing)
            {
                if (disposing)
                {
                    _mainLoopMutex.Dispose();
                }

                if (_context.IsValueCreated)
                {
                    unsafe
                    {
                        pa_context_unref(_context.Value);
                    }
                }

                if (_mainloop.IsValueCreated)
                {
                    unsafe
                    {
                        pa_mainloop_free(_mainloop.Value);
                    }
                }
            }

            _state.EndDispose();
        }
    }
}
