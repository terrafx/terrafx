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
    // Encodes raw audio into a format specified by subclassers.
    // Endianness and bit depth are specified by subclassers, eg. FloatOpusEncoder, ShortOpusEncoder
    // System endianness is the preferred format, however.

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
    // Decodes audio into another format, specified by subclassers
    // Endianness and bit depth is decided by subclasser too, e.g. FloatOpusDecoder, ShortOpusDecoder
    // System endianness is preferred, however.

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


public abstract class AudioService
{
    // Provides sound devices to code, so that they can be requested and used to
    // play sound. Implementors are likely to name based on subsystem, e.g. XAudio2AudioService, OpenALAudioService

    // Requests an audio device supporting the given options. If none could be
    // found, returns null.
    public abstract IAudioDevice RequestAudioDevice(IAudioDeviceOptions options = null);

    // Returns an enumerable which can be used to enumerate available audio
    // devices. Does not initialize them.
    public abstract IEnumerable<IAudioDeviceOptions> EnumerateAudioDevices();
}


public interface IAudioDevice : IDisposable
{
    // Provides audio data to a sound device, so that they can be heard by
    // users. Endianness and data format are specified by implementors.

    // Since this is write-only, we only need the writer
    public PipeWriter Writer;

    public Task RunAsync();
}

public interface IAudioDeviceOptions
{
    // Represents a set of options supported by a given audio device.
    // Implementors can add more options, and they are likely to encode format
    // information in the name, eg. OpusAudioDeviceOptions, MpegAudioDeviceOptions

    // Sample rate of input data
    public int SampleRate { get; set; }

    // Bit depth of input data
    public int BitDepth { get; set; }

    // Number of channels in input data
    public int Channels { get; set; }

    // Endianness of input data: true if big endian
    public bool BigEndian { get; set; }
}


public static sealed class WellKnownSampleRates
{
    // A list of of well known sampling rates, in samples per second.

    public const int CdAudio = 44100;

    public const int DvdAudio = 48000;
}
```

# Further Considerations #

- How do we name well-known sampling rates?
  - `CdAudio` and `DvdAudio` are based on well-known sample rates for CDs and
    DVDs, but how do we handle cases, e.g. 192kHz?
- How do we handle cases where an audio service does not match a requested
  device?
  - If byte order does not match, byte swapping can likely be performed
    transparently using vectorised operations
  - If sample rate does not match requested, do we resample internally?
- How do we handle buffer under/overruns?
  - Underruns can be filled with silence samples, most likely
  - Overruns should be impossible due to Pipelines?
- How do we provide audio samples to devices?
  - SDL has `SDL_QueueAudio` for pushing data into their internal buffer, as
    well as `SDL_AudioCallback` for pull/request-based data using our own
    buffer.
  - XAudio2 also appears to have something to this effect
- Is channel remapping necessary, or do we just assume channels are already in
  the correct order?
- Do we make `IAudioDeviceOptions` have read-only properties, or introduce what
  would effectively be an identical interface/struct for requests and available
  devices?
