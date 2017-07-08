// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("C54A6B66-72DF-4EE8-8BE5-A946A1429214")]
    unsafe public /* blittable */ struct ID3D12RootSignature
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        // ID3D12RootSignature declares no new members
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12DeviceChild.Vtbl BaseVtbl;
            #endregion
        }
        #endregion
    }
}
