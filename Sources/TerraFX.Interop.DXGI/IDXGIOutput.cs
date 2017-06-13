// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("AE02EEDB-C735-4690-8D52-5A8DC20213AA")]
    unsafe public struct IDXGIOutput
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] IDXGIOutput* This,
            [Out] DXGI_OUTPUT_DESC* pDesc
        );

        public /* static */ delegate HRESULT GetDisplayModeList(
            [In] IDXGIOutput* This,
            [In] DXGI_FORMAT EnumFormat,
            [In] DXGI_ENUM_MODES Flags,
            [In, Out] uint* pNumModes,
            [Out, Optional] DXGI_MODE_DESC* pDesc
        );

        public /* static */ delegate HRESULT FindClosestMatchingMode(
            [In] IDXGIOutput* This,
            [In] /* readonly */ DXGI_MODE_DESC* pModeToMatch,
            [Out] DXGI_MODE_DESC* pClosestMatch,
            [In, Optional] IUnknown* pConcernedDevice
        );

        public /* static */ delegate HRESULT WaitForVBlank(
            [In] IDXGIOutput* This
        );

        public /* static */ delegate HRESULT TakeOwnership(
            [In] IDXGIOutput* This,
            [In] IUnknown* pDevice,
            [In] BOOL Exclusive
        );

        public /* static */ delegate void ReleaseOwnership(
            [In] IDXGIOutput* This
        );

        public /* static */ delegate HRESULT GetGammaControlCapabilities(
            [In] IDXGIOutput* This,
            [Out] DXGI_GAMMA_CONTROL_CAPABILITIES* pGammaCaps
        );

        public /* static */ delegate HRESULT SetGammaControl(
            [In] IDXGIOutput* This,
            [In] /* readonly */ DXGI_GAMMA_CONTROL* pArray
        );

        public /* static */ delegate HRESULT GetGammaControl(
            [In] IDXGIOutput* This,
            [Out] DXGI_GAMMA_CONTROL* pArray
        );

        public /* static */ delegate HRESULT SetDisplaySurface(
            [In] IDXGIOutput* This,
            [In] IDXGISurface* pScanoutSurface
        );

        public /* static */ delegate HRESULT GetDisplaySurfaceData(
            [In] IDXGIOutput* This,
            [In] IDXGISurface* pDestination
        );

        public /* static */ delegate HRESULT GetFrameStatistics(
            [In] IDXGIOutput* This,
            [Out] DXGI_FRAME_STATISTICS* pStats
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIObject.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public GetDisplayModeList GetDisplayModeList;

            public FindClosestMatchingMode FindClosestMatchingMode;

            public WaitForVBlank WaitForVBlank;

            public TakeOwnership TakeOwnership;

            public ReleaseOwnership ReleaseOwnership;

            public GetGammaControlCapabilities GetGammaControlCapabilities;

            public SetGammaControl SetGammaControl;

            public GetGammaControl GetGammaControl;

            public SetDisplaySurface SetDisplaySurface;

            public GetDisplaySurfaceData GetDisplaySurfaceData;

            public GetFrameStatistics GetFrameStatistics;
            #endregion
        }
        #endregion
    }
}
