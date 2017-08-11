// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\synchapi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    unsafe public static partial class Kernel32
    {
        #region Extern Methods
        [DllImport("Kernel32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateEventW", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("HANDLE")]
        public static extern IntPtr CreateEvent(
            [In, Optional, ComAliasName("LPSECURITY_ATTRIBUTES")] SECURITY_ATTRIBUTES* lpEventAttributes,
            [In, ComAliasName("BOOL")] int bManualReset,
            [In, ComAliasName("BOOL")] int bInitialState,
            [In, ComAliasName("LPCWSTR")] /* readonly */ char* lpName = null
        );

        [DllImport("Kernel32", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "WaitForSingleObject", ExactSpelling = true, PreserveSig = true, SetLastError = true, ThrowOnUnmappableChar = false)]
        [SuppressUnmanagedCodeSecurity]
        [return: ComAliasName("DWORD")]
        public static extern uint WaitForSingleObject(
            [In, ComAliasName("HANDLE")] IntPtr hHandle,
            [In, ComAliasName("DWORD")] uint dwMilliseconds
        );
        #endregion
    }
}
