// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.D3D;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    public struct D3D12_FUNCTION_DESC
    {
        #region Fields
        public uint Version;

        public LPSTR Creator;

        public uint Flags;

        public uint ConstantBuffers;

        public uint BoundResources;

        public uint InstructionCount;

        public uint TempRegisterCount;

        public uint TempArrayCount;

        public uint DefCount;

        public uint DclCount;

        public uint TextureNormalInstructions;

        public uint TextureLoadInstructions;

        public uint TextureCompInstructions;

        public uint TextureBiasInstructions;

        public uint TextureGradientInstructions;

        public uint FloatInstructionCount;

        public uint IntInstructionCount;

        public uint UintInstructionCount;

        public uint StaticFlowControlCount;

        public uint DynamicFlowControlCount;

        public uint MacroInstructionCount;

        public uint ArrayInstructionCount;

        public uint MovInstructionCount;

        public uint MovcInstructionCount;

        public uint ConversionInstructionCount;

        public uint BitwiseInstructionCount;

        public D3D_FEATURE_LEVEL MinFeatureLevel;

        public ulong RequiredFeatureFlags;

        public LPSTR Name;

        public int FunctionParameterCount;

        public BOOL HasReturn;

        public BOOL Has10Level9VertexShader;

        public BOOL Has10Level9PixelShader;
        #endregion
    }
}
