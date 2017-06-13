// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("7F91CE67-090C-4BB7-B78E-ED8FF2E31DA0")]
    unsafe public struct ID3D12VersionedRootSignatureDeserializer
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12VersionedRootSignatureDeserializer).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetRootSignatureDescAtVersion(
            [In] ID3D12VersionedRootSignatureDeserializer* This,
            [In] D3D_ROOT_SIGNATURE_VERSION convertToVersion,
            [Out] D3D12_VERSIONED_ROOT_SIGNATURE_DESC** ppDesc
        );

        public /* static */ delegate D3D12_VERSIONED_ROOT_SIGNATURE_DESC* GetUnconvertedRootSignatureDesc(
            [In] ID3D12VersionedRootSignatureDeserializer* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetRootSignatureDescAtVersion GetRootSignatureDescAtVersion;

            public GetUnconvertedRootSignatureDesc GetUnconvertedRootSignatureDesc;
            #endregion
        }
        #endregion
    }
}
