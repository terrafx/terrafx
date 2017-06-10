// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("595E39D1-2724-4663-99B1-DA969DE28364")]
    unsafe public struct IDXGIOutput2
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate BOOL SupportsOverlays(
            [In] IDXGIOutput2* This
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

            public SupportsOverlays SupportsOverlays;
            #endregion
        }
        #endregion
    }
}
