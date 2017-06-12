// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("344488B7-6846-474B-B989-F027448245E0")]
    unsafe public struct ID3D12Debug
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Debug).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void EnableDebugLayer(
            [In] ID3D12Debug* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public EnableDebugLayer EnableDebugLayer;
            #endregion
        }
        #endregion
    }
}
