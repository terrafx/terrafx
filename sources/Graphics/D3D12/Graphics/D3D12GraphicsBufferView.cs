// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsBufferView : GraphicsBufferView
{
    private readonly ulong _d3d12ResourceGpuVirtualAddress;
    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsBufferView(D3D12GraphicsBuffer buffer, in GraphicsMemoryRegion memoryRegion, uint stride)
        : base(buffer, in memoryRegion, stride)
    {
        buffer.AddView(this);
        _d3d12ResourceGpuVirtualAddress = buffer.D3D12ResourceGpuVirtualAddress + memoryRegion.Offset;

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsBufferView);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBufferView" /> class.</summary>
    ~D3D12GraphicsBufferView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsResourceView.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the GPU virtual address for the buffer view.</summary>
    public ulong D3D12ResourceGpuVirtualAddress => _d3d12ResourceGpuVirtualAddress;

    /// <inheritdoc cref="GraphicsResourceView.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets or sets the name for the buffer view.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? "";
        }
    }

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new D3D12GraphicsBuffer Resource => base.Resource.As<D3D12GraphicsBuffer>();

    /// <inheritdoc cref="GraphicsResourceView.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                _ = Resource.RemoveView(this);
            }
            MemoryRegion.Dispose();
        }

        _state.EndDispose();
    }
}
