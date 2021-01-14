// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary><see cref="Colors" /> defines standard colors using names and RGB values from https://www.w3.org/TR/css-color-3 .</summary>
    public static class Colors
    {
        /// <summary>Transparent as <see cref="ColorRgba" />.</summary>
        /// <returns>Transparent as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Transparent => new ColorRgba(0, 0, 0, 0);

        /// <summary>Black as <see cref="ColorRgba" />.</summary>
        /// <returns>Black as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Black => new ColorRgba(0, 0, 0, 1);

        /// <summary>Silver as <see cref="ColorRgba" />.</summary>
        /// <returns>Silver as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Silver => new ColorRgba(192 / 255f, 192 / 255f, 192 / 255f, 1f);

        /// <summary>Gray as <see cref="ColorRgba" />.</summary>
        /// <returns>Gray as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Gray => new ColorRgba(128 / 255f, 128 / 255f, 128 / 255f, 1f);

        /// <summary>White as <see cref="ColorRgba" />.</summary>
        /// <returns>White as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba White => new ColorRgba(1, 1, 1, 1);

        /// <summary>Maroon as <see cref="ColorRgba" />.</summary>
        /// <returns>Maroon as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Maroon => new ColorRgba(128 / 255f, 0, 0, 1f);

        /// <summary>Red as <see cref="ColorRgba" />.</summary>
        /// <returns>Red as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Red => new ColorRgba(255 / 255f, 0, 0, 1f);

        /// <summary>Purple as <see cref="ColorRgba" />.</summary>
        /// <returns>Purple as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Purple => new ColorRgba(128 / 255f, 0, 128 / 255f, 1f);

        /// <summary>Fuchsia as <see cref="ColorRgba" />.</summary>
        /// <returns>Fuchsia as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Fuchsia => new ColorRgba(255 / 255f, 0, 255 / 255f, 1f);

        /// <summary>Green as <see cref="ColorRgba" />.</summary>
        /// <returns>Green as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Green => new ColorRgba(0, 128 / 255f, 0, 1f);

        /// <summary>Lime as <see cref="ColorRgba" />.</summary>
        /// <returns>Lime as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Lime => new ColorRgba(0, 255 / 255f, 0, 1f);

        /// <summary>Olive as <see cref="ColorRgba" />.</summary>
        /// <returns>Olive as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Olive => new ColorRgba(128 / 255f, 128 / 255f, 0, 1f);

        /// <summary>Yellow as <see cref="ColorRgba" />.</summary>
        /// <returns>Yellow as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Yellow => new ColorRgba(255 / 255f, 255 / 255f, 0, 1f);

        /// <summary>Navy as <see cref="ColorRgba" />.</summary>
        /// <returns>Navy as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Navy => new ColorRgba(0, 0, 128 / 255f, 1f);

        /// <summary>Blue as <see cref="ColorRgba" />.</summary>
        /// <returns>Blue as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Blue => new ColorRgba(0, 0, 255 / 255f, 1f);

        /// <summary>Teal as <see cref="ColorRgba" />.</summary>
        /// <returns>Teal as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Teal => new ColorRgba(0, 128 / 255f, 128 / 255f, 1f);

        /// <summary>Aqua as <see cref="ColorRgba" />.</summary>
        /// <returns>Aqua as <see cref="ColorRgba" /> instance.</returns>
        public static ColorRgba Aqua => new ColorRgba(0, 128 / 255f, 128 / 255f, 1f);
    }
}
