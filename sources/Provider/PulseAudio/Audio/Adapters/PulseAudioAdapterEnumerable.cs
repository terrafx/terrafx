// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Audio;
using TerraFX.Interop;
using TerraFX.Utilities;

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.PulseAudio.Audio
{
    /// <inheritdoc />
    public class PulseAudioAdapterEnumerable : IAudioAdapterEnumerable
    {
        // Only call the internal methods from a single thread (usually the pulse event loop thread)
        // _completeSignal may be overwritten improperly causing a deadlock if multiple threads try
        // to call internal methods concurrently

        private readonly List<IAudioAdapter> _backingCollection;
        private readonly Thread _eventLoopThread;

        // WORKAROUND: https://github.com/dotnet/roslyn/issues/38143
        // 'static' local functions might not be emitted as static

        //private readonly NativeDelegate<pa_source_info_cb_t> _sourceCallback;
        //private readonly NativeDelegate<pa_sink_info_cb_t> _sinkCallback;
        private readonly pa_source_info_cb_t _sourceCallback;
        private readonly pa_sink_info_cb_t _sinkCallback;

        private TaskCompletionSource<bool> _completeSignal;

        internal IntPtr SourceCallback //=> _sourceCallback;
            { get; }
        internal IntPtr SinkCallback //=> _sinkCallback;
            { get; }

        internal PulseAudioAdapterEnumerable(Thread eventLoopThread, pa_source_info_cb_t sourceCallback, pa_sink_info_cb_t sinkCallback)
        {
            _backingCollection = new List<IAudioAdapter>(16);
            _eventLoopThread = eventLoopThread;
            //_sourceCallback = new NativeDelegate<pa_source_info_cb_t>(sourceCallback);
            //_sinkCallback = new NativeDelegate<pa_sink_info_cb_t>(sinkCallback);
            _sourceCallback = sourceCallback;
            _sinkCallback = sinkCallback;
            // Run continuations asynchronously so that we do not block the event loop thread and potentially deadlock
            _completeSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            SourceCallback = Marshal.GetFunctionPointerForDelegate(_sourceCallback);
            SinkCallback = Marshal.GetFunctionPointerForDelegate(_sinkCallback);
        }

        private void SetCompleteSignal(bool value)
        {
            var previous = Interlocked.Exchange(ref _completeSignal, new TaskCompletionSource<bool>());
            previous.TrySetResult(value);
        }
        internal unsafe void Add(IAudioAdapter adapter)
        {
            ThrowIfNotThread(_eventLoopThread);

            _backingCollection.Add(adapter);
            SetCompleteSignal(false);
        }

        internal void Complete()
        {
            ThrowIfNotThread(_eventLoopThread);

            // Should always be successful due to being called only from one thread
            // Do not overwrite _completeSignal past this point
            _completeSignal.TrySetResult(true);
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IAudioAdapter> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var complete = false;

            for (int position = 0; !complete; position++)
            {
                if (position == _backingCollection.Count)
                {
                    var done = await _completeSignal.Task;
                    complete = position == _backingCollection.Count && done;
                }

                if (!complete)
                {
                    yield return _backingCollection[position];
                }
            }
        }

        /// <inheritdoc />
        public IEnumerator<IAudioAdapter> GetEnumerator()
        {
            var complete = false;

            for (int position = 0; !complete; position++)
            {
                if (position == _backingCollection.Count)
                {
                    var task = _completeSignal.Task;
                    task.Wait();
                    var done = task.Result;
                    complete = position == _backingCollection.Count && done;
                }

                if (!complete)
                {
                    yield return _backingCollection[position];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
