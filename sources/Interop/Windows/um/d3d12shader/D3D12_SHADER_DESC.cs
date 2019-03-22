// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct D3D12_SHADER_DESC
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint Version;

        [ComAliasName("LPCSTR")]
        public sbyte* Creator;

        [ComAliasName("UINT")]
        public uint Flags;

        [ComAliasName("UINT")]
        public uint ConstantBuffers;

        [ComAliasName("UINT")]
        public uint BoundResources;

        [ComAliasName("UINT")]
        public uint InputParameters;

        [ComAliasName("UINT")]
        public uint OutputParameters;

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
        public uint CutInstructionCount;

        [ComAliasName("UINT")]
        public uint EmitInstructionCount;

        public D3D_PRIMITIVE_TOPOLOGY GSOutputTopology;

        [ComAliasName("UINT")]
        public uint GSMaxOutputVertexCount;

        public D3D_PRIMITIVE InputPrimitive;

        [ComAliasName("UINT")]
        public uint PatchConstantParameters;

        [ComAliasName("UINT")]
        public uint cGSInstanceCount;

        [ComAliasName("UINT")]
        public uint cControlPoints;

        public D3D_TESSELLATOR_OUTPUT_PRIMITIVE HSOutputPrimitive;

        public D3D_TESSELLATOR_PARTITIONING HSPartitioning;

        public D3D_TESSELLATOR_DOMAIN TessellatorDomain;

        [ComAliasName("UINT")]
        public uint cBarrierInstructions;

        [ComAliasName("UINT")]
        public uint cInterlockedInstructions;

        [ComAliasName("UINT")]
        public uint cTextureStoreInstructions;
        #endregion
    }
}
