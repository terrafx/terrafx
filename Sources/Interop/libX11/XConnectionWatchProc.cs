// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = false, ThrowOnUnmappableChar = false)]
    public /* static */ unsafe delegate void XConnectionWatchProc(
        [In, ComAliasName("Display")] IntPtr display,
        [In, ComAliasName("XPointer")] sbyte* client_data,
        [In] int fx,
        [In, ComAliasName("Bool")] int opening,
        [In, ComAliasName("XPointer")] sbyte** watch_data
    );
}
