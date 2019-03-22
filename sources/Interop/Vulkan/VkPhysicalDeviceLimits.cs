// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkPhysicalDeviceLimits
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

        [NativeTypeName("VkDeviceSize")]
        public ulong bufferImageGranularity;

        [NativeTypeName("VkDeviceSize")]
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

        [NativeTypeName("VkDeviceSize")]
        public ulong minTexelBufferOffsetAlignment;

        [NativeTypeName("VkDeviceSize")]
        public ulong minUniformBufferOffsetAlignment;

        [NativeTypeName("VkDeviceSize")]
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

        [NativeTypeName("VkSampleCountFlags")]
        public uint framebufferColorSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint framebufferDepthSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint framebufferStencilSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint framebufferNoAttachmentsSampleCounts;

        public uint maxColorAttachments;

        [NativeTypeName("VkSampleCountFlags")]
        public uint sampledImageColorSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint sampledImageIntegerSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint sampledImageDepthSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint sampledImageStencilSampleCounts;

        [NativeTypeName("VkSampleCountFlags")]
        public uint storageImageSampleCounts;

        public uint maxSampleMaskWords;

        [NativeTypeName("VkBool32")]
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

        [NativeTypeName("VkBool32")]
        public uint strictLines;

        [NativeTypeName("VkBool32")]
        public uint standardSampleLocations;

        [NativeTypeName("VkDeviceSize")]
        public ulong optimalBufferCopyOffsetAlignment;

        [NativeTypeName("VkDeviceSize")]
        public ulong optimalBufferCopyRowPitchAlignment;

        [NativeTypeName("VkDeviceSize")]
        public ulong nonCoherentAtomSize;
        #endregion
    }
}
