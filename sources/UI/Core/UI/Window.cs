// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Numerics;

namespace TerraFX.UI;

/// <summary>Defines a window.</summary>
public abstract class Window : UIDispatcherObject, IGraphicsSurface
{
    /// <summary>Initializes a new instance of the <see cref="Window" /> class.</summary>
    /// <param name="dispatcher">The dispatcher for which the window is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dispatcher" /> is <c>null</c>.</exception>
    protected Window(UIDispatcher dispatcher)
        : base(dispatcher)
    {
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

    /// <summary>Gets the bounds of the window.</summary>
    public abstract BoundingRectangle Bounds { get; }

    /// <summary>Gets the bounds of the client area for the window.</summary>
    public abstract BoundingRectangle ClientBounds { get; }

    /// <summary>Gets the location of the client area for the window.</summary>
    public Vector2 ClientLocation => ClientBounds.Location;

    /// <summary>Gets the size of the client area for the window.</summary>
    public Vector2 ClientSize => ClientBounds.Size;

    /// <summary>Gets flow direction for the window.</summary>
    public abstract FlowDirection FlowDirection { get; }

    /// <summary>Gets <c>true</c> if the window is active; otherwise, <c>false</c>.</summary>
    public abstract bool IsActive { get; }

    /// <summary>Gets <c>true</c> if the window is enabled; otherwise, <c>false</c>.</summary>
    public abstract bool IsEnabled { get; }

    /// <summary>Gets <c>true</c> if the window is visible; otherwise, <c>false</c>.</summary>
    public abstract bool IsVisible { get; }

    /// <summary>Gets the location of the window.</summary>
    public Vector2 Location => Bounds.Location;

    /// <summary>Gets the reading directionfor the window.</summary>
    public abstract ReadingDirection ReadingDirection { get; }

    /// <summary>Gets the size of the window.</summary>
    public Vector2 Size => Bounds.Size;

    /// <summary>Gets the title for the window.</summary>
    public abstract string Title { get; }

    /// <summary>Gets the state for the window.</summary>
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
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Activate();

    /// <summary>Closes the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Close();

    /// <summary>Disables the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Disable();

    /// <summary>Enables the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Enable();

    /// <summary>Hides the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Hide();

    /// <summary>Maximizes the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Maximize();

    /// <summary>Minimizes the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Minimize();

    /// <summary>Relocates the window to the specified location.</summary>
    /// <param name="location">The new location for the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Relocate(Vector2 location);

    /// <summary>Relocates the client area for the window to the specified location.</summary>
    /// <param name="location">The new location for the client area of the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void RelocateClient(Vector2 location);

    /// <summary>Resizes the window to the specified size.</summary>
    /// <param name="size">The new size for the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Resize(Vector2 size);

    /// <summary>Resizes the client area for the window to the specified size.</summary>
    /// <param name="size">The new size for the client area of the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void ResizeClient(Vector2 size);

    /// <summary>Restores the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Restore();

    /// <summary>Sets the title to the specified value.</summary>
    /// <param name="title">The new title for the window.</param>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void SetTitle(string title);

    /// <summary>Shows the window.</summary>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract void Show();

    /// <summary>Tries to activate the window.</summary>
    /// <returns><c>true</c> if the window was succesfully activated; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The window has been disposed.</exception>
    public abstract bool TryActivate();
}
