// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Audio
{
    /// <summary>Options for a <see cref="AudioEncoder"/>.</summary>
    public class AudioEncoderOptions
    {
        /// <summary>The sample rate of input data.</summary>
        public int SampleRate { get; set; }

        /// <summary>The requested bitrate of output data.</summary>
        public int BitRate { get; set; }

        /// <summary>The number of input channels.</summary>
        public int Channels { get; set; }
    }
}
