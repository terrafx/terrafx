// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.ApplicationModel;
using TerraFX.UI;

namespace TerraFX.Samples.ServiceProviders;

public class Win32WindowServiceProvider : ApplicationServiceProvider
{
    private ValueLazy<Win32WindowService> _windowService;

    public Win32WindowServiceProvider()
    {
        _windowService = new ValueLazy<Win32WindowService>(() => new Win32WindowService());
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(Win32WindowService)))
        {
            service = _windowService.Value;
            return true;
        }

        service = null;
        return false;
    }

    protected override void DisposeCore(bool isDisposing)
    {
        _windowService.Dispose(DisposeWindowService);
    }

    private void DisposeWindowService(WindowService windowService)
        => windowService.Dispose();
}
