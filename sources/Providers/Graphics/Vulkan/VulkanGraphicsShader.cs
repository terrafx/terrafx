// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.IntegerUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsShader : GraphicsShader
    {
        private readonly VkShaderModuleCreateInfo _vulkanShaderModuleCreateInfo;

        private ValueLazy<VkShaderModule> _vulkanShaderModule;

        private State _state;

        internal VulkanGraphicsShader(VulkanGraphicsDevice device, GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
            : base(device, kind, entryPointName)
        {
            var bytecodeLength = (nuint)bytecode.Length;

            _vulkanShaderModuleCreateInfo.sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO;
            _vulkanShaderModuleCreateInfo.codeSize = bytecodeLength;
            _vulkanShaderModuleCreateInfo.pCode = AllocateArray<uint>(AlignUp(bytecodeLength, SizeOf<nuint>()));

            var destination = new Span<byte>(_vulkanShaderModuleCreateInfo.pCode, (int)bytecodeLength);
            bytecode.CopyTo(destination);

            _vulkanShaderModule = new ValueLazy<VkShaderModule>(CreateVulkanShaderModule);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsShader" /> class.</summary>
        ~VulkanGraphicsShader() => Dispose(isDisposing: true);

        /// <inheritdoc />
        public override ReadOnlySpan<byte> Bytecode => new ReadOnlySpan<byte>(_vulkanShaderModuleCreateInfo.pCode, (int)_vulkanShaderModuleCreateInfo.codeSize);

        /// <inheritdoc cref="GraphicsShader.Device" />
        public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

        /// <summary>Gets the underlying <see cref="VkShaderModule" /> for the shader.</summary>
        public VkShaderModule VulkanShaderModule => _vulkanShaderModule.Value;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanShaderModule.Dispose(DisposeVulkanShaderModule);
                DisposeVulkanShaderModuleCreateInfo();
            }

            _state.EndDispose();
        }

        private VkShaderModule CreateVulkanShaderModule()
        {
            VkShaderModule vulkanShaderModule;

            fixed (VkShaderModuleCreateInfo* shaderModuleCreateInfo = &_vulkanShaderModuleCreateInfo)
            {
                ThrowExternalExceptionIfNotSuccess(vkCreateShaderModule(Device.VulkanDevice, shaderModuleCreateInfo, pAllocator: null, (ulong*)&vulkanShaderModule), nameof(vkCreateShaderModule));
            }

            return vulkanShaderModule;
        }

        private void DisposeVulkanShaderModule(VkShaderModule vulkanShaderModule)
        {
            if (vulkanShaderModule != VK_NULL_HANDLE)
            {
                vkDestroyShaderModule(Device.VulkanDevice, vulkanShaderModule, pAllocator: null);
            }
        }

        private void DisposeVulkanShaderModuleCreateInfo()
        {
            var code = _vulkanShaderModuleCreateInfo.pCode;
            Free(code);
        }
    }
}
