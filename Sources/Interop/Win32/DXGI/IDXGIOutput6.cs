// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("068346E8-AAEC-4B84-ADD7-137F513F77A1")]
    unsafe public struct IDXGIOutput6
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIOutput6).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc1(
            [In] IDXGIOutput6* This,
            [Out] DXGI_OUTPUT_DESC1* pDesc
        );

        public /* static */ delegate HRESULT CheckHardwareCompositionSupport(
            [In] IDXGIOutput6* This,
            [Out] DXGI_HARDWARE_COMPOSITION_SUPPORT_FLAGS* pFlags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIOutput5.Vtbl BaseVtbl;

            public GetDesc1 GetDesc1;

            public CheckHardwareCompositionSupport CheckHardwareCompositionSupport;
            #endregion
        }
        #endregion
    }
}
