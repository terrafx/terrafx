// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Numerics;

namespace TerraFX.Provider.X11.UI
{
    /// <summary>Represents a graphics surface.</summary>
    public sealed unsafe class GraphicsSurface : IGraphicsSurface
    {
        private readonly Window _window;
        private readonly Vector2 _size;

        internal GraphicsSurface(Window window, Vector2 size)
        {
            _window = window;
            _size = size;
        }

        /// <summary>Gets the kind of surface represented by the instance.</summary>
        public GraphicsSurfaceKind Kind => GraphicsSurfaceKind.Win32;

        /// <summary>Gets the size of the instance.</summary>
        public Vector2 Size => _size;

        /// <summary>Gets the window provider handle for the instance.</summary>
        public IntPtr WindowProviderHandle => _window.WindowProvider.Handle;

        /// <summary>Gets the window handle for the instance.</summary>
        public IntPtr WindowHandle => _window.Handle;
    }
}
