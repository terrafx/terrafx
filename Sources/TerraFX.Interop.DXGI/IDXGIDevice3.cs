// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("6007896C-3244-4AFD-BF18-A6D3BEDA5023")]
    unsafe public struct IDXGIDevice3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void Trim(
            [In] IDXGIDevice3* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIDevice2.Vtbl BaseVtbl;

            public Trim Trim;
            #endregion
        }
        #endregion
    }
}
