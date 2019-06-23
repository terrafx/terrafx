// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;

namespace TerraFX.Audio
{
    /// <summary>Represents an enumerable collection of audio devices.</summary>
    public interface IAudioDeviceEnumerable
        : IEnumerable<IAudioDeviceOptions>, IAsyncEnumerable<IAudioDeviceOptions>
    { }
}
