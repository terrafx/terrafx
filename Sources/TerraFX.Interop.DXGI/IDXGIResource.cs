// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("035F3AB4-482E-4E50-B41F-8A7F8BD8960B")]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIResource : IDXGIDeviceSubObject
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new IntPtr GetDevice([In] ref Guid riid);

        void GetSharedHandle(out IntPtr pSharedHandle);

        void GetUsage(out uint pUsage);

        void SetEvictionPriority([In] uint EvictionPriority);

        uint GetEvictionPriority();
    }
}
