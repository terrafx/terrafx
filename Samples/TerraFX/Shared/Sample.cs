// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using TerraFX.ApplicationModel;

namespace TerraFX.Samples
{
    public abstract class Sample : IDisposable
    {
        #region Default Providers
        private static readonly Assembly Win32Provider = Assembly.LoadFrom("TerraFX.Provider.Win32.dll");
        private static readonly Assembly libX11Provider = Assembly.LoadFrom("TerraFX.Provider.libX11.dll");
        #endregion

        #region Fields
        private string _name;
        private List<Assembly> _compositionAssemblies;
        #endregion

        #region Constructors
        protected Sample(string name, IEnumerable<Assembly> compositionAssemblies)
        {
            _name = name;
            _compositionAssemblies = new List<Assembly>(compositionAssemblies);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _compositionAssemblies.Add(Win32Provider);
            }
            else
            {
                _compositionAssemblies.Add(libX11Provider);
            }
        }
        #endregion

        #region Destructors
        ~Sample()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Properties
        public IEnumerable<Assembly> CompositionAssemblies
        {
            get
            {
                return _compositionAssemblies;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }
        #endregion

        #region Methods
        public abstract void OnIdle(object sender, IdleEventArgs eventArgs);

        protected virtual void Dispose(bool isDisposing)
        {
        }
        #endregion

        #region System.IDisposable Methods
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
