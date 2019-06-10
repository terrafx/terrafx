using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public unsafe struct pa_sink_info
    {
        [NativeTypeName("const char")]
        public byte* name;
        public uint index;
        [NativeTypeName("const char")]
        public byte* description;
        public pa_sample_spec sample_spec;
        public pa_channel_map channel_map;
        public uint owner_module;
        public pa_cvolume volume;
        public int mute;
        public uint monitor_source;
        [NativeTypeName("const char")]
        public byte* monitor_source_name;
        [NativeTypeName("pa_usec_t")]
        public ulong latency;
        [NativeTypeName("const char")]
        public byte* driver;
        [NativeTypeName("pa_sink_flags_t")]
        public int flags;
        [NativeTypeName("pa_proplist")]
        public IntPtr propslist;
        [NativeTypeName("pa_usec_t")]
        public ulong configured_latency;
        [NativeTypeName("pa_volume_t")]
        public uint base_volume;
        [NativeTypeName("pa_sink_state_t")]
        public int state;
        public uint n_volume_steps;
        public uint card;
        public uint n_ports;
        public pa_sink_port_info** ports;
        public pa_sink_port_info* active_port;
        public byte n_formats;
        public pa_format_info** formats;
    }
}
