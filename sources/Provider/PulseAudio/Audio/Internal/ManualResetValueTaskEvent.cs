using System;
using System.Threading.Tasks.Sources;

namespace TerraFX.Provider.PulseAudio.Audio
{
    internal class ManualResetValueTaskEvent<T> : IValueTaskSource<T>
    {
        private ManualResetValueTaskSourceCore<T> _source;

        public bool RunContinuationsAsynchronously
        {
            get => _source.RunContinuationsAsynchronously;
            set => _source.RunContinuationsAsynchronously = value;
        }

        public short Version => _source.Version;

        public void Reset() => _source.Reset();
        public void SetResult(T result) => _source.SetResult(result);
        public void SetException(Exception e) => _source.SetException(e);

        public T GetResult(short token) => _source.GetResult(token);
        public ValueTaskSourceStatus GetStatus(short token) => _source.GetStatus(token);
        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags) => _source.OnCompleted(continuation, state, token, flags);
    }
}
