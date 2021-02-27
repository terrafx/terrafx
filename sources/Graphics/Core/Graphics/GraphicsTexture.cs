// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{
    /// <inheritdoc />
    public abstract unsafe class GraphicsTexture : GraphicsResource
    {
        private readonly GraphicsTextureKind _kind;
        private readonly uint _width;
        private readonly uint _height;
        private readonly ushort _depth;

        /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
        /// <param name="device">The device for which the texture was created.</param>
        /// <param name="kind">The texture kind.</param>
        /// <param name="blockRegion">The memory block region in which the resource exists.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
        /// <param name="width">The width, in pixels, of the graphics texture.</param>
        /// <param name="height">The height, in pixels, of the graphics texture.</param>
        /// <param name="depth">The depth, in pixels, of the graphics texture.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c></exception>
        /// <exception cref="ArgumentNullException"><paramref name="blockRegion" />.<see cref="GraphicsMemoryRegion{TCollection}.Collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockRegion" /> was not created for <paramref name="device" />.</exception>
        protected GraphicsTexture(GraphicsDevice device, GraphicsTextureKind kind, in GraphicsMemoryRegion<GraphicsMemoryBlock> blockRegion, GraphicsResourceCpuAccess cpuAccess, uint width, uint height, ushort depth)
            : base(device, in blockRegion, cpuAccess)
        {
            _kind = kind;
            _width = width;
            _height = height;
            _depth = depth;
        }

        /// <inheritdoc />
        public ushort Depth => _depth;

        /// <inheritdoc />
        public uint Height => _height;

        /// <inheritdoc />
        public GraphicsTextureKind Kind => _kind;

        /// <inheritdoc />
        public uint Width => _width;
    }
}
