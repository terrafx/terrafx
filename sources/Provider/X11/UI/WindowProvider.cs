// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.X11;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.X11.UI
{
    /// <summary>Provides access to an X11 based window subsystem.</summary>
    [Export(typeof(IWindowProvider))]
    [Export(typeof(WindowProvider))]
    [Shared]
    public sealed unsafe class WindowProvider : IDisposable, IWindowProvider
    {
        private const int False = 0;
        private const int Success = 0;

        private readonly Lazy<DispatchProvider> _dispatchProvider;
        private readonly ConcurrentDictionary<IntPtr, Window> _windows;

        private State _state;

        /// <summary>Initializes a new instance of the <see cref="WindowProvider" /> class.</summary>
        [ImportingConstructor]
        public WindowProvider(
            [Import] Lazy<DispatchProvider> dispatchProvider
        )
        {
            _dispatchProvider = dispatchProvider;
            _windows = new ConcurrentDictionary<IntPtr, Window>();
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="WindowProvider" /> class.</summary>
        ~WindowProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="DispatchProvider" /> for the instance.</summary>
        public DispatchProvider DispatchProvider
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _dispatchProvider.Value;
            }
        }

        /// <summary>Gets the handle for the instance.</summary>
        public IntPtr Handle => DispatchProvider.Display;

        /// <summary>Gets the <see cref="IWindow" /> objects created by the instance.</summary>
        public IEnumerable<IWindow> Windows => _state.IsNotDisposedOrDisposing ? (IEnumerable<IWindow>)_windows : Array.Empty<IWindow>();

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Create a new <see cref="IWindow" /> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var window = new Window(this);
            _ = _windows.TryAdd(window.Handle, window);

            return window;
        }

        internal static void ForwardWindowEvent(UIntPtr windowProviderProperty, in XEvent xevent)
        {
            byte* prop;

            var result = XGetWindowProperty(
                xevent.xany.display,
                xevent.xany.window,
                windowProviderProperty,
                IntPtr.Zero,
                (IntPtr)(IntPtr.Size >> 2),
                False,
                (UIntPtr)32,
                null,
                null,
                null,
                null,
                &prop
            );

            if (result != Success)
            {
                ThrowExternalException(nameof(XGetWindowProperty), result);
            }

            var windowProvider = (WindowProvider)GCHandle.FromIntPtr((IntPtr)prop).Target!;

            if (windowProvider._windows.TryGetValue((IntPtr)(void*)xevent.xany.window, out var window))
            {
                window.ProcessWindowEvent(in xevent);
            }
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeWindows(isDisposing);
            }

            _state.EndDispose();
        }

        private void DisposeWindows(bool isDisposing)
        {
            _state.AssertDisposing();

            if (isDisposing)
            {
                foreach (var windowHandle in _windows.Keys)
                {
                    if (_windows.TryRemove(windowHandle, out var createdWindow))
                    {
                        createdWindow.Dispose();
                    }
                }
            }
            else
            {
                _windows.Clear();
            }

            Assert(_windows.IsEmpty, Resources.ArgumentOutOfRangeExceptionMessage, nameof(_windows.IsEmpty), _windows.IsEmpty);
        }
    }
}
