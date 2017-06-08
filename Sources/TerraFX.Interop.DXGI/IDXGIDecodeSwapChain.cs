// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("2633066B-4514-4C7A-8FD8-12EA98059D18")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIDecodeSwapChain
    {
        void PresentBuffer(uint BufferToPresent, uint SyncInterval, uint Flags);

        void SetSourceRect(ref RECT pRect);

        void SetTargetRect(ref RECT pRect);

        void SetDestSize(uint Width, uint Height);

        void GetSourceRect(out RECT pRect);

        void GetTargetRect(out RECT pRect);

        void GetDestSize(out uint pWidth, out uint pHeight);

        void SetColorSpace(DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS ColorSpace);

        [PreserveSig]
        DXGI_MULTIPLANE_OVERLAY_YCbCr_FLAGS GetColorSpace();
    }
}
