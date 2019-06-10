using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public unsafe struct pa_format_info
    {
        [NativeTypeName("pa_encoding_t")]
        public int encoding;

        public IntPtr plist;
    }
}
