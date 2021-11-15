// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Interop.PulseAudio;
using TerraFX.Threading;
using static TerraFX.Interop.PulseAudio.pa_sample_format_t;
using static TerraFX.Interop.PulseAudio.pa_seek_mode_t;
using static TerraFX.Interop.PulseAudio.pa_stream_flags_t;
using static TerraFX.Interop.PulseAudio.PulseAudio;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Audio
{
    /// <inheritdoc />
    public sealed class PulseAudioPlaybackDevice : IAudioPlaybackDevice
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Completed = 4;

        private static readonly byte[] s_playbackName = Encoding.UTF8.GetBytes("Playback");

        private readonly unsafe pa_context* _context;
        private readonly Pipe _sampleDataPipe;
        private readonly Lazy<Pointer<pa_stream>> _stream;
        private readonly ValueLazy<GCHandle> _writeDelegateHandle;
        private readonly unsafe delegate* unmanaged<pa_stream*, nuint, void*, void> _writeDelegate;
        private readonly ManualResetValueTaskSource<int> _writeRequest;

        private VolatileState _state;

        /// <inheritdoc />
        public IAudioAdapter Adapter { get; }

        /// <inheritdoc />
        public PipeWriter Writer => _sampleDataPipe.Writer;

        private GCHandle WriteDelegateHandle => _writeDelegateHandle.Value;

        private unsafe pa_stream* Stream => _stream.Value;

        /// <summary>Initializes a new instance of the <see cref="PulseAudioPlaybackDevice" /> class.</summary>
        internal unsafe PulseAudioPlaybackDevice(IAudioAdapter adapter, pa_context* context)
        {
            Assert(AssertionsEnabled && (context != null));

            if (adapter.DeviceType != AudioDeviceType.Playback)
            {
                ThrowForInvalidKind(adapter.DeviceType, nameof(adapter), AudioDeviceType.Playback);
            }

            _context = context;
            _sampleDataPipe = new Pipe();
            _stream = new Lazy<Pointer<pa_stream>>(CreateStream, isThreadSafe: true);
            _writeDelegateHandle = new ValueLazy<GCHandle>(CreateHandle);
            _writeDelegate = &WriteCallback;
            _writeRequest = new ManualResetValueTaskSource<int>() {
                RunContinuationsAsynchronously = true
            };

            Adapter = adapter;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="PulseAudioPlaybackDevice" /> class.</summary>
        ~PulseAudioPlaybackDevice() => Dispose(false);

        private GCHandle CreateHandle() => GCHandle.Alloc(this);

        private unsafe Pointer<pa_stream> CreateStream()
        {
            pa_sample_spec spec;

            spec.channels = (byte)Adapter.Channels;
            spec.format = GetSampleFormat(Adapter);
            spec.rate = (uint)Adapter.SampleRate;

            pa_stream* stream;
            fixed(byte* streamName = s_playbackName)
            {
                stream = pa_stream_new(_context, (sbyte*)streamName, &spec, null);
            }

            Assert(AssertionsEnabled && (stream != null));

            var userdata = (void*)GCHandle.ToIntPtr(WriteDelegateHandle);
            pa_stream_set_write_callback(stream, _writeDelegate, userdata);

            return stream;

            static pa_sample_format_t GetSampleFormat(IAudioAdapter adapter)
            {
                // TODO: make this return some appropriate value
                return PA_SAMPLE_S16LE;
            }
        }

        // Called from the event loop thread when it wants audio data.
        // This happens infrequently, so in theory using a valuetask-based event should be fine
        private short _lastToken = -1; // cache the previous token used so we can detect cases where we're called twice

        [UnmanagedCallersOnly]
        private static unsafe void WriteCallback(pa_stream* stream, nuint length, void* userdata)
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
        public unsafe void Reset()
        {
            _state.Transition(from: Completed, to: Initialized);
            Assert(AssertionsEnabled && (pa_stream_disconnect(Stream) == 0));
            _sampleDataPipe.Reset();
        }

        /// <inheritdoc />
        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _state.Transition(from: Initialized, to: Starting);

            unsafe
            {
                if (pa_stream_connect_playback(Stream, null, null, PA_STREAM_NOFLAGS, null, null) != 0)
                {
                    ThrowExternalException(nameof(pa_stream_connect_playback), pa_context_errno(_context));
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

                    var status = TryPrepareAndWriteBlock(result.Buffer, bytesToWrite, out var written);
                    Assert(AssertionsEnabled && status);
                    bytesWritten += written;
                }
            }

            _ = _state.TryTransition(from: Running, to: Completed);

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
                var bytesAvailable = (nuint)length;

                Assert(AssertionsEnabled && (pa_stream_begin_write(Stream, &writeLocation, &bytesAvailable) == 0));
                AssertNotNull(writeLocation);

                var destination = new Span<byte>(writeLocation, (int)bytesAvailable);
                data.Slice(0, destination.Length).CopyTo(destination);
                block = destination;

                return true;
            }

            unsafe bool TryWriteBlock(Span<byte> block)
            {
                fixed(byte* location = block)
                {
                    Assert(AssertionsEnabled && (pa_stream_write(Stream, location, (nuint)block.Length, null, 0, PA_SEEK_RELATIVE) == 0));
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

        private unsafe void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                if (_stream.IsValueCreated)
                {
                    pa_stream_unref(_stream.Value);
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
