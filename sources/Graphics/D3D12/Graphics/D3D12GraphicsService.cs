// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using TerraFX.Collections;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_DEBUG_RLO_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsService : GraphicsService
{
    private IDXGIFactory3* _dxgiFactory;
    private readonly uint _dxgiFactoryVersion;

    private readonly ValueList<D3D12GraphicsAdapter> _adapters;

    /// <summary>Initializes a new instance of the <see cref="D3D12GraphicsService" /> class.</summary>
    public D3D12GraphicsService()
    {
        _dxgiFactory = CreateDxgiFactory(out _dxgiFactoryVersion);
        _adapters = GetAdapters();

        static IDXGIFactory3* CreateDxgiFactory(out uint dxgiFactoryVersion)
        {
            IDXGIFactory3* dxgiFactory3;

            var createFlags = TryEnableDebugMode() ? DXGI_CREATE_FACTORY_DEBUG : 0u;
            ThrowExternalExceptionIfFailed(CreateDXGIFactory2(createFlags, __uuidof<IDXGIFactory4>(), (void**)&dxgiFactory3));

            return GetLatestDxgiFactory(dxgiFactory3, out dxgiFactoryVersion);
        }

        ValueList<D3D12GraphicsAdapter> GetAdapters()
        {
            IDXGIAdapter1* dxgiAdapter1 = null;

            var adapters = new ValueList<D3D12GraphicsAdapter>();
            var index = 0u;

            var dxgiFactory = _dxgiFactory;

            do
            {
                var result = dxgiFactory->EnumAdapters1(index, &dxgiAdapter1);

                if (result.FAILED)
                {
                    if (result != DXGI_ERROR_NOT_FOUND)
                    {
                        ReleaseIfNotNull(dxgiAdapter1);
                        ThrowExternalException(nameof(IDXGIFactory1.EnumAdapters1), result);
                    }
                    index = 0;
                }
                else
                {
                    var adapter = new D3D12GraphicsAdapter(this, dxgiAdapter1);
                    adapters.Add(adapter);

                    index++;
                    dxgiAdapter1 = null;
                }
            }
            while (index != 0);

            return adapters;
        }

        static bool TryEnableDebugMode()
        {
            var debugModeEnabled = false;

            ID3D12Debug* d3d12Debug = null;
            if (EnableDebugMode && D3D12GetDebugInterface(__uuidof<ID3D12Debug>(), (void**)&d3d12Debug).SUCCEEDED)
            {
                d3d12Debug->EnableDebugLayer();

                ID3D12Debug1* d3d12Debug1 = null;
                if (EnableGpuValidation && d3d12Debug->QueryInterface(__uuidof<ID3D12Debug1>(), (void**)&d3d12Debug1).SUCCEEDED)
                {
                    d3d12Debug1->SetEnableGPUBasedValidation(TRUE);
                    d3d12Debug1->SetEnableSynchronizedCommandQueueValidation(TRUE);
                }
                ReleaseIfNotNull(d3d12Debug1);

                debugModeEnabled = true;
            }
            ReleaseIfNotNull(d3d12Debug);

            return debugModeEnabled;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsService" /> class.</summary>
    ~D3D12GraphicsService() => Dispose(isDisposing: false);

    /// <inheritdoc />
    public override IEnumerable<D3D12GraphicsAdapter> Adapters
    {
        get
        {
            ThrowIfDisposed();
            return _adapters;
        }
    }

    /// <summary>Gets the underlying <see cref="IDXGIFactory4" /> for the service.</summary>
    public IDXGIFactory3* DxgiFactory => _dxgiFactory;

    /// <summary>Gets the interface version of <see cref="DxgiFactory" />.</summary>
    public uint DxgiFactoryVersion => _dxgiFactoryVersion;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            for (var index = _adapters.Count - 1; index >= 0; index--)
            {
                var adapter = _adapters.GetReferenceUnsafe(index);
                adapter.Dispose();
            }
            _adapters.Clear();
        }

        ReleaseIfNotNull(_dxgiFactory);
        _dxgiFactory = null;

        ReportLiveObjects();

        static void ReportLiveObjects()
        {
            IDXGIDebug* dxgiDebug = null;

            if (EnableDebugMode && DXGIGetDebugInterface(__uuidof<IDXGIDebug>(), (void**)dxgiDebug).SUCCEEDED)
            {
                _ = dxgiDebug->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_ALL);
            }

            ReleaseIfNotNull(dxgiDebug);
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
