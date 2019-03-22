// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct VkPhysicalDeviceFeatures
    {
        #region Fields
        [NativeTypeName("VkBool32")]
        public uint robustBufferAccess;

        [NativeTypeName("VkBool32")]
        public uint fullDrawIndexUint32;

        [NativeTypeName("VkBool32")]
        public uint imageCubeArray;

        [NativeTypeName("VkBool32")]
        public uint independentBlend;

        [NativeTypeName("VkBool32")]
        public uint geometryShader;

        [NativeTypeName("VkBool32")]
        public uint tessellationShader;

        [NativeTypeName("VkBool32")]
        public uint sampleRateShading;

        [NativeTypeName("VkBool32")]
        public uint dualSrcBlend;

        [NativeTypeName("VkBool32")]
        public uint logicOp;

        [NativeTypeName("VkBool32")]
        public uint multiDrawIndirect;

        [NativeTypeName("VkBool32")]
        public uint drawIndirectFirstInstance;

        [NativeTypeName("VkBool32")]
        public uint depthClamp;

        [NativeTypeName("VkBool32")]
        public uint depthBiasClamp;

        [NativeTypeName("VkBool32")]
        public uint fillModeNonSolid;

        [NativeTypeName("VkBool32")]
        public uint depthBounds;

        [NativeTypeName("VkBool32")]
        public uint wideLines;

        [NativeTypeName("VkBool32")]
        public uint largePoints;

        [NativeTypeName("VkBool32")]
        public uint alphaToOne;

        [NativeTypeName("VkBool32")]
        public uint multiViewport;

        [NativeTypeName("VkBool32")]
        public uint samplerAnisotropy;

        [NativeTypeName("VkBool32")]
        public uint textureCompressionETC2;

        [NativeTypeName("VkBool32")]
        public uint textureCompressionASTC_LDR;

        [NativeTypeName("VkBool32")]
        public uint textureCompressionBC;

        [NativeTypeName("VkBool32")]
        public uint occlusionQueryPrecise;

        [NativeTypeName("VkBool32")]
        public uint pipelineStatisticsQuery;

        [NativeTypeName("VkBool32")]
        public uint vertexPipelineStoresAndAtomics;

        [NativeTypeName("VkBool32")]
        public uint fragmentStoresAndAtomics;

        [NativeTypeName("VkBool32")]
        public uint shaderTessellationAndGeometryPointSize;

        [NativeTypeName("VkBool32")]
        public uint shaderImageGatherExtended;

        [NativeTypeName("VkBool32")]
        public uint shaderStorageImageExtendedFormats;

        [NativeTypeName("VkBool32")]
        public uint shaderStorageImageMultisample;

        [NativeTypeName("VkBool32")]
        public uint shaderStorageImageReadWithoutFormat;

        [NativeTypeName("VkBool32")]
        public uint shaderStorageImageWriteWithoutFormat;

        [NativeTypeName("VkBool32")]
        public uint shaderUniformBufferArrayDynamicIndexing;

        [NativeTypeName("VkBool32")]
        public uint shaderSampledImageArrayDynamicIndexing;

        [NativeTypeName("VkBool32")]
        public uint shaderStorageBufferArrayDynamicIndexing;

        [NativeTypeName("VkBool32")]
        public uint shaderStorageImageArrayDynamicIndexing;

        [NativeTypeName("VkBool32")]
        public uint shaderClipDistance;

        [NativeTypeName("VkBool32")]
        public uint shaderCullDistance;

        [NativeTypeName("VkBool32")]
        public uint shaderFloat64;

        [NativeTypeName("VkBool32")]
        public uint shaderInt64;

        [NativeTypeName("VkBool32")]
        public uint shaderInt16;

        [NativeTypeName("VkBool32")]
        public uint shaderResourceResidency;

        [NativeTypeName("VkBool32")]
        public uint shaderResourceMinLod;

        [NativeTypeName("VkBool32")]
        public uint sparseBinding;

        [NativeTypeName("VkBool32")]
        public uint sparseResidencyBuffer;

        [NativeTypeName("VkBool32")]
        public uint sparseResidencyImage2D;

        [NativeTypeName("VkBool32")]
        public uint sparseResidencyImage3D;

        [NativeTypeName("VkBool32")]
        public uint sparseResidency2Samples;

        [NativeTypeName("VkBool32")]
        public uint sparseResidency4Samples;

        [NativeTypeName("VkBool32")]
        public uint sparseResidency8Samples;

        [NativeTypeName("VkBool32")]
        public uint sparseResidency16Samples;

        [NativeTypeName("VkBool32")]
        public uint sparseResidencyAliased;

        [NativeTypeName("VkBool32")]
        public uint variableMultisampleRate;

        [NativeTypeName("VkBool32")]
        public uint inheritedQueries;
        #endregion
    }
}
