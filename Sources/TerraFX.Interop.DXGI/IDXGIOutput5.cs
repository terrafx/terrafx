// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("80A07424-AB52-42EB-833C-0C42FD282D98")]
    unsafe public struct IDXGIOutput5
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT DuplicateOutput1(
            [In] IDXGIOutput5* This,
            [In] IUnknown* pDevice,
            [In] uint Flags,
            [In] uint SupportedFormatsCount,
            [In] /* readonly */ DXGI_FORMAT* pSupportedFormats,
            [Out] IDXGIOutputDuplication** ppOutputDuplication
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIOutput4.Vtbl BaseVtbl;

            public DuplicateOutput1 DuplicateOutput1;
            #endregion
        }
        #endregion
    }
}
