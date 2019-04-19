using System;
using System.Collections.Generic;

namespace TerraFX.Audio
{
    public interface IAudioProvider
    {
        IAudioDevice RequestAudioDevice(IAudioDeviceOptions options = null);

        IEnumerable<IAudioDeviceOptions> EnumerateAudioDevices();
    }
}
