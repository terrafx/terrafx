// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("310D36A0-D2E7-4C0A-AA04-6A9D23B8886A")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGISwapChain : IDXGIDeviceSubObject
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new IntPtr GetDevice([In] ref Guid riid);

        void Present([In] uint SyncInterval, [In] uint Flags);

        void GetBuffer([In] uint Buffer, [In] ref Guid riid, [In, Out] ref IntPtr ppSurface);

        void SetFullscreenState([In] int Fullscreen, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pTarget);

        void GetFullscreenState(out int pFullscreen, [MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppTarget);

        void GetDesc(out DXGI_SWAP_CHAIN_DESC pDesc);

        void ResizeBuffers([In] uint BufferCount, [In] uint Width, [In] uint Height, [In] DXGI_FORMAT NewFormat, [In] uint SwapChainFlags);

        void ResizeTarget([In] ref DXGI_MODE_DESC pNewTargetParameters);

        void GetContainingOutput([MarshalAs(UnmanagedType.Interface)] out IDXGIOutput ppOutput);

        void GetFrameStatistics(out DXGI_FRAME_STATISTICS pStats);

        void GetLastPresentCount(out uint pLastPresentCount);
    }
}
