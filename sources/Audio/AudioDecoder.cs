using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    public abstract class AudioDecoder : IDisposable
    {
        private readonly Pipe _pipe;

        public AudioDecoder(AudioDecoderOptions options,
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
