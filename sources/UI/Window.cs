// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Collections;
using TerraFX.Graphics;
using TerraFX.Graphics.Geometry2D;
using TerraFX.Numerics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI
{
    /// <summary>Defines a window.</summary>
    public abstract class Window : IGraphicsSurface
    {
        private readonly WindowProvider _windowProvider;
        private readonly Thread _parentThread;

        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        /// <param name="windowProvider">The window provider which created the window.</param>
        /// <param name="parentThread">The thread on which window operates.</param>
        /// <exception cref="ArgumentNullException"><paramref name="windowProvider" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="parentThread" /> is <c>null</c>.</exception>
        protected Window(WindowProvider windowProvider, Thread parentThread)
        {
            ThrowIfNull(windowProvider, nameof(windowProvider));
            ThrowIfNull(parentThread, nameof(parentThread));

            _windowProvider = windowProvider;
            _parentThread = parentThread;
        }

        /// <summary>Occurs when the <see cref="Location" /> property changes.</summary>
        public abstract event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

        /// <inheritdoc />
        public abstract event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the instance.</summary>
        public abstract Rectangle Bounds { get; }

        /// <summary>Gets <see cref="FlowDirection" /> for the instance.</summary>
        public abstract FlowDirection FlowDirection { get; }

        /// <summary>Gets a value that indicates whether the instance is the active window.</summary>
        public abstract bool IsActive { get; }

        /// <summary>Gets a value that indicates whether the instance is enabled.</summary>
        public abstract bool IsEnabled { get; }

        /// <summary>Gets a value that indicates whether the instance is visible.</summary>
        public abstract bool IsVisible { get; }

        /// <summary>Gets a <see cref="Vector2" /> that represents the location of the instance.</summary>
        public Vector2 Location => Bounds.Location;

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread => _parentThread;

        /// <summary>Gets the <see cref="IPropertySet" /> for the instance.</summary>
        public abstract IPropertySet Properties { get; }

        /// <summary>Gets the <see cref="ReadingDirection" /> for the instance.</summary>
        public abstract ReadingDirection ReadingDirection { get; }

        /// <inheritdoc />
        public Vector2 Size => Bounds.Size;

        /// <inheritdoc />
        public abstract IntPtr SurfaceContextHandle { get; }

        /// <inheritdoc />
        public abstract IntPtr SurfaceHandle { get; }

        /// <inheritdoc />
        public abstract GraphicsSurfaceKind SurfaceKind { get; }

        /// <summary>Gets the title for the instance.</summary>
        public abstract string Title { get; }

        /// <summary>Gets the <see cref="UI.WindowProvider" /> for the instance.</summary>
        public WindowProvider WindowProvider => _windowProvider;

        /// <summary>Gets the <see cref="WindowState" /> for the instance.</summary>
        public abstract WindowState WindowState { get; }

        /// <summary>Activates the instance.</summary>
        public abstract void Activate();

        /// <summary>Closes the instance.</summary>
        public abstract void Close();

        /// <summary>Disables the instance.</summary>
        public abstract void Disable();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Enables the instance.</summary>
        public abstract void Enable();

        /// <summary>Hides the instance.</summary>
        public abstract void Hide();

        /// <summary>Maximizes the instance.</summary>
        public abstract void Maximize();

        /// <summary>Minimizes the instance.</summary>
        public abstract void Minimize();

        /// <summary>Restores the instance.</summary>
        public abstract void Restore();

        /// <summary>Shows the instance.</summary>
        public abstract void Show();

        /// <summary>Tries to activate the instance.</summary>
        /// <returns><c>true</c> if the instance was succesfully activated; otherwise, <c>false</c>.</returns>
        public abstract bool TryActivate();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
