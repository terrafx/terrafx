// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.UI.Providers.Xlib
{
    /// <summary>Represents a graphics surface.</summary>
    public sealed unsafe class GraphicsSurface : IGraphicsSurface
    {
        private readonly Window _window;
        private readonly int _bufferCount;

        internal GraphicsSurface(Window window, int bufferCount)
        {
            Assert(window != null, Resources.ArgumentNullExceptionMessage, nameof(bufferCount));
            Assert(bufferCount > 0, Resources.ArgumentOutOfRangeExceptionMessage, nameof(bufferCount), bufferCount);

            _window = window;
            _bufferCount = bufferCount;
        }

        /// <inheritdoc />
        public event EventHandler<PropertyChangedEventArgs<Vector2>>? SizeChanged
        {
            add
            {
                _window.SizeChanged += value;
            }

            remove
            {
                _window.SizeChanged -= value;
            }
        }

        /// <inheritdoc />
        public int BufferCount => _bufferCount;

        /// <inheritdoc />
        public IntPtr DisplayHandle => (IntPtr)(void*)DispatchProvider.Instance.Display;

        /// <inheritdoc />
        public GraphicsSurfaceKind Kind => GraphicsSurfaceKind.Xlib;

        /// <inheritdoc />
        public Vector2 Size => _window.Bounds.Size;

        /// <inheritdoc />
        public IntPtr WindowHandle => (IntPtr)(void*)_window.Handle;
    }
}
