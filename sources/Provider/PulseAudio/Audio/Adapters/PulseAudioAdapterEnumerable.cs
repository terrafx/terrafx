using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Audio;
using TerraFX.Interop;

namespace TerraFX.Provider.PulseAudio.Audio
{
    /// <inheritdoc />
    public class PulseAudioAdapterEnumerable : IAudioAdapterEnumerable, IDisposable
    {
        private readonly LinkedList<IAudioAdapter> _backingCollection;

        private readonly pa_source_info_cb_t _sourceCallback;
        private readonly pa_sink_info_cb_t _sinkCallback;

        internal IntPtr SourceCallback { get; }
        internal IntPtr SinkCallback { get; }

        internal PulseAudioAdapterEnumerable(pa_source_info_cb_t sourceCallback, pa_sink_info_cb_t sinkCallback)
        {
            _backingCollection = new LinkedList<IAudioAdapter>();
            _sourceCallback = sourceCallback;
            _sinkCallback = sinkCallback;

            SourceCallback = Marshal.GetFunctionPointerForDelegate(_sourceCallback);
            SinkCallback = Marshal.GetFunctionPointerForDelegate(_sinkCallback);
        }

        internal unsafe void Add(pa_source_info* i)
        {
            var adapter = new PulseSourceAdapter(i);
        }

        internal unsafe void Add(pa_sink_info* i)
        {
            var adapter = new PulseSinkAdapter(i);
        }

        internal void Complete()
        {
        }

        /// <inheritdoc />
        public async IAsyncEnumerator<IAudioAdapter> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            var current = _backingCollection.First;
        }

        /// <inheritdoc />
        public IEnumerator<IAudioAdapter> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _completeSource.Dispose();
        }
    }
}
