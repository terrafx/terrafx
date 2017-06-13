// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("8337A8A6-A216-444A-B2F4-314733A73AEA")]
    unsafe public struct ID3D12ShaderReflectionVariable
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12ShaderReflectionVariable).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12ShaderReflectionVariable* This,
            [Out] D3D12_SHADER_VARIABLE_DESC *pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionType* _GetType(
            [In] ID3D12ShaderReflectionVariable* This
        );

        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetBuffer(
            [In] ID3D12ShaderReflectionVariable* This
        );

        public /* static */ delegate uint GetInterfaceSlot(
            [In] ID3D12ShaderReflectionVariable* This,
            [In] uint uArrayIndex
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public GetDesc GetDesc;

            public _GetType _GetType;

            public GetBuffer GetBuffer;

            public GetInterfaceSlot GetInterfaceSlot;
            #endregion
        }
        #endregion
    }
}
