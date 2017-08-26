// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct D3D12_FUNCTION_DESC
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint Version;

        [ComAliasName("LPCSTR")]
        public /* readonly */ sbyte* Creator;

        [ComAliasName("UINT")]
        public uint Flags;

        [ComAliasName("UINT")]
        public uint ConstantBuffers;

        [ComAliasName("UINT")]
        public uint BoundResources;

        [ComAliasName("UINT")]
        public uint InstructionCount;

        [ComAliasName("UINT")]
        public uint TempRegisterCount;

        [ComAliasName("UINT")]
        public uint TempArrayCount;

        [ComAliasName("UINT")]
        public uint DefCount;

        [ComAliasName("UINT")]
        public uint DclCount;

        [ComAliasName("UINT")]
        public uint TextureNormalInstructions;

        [ComAliasName("UINT")]
        public uint TextureLoadInstructions;

        [ComAliasName("UINT")]
        public uint TextureCompInstructions;

        [ComAliasName("UINT")]
        public uint TextureBiasInstructions;

        [ComAliasName("UINT")]
        public uint TextureGradientInstructions;

        [ComAliasName("UINT")]
        public uint FloatInstructionCount;

        [ComAliasName("UINT")]
        public uint IntInstructionCount;

        [ComAliasName("UINT")]
        public uint UintInstructionCount;

        [ComAliasName("UINT")]
        public uint StaticFlowControlCount;

        [ComAliasName("UINT")]
        public uint DynamicFlowControlCount;

        [ComAliasName("UINT")]
        public uint MacroInstructionCount;

        [ComAliasName("UINT")]
        public uint ArrayInstructionCount;

        [ComAliasName("UINT")]
        public uint MovInstructionCount;

        [ComAliasName("UINT")]
        public uint MovcInstructionCount;

        [ComAliasName("UINT")]
        public uint ConversionInstructionCount;

        [ComAliasName("UINT")]
        public uint BitwiseInstructionCount;

        public D3D_FEATURE_LEVEL MinFeatureLevel;

        [ComAliasName("UINT64")]
        public ulong RequiredFeatureFlags;

        [ComAliasName("LPCSTR")]
        public /* readonly */ sbyte* Name;

        [ComAliasName("INT")]
        public int FunctionParameterCount;

        [ComAliasName("BOOL")]
        public int HasReturn;

        [ComAliasName("BOOL")]
        public int Has10Level9VertexShader;

        [ComAliasName("BOOL")]
        public int Has10Level9PixelShader;
        #endregion
    }
}
