// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using TerraFX.ApplicationModel;

namespace TerraFX.Samples
{
    public abstract class Sample
    {
        private static readonly Assembly s_uiProviderWin32 = Assembly.LoadFrom("TerraFX.UI.Providers.Win32.dll");
        private static readonly Assembly s_uiProviderXlib = Assembly.LoadFrom("TerraFX.UI.Providers.Xlib.dll");

        private readonly string _name;
        private readonly Assembly[] _compositionAssemblies;

        protected Sample(string name, Assembly[] compositionAssemblies)
        {
            _name = name;

            _compositionAssemblies = new Assembly[compositionAssemblies.Length + 1];
            _compositionAssemblies[0] = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? s_uiProviderWin32 : s_uiProviderXlib;

            Array.Copy(compositionAssemblies, 0, _compositionAssemblies, 1, compositionAssemblies.Length);
        }

        public Assembly[] CompositionAssemblies => _compositionAssemblies;

        public string Name => _name;

        public virtual void Cleanup()
        {
        }

        public virtual void Initialize(Application application) => application.Idle += OnIdle;

        protected abstract void OnIdle(object? sender, ApplicationIdleEventArgs eventArgs);
    }
}
