using System.Collections.Generic;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    public interface IAudioProvider
    {
        ValueTask StartAsync();
        ValueTask StopAsync();

        ValueTask<IAudioDevice> RequestAudioDeviceAsync(IAudioDeviceOptions? options = null);
        IEnumerable<IAudioDeviceOptions> EnumerateAudioDevices();
    }
}
