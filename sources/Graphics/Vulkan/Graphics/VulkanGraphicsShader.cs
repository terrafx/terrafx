// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsShader : GraphicsShader
{
    private VkShaderModule _vkShaderModule;

    internal VulkanGraphicsShader(VulkanGraphicsDevice device, in GraphicsShaderCreateOptions createOptions) : base(device)
    {
        device.AddShader(this);

        if (createOptions.TakeBytecodeOwnership)
        {
            ShaderInfo.Bytecode = createOptions.Bytecode;
        }
        else
        {
            ShaderInfo.Bytecode = new UnmanagedArray<byte>(createOptions.Bytecode.Length, createOptions.Bytecode.Alignment);
            createOptions.Bytecode.CopyTo(ShaderInfo.Bytecode);
        }

        ShaderInfo.EntryPointName = createOptions.EntryPointName;
        ShaderInfo.Kind = createOptions.ShaderKind;

        _vkShaderModule = CreateVkShaderModule();

        SetNameUnsafe(Name);

        VkShaderModule CreateVkShaderModule()
        {
            VkShaderModule vkShaderModule;

            var bytecode = ShaderInfo.Bytecode;

            var vkShaderModuleCreateInfo = new VkShaderModuleCreateInfo {
                sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                pNext = null,
                flags = 0,
                codeSize = bytecode.Length,
                pCode = (uint*)bytecode.GetPointerUnsafe(0),
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateShaderModule(device.VkDevice, &vkShaderModuleCreateInfo, pAllocator: null, &vkShaderModule));

            return vkShaderModule;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsShader" /> class.</summary>
    ~VulkanGraphicsShader() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkShaderModule" /> for the shader.</summary>
    public VkShaderModule VkShaderModule => _vkShaderModule;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        DisposeVkShaderModule(Device.VkDevice, _vkShaderModule);
        _vkShaderModule = VkShaderModule.NULL;

        ShaderInfo.Bytecode.Dispose();

        _ = Device.RemoveShader(this);

        static void DisposeVkShaderModule(VkDevice vkDevice, VkShaderModule vkShaderModule)
        {
            if (vkShaderModule != VkShaderModule.NULL)
            {
                vkDestroyShaderModule(vkDevice, vkShaderModule, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_SHADER_MODULE, VkShaderModule, value);
    }
}
