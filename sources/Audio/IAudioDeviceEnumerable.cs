using System.Collections.Generic;

namespace TerraFX.Audio
{
    /// <summary>Represents an enumerable collection of audio devices.</summary>
    public interface IAudioDeviceEnumerable
        : IEnumerable<IAudioDeviceOptions>, IAsyncEnumerable<IAudioDeviceOptions>
    { }
}
