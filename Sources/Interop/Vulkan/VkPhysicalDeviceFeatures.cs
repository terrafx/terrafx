// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct VkPhysicalDeviceFeatures
    {
        #region Fields
        [ComAliasName("VkBool32")]
        public uint robustBufferAccess;

        [ComAliasName("VkBool32")]
        public uint fullDrawIndexUint32;

        [ComAliasName("VkBool32")]
        public uint imageCubeArray;

        [ComAliasName("VkBool32")]
        public uint independentBlend;

        [ComAliasName("VkBool32")]
        public uint geometryShader;

        [ComAliasName("VkBool32")]
        public uint tessellationShader;

        [ComAliasName("VkBool32")]
        public uint sampleRateShading;

        [ComAliasName("VkBool32")]
        public uint dualSrcBlend;

        [ComAliasName("VkBool32")]
        public uint logicOp;

        [ComAliasName("VkBool32")]
        public uint multiDrawIndirect;

        [ComAliasName("VkBool32")]
        public uint drawIndirectFirstInstance;

        [ComAliasName("VkBool32")]
        public uint depthClamp;

        [ComAliasName("VkBool32")]
        public uint depthBiasClamp;

        [ComAliasName("VkBool32")]
        public uint fillModeNonSolid;

        [ComAliasName("VkBool32")]
        public uint depthBounds;

        [ComAliasName("VkBool32")]
        public uint wideLines;

        [ComAliasName("VkBool32")]
        public uint largePoints;

        [ComAliasName("VkBool32")]
        public uint alphaToOne;

        [ComAliasName("VkBool32")]
        public uint multiViewport;

        [ComAliasName("VkBool32")]
        public uint samplerAnisotropy;

        [ComAliasName("VkBool32")]
        public uint textureCompressionETC2;

        [ComAliasName("VkBool32")]
        public uint textureCompressionASTC_LDR;

        [ComAliasName("VkBool32")]
        public uint textureCompressionBC;

        [ComAliasName("VkBool32")]
        public uint occlusionQueryPrecise;

        [ComAliasName("VkBool32")]
        public uint pipelineStatisticsQuery;

        [ComAliasName("VkBool32")]
        public uint vertexPipelineStoresAndAtomics;

        [ComAliasName("VkBool32")]
        public uint fragmentStoresAndAtomics;

        [ComAliasName("VkBool32")]
        public uint shaderTessellationAndGeometryPointSize;

        [ComAliasName("VkBool32")]
        public uint shaderImageGatherExtended;

        [ComAliasName("VkBool32")]
        public uint shaderStorageImageExtendedFormats;

        [ComAliasName("VkBool32")]
        public uint shaderStorageImageMultisample;

        [ComAliasName("VkBool32")]
        public uint shaderStorageImageReadWithoutFormat;

        [ComAliasName("VkBool32")]
        public uint shaderStorageImageWriteWithoutFormat;

        [ComAliasName("VkBool32")]
        public uint shaderUniformBufferArrayDynamicIndexing;

        [ComAliasName("VkBool32")]
        public uint shaderSampledImageArrayDynamicIndexing;

        [ComAliasName("VkBool32")]
        public uint shaderStorageBufferArrayDynamicIndexing;

        [ComAliasName("VkBool32")]
        public uint shaderStorageImageArrayDynamicIndexing;

        [ComAliasName("VkBool32")]
        public uint shaderClipDistance;

        [ComAliasName("VkBool32")]
        public uint shaderCullDistance;

        [ComAliasName("VkBool32")]
        public uint shaderFloat64;

        [ComAliasName("VkBool32")]
        public uint shaderInt64;

        [ComAliasName("VkBool32")]
        public uint shaderInt16;

        [ComAliasName("VkBool32")]
        public uint shaderResourceResidency;

        [ComAliasName("VkBool32")]
        public uint shaderResourceMinLod;

        [ComAliasName("VkBool32")]
        public uint sparseBinding;

        [ComAliasName("VkBool32")]
        public uint sparseResidencyBuffer;

        [ComAliasName("VkBool32")]
        public uint sparseResidencyImage2D;

        [ComAliasName("VkBool32")]
        public uint sparseResidencyImage3D;

        [ComAliasName("VkBool32")]
        public uint sparseResidency2Samples;

        [ComAliasName("VkBool32")]
        public uint sparseResidency4Samples;

        [ComAliasName("VkBool32")]
        public uint sparseResidency8Samples;

        [ComAliasName("VkBool32")]
        public uint sparseResidency16Samples;

        [ComAliasName("VkBool32")]
        public uint sparseResidencyAliased;

        [ComAliasName("VkBool32")]
        public uint variableMultisampleRate;

        [ComAliasName("VkBool32")]
        public uint inheritedQueries;
        #endregion
    }
}
