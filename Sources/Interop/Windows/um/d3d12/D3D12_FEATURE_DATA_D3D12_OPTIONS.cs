// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_FEATURE_DATA_D3D12_OPTIONS
    {
        #region Fields
        [ComAliasName("BOOL")]
        public int DoublePrecisionFloatShaderOps;

        [ComAliasName("BOOL")]
        public int OutputMergerLogicOp;

        public D3D12_SHADER_MIN_PRECISION_SUPPORT MinPrecisionSupport;

        public D3D12_TILED_RESOURCES_TIER TiledResourcesTier;

        public D3D12_RESOURCE_BINDING_TIER ResourceBindingTier;

        [ComAliasName("BOOL")]
        public int PSSpecifiedStencilRefSupported;

        [ComAliasName("BOOL")]
        public int TypedUAVLoadAdditionalFormats;

        [ComAliasName("BOOL")]
        public int ROVsSupported;

        public D3D12_CONSERVATIVE_RASTERIZATION_TIER ConservativeRasterizationTier;

        [ComAliasName("UINT")]
        public uint MaxGPUVirtualAddressBitsPerResource;

        [ComAliasName("BOOL")]
        public int StandardSwizzle64KBSupported;

        public D3D12_CROSS_NODE_SHARING_TIER CrossNodeSharingTier;

        [ComAliasName("BOOL")]
        public int CrossAdapterRowMajorTextureSupported;

        [ComAliasName("BOOL")]
        public int VPAndRTArrayIndexFromAnyShaderFeedingRasterizerSupportedWithoutGSEmulation;

        public D3D12_RESOURCE_HEAP_TIER ResourceHeapTier;
        #endregion
    }
}
