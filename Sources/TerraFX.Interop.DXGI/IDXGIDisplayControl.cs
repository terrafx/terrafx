// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("EA9DBF1A-C88E-4486-854A-98AA0138F30C")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIDisplayControl
    {
        #region Methods
        [PreserveSig]
        int IsStereoEnabled();

        [PreserveSig]
        void SetStereoEnabled(int enabled);
        #endregion
    }
}
