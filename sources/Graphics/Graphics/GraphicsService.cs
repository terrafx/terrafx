// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_DEBUG_RLO_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;

namespace TerraFX.Graphics;

/// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
public sealed unsafe class GraphicsService : DisposableObject
{
    /// <summary>Gets <c>true</c> if debug mode should be enabled for the service; otherwise, <c>false</c>.</summary>
    /// <remarks>This defaults to <see cref="IsDebug" /> causing it to be enabled for debug builds and disabled for release builds by default.</remarks>
    public static bool EnableDebugMode { get; } = GetAppContextData(
        $"{typeof(GraphicsService).FullName}.{nameof(EnableDebugMode)}",
        defaultValue: IsDebug
    );

    /// <summary>Gets <c>true</c> if GPU validation should be enabled for the service; otherwise, <c>false</c>.</summary>
    /// <remarks>This defaults to <see cref="IsDebug" /> causing it to be enabled for debug builds and disabled for release builds by default.</remarks>
    public static bool EnableGpuValidation { get; } = GetAppContextData(
        $"{typeof(GraphicsService).FullName}.{nameof(EnableGpuValidation)}",
        defaultValue: IsDebug
    );

    private ValueList<GraphicsAdapter> _adapters;

    private ComPtr<IDXGIFactory3> _dxgiFactory;
    private readonly uint _dxgiFactoryVersion;

    /// <summary>Initializes a new instance of the <see cref="GraphicsService" /> class.</summary>
    public GraphicsService() : base(name: null)
    {
        var dxgiFactory = CreateDxgiFactory(out _dxgiFactoryVersion);
        _dxgiFactory.Attach(dxgiFactory);

        _adapters = GetAdapters(dxgiFactory);

        static IDXGIFactory3* CreateDxgiFactory(out uint dxgiFactoryVersion)
        {
            IDXGIFactory3* dxgiFactory3;

            var createFlags = TryEnableDebugMode() ? DXGI_CREATE_FACTORY_DEBUG : 0u;
            ThrowExternalExceptionIfFailed(CreateDXGIFactory2(createFlags, __uuidof<IDXGIFactory4>(), (void**)&dxgiFactory3));

            return GetLatestDxgiFactory(dxgiFactory3, out dxgiFactoryVersion);
        }

        ValueList<GraphicsAdapter> GetAdapters(IDXGIFactory3* dxgiFactory)
        {
            IDXGIAdapter1* dxgiAdapter1 = null;

            var adapters = new ValueList<GraphicsAdapter>();
            var index = 0u;

            do
            {
                var result = dxgiFactory->EnumAdapters1(index, &dxgiAdapter1);

                if (result.FAILED)
                {
                    if (result != DXGI_ERROR_NOT_FOUND)
                    {
                        ReleaseIfNotNull(dxgiAdapter1);
                        ExceptionUtilities.ThrowExternalException(nameof(IDXGIFactory1.EnumAdapters1), result);
                    }
                    index = 0;
                }
                else
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    var adapter = new GraphicsAdapter(this, dxgiAdapter1);
                    adapters.Add(adapter);

                    index++;
                    dxgiAdapter1 = null;
#pragma warning restore CA2000 // Dispose objects before losing scope
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

    /// <summary>Finalizes an instance of the <see cref="GraphicsService" /> class.</summary>
    ~GraphicsService() => Dispose(isDisposing: false);

    /// <summary>Gets the adapters available to the service.</summary>
    /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
    public IEnumerable<GraphicsAdapter> Adapters
    {
        get
        {
            ThrowIfDisposed();
            return _adapters;
        }
    }

    internal IDXGIFactory3* DxgiFactory => _dxgiFactory;

    internal uint DxgiFactoryVersion => _dxgiFactoryVersion;

    internal static void ReportLiveObjects()
    {
        IDXGIDebug* dxgiDebug = null;

        if (EnableDebugMode && DXGIGetDebugInterface(__uuidof<IDXGIDebug>(), (void**)&dxgiDebug).SUCCEEDED)
        {
            _ = dxgiDebug->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_ALL);
        }

        ReleaseIfNotNull(dxgiDebug);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _adapters.Dispose();
        }

        _ = _dxgiFactory.Reset();

        ReportLiveObjects();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
    }
}
