// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("790A45F7-0D42-4876-983A-0A55CFE6F4AA")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGISwapChain1 : IDXGISwapChain
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new IntPtr GetDevice([In] ref Guid riid);

        new void Present([In] uint SyncInterval, [In] uint Flags);

        new void GetBuffer([In] uint Buffer, [In] ref Guid riid, [In, Out] ref IntPtr ppSurface);

        new void SetFullscreenState([In] int Fullscreen, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pTarget);

        new void GetFullscreenState(out int pFullscreen, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppTarget);

        new void GetDesc(out DXGI_SWAP_CHAIN_DESC pDesc);

        new void ResizeBuffers([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT NewFormat, [In] uint SwapChainFlags);

        new void ResizeTarget([In] ref DXGI_MODE_DESC pNewTargetParameters);

        new void GetContainingOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppOutput);

        new void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);

        new void GetLastPresentCount(out uint pLastPresentCount);

        void GetDesc1(out DXGI_SWAP_CHAIN_DESC1 pDesc);

        void GetFullscreenDesc(out DXGI_SWAP_CHAIN_FULLSCREEN_DESC pDesc);

        void GetHwnd([Out] IntPtr pHwnd);

        void GetCoreWindow([In] ref Guid refiid, out IntPtr ppUnk);

        void Present1([In] uint SyncInterval, [In] uint PresentFlags, [In] ref DXGI_PRESENT_PARAMETERS pPresentParameters);

        [PreserveSig]
        int IsTemporaryMonoSupported();

        void GetRestrictToOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppRestrictToOutput);

        void SetBackgroundColor([In] ref DXGI_RGBA pColor);

        void GetBackgroundColor(out DXGI_RGBA pColor);

        void SetRotation([In] DXGI_MODE_ROTATION Rotation);

        void GetRotation(out DXGI_MODE_ROTATION pRotation);
    }
}
