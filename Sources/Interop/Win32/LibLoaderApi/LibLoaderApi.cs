// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\libloaderapi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
namespace TerraFX.Interop
{
    /// <summary>Exposes methods exported by <c>um\libloaderapi.h</c>.</summary>
    unsafe public static partial class LibLoaderApi
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
        #endregion
    }
}
