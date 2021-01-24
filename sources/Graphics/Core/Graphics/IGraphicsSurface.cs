// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Numerics;

namespace TerraFX.Graphics
{
    /// <summary>A graphics surface which can be rendered on by a graphics device.</summary>
    public interface IGraphicsSurface : IDisposable
    {
        /// <summary>Occurs when the <see cref="Size" /> property changes.</summary>
        event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        /// <summary>Gets the height of the surface.</summary>
        float Height => Size.Y;

        /// <summary>Gets the width of the surface.</summary>
        float Width => Size.X;

        /// <summary>Gets a context handle for the surface.</summary>
        /// <exception cref="ObjectDisposedException">The surface has been disposed.</exception>
        IntPtr ContextHandle { get; }

        /// <summary>Gets a handle for the surface.</summary>
        /// <exception cref="ObjectDisposedException">The surface has been disposed.</exception>
        IntPtr Handle { get; }

        /// <summary>Gets the surface kind.</summary>
        GraphicsSurfaceKind Kind { get; }

        /// <summary>Gets the size of the surface.</summary>
        Vector2 Size { get; }
    }
}
