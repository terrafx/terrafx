// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;
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
    private readonly UnmanagedArray<byte> _bytecode;
    private readonly VkShaderModule _vkShaderModule;

    internal VulkanGraphicsShader(VulkanGraphicsDevice device, GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
        : base(device, kind, entryPointName)
    {
        _bytecode = GetBytecode(bytecode);
        _vkShaderModule = CreateVkShaderModule(device, _bytecode);

        static VkShaderModule CreateVkShaderModule(VulkanGraphicsDevice device, UnmanagedReadOnlySpan<byte> bytecode)
        {
            VkShaderModule vkShaderModule;

            var vkShaderModuleCreateInfo = new VkShaderModuleCreateInfo {
                sType = VK_STRUCTURE_TYPE_SHADER_MODULE_CREATE_INFO,
                codeSize = bytecode.Length,
                pCode = (uint*)bytecode.GetPointerUnsafe(0),
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateShaderModule(device.VkDevice, &vkShaderModuleCreateInfo, pAllocator: null, &vkShaderModule));

            return vkShaderModule;
        }

        static UnmanagedArray<byte> GetBytecode(ReadOnlySpan<byte> source)
        {
            var bytecode = new UnmanagedArray<byte>((uint)source.Length, zero: false);

            var destination = new Span<byte>(bytecode.GetPointerUnsafe(0), source.Length);
            source.CopyTo(destination);

            return bytecode;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsShader" /> class.</summary>
    ~VulkanGraphicsShader() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc />
    public override UnmanagedReadOnlySpan<byte> Bytecode => _bytecode;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkShaderModule" /> for the shader.</summary>
    public VkShaderModule VkShaderModule
    {
        get
        {
            AssertNotDisposed();
            return _vkShaderModule;
        }
    }

    /// <inheritdoc />
    public override void SetName(string value)
    {
        value = Device.UpdateName(VK_OBJECT_TYPE_SHADER_MODULE, VkShaderModule, value);
        base.SetName(value);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        DisposeVkShaderModule(Device.VkDevice, _vkShaderModule);
        _bytecode.Dispose();

        static void DisposeVkShaderModule(VkDevice vkDevice, VkShaderModule vkShaderModule)
        {
            if (vkShaderModule != VkShaderModule.NULL)
            {
                vkDestroyShaderModule(vkDevice, vkShaderModule, pAllocator: null);
            }
        }
    }
}
