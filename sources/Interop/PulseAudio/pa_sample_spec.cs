using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public unsafe struct pa_sample_spec
    {
        [NativeTypeName("pa_sample_format_t")]
        public int format;
        public uint rate;
        public byte channels;
    }
}
