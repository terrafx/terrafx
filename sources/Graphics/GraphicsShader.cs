// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics
{
    /// <summary>A graphics shader which performs a transformation for a graphics device.</summary>
    public abstract class GraphicsShader : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly string _entryPointName;
        private readonly GraphicsShaderKind _kind;

        /// <summary>Initializes a new instance of the <see cref="GraphicsShader" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device for which the shader was created.</param>
        /// <param name="kind">The shader kind.</param>
        /// <param name="entryPointName">The name of the entry point for the shader.</param>
        /// <exception cref="ArgumentNullException"><paramref name="graphicsDevice" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="entryPointName" /> is <c>null</c>.</exception>
        protected GraphicsShader(GraphicsDevice graphicsDevice, GraphicsShaderKind kind, string entryPointName)
        {
            ThrowIfNull(graphicsDevice, nameof(graphicsDevice));
            ThrowIfNull(entryPointName, nameof(entryPointName));

            _graphicsDevice = graphicsDevice;
            _entryPointName = entryPointName;
            _kind = kind;
        }

        /// <summary>Gets the underlying bytecode for the shader.</summary>
        public abstract ReadOnlySpan<byte> Bytecode { get; }

        /// <summary>Gets the name of the entry point for the shader.</summary>
        public string EntryPointName => _entryPointName;

        /// <summary>Gets the graphics device for which the shader was created.</summary>
        public GraphicsDevice GraphicsDevice => _graphicsDevice;

        /// <summary>Gets the kind of shader.</summary>
        public GraphicsShaderKind Kind => _kind;

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
