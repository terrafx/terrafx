// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("0A753DCF-C4D8-4B91-ADF6-BE5A60D95A76")]
    unsafe public struct ID3D12Fence
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Fence).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate ulong GetCompletedValue(
            [In] ID3D12Fence* This
        );

        public /* static */ delegate HRESULT SetEventOnCompletion(
            [In] ID3D12Fence* This,
            [In] ulong Value,
            [In] IntPtr hEvent
        );

        public /* static */ delegate HRESULT Signal(
            [In] ID3D12Fence* This,
            [In] ulong Value
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public GetCompletedValue GetCompletedValue;

            public SetEventOnCompletion SetEventOnCompletion;

            public Signal Signal;
            #endregion
        }
        #endregion
    }
}
