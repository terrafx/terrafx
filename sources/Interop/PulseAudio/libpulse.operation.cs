using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public static unsafe partial class libpulse
    {
        public delegate void pa_operation_notify_cb(
            [NativeTypeName("pa_operation")]IntPtr o,
            IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_operation_ref", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_operation_ref(
            [In, NativeTypeName("pa_operation")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_operation_unref", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_operation_unref(
            [In, NativeTypeName("pa_operation")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_operation_cancel", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_operation_cancel(
            [In, NativeTypeName("pa_operation")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_operation_get_state", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation_state_t")]
        public static extern int pa_operation_get_state(
            [In, NativeTypeName("pa_operation")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_operation_set_state_callback", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_operation_set_state_callback(
            [In, NativeTypeName("pa_operation")] IntPtr c,
            [In, NativeTypeName("pa_operation_notify_cb_t")] pa_operation_notify_cb cb,
            [In] IntPtr userdata
        );
    }
}
