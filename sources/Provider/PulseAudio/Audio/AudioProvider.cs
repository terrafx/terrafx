using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Audio;
using TerraFX.Interop;

namespace TerraFX.Provider.PulseAudio.Audio
{
    [Export(typeof(IAudioProvider))]
    [Export(typeof(AudioProvider))]
    [Shared]
    public sealed class AudioProvider : IDisposable, IAudioProvider
    {
        private static readonly byte[] PulseContextName =
            Encoding.UTF8.GetBytes("TerraFX");

        private IntPtr _pulseMainLoop = IntPtr.Zero;
        private IntPtr _pulseContext = IntPtr.Zero;

        private int _connectionState;
        private SemaphoreSlim _stateChangeSignal = new SemaphoreSlim(1);
        private bool _disposed = false;
        private bool _connected = false;

        public unsafe AudioProvider()
        {
            _pulseMainLoop = libpulse.pa_threaded_mainloop_new();

            if (_pulseMainLoop == IntPtr.Zero)
                throw new InvalidOperationException(
                    "Could not create PulseAudio event loop");

            var api = libpulse.pa_threaded_mainloop_get_api(_pulseMainLoop);
            fixed (byte* pulseContextName = PulseContextName)
                _pulseContext = libpulse.pa_context_new(api, pulseContextName);

            if (_pulseContext == IntPtr.Zero)
            {
                libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);
                libpulse.pa_threaded_mainloop_free(_pulseMainLoop);

                throw new InvalidOperationException(
                    "Could not create PulseAudio context");
            }

            libpulse.pa_context_set_state_callback(_pulseContext, ContextStateCallback, IntPtr.Zero);
        }

        ~AudioProvider()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public unsafe ValueTask StartAsync()
        {
            if (_connected)
                throw new InvalidOperationException("Already connected to the PulseAudio daemon");

            libpulse.pa_threaded_mainloop_lock(_pulseMainLoop);

            if (libpulse.pa_context_connect(_pulseContext, null, 0, IntPtr.Zero) < 0)
            {
                var message = ErrorMappings.GetErrorMapping(
                    libpulse.pa_context_errno(_pulseContext));

                libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);
                throw new InvalidOperationException($"Could not connect to the PulseAudio daemon: {message}");
            }

            if (libpulse.pa_threaded_mainloop_start(_pulseMainLoop) < 0)
            {
                libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);
                throw new InvalidOperationException("Could not start the PulseAudio main loop");
            }

            return new ValueTask(WaitUntilReady());
        }

        private async Task WaitUntilReady()
        {
            libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);

            while (true)
            {
                await _stateChangeSignal.WaitAsync().ConfigureAwait(false);
                var state = _connectionState;

                if (state == libpulse.PA_CONTEXT_READY)
                    break;
                else if (state == libpulse.PA_CONTEXT_FAILED || state == libpulse.PA_CONTEXT_TERMINATED)
                    throw new InvalidOperationException($"Could not connect to the PulseAudio daemon (state={state})");
            }

            _connected = true;
        }

        public ValueTask StopAsync()
        {
            libpulse.pa_threaded_mainloop_lock(_pulseMainLoop);

            libpulse.pa_context_disconnect(_pulseContext);
            libpulse.pa_threaded_mainloop_stop(_pulseMainLoop);


            _connected = false;
            libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);
            return new ValueTask();
        }


        public unsafe IEnumerable<IAudioDeviceOptions> EnumerateAudioDevices()
        {
            if (!_connected)
                throw new InvalidOperationException("Not connected to the PulseAudio daemon");

            libpulse.pa_threaded_mainloop_lock(_pulseMainLoop);

            var devices = new BlockingCollection<IAudioDeviceOptions>();
            var completed = 0;
            IntPtr source_op = IntPtr.Zero, sink_op = IntPtr.Zero;
            source_op = libpulse.pa_context_get_source_info_list(_pulseContext, HandleSourceDevice, IntPtr.Zero);
            sink_op = libpulse.pa_context_get_sink_info_list(_pulseContext, HandleSinkDevice, IntPtr.Zero);

            libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);

            return devices.GetConsumingEnumerable();

            void HandleSourceDevice(IntPtr c, pa_source_info* i, int eol, IntPtr userdata)
            {
                if (i != null)
                    devices.Add(new SourceDeviceOptions(i));

                if (eol != 0)
                {
                    libpulse.pa_operation_unref(source_op);

                    if (++completed == 2)
                    {
                        devices.CompleteAdding();
                    }
                }
            }

            void HandleSinkDevice(IntPtr c, pa_sink_info* i, int eol, IntPtr userdata)
            {
                if (i != null)
                    devices.Add(new SinkDeviceOptions(i));

                if (eol != 0)
                {
                    libpulse.pa_operation_unref(sink_op);

                    if (++completed == 2)
                    {
                        devices.CompleteAdding();
                    }
                }
            }
        }

        public ValueTask<IAudioDevice> RequestAudioDeviceAsync(IAudioDeviceOptions? options = null)
        {
            throw new NotImplementedException();
        }

        private void ContextStateCallback(IntPtr context, IntPtr userdata)
        {
            var state = libpulse.pa_context_get_state(context);

            _connectionState = state;
            if (_stateChangeSignal.CurrentCount < 1)
                _ = _stateChangeSignal.Release();
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // no-op: no managed resources to dispose
                }

                // Order of destruction based on chromium sources

                if (_pulseMainLoop != IntPtr.Zero)
                {
                    libpulse.pa_threaded_mainloop_lock(_pulseMainLoop);
                    libpulse.pa_threaded_mainloop_stop(_pulseMainLoop);
                }

                if (_pulseContext != IntPtr.Zero)
                {
                    libpulse.pa_context_disconnect(_pulseContext);
                    libpulse.pa_context_unref(_pulseContext);

                    _pulseContext = IntPtr.Zero;
                }

                if (_pulseMainLoop != IntPtr.Zero)
                {
                    libpulse.pa_threaded_mainloop_unlock(_pulseMainLoop);
                    libpulse.pa_threaded_mainloop_free(_pulseMainLoop);

                    _pulseMainLoop = IntPtr.Zero;
                }
            }

            _disposed = true;
        }
    }
}
