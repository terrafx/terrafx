// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Collections;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.DirectX.DXGI;
using static TerraFX.Interop.DirectX.DXGI_DEBUG_RLO_FLAGS;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AppContextUtilities;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>Provides the base access required for interacting with a graphics subsystem.</summary>
public sealed unsafe class GraphicsService : IDisposable, INameable
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

    private string _name;
    private VolatileState _state;

    private GraphicsService()
    {
        var dxgiFactory = CreateDxgiFactory(out _dxgiFactoryVersion);
        _dxgiFactory.Attach(dxgiFactory);

        _adapters = GetAdapters(dxgiFactory);

        _name = GetType().Name;
        _ = _state.Transition(VolatileState.Initialized);

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
                    var adapter = new GraphicsAdapter(this, dxgiAdapter1);
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

    /// <summary>Finalizes an instance of the <see cref="GraphicsService" /> class.</summary>
    ~GraphicsService() => Dispose(isDisposing: false);

    /// <summary>Creates a new instance of the <see cref="GraphicsService" /> class.</summary>
    /// <returns>A new instance of the <see cref="GraphicsService" /> class.</returns>
    public static GraphicsService Create() => new GraphicsService();

    /// <summary>Gets the adapters available to the service.</summary>
    /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
    public IEnumerable<GraphicsAdapter> Adapters
    {
        get
        {
            ThrowIfDisposedOrDisposing(_state, _name);
            return _adapters;
        }
    }

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

    internal IDXGIFactory3* DxgiFactory => _dxgiFactory;

    internal uint DxgiFactoryVersion => _dxgiFactoryVersion;

    internal static void ReportLiveObjects()
    {
        IDXGIDebug* dxgiDebug = null;

        // DXGIGetDebugInterface1 is exported from dxgi.dll (available since Windows 8.1) rather than
        // dxgidebug.dll, which ships with the Graphics Tools optional feature and isn't always installed
        // (for example, on some arm64 machines and CI runners). When the debug layer is unavailable this
        // simply returns a failing HRESULT instead of throwing a DllNotFoundException.
        if (EnableDebugMode && DXGIGetDebugInterface1(Flags: 0, __uuidof<IDXGIDebug>(), (void**)&dxgiDebug).SUCCEEDED)
        {
            _ = dxgiDebug->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_ALL);
        }

        ReleaseIfNotNull(dxgiDebug);
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

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _adapters.Dispose();
        }

        _ = _dxgiFactory.Reset();

        ReportLiveObjects();
    }
}
