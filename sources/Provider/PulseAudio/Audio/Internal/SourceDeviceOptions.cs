using System;
using System.Runtime.InteropServices;
using TerraFX.Audio;
using TerraFX.Interop;

namespace TerraFX.Provider.PulseAudio.Audio
{
    public class SourceDeviceOptions : IAudioDeviceOptions
    {
        internal unsafe SourceDeviceOptions(pa_source_info* device)
        {
            DeviceType = AudioDeviceType.Recording;

            Name = Marshal.PtrToStringUTF8((IntPtr)device->name);
            Description = Marshal.PtrToStringUTF8((IntPtr)device->description);

            Channels = device->sample_spec.channels;
            SampleRate = unchecked((int)device->sample_spec.rate);

            var format = device->sample_spec.format;
            switch (format)
            {
                case libpulse.PA_SAMPLE_U8:
                case libpulse.PA_SAMPLE_ALAW:
                case libpulse.PA_SAMPLE_ULAW:
                    BigEndian = false;
                    FloatingPoint = false;
                    BitDepth = 8;
                    PackedBitDepth = 8;
                    break;
                case libpulse.PA_SAMPLE_S16LE:
                case libpulse.PA_SAMPLE_S16BE:
                    BigEndian = format == libpulse.PA_SAMPLE_S16BE;
                    FloatingPoint = false;
                    BitDepth = 16;
                    PackedBitDepth = 16;
                    break;
                case libpulse.PA_SAMPLE_FLOAT32LE:
                case libpulse.PA_SAMPLE_FLOAT32BE:
                    BigEndian = format == libpulse.PA_SAMPLE_FLOAT32BE;
                    FloatingPoint = true;
                    BitDepth = 32;
                    PackedBitDepth = 32;
                    break;
                case libpulse.PA_SAMPLE_S32LE:
                case libpulse.PA_SAMPLE_S32BE:
                    BigEndian = format == libpulse.PA_SAMPLE_S32BE;
                    FloatingPoint = false;
                    BitDepth = 32;
                    PackedBitDepth = 32;
                    break;
                case libpulse.PA_SAMPLE_S24LE:
                case libpulse.PA_SAMPLE_S24BE:
                    BigEndian = format == libpulse.PA_SAMPLE_S24BE;
                    FloatingPoint = false;
                    BitDepth = 24;
                    PackedBitDepth = 24;
                    break;
                case libpulse.PA_SAMPLE_S24_32LE:
                case libpulse.PA_SAMPLE_S24_32BE:
                    BigEndian = format == libpulse.PA_SAMPLE_S24_32BE;
                    FloatingPoint = false;
                    BitDepth = 32;
                    PackedBitDepth = 24;
                    break;
            }
        }

        public AudioDeviceType DeviceType { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int SampleRate { get; set; }
        public int BitDepth { get; set; }
        public int PackedBitDepth { get; set; }
        public int Channels { get; set; }
        public bool FloatingPoint { get; set; }
        public bool BigEndian { get; set; }
    }
}
