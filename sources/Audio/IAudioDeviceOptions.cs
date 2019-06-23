// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Audio
{
    /// <summary>Options/information about a given audio device.</summary>
    public interface IAudioDeviceOptions
    {
        /// <summary>The type of device this object represents.</summary>
        AudioDeviceType DeviceType { get; set; }

        /// <summary>The name of the device.</summary>
        string Name { get; set; }

        /// <summary>The sample rate the device operates at.</summary>
        int SampleRate { get; set; }

        /// <summary>The number of bits in each sample this device operates at.</summary>
        int BitDepth { get; set; }

        /// <summary>The number of channels this device operates at.</summary>
        int Channels { get; set; }

        /// <summary>The endianness of the device.</summary>
        /// <remarks>
        /// If the device operates in big endian mode (MSB first), this will be <code>true</code>.
        /// If it operates in little endian mode (LSB first), this will be <code>false</code>.
        /// </remarks>
        bool BigEndian { get; set; }
    }
}
