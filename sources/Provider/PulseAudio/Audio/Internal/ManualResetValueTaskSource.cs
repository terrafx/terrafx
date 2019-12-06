using System;
using System.Threading.Tasks.Sources;

namespace TerraFX.Provider.PulseAudio.Audio
{
    internal sealed class ManualResetValueTaskSource<TResult>
        : IValueTaskSource<TResult>
    {
        private ManualResetValueTaskSourceCore<TResult> _core;

        public bool RunContinuationsAsynchronously
        {
            get
            {
                return _core.RunContinuationsAsynchronously;
            }

            set
            {
                _core.RunContinuationsAsynchronously = value;
            }
        }

        public short Token => _core.Version;

        public void Set(TResult result) => _core.SetResult(result);

        public void Reset() => _core.Reset();

        public TResult GetResult(short token) => _core.GetResult(token);

        public ValueTaskSourceStatus GetStatus(short token) => _core.GetStatus(token);

        public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) => _core.OnCompleted(continuation, state, token, flags);
    }
}
