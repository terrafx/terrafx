// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class D3D12GraphicsTextureView : GraphicsTextureView
{
    private readonly UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> _d3d12PlacedSubresourceFootprints;
    private readonly uint _d3d12SubresourceIndex;

    internal D3D12GraphicsTextureView(D3D12GraphicsTexture texture, in GraphicsTextureViewInfo textureViewInfo, uint d3d12SubresourceIndex, UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> d3d12PlacedSubresourceFootprints)
        : base(texture, in textureViewInfo)
    {
        texture.AddView(this);

        _d3d12PlacedSubresourceFootprints = d3d12PlacedSubresourceFootprints;
        _d3d12SubresourceIndex = d3d12SubresourceIndex;
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTextureView" /> class.</summary>
    ~D3D12GraphicsTextureView() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="D3D12_PLACED_SUBRESOURCE_FOOTPRINT" />s for the texture view.</summary>
    public UnmanagedReadOnlySpan<D3D12_PLACED_SUBRESOURCE_FOOTPRINT> D3D12PlacedSubresourceFootprints => _d3d12PlacedSubresourceFootprints;

    /// <summary>Gets the subresource index for the texture view.</summary>
    public uint D3D12SubresourceIndex => _d3d12SubresourceIndex;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsResourceView.Resource" />
    public new D3D12GraphicsTexture Resource => base.Resource.As<D3D12GraphicsTexture>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _ = Resource.RemoveView(this);
        }
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
    }
}
