// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.D3D12
{
    [Guid("6B3B2502-6E51-45B3-90EE-9884265E8DF3")]
    unsafe public struct ID3D12Heap
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Heap).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate D3D12_HEAP_DESC GetDesc(
            [In] ID3D12Heap* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public GetDesc GetDesc;
            #endregion
        }
        #endregion
    }
}
