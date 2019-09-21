// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.Windows;
using static TerraFX.Provider.D3D12.HelperUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Provides access to a Direct3D 12 based graphics subsystem.</summary>
    [Export(typeof(IGraphicsProvider))]
    [Export(typeof(GraphicsProvider))]
    [Shared]
    public sealed unsafe class GraphicsProvider : IDisposable, IGraphicsProvider
    {
#if DEBUG
        private const uint CreateFactoryFlags = DXGI_CREATE_FACTORY_DEBUG;
#else
        private const uint CreateFactoryFlags = 0;
#endif

        private ResettableLazy<ImmutableArray<GraphicsAdapter>> _adapters;
        private ResettableLazy<IntPtr> _factory;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsProvider" /> class.</summary>
        [ImportingConstructor]
        public GraphicsProvider()
        {
            _adapters = new ResettableLazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters);
            _factory = new ResettableLazy<IntPtr>(CreateFactory);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsProvider" /> class.</summary>
        ~GraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
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
                return (IDXGIFactory2*)_factory.Value;
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private static IntPtr CreateFactory()
        {
            IntPtr factory;

            var iid = IID_IDXGIFactory2;
            ThrowExternalExceptionIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(CreateFactoryFlags, &iid, (void**)&factory));

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
