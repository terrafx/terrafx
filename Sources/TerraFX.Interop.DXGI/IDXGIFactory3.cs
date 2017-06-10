// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("25483823-CD46-4C7D-86CA-47AA95B837BD")]
    unsafe public struct IDXGIFactory3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate DXGI_CREATE_FACTORY_FLAG GetCreationFlags(
            [In] IDXGIFactory3* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIFactory2.Vtbl BaseVtbl;

            public GetCreationFlags GetCreationFlags;
            #endregion
        }
        #endregion
    }
}
