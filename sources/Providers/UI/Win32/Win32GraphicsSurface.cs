// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics;
using TerraFX.Numerics;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.UI.Providers.Win32
{
    /// <summary>Represents a graphics surface.</summary>
    public sealed unsafe class Win32GraphicsSurface : IGraphicsSurface
    {
        private readonly Win32Window _window;
        private readonly int _bufferCount;

        internal Win32GraphicsSurface(Win32Window window, int bufferCount)
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
        public IntPtr DisplayHandle => Win32WindowProvider.EntryPointModule;

        /// <inheritdoc />
        public GraphicsSurfaceKind Kind => GraphicsSurfaceKind.Win32;

        /// <inheritdoc />
        public Vector2 Size => _window.Bounds.Size;

        /// <inheritdoc />
        public IntPtr WindowHandle => _window.Handle;
    }
}
