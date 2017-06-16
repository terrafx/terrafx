// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public static class DXGIDebug
    {
        #region Methods
        [DllImport("DXGIDebug", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DXGIGetDebugInterface", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT DXGIGetDebugInterface(
            [In] ref /* readonly */ Guid riid,
            [Out] void** pDebug
        );
        #endregion
    }
}
