// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.DXGI_MEMORY_SEGMENT_GROUP;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsAdapter : GraphicsAdapter
{
    private IDXGIAdapter1* _dxgiAdapter;
    private readonly uint _dxgiAdapterVersion;

    private ValueList<D3D12GraphicsDevice> _devices;
    private readonly ValueMutex _devicesMutex;

    internal D3D12GraphicsAdapter(D3D12GraphicsService service, IDXGIAdapter1* dxgiAdapter) : base(service)
    {
        _dxgiAdapter = GetLatestDxgiAdapter(dxgiAdapter, out _dxgiAdapterVersion);

        DXGI_ADAPTER_DESC1 dxgiAdapterDesc;
        ThrowExternalExceptionIfFailed(_dxgiAdapter->GetDesc1(&dxgiAdapterDesc));

        AdapterInfo.Description = GetUtf16Span(dxgiAdapterDesc.Description, 128).GetString() ?? string.Empty;
        AdapterInfo.PciDeviceId = dxgiAdapterDesc.DeviceId;
        AdapterInfo.PciVendorId = dxgiAdapterDesc.VendorId;

        SetName(AdapterInfo.Description);

        _devices = new ValueList<D3D12GraphicsDevice>();
        _devicesMutex = new ValueMutex();
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsAdapter" /> class.</summary>
    ~D3D12GraphicsAdapter() => Dispose(isDisposing: false);

    /// <summary>Gets the the underlying <see cref="IDXGIAdapter1" /> for the adapter.</summary>
    public IDXGIAdapter1* DxgiAdapter => _dxgiAdapter;

    /// <summary>Gets the interface version of <see cref="DxgiAdapter" />.</summary>
    public uint DxgiAdapterVersion => _dxgiAdapterVersion;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <summary>Tries to query the <see cref="DXGI_QUERY_VIDEO_MEMORY_INFO" /> for <see cref="DXGI_MEMORY_SEGMENT_GROUP_LOCAL" />.</summary>
    /// <param name="dxgiLocalVideoMemoryInfo">The video memory info that will be filled.</param>
    /// <returns><c>true</c> if the query succeeded; otherwise, <c>false</c>.</returns>
    public bool TryQueryLocalVideoMemoryInfo(DXGI_QUERY_VIDEO_MEMORY_INFO* dxgiLocalVideoMemoryInfo)
    {
        var result = false;

        if (DxgiAdapterVersion >= 3)
        {
            var dxgiAdapter3 = (IDXGIAdapter3*)DxgiAdapter;
            result = dxgiAdapter3->QueryVideoMemoryInfo(NodeIndex: 0, DXGI_MEMORY_SEGMENT_GROUP_LOCAL, dxgiLocalVideoMemoryInfo).SUCCEEDED;
        }

        return result;
    }

    /// <summary>Tries to query the <see cref="DXGI_QUERY_VIDEO_MEMORY_INFO" /> for <see cref="DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL" />.</summary>
    /// <param name="dxgiNonLocalVideoMemoryInfo">The video memory info that will be filled.</param>
    /// <returns><c>true</c> if the query succeeded; otherwise, <c>false</c>.</returns>
    public bool TryQueryNonLocalVideoMemoryInfo(DXGI_QUERY_VIDEO_MEMORY_INFO* dxgiNonLocalVideoMemoryInfo)
    {
        var result = false;

        if (DxgiAdapterVersion >= 3)
        {
            var dxgiAdapter3 = (IDXGIAdapter3*)DxgiAdapter;
            result = dxgiAdapter3->QueryVideoMemoryInfo(NodeIndex: 0, DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL, dxgiNonLocalVideoMemoryInfo).SUCCEEDED;
        }

        return result;
    }

    /// <inheritdoc />
    protected override D3D12GraphicsDevice CreateDeviceUnsafe(in GraphicsDeviceCreateOptions createOptions)
    {
        return new D3D12GraphicsDevice(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            for (var index = _devices.Count - 1; index >= 0; index--)
            {
                var device = _devices.GetReferenceUnsafe(index);
                device.Dispose();
            }
            _devices.Clear();
        }
        _devicesMutex.Dispose();

        ReleaseIfNotNull(_dxgiAdapter);
        _dxgiAdapter = null;
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    internal void AddDevice(D3D12GraphicsDevice device)
    {
        _devices.Add(device, _devicesMutex);
    }

    internal bool RemoveDevice(D3D12GraphicsDevice device)
    {
        return IsDisposed || _devices.Remove(device, _devicesMutex);
    }
}
