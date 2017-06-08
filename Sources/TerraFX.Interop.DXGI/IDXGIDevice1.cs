// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [SuppressUnmanagedCodeSecurity]
    [Guid("77DB970F-6276-48BA-BA28-070143B4392C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IDXGIDevice1 : IDXGIDevice
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new void GetAdapter([MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter pAdapter);

        new void CreateSurface([In] ref DXGI_SURFACE_DESC pDesc, [In] uint NumSurfaces, [In] uint Usage, [In] ref DXGI_SHARED_RESOURCE pSharedResource, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface ppSurface);

        new void QueryResourceResidency([MarshalAs(UnmanagedType.IUnknown), In] ref object ppResources, out DXGI_RESIDENCY pResidencyStatus, [In] uint NumResources);

        new void SetGPUThreadPriority([In] int Priority);

        new int GetGPUThreadPriority();

        void SetMaximumFrameLatency([In] uint MaxLatency);

        void GetMaximumFrameLatency(out uint pMaxLatency);
    }
}
