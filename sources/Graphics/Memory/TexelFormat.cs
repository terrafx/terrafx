// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics
{

    /// <summary>
    /// TexelFormat specifies the format of the texels in a texture.
    /// </summary>
    public enum TexelFormat
    {
        /// <summary>Defines a RGBA one byte each TexelFormat.</summary>
        RGBA4xByte,

        /// <summary>Defines a single channel signed Int16 TexelFormat.</summary>
        XSInt16,
    }
}
