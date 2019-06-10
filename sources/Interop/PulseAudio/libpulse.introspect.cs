using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public static unsafe partial class libpulse
    {
        public delegate void pa_sink_info_cb(
            [NativeTypeName("pa_context")]IntPtr c,
            [NativeTypeName("const pa_sink_info")]pa_sink_info* i,
            int eol,
            IntPtr userdata
        );
        public delegate void pa_source_info_cb(
            [NativeTypeName("pa_context")]IntPtr c,
            [NativeTypeName("const pa_source_info")]pa_source_info* i,
            int eol,
            IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_sink_info_list", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_get_sink_info_list(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_sink_info_cb_t")] pa_sink_info_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_source_info_list", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_get_source_info_list(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_source_info_cb_t")] pa_source_info_cb cb,
            [In] IntPtr userdata
        );
    }
}
