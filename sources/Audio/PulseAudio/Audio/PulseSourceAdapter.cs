// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Runtime;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Audio
{
    /// <inhertidoc />
    public class PulseSourceAdapter : IAudioAdapter
    {
        internal unsafe PulseSourceAdapter(pa_source_info* i)
        {
            SourceInfo = i;

            Name = Marshal.PtrToStringUTF8((IntPtr)i->name)!;
            Description = Marshal.PtrToStringUTF8((IntPtr)i->description)!;
            SampleRate = unchecked((int)i->sample_spec.rate);
            Channels = i->sample_spec.channels;

            var format = i->sample_spec.format;
            switch (format)
            {
                case pa_sample_format.PA_SAMPLE_U8:
                case pa_sample_format.PA_SAMPLE_ALAW:
                case pa_sample_format.PA_SAMPLE_ULAW:
                {
                    IsUnsigned = true;
                    BitDepth = 8;
                    PackedSize = 8;
                    IsBigEndian = false;
                    break;
                }

                case pa_sample_format.PA_SAMPLE_S16LE:
                case pa_sample_format.PA_SAMPLE_S16BE:
                {
                    IsUnsigned = false;
                    BitDepth = 16;
                    PackedSize = 16;
                    IsBigEndian = format == pa_sample_format.PA_SAMPLE_S16BE;
                    break;
                }

                case pa_sample_format.PA_SAMPLE_FLOAT32LE:
                case pa_sample_format.PA_SAMPLE_FLOAT32BE:
                {
                    IsUnsigned = false;
                    BitDepth = 32;
                    PackedSize = 32;
                    IsBigEndian = format == pa_sample_format.PA_SAMPLE_FLOAT32BE;
                    IsFloatingPoint = true;
                    break;
                }

                case pa_sample_format.PA_SAMPLE_S32LE:
                case pa_sample_format.PA_SAMPLE_S32BE:
                {
                    IsUnsigned = false;
                    BitDepth = 32;
                    PackedSize = 32;
                    IsBigEndian = format == pa_sample_format.PA_SAMPLE_S32BE;
                    IsFloatingPoint = false;
                    break;
                }

                case pa_sample_format.PA_SAMPLE_S24LE:
                case pa_sample_format.PA_SAMPLE_S24BE:
                {
                    IsUnsigned = false;
                    BitDepth = 24;
                    PackedSize = 24;
                    IsBigEndian = format == pa_sample_format.PA_SAMPLE_S32BE;
                    IsFloatingPoint = false;
                    break;
                }

                case pa_sample_format.PA_SAMPLE_S24_32LE:
                case pa_sample_format.PA_SAMPLE_S24_32BE:
                {
                    IsUnsigned = false;
                    BitDepth = 24;
                    PackedSize = 32;
                    IsBigEndian = format == pa_sample_format.PA_SAMPLE_S24_32BE;
                    IsFloatingPoint = false;
                    break;
                }

                case pa_sample_format.PA_SAMPLE_MAX:
                case pa_sample_format.PA_SAMPLE_INVALID:
                default:
                {
                    ThrowForInvalidKind(format, nameof(format));
                    break;
                }
            }
        }

        /// <summary>Gets the underlying native pointer for the PulseAudio source info.</summary>
        public unsafe pa_source_info* SourceInfo { get; }

        /// <inhertidoc />
        public AudioDeviceType DeviceType { get; set; } = AudioDeviceType.Recording;

        /// <inhertidoc />
        public string Name { get; set; }

        /// <summary>Gets a string representing the description of this device.</summary>
        public string Description { get; set; }

        /// <inhertidoc />
        public int SampleRate { get; set; }

        /// <inhertidoc />
        public int BitDepth { get; set; }

        /// <inhertidoc />
        public int Channels { get; set; }

        /// <inhertidoc />
        public bool IsBigEndian { get; set; }

        /// <summary>Gets a value representing whether the adapter sample format is unsigned or not.</summary>
        public bool IsUnsigned { get; set; }

        /// <summary>Gets a value representing whether the adapter sample format uses floating point or not.</summary>
        public bool IsFloatingPoint { get; set; }

        /// <summary>Gets a value representing the packed sample size in bits.</summary>
        public int PackedSize { get; set; }
    }
}
