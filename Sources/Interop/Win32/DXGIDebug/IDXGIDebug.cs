// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("119E7452-DE9E-40FE-8806-88F90C12B441")]
    unsafe public struct IDXGIDebug
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIDebug).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT ReportLiveObjects(
            [In] IDXGIDebug* This,
            [In] Guid apiid,
            [In] DXGI_DEBUG_RLO_FLAGS flags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public ReportLiveObjects ReportLiveObjects;
            #endregion
        }
        #endregion
    }
}
