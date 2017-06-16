// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("CAFCB56C-6AC3-4889-BF47-9E23BBD260EC")]
    unsafe public struct IDXGISurface
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGISurface).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] IDXGISurface* This,
            [Out] DXGI_SURFACE_DESC* pDesc
        );

        public /* static */ delegate HRESULT Map(
            [In] IDXGISurface* This,
            [Out] DXGI_MAPPED_RECT* pLockedRect,
            [In] DXGI_MAP_FLAG MapFlags
        );

        public /* static */ delegate HRESULT Unmap(
            [In] IDXGISurface* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIDeviceSubObject.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public Map Map;

            public Unmap Unmap;
            #endregion
        }
        #endregion
    }
}
