// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsTextureView : GraphicsTextureView
{
    private readonly UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;
    private readonly uint _d3d12SubresourceIndex;

    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsTextureView(D3D12GraphicsTexture texture, in GraphicsTextureViewInfo textureViewInfo, uint d3d12SubresourceIndex, UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> d3d12PlacedSubresourceFootprints)
        : base(texture, in textureViewInfo)
    {
        texture.AddView(this);

        _d3d12PlacedSubresourceFootprints = d3d12PlacedSubresourceFootprints;
        _d3d12SubresourceIndex = d3d12SubresourceIndex;

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsTextureView);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTextureView" /> class.</summary>
    ~D3D12GraphicsTextureView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsResourceView.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT" />s for the texture view.</summary>
    public UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <summary>Gets the subresource index for the texture view.</summary>
    public uint D3D12SubresourceIndex => _d3d12SubresourceIndex;

    /// <inheritdoc cref="GraphicsResourceView.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets or sets the name for the texture view.</summary>
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
    public new D3D12GraphicsTexture Resource => base.Resource.As<D3D12GraphicsTexture>();

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
        }

        _state.EndDispose();
    }
}
