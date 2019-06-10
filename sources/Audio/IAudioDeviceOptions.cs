using System;

namespace TerraFX.Audio
{
    public interface IAudioDeviceOptions
    {
        AudioDeviceType DeviceType { get; set; }

        string Name { get; set; }
        int SampleRate { get; set; }
        int BitDepth { get; set; }
        int Channels { get; set; }
        bool BigEndian { get; set; }
    }
}
