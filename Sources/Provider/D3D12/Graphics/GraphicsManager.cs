// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static System.Threading.Interlocked;
using static TerraFX.Interop.DXGI;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Provides a means of managing the graphics subsystem.</summary>
    [Export(typeof(IGraphicsManager))]
    [Export(typeof(GraphicsManager))]
    [Shared]
    public sealed unsafe class GraphicsManager : IDisposable, IGraphicsManager
    {
        #region State Constants
        /// <summary>Indicates the graphics manager is not disposing or disposed.</summary>
        internal const int NotDisposingOrDisposed = 0;

        /// <summary>Indicates the graphics manager is being disposed.</summary>
        internal const int Disposing = 1;

        /// <summary>Indicates the graphics manager has been disposed.</summary>
        internal const int Disposed = 2;
        #endregion

        #region Fields
        /// <summary>The DXGI factory.</summary>
        internal IDXGIFactory3* _factory;

        /// <summary>The <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        internal ImmutableArray<GraphicsAdapter> _graphicsAdapters;

        /// <summary>The state for the instance.</summary>
        /// <remarks>
        ///     <para>This field is <c>volatile</c> to ensure state changes update all threads simultaneously.</para>
        ///     <para><c>volatile</c> does add a read/write barrier at every access, but the state transitions are believed to be infrequent enough for this to not be a problem.</para>
        /// </remarks>
        internal volatile int _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsManager" /> class.</summary>
        /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2(uint, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIFactory1.EnumAdapters1(uint, IDXGIAdapter1**)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        [ImportingConstructor]
        internal GraphicsManager()
        {
            fixed (IDXGIFactory3** factory = &_factory)
            {
                CreateDXGIFactory(factory);
            }
            _graphicsAdapters = GetGraphicsAdapters(this, (IDXGIFactory1*)(_factory));
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="GraphicsManager" /> class.</summary>
        ~GraphicsManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Static Methods
        /// <summary>Creates a DXGI factory</summary>
        /// <returns>A DXGI factory.</returns>
        /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2(uint, Guid*, void**)" /> failed.</exception>
        internal static void CreateDXGIFactory(IDXGIFactory3** factory)
        {
#if DEBUG
            var flags = DXGI_CREATE_FACTORY_DEBUG;
#else
            var flags = 0u;
#endif

            var iid = IID_IDXGIFactory3;
            ThrowExternalExceptionIfFailed(nameof(CreateDXGIFactory2), CreateDXGIFactory2(flags, &iid, (void**)(factory)));
        }

        /// <summary>Gets the <see cref="GraphicsAdapter" /> instances available in the system.</summary>
        /// <param name="graphicsManager">The <see cref="GraphicsManager" /> the adapters belong to.</param>
        /// <param name="factory">The DXGI factory used to enumerate the available adapters.</param>
        /// <returns>The <see cref="GraphicsAdapter" /> instances available in the system.</returns>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIFactory1.EnumAdapters1(uint, IDXGIAdapter1**)" /> failed.</exception>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        internal static ImmutableArray<GraphicsAdapter> GetGraphicsAdapters(GraphicsManager graphicsManager, IDXGIFactory1* factory)
        {
            IDXGIAdapter1* adapter = null;
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

                    var graphicsAdapter = new GraphicsAdapter(graphicsManager, adapter);
                    graphicsAdapters.Add(graphicsAdapter);
                    adapter = null;
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

        /// <summary>Throws a <see cref="ObjectDisposedException" /> if the instance has already been disposed.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        internal static void ThrowIfDisposed(int state)
        {
            if (state >= Disposing) // (_state == Disposing) || (_state == Disposed)
            {
                ThrowObjectDisposedException(nameof(GraphicsManager));
            }
        }
        #endregion

        #region Methods
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void Dispose(bool isDisposing)
        {
            var previousState = Exchange(ref _state, Disposing);

            if (previousState < Disposing) // (previousState != Disposing) && (previousState != Disposed)
            {
                DisposeGraphicsAdapters(isDisposing);
                DisposeDXGIFactory();
            }

            Debug.Assert(_graphicsAdapters.IsEmpty);
            Debug.Assert(_factory == null);

            _state = Disposed;
        }

        /// <summary>Disposes of the DXGI factory that was created.</summary>
        internal void DisposeDXGIFactory()
        {
            Debug.Assert(_state == Disposing);

            if (_factory != null)
            {
                _factory->Release();
                _factory = null;
            }
        }

        /// <summary>Disposes of all <see cref="GraphicsAdapter" /> instances that have been created.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void DisposeGraphicsAdapters(bool isDisposing)
        {
            Debug.Assert(_state == Disposing);

            if (isDisposing)
            {
                foreach (var graphicsAdapter in _graphicsAdapters)
                {
                    graphicsAdapter.Dispose();
                }
            }
            else
            {
                _graphicsAdapters = _graphicsAdapters.Clear();
            }

            Debug.Assert(_graphicsAdapters.IsEmpty);
        }
        #endregion

        #region TerraFX.Graphics.IGraphicsManager Properties
        /// <summary>Gets the <see cref="IGraphicsAdapter" /> instances currently available.</summary>
        public IEnumerable<IGraphicsAdapter> GraphicsAdapters
        {
            get
            {
                ThrowIfDisposed(_state);
                return _graphicsAdapters;
            }
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
