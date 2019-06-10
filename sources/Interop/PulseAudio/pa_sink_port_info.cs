using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public unsafe struct pa_sink_port_info
    {
        [NativeTypeName("const char")]
        public byte* name;
        [NativeTypeName("const char")]
        public byte* description;
        public uint priority;
        public int available;
    }
}
