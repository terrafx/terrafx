// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Defines a view into a graphics buffer.</summary>
    public readonly struct GraphicsBufferView
    {
        private readonly ulong _size;
        private readonly GraphicsBuffer _buffer;
        private readonly uint _stride;

        /// <summary>Initializes a new instance of the <see cref="GraphicsBufferView" /> structs.</summary>
        /// <param name="buffer">The buffer for which the view was created.</param>
        /// <param name="size">The size of the view, in bytes.</param>
        /// <param name="stride">The size of elements contained by the view, in bytes.</param>
        public GraphicsBufferView(GraphicsBuffer buffer, ulong size, uint stride)
        {
            ThrowIfNull(buffer, nameof(buffer));

            _size = size;
            _buffer = buffer;
            _stride = stride;
        }

        /// <summary>Gets the buffer for which the view was created.</summary>
        public GraphicsBuffer Buffer => _buffer;

        /// <summary>Gets the size of the view, in bytes.</summary>
        public ulong Size => _size;

        /// <summary>Gets the size of elements contained by the view, in bytes.</summary>
        public uint Stride => _stride;
    }
}
