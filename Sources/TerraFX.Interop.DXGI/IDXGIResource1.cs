// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("30961379-4609-4A41-998E-54FE567EE0C1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIResource1 : IDXGIResource
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region IDXGIDeviceSubObject
        new IntPtr GetDevice([In] ref Guid riid);
        #endregion

        #region IDXGIResource
        new void GetSharedHandle(out IntPtr pSharedHandle);

        new void GetUsage(out uint pUsage);

        new void SetEvictionPriority([In] DXGI_RESOURCE_PRIORITY EvictionPriority);

        new DXGI_RESOURCE_PRIORITY GetEvictionPriority();
        #endregion

        #region Methods
        void CreateSubresourceSurface(uint index, [MarshalAs(UnmanagedType.Interface)] out IDXGISurface2 ppSurface);

        void CreateSharedHandle([In] ref SECURITY_ATTRIBUTES pAttributes, [In] uint dwAccess, [MarshalAs(UnmanagedType.LPWStr), In] string lpName, out IntPtr pHandle);
        #endregion
    }
}
