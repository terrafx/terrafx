// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.DXGI_MEMORY_SEGMENT_GROUP;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics adapter which can be used for computational or graphical operations.</summary>
public sealed unsafe class GraphicsAdapter : GraphicsServiceObject
{
    private readonly string _description;

    private ValueList<GraphicsDevice> _devices;
    private readonly ValueMutex _devicesMutex;

    private ComPtr<IDXGIAdapter1> _dxgiAdapter;
    private readonly uint _dxgiAdapterVersion;

    private readonly uint _pciDeviceId;
    private readonly uint _pciVendorId;

    internal GraphicsAdapter(GraphicsService service, IDXGIAdapter1* dxgiAdapter) : base(service)
    {
        dxgiAdapter = GetLatestDxgiAdapter(dxgiAdapter, out _dxgiAdapterVersion);
        _dxgiAdapter.Attach(dxgiAdapter);

        DXGI_ADAPTER_DESC1 dxgiAdapterDesc;
        ThrowExternalExceptionIfFailed(dxgiAdapter->GetDesc1(&dxgiAdapterDesc));

        _description = GetUtf16Span(dxgiAdapterDesc.Description, 128).GetString() ?? string.Empty;
        _pciDeviceId = dxgiAdapterDesc.DeviceId;
        _pciVendorId = dxgiAdapterDesc.VendorId;

        SetName(_description);

        _devices = new ValueList<GraphicsDevice>();
        _devicesMutex = new ValueMutex();
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsAdapter" /> class.</summary>
    ~GraphicsAdapter() => Dispose(isDisposing: false);

    /// <summary>Gets a description of the adapter.</summary>
    public string Description => _description;

    /// <summary>Gets the PCI Device ID (DID) for the adapter.</summary>
    public uint PciDeviceId => _pciDeviceId;

    /// <summary>Gets the PCI Vendor ID (VID) for the adapter.</summary>
    public uint PciVendorId => _pciVendorId;

    internal IDXGIAdapter1* DxgiAdapter => _dxgiAdapter;

    internal uint DxgiAdapterVersion => _dxgiAdapterVersion;

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
    public GraphicsDevice CreateDevice()
    {
        ThrowIfDisposed();

        var createOptions = new GraphicsDeviceCreateOptions {
            CreateMemoryAllocator = default,
        };
        return CreateDeviceUnsafe(in createOptions);
    }

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <param name="createOptions">The options to use when creating the device.</param>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
    public GraphicsDevice CreateDevice(in GraphicsDeviceCreateOptions createOptions)
    {
        ThrowIfDisposed();
        return CreateDeviceUnsafe(in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _devices.Dispose();
        }
        _devicesMutex.Dispose();

        _ = _dxgiAdapter.Reset();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }

    internal void AddDevice(GraphicsDevice device)
    {
        _devices.Add(device, _devicesMutex);
    }

    internal bool RemoveDevice(GraphicsDevice device)
    {
        return IsDisposed || _devices.Remove(device, _devicesMutex);
    }

    internal bool TryQueryLocalVideoMemoryInfo(DXGI_QUERY_VIDEO_MEMORY_INFO* dxgiLocalVideoMemoryInfo)
    {
        var result = false;

        if (DxgiAdapterVersion >= 3)
        {
            var dxgiAdapter3 = (IDXGIAdapter3*)DxgiAdapter;
            result = dxgiAdapter3->QueryVideoMemoryInfo(NodeIndex: 0, DXGI_MEMORY_SEGMENT_GROUP_LOCAL, dxgiLocalVideoMemoryInfo).SUCCEEDED;
        }

        return result;
    }

    internal bool TryQueryNonLocalVideoMemoryInfo(DXGI_QUERY_VIDEO_MEMORY_INFO* dxgiNonLocalVideoMemoryInfo)
    {
        var result = false;

        if (DxgiAdapterVersion >= 3)
        {
            var dxgiAdapter3 = (IDXGIAdapter3*)DxgiAdapter;
            result = dxgiAdapter3->QueryVideoMemoryInfo(NodeIndex: 0, DXGI_MEMORY_SEGMENT_GROUP_NON_LOCAL, dxgiNonLocalVideoMemoryInfo).SUCCEEDED;
        }

        return result;
    }

    private GraphicsDevice CreateDeviceUnsafe(in GraphicsDeviceCreateOptions createOptions)
    {
        return new GraphicsDevice(this, in createOptions);
    }
}
