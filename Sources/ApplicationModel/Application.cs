// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading;
using TerraFX.Threading;
using TerraFX.UI;
using TerraFX.Utilities;

namespace TerraFX.ApplicationModel
{
    /// <summary>Represents a multimedia-based application.</summary>
    public sealed class Application : IDisposable
    {
        #region Fields
        private readonly CompositionHost _compositionHost;
        private readonly Lazy<IDispatchManager> _dispatchManager;
        private readonly Lazy<IWindowManager> _windowManager;

        private volatile int _isRunning;
        private bool _exitRequested;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Application" /> class.</summary>
        /// <param name="compositionAssemblies">The set of <see cref="Assembly" /> instances to search for type exports.</param>
        /// <exception cref="ArgumentNullException"><paramref name="compositionAssemblies" /> is <c>null</c>.</exception>
        public Application(IEnumerable<Assembly> compositionAssemblies)
        {
            if (compositionAssemblies is null)
            {
                ExceptionUtilities.ThrowArgumentNullException(nameof(compositionAssemblies));
            }

            var containerConfiguration = new ContainerConfiguration();
            {
                containerConfiguration = containerConfiguration.WithAssemblies(compositionAssemblies);
            }
            _compositionHost = containerConfiguration.CreateContainer();

            _dispatchManager = new Lazy<IDispatchManager>(_compositionHost.GetExport<IDispatchManager>, isThreadSafe: true);
            _windowManager = new Lazy<IWindowManager>(_compositionHost.GetExport<IWindowManager>, isThreadSafe: true);
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="Application" /> class.</summary>
        ~Application()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Properties
        /// <summary>Gets the <see cref="CompositionHost" /> for the current instance.</summary>
        public CompositionHost CompositionHost
        {
            get
            {
                return _compositionHost;
            }
        }

        /// <summary>Gets the <see cref="IDispatchManager" /> for the current instance.</summary>
        public IDispatchManager DispatchManager
        {
            get
            {
                return _dispatchManager.Value;
            }
        }

        /// <summary>Gets a value that indicates whether the current instance is running.</summary>
        public bool IsRunning
        {
            get
            {
                return (_isRunning != 0);
            }
        }

        /// <summary>Gets the <see cref="IWindowManager" /> for the current instance.</summary>
        public IWindowManager WindowManager
        {
            get
            {
                return _windowManager.Value;
            }
        }
        #endregion

        #region Methods
        /// <summary>Requests that the current instance exits the event loop.</summary>
        /// <remarks>This method does nothing if <see cref="IsRunning" /> is <c>false</c>.</remarks>
        public void RequestExit()
        {
            if (IsRunning)
            {
                _exitRequested = true;
            }
        }

        /// <summary>Runs the event loop for the current instance.</summary>
        /// <exception cref="InvalidOperationException">The current instance is already running.</exception>
        /// <exception cref="InvalidOperationException">The <see cref="ApartmentState" /> for the <see cref="Thread.CurrentThread" /> is not <see cref="ApartmentState.STA"/>.</exception>
        public void Run()
        {
            if (Interlocked.Exchange(ref _isRunning, 1) != 0)
            {
                ExceptionUtilities.ThrowInvalidOperationException(nameof(IsRunning), true);
            }

            var currentApartmentState = Thread.CurrentThread.GetApartmentState();

            if (currentApartmentState != ApartmentState.STA)
            {
                ExceptionUtilities.ThrowInvalidOperationException(nameof(currentApartmentState), currentApartmentState);
            }

            var currentDispatcher = DispatchManager.DispatcherForCurrentThread;

            while (_exitRequested == false)
            {
                currentDispatcher.DispatchPending();
            }

            _exitRequested = false;
            _isRunning = 0;
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _compositionHost?.Dispose();
            }
        }
        #endregion

        #region System.IDisposable
        /// <summary>Disposes of any unmanaged resources owned by the current instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
