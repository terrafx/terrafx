// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsBufferView : GraphicsBufferView
{
    private ulong _gpuVirtualAddress;

    internal D3D12GraphicsBufferView(D3D12GraphicsBuffer buffer, in GraphicsBufferViewCreateOptions createOptions, in GraphicsMemoryRegion memoryRegion) : base(buffer)
    {
        buffer.AddBufferView(this);

        ResourceViewInfo.ByteOffset = memoryRegion.ByteOffset;
        ResourceViewInfo.ByteLength = memoryRegion.ByteLength;
        ResourceViewInfo.BytesPerElement = createOptions.BytesPerElement;

        BufferViewInfo.Kind = buffer.Kind;
        BufferViewInfo.MemoryRegion = memoryRegion;

        _gpuVirtualAddress = buffer.GpuVirtualAddress + memoryRegion.ByteOffset;
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBufferView" /> class.</summary>
    ~D3D12GraphicsBufferView() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets the GPU virtual address for the buffer view.</summary>
    public ulong GpuVirtualAddress => _gpuVirtualAddress;

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new D3D12GraphicsBuffer Resource => base.Resource.As<D3D12GraphicsBuffer>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _gpuVirtualAddress = 0;

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
