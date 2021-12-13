// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsAdapter : GraphicsAdapter
{
    private readonly IDXGIAdapter1* _dxgiAdapter;

    private readonly DXGI_ADAPTER_DESC1 _dxgiAdapterDesc;
    private readonly string _name;

    private VolatileState _state;

    internal D3D12GraphicsAdapter(D3D12GraphicsService service, IDXGIAdapter1* dxgiAdapter)
        : base(service)
    {
        ThrowIfNull(dxgiAdapter);

        _dxgiAdapter = dxgiAdapter;

        _dxgiAdapterDesc = GetDxgiAdapterDesc(dxgiAdapter);
        _name = GetName(in _dxgiAdapterDesc);

        _ = _state.Transition(to: Initialized);

        static DXGI_ADAPTER_DESC1 GetDxgiAdapterDesc(IDXGIAdapter1* dxgiAdapter)
        {
            DXGI_ADAPTER_DESC1 dxgiAdapterDesc;
            ThrowExternalExceptionIfFailed(dxgiAdapter->GetDesc1(&dxgiAdapterDesc));
            return dxgiAdapterDesc;
        }

        static string GetName(in DXGI_ADAPTER_DESC1 dxgiAdapterDesc)
        {
            var name = GetUtf16Span(in dxgiAdapterDesc.Description[0], 128).GetString();
            return name ?? string.Empty;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsAdapter" /> class.</summary>
    ~D3D12GraphicsAdapter() => Dispose(isDisposing: true);

    /// <inheritdoc />
    public override uint DeviceId => DxgiAdapterDesc.DeviceId;

    /// <summary>Gets the the underlying <see cref="IDXGIAdapter1" /> for the adapter.</summary>
    public IDXGIAdapter1* DxgiAdapter
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _dxgiAdapter;
        }
    }

    /// <summary>Gets the <see cref="DXGI_ADAPTER_DESC1" /> for <see cref="DxgiAdapter" />.</summary>
    public ref readonly DXGI_ADAPTER_DESC1 DxgiAdapterDesc => ref _dxgiAdapterDesc;

    /// <inheritdoc />
    public override string Name => _name;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override uint VendorId => DxgiAdapterDesc.VendorId;

    /// <inheritdoc />
    public override D3D12GraphicsDevice CreateDevice()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsAdapter));
        return new D3D12GraphicsDevice(this);
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
}
