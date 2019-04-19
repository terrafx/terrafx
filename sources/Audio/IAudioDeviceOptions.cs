using System;

namespace TerraFX.Audio
{
    public interface IAudioDeviceOptions
    {
        int SampleRate { get; set; }
        int BitDepth { get; set; }
        int Channels { get; set; }
        bool BigEndian { get; set; }
    }
}
