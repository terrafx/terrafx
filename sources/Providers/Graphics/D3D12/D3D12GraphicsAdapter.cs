// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsAdapter : GraphicsAdapter
    {
        private readonly IDXGIAdapter1* _dxgiAdapter;

        private ValueLazy<DXGI_ADAPTER_DESC1> _dxgiAdapterDesc;
        private ValueLazy<string> _name;

        private State _state;

        internal D3D12GraphicsAdapter(D3D12GraphicsProvider graphicsProvider, IDXGIAdapter1* dxgiAdapter)
            : base(graphicsProvider)
        {
            AssertNotNull(dxgiAdapter, nameof(dxgiAdapter));

            _dxgiAdapter = dxgiAdapter;

            _dxgiAdapterDesc = new ValueLazy<DXGI_ADAPTER_DESC1>(GetDxgiAdapterDesc);
            _name = new ValueLazy<string>(GetName);

            _ = _state.Transition(to: Initialized);
        }

        /// <inheritdoc cref="GraphicsAdapter.GraphicsProvider" />
        public D3D12GraphicsProvider D3D12GraphicsProvider => (D3D12GraphicsProvider)GraphicsProvider;

        /// <inheritdoc />
        public override uint DeviceId => DxgiAdapterDesc.DeviceId;

        /// <summary>Gets the the underlying <see cref="IDXGIAdapter1" /> for the adapter.</summary>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
        public IDXGIAdapter1* DxgiAdapter
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _dxgiAdapter;
            }
        }

        /// <summary>Gets the <see cref="DXGI_ADAPTER_DESC1" /> for <see cref="DxgiAdapter" />.</summary>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
        public ref readonly DXGI_ADAPTER_DESC1 DxgiAdapterDesc => ref _dxgiAdapterDesc.RefValue;

        /// <inheritdoc />
        public override string Name => _name.Value;

        /// <inheritdoc />
        public override uint VendorId => DxgiAdapterDesc.VendorId;

        /// <inheritdoc cref="CreateGraphicsDevice(IGraphicsSurface)" />
        public D3D12GraphicsDevice CreateD3D12GraphicsDevice(IGraphicsSurface graphicsSurface)
        {
            _state.ThrowIfDisposedOrDisposing();
            return new D3D12GraphicsDevice(this, graphicsSurface);
        }

        /// <inheritdoc />
        public override GraphicsDevice CreateGraphicsDevice(IGraphicsSurface graphicsSurface) => CreateD3D12GraphicsDevice(graphicsSurface);

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                ReleaseIfNotNull(_dxgiAdapter);
            }

            _state.EndDispose();
        }

        private DXGI_ADAPTER_DESC1 GetDxgiAdapterDesc()
        {
            _state.ThrowIfDisposedOrDisposing();

            DXGI_ADAPTER_DESC1 dxgiAdapterDesc;
            ThrowExternalExceptionIfFailed(nameof(IDXGIAdapter1.GetDesc1), DxgiAdapter->GetDesc1(&dxgiAdapterDesc));
            return dxgiAdapterDesc;
        }

        private string GetName()
        {
            _state.ThrowIfDisposedOrDisposing();
            return MarshalNullTerminatedStringUtf16(in DxgiAdapterDesc.Description[0], 128).AsString() ?? string.Empty;
        }
    }
}
