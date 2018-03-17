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
    /// <summary>Provides a means of managing the graphics subsystem.</summary>
    [Export(typeof(IGraphicsManager))]
    [Export(typeof(GraphicsManager))]
    [Shared]
    public sealed unsafe class GraphicsManager : IDisposable, IGraphicsManager
    {
        #region Constants
#if DEBUG
        internal const uint CreateFactoryFlags = DXGI_CREATE_FACTORY_DEBUG;
#else
        internal const uint CreateFactoryFlags = 0;
#endif
        #endregion

        #region Fields
        /// <summary>The DXGI factory.</summary>
        internal readonly Lazy<IntPtr> _factory;

        /// <summary>The <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        internal readonly Lazy<ImmutableArray<GraphicsAdapter>> _adapters;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsManager" /> class.</summary>
        [ImportingConstructor]
        public GraphicsManager()
        {
            _factory = new Lazy<IntPtr>(CreateFactory, isThreadSafe: true);
            _adapters = new Lazy<ImmutableArray<GraphicsAdapter>>(GetGraphicsAdapters, isThreadSafe: true);
            _state.Transition(to: Initialized);
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="GraphicsManager" /> class.</summary>
        ~GraphicsManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region TerraFX.Graphics.IGraphicsManager Properties
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
        #endregion

        #region Static Methods
        /// <summary>Creates a DXGI factory</summary>
        /// <returns>A DXGI factory.</returns>
        /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2(uint, Guid*, void**)" /> failed.</exception>
        internal static IntPtr CreateFactory()
        {
            IntPtr factory;

            var iid = IID_IDXGIFactory3;
            ThrowExternalExceptionIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(CreateFactoryFlags, &iid, (void**)(&factory)));

            return factory;
        }
        #endregion

        #region Methods
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void Dispose(bool isDisposing)
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
        internal void DisposeFactory()
        {
            if (_factory.IsValueCreated)
            {
                var factory = (IDXGIFactory3*)(_factory.Value);
                factory->Release();
            }
        }

        /// <summary>Disposes of all <see cref="GraphicsAdapter" /> instances that have been created.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void DisposeGraphicsAdapters(bool isDisposing)
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
        internal ImmutableArray<GraphicsAdapter> GetGraphicsAdapters()
        {
            var factory = (IDXGIFactory3*)(_factory.Value);
            var adapter = (IDXGIAdapter1*)(null);

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
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
