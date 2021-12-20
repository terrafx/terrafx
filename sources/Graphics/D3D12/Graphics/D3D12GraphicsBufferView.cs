// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsBufferView : GraphicsBufferView
{
    private readonly ulong _d3d12ResourceGpuVirtualAddress;

    internal D3D12GraphicsBufferView(D3D12GraphicsBuffer buffer, in GraphicsMemoryRegion memoryRegion, uint stride)
        : base(buffer, in memoryRegion, stride)
    {
        buffer.AddView(this);
        _d3d12ResourceGpuVirtualAddress = buffer.D3D12ResourceGpuVirtualAddress + memoryRegion.Offset;
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBufferView" /> class.</summary>
    ~D3D12GraphicsBufferView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the GPU virtual address for the buffer view.</summary>
    public ulong D3D12ResourceGpuVirtualAddress => _d3d12ResourceGpuVirtualAddress;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new D3D12GraphicsBuffer Resource => base.Resource.As<D3D12GraphicsBuffer>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _ = Resource.RemoveView(this);
        }
        MemoryRegion.Dispose();
    }
}
