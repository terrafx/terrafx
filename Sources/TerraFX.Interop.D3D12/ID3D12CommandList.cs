// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("7116D91C-E7E4-47CE-B8C6-EC8168F437E5")]
    unsafe public struct ID3D12CommandList
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12CommandList).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate D3D12_COMMAND_LIST_TYPE _GetType(
            [In] ID3D12CommandList* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public _GetType _GetType;
            #endregion
        }
        #endregion
    }
}
