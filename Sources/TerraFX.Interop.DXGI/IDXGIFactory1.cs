// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("770AAE78-F26F-4DBA-A829-253C83D1B387")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIFactory1 : IDXGIFactory
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

        void EnumAdapters1([In] uint Adapter, [MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter1 ppAdapter);

        [PreserveSig]
        int IsCurrent();
    }
}
