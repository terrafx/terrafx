// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public struct VkPhysicalDeviceFeatures
    {
        #region Fields
        public VkBool32 robustBufferAccess;

        public VkBool32 fullDrawIndexUint32;

        public VkBool32 imageCubeArray;

        public VkBool32 independentBlend;

        public VkBool32 geometryShader;

        public VkBool32 tessellationShader;

        public VkBool32 sampleRateShading;

        public VkBool32 dualSrcBlend;

        public VkBool32 logicOp;

        public VkBool32 multiDrawIndirect;

        public VkBool32 drawIndirectFirstInstance;

        public VkBool32 depthClamp;

        public VkBool32 depthBiasClamp;

        public VkBool32 fillModeNonSolid;

        public VkBool32 depthBounds;

        public VkBool32 wideLines;

        public VkBool32 largePoints;

        public VkBool32 alphaToOne;

        public VkBool32 multiViewport;

        public VkBool32 samplerAnisotropy;

        public VkBool32 textureCompressionETC2;

        public VkBool32 textureCompressionASTC_LDR;

        public VkBool32 textureCompressionBC;

        public VkBool32 occlusionQueryPrecise;

        public VkBool32 pipelineStatisticsQuery;

        public VkBool32 vertexPipelineStoresAndAtomics;

        public VkBool32 fragmentStoresAndAtomics;

        public VkBool32 shaderTessellationAndGeometryPointSize;

        public VkBool32 shaderImageGatherExtended;

        public VkBool32 shaderStorageImageExtendedFormats;

        public VkBool32 shaderStorageImageMultisample;

        public VkBool32 shaderStorageImageReadWithoutFormat;

        public VkBool32 shaderStorageImageWriteWithoutFormat;

        public VkBool32 shaderUniformBufferArrayDynamicIndexing;

        public VkBool32 shaderSampledImageArrayDynamicIndexing;

        public VkBool32 shaderStorageBufferArrayDynamicIndexing;

        public VkBool32 shaderStorageImageArrayDynamicIndexing;

        public VkBool32 shaderClipDistance;

        public VkBool32 shaderCullDistance;

        public VkBool32 shaderFloat64;

        public VkBool32 shaderInt64;

        public VkBool32 shaderInt16;

        public VkBool32 shaderResourceResidency;

        public VkBool32 shaderResourceMinLod;

        public VkBool32 sparseBinding;

        public VkBool32 sparseResidencyBuffer;

        public VkBool32 sparseResidencyImage2D;

        public VkBool32 sparseResidencyImage3D;

        public VkBool32 sparseResidency2Samples;

        public VkBool32 sparseResidency4Samples;

        public VkBool32 sparseResidency8Samples;

        public VkBool32 sparseResidency16Samples;

        public VkBool32 sparseResidencyAliased;

        public VkBool32 variableMultisampleRate;

        public VkBool32 inheritedQueries;
        #endregion
    }
}
