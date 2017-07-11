// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from X11\xlib.h in the Xlib - C Language X Interface: X Version 11, Release 7.7
// Original source is Copyright © The Open Group.

namespace TerraFX.Interop
{
    public /* blittable */ struct XPixmapFormatValues
    {
        #region Fields
        public int depth;

        public int bits_per_pixel;

        public int scanline_pad;
        #endregion
    }
}
