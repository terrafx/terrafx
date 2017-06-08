// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("ABA496DD-B617-4CB8-A866-BC44D7EB1FA2")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGISurface2 : IDXGISurface1
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new IntPtr GetDevice([In] ref Guid riid);

        new void GetDesc(out DXGI_SURFACE_DESC pDesc);

        new void Map(out DXGI_MAPPED_RECT pLockedRect, [In] uint MapFlags);

        new void Unmap();

        new void GetDC([In] int Discard, [Out] IntPtr phdc);

        new void ReleaseDC([In] ref RECT pDirtyRect);

        void GetResource([In] ref Guid riid, out IntPtr ppParentResource, out uint pSubresourceIndex);
    }
}
