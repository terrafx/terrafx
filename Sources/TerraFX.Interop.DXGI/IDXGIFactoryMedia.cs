// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("41E7D1F2-A591-4F7B-A2E5-FA9C843E1C12")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIFactoryMedia
    {
        void CreateSwapChainForCompositionSurfaceHandle([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] IntPtr hSurface, [In] ref DXGI_SWAP_CHAIN_DESC1 pDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGISwapChain1 ppSwapChain);

        void CreateDecodeSwapChainForCompositionSurfaceHandle([MarshalAs(UnmanagedType.IUnknown), In] object pDevice, [In] IntPtr hSurface, [In] ref DXGI_DECODE_SWAP_CHAIN_DESC pDesc, [MarshalAs(UnmanagedType.Interface), In] IDXGIResource pYuvDecodeBuffers, [MarshalAs(UnmanagedType.Interface), In] IDXGIOutput pRestrictToOutput, [MarshalAs(UnmanagedType.Interface)] out IDXGIDecodeSwapChain ppSwapChain);
    }
}
