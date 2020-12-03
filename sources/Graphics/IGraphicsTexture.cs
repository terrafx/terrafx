// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>A graphics texture which can hold data for a graphics device.</summary>
    public interface IGraphicsTexture : IGraphicsResource
    {
        /// <summary>Gets the depth, in pixels, of the graphics texture.</summary>
        ushort Depth { get; }

        /// <summary>Gets the height, in pixels, of the graphics texture.</summary>
        uint Height { get; }

        /// <summary>Gets the texture kind.</summary>
        GraphicsTextureKind Kind { get; }

        /// <summary>Gets the width, in pixels, of the graphics texture.</summary>
        uint Width { get; }
    }
}
