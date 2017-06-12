// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("6102DEE4-AF59-4B09-B999-B44D73F09B24")]
    unsafe public struct ID3D12CommandAllocator
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12CommandAllocator).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Reset(
            [In] ID3D12CommandAllocator* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public Reset Reset;
            #endregion
        }
        #endregion
    }
}
