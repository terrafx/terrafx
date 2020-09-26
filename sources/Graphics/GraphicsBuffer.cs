// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>A graphics buffer which can hold data for a graphics device.</summary>
    public abstract class GraphicsBuffer : GraphicsResource
    {
        private readonly GraphicsBufferKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsBuffer" /> class.</summary>
        /// <param name="kind">The buffer kind.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the resource.</param>
        /// <param name="memoryBlockRegion">The memory block region in which the resource exists.</param>
        /// <inheritdoc cref="GraphicsResource(GraphicsResourceCpuAccess, in GraphicsMemoryBlockRegion)" />
        protected GraphicsBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, in GraphicsMemoryBlockRegion memoryBlockRegion)
            : base(cpuAccess, in memoryBlockRegion)
        {
            _kind = kind;
        }

        /// <summary>Gets the buffer kind.</summary>
        public GraphicsBufferKind Kind => _kind;
    }
}
