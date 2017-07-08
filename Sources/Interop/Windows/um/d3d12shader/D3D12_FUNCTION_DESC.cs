// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_FUNCTION_DESC
    {
        #region Fields
        public UINT Version;

        public LPCSTR Creator;

        public UINT Flags;

        public UINT ConstantBuffers;

        public UINT BoundResources;

        public UINT InstructionCount;

        public UINT TempRegisterCount;

        public UINT TempArrayCount;

        public UINT DefCount;

        public UINT DclCount;

        public UINT TextureNormalInstructions;

        public UINT TextureLoadInstructions;

        public UINT TextureCompInstructions;

        public UINT TextureBiasInstructions;

        public UINT TextureGradientInstructions;

        public UINT FloatInstructionCount;

        public UINT IntInstructionCount;

        public UINT UintInstructionCount;

        public UINT StaticFlowControlCount;

        public UINT DynamicFlowControlCount;

        public UINT MacroInstructionCount;

        public UINT ArrayInstructionCount;

        public UINT MovInstructionCount;

        public UINT MovcInstructionCount;

        public UINT ConversionInstructionCount;

        public UINT BitwiseInstructionCount;

        public D3D_FEATURE_LEVEL MinFeatureLevel;

        public UINT64 RequiredFeatureFlags;

        public LPCSTR Name;

        public INT FunctionParameterCount;

        public BOOL HasReturn;

        public BOOL Has10Level9VertexShader;

        public BOOL Has10Level9PixelShader;
        #endregion
    }
}
