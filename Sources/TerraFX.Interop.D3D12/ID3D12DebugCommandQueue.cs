// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("09E0BF36-54AC-484F-8847-4BAEEAB6053A")]
    unsafe public struct ID3D12DebugCommandQueue
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DebugCommandQueue).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate BOOL AssertResourceState(
            [In] ID3D12DebugCommandQueue* This,
            [In] ID3D12Resource* pResource,
            [In] uint Subresource,
            [In] uint State
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public AssertResourceState AssertResourceState;
            #endregion
        }
        #endregion
    }
}
