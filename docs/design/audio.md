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

    public AudioEncoder(AudioEncoderOptions options, PipeOptions pipeOptions = null) { throw null; }

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

    public AudioDecoder(AudioDecoderOptions options, PipeOptions pipeOptions = null) { throw null; }

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


public abstract class AudioProvider : IDisposable
{
    // Provides PCM audio samples of system endianness to a sound backend, so that they can be
    // heard by users

    public AudioProvider(AudioProviderOptions options, PipeOptions pipeOptions = null) { throw null; }

    // Since this is write-only, we only need the writer
    public PipeWriter Writer => throw null;

    public Task RunAsync() { throw null; }

    protected void Dispose(bool disposing) { throw null; }
    void IDisposable.Dispose() { Dispose(true); }
}

public abstract class AudioProviderOptions
{
    // Sample rate of input PCM
    public int SampleRate { get; set; }

    // Bit depth of input PCM
    public int BitDepth { get; set; }

    // Number of channels in input PCM
    public int Channels { get; set; }
}
```

# Further Considerations #

- Is an enum better for `AudioProviderOptions#SampleRate`? I'm not sure whether
  APIs provided by Windows/SDL/OpenAL/etc. allow arbitrary sample rates.
  - On one hand, using an enum would make it easier for the user to decide
    and choose a valid option
  - On the other, it may limit our options in the future
- How do we handle cases where obtained audio devices do not match requested
  devices?
  - If byte order does not match, would it be expensive to perform byte
    swapping?
    - Might be a possible optimisation point using vectorized operations, if
      necessary.
  - If sample rate does not match requested, do we:
    - Resample internally?
    - Throw?
    - Ignore?
  - Do we provide a static function to request an audio device? This would
    likely only be present on implementations
    - `TryGetAudioDevice`? Returning as an out-variable seems odd though...
- How do we handle buffer under/overruns?
  - Underruns can be filled with silence samples, most likely
  - Overruns should be impossible due to Pipelines?
- How do we provide audio samples to devices?
  - SDL has `SDL_QueueAudio` for pushing data into their internal buffer, as
    well as `SDL_AudioCallback` for pull/request-based data using our own
    buffer.
- Is channel remapping necessary, or do we just assume channels are already in
  the correct order?
