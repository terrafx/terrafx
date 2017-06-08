// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIAdapter : IDXGIObject
    {
        #region IDXGIObject
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);
        #endregion

        #region Methods
        void EnumOutputs([In] uint Output, [MarshalAs(UnmanagedType.Interface), In, Out] ref IDXGIOutput ppOutput);

        void GetDesc(out DXGI_ADAPTER_DESC pDesc);

        void CheckInterfaceSupport([In] ref Guid InterfaceName, out long pUMDVersion);
        #endregion
    }
}
