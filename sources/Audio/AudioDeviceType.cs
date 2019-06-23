// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Audio
{
    /// <summary>Represents a type of <see cref="IAudioAdapter"/></summary>
    public enum AudioDeviceType
    {
        /// <summary>Indicates that this is a device used for recording.</summary>
        Recording,

        /// <summary>Indicates that this is a device used for playback.</summary>
        Playback
    }
}
