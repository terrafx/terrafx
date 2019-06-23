// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO.Pipelines;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    /// <summary>A pipe used to decode audio from one format to another.</summary>
    public abstract class AudioDecoder : IDisposable
    {
        /// <summary> The input pipe for encoded data.</summary>
        private readonly Pipe _inputPipe;

        /// <summary>The output pipe for decoded data.</summary>
        private readonly Pipe _outputPipe;

        /// <summary>Initializes a new instance of the <see cref="AudioDecoder"/> class.</summary>
        /// <param name="options">Audio decoder options.</param>
        /// <param name="pipeOptions">Options for input/output pipes. Defaults to <see cref="PipeOptions.Default" />.</param>
        public AudioDecoder(AudioDecoderOptions options,
            PipeOptions? pipeOptions = null)
        {
            _inputPipe = new Pipe(pipeOptions ?? PipeOptions.Default);
            _outputPipe = new Pipe(pipeOptions ?? PipeOptions.Default);
        }

        /// <summary>Reader for encoded input data.</summary>
        protected PipeReader InputReader => _inputPipe.Reader;

        /// <summary>Writer for decoded output data.</summary>
        protected PipeWriter OutputWriter => _outputPipe.Writer;


        /// <summary>Reader for decoded output data.</summary>
        public PipeReader Reader => _outputPipe.Reader;

        /// <summary>Writer for encoded input data.</summary>
        public PipeWriter Writer => _inputPipe.Writer;

        /// <summary>Runs the decoder pipeline.</summary>
        public abstract Task DecodeAsync();

        /// <summary>Resets the decoder pipeline.</summary>
        public virtual void Reset()
        {
            _inputPipe.Writer.Complete();

            _inputPipe.Reset();
            _outputPipe.Reset();
        }

        /// <inheritdoc />
        public abstract void Dispose();
    }
}
