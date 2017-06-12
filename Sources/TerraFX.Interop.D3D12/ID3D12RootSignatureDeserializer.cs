// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("34AB647B-3CC8-46AC-841B-C0965645C046")]
    unsafe public struct ID3D12RootSignatureDeserializer
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12RootSignatureDeserializer).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate D3D12_ROOT_SIGNATURE_DESC* GetRootSignatureDesc(
            [In] ID3D12RootSignatureDeserializer* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetRootSignatureDesc GetRootSignatureDesc;
            #endregion
        }
        #endregion
    }
}
