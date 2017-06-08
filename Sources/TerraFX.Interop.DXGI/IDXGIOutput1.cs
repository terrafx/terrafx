// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("00CDDEA8-939B-4B83-A340-A685226666CC")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIOutput1 : IDXGIOutput
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new void GetDesc(out DXGI_OUTPUT_DESC pDesc);

        new void GetDisplayModeList([In] DXGI_FORMAT EnumFormat, [In] uint Flags, [In, Out] ref uint pNumModes, out DXGI_MODE_DESC pDesc);

        new void FindClosestMatchingMode([In] ref DXGI_MODE_DESC pModeToMatch, out DXGI_MODE_DESC pClosestMatch, [MarshalAs(UnmanagedType.IUnknown), In] object pConcernedDevice);

        new void WaitForVBlank();

        new void TakeOwnership([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, int Exclusive);

        [PreserveSig]
        new void ReleaseOwnership();

        new void GetGammaControlCapabilities(out DXGI_GAMMA_CONTROL_CAPABILITIES pGammaCaps);

        new void SetGammaControl([In] ref DXGI_GAMMA_CONTROL pArray);

        new void GetGammaControl(out DXGI_GAMMA_CONTROL pArray);

        new void SetDisplaySurface([MarshalAs(UnmanagedType.Interface), In] IDXGISurface pScanoutSurface);

        new void GetDisplaySurfaceData([MarshalAs(UnmanagedType.Interface), In] IDXGISurface pDestination);

        new void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);

        void GetDisplayModeList1([In] DXGI_FORMAT EnumFormat, [In] uint Flags, [In, Out] ref uint pNumModes, out DXGI_MODE_DESC1 pDesc);

        void FindClosestMatchingMode1([In] ref DXGI_MODE_DESC1 pModeToMatch, out DXGI_MODE_DESC1 pClosestMatch, [MarshalAs(UnmanagedType.IUnknown), In] object pConcernedDevice);

        void GetDisplaySurfaceData1([MarshalAs(UnmanagedType.Interface), In] IDXGIResource pDestination);

        void DuplicateOutput([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutputDuplication ppOutputDuplication);
    }
}
