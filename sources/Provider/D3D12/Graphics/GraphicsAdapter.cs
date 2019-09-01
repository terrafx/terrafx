// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
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
        private readonly GraphicsProvider _graphicsProvider;
        private readonly IDXGIAdapter1* _adapter;
        private readonly string _deviceName;
        private readonly uint _vendorId;
        private readonly uint _deviceId;

        private State _state;

        internal GraphicsAdapter(GraphicsProvider graphicsProvider, IDXGIAdapter1* adapter)
        {
            _graphicsProvider = graphicsProvider;
            _adapter = adapter;

            DXGI_ADAPTER_DESC1 desc;
            ThrowExternalExceptionIfFailed(nameof(IDXGIAdapter1.GetDesc1), adapter->GetDesc1(&desc));

            _deviceName = Marshal.PtrToStringUni((IntPtr)desc.Description)!;
            _vendorId = desc.VendorId;
            _deviceId = desc.DeviceId;

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Gets the PCI ID of the device.</summary>
        public uint DeviceId => _deviceId;

        /// <summary>Gets the name of the device.</summary>
        public string DeviceName => _deviceName;

        /// <summary>Gets the <see cref="IGraphicsProvider" /> for the instance.</summary>
        public IGraphicsProvider GraphicsProvider => _graphicsProvider;

        /// <summary>Gets the underlying handle for the instance.</summary>
        public IntPtr Handle => (IntPtr)_adapter;

        /// <summary>Gets the PCI ID of the vendor.</summary>
        public uint VendorId => _vendorId;

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private static void ThrowIfDisposed(int state)
        {
            if (state >= Disposing) // (_state == Disposing) || (_state == Disposed)
            {
                ThrowObjectDisposedException(nameof(GraphicsProvider));
            }
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeAdapter();
            }

            _state.EndDispose();
        }

        private void DisposeAdapter()
        {
            _state.AssertDisposing();

            if (_adapter != null)
            {
                _ = _adapter->Release();
            }
        }
    }
}
