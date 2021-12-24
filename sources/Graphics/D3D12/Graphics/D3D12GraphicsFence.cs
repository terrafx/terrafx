// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_FENCE_FLAGS;
using static TerraFX.Interop.Windows.WAIT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsFence : GraphicsFence
{
    private ID3D12Fence* _d3d12Fence;
    private readonly uint _d3d12FenceVersion;

    private HANDLE _signalEventHandle;

    internal D3D12GraphicsFence(D3D12GraphicsDevice device, in GraphicsFenceCreateOptions createOptions) : base(device)
    {
        device.AddFence(this);

        FenceInfo.IsSignalled = createOptions.IsSignalled;

        _d3d12Fence = CreateD3D12Fence(in createOptions, out _d3d12FenceVersion);

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

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsFence" /> class.</summary>
    ~D3D12GraphicsFence() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12Fence" /> for the fence.</summary>
    public ID3D12Fence* D3D12Fence => _d3d12Fence;

    /// <summary>Gets the interface version of <see cref="D3D12Fence" />.</summary>
    public uint D3D12FenceVersion => _d3d12FenceVersion;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <summary>Gets the event <see cref="HANDLE" /> which is raised when the fence enters the signalled state.</summary>
    public HANDLE SignalEventHandle => _signalEventHandle;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12Fence);
        _d3d12Fence = null;

        CloseIfNotNull(_signalEventHandle);
        _signalEventHandle = HANDLE.NULL;

        _ = Device.RemoveFence(this);
    }

    /// <inheritdoc />
    protected override void ResetUnsafe()
    {
        ThrowForLastErrorIfZero(ResetEvent(_signalEventHandle));
        ThrowExternalExceptionIfFailed(_d3d12Fence->Signal(0));
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12Fence->SetD3D12Name(value);
    }

    /// <inheritdoc />
    protected override bool TryWaitUnsafe(uint millisecondsTimeout)
    {
        var isSignalled = false;
        var signalEventHandle = SignalEventHandle;

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
