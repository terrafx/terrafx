using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public static unsafe partial class libpulse
    {
        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_new", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_threaded_mainloop")]
        public static extern IntPtr pa_threaded_mainloop_new();

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_free", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_free(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_start", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_threaded_mainloop_start(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_stop", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_stop(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_lock", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_lock(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_unlock", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_unlock(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_wait", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_wait(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_signal", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_signal(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m,
            [In] int wait_for_accept
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_accept", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_accept(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_get_retval", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_threaded_mainloop_get_retval(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_get_api", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_mainloop_api")]
        public static extern IntPtr pa_threaded_mainloop_get_api(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_in_thread", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_in_thread(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_threaded_mainloop_set_name", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_threaded_mainloop_set_name(
            [In, NativeTypeName("pa_threaded_mainloop")] IntPtr m,
            [In, NativeTypeName("const char*")] byte* name
        );
    }
}
