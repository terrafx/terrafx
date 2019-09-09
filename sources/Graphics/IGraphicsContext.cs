// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>Represents a graphics context, which can be used for rendering images.</summary>
    public interface IGraphicsContext
    {
        /// <summary>Gets the <see cref="IGraphicsAdapter" /> for the instance.</summary>
        IGraphicsAdapter GraphicsAdapter { get; }

        /// <summary>Gets the <see cref="IGraphicsSurface" /> for the instance.</summary>
        IGraphicsSurface GraphicsSurface { get; }

        /// <summary>Begins a new frame for rendering.</summary>
        /// <param name="backgroundColor">A color to which the background should be cleared.</param>
        void BeginFrame(ColorRgba backgroundColor);

        /// <summary>Ends the frame currently be rendered.</summary>
        void EndFrame();

        /// <summary>Presents the last frame rendered.</summary>
        void PresentFrame();
    }
}
