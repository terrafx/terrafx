// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using TerraFX.Graphics;

namespace TerraFX.Samples.ServiceProviders;

[SupportedOSPlatform("windows10.0.17763.0")]
public sealed class D3D12GraphicsServiceProvider : Win32UIServiceProvider
{
    private ValueLazy<D3D12GraphicsService> _graphicsService;

    public D3D12GraphicsServiceProvider()
    {
        _graphicsService = new ValueLazy<D3D12GraphicsService>(() => new D3D12GraphicsService());
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(D3D12GraphicsService)))
        {
            service = _graphicsService.Value;
            return true;
        }

        return base.TryGetService(serviceType, out service);
    }

    protected override void DisposeCore(bool isDisposing)
    {
        _graphicsService.Dispose(DisposeGraphicsService);
        base.DisposeCore(isDisposing);
    }

    private void DisposeGraphicsService(GraphicsService graphicsService)
        => graphicsService.Dispose();
}
