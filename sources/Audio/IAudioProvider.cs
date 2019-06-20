using System.Collections.Generic;
using System.Threading.Tasks;

namespace TerraFX.Audio
{
    /// <summary>Provides access to an audio subsystem.</summary>
    public interface IAudioProvider
    {
        /// <summary>Starts any asynchronous processing necessary to use this device.</summary>
        ValueTask StartAsync();

        /// <summary>Stops any asynchronous processing necessary for this device to function.</summary>
        ValueTask StopAsync();

        /// <summary>Requests an available audio device from the underlying subsystem.</summary>
        /// <param name="options">The settings for the device to operate at.</param>
        /// <returns>Returns a Task which, when completed, returns the audio device requested.</returns>
        ValueTask<IAudioDevice> RequestAudioDeviceAsync(IAudioDeviceOptions? options = null);

        /// <summary>Enumerates the available audio devices reported by the underlying subsystem.</summary>
        IAudioDeviceEnumerable EnumerateAudioDevices();
    }
}
