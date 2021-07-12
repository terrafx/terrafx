// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Interop.DXGI_DEBUG_RLO_FLAGS;
using static TerraFX.Interop.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsService : GraphicsService
    {
        private ValueLazy<Pointer<IDXGIFactory2>> _dxgiFactory;
        private ValueLazy<ImmutableArray<D3D12GraphicsAdapter>> _adapters;

        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="D3D12GraphicsService" /> class.</summary>
        public D3D12GraphicsService()
        {
            _dxgiFactory = new ValueLazy<Pointer<IDXGIFactory2>>(CreateDxgiFactory);
            _adapters = new ValueLazy<ImmutableArray<D3D12GraphicsAdapter>>(GetAdapters);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsService" /> class.</summary>
        ~D3D12GraphicsService() => Dispose(isDisposing: false);

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="IDXGIFactory1.EnumAdapters1(uint, IDXGIAdapter1**)" /> failed.</exception>
        public override IEnumerable<D3D12GraphicsAdapter> Adapters => _adapters.Value;

        /// <summary>Gets the underlying <see cref="IDXGIFactory2" /> for the service.</summary>
        /// <exception cref="ExternalException">The call to <see cref="CreateDXGIFactory2" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The service has been disposed.</exception>
        public IDXGIFactory2* DxgiFactory => _dxgiFactory.Value;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                if (_adapters.IsValueCreated)
                {
                    foreach (var adapter in _adapters.Value)
                    {
                        adapter?.Dispose();
                    }
                }

                _dxgiFactory.Dispose(ReleaseIfNotNull);

                if (DebugModeEnabled)
                {
                    TryReportLiveObjects();
                }
            }

            _state.EndDispose();

            static void TryReportLiveObjects()
            {
                IDXGIDebug* dxgiDebug = null;

                try
                {
                    var iid = IID_IDXGIDebug;

                    if (SUCCEEDED(DXGIGetDebugInterface(&iid, (void**)&dxgiDebug)))
                    {
                        // We don't want to throw if the debug interface fails to be created
                        _ = dxgiDebug->ReportLiveObjects(DXGI_DEBUG_ALL, DXGI_DEBUG_RLO_DETAIL | DXGI_DEBUG_RLO_IGNORE_INTERNAL);
                    }
                }
                finally
                {
                    ReleaseIfNotNull(dxgiDebug);
                }
            }
        }

        private Pointer<IDXGIFactory2> CreateDxgiFactory()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsService));

            IDXGIFactory2* dxgiFactory;

            var createFlags = (DebugModeEnabled && TryEnableDebugMode()) ? DXGI_CREATE_FACTORY_DEBUG : 0u;
            var iid = IID_IDXGIFactory2;
            ThrowExternalExceptionIfFailed(CreateDXGIFactory2(createFlags, &iid, (void**)&dxgiFactory), nameof(CreateDXGIFactory2));

            return dxgiFactory;

            static bool TryEnableDebugMode()
            {
                var succesfullyEnabled = false;

                ID3D12Debug* dxgiDebug = null;
                ID3D12Debug1* dxgiDebug1 = null;

                try
                {
                    var iid = IID_ID3D12Debug;

                    if (SUCCEEDED(D3D12GetDebugInterface(&iid, (void**)&dxgiDebug)))
                    {
                        // We don't want to throw if the debug interface fails to be created
                        dxgiDebug->EnableDebugLayer();

                        iid = IID_ID3D12Debug1;
                        if (SUCCEEDED(dxgiDebug->QueryInterface(&iid, (void**)&dxgiDebug1)))
                        {
                            dxgiDebug1->SetEnableGPUBasedValidation(TRUE);
                            dxgiDebug1->SetEnableSynchronizedCommandQueueValidation(TRUE);
                        }
                        succesfullyEnabled = true;
                    }
                }
                finally
                {
                    ReleaseIfNotNull(dxgiDebug1);
                    ReleaseIfNotNull(dxgiDebug);
                }

                return succesfullyEnabled;
            }
        }

        private ImmutableArray<D3D12GraphicsAdapter> GetAdapters()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsService));

            var adapters = ImmutableArray.CreateBuilder<D3D12GraphicsAdapter>();

            var dxgiFactory = DxgiFactory;
            IDXGIAdapter1* dxgiAdapter = null;

            try
            {
                uint index = 0;

                do
                {
                    var result = dxgiFactory->EnumAdapters1(index, &dxgiAdapter);

                    if (FAILED(result))
                    {
                        if (result != DXGI_ERROR_NOT_FOUND)
                        {
                            ThrowExternalException(nameof(IDXGIFactory1.EnumAdapters1), result);
                        }
                        index = 0;
                    }
                    else
                    {
                        var adapter = new D3D12GraphicsAdapter(this, dxgiAdapter);
                        adapters.Add(adapter);

                        dxgiAdapter = null;
                        index++;
                    }
                }
                while (index != 0);
            }
            finally
            {
                // We explicitly set adapter to null in the enumeration above so that we only
                // release in the case of an exception being thrown.
                ReleaseIfNotNull(dxgiAdapter);
            }

            return adapters.ToImmutable();
        }
    }
}
