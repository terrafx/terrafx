// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.ApplicationModel;
using TerraFX.UI;

namespace TerraFX.Samples.ServiceProviders
{
    public class XlibWindowServiceProvider : ApplicationServiceProvider
    {
        private ValueLazy<XlibWindowService> _windowService;

        public XlibWindowServiceProvider()
        {
            _windowService = new ValueLazy<XlibWindowService>(() => new XlibWindowService());
        }

        public override bool TryGetService(Type serviceType, [NotNullWhen(true)] out object? service)
        {
            if (serviceType.IsAssignableFrom(typeof(XlibWindowService)))
            {
                service = _windowService.Value;
                return true;
            }

            service = null;
            return false;
        }

        protected override void DisposeCore(bool isDisposing)
        {
            if (isDisposing)
            {
                _windowService.Dispose(DisposeWindowService);
            }
        }

        private void DisposeWindowService(WindowService windowService)
            => windowService.Dispose();
    }
}
