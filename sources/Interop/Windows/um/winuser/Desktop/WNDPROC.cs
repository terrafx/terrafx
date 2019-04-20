// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\winuser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;

namespace TerraFX.Interop.Desktop
{
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Winapi, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
    [return: NativeTypeName("LRESULT")]
    public /* static */ unsafe delegate nint WNDPROC(
        [In, NativeTypeName("HWND")] IntPtr hWnd,
        [In, NativeTypeName("UINT")] uint Msg,
        [In, NativeTypeName("WPARAM")] nuint wParam,
        [In, NativeTypeName("LPARAM")] nint lParam
    );
}
