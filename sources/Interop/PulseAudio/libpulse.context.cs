using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    public static unsafe partial class libpulse
    {
        // TODO: candidate for function pointers
        public delegate void pa_context_notify_cb(
            [NativeTypeName("pa_context")] IntPtr c,
            IntPtr userdata
        );

        // TODO: candidate for function pointers
        public delegate void pa_context_success_cb(
            [NativeTypeName("pa_context")] IntPtr c,
            int success,
            IntPtr userdata
        );

        // TODO: candidate for function pointers
        public delegate void pa_context_event_cb(
            [NativeTypeName("pa_context")] IntPtr c,
            [NativeTypeName("const char")] byte* name,
            [NativeTypeName("pa_proplist")] IntPtr p,
            IntPtr userdata
        );

        public const int PA_CONTEXT_UNCONNECTED = 0;
        public const int PA_CONTEXT_CONNECTING = 1;
        public const int PA_CONTEXT_AUTHORIZING = 2;
        public const int PA_CONTEXT_SETTING_NAME = 3;
        public const int PA_CONTEXT_READY = 4;
        public const int PA_CONTEXT_FAILED = 5;
        public const int PA_CONTEXT_TERMINATED = 6;

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_new", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_context")]
        public static extern IntPtr pa_context_new(
            [In, NativeTypeName("pa_mainloop_api")] IntPtr mainloop,
            [In, NativeTypeName("const char")] byte* name
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_new_with_proplist", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_context")]
        public static extern IntPtr pa_context_new_with_proplist(
            [In, NativeTypeName("pa_mainloop_api")] IntPtr mainloop,
            [In, NativeTypeName("const char")] byte* name,
            [In, NativeTypeName("pa_proplist")] IntPtr proplist
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_unref", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_context_unref(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_ref", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_context")]
        public static extern IntPtr pa_context_ref(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_set_state_callback", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_context_set_state_callback(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_notify_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_set_event_callback", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_context_set_event_callback(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_context_event_cb_t")] pa_context_event_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_errno", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_context_errno(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_is_pending", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_context_is_pending(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_state", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_context_state_t")]
        public static extern int pa_context_get_state(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_connect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_context_connect(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("const char")] byte* server,
            [In, NativeTypeName("pa_context_flags_t")] int flags,
            [In, NativeTypeName("const pa_spawn_api")] IntPtr api
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_disconnect", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_context_disconnect(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_drain", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_drain(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_notify_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_exit_daemon", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_exit_daemon(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_success_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_set_default_sink", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_set_default_sink(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("const char")] byte* name,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_success_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_set_default_source", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_default_source(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("const char")] byte* name,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_success_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_is_local", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_context_is_local(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_set_name", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_set_name(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("const char")] byte* name,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_success_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_server", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("const char")]
        public static extern byte* pa_context_get_server(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_protocol_version", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern uint pa_context_get_protocol_version(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_server_protocol_version", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern uint pa_context_get_server_protocol_version(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_proplist_update", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_proplist_update(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_update_mode_t")] int mode,
            [In, NativeTypeName("pa_proplist")] IntPtr p,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_success_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_proplist_remove", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_operation")]
        public static extern IntPtr pa_context_proplist_remove(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("const char* const[]")] byte* keys,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_success_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_get_index", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern uint pa_context_get_index(
            [In, NativeTypeName("pa_context")] IntPtr c
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_rttime_new", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: NativeTypeName("pa_time_event")]
        public static extern IntPtr pa_context_rttime_new(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_usec_t")] ulong usec,
            [In, NativeTypeName("pa_context_notify_cb_t")] pa_context_event_cb cb,
            [In] IntPtr userdata
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_rttime_restart", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void pa_context_rttime_restart(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("pa_time_event")] IntPtr e,
            [In, NativeTypeName("pa_usec_t")] ulong usec
        );

        [DllImport(DllName, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "pa_context_load_cookie_from_file", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int pa_context_load_cookie_from_file(
            [In, NativeTypeName("pa_context")] IntPtr c,
            [In, NativeTypeName("const char")] byte* cookie_file_path
        );
    }
}
