// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
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
    private readonly IDXGIFactory4* _dxgiFactory;
    private readonly ImmutableArray<D3D12GraphicsAdapter> _adapters;

    /// <summary>Initializes a new instance of the <see cref="D3D12GraphicsService" /> class.</summary>
    public D3D12GraphicsService() : base()
    {
        var dxgiFactory = CreateDxgiFactory(EnableDebugMode);

        _dxgiFactory = dxgiFactory;
        _adapters = GetAdapters(this, dxgiFactory);

        static IDXGIFactory4* CreateDxgiFactory(bool enableDebugMode)
        {
            IDXGIFactory4* dxgiFactory;

            var createFlags = (enableDebugMode && TryEnableDebugMode()) ? DXGI_CREATE_FACTORY_DEBUG : 0u;
            ThrowExternalExceptionIfFailed(CreateDXGIFactory2(createFlags, __uuidof<IDXGIFactory4>(), (void**)&dxgiFactory));

            return dxgiFactory;
        }

        static ImmutableArray<D3D12GraphicsAdapter> GetAdapters(D3D12GraphicsService service, IDXGIFactory4* dxgiFactory)
        {
            IDXGIAdapter1* dxgiAdapter = null;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return GetAdaptersInternal(service, dxgiFactory, &dxgiAdapter);
            }
            finally
            {
                // We explicitly set adapter to null in the enumeration above so that we only
                // release in the case of an exception being thrown.
                ReleaseIfNotNull(dxgiAdapter);
            }
        }

        static ImmutableArray<D3D12GraphicsAdapter> GetAdaptersInternal(D3D12GraphicsService service, IDXGIFactory4* dxgiFactory, IDXGIAdapter1** pDxgiAdapter)
        {
            var adaptersBuilder = ImmutableArray.CreateBuilder<D3D12GraphicsAdapter>();
            uint index = 0;

            do
            {
                var result = dxgiFactory->EnumAdapters1(index, pDxgiAdapter);

                if (FAILED(result))
                {
                    if (result != DXGI_ERROR_NOT_FOUND)
                    {
                        ThrowExternalException(nameof(IDXGIFactory1.EnumAdapters1), result);
                    }
                    index = 0;
                }
                else
                {
                    IDXGIAdapter3* dxgiAdapter3;

                    if (SUCCEEDED(pDxgiAdapter[0]->QueryInterface(__uuidof<IDXGIAdapter3>(), (void**)&dxgiAdapter3)))
                    {
                        var adapter = new D3D12GraphicsAdapter(service, dxgiAdapter3);
                        adaptersBuilder.Add(adapter);
                    }

                    _ = pDxgiAdapter[0]->Release();
                    pDxgiAdapter[0] = null;

                    index++;
                }
            }
            while (index != 0);

            return adaptersBuilder.ToImmutable();
        }

        static bool TryEnableDebugMode()
        {
            ID3D12Debug* d3d12Debug = null;
            ID3D12Debug1* d3d12Debug1 = null;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return TryEnableDebugModeInternal(&d3d12Debug, &d3d12Debug1);
            }
            finally
            {
                ReleaseIfNotNull(d3d12Debug1);
                ReleaseIfNotNull(d3d12Debug);
            }
        }

        static bool TryEnableDebugModeInternal(ID3D12Debug** pD3D12Debug, ID3D12Debug1** pD3D12Debug1)
        {
            var debugModeEnabled = false;

            if (SUCCEEDED(D3D12GetDebugInterface(__uuidof<ID3D12Debug>(), (void**)pD3D12Debug)))
            {
                // We don't want to throw if the debug interface fails to be created
                pD3D12Debug[0]->EnableDebugLayer();

                if (EnableGpuValidation && SUCCEEDED(pD3D12Debug[0]->QueryInterface(__uuidof<ID3D12Debug1>(), (void**)pD3D12Debug1)))
                {
                    pD3D12Debug1[0]->SetEnableGPUBasedValidation(TRUE);
                    pD3D12Debug1[0]->SetEnableSynchronizedCommandQueueValidation(TRUE);
                }
                debugModeEnabled = true;
            }

            return debugModeEnabled;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsService" /> class.</summary>
    ~D3D12GraphicsService() => Dispose(isDisposing: false);

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="IDXGIFactory1.EnumAdapters1(uint, IDXGIAdapter1**)" /> failed.</exception>
    public override IEnumerable<D3D12GraphicsAdapter> Adapters => _adapters;

    /// <summary>Gets the underlying <see cref="IDXGIFactory4" /> for the service.</summary>
    /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2" /> failed.</exception>
    /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
    public IDXGIFactory4* DxgiFactory
    {
        get
        {
            AssertNotDisposed();
            return _dxgiFactory;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            foreach (var adapter in _adapters)
            {
                adapter?.Dispose();
            }
        }

        ReleaseIfNotNull(_dxgiFactory);

        if (EnableDebugMode)
        {
            TryReportLiveObjects();
        }

        static void TryReportLiveObjects()
        {
            IDXGIDebug* dxgiDebug = null;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                TryReportLiveObjectsInternal(&dxgiDebug);
            }
            finally
            {
                ReleaseIfNotNull(dxgiDebug);
            }
        }

        static void TryReportLiveObjectsInternal(IDXGIDebug** dxgiDebug)
        {
            if (SUCCEEDED(DXGIGetDebugInterface(__uuidof<IDXGIDebug>(), (void**)dxgiDebug)))
            {
                // We don't want to throw if the debug interface fails to be created
                _ = dxgiDebug[0]->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_DETAIL | DXGI_DEBUG_RLO_IGNORE_INTERNAL);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
    }
}
