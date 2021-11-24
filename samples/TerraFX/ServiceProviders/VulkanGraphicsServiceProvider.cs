// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.ApplicationModel;
using TerraFX.Graphics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Samples.ServiceProviders;

public sealed class VulkanGraphicsServiceProvider : ApplicationServiceProvider
{
    private readonly ApplicationServiceProvider _baseServiceProvider;
    private ValueLazy<VulkanGraphicsService> _graphicsService;

    public VulkanGraphicsServiceProvider(ApplicationServiceProvider baseServiceProvider)
    {
        ThrowIfNull(baseServiceProvider, nameof(baseServiceProvider));
        _baseServiceProvider = baseServiceProvider;
        _graphicsService = new ValueLazy<VulkanGraphicsService>(() => new VulkanGraphicsService());
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(VulkanGraphicsService)))
        {
            service = _graphicsService.Value;
            return true;
        }

        return _baseServiceProvider.TryGetService(serviceType, out service);
    }

    protected override void DisposeCore(bool isDisposing)
    {
        if (isDisposing)
        {
            _graphicsService.Dispose(DisposeGraphicsService);
            _baseServiceProvider?.Dispose();
        }
    }

    private void DisposeGraphicsService(GraphicsService graphicsService)
        => graphicsService.Dispose();
}
