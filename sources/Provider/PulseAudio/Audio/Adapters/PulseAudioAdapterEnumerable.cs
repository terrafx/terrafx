using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using TerraFX.Audio;
using TerraFX.Interop;

namespace TerraFX.Provider.PulseAudio.Audio
{
    /// <inheritdoc />
    public class PulseAudioAdapterEnumerable : IAudioAdapterEnumerable
    {
        // Only call the internal methods from a single thread (usually the pulse event loop thread)
        // _completeSignal may be overwritten improperly causing a deadlock if multiple threads try
        // to call internal methods concurrently

        private readonly LinkedList<IAudioAdapter> _backingCollection;

        private TaskCompletionSource<bool> _completeSignal;

        private readonly pa_source_info_cb_t _sourceCallback;
        private readonly pa_sink_info_cb_t _sinkCallback;

        internal IntPtr SourceCallback { get; }
        internal IntPtr SinkCallback { get; }

        internal PulseAudioAdapterEnumerable(pa_source_info_cb_t sourceCallback, pa_sink_info_cb_t sinkCallback)
        {
            _backingCollection = new LinkedList<IAudioAdapter>();
            // Run continuations asynchronously so that we do not block the event loop thread and potentially deadlock
            _completeSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _sourceCallback = sourceCallback;
            _sinkCallback = sinkCallback;

            SourceCallback = Marshal.GetFunctionPointerForDelegate(_sourceCallback);
            SinkCallback = Marshal.GetFunctionPointerForDelegate(_sinkCallback);
        }

        private void SetCompleteSignal(bool value)
        {
            // No need to use Interlocked.CompareExchange or anything like this
            // since we are only called from one thread
            _completeSignal.TrySetResult(value);
            _completeSignal = new TaskCompletionSource<bool>();
        }

        internal unsafe void Add(pa_source_info* i)
        {
            var adapter = new PulseSourceAdapter(i);

            _backingCollection.AddLast(adapter);
            SetCompleteSignal(false);
        }

        internal unsafe void Add(pa_sink_info* i)
        {
            var adapter = new PulseSinkAdapter(i);

            _backingCollection.AddLast(adapter);
            SetCompleteSignal(false);
        }

        internal void Complete()
        {
            // Should always be successful due to being called only from one thread
            // Do not overwrite _completeSignal past this point
            _completeSignal.TrySetResult(true);
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IAudioAdapter> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var current = _backingCollection.First;
            var done = false;

            while (current?.Next != null || !done)
            {
                if (current == null)
                {
                    done = await _completeSignal.Task;
                    current = _backingCollection.First!;
                }

                yield return current.Value;

                if (current.Next == null)
                {
                    done = await _completeSignal.Task;
                }

                current = current.Next;
            }
        }

        /// <inheritdoc />
        public IEnumerator<IAudioAdapter> GetEnumerator()
        {
            var current = _backingCollection.First;
            var done = false;

            while (current?.Next != null || !done)
            {
                if (current == null)
                {
                    var tsk = _completeSignal.Task;
                    tsk.Wait();
                    done = tsk.Result;
                    current = _backingCollection.First!;
                }

                yield return current.Value;

                if (current.Next == null)
                {
                    var tsk = _completeSignal.Task;
                    tsk.Wait();
                    done = tsk.Result;
                }

                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }
    }
}
