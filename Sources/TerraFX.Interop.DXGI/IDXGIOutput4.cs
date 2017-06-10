// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("DC7DCA35-2196-414D-9F53-617884032A60")]
    unsafe public struct IDXGIOutput4
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CheckOverlayColorSpaceSupport(
            [In] IDXGIOutput4* This,
            [In] DXGI_FORMAT Format,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [In] IUnknown* pConcernedDevice,
            [Out] DXGI_OVERLAY_COLOR_SPACE_SUPPORT_FLAG *pFlags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIOutput3.Vtbl BaseVtbl;

            public CheckOverlayColorSpaceSupport CheckOverlayColorSpaceSupport;
            #endregion
        }
        #endregion
    }
}
