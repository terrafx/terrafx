// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct VkPhysicalDeviceLimits
    {
        #region Fields
        public uint maxImageDimension1D;

        public uint maxImageDimension2D;

        public uint maxImageDimension3D;

        public uint maxImageDimensionCube;

        public uint maxImageArrayLayers;

        public uint maxTexelBufferElements;

        public uint maxUniformBufferRange;

        public uint maxStorageBufferRange;

        public uint maxPushConstantsSize;

        public uint maxMemoryAllocationCount;

        public uint maxSamplerAllocationCount;

        [ComAliasName("VkDeviceSize")]
        public ulong bufferImageGranularity;

        [ComAliasName("VkDeviceSize")]
        public ulong sparseAddressSpaceSize;

        public uint maxBoundDescriptorSets;

        public uint maxPerStageDescriptorSamplers;

        public uint maxPerStageDescriptorUniformBuffers;

        public uint maxPerStageDescriptorStorageBuffers;

        public uint maxPerStageDescriptorSampledImages;

        public uint maxPerStageDescriptorStorageImages;

        public uint maxPerStageDescriptorInputAttachments;

        public uint maxPerStageResources;

        public uint maxDescriptorSetSamplers;

        public uint maxDescriptorSetUniformBuffers;

        public uint maxDescriptorSetUniformBuffersDynamic;

        public uint maxDescriptorSetStorageBuffers;

        public uint maxDescriptorSetStorageBuffersDynamic;

        public uint maxDescriptorSetSampledImages;

        public uint maxDescriptorSetStorageImages;

        public uint maxDescriptorSetInputAttachments;

        public uint maxVertexInputAttributes;

        public uint maxVertexInputBindings;

        public uint maxVertexInputAttributeOffset;

        public uint maxVertexInputBindingStride;

        public uint maxVertexOutputComponents;

        public uint maxTessellationGenerationLevel;

        public uint maxTessellationPatchSize;

        public uint maxTessellationControlPerVertexInputComponents;

        public uint maxTessellationControlPerVertexOutputComponents;

        public uint maxTessellationControlPerPatchOutputComponents;

        public uint maxTessellationControlTotalOutputComponents;

        public uint maxTessellationEvaluationInputComponents;

        public uint maxTessellationEvaluationOutputComponents;

        public uint maxGeometryShaderInvocations;

        public uint maxGeometryInputComponents;

        public uint maxGeometryOutputComponents;

        public uint maxGeometryOutputVertices;

        public uint maxGeometryTotalOutputComponents;

        public uint maxFragmentInputComponents;

        public uint maxFragmentOutputAttachments;

        public uint maxFragmentDualSrcAttachments;

        public uint maxFragmentCombinedOutputResources;

        public uint maxComputeSharedMemorySize;

        public fixed uint maxComputeWorkGroupCount[3];

        public uint maxComputeWorkGroupInvocations;

        public fixed uint maxComputeWorkGroupSize[3];

        public uint subPixelPrecisionBits;

        public uint subTexelPrecisionBits;

        public uint mipmapPrecisionBits;

        public uint maxDrawIndexedIndexValue;

        public uint maxDrawIndirectCount;

        public float maxSamplerLodBias;

        public float maxSamplerAnisotropy;

        public uint maxViewports;

        public fixed uint maxViewportDimensions[2];

        public fixed float viewportBoundsRange[2];

        public uint viewportSubPixelBits;

        public nuint minMemoryMapAlignment;

        [ComAliasName("VkDeviceSize")]
        public ulong minTexelBufferOffsetAlignment;

        [ComAliasName("VkDeviceSize")]
        public ulong minUniformBufferOffsetAlignment;

        [ComAliasName("VkDeviceSize")]
        public ulong minStorageBufferOffsetAlignment;

        public int minTexelOffset;

        public uint maxTexelOffset;

        public int minTexelGatherOffset;

        public uint maxTexelGatherOffset;

        public float minInterpolationOffset;

        public float maxInterpolationOffset;

        public uint subPixelInterpolationOffsetBits;

        public uint maxFramebufferWidth;

        public uint maxFramebufferHeight;

        public uint maxFramebufferLayers;

        [ComAliasName("VkSampleCountFlags")]
        public uint framebufferColorSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint framebufferDepthSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint framebufferStencilSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint framebufferNoAttachmentsSampleCounts;

        public uint maxColorAttachments;

        [ComAliasName("VkSampleCountFlags")]
        public uint sampledImageColorSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint sampledImageIntegerSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint sampledImageDepthSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint sampledImageStencilSampleCounts;

        [ComAliasName("VkSampleCountFlags")]
        public uint storageImageSampleCounts;

        public uint maxSampleMaskWords;

        [ComAliasName("VkBool32")]
        public uint timestampComputeAndGraphics;

        public float timestampPeriod;

        public uint maxClipDistances;

        public uint maxCullDistances;

        public uint maxCombinedClipAndCullDistances;

        public uint discreteQueuePriorities;

        public fixed float pointSizeRange[2];

        public fixed float lineWidthRange[2];

        public float pointSizeGranularity;

        public float lineWidthGranularity;

        [ComAliasName("VkBool32")]
        public uint strictLines;

        [ComAliasName("VkBool32")]
        public uint standardSampleLocations;

        [ComAliasName("VkDeviceSize")]
        public ulong optimalBufferCopyOffsetAlignment;

        [ComAliasName("VkDeviceSize")]
        public ulong optimalBufferCopyRowPitchAlignment;

        [ComAliasName("VkDeviceSize")]
        public ulong nonCoherentAtomSize;
        #endregion
    }
}
