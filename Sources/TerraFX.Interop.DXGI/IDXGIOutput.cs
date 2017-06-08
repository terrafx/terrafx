// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("AE02EEDB-C735-4690-8D52-5A8DC20213AA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIOutput : IDXGIObject
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region Methods
        void GetDesc(out DXGI_OUTPUT_DESC pDesc);

        void GetDisplayModeList([In] DXGI_FORMAT EnumFormat, [In] DXGI_ENUM_MODES Flags, [In, Out] ref uint pNumModes, out DXGI_MODE_DESC pDesc);

        void FindClosestMatchingMode([In] ref DXGI_MODE_DESC pModeToMatch, out DXGI_MODE_DESC pClosestMatch, [MarshalAs(UnmanagedType.IUnknown), In] object pConcernedDevice);

        void WaitForVBlank();

        void TakeOwnership([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, int Exclusive);

        [PreserveSig]
        void ReleaseOwnership();

        void GetGammaControlCapabilities(out DXGI_GAMMA_CONTROL_CAPABILITIES pGammaCaps);

        void SetGammaControl([In] ref DXGI_GAMMA_CONTROL pArray);

        void GetGammaControl(out DXGI_GAMMA_CONTROL pArray);

        void SetDisplaySurface([MarshalAs(UnmanagedType.Interface), In] IDXGISurface pScanoutSurface);

        void GetDisplaySurfaceData([MarshalAs(UnmanagedType.Interface), In] IDXGISurface pDestination);

        void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);
        #endregion
    }
}
