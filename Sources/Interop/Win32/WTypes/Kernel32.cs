// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\libloaderapi.h and um\profileapi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Exposes methods exported by <c>Kernel32.dll</c>.</summary>
    unsafe public static partial class Kernel32
    {
        #region Methods
        /// <summary>Retrieves a module handle for the specified module. The module must have been loaded by the calling process.</summary>
        /// <param name="lpModuleName">
        ///     <para>The name of the loaded module (either a <c>.dll</c> or <c>.exe</c> file). If the name is ommitted, the default library extension .dll is appended. The file name string can include a trailing point character (.) to indicate that the module name has no extension. The string does not have to specify a path. When specifying a path, be sure to use backslashes (\), not forward slashes (/). The name is compared (case independently) to the names of modules currently mapped into the address space of the calling process.</para>
        ///     <para>If this parameter is <see cref="LPWSTR.NULL" />, this method returns a handle to the file used to create the calling process (<c>.exe</c> file).</para>
        ///     <para>This function does not retrieve handles for modules that were loaded using the <c>LOAD_LIBRARY_AS_DATAFILE</c> flag.</para>
        /// </param>
        /// <returns>
        ///     <para>If the function succeeds, the return value is a handle to the specified module.</para>
        ///     <para>If the function fails, the return value is <see cref="HMODULE.NULL" />. To get extended error information, call <see cref="Marshal.GetLastWin32Error()" />.</para>
        /// </returns>
        [DllImport("Kernel32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "GetModuleHandleW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern HMODULE GetModuleHandle(
            [In, Optional] LPWSTR lpModuleName
        );

        /// <summary>Retrieves the current value of the performance counter, which is a high resolution (&lt;1us) time stamp that can be used for time-interval measurements.</summary>
        /// <param name="lpPerformanceCount">A pointer to a variable that receives the current performance-counter value, in counts.</param>
        /// <returns>
        ///     <para>If the function succeeds, the return value is nonzero.</para>
        ///     <para>If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error()" />. On systems that run Windows XP or later, the function will always succeed and will thus never return zero.</para>
        /// </returns>
        [DllImport("Kernel32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "QueryPerformanceCounter", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL QueryPerformanceCounter(
            [Out] long* lpPerformanceCount
        );

        /// <summary>Retrieves the frequency of the performance counter. The frequency of the performance counter is fixed at system boot and is consistent across all processors. Therefore, the frequency need only be queried upon application initialization, and the result can be cached.</summary>
        /// <param name="lpFrequency">A pointer to a variable that receives the current performance-counter frequency, in counts per second. If the installed hardware doesn't support a high-resolution performance counter, this parameter can be zero (this will not occur on systems that run Windows XP or later).</param>
        /// <returns>
        ///     <para>If the installed hardware supports a high-resolution performance counter, the return value is nonzero.</para>
        ///     <para>If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error()" />. On systems that run Windows XP or later, the function will always succeed and will thus never return zero.</para>
        /// </returns>
        [DllImport("Kernel32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "QueryPerformanceFrequency", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        public static extern BOOL QueryPerformanceFrequency(
            [Out] long* lpFrequency
        );
        #endregion
    }
}
