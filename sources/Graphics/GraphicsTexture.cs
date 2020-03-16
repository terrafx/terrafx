// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <summary>A graphics texture which can hold data for a graphics device.</summary>
    public abstract class GraphicsTexture : GraphicsResource
    {
        private readonly ulong _width;
        private readonly uint _height;
        private readonly ushort  _depth;
        private readonly GraphicsTextureKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
        /// <param name="kind">The texture kind.</param>
        /// <param name="graphicsHeap">The graphics heap on which the texture was created.</param>
        /// <param name="offset">The offset, in bytes, of the texture in relation to <paramref name="graphicsHeap" />.</param>
        /// <param name="size">The size, in bytes, of the texture.</param>
        /// <param name="width">The width, in pixels, of the texture.</param>
        /// <param name="height">The height, in pixels, of the texture.</param>
        /// <param name="depth">The depth, in pixels, of the texture.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsHeap" /> is <c>null</c>.</exception>
        protected GraphicsTexture(GraphicsTextureKind kind, GraphicsHeap graphicsHeap, ulong offset, ulong size, ulong width, uint height, ushort depth)
            : base(graphicsHeap, offset, size)
        {
            _width = width;
            _height = height;
            _depth = depth;
            _kind = kind;
        }

        /// <summary>Gets the depth, in bytes, of the texture.</summary>
        public ushort Depth => _depth;

        /// <summary>Gets the height, in bytes, of the texture.</summary>
        public uint Height => _height;

        /// <summary>Gets the kind of texture.</summary>
        public GraphicsTextureKind Kind => _kind;

        /// <summary>Gets the width, in bytes, of the texture.</summary>
        public ulong Width => _width;
    }
}
