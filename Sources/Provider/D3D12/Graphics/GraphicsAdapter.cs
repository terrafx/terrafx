// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TerraFX.Graphics;
using TerraFX.Interop;
using static System.Threading.Interlocked;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a graphics adapter.</summary>
    public sealed unsafe class GraphicsAdapter : IDisposable, IGraphicsAdapter
    {
        #region State Constants
        /// <summary>Indicates the graphics adapter is not disposing or disposed.</summary>
        internal const int NotDisposingOrDisposed = 0;

        /// <summary>Indicates the graphics adapter is being disposed.</summary>
        internal const int Disposing = 1;

        /// <summary>Indicates the graphics adapter has been disposed.</summary>
        internal const int Disposed = 2;
        #endregion

        #region Fields
        /// <summary>The <see cref="GraphicsManager" /> for the instance.</summary>
        internal readonly GraphicsManager _graphicsManager;

        /// <summary>The <see cref="IDXGIAdapter1" /> for the instance.</summary>
        internal IDXGIAdapter1* _adapter;

        /// <summary>The name of the device.</summary>
        internal string _deviceName;

        /// <summary>The PCI ID of the vendor.</summary>
        internal uint _vendorId;

        /// <summary>The PCI ID of the device.</summary>
        internal uint _deviceId;

        /// <summary>The state for the instance.</summary>
        /// <remarks>
        ///     <para>This field is <c>volatile</c> to ensure state changes update all threads simultaneously.</para>
        ///     <para><c>volatile</c> does add a read/write barrier at every access, but the state transitions are believed to be infrequent enough for this to not be a problem.</para>
        /// </remarks>
        internal volatile int _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="GraphicsAdapter" /> class.</summary>
        /// <param name="graphicsManager">The <see cref="GraphicsManager" /> for the instance.</param>
        /// <param name="adapter">The <see cref="IDXGIAdapter1" /> for the instance.</param>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        internal GraphicsAdapter(GraphicsManager graphicsManager, IDXGIAdapter1* adapter)
        {
            _graphicsManager = graphicsManager;
            _adapter = adapter;

            DXGI_ADAPTER_DESC1 desc;
            ThrowExternalExceptionIfFailed(nameof(IDXGIAdapter1.GetDesc1), adapter->GetDesc1(&desc));

            _deviceName = Marshal.PtrToStringUni((IntPtr)(desc.Description));
            _vendorId = desc.VendorId;
            _deviceId = desc.DeviceId;
        }
        #endregion

        #region Static Methods
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
                DisposeDXGIAdapter();
            }

            Debug.Assert(_adapter == null);

            _state = Disposed;
        }

        /// <summary>Disposes of the DXGI adapter associated with the instance.</summary>
        internal void DisposeDXGIAdapter()
        {
            Debug.Assert(_state == Disposing);

            if (_adapter != null)
            {
                _adapter->Release();
                _adapter = null;
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

        #region TerraFX.Graphics.IGraphicsAdapter Properties
        /// <summary>Gets the name of the device.</summary>
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }
        }

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId
        {
            get
            {
                return _vendorId;
            }
        }

        /// <summary>Gets the PCI ID of the device.</summary>
        public uint DeviceId
        {
            get
            {
                return _deviceId;
            }
        }
        #endregion
    }
}
