// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Audio
{
    /// <summary>An interface representing an audio adapter.</summary>
    public interface IAudioAdapter
    {
        /// <summary>The type of device this adapter represents.</summary>
        AudioDeviceType DeviceType { get; set; }

        /// <summary>The name of the adapter.</summary>
        string Name { get; set; }

        /// <summary>The sample rate the adapter operates at.</summary>
        int SampleRate { get; set; }

        /// <summary>The number of bits in each sample this adapter operates at.</summary>
        int BitDepth { get; set; }

        /// <summary>The number of channels this adapter operates at.</summary>
        int Channels { get; set; }

        /// <summary>The endianness of the adapter.</summary>
        /// <remarks>If the adapter operates in big endian mode (MSB first), this will be <c>true</c>; otherwise, it operates in little endian mode (LSB first) and will be <c>false</c>.</remarks>
        bool IsBigEndian { get; set; }
    }
}
