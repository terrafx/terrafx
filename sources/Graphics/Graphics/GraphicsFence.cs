// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.Windows.CREATE;
using static TerraFX.Interop.Windows.EVENT;
using static TerraFX.Interop.Windows.WAIT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics fence which can be used to synchronize the processor and a graphics context.</summary>
public sealed unsafe class GraphicsFence : IDisposable, INameable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsService _service;

    private ComPtr<ID3D12Fence> _d3d12Fence;
    private readonly uint _d3d12FenceVersion;

    private bool _isSignaled;

    private HANDLE _signalEventHandle;

    private string _name;
    private VolatileState _state;

    internal GraphicsFence(GraphicsDevice device, in GraphicsFenceCreateOptions createOptions)
    {
        AssertNotNull(device);
        _device = device;

        var adapter = device.Adapter;
        _adapter = adapter;

        var service = adapter.Service;
        _service = service;

        device.AddFence(this);

        var d3d12Fence = CreateD3D12Fence(in createOptions, out _d3d12FenceVersion);
        _d3d12Fence.Attach(d3d12Fence);

        uint flags = CREATE_EVENT_MANUAL_RESET;

        if (createOptions.IsSignaled)
        {
            flags |= CREATE_EVENT_INITIAL_SET;
            _isSignaled = true;
        }
        ThrowForLastErrorIfZero(_signalEventHandle = CreateEventExW(lpEventAttributes: null, lpName: null, flags, EVENT_MODIFY_STATE | SYNCHRONIZE));

        _name = GetType().Name;
        SetNameUnsafe(Name);
        _ = _state.Transition(VolatileState.Initialized);

        ID3D12Fence* CreateD3D12Fence(in GraphicsFenceCreateOptions createOptions, out uint d3d12FenceVersion)
        {
            ID3D12Fence* d3d12Fence;

            var initialValue = createOptions.IsSignaled ? 0UL : 1UL;
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateFence(initialValue, D3D12_FENCE_FLAG_NONE, __uuidof<ID3D12Fence>(), (void**)&d3d12Fence));

            return GetLatestD3D12Fence(d3d12Fence, out d3d12FenceVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsFence" /> class.</summary>
    ~GraphicsFence() => Dispose(isDisposing: false);

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <summary>Gets <c>true</c> if the fence is in the signaled state; otherwise, <c>false</c>.</summary>
    public bool IsSignaled => _isSignaled;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
            SetNameUnsafe(_name);
        }
    }

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    internal ID3D12Fence* D3D12Fence => _d3d12Fence;

    internal uint D3D12FenceVersion => _d3d12FenceVersion;

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <summary>Resets the fence to the unsignaled state.</summary>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        if (_isSignaled)
        {
            ResetUnsafe();
            _isSignaled = false;
        }
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    /// <summary>Waits for the fence to transition to the signaled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signaled state before failing.</param>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    /// <exception cref="TimeoutException"><paramref name="millisecondsTimeout" /> was reached before the fence transitioned to the signaled state.</exception>
    /// <remarks>This method treats <see cref="uint.MaxValue" /> as having no timeout.</remarks>
    public void Wait(uint millisecondsTimeout = uint.MaxValue)
    {
        if (!TryWait(millisecondsTimeout))
        {
            ThrowTimeoutException(nameof(Wait), TimeSpan.FromMilliseconds(millisecondsTimeout));
        }
    }

    /// <summary>Waits for the fence to transition to the signaled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signaled state before failing.</param>
    /// <returns><c>true</c> if the fence transitioned to the signaled state; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    /// <remarks>This method treats <see cref="uint.MaxValue" /> as having no timeout.</remarks>
    public bool TryWait(uint millisecondsTimeout = uint.MaxValue)
    {
        ThrowIfDisposedOrDisposing(_state, _name);

        if (!_isSignaled)
        {
            _isSignaled = TryWaitUnsafe(millisecondsTimeout);
        }

        return _isSignaled;
    }

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            // Nothing to handle
        }
        _ = _d3d12Fence.Reset();

        CloseIfNotNull(_signalEventHandle);
        _signalEventHandle = HANDLE.NULL;

        _ = Device.RemoveFence(this);
    }

    private void ResetUnsafe()
    {
        ThrowForLastErrorIfZero(ResetEvent(_signalEventHandle));
        ThrowExternalExceptionIfFailed(D3D12Fence->Signal(0));
    }

    private void SetNameUnsafe(string value) => D3D12Fence->SetD3D12Name(value);

    private bool TryWaitUnsafe(uint millisecondsTimeout)
    {
        var isSignaled = false;
        var signalEventHandle = _signalEventHandle;

        ThrowExternalExceptionIfFailed(D3D12Fence->SetEventOnCompletion(1, signalEventHandle));
        var result = WaitForSingleObject(signalEventHandle, millisecondsTimeout);

        if (result == WAIT_OBJECT_0)
        {
            isSignaled = true;
        }
        else if (result != WAIT_TIMEOUT)
        {
            ThrowForLastError(nameof(WaitForSingleObject));
        }

        return isSignaled;
    }
}
