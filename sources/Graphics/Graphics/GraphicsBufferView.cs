// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics buffer view.</summary>
public sealed unsafe class GraphicsBufferView : GraphicsResourceView
{
    private ulong _d3d12GpuVirtualAddress;

    private readonly GraphicsBufferKind _kind;

    private readonly GraphicsMemoryRegion _memoryRegion;

    internal GraphicsBufferView(GraphicsBuffer buffer, in GraphicsBufferViewCreateOptions createOptions, in GraphicsMemoryRegion memoryRegion) : base(buffer, in createOptions, in memoryRegion)
    {
        buffer.AddBufferView(this);

        _d3d12GpuVirtualAddress = buffer.D3D12GpuVirtualAddress + memoryRegion.ByteOffset;

        _kind = buffer.Kind;

        _memoryRegion = memoryRegion;
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsBufferView" /> class.</summary>
    ~GraphicsBufferView() => Dispose(isDisposing: false);

    /// <summary>Gets the buffer view kind.</summary>
    public new GraphicsBufferKind Kind => _kind;

    /// <summary>Gets the memory region in which the buffer view exists.</summary>
    public ref readonly GraphicsMemoryRegion MemoryRegion => ref _memoryRegion;

    /// <inheritdoc cref="GraphicsResourceObject.Resource" />
    public new GraphicsBuffer Resource => base.Resource.As<GraphicsBuffer>();

    internal ulong D3D12GpuVirtualAddress => _d3d12GpuVirtualAddress;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _d3d12GpuVirtualAddress = 0;

        _memoryRegion.Dispose();

        _ = Resource.RemoveBufferView(this);
    }

    private protected override unsafe byte* MapForReadUnsafe()
    {
        return Resource.MapForReadUnsafe(subresource: 0, ByteOffset, ByteLength);
    }

    private protected override unsafe byte* MapForReadUnsafe(nuint byteStart, nuint byteLength)
    {
        return Resource.MapForReadUnsafe(subresource: 0, ByteOffset + byteStart, byteLength);
    }

    private protected override unsafe byte* MapUnsafe()
    {
        return Resource.MapUnsafe(subresource: 0) + ByteOffset;
    }

    private protected override void UnmapAndWriteUnsafe()
    {
        Resource.UnmapAndWriteUnsafe(subresource: 0, ByteOffset, ByteLength);
    }

    private protected override void UnmapAndWriteUnsafe(nuint byteStart, nuint byteLength)
    {
        Resource.UnmapAndWriteUnsafe(subresource: 0, ByteOffset + byteStart, byteLength);
    }

    private protected override void UnmapUnsafe()
    {
        Resource.UnmapUnsafe(subresource: 0);
    }
}
