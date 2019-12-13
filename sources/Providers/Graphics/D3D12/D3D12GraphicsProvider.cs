// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.DXGI_DEBUG_RLO_FLAGS;
using static TerraFX.Interop.DXGIDebug;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc cref="GraphicsProvider" />
    [Export(typeof(GraphicsProvider))]
    [Shared]
    public sealed unsafe class D3D12GraphicsProvider : GraphicsProvider
    {
        private ValueLazy<ImmutableArray<D3D12GraphicsAdapter>> _graphicsAdapters;
        private ValueLazy<Pointer<IDXGIFactory2>> _factory;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="D3D12GraphicsProvider" /> class.</summary>
        [ImportingConstructor]
        public D3D12GraphicsProvider()
        {
            _graphicsAdapters = new ValueLazy<ImmutableArray<D3D12GraphicsAdapter>>(GetGraphicsAdapters);
            _factory = new ValueLazy<Pointer<IDXGIFactory2>>(CreateFactory);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsProvider" /> class.</summary>
        ~D3D12GraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="IDXGIFactory1.EnumAdapters1(uint, IDXGIAdapter1**)" /> failed.</exception>
        public override IEnumerable<GraphicsAdapter> GraphicsAdapters
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _graphicsAdapters.Value;
            }
        }

        /// <summary>Gets the underlying <see cref="IDXGIFactory2" />.</summary>
        /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        public IDXGIFactory2* Factory
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _factory.Value;
            }
        }

        private Pointer<IDXGIFactory2> CreateFactory()
        {
            _state.AssertNotDisposedOrDisposing();

            IDXGIFactory2* factory;

            var createFactoryFlags = (DebugModeEnabled && TryEnableDebugMode()) ? DXGI_CREATE_FACTORY_DEBUG : 0;
            var iid = IID_IDXGIFactory2;
            ThrowExternalExceptionIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(createFactoryFlags, &iid, (void**)&factory));

            return factory;

            static bool TryEnableDebugMode()
            {
                ID3D12Debug* debug = null;
                ID3D12Debug1* debug1 = null;
                var succesfullyEnabled = false;

                try
                {
                    var iid = IID_ID3D12Debug;

                    if (SUCCEEDED(D3D12GetDebugInterface(&iid, (void**)&debug)))
                    {
                        // We don't want to throw if the debug interface fails to be created
                        debug->EnableDebugLayer();

                        iid = IID_ID3D12Debug1;
                        if (SUCCEEDED(debug->QueryInterface(&iid, (void**)&debug1)))
                        {
                            debug1->SetEnableGPUBasedValidation(TRUE);
                            debug1->SetEnableSynchronizedCommandQueueValidation(TRUE);
                        }
                        succesfullyEnabled = true;
                    }
                }
                finally
                {
                    ReleaseIfNotNull(debug1);
                    ReleaseIfNotNull(debug);
                }

                return succesfullyEnabled;
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                if (isDisposing)
                {
                    DisposeIfCreated(_graphicsAdapters);
                }

                ReleaseIfCreated(_factory);

                if (DebugModeEnabled)
                {
                    TryReportLiveObjects();
                }
            }

            _state.EndDispose();

            static void TryReportLiveObjects()
            {
                IDXGIDebug* dxgiDebug = null;

                try
                {
                    var iid = IID_IDXGIDebug;

                    if (SUCCEEDED(DXGIGetDebugInterface(&iid, (void**)&dxgiDebug)))
                    {
                        // We don't want to throw if the debug interface fails to be created
                        _ = dxgiDebug->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_DETAIL | DXGI_DEBUG_RLO_IGNORE_INTERNAL);
                    }
                }
                finally
                {
                    ReleaseIfNotNull(dxgiDebug);
                }
            }
        }

        private ImmutableArray<D3D12GraphicsAdapter> GetGraphicsAdapters()
        {
            _state.AssertNotDisposedOrDisposing();

            IDXGIAdapter1* adapter = null;
            var graphicsAdapters = ImmutableArray.CreateBuilder<D3D12GraphicsAdapter>();

            try
            {
                uint index = 0;

                do
                {
                    var hr = Factory->EnumAdapters1(index, &adapter);

                    if (FAILED(hr))
                    {
                        if (hr != DXGI_ERROR_NOT_FOUND)
                        {
                            ThrowExternalException(nameof(IDXGIFactory1.EnumAdapters1), hr);
                        }
                        index = 0;
                    }
                    else
                    {
                        var graphicsAdapter = new D3D12GraphicsAdapter(this, adapter);
                        graphicsAdapters.Add(graphicsAdapter);

                        adapter = null;
                        index++;
                    }
                }
                while (index != 0);
            }
            finally
            {
                // We explicitly set adapter to null in the enumeration above so that we only
                // release in the case of an exception being thrown.
                ReleaseIfNotNull(adapter);
            }

            return graphicsAdapters.ToImmutable();
        }
    }
}
