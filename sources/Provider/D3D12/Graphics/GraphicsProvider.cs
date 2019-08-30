// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Runtime.InteropServices;

using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;

using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.Windows;
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

        /// <summary>The DXGI factory.</summary>
        private readonly Lazy<IntPtr> _factory;

        /// <summary>The <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        private readonly Lazy<ImmutableArray<GraphicsAdapter>> _adapters;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="GraphicsProvider" /> class.</summary>
        [ImportingConstructor]
        public GraphicsProvider()
        {
            _factory = new Lazy<IntPtr>((Func<IntPtr>)CreateFactory, isThreadSafe: true);
            _adapters = new Lazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters, isThreadSafe: true);
            _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="GraphicsProvider" /> class.</summary>
        ~GraphicsProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
        public IEnumerable<IGraphicsAdapter> GraphicsAdapters
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _adapters.Value;
            }
        }

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle
        {
            get
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>Creates a DXGI factory</summary>
        /// <returns>A DXGI factory.</returns>
        /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2(uint, Guid*, void**)" /> failed.</exception>
        private static IntPtr CreateFactory()
        {
            IntPtr factory;

            var iid = IID_IDXGIFactory3;
            ThrowExternalExceptionIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(CreateFactoryFlags, &iid, (void**)&factory));

            return factory;
        }

        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
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

        /// <summary>Disposes of the DXGI factory that was created.</summary>
        private void DisposeFactory()
        {
            if (_factory.IsValueCreated)
            {
                var factory = (IDXGIFactory3*)_factory.Value;
                factory->Release();
            }
        }

        /// <summary>Disposes of all <see cref="GraphicsAdapter" /> instances that have been created.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        private void DisposeGraphicsAdapters(bool isDisposing)
        {
            if (isDisposing && _adapters.IsValueCreated)
            {
                var adapters = _adapters.Value;

                foreach (var adapter in adapters)
                {
                    adapter.Dispose();
                }
            }
        }

        /// <summary>Gets the <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        /// <returns>The <see cref="GraphicsAdapter" /> instances available in the system.</returns>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIFactory1.EnumAdapters1(uint, IDXGIAdapter1**)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        private ImmutableArray<GraphicsAdapter> GetGraphicsAdapters()
        {
            var factory = (IDXGIFactory3*)_factory.Value;
            var adapter = (IDXGIAdapter1*)null;

            var graphicsAdapters = ImmutableArray.CreateBuilder<GraphicsAdapter>();

            try
            {
                var index = 0u;

                while (true)
                {
                    var hr = factory->EnumAdapters1(index, &adapter);

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
                    adapter->Release();
                }
            }

            return graphicsAdapters.ToImmutable();
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
