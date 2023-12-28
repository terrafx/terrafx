// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.CompilerServices;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_RTV_DIMENSION;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;

namespace TerraFX.Graphics;

/// <summary>A graphics render target which defines a swapchain backbuffer on which rendering can occur.</summary>
public sealed unsafe class GraphicsRenderTarget : GraphicsSwapchainObject
{
    private readonly D3D12_CPU_DESCRIPTOR_HANDLE _d3d12RtvDescriptorHandle;

    private ComPtr<ID3D12Resource> _d3d12RtvResource;
    private readonly uint _d3d12RtvResourceVersion;

    private readonly int _index;

    internal GraphicsRenderTarget(GraphicsSwapchain swapchain, int index) : base(swapchain)
    {
        _index = index;

        _d3d12RtvDescriptorHandle = GetD3D12RtvDescriptorHandle();

        var d3d12RtvResource = CreateD3D12RtvResource(out _d3d12RtvResourceVersion);
        _d3d12RtvResource.Attach(d3d12RtvResource);

        ID3D12Resource* CreateD3D12RtvResource(out uint d3d12RtvResourceVersion)
        {
            var d3d12RenderTargetViewDesc = new D3D12_RENDER_TARGET_VIEW_DESC {
                Format = swapchain.RenderTargetFormat.AsDxgiFormat(),
                ViewDimension = D3D12_RTV_DIMENSION_TEXTURE2D,
                Texture2D = new D3D12_TEX2D_RTV()
            };

            ID3D12Resource* d3d12RtvResource;
            ThrowExternalExceptionIfFailed(swapchain.DxgiSwapchain->GetBuffer((uint)_index, __uuidof<ID3D12Resource>(), (void**)&d3d12RtvResource));

            swapchain.Device.D3D12Device->CreateRenderTargetView(d3d12RtvResource, &d3d12RenderTargetViewDesc, _d3d12RtvDescriptorHandle);
            return GetLatestD3D12Resource(d3d12RtvResource, out d3d12RtvResourceVersion);
        }

        D3D12_CPU_DESCRIPTOR_HANDLE GetD3D12RtvDescriptorHandle()
        {
            var d3d12RtvDescriptorHandleIncrementSize = Device.D3D12RtvDescriptorSize;
            var d3d12RtvDescriptorHandleForHeapStart = Swapchain.D3D12RtvDescriptorHeap->GetCPUDescriptorHandleForHeapStart();

            Unsafe.SkipInit(out D3D12_CPU_DESCRIPTOR_HANDLE d3d12RtvDescriptorHandle);

            D3D12_CPU_DESCRIPTOR_HANDLE.InitOffsetted(ref d3d12RtvDescriptorHandle, d3d12RtvDescriptorHandleForHeapStart, _index, d3d12RtvDescriptorHandleIncrementSize);
            return d3d12RtvDescriptorHandle;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsRenderTarget" /> class.</summary>
    ~GraphicsRenderTarget() => Dispose(isDisposing: false);

    /// <summary>Gets the index for the render target.</summary>
    public int Index => _index;

    internal D3D12_CPU_DESCRIPTOR_HANDLE D3D12RtvDescriptorHandle => _d3d12RtvDescriptorHandle;

    internal ID3D12Resource* D3D12RtvResource => _d3d12RtvResource;

    internal uint D3D12RtvResourceVersion => _d3d12RtvResourceVersion;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing) => _ = _d3d12RtvResource.Reset();

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value) => D3D12RtvResource->SetD3D12Name(value);
}
