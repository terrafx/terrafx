// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc cref="IGraphicsAdapter" />
    public sealed unsafe class D3D12GraphicsAdapter : IGraphicsAdapter
    {
        private readonly D3D12GraphicsProvider _graphicsProvider;
        private readonly IDXGIAdapter1* _adapter;

        private ValueLazy<DXGI_ADAPTER_DESC1> _adapterDesc;
        private ValueLazy<string> _deviceName;

        private State _state;

        internal D3D12GraphicsAdapter(D3D12GraphicsProvider graphicsProvider, IDXGIAdapter1* adapter)
        {
            _graphicsProvider = graphicsProvider;
            _adapter = adapter;

            _adapterDesc = new ValueLazy<DXGI_ADAPTER_DESC1>(GetAdapterDesc);
            _deviceName = new ValueLazy<string>(GetDeviceName);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Gets the the underlying <see cref="IDXGIAdapter1" />.</summary>
        /// <exception cref="ObjectDisposedException">The instance has been disposed.</exception>
        public IDXGIAdapter1* Adapter
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _adapter;
            }
        }

        /// <summary>Gets the <see cref="DXGI_ADAPTER_DESC1" /> for <see cref="Adapter" />.</summary>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The instance has been disposed and the value was not otherwise cached.</exception>
        public ref readonly DXGI_ADAPTER_DESC1 AdapterDesc => ref _adapterDesc.RefValue;

        /// <inheritdoc />
        public uint DeviceId => AdapterDesc.DeviceId;

        /// <inheritdoc />
        public string DeviceName => _deviceName.Value;

        /// <inheritdoc />
        public IGraphicsProvider GraphicsProvider => _graphicsProvider;

        /// <inheritdoc />
        public uint VendorId => AdapterDesc.VendorId;

        /// <inheritdoc />
        public IGraphicsContext CreateGraphicsContext(IGraphicsSurface graphicsSurface)
        {
            _state.ThrowIfDisposedOrDisposing();
            ThrowIfNull(graphicsSurface, nameof(graphicsSurface));
            return new D3D12GraphicsContext(this, graphicsSurface);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                ReleaseIfNotNull(_adapter);
            }

            _state.EndDispose();
        }

        private DXGI_ADAPTER_DESC1 GetAdapterDesc()
        {
            _state.ThrowIfDisposedOrDisposing();

            DXGI_ADAPTER_DESC1 desc;
            ThrowExternalExceptionIfFailed(nameof(IDXGIAdapter1.GetDesc1), Adapter->GetDesc1(&desc));
            return desc;
        }

        private string GetDeviceName() => MarshalNullTerminatedStringUtf16(in AdapterDesc.Description[0], 128).AsString() ?? string.Empty;
    }
}
