# API Design Requirements #

1. Reduce latency as much as possible
2. (Try to) Do the right thing

These are just initial requirements, especially for the low-level API: more
requirements will likely be added as necessary as the API surface is fleshed
out.

## Reduce latency as much as possible ##

- Use System.IO.Pipelines for buffering and efficient implementation of partial
  reads in encodings which require certain amounts of data before working
- Use ValueTask where appropriate in code which often runs synchronously but
  may await if more data is necessary, e.g. transcoding PCM to Opus expects at
  least 20ms worth of sample data at standard settings
- Avoid exceptions in low-level code except in unrecoverable situations. Higher
  level code should make guarantees about input format, while lower-level code
  should just assume the input is correct.
  - If consumers have guarantees about their input data, they can use lower
    level implementations to avoid exception costs

## Do the right thing ##

Certain audio formats have certain requirements. In case those requirements are
not met, we should try to do the right thing. Using System.IO.Pipelines will
allow us to alleviate some of these issues, such as:
- Audio frames being split across network packets (partial reads)
- Variations in network latency when reading from a socket
- Unexpected end-of-file when reading from a file or similar end-of-stream
  scenarios
- Buffering and buffer re-use using `ArrayPool<T>`/`MemoryPool<T>`

High level API should perform explicit checks to assert inputs are correct and
valid: throwing at the earliest possible moment will prevent corrupted state or
audio later on.

# Initial thoughts #

```cs
public abstract class AudioEncoder : IDisposable
{
    // Encodes raw PCM of system endianness into a format specified by subclassers
    // Bit depth is decided by subclasser too, e.g. FloatOpusEncoder, ShortOpusEncoder

    public AudioEncoder(AudioEncoderOptions options) { throw null; }

    // Expose both ends of the pipe so consumers can easily hook them into other APIs
    // e.g. providing encoded Opus frames to a network stream
    public PipeReader Reader => throw null;
    public PipeWriter Writer => throw null;

    // Performs the actual encode, reading from the Writer pipe and writing to the
    // Reader pipe
    public Task RunAsync() { throw null; }

    // IDisposable pattern used as most encoders will use native resources
    protected void Dispose(bool disposing) { throw null; }
    void IDisposable.Dispose() { Dispose(true); }
}

// Options apply to output
public abstract class AudioEncoderOptions
{
    // Samples per second, input is expected to be this sample rate
    public int SampleRate { get; set; }
    // Bits per second, -1 = variable
    public int BitRate { get; set; }
    // Number of output channels, input is expected to have the same number of channels.
    // PCM samples alternate channels, so for 3 channels the input would look like:
    // Sample1:[Channel1,Channel2,Channel3],Sample2:[Channel1,Channel2,Channel3]
    public int Channels { get; set; }
}


public abstract class AudioDecoder : IDisposable
{
    // Decodes audio into PCM samples of system endianness, input format specified subclassers
    // Bit depth is decided by subclasser too, e.g. FloatOpusDecoder, ShortOpusDecoder

    public AudioDecoder(AudioDecoderOptions options) { throw null; }

    // Below is mirrored from AudioEncoder, for the same reasons
    public PipeReader Reader => throw null;
    public PipeWriter Writer => throw null;

    public Task RunAsync() { throw null; }

    protected void Dispose(bool disposing) { throw null; }
    void IDisposable.Dispose() { Dispose(true); }
}

// Options apply to input
public abstract class AudioDecoderOptions
{
    // Sample rate of input data. Output matches this.
    public int SampleRate { get; set; }
    // Number of channels in input data. Output matches this, following the sample
    // format described above
    public int Channels { get; set; }
}


public class AudioTranscoder
{
    // Transcodes from one format to another, by decoding it into PCM and re-encoding it

    public AudioTranscoder(AudioDecoder decoder, AudioEncoder encoder) { throw null; }

    // No reader/writer exposed as they are exposed by the encoder and decoder
    // but it could be a possibility?

    public Task RunAsync() { throw null; }
}


public sealed class AudioResampler
{
    // Resamples PCM samples from one sample rate to another

    public AudioResampler(AudioResamplerOptions options) { throw null; }

    public PipeReader Reader => throw null;
    public PipeWriter Writer => throw null;

    public Task RunAsync() { throw null; }
}

public sealed class AudioResamplerOptions
{
    // How to interpolate between samples if necessary
    public InterpolationAlgorithm Interpolation { get; set; }

    // Sample rate of input and output PCM respectively.
    public int InputSampleRate { get; set; }
    public int OutputSampleRate { get; set; }
}

public enum InterpolationAlgorithm
{
    // Specifies how to interpolate between samples in the resampler

    None, // Do not interpolate. Samples may be duplicated or dropped entirely.

    Linear,
    Cubic,
    Gaussian,
    Sinc
}
```

# Further Considerations #

- Is implementing multiple forms of interpolation as an enum smart? It might be
  easier and faster to implement them in multiple types.
  - Multiple types might hurt discoverability. What's the difference between
    `BlepSynthesisAudioResampler` and `BlamSynthesisAudioResampler`? (using
    terms from the Foobar2000 foo_dsp_multiresampler component)
- Is mixing important enough to implement?
  - Clipping is a hard problem to solve, and has multiple solutions:
    - We could allow samples to lie outside the valid range of samples
      - This would require working in a more expensive format, e.g. float
      - It would also require a further final volume control at the end
    - We could drop input volume automatically
      - Multiple algorithms exist to do this, with varying results
    - We could just naively clip the samples
      - Audio will sound awful at louder volumes, so it would be on the
        consumer to ensure inputs do not clip
