// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("4AE63092-6327-4C1B-80AE-BFE12EA32B86")]
    unsafe public struct IDXGISurface1
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDC(
            [In] IDXGISurface1* This,
            [In] BOOL Discard,
            [Out] HDC* phdc
        );

        public /* static */ delegate HRESULT ReleaseDC(
            [In] IDXGISurface1* This,
            [In, Optional] RECT* pDirtyRect
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGISurface.Vtbl BaseVtbl;

            public GetDC GetDC;

            public ReleaseDC ReleaseDC;
            #endregion
        }
        #endregion
    }
}
