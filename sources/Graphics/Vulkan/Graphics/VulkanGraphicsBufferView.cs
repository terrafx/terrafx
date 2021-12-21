// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsBufferView : GraphicsBufferView
{
    internal VulkanGraphicsBufferView(VulkanGraphicsBuffer buffer, in GraphicsMemoryRegion memoryRegion, uint stride)
        : base(buffer, in memoryRegion, stride)
    {
        buffer.AddView(this);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBufferView" /> class.</summary>
    ~VulkanGraphicsBufferView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new VulkanGraphicsBuffer Resource => base.Resource.As<VulkanGraphicsBuffer>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _ = Resource.RemoveView(this);
        }
        MemoryRegion.Dispose();
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
    }
}
