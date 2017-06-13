// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("E913C351-783D-48CA-A1D1-4F306284AD56")]
    unsafe public struct ID3D12ShaderReflectionType
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12ShaderReflectionType).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12ShaderReflectionType* This,
            [Out] D3D12_SHADER_TYPE_DESC* pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionType* GetMemberTypeByIndex(
            [In] ID3D12ShaderReflectionType* This,
            [In] uint Index
        );

        public /* static */ delegate ID3D12ShaderReflectionType* GetMemberTypeByName(
            [In] ID3D12ShaderReflectionType* This,
            [In] LPSTR Name
        );

        public /* static */ delegate LPSTR GetMemberTypeName(
            [In] ID3D12ShaderReflectionType* This,
            [In] uint Index
        );

        public /* static */ delegate HRESULT IsEqual(
            [In] ID3D12ShaderReflectionType* This,
            [In] ID3D12ShaderReflectionType* pType
        );

        public /* static */ delegate ID3D12ShaderReflectionType* GetSubType(
            [In] ID3D12ShaderReflectionType* This
        );

        public /* static */ delegate ID3D12ShaderReflectionType* GetBaseClass(
            [In] ID3D12ShaderReflectionType* This
        );

        public /* static */ delegate uint GetNumInterfaces(
            [In] ID3D12ShaderReflectionType* This
        );

        public /* static */ delegate ID3D12ShaderReflectionType* GetInterfaceByIndex(
            [In] ID3D12ShaderReflectionType* This,
            [In] uint uIndex
        );

        public /* static */ delegate HRESULT IsOfType(
            [In] ID3D12ShaderReflectionType* This,
            [In] ID3D12ShaderReflectionType* pType
        );

        public /* static */ delegate HRESULT ImplementsInterface(
            [In] ID3D12ShaderReflectionType* This,
            [In] ID3D12ShaderReflectionType* pBase
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public GetDesc GetDesc;

            public GetMemberTypeByIndex GetMemberTypeByIndex;

            public GetMemberTypeByName GetMemberTypeByName;

            public GetMemberTypeName GetMemberTypeName;

            public IsEqual IsEqual;

            public GetSubType GetSubType;

            public GetBaseClass GetBaseClass;

            public GetNumInterfaces GetNumInterfaces;

            public GetInterfaceByIndex GetInterfaceByIndex;

            public IsOfType IsOfType;

            public ImplementsInterface ImplementsInterface;
            #endregion
        }
        #endregion
    }
}
