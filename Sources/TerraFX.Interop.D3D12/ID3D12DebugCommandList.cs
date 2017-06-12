// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("09E0BF36-54AC-484F-8847-4BAEEAB6053F")]
    unsafe public struct ID3D12DebugCommandList
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DebugCommandList).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate int AssertResourceState(
            [In] ID3D12DebugCommandList* This,
            [In] ID3D12Resource* pResource,
            [In] uint Subresource,
            [In] uint State
        );

        public /* static */ delegate HRESULT SetFeatureMask(
            [In] ID3D12DebugCommandList* This,
            [In] D3D12_DEBUG_FEATURE Mask
        );

        public /* static */ delegate D3D12_DEBUG_FEATURE GetFeatureMask(
            [In] ID3D12DebugCommandList* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public AssertResourceState AssertResourceState;

            public SetFeatureMask SetFeatureMask;

            public GetFeatureMask GetFeatureMask;
            #endregion
        }
        #endregion
    }
}
