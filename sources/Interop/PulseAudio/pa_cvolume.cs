using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public unsafe struct pa_cvolume
    {
        public byte channels;

        [NativeTypeName("pa_volume_t")]
        public fixed uint values[libpulse.PA_CHANNELS_MAX];
    }
}
