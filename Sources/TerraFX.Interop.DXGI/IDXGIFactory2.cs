// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [SuppressUnmanagedCodeSecurity]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("50C83A1C-E072-4C48-87B0-3630FA36A6D0")]
    public interface IDXGIFactory2 : IDXGIFactory1
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new void EnumAdapters([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);

        new void MakeWindowAssociation(IntPtr WindowHandle, uint Flags);

        new void GetWindowAssociation([Out] IntPtr pWindowHandle);

        new void CreateSwapChain([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC pDesc, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain ppSwapChain);

        new void CreateSoftwareAdapter([In] IntPtr Module, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);

        new void EnumAdapters1([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter1 ppAdapter);

        [PreserveSig]
        new int IsCurrent();

        [PreserveSig]
        int IsWindowedStereoEnabled();

        void CreateSwapChainForHwnd([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] IntPtr hWnd, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [In] ref DXGI_SWAP_CHAIN_FULLSCREEN_DESC pFullscreenDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);

        void CreateSwapChainForCoreWindow([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [MarshalAs(UnmanagedType.IUnknown), In] object pWindow, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);

        void GetSharedResourceAdapterLuid(IntPtr hResource, ref long plong);

        void RegisterStereoStatusWindow([In] IntPtr WindowHandle, [In] uint wMsg, out uint pdwCookie);

        void RegisterStereoStatusEvent([In] IntPtr hEvent, out uint pdwCookie);

        [PreserveSig]
        void UnregisterStereoStatus([In] uint dwCookie);

        void RegisterOcclusionStatusWindow([In] IntPtr WindowHandle, [In] uint wMsg, out uint pdwCookie);

        void RegisterOcclusionStatusEvent([In] IntPtr hEvent, out uint pdwCookie);

        [PreserveSig]
        void UnregisterOcclusionStatus([In] uint dwCookie);

        void CreateSwapChainForComposition([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);
    }
}
