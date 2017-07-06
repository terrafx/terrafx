// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3D585D5A-BD4A-489E-B1F4-3DBCB6452FFB")]
    unsafe public /* blittable */ struct IDXGISwapChain4
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetHDRMetaData(
            [In] IDXGISwapChain4* This,
            [In] DXGI_HDR_METADATA_TYPE Type,
            [In] UINT Size,
            [In, Optional] void* pMetaData
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGISwapChain3.Vtbl BaseVtbl;

            public SetHDRMetaData SetHDRMetaData;
            #endregion
        }
        #endregion
    }
}
