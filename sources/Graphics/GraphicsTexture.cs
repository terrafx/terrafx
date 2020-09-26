// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>A graphics texture which can hold data for a graphics device.</summary>
    public abstract class GraphicsTexture : GraphicsResource
    {
        private readonly GraphicsTextureKind _kind;
        private readonly uint _width;
        private readonly uint _height;
        private readonly ushort _depth;

        /// <summary>Initializes a new instance of the <see cref="GraphicsTexture" /> class.</summary>
        /// <param name="kind">The texture kind.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
        /// <param name="memoryBlockRegion">The memory block region in which the resource exists.</param>
        /// <param name="width">The width, in pixels, of the graphics texture.</param>
        /// <param name="height">The height, in pixels, of the graphics texture.</param>
        /// <param name="depth">The depth, in pixels, of the graphics texture.</param>
        /// <inheritdoc cref="GraphicsResource(GraphicsResourceCpuAccess, in GraphicsMemoryBlockRegion)" />
        protected GraphicsTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, in GraphicsMemoryBlockRegion memoryBlockRegion, uint width, uint height, ushort depth)
            : base(cpuAccess, in memoryBlockRegion)
        {
            _kind = kind;
            _width = width;
            _height = height;
            _depth = depth;
        }

        /// <summary>Gets the depth, in pixels, of the graphics texture.</summary>
        public ushort Depth => _depth;

        /// <summary>Gets the texture kind.</summary>
        public GraphicsTextureKind Kind => _kind;

        /// <summary>Gets the height, in pixels, of the graphics texture.</summary>
        public uint Height => _height;

        /// <summary>Gets the width, in pixels, of the graphics texture.</summary>
        public uint Width => _width;
    }
}
