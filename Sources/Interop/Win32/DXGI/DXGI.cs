// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h and shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public static partial class DXGI
    {
        #region Methods
        [DllImport("DXGI", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateDXGIFactory1", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern HRESULT CreateFactory1(
            [In] ref /* readonly */ Guid riid,
            [Out] void** ppFactory
        );

        [DllImport("DXGI", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "CreateDXGIFactory2", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void CreateDXGIFactory2(
            [In] DXGI_CREATE_FACTORY_FLAG Flags,
            [In] ref /* readonly */ Guid riid,
            [Out] void** ppFactory
        );

        [DllImport("DXGI", BestFitMapping = false, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode, EntryPoint = "DXGIGetDebugInterface1", ExactSpelling = true, PreserveSig = true, SetLastError = false, ThrowOnUnmappableChar = false)]
        public static extern void DXGIGetDebugInterface1(
            [In] uint Flags,
            [In] ref /* readonly */ Guid riid,
            [Out] void** pDebug
        );
        #endregion
    }
}
