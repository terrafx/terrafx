// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics shader which performs a transformation for a graphics device.</summary>
    public abstract class GraphicsShader : GraphicsDeviceObject
    {
        private readonly string _entryPointName;
        private readonly GraphicsShaderKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsShader" /> class.</summary>
        /// <param name="device">The device for which the shader was created.</param>
        /// <param name="kind">The shader kind.</param>
        /// <param name="entryPointName">The name of the entry point for the shader.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
        protected GraphicsShader(GraphicsDevice device, GraphicsShaderKind kind, string entryPointName)
            : base(device)
        {
            ThrowIfNull(entryPointName, nameof(entryPointName));

            _entryPointName = entryPointName;
            _kind = kind;
        }

        /// <summary>Gets the underlying bytecode for the shader.</summary>
        public abstract ReadOnlySpan<byte> Bytecode { get; }

        /// <summary>Gets the name of the entry point for the shader.</summary>
        public string EntryPointName => _entryPointName;

        /// <summary>Gets the kind of shader.</summary>
        public GraphicsShaderKind Kind => _kind;
    }
}
