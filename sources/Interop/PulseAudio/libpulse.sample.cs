using System;

namespace TerraFX.Interop
{
    public static unsafe partial class libpulse
    {
        public const int PA_CHANNELS_MAX = 32;
        public const int PA_RATE_MAX = 48000 * 8;

        public const int PA_SAMPLE_U8 = 0;
        public const int PA_SAMPLE_S16LE = 1;
        public const int PA_SAMPLE_S16BE = 2;
        public const int PA_SAMPLE_FLOAT32LE = 3;
        public const int PA_SAMPLE_FLOAT32BE = 4;
        public const int PA_SAMPLE_ALAW = 5;
        public const int PA_SAMPLE_ULAW = 6;
        public const int PA_SAMPLE_S32LE = 7;
        public const int PA_SAMPLE_S32BE = 8;
        public const int PA_SAMPLE_S24LE = 9;
        public const int PA_SAMPLE_S24BE = 10;
        public const int PA_SAMPLE_S24_32LE = 11;
        public const int PA_SAMPLE_S24_32BE = 12;
        public const int PA_SAMPLE_MAX = 13;
        public const int PA_SAMPLE_INVALID = -1;

        public static readonly int PA_SAMPLE_S16NE = BitConverter.IsLittleEndian ? PA_SAMPLE_S16LE : PA_SAMPLE_S16BE;
        public static readonly int PA_SAMPLE_FLOAT32NE = BitConverter.IsLittleEndian ? PA_SAMPLE_FLOAT32LE : PA_SAMPLE_FLOAT32BE;
        public static readonly int PA_SAMPLE_S32NE = BitConverter.IsLittleEndian ? PA_SAMPLE_S32LE : PA_SAMPLE_S32BE;
        public static readonly int PA_SAMPLE_S24NE = BitConverter.IsLittleEndian ? PA_SAMPLE_S24LE : PA_SAMPLE_S24BE;
        public static readonly int PA_SAMPLE_S24_32NE = BitConverter.IsLittleEndian ? PA_SAMPLE_S24_32LE : PA_SAMPLE_S24_32BE;

        public static readonly int PA_SAMPLE_S16RE = !BitConverter.IsLittleEndian ? PA_SAMPLE_S16LE : PA_SAMPLE_S16BE;
        public static readonly int PA_SAMPLE_FLOAT32RE = !BitConverter.IsLittleEndian ? PA_SAMPLE_FLOAT32LE : PA_SAMPLE_FLOAT32BE;
        public static readonly int PA_SAMPLE_S32RE = !BitConverter.IsLittleEndian ? PA_SAMPLE_S32LE : PA_SAMPLE_S32BE;
        public static readonly int PA_SAMPLE_S24RE = !BitConverter.IsLittleEndian ? PA_SAMPLE_S24LE : PA_SAMPLE_S24BE;
        public static readonly int PA_SAMPLE_S24_32RE = !BitConverter.IsLittleEndian ? PA_SAMPLE_S24_32LE : PA_SAMPLE_S24_32BE;
    }
}
