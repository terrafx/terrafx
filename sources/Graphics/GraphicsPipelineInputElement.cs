// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics pipeline input element which describes an element of an input vertex.</summary>
    public readonly struct GraphicsPipelineInputElement
    {
        private readonly Type _type;
        private readonly GraphicsPipelineInputElementKind _kind;
        private readonly uint _size;

        /// <summary>Creates a new instance of the <see cref="GraphicsPipelineInputElement" /> struct.</summary>
        /// <param name="type">The type of the input element.</param>
        /// <param name="kind">The kind of the input element.</param>
        /// <param name="size">The size, in bytes, of the input element.</param>
        /// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
        public GraphicsPipelineInputElement(Type type, GraphicsPipelineInputElementKind kind, uint size)
        {
            ThrowIfNull(type, nameof(type));

            _type = type;
            _kind = kind;
            _size = size;
        }

        /// <summary>Gets the kind of the input element.</summary>
        public GraphicsPipelineInputElementKind Kind => _kind;

        /// <summary>The size, in bytes, of the input element.</summary>
        public uint Size => _size;

        /// <summary>The type of the input element.</summary>
        public Type Type => _type;
    }
}
