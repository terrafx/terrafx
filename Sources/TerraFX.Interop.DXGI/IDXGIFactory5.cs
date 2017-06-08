// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("7632E1F5-EE65-4DCA-87FD-84CD75F8838D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIFactory5 : IDXGIFactory4
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region IDXGIFactory
        new void EnumAdapters([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);

        new void MakeWindowAssociation(IntPtr WindowHandle, DXGI_MWA_FLAG Flags);

        new void GetWindowAssociation([Out] IntPtr pWindowHandle);

        new void CreateSwapChain([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] ref DXGI_SWAP_CHAIN_DESC pDesc, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain ppSwapChain);

        new void CreateSoftwareAdapter([In] IntPtr Module, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter ppAdapter);
        #endregion

        #region IDXGIFactory1
        new void EnumAdapters1([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter1 ppAdapter);

        [PreserveSig]
        new int IsCurrent();
        #endregion

        #region IDXGIFactory2
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
        #endregion

        #region IDXGIFactory3
        [PreserveSig]
        new DXGI_CREATE_FACTORY_FLAG GetCreationFlags();
        #endregion

        #region IDXGIFactory4
        new void EnumAdapterByLuid(long AdapterLuid, ref Guid riid, ref IntPtr ppvAdapter);

        new void EnumWarpAdapter(ref Guid riid, ref IntPtr ppvAdapter);
        #endregion

        #region Methods
        void CheckFeatureSupport(DXGI_FEATURE Feature, IntPtr pFeatureSupportData, uint FeatureSupportDataSize);
        #endregion
    }
}
