// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.Windows.WAIT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsFence : GraphicsFence
    {
        private ValueLazy<Pointer<ID3D12Fence>> _d3d12Fence;
        private ValueLazy<HANDLE> _d3d12FenceSignalEvent;

        private ulong _d3d12FenceSignalValue;

        private VolatileState _state;

        internal D3D12GraphicsFence(D3D12GraphicsDevice device)
            : base(device)
        {
            _d3d12Fence = new ValueLazy<Pointer<ID3D12Fence>>(CreateD3D12Fence);
            _d3d12FenceSignalEvent = new ValueLazy<HANDLE>(CreateEventHandle);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsFence" /> class.</summary>
        ~D3D12GraphicsFence() => Dispose(isDisposing: false);

        /// <summary>Gets the underlying <see cref="ID3D12Fence" /> for the fence.</summary>
        /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
        public ID3D12Fence* D3D12Fence => _d3d12Fence.Value;

        /// <summary>Gets a <see cref="HANDLE" /> to an event which is raised when the fence enters the signalled state.</summary>
        /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
        public HANDLE D3D12FenceSignalEvent => _d3d12FenceSignalEvent.Value;

        /// <summary>Gets the value at which the fence will enter the signalled state.</summary>
        /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
        public ulong D3D12FenceSignalValue => _d3d12FenceSignalValue;

        /// <inheritdoc cref="GraphicsDeviceObject.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

        /// <inheritdoc />
        public override bool IsSignalled => D3D12Fence->GetCompletedValue() >= D3D12FenceSignalValue;

        /// <inheritdoc />
        public override void Reset()
        {
            if (IsSignalled)
            {
                _d3d12FenceSignalValue = D3D12Fence->GetCompletedValue() + 1;
            }
        }

        /// <inheritdoc />
        public override bool TryWait(int millisecondsTimeout = -1)
        {
            Assert(AssertionsEnabled && (millisecondsTimeout >= Timeout.Infinite));
            return TryWait(unchecked((uint)millisecondsTimeout));
        }

        /// <inheritdoc />
        public override bool TryWait(TimeSpan timeout)
        {
            var remainingMilliseconds = (long)timeout.TotalMilliseconds;
            Assert(AssertionsEnabled && (remainingMilliseconds >= Timeout.Infinite));

            var fenceSignalled = false;

            while (remainingMilliseconds > INFINITE)
            {
                const uint MillisecondsTimeout = INFINITE - 1;

                if (TryWait(MillisecondsTimeout))
                {
                    fenceSignalled = true;
                    break;
                }

                remainingMilliseconds -= MillisecondsTimeout;
            }

            return fenceSignalled || TryWait(unchecked((uint)remainingMilliseconds));
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _d3d12Fence.Dispose(ReleaseIfNotNull);
                _d3d12FenceSignalEvent.Dispose(DisposeEventHandle);
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12Fence> CreateD3D12Fence()
        {
            AssertNotDisposedOrDisposing(_state);

            ID3D12Fence* d3d12Fence;
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateFence(InitialValue: 0, D3D12_FENCE_FLAG_NONE, __uuidof<ID3D12Fence>(), (void**)&d3d12Fence), nameof(ID3D12Device.CreateFence));

            return d3d12Fence;
        }

        private HANDLE CreateEventHandle()
        {
            AssertNotDisposedOrDisposing(_state);

            var eventHandle = CreateEventW(lpEventAttributes: null, bManualReset: FALSE, bInitialState: FALSE, lpName: null);

            if (eventHandle == HANDLE.NULL)
            {
                ThrowForLastError(nameof(CreateEventW));
            }

            return eventHandle;
        }

        private void DisposeEventHandle(HANDLE eventHandle)
        {
            AssertDisposing(_state);

            if (eventHandle != HANDLE.NULL)
            {
                _ = CloseHandle(eventHandle);
            }
        }

        private bool TryWait(uint millisecondsTimeout)
        {
            var fenceSignalled = IsSignalled;

            var fence = D3D12Fence;
            var fenceSignalEvent = D3D12FenceSignalEvent;

            if (!fenceSignalled)
            {
                ThrowExternalExceptionIfFailed(D3D12Fence->SetEventOnCompletion(D3D12FenceSignalValue, fenceSignalEvent), nameof(ID3D12Fence.SetEventOnCompletion));

                var result = WaitForSingleObject(fenceSignalEvent, millisecondsTimeout);

                if (result == WAIT_OBJECT_0)
                {
                    fenceSignalled = true;
                }
                else if (result != WAIT_TIMEOUT)
                {
                    ThrowForLastError(nameof(WaitForSingleObject));
                }
            }

            return fenceSignalled;
        }
    }
}
