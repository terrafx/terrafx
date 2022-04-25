// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.Windows.WAIT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics fence which can be used to synchronize the processor and a graphics context.</summary>
public sealed unsafe class GraphicsFence : GraphicsDeviceObject
{
    private ComPtr<ID3D12Fence> _d3d12Fence;
    private readonly uint _d3d12FenceVersion;

    private bool _isSignalled;

    private HANDLE _signalEventHandle;

    internal GraphicsFence(GraphicsDevice device, in GraphicsFenceCreateOptions createOptions) : base(device)
    {
        device.AddFence(this);

        var d3d12Fence = CreateD3D12Fence(in createOptions, out _d3d12FenceVersion);
        _d3d12Fence.Attach(d3d12Fence);

        _isSignalled = createOptions.IsSignalled;

        var initialState = createOptions.IsSignalled ? TRUE : FALSE;
        ThrowForLastErrorIfZero(_signalEventHandle = CreateEventW(lpEventAttributes: null, bManualReset: TRUE, initialState, lpName: null));

        SetNameUnsafe(Name);

        ID3D12Fence* CreateD3D12Fence(in GraphicsFenceCreateOptions createOptions, out uint d3d12FenceVersion)
        {
            ID3D12Fence* d3d12Fence;

            var initialValue = createOptions.IsSignalled ? 0UL : 1UL;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateFence(initialValue, D3D12_FENCE_FLAG_NONE, __uuidof<ID3D12Fence>(), (void**)&d3d12Fence));

            return GetLatestD3D12Fence(d3d12Fence, out d3d12FenceVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsFence" /> class.</summary>
    ~GraphicsFence() => Dispose(isDisposing: false);

    /// <summary>Gets <c>true</c> if the fence is in the signalled state; otherwise, <c>false</c>.</summary>
    public bool IsSignalled => _isSignalled;

    internal ID3D12Fence* D3D12Fence => _d3d12Fence;

    internal uint D3D12FenceVersion => _d3d12FenceVersion;

    /// <summary>Resets the fence to the unsignalled state.</summary>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposed();

        if (_isSignalled)
        {
            ResetUnsafe();
            _isSignalled = false;
        }
    }

    /// <summary>Waits for the fence to transition to the signalled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signalled state before failing.</param>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    /// <exception cref="TimeoutException"><paramref name="millisecondsTimeout" /> was reached before the fence transitioned to the signalled state.</exception>
    /// <remarks>This method treats <see cref="uint.MaxValue" /> as having no timeout.</remarks>
    public void Wait(uint millisecondsTimeout = uint.MaxValue)
    {
        if (!TryWait(millisecondsTimeout))
        {
            ThrowTimeoutException(nameof(Wait), TimeSpan.FromMilliseconds(millisecondsTimeout));
        }
    }

    /// <summary>Waits for the fence to transition to the signalled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signalled state before failing.</param>
    /// <returns><c>true</c> if the fence transitioned to the signalled state; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    /// <remarks>This method treats <see cref="uint.MaxValue" /> as having no timeout.</remarks>
    public bool TryWait(uint millisecondsTimeout = uint.MaxValue)
    {
        ThrowIfDisposed();

        if (!_isSignalled)
        {
            _isSignalled = TryWaitUnsafe(millisecondsTimeout);
        }

        return _isSignalled;
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _ = _d3d12Fence.Reset();

        CloseIfNotNull(_signalEventHandle);
        _signalEventHandle = HANDLE.NULL;

        _ = Device.RemoveFence(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12Fence->SetD3D12Name(value);
    }

    private void ResetUnsafe()
    {
        ThrowForLastErrorIfZero(ResetEvent(_signalEventHandle));
        ThrowExternalExceptionIfFailed(D3D12Fence->Signal(0));
    }

    private bool TryWaitUnsafe(uint millisecondsTimeout)
    {
        var isSignalled = false;
        var signalEventHandle = _signalEventHandle;

        ThrowExternalExceptionIfFailed(D3D12Fence->SetEventOnCompletion(1, signalEventHandle));
        var result = WaitForSingleObject(signalEventHandle, millisecondsTimeout);

        if (result == WAIT_OBJECT_0)
        {
            isSignalled = true;
        }
        else if (result != WAIT_TIMEOUT)
        {
            ThrowForLastError(nameof(WaitForSingleObject));
        }

        return isSignalled;
    }
}
