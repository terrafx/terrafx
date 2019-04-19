using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    public abstract class AudioEncoder : IDisposable
    {
        private readonly Pipe _pipe;

        public AudioEncoder(AudioEncoderOptions options,
            PipeOptions pipeOptions = null)
        {
            _pipe = new Pipe(pipeOptions ?? PipeOptions.Default);
        }

        public PipeReader Reader => _pipe.Reader;
        public PipeWriter Writer => _pipe.Writer;

        public abstract Task RunAsync();

        protected abstract void Dispose(bool disposing);
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
    }
}
