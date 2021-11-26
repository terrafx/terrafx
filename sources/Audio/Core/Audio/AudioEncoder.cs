// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Audio;

/// <summary>A pipe used to encode audio from one format to another.</summary>
public abstract class AudioEncoder : IDisposable
{
    private readonly Pipe _inputPipe;
    private readonly Pipe _outputPipe;

    /// <summary>Initializes a new instance of the <see cref="AudioEncoder" /> class.</summary>
    /// <param name="options">Audio encoder options.</param>
    /// <param name="pipeOptions">Options for input/output pipes. Defaults to <see cref="PipeOptions.Default" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="options" /> is <c>null</c>.</exception>
    public AudioEncoder(AudioEncoderOptions options, PipeOptions? pipeOptions = null)
    {
        ThrowIfNull(options, nameof(options));

        _inputPipe = new Pipe(pipeOptions ?? PipeOptions.Default);
        _outputPipe = new Pipe(pipeOptions ?? PipeOptions.Default);
    }

    /// <summary>Reader for encoded output data.</summary>
    public PipeReader Reader => _outputPipe.Reader;

    /// <summary>Writer for decoded input data.</summary>
    public PipeWriter Writer => _inputPipe.Writer;

    /// <summary>Reader for decoded input data.</summary>
    protected PipeReader InputReader => _inputPipe.Reader;

    /// <summary>Writer for encoded output data.</summary>
    protected PipeWriter OutputWriter => _outputPipe.Writer;

    /// <inheritdoc />
    public abstract void Dispose();

    /// <summary>Runs the encoder pipeline.</summary>
    public abstract Task EncodeAsync(CancellationToken cancellationToken = default);

    /// <summary>Resets the encoder pipeline.</summary>
    public virtual void Reset()
    {
        _inputPipe.Reset();
        _outputPipe.Reset();
    }
}
