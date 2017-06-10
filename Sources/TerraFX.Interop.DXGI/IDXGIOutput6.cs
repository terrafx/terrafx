// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("068346E8-AAEC-4B84-ADD7-137F513F77A1")]
    unsafe public struct IDXGIOutput6
    {
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
            public IUnknown.QueryInterface QueryInterface;

            public IUnknown.AddRef AddRef;

            public IUnknown.Release Release;

            public IDXGIObject.SetPrivateData SetPrivateData;

            public IDXGIObject.SetPrivateDataInterface SetPrivateDataInterface;

            public IDXGIObject.GetPrivateData GetPrivateData;

            public IDXGIObject.GetParent GetParent;

            public IDXGIOutput.GetDesc GetDesc;

            public IDXGIOutput.GetDisplayModeList GetDisplayModeList;

            public IDXGIOutput.FindClosestMatchingMode FindClosestMatchingMode;

            public IDXGIOutput.WaitForVBlank WaitForVBlank;

            public IDXGIOutput.TakeOwnership TakeOwnership;

            public IDXGIOutput.ReleaseOwnership ReleaseOwnership;

            public IDXGIOutput.GetGammaControlCapabilities GetGammaControlCapabilities;

            public IDXGIOutput.SetGammaControl SetGammaControl;

            public IDXGIOutput.GetGammaControl GetGammaControl;

            public IDXGIOutput.SetDisplaySurface SetDisplaySurface;

            public IDXGIOutput.GetDisplaySurfaceData GetDisplaySurfaceData;

            public IDXGIOutput.GetFrameStatistics GetFrameStatistics;

            public IDXGIOutput1.GetDisplayModeList1 GetDisplayModeList1;

            public IDXGIOutput1.FindClosestMatchingMode1 FindClosestMatchingMode1;

            public IDXGIOutput1.GetDisplaySurfaceData1 GetDisplaySurfaceData1;

            public IDXGIOutput1.DuplicateOutput DuplicateOutput;

            public IDXGIOutput2.SupportsOverlays SupportsOverlays;

            public IDXGIOutput3.CheckOverlaySupport CheckOverlaySupport;

            public IDXGIOutput4.CheckOverlayColorSpaceSupport CheckOverlayColorSpaceSupport;

            public IDXGIOutput5.DuplicateOutput1 DuplicateOutput1;

            public GetDesc1 GetDesc1;

            public CheckHardwareCompositionSupport CheckHardwareCompositionSupport;
            #endregion
        }
        #endregion
    }
}
