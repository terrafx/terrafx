// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("3D585D5A-BD4A-489E-B1F4-3DBCB6452FFB")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGISwapChain4 : IDXGISwapChain3
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region IDXGIDeviceSubObject
        new IntPtr GetDevice([In] ref Guid riid);
        #endregion

        #region IDXGISwapChain
        new void Present([In] uint SyncInterval, [In] DXGI_PRESENT_FLAG Flags);

        new void GetBuffer([In] uint Buffer, [In] ref Guid riid, [In, Out] ref IntPtr ppSurface);

        new void SetFullscreenState([In] int Fullscreen, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pTarget);

        new void GetFullscreenState(out int pFullscreen, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppTarget);

        new void GetDesc(out DXGI_SWAP_CHAIN_DESC pDesc);

        new void ResizeBuffers([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT NewFormat, [In] DXGI_SWAP_CHAIN_FLAG SwapChainFlags);

        new void ResizeTarget([In] ref DXGI_MODE_DESC pNewTargetParameters);

        new void GetContainingOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppOutput);

        new void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);

        new void GetLastPresentCount(out uint pLastPresentCount);
        #endregion

        #region IDXGISwapChain1
        new void GetDesc1(out DXGI_SWAP_CHAIN_DESC1 pDesc);

        new void GetFullscreenDesc(out DXGI_SWAP_CHAIN_FULLSCREEN_DESC pDesc);

        new void GetHwnd([Out] IntPtr pHwnd);

        new void GetCoreWindow([In] ref Guid refiid, out IntPtr ppUnk);

        new void Present1([In] uint SyncInterval, [In] DXGI_PRESENT_FLAG PresentFlags, [In] ref DXGI_PRESENT_PARAMETERS pPresentParameters);

        [PreserveSig]
        new int IsTemporaryMonoSupported();

        new void GetRestrictToOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppRestrictToOutput);

        new void SetBackgroundColor([In] ref DXGI_RGBA pColor);

        new void GetBackgroundColor(out DXGI_RGBA pColor);

        new void SetRotation([In] DXGI_MODE_ROTATION Rotation);

        new void GetRotation(out DXGI_MODE_ROTATION pRotation);
        #endregion

        #region IDXGISwapChain2
        new void SetSourceSize(uint Width, uint Height);

        new void GetSourceSize(out uint pWidth, out uint pHeight);

        new void SetMaximumFrameLatency(uint MaxLatency);

        new void GetMaximumFrameLatency(out uint pMaxLatency);

        [PreserveSig]
        new IntPtr GetFrameLatencyWaitableObject();

        new void SetMatrixTransform(ref DXGI_MATRIX_3X2_F pMatrix);

        new void GetMatrixTransform(out DXGI_MATRIX_3X2_F pMatrix);
        #endregion

        #region IDXGISwapChain3
        [PreserveSig]
        new uint GetCurrentBackBufferIndex();

        new void CheckColorSpaceSupport([In] DXGI_COLOR_SPACE_TYPE ColorSpace, out uint pColorSpaceSupport);

        new void SetColorSpace1([In] DXGI_COLOR_SPACE_TYPE ColorSpace);

        new void ResizeBuffers1([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT Format, [In] DXGI_SWAP_CHAIN_FLAG SwapChainFlags, [In] ref uint pCreationNodeMask, [MarshalAs(UnmanagedType.IUnknown), In] ref object ppPresentQueue);
        #endregion

        #region Methods
        void SetHDRMetaData([In] DXGI_HDR_METADATA_TYPE Type, [In] uint Size, [In] IntPtr pMetaData);
        #endregion
    }
}
