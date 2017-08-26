// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Composition;
using System.Diagnostics;
using TerraFX.Provider.libX11.Threading;
using TerraFX.UI;
using GC = System.GC;
using XWindow = TerraFX.Interop.Window;

namespace TerraFX.Provider.libX11.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    [Export(typeof(IWindowManager))]
    [Export(typeof(WindowManager))]
    [Shared]
    public sealed unsafe class WindowManager : IDisposable, IWindowManager
    {
        #region Static Fields
        internal static readonly ConcurrentDictionary<XWindow, Window> CreatedWindows = new ConcurrentDictionary<XWindow, Window>();
        #endregion

        #region Fields
        internal readonly Lazy<DispatchManager> _dispatchManager;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WindowManager" /> class.</summary>
        [ImportingConstructor]
        public WindowManager(
            [Import] Lazy<DispatchManager> dispatchManager
        )
        {
            _dispatchManager = dispatchManager;
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="WindowManager" /> class.</summary>
        ~WindowManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Static Methods
        internal static void DisposeCreatedWindows()
        {
            foreach (var createdWindowHandle in CreatedWindows.Keys)
            {
                if (CreatedWindows.TryRemove(createdWindowHandle, out var createdWindow))
                {
                    createdWindow.Dispose();
                }
            }
        }
        #endregion

        #region Methods
        internal void DestroyWindow(XWindow windowHandle)
        {
            CreatedWindows.TryRemove(windowHandle, out _);
        }

        internal void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                DisposeCreatedWindows();
            }
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region TerraFX.UI.IWindowManager Methods
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        public IWindow CreateWindow()
        {
            var window = new Window(this, _dispatchManager.Value);

            var succeeded = CreatedWindows.TryAdd(window._handle, window);
            Debug.Assert(succeeded);

            return window;
        }
        #endregion
    }
}
