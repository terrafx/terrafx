// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using TerraFX.ApplicationModel;
using TerraFX.UI;

namespace TerraFX.Samples.ServiceProviders;

[SupportedOSPlatform("windows10.0.17763.0")]
public class Win32UIServiceProvider : ApplicationServiceProvider
{
    private ValueLazy<Win32UIService> _uiService;

    public Win32UIServiceProvider()
    {
        _uiService = new ValueLazy<Win32UIService>(() => Win32UIService.Instance);
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(Win32UIService)))
        {
            service = _uiService.Value;
            return true;
        }

        service = null;
        return false;
    }

    protected override void DisposeCore(bool isDisposing)
    {
        _uiService.Dispose(DisposeUIService);
    }

    private void DisposeUIService(UIService uiService) => uiService.Dispose();
}
