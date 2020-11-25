// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.pa_sample_format;
using static TerraFX.Interop.pa_seek_mode;
using static TerraFX.Interop.pa_stream_flags;
using static TerraFX.Interop.Pulse;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Audio.Providers.PulseAudio
{
    /// <inheritdoc />
    public sealed class PulseAudioPlaybackDevice : IAudioPlaybackDevice
    {
        private const int Starting = 2;
        private const int Running = 3;
        private const int Completed = 4;

        private static readonly byte[] s_playbackName = Encoding.UTF8.GetBytes("Playback");

        private readonly IntPtr _context;
        private readonly Pipe _sampleDataPipe;
        private readonly Lazy<IntPtr> _stream;
        private readonly ValueLazy<GCHandle> _writeDelegateHandle;
        private readonly unsafe delegate* unmanaged<IntPtr, nuint, void*, void> _writeDelegate;
        private readonly ManualResetValueTaskSource<int> _writeRequest;

        private State _state;

        /// <inheritdoc />
        public IAudioAdapter Adapter { get; }

        /// <inheritdoc />
        public PipeWriter Writer => _sampleDataPipe.Writer;

        private GCHandle WriteDelegateHandle => _writeDelegateHandle.Value;

        private IntPtr Stream => _stream.Value;

        /// <summary>Initializes a new instance of the <see cref="PulseAudioPlaybackDevice" /> class.</summary>
        internal unsafe PulseAudioPlaybackDevice(IAudioAdapter adapter, IntPtr context)
        {
            Assert(context != IntPtr.Zero, "pa_context passed was IntPtr.Zero");

            if (adapter.DeviceType != AudioDeviceType.Playback)
            {
                ThrowInvalidOperationException(adapter.DeviceType, nameof(adapter));
            }

            _context = context;
            _sampleDataPipe = new Pipe();
            _stream = new Lazy<IntPtr>(CreateStream, isThreadSafe: true);
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

        private unsafe IntPtr CreateStream()
        {
            pa_sample_spec spec;

            spec.channels = (byte)Adapter.Channels;
            spec.format = GetSampleFormat(Adapter);
            spec.rate = (uint)Adapter.SampleRate;

            IntPtr stream;
            fixed(byte* streamName = s_playbackName)
            {
                stream = pa_stream_new(_context, (sbyte*)streamName, &spec, null);
            }

            Assert(stream != IntPtr.Zero, "pa_stream_new failed");

            var userdata = (void*)GCHandle.ToIntPtr(WriteDelegateHandle);
            pa_stream_set_write_callback(stream, _writeDelegate, userdata);

            return stream;

            static pa_sample_format GetSampleFormat(IAudioAdapter adapter)
            {
                // TODO: make this return some appropriate value
                return PA_SAMPLE_S16LE;
            }
        }

        // Called from the event loop thread when it wants audio data.
        // This happens infrequently, so in theory using a valuetask-based event should be fine
        private short _lastToken = -1; // cache the previous token used so we can detect cases where we're called twice

        [UnmanagedCallersOnly]
        private static unsafe void WriteCallback(IntPtr stream, nuint length, void* userdata)
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
                ThrowInvalidOperationException(Resources.DeviceNotCompletedMessage);
            }

            Assert(pa_stream_disconnect(Stream) == 0, "pa_stream_disconnect returned != 0");
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
                if (pa_stream_connect_playback(Stream, null, null, PA_STREAM_NOFLAGS, null, IntPtr.Zero) != 0)
                {
                    ThrowInvalidOperationException(Resources.CouldNotConnectPlaybackStreamMessage);
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
                    Assert(status, "Failed to prepare and write block");
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
                Assert(pa_stream_begin_write(Stream, &writeLocation, &bytesAvailable) == 0, "pa_stream_begin_write returned != 0");
                Assert(writeLocation is not null, "writeLocation is null");

                var destination = new Span<byte>(writeLocation, (int)bytesAvailable);
                data.Slice(0, destination.Length).CopyTo(destination);
                block = destination;

                return true;
            }

            unsafe bool TryWriteBlock(Span<byte> block)
            {
                fixed(byte* location = block)
                {
                    Assert(pa_stream_write(Stream, location, (nuint)block.Length, null, 0, PA_SEEK_RELATIVE) == 0, "pa_stream_write returned != 0");
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

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                if (_stream.IsValueCreated)
                {
                    pa_stream_unref(_stream.Value);
                }

                if (_writeDelegateHandle.IsCreated)
                {
                    _writeDelegateHandle.Value.Free();
                }
            }

            _state.EndDispose();
        }
    }
}
