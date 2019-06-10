using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public unsafe struct pa_channel_map
    {
        public byte channels;

        [NativeTypeName("pa_channel_position_t")]
        public fixed int map[libpulse.PA_CHANNELS_MAX];
    }
}
