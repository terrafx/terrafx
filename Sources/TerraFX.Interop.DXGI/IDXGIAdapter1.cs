// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("29038F61-3839-4626-91FD-086879011A05")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIAdapter1 : IDXGIAdapter
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region IDXGIAdapter
        new void EnumOutputs([In] uint Output, [MarshalAs(UnmanagedType.Interface), In, Out] ref IDXGIOutput ppOutput);

        new void GetDesc(out DXGI_ADAPTER_DESC pDesc);

        new void CheckInterfaceSupport([In] ref Guid InterfaceName, out long pUMDVersion);
        #endregion

        #region Methods
        void GetDesc1(out DXGI_ADAPTER_DESC1 pDesc);
        #endregion
    }
}
