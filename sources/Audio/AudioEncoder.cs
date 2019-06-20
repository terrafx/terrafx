using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    /// <summary>A pipe used to encode audio from one format to another.</summary>
    public abstract class AudioEncoder : IDisposable
    {
        /// <summary> The input pipe for decoded data.</summary>
        protected readonly Pipe _inputPipe;

        /// <summary>The output pipe for encoded data.</summary>
        protected readonly Pipe _outputPipe;


        /// <summary>Initializes a new instance of the <see cref="AudioEncoder"/> class.</summary>
        /// <param name="options">Audio encoder options.</param>
        /// <param name="pipeOptions">Options for input/output pipes. Defaults to <see cref="PipeOptions.Default" />.</param>
        public AudioEncoder(AudioEncoderOptions options,
            PipeOptions? pipeOptions = null)
        {
            _inputPipe = new Pipe(pipeOptions ?? PipeOptions.Default);
            _outputPipe = new Pipe(pipeOptions ?? PipeOptions.Default);
        }

        /// <summary>Reader for encoded output data.</summary>
        public PipeReader Reader => _outputPipe.Reader;
        /// <summary>Writer for decoded input data.</summary>
        public PipeWriter Writer => _inputPipe.Writer;

        /// <summary>Runs the decoder pipeline.</summary>
        public abstract Task RunAsync();

        /// <summary>Used to dispose of managed and unmanaged state.</summary>
        /// <param name="disposing"><code>true</code> if Dispose was called.</param>
        protected abstract void Dispose(bool disposing);

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
