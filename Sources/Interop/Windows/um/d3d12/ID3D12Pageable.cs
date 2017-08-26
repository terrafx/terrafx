// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("63EE58FB-1268-4835-86DA-F008CE62F0D6")]
    public /* blittable */ unsafe struct ID3D12Pageable
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        // ID3D12Pageable declares no new members
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
