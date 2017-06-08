// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
    [SuppressUnmanagedCodeSecurity]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIFactory4 : IDXGIFactory3
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
        new int IsWindowedStereoEnabled();

        new void CreateSwapChainForHwnd([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] IntPtr hWnd, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [In] ref DXGI_SWAP_CHAIN_FULLSCREEN_DESC pFullscreenDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);

        new void CreateSwapChainForCoreWindow([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [MarshalAs(UnmanagedType.IUnknown), In] object pWindow, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);

        new void GetSharedResourceAdapterLuid(IntPtr hResource, ref long plong);

        new void RegisterStereoStatusWindow([In] IntPtr WindowHandle, [In] uint wMsg, out uint pdwCookie);

        new void RegisterStereoStatusEvent([In] IntPtr hEvent, out uint pdwCookie);

        [PreserveSig]
        new void UnregisterStereoStatus([In] uint dwCookie);

        new void RegisterOcclusionStatusWindow([In] IntPtr WindowHandle, [In] uint wMsg, out uint pdwCookie);

        new void RegisterOcclusionStatusEvent([In] IntPtr hEvent, out uint pdwCookie);

        [PreserveSig]
        new void UnregisterOcclusionStatus([In] uint dwCookie);

        new void CreateSwapChainForComposition([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);

        [PreserveSig]
        new uint GetCreationFlags();

        void EnumAdapterBylong(long AdapterLuid, ref Guid riid, ref IntPtr ppvAdapter);

        void EnumWarpAdapter(ref Guid riid, ref IntPtr ppvAdapter);
    }
}
