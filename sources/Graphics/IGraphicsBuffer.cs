// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>A graphics buffer which can hold data for a graphics device.</summary>
    public interface IGraphicsBuffer : IGraphicsResource
    {
        /// <summary>Gets the buffer kind.</summary>
        GraphicsBufferKind Kind { get; }
    }
}
