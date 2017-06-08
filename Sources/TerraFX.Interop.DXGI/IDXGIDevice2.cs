// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("05008617-FBFD-4051-A790-144884B4F6A9")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIDevice2 : IDXGIDevice1
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region IDXGIDevice
        new void GetAdapter([MarshalAs(UnmanagedType.Interface)] out IDXGIAdapter pAdapter);

        new void CreateSurface([In] ref DXGI_SURFACE_DESC pDesc, [In] uint NumSurfaces, [In] uint Usage, [In] ref DXGI_SHARED_RESOURCE pSharedResource, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface ppSurface);

        new void QueryResourceResidency([MarshalAs(UnmanagedType.IUnknown), In] ref object ppResources, out DXGI_RESIDENCY pResidencyStatus, [In] uint NumResources);

        new void SetGPUThreadPriority([In] int Priority);

        new int GetGPUThreadPriority();
        #endregion

        #region IDXGIDevice1
        new void SetMaximumFrameLatency([In] uint MaxLatency);

        new void GetMaximumFrameLatency(out uint pMaxLatency);
        #endregion

        #region Methods
        void OfferResources([In] uint NumResources, [MarshalAs(UnmanagedType.Interface), In] ref IDXGIResource ppResources, [In] DXGI_OFFER_RESOURCE_PRIORITY Priority);

        void ReclaimResources([In] uint NumResources, [MarshalAs(UnmanagedType.Interface), In] ref IDXGIResource ppResources, out int pDiscarded);

        void EnqueueSetEvent([In] IntPtr hEvent);
        #endregion
    }
}
