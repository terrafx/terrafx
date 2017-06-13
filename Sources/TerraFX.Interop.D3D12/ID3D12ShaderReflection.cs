// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("5A58797D-A72C-478D-8BA2-EFC6B0EFE88E")]
    unsafe public struct ID3D12ShaderReflection
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12ShaderReflection).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12ShaderReflection* This,
            [Out] D3D12_SHADER_DESC* pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByIndex(
            [In] ID3D12ShaderReflection* This,
            [In] uint Index
        );

        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByName(
            [In] ID3D12ShaderReflection* This,
            [In] LPSTR Name
        );

        public /* static */ delegate HRESULT GetResourceBindingDesc(
            [In] ID3D12ShaderReflection* This,
            [In] uint ResourceIndex,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        public /* static */ delegate HRESULT GetInputParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        public /* static */ delegate HRESULT GetOutputParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        public /* static */ delegate HRESULT GetPatchConstantParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionVariable* GetVariableByName(
            [In] ID3D12ShaderReflection* This,
            [In] LPSTR Name
        );

        public /* static */ delegate HRESULT GetResourceBindingDescByName(
            [In] ID3D12ShaderReflection* This,
            [In] LPSTR Name,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        public /* static */ delegate uint GetMovInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate uint GetMovcInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate uint GetConversionInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate uint GetBitwiseInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate D3D_PRIMITIVE GetGSInputPrimitive(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate BOOL IsSampleFrequencyShader(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate uint GetNumInterfaceSlots(
            [In] ID3D12ShaderReflection* This
        );

        public /* static */ delegate HRESULT GetMinFeatureLevel(
            [In] ID3D12ShaderReflection* This,
            [Out] D3D_FEATURE_LEVEL* pLevel
        );

        public /* static */ delegate uint GetThreadGroupSize(
            [In] ID3D12ShaderReflection* This,
            [Out, Optional] uint* pSizeX,
            [Out, Optional] uint* pSizeY,
            [Out, Optional] uint* pSizeZ
        );

        public /* static */ delegate ulong GetRequiresFlags(
            [In] ID3D12ShaderReflection* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public GetConstantBufferByIndex GetConstantBufferByIndex;

            public GetConstantBufferByName GetConstantBufferByName;

            public GetResourceBindingDesc GetResourceBindingDesc;

            public GetInputParameterDesc GetInputParameterDesc;

            public GetOutputParameterDesc GetOutputParameterDesc;

            public GetPatchConstantParameterDesc GetPatchConstantParameterDesc;

            public GetVariableByName GetVariableByName;

            public GetResourceBindingDescByName GetResourceBindingDescByName;

            public GetMovInstructionCount GetMovInstructionCount;

            public GetMovcInstructionCount GetMovcInstructionCount;

            public GetConversionInstructionCount GetConversionInstructionCount;

            public GetBitwiseInstructionCount GetBitwiseInstructionCount;

            public GetGSInputPrimitive GetGSInputPrimitive;

            public IsSampleFrequencyShader IsSampleFrequencyShader;

            public GetNumInterfaceSlots GetNumInterfaceSlots;

            public GetMinFeatureLevel GetMinFeatureLevel;

            public GetThreadGroupSize GetThreadGroupSize;

            public GetRequiresFlags GetRequiresFlags;
            #endregion
        }
        #endregion
    }
}
