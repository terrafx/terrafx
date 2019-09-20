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
    public sealed class PulseAudioPlaybackDevice : IAudioPlaybackDevice
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Completed = 4;

        private static readonly byte[] PlaybackName = Encoding.UTF8.GetBytes("Playback");

        private unsafe readonly pa_context* _context;
        private readonly Pipe _sampleDataPipe;
        private readonly Lazy<IntPtr> _stream;
        private readonly Lazy<GCHandle> _writeDelegateHandle;
        private readonly NativeDelegate<pa_stream_request_cb_t> _writeDelegate;
        private readonly ManualResetValueTaskSource<int> _writeRequest;

        private State _state;

        /// <inheritdoc />
        public IAudioAdapter Adapter { get; }
        /// <inheritdoc />
        public PipeWriter Writer => _sampleDataPipe.Writer;

        private GCHandle WriteDelegateHandle => _writeDelegateHandle.Value;
        private unsafe pa_stream* Stream => (pa_stream*)_stream.Value;

        /// <summary>Initializes a new instance of the <see cref="PulseAudioPlaybackDevice" /> class.</summary>
        internal unsafe PulseAudioPlaybackDevice(IAudioAdapter adapter, pa_context* context)
        {
            Assert(context != null, "pa_context passed was null");

            if (adapter.DeviceType != AudioDeviceType.Playback)
            {
                ThrowInvalidOperationException(nameof(adapter), adapter.DeviceType);
            }

            _context = context;
            _sampleDataPipe = new Pipe();
            _stream = new Lazy<IntPtr>(CreateStream, isThreadSafe: true);
            _writeDelegateHandle = new Lazy<GCHandle>(CreateHandle, isThreadSafe: true);
            _writeDelegate = new NativeDelegate<pa_stream_request_cb_t>(WriteCallback);
            _writeRequest = new ManualResetValueTaskSource<int>()
            {
                RunContinuationsAsynchronously = true
            };

            Adapter = adapter;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="PulseAudioPlaybackDevice" /> class.</summary>
        ~PulseAudioPlaybackDevice()
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
            fixed(byte* streamName = PlaybackName)
            {
                stream = pa_stream_new(_context, (sbyte*)streamName, &spec, null);
            }

            Assert(stream != null, "pa_stream_new failed");

            var userdata = (void*)GCHandle.ToIntPtr(WriteDelegateHandle);
            pa_stream_set_write_callback(stream, _writeDelegate, userdata);

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
        private static unsafe void WriteCallback(pa_stream* stream, UIntPtr length, void* userdata)
        {
            var handle = GCHandle.FromIntPtr((IntPtr)userdata);
            var device = (PulseAudioPlaybackDevice)handle.Target!;

            if (device._writeRequest.Token != device._lastToken)
            {
                device._lastToken = device._writeRequest.Token;
                device._writeRequest.Set((int)length);
            }
        }

        /// <summary>Resets the playback device to a usable state if it was completed.</summary>
        public void Reset()
        {
            if (_state.TryTransition(from: Completed, to: Initialized) != Completed)
            {
                throw new InvalidOperationException("Device is not completed");
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
                throw new InvalidOperationException("Device is already started");
            }

            unsafe
            {
                if (pa_stream_connect_playback(Stream, null, null, pa_stream_flags.PA_STREAM_NOFLAGS, null, null) != 0)
                {
                    throw new InvalidOperationException("Could not connect stream for playback");
                }
            }

            _ = _state.Transition(to: Running);

            var reader = _sampleDataPipe.Reader;
            ReadResult result = default;
            while (!result.IsCompleted)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var bytesRequested = await new ValueTask<int>(_writeRequest, _writeRequest.Token);
                var bytesWritten = 0;

                while (bytesWritten < bytesRequested)
                {
                    result = await reader.ReadAsync(cancellationToken);
                    var bytesToWrite = bytesRequested - bytesWritten;

                    if (bytesToWrite > result.Buffer.Length)
                    {
                        bytesToWrite = (int)result.Buffer.Length;
                    }

                    var status = TryPrepareAndWriteBlock(result.Buffer, bytesToWrite, out int written);
                    Assert(status, "Failed to prepare and write block");
                    bytesWritten += written;
                }
            }

            _state.TryTransition(from: Running, to: Completed);

            bool TryPrepareAndWriteBlock(ReadOnlySequence<byte> data, int bytesRequested, out int bytesWritten)
            {
                if (!TryPrepareBlock(data, bytesRequested, out var block))
                {
                    bytesWritten = 0;
                    return false;
                }

                reader.AdvanceTo(data.GetPosition(block.Length));
                _writeRequest.Reset();

                bytesWritten = block.Length;
                return TryWriteBlock(block);
            }

            unsafe bool TryPrepareBlock(ReadOnlySequence<byte> data, int length, out Span<byte> block)
            {
                void* writeLocation = null;
                UIntPtr bytesAvailable = new UIntPtr((uint)length);
                Assert(pa_stream_begin_write(Stream, &writeLocation, &bytesAvailable) == 0, "pa_stream_begin_write returned != 0");
                Assert(writeLocation != null, "writeLocation == null");

                var destination = new Span<byte>(writeLocation, (int)bytesAvailable.ToUInt32());
                data.Slice(0, destination.Length).CopyTo(destination);
                block = destination;

                return true;
            }

            unsafe bool TryWriteBlock(Span<byte> block)
            {
                fixed(byte* location = block)
                {
                    Assert(pa_stream_write(Stream, (void*)location, new UIntPtr((uint)block.Length), IntPtr.Zero, IntPtr.Zero, pa_seek_mode.PA_SEEK_RELATIVE) == 0, "pa_stream_write returned != 0");
                }

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

                if (_writeDelegateHandle.IsValueCreated)
                {
                    _writeDelegateHandle.Value.Free();
                }
            }

            _state.EndDispose();
        }
    }
}
