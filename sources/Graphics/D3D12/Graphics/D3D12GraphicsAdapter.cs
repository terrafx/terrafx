// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.DirectX;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.DXGI_MEMORY_SEGMENT_GROUP;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using TerraFX.Advanced;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsAdapter : GraphicsAdapter
{
    private readonly IDXGIAdapter3* _dxgiAdapter;
    private readonly DXGI_ADAPTER_DESC2 _dxgiAdapterDesc;

    internal D3D12GraphicsAdapter(D3D12GraphicsService service, IDXGIAdapter3* dxgiAdapter)
        : base(service)
    {
        ThrowIfNull(dxgiAdapter);

        _dxgiAdapter = dxgiAdapter;
        _dxgiAdapterDesc = GetDxgiAdapterDesc(dxgiAdapter);

        var name = GetName(in _dxgiAdapterDesc);
        SetName(name);

        static DXGI_ADAPTER_DESC2 GetDxgiAdapterDesc(IDXGIAdapter3* dxgiAdapter)
        {
            DXGI_ADAPTER_DESC2 dxgiAdapterDesc;
            ThrowExternalExceptionIfFailed(dxgiAdapter->GetDesc2(&dxgiAdapterDesc));
            return dxgiAdapterDesc;
        }

        static string GetName(in DXGI_ADAPTER_DESC2 dxgiAdapterDesc)
        {
            var name = GetUtf16Span(in dxgiAdapterDesc.Description[0], 128).GetString();
            return name ?? string.Empty;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsAdapter" /> class.</summary>
    ~D3D12GraphicsAdapter() => Dispose(isDisposing: false);

    /// <inheritdoc />
    public override uint DeviceId => DxgiAdapterDesc.DeviceId;

    /// <summary>Gets the the underlying <see cref="IDXGIAdapter3" /> for the adapter.</summary>
    public IDXGIAdapter3* DxgiAdapter
    {
        get
        {
            AssertNotDisposed();
            return _dxgiAdapter;
        }
    }

    /// <summary>Gets the <see cref="DXGI_ADAPTER_DESC2" /> for <see cref="DxgiAdapter" />.</summary>
    public ref readonly DXGI_ADAPTER_DESC2 DxgiAdapterDesc => ref _dxgiAdapterDesc;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override uint VendorId => DxgiAdapterDesc.VendorId;

    /// <inheritdoc />
    public override D3D12GraphicsDevice CreateDevice(GraphicsMemoryAllocatorCreateFunc createMemoryAllocator)
    {
        ThrowIfDisposed();
        return new D3D12GraphicsDevice(this, createMemoryAllocator);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_dxgiAdapter);
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
    }

    /// <summary>Tries to query the <see cref="DXGI_QUERY_VIDEO_MEMORY_INFO" /> for <see cref="DXGI_MEMORY_SEGMENT_GROUP_LOCAL" />.</summary>
    /// <param name="dxgiLocalVideoMemoryInfo">The video memory info that will be filled.</param>
    /// <returns><c>true</c> if the query succeeded; otherwise, <c>false</c>.</returns>
    public bool TryGetDxgiQueryLocalVideoMemoryInfo(DXGI_QUERY_VIDEO_MEMORY_INFO* dxgiLocalVideoMemoryInfo)
    {
        var result = DxgiAdapter->QueryVideoMemoryInfo(NodeIndex: 0, DXGI_MEMORY_SEGMENT_GROUP_LOCAL, dxgiLocalVideoMemoryInfo);
        return result.SUCCEEDED;
    }

    /// <summary>Tries to query the <see cref="DXGI_QUERY_VIDEO_MEMORY_INFO" /> for <see cref="DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL" />.</summary>
    /// <param name="dxgiNonLocalVideoMemoryInfo">The video memory info that will be filled.</param>
    /// <returns><c>true</c> if the query succeeded; otherwise, <c>false</c>.</returns>
    public bool TryGetDxgiQueryNonLocalVideoMemoryInfo(DXGI_QUERY_VIDEO_MEMORY_INFO* dxgiNonLocalVideoMemoryInfo)
    {
        var result = DxgiAdapter->QueryVideoMemoryInfo(NodeIndex: 0, DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL, dxgiNonLocalVideoMemoryInfo);
        return result.SUCCEEDED;
    }
}
