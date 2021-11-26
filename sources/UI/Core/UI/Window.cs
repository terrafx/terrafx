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
        private readonly WindowService _windowService;
        private readonly Thread _parentThread;

        /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
        /// <param name="windowService">The window service which created the window.</param>
        /// <param name="parentThread">The thread on which window operates.</param>
        /// <exception cref="ArgumentNullException"><paramref name="windowService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="parentThread" /> is <c>null</c>.</exception>
        protected Window(WindowService windowService, Thread parentThread)
        {
            ThrowIfNull(windowService);
            ThrowIfNull(parentThread);

            _windowService = windowService;
            _parentThread = parentThread;
        }

        /// <summary>Occurs when the <see cref="ClientLocation" /> property changes.</summary>
        public abstract event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientLocationChanged;

        /// <summary>Occurs when the <see cref="ClientSize" /> property changes.</summary>
        public abstract event EventHandler<PropertyChangedEventArgs<Vector2>>? ClientSizeChanged;

        /// <summary>Occurs when the <see cref="Location" /> property changes.</summary>
        public abstract event EventHandler<PropertyChangedEventArgs<Vector2>>? LocationChanged;

        /// <summary>Occurs when the <see cref="Size" /> property changes.</summary>
        public abstract event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        event EventHandler<PropertyChangedEventArgs<Vector2>>? IGraphicsSurface.SizeChanged
        {
            add
            {
                ClientSizeChanged += value;
            }

            remove
            {
                ClientSizeChanged -= value;
            }
        }

        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the window.</summary>
        public abstract Rectangle Bounds { get; }

        /// <summary>Gets a <see cref="Rectangle" /> that represents the bounds of the client area for the window.</summary>
        public abstract Rectangle ClientBounds { get; }

        /// <summary>Gets a <see cref="Vector2" /> that represents the location of the client area for the window.</summary>
        public Vector2 ClientLocation => ClientBounds.Location;

        /// <summary>Gets a <see cref="Vector2" /> that represents the size of the client area for the window.</summary>
        public Vector2 ClientSize => ClientBounds.Size;

        /// <summary>Gets <see cref="FlowDirection" /> for the window.</summary>
        public abstract FlowDirection FlowDirection { get; }

        /// <summary>Gets a value that indicates whether the window is the active window.</summary>
        public abstract bool IsActive { get; }

        /// <summary>Gets a value that indicates whether the window is enabled.</summary>
        public abstract bool IsEnabled { get; }

        /// <summary>Gets a value that indicates whether the window is visible.</summary>
        public abstract bool IsVisible { get; }

        /// <summary>Gets a <see cref="Vector2" /> that represents the location of the window.</summary>
        public Vector2 Location => Bounds.Location;

        /// <summary>Gets the <see cref="Thread" /> that was used to create the window.</summary>
        public Thread ParentThread => _parentThread;

        /// <summary>Gets the <see cref="IPropertySet" /> for the window.</summary>
        public abstract IPropertySet Properties { get; }

        /// <summary>Gets the <see cref="ReadingDirection" /> for the window.</summary>
        public abstract ReadingDirection ReadingDirection { get; }

        /// <summary>Gets a <see cref="Vector2" /> that represents the size of the window.</summary>
        public Vector2 Size => Bounds.Size;

        /// <summary>Gets the title for the window.</summary>
        public abstract string Title { get; }

        /// <summary>Gets the <see cref="UI.WindowService" /> for the window.</summary>
        public WindowService WindowService => _windowService;

        /// <summary>Gets the <see cref="WindowState" /> for the window.</summary>
        public abstract WindowState WindowState { get; }

        /// <inheritdoc cref="IGraphicsSurface.ContextHandle" />
        protected abstract IntPtr SurfaceContextHandle { get; }

        /// <inheritdoc cref="IGraphicsSurface.Handle" />
        protected abstract IntPtr SurfaceHandle { get; }

        /// <inheritdoc cref="IGraphicsSurface.Kind" />
        protected abstract GraphicsSurfaceKind SurfaceKind { get; }

        IntPtr IGraphicsSurface.ContextHandle => SurfaceContextHandle;

        IntPtr IGraphicsSurface.Handle => SurfaceHandle;

        GraphicsSurfaceKind IGraphicsSurface.Kind => SurfaceKind;

        Vector2 IGraphicsSurface.Size => ClientSize;

        /// <summary>Activates the window.</summary>
        public abstract void Activate();

        /// <summary>Closes the window.</summary>
        public abstract void Close();

        /// <summary>Disables the window.</summary>
        public abstract void Disable();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Enables the window.</summary>
        public abstract void Enable();

        /// <summary>Hides the window.</summary>
        public abstract void Hide();

        /// <summary>Maximizes the window.</summary>
        public abstract void Maximize();

        /// <summary>Minimizes the window.</summary>
        public abstract void Minimize();

        /// <summary>Relocates the window to the specified location.</summary>
        /// <param name="location">The new location for the window.</param>
        public abstract void Relocate(Vector2 location);

        /// <summary>Relocates the client area for the window to the specified location.</summary>
        /// <param name="location">The new location for the client area of the window.</param>
        public abstract void RelocateClient(Vector2 location);

        /// <summary>Resizes the window to the specified size.</summary>
        /// <param name="size">The new size for the window.</param>
        public abstract void Resize(Vector2 size);

        /// <summary>Resizes the client area for the window to the specified size.</summary>
        /// <param name="size">The new size for the client area of the window.</param>
        public abstract void ResizeClient(Vector2 size);

        /// <summary>Restores the window.</summary>
        public abstract void Restore();

        /// <summary>Sets the title to the specified value.</summary>
        /// <param name="title">The new title for the window.</param>
        public abstract void SetTitle(string title);

        /// <summary>Shows the window.</summary>
        public abstract void Show();

        /// <summary>Tries to activate the window.</summary>
        /// <returns><c>true</c> if the window was succesfully activated; otherwise, <c>false</c>.</returns>
        public abstract bool TryActivate();

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
