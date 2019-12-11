// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

#if DEBUG
using static TerraFX.Interop.DXGIDebug;
using static TerraFX.Interop.DXGI_DEBUG_RLO_FLAGS;
#endif

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <summary>Provides access to a Direct3D 12 based graphics subsystem.</summary>
    [Export(typeof(IGraphicsProvider))]
    [Shared]
    public sealed unsafe class GraphicsProvider : IDisposable, IGraphicsProvider
    {
        private ResettableLazy<ImmutableArray<GraphicsAdapter>> _adapters;
        private ResettableLazy<Pointer<IDXGIFactory2>> _factory;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsProvider" /> class.</summary>
        [ImportingConstructor]
        public GraphicsProvider()
        {
            _adapters = new ResettableLazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters);
            _factory = new ResettableLazy<Pointer<IDXGIFactory2>>(CreateFactory);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsProvider" /> class.</summary>
        ~GraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IEnumerable<IGraphicsAdapter> GraphicsAdapters
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _adapters.Value;
            }
        }

        /// <summary>Gets the <see cref="IDXGIFactory2" /> for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IDXGIFactory2* Factory
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _factory.Value;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private static Pointer<IDXGIFactory2> CreateFactory()
        {
            Guid iid;
            uint createFactoryFlags = 0;

#if DEBUG
            ID3D12Debug* debug;
            iid = IID_ID3D12Debug;

            if (SUCCEEDED(D3D12GetDebugInterface(&iid, (void**)&debug)))
            {
                // We don't want to throw if the debug interface fails to be created

                debug->EnableDebugLayer();

                ID3D12Debug1* debug1;
                iid = IID_ID3D12Debug1;

                if (SUCCEEDED(debug->QueryInterface(&iid, (void**)&debug1)))
                {
                    debug1->SetEnableGPUBasedValidation(TRUE);
                    debug1->SetEnableSynchronizedCommandQueueValidation(TRUE);
                }
                _ = debug->Release();

                createFactoryFlags |= DXGI_CREATE_FACTORY_DEBUG;
            }
#endif

            IDXGIFactory2* factory;

            iid = IID_IDXGIFactory2;
            ThrowExternalExceptionIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(createFactoryFlags, &iid, (void**)&factory));

            return factory;
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeGraphicsAdapters(isDisposing);
                DisposeFactory();
            }

            _state.EndDispose();
        }

        private void DisposeFactory()
        {
            _state.AssertDisposing();

            if (_factory.IsCreated)
            {
                var factory = (IDXGIFactory2*)_factory.Value;
                _ = factory->Release();

#if DEBUG
                IDXGIDebug* debug;
                var iid = IID_IDXGIDebug;

                if (SUCCEEDED(DXGIGetDebugInterface1(Flags: 0, &iid, (void**)&debug)))
                {
                    // We don't want to throw if the debug interface fails to be created
                    _ = debug->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_DETAIL | DXGI_DEBUG_RLO_IGNORE_INTERNAL);
                    _ = debug->Release();
                }
#endif
            }
        }

        private void DisposeGraphicsAdapters(bool isDisposing)
        {
            _state.AssertDisposing();

            if (isDisposing && _adapters.IsCreated)
            {
                var adapters = _adapters.Value;

                foreach (var adapter in adapters)
                {
                    adapter.Dispose();
                }
            }
        }

        private ImmutableArray<GraphicsAdapter> GetGraphicsAdapters()
        {
            IDXGIAdapter1* adapter = null;
            var graphicsAdapters = ImmutableArray.CreateBuilder<GraphicsAdapter>();

            try
            {
                var index = 0u;

                while (true)
                {
                    var hr = Factory->EnumAdapters1(index, &adapter);

                    if (FAILED(hr))
                    {
                        if (hr == DXGI_ERROR_NOT_FOUND)
                        {
                            break;
                        }

                        ThrowExternalException(nameof(IDXGIFactory1.EnumAdapters1), hr);
                    }

                    var graphicsAdapter = new GraphicsAdapter(this, adapter);
                    graphicsAdapters.Add(graphicsAdapter);
                    adapter = null;

                    index++;
                }
            }
            finally
            {
                // We explicitly set adapter to null in the enumeration above so that we only
                // release in the case of an exception being thrown.

                if (adapter != null)
                {
                    _ = adapter->Release();
                }
            }

            return graphicsAdapters.ToImmutable();
        }
    }
}
