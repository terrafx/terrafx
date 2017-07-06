// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("770AAE78-F26F-4DBA-A829-253C83D1B387")]
    unsafe public /* blittable */ struct IDXGIFactory1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT EnumAdapters1(
            [In] IDXGIFactory1* This,
            [In] UINT Adapter,
            [Out] IDXGIAdapter1** ppAdapter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL IsCurrent(
            [In] IDXGIFactory1* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIFactory.Vtbl BaseVtbl;

            public EnumAdapters1 EnumAdapters1;

            public IsCurrent IsCurrent;
            #endregion
        }
        #endregion
    }
}
