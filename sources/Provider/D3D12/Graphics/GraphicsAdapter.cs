// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using TerraFX.Graphics;
using TerraFX.Interop;
using TerraFX.Utilities;

using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.D3D12.Graphics
{
    /// <summary>Represents a graphics adapter.</summary>
    public sealed unsafe class GraphicsAdapter : IDisposable, IGraphicsAdapter
    {
        #region Fields
        /// <summary>The <see cref="GraphicsManager" /> for the instance.</summary>
        internal readonly GraphicsManager _graphicsManager;

        /// <summary>The <see cref="IDXGIAdapter1" /> for the instance.</summary>
        internal readonly IDXGIAdapter1* _adapter;

        /// <summary>The name of the device.</summary>
        internal string _deviceName;

        /// <summary>The PCI ID of the vendor.</summary>
        internal uint _vendorId;

        /// <summary>The PCI ID of the device.</summary>
        internal uint _deviceId;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;
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

            _state.Transition(to: Initialized);
        }
        #endregion

        #region TerraFX.Graphics.IGraphicsAdapter Properties
        /// <summary>Gets the PCI ID of the device.</summary>
        public uint DeviceId
        {
            get
            {
                return _deviceId;
            }
        }

        /// <summary>Gets the name of the device.</summary>
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }
        }

        /// <summary>Gets the <see cref="IGraphicsManager" /> for the instance.</summary>
        public IGraphicsManager GraphicsManager
        {
            get
            {
                return _graphicsManager;
            }
        }

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle
        {
            get
            {
                return (IntPtr)(_adapter);
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
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeAdapter();
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of the DXGI adapter associated with the instance.</summary>
        internal void DisposeAdapter()
        {
            Debug.Assert(_state == Disposing);

            if (_adapter != null)
            {
                _adapter->Release();
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
