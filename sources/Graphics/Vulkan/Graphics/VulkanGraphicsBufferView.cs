// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsBufferView : GraphicsBufferView
{
    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsBufferView(VulkanGraphicsBuffer buffer, in GraphicsMemoryRegion memoryRegion, uint stride)
        : base(buffer, in memoryRegion, stride)
    {
        buffer.AddView(this);
        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsBufferView);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBufferView" /> class.</summary>
    ~VulkanGraphicsBufferView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsResourceView.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsResourceView.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

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
    public new VulkanGraphicsBuffer Resource => base.Resource.As<VulkanGraphicsBuffer>();

    /// <inheritdoc cref="GraphicsResourceView.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

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
