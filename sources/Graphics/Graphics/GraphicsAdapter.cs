// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.DXGI_MEMORY_SEGMENT_GROUP;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics adapter which can be used for computational or graphical operations.</summary>
public sealed unsafe class GraphicsAdapter : IDisposable, INameable
{
    private readonly GraphicsService _service;
    private readonly string _description;

    private ValueList<GraphicsDevice> _devices;
    private readonly ValueMutex _devicesMutex;

    private ComPtr<IDXGIAdapter1> _dxgiAdapter;
    private readonly uint _dxgiAdapterVersion;

    private readonly DXGI_ADAPTER_DESC1 _dxgiAdapterDesc;

    private string _name;
    private VolatileState _state;

    internal GraphicsAdapter(GraphicsService service, IDXGIAdapter1* dxgiAdapter)
    {
        AssertNotNull(service);
        _service = service;

        dxgiAdapter = GetLatestDxgiAdapter(dxgiAdapter, out _dxgiAdapterVersion);
        _dxgiAdapter.Attach(dxgiAdapter);

        DXGI_ADAPTER_DESC1 dxgiAdapterDesc;
        ThrowExternalExceptionIfFailed(dxgiAdapter->GetDesc1(&dxgiAdapterDesc));

        _description = GetUtf16Span(in dxgiAdapterDesc.Description[0], 128).GetString() ?? string.Empty;
        _dxgiAdapterDesc = dxgiAdapterDesc;

        _devices = [];
        _devicesMutex = new ValueMutex();

        _name = _description;
        _ = _state.Transition(VolatileState.Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsAdapter" /> class.</summary>
    ~GraphicsAdapter() => Dispose(isDisposing: false);

    /// <summary>Gets a description of the adapter.</summary>
    public string Description => _description;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
        }
    }

    /// <summary>Gets the PCI Device ID (DID) for the adapter.</summary>
    public uint PciDeviceId => _dxgiAdapterDesc.DeviceId;

    /// <summary>Gets the PCI Vendor ID (VID) for the adapter.</summary>
    public uint PciVendorId => _dxgiAdapterDesc.VendorId;

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    internal IDXGIAdapter1* DxgiAdapter => _dxgiAdapter;

    internal uint DxgiAdapterVersion => _dxgiAdapterVersion;

    internal nuint DxgiDedicatedVideoMemory => _dxgiAdapterDesc.DedicatedVideoMemory;

    internal nuint DxgiSharedSystemMemory => _dxgiAdapterDesc.SharedSystemMemory;

    /// <summary>Creates a new graphics device which utilizes the adapter.</summary>
    /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
    public GraphicsDevice CreateDevice()
    {
        ThrowIfDisposedOrDisposing(_state, _name);

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
        ThrowIfDisposedOrDisposing(_state, _name);
        return CreateDeviceUnsafe(in createOptions);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    internal void AddDevice(GraphicsDevice device) => _devices.Add(device, _devicesMutex);

    internal bool RemoveDevice(GraphicsDevice device) => IsDisposed || _devices.Remove(device, _devicesMutex);

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

    private GraphicsDevice CreateDeviceUnsafe(in GraphicsDeviceCreateOptions createOptions) => new GraphicsDevice(this, in createOptions);

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _devices.Dispose();
        }
        _devicesMutex.Dispose();

        _ = _dxgiAdapter.Reset();
    }
}
