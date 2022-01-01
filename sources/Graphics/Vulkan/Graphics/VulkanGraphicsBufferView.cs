// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsBufferView : GraphicsBufferView
{
    internal VulkanGraphicsBufferView(VulkanGraphicsBuffer buffer, in GraphicsBufferViewCreateOptions createOptions, in GraphicsMemoryRegion memoryRegion) : base(buffer)
    {
        buffer.AddBufferView(this);

        ResourceViewInfo.ByteOffset = memoryRegion.ByteOffset;
        ResourceViewInfo.ByteLength = memoryRegion.ByteLength;
        ResourceViewInfo.BytesPerElement = createOptions.BytesPerElement;

        BufferViewInfo.Kind = buffer.Kind;
        BufferViewInfo.MemoryRegion = memoryRegion;
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBufferView" /> class.</summary>
    ~VulkanGraphicsBufferView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new VulkanGraphicsBuffer Resource => base.Resource.As<VulkanGraphicsBuffer>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        BufferViewInfo.MemoryRegion.Dispose();

        _ = Resource.RemoveBufferView(this);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapUnsafe()
    {
        return Resource.MapView(ByteOffset);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapForReadUnsafe()
    {
        return Resource.MapViewForRead(ByteOffset, ByteLength);
    }

    /// <inheritdoc />
    protected override unsafe byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        return Resource.MapViewForRead(ByteOffset + byteStart, byteLength);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        Resource.UnmapView();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        Resource.UnmapViewAndWrite(ByteOffset, ByteLength);
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        Resource.UnmapViewAndWrite(ByteOffset + byteStart, byteLength);
    }
}
