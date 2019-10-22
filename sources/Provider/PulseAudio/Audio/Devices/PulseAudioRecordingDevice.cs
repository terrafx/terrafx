// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Audio;
using TerraFX.Interop;
using TerraFX.Utilities;

using static TerraFX.Interop.Pulse;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.PulseAudio.Audio
{
    /// <inheritdoc />
    public sealed class PulseAudioRecordingDevice : IAudioRecordingDevice
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Completed = 4;

        private static readonly byte[] RecordingName = Encoding.UTF8.GetBytes("Recording");

        private unsafe readonly pa_context* _context;
        private readonly Pipe _sampleDataPipe;
        private readonly Lazy<IntPtr> _stream;
        private readonly Lazy<GCHandle> _readDelegateHandle;
        private readonly NativeDelegate<pa_stream_request_cb_t> _readDelegate;
        private readonly ManualResetValueTaskSource<int> _readRequest;

        private State _state;

        /// <inheritdoc />
        public IAudioAdapter Adapter { get; }
        /// <inheritdoc />
        public PipeReader Reader => _sampleDataPipe.Reader;

        private GCHandle ReadDelegateHandle => _readDelegateHandle.Value;
        private unsafe pa_stream* Stream => (pa_stream*)_stream.Value;

        /// <summary>Initializes a new instance of the <see cref="PulseAudioRecordingDevice" /> class.</summary>
        internal unsafe PulseAudioRecordingDevice(IAudioAdapter adapter, pa_context* context)
        {
            Assert(context != null, "pa_context passed was null");

            if (adapter.DeviceType != AudioDeviceType.Recording)
            {
                ThrowInvalidOperationException(nameof(adapter), adapter.DeviceType);
            }

            _context = context;
            _sampleDataPipe = new Pipe();
            _stream = new Lazy<IntPtr>(CreateStream, isThreadSafe: true);
            _readDelegateHandle = new Lazy<GCHandle>(CreateHandle, isThreadSafe: true);
            _readDelegate = new NativeDelegate<pa_stream_request_cb_t>(ReadCallback);
            _readRequest = new ManualResetValueTaskSource<int>()
            {
                RunContinuationsAsynchronously = true
            };

            Adapter = adapter;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="PulseAudioRecordingDevice" /> class.</summary>
        ~PulseAudioRecordingDevice()
        {
            Dispose(false);
        }

        private GCHandle CreateHandle()
        {
            return GCHandle.Alloc(this);
        }

        private unsafe IntPtr CreateStream()
        {
            pa_sample_spec spec;

            spec.channels = (byte)Adapter.Channels;
            spec.format = GetSampleFormat(Adapter);
            spec.rate = (uint)Adapter.SampleRate;

            pa_stream* stream;
            fixed(byte* streamName = RecordingName)
            {
                stream = pa_stream_new(_context, (sbyte*)streamName, &spec, null);
            }

            Assert(stream != null, "pa_stream_new failed");

            var userdata = (void*)GCHandle.ToIntPtr(ReadDelegateHandle);
            pa_stream_set_read_callback(stream, _readDelegate, userdata);

            return (IntPtr)stream;

            static pa_sample_format GetSampleFormat(IAudioAdapter adapter)
            {
                // TODO: make this return some appropriate value
                return pa_sample_format.PA_SAMPLE_S16LE;
            }
        }

        // Called from the event loop thread when it wants audio data.
        // This happens infrequently, so in theory using a valuetask-based event should be fine
        private short _lastToken = -1; // cache the previous token used so we can detect cases where we're called twice
        private static unsafe void ReadCallback(pa_stream* stream, UIntPtr length, void* userdata)
        {
            var handle = GCHandle.FromIntPtr((IntPtr)userdata);
            var device = (PulseAudioRecordingDevice)handle.Target!;

            if (device._readRequest.Token != device._lastToken)
            {
                device._lastToken = device._readRequest.Token;
                device._readRequest.Set((int)length);
            }
        }

        /// <summary>Resets the playback device to a usable state if it was completed.</summary>
        public void Reset()
        {
            if (_state.TryTransition(from: Completed, to: Initialized) != Completed)
            {
                ThrowInvalidOperationException(Resources.DeviceNotCompletedMessage);
            }

            unsafe
            {
                Assert(pa_stream_disconnect(Stream) == 0, "pa_stream_disconnect returned != 0");
            }

            _sampleDataPipe.Reset();
        }

        /// <inheritdoc />
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            if (_state.TryTransition(from: Initialized, to: Starting) != Initialized)
            {
                ThrowInvalidOperationException(Resources.DeviceAlreadyStartedMessage);
            }

            unsafe
            {
                if (pa_stream_connect_record(Stream, null, null, pa_stream_flags.PA_STREAM_NOFLAGS) != 0)
                {
                    ThrowInvalidOperationException(Resources.CouldNotConnectPlaybackStreamMessage);
                }
            }

            _ = _state.Transition(to: Running);

            var writer = _sampleDataPipe.Writer;
            FlushResult result = default;
            while (!result.IsCompleted)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var bytesAvailable = await new ValueTask<int>(_readRequest, _readRequest.Token);
                _readRequest.Reset();
                var bytesRead = 0;

                while (bytesRead < bytesAvailable)
                {
                    var buffer = writer.GetMemory(bytesAvailable);

                    if (TryReadBlock(buffer.Span, out int read))
                        writer.Advance(read);

                    bytesRead += read;
                }

                result = await writer.FlushAsync(cancellationToken);
            }

            _state.TryTransition(from: Running, to: Completed);

            unsafe bool TryReadBlock(Span<byte> location, out int read)
            {
                void* buffer;
                UIntPtr size;

                if (pa_stream_peek(Stream, &buffer, &size) != 0)
                {
                    read = 0;
                    Assert(false, "pa_stream_peak returned != 0");
                    return false;
                }

                if (size.ToUInt32() == 0)
                {
                    read = 0;
                    return false;
                }

                read = unchecked((int)size.ToUInt32());

                if (buffer != null)
                {
                    var bufferSpan = new Span<byte>(buffer, read);

                    if (!bufferSpan.TryCopyTo(location))
                    {
                        pa_stream_drop(Stream);
                        Assert(false, $"copy failed, buffer size = {read}, location size = {location.Length}");
                        return false;
                    }
                }

                pa_stream_drop(Stream);
                return true;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private unsafe void Dispose(bool disposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                if (_stream.IsValueCreated)
                {
                    pa_stream_unref((pa_stream*)_stream.Value);
                }

                if (_readDelegateHandle.IsValueCreated)
                {
                    _readDelegateHandle.Value.Free();
                }
            }

            _state.EndDispose();
        }
    }
}
