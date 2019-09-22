// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Numerics;

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics surface.</summary>
    public interface IGraphicsSurface
    {
        /// <summary>Occurs when the <see cref="Size" /> property changes.</summary>
        event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged;

        /// <summary>Gets the height of the instance.</summary>
        public float Height => Size.Y;

        /// <summary>Gets the width of the instance.</summary>
        public float Width => Size.X;

        /// <summary>Gets the number of buffers for the instance.</summary>
        int BufferCount { get; }

        /// <summary>Gets the display handle for the instance.</summary>
        IntPtr DisplayHandle { get; }

        /// <summary>Gets the kind of surface represented by the instance.</summary>
        GraphicsSurfaceKind Kind { get; }

        /// <summary>Gets the size of the instance.</summary>
        Vector2 Size { get; }

        /// <summary>Gets the window handle for the instance.</summary>
        IntPtr WindowHandle { get; }
    }
}
