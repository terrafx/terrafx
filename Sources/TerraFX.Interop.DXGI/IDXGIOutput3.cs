// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("8A6BB301-7E7E-41F4-A8E0-5B32F7F99B18")]
    unsafe public struct IDXGIOutput3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CheckOverlaySupport(
            [In] IDXGIOutput3* This,
            [In] DXGI_FORMAT EnumFormat,
            [In] IUnknown* pConcernedDevice,
            [Out] DXGI_OVERLAY_SUPPORT_FLAG* pFlags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIOutput2.Vtbl BaseVtbl;

            public CheckOverlaySupport CheckOverlaySupport;
            #endregion
        }
        #endregion
    }
}
