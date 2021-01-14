// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsAdapter : GraphicsAdapter
    {
        private readonly IDXGIAdapter1* _dxgiAdapter;

        private ValueLazy<DXGI_ADAPTER_DESC1> _dxgiAdapterDesc;
        private ValueLazy<string> _name;

        private VolatileState _state;

        internal D3D12GraphicsAdapter(D3D12GraphicsProvider provider, IDXGIAdapter1* dxgiAdapter)
            : base(provider)
        {
            ThrowIfNull(dxgiAdapter, nameof(dxgiAdapter));

            _dxgiAdapter = dxgiAdapter;

            _dxgiAdapterDesc = new ValueLazy<DXGI_ADAPTER_DESC1>(GetDxgiAdapterDesc);
            _name = new ValueLazy<string>(GetName);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsAdapter" /> class.</summary>
        ~D3D12GraphicsAdapter() => Dispose(isDisposing: true);

        /// <inheritdoc />
        public override uint DeviceId => DxgiAdapterDesc.DeviceId;

        /// <summary>Gets the the underlying <see cref="IDXGIAdapter1" /> for the adapter.</summary>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed.</exception>
        public IDXGIAdapter1* DxgiAdapter
        {
            get
            {
                ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsAdapter));
                return _dxgiAdapter;
            }
        }

        /// <summary>Gets the <see cref="DXGI_ADAPTER_DESC1" /> for <see cref="DxgiAdapter" />.</summary>
        /// <exception cref="ExternalException">The call to <see cref="IDXGIAdapter1.GetDesc1(DXGI_ADAPTER_DESC1*)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The adapter has been disposed and the value was not otherwise cached.</exception>
        public ref readonly DXGI_ADAPTER_DESC1 DxgiAdapterDesc => ref _dxgiAdapterDesc.ValueRef;

        /// <inheritdoc />
        public override string Name => _name.Value;

        /// <inheritdoc cref="GraphicsAdapter.Provider" />
        public new D3D12GraphicsProvider Provider => (D3D12GraphicsProvider)base.Provider;

        /// <inheritdoc />
        public override uint VendorId => DxgiAdapterDesc.VendorId;

        /// <inheritdoc />
        public override D3D12GraphicsDevice CreateDevice(IGraphicsSurface surface, int contextCount)
        {
            ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsAdapter));
            return new D3D12GraphicsDevice(this, surface, contextCount);
        }

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
            ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsAdapter));

            DXGI_ADAPTER_DESC1 adapterDesc;
            ThrowExternalExceptionIfFailed(DxgiAdapter->GetDesc1(&adapterDesc), nameof(IDXGIAdapter1.GetDesc1));
            return adapterDesc;
        }

        private string GetName()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsAdapter));
            return MarshalUtf16ToReadOnlySpan(in DxgiAdapterDesc.Description[0], 128).AsString() ?? string.Empty;
        }
    }
}
