// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.ApplicationModel;
using TerraFX.UI;

namespace TerraFX.Samples.ServiceProviders;

public class XlibWindowServiceProvider : ApplicationServiceProvider
{
    private ValueLazy<XlibUIService> _uiService;

    public XlibWindowServiceProvider()
    {
        _uiService = new ValueLazy<XlibUIService>(() => XlibUIService.Instance);
    }

    public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
    {
        if (serviceType.IsAssignableFrom(typeof(XlibUIService)))
        {
            service = _uiService.Value;
            return true;
        }

        service = null;
        return false;
    }

    protected override void DisposeCore(bool isDisposing)
    {
        if (isDisposing)
        {
            _uiService.Dispose(DisposeUIService);
        }
    }

    private void DisposeUIService(UIService uiService) => uiService.Dispose();
}
