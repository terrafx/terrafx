// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Geometry enclosing of text positions.</summary>
    public /* blittable */ struct DWRITE_HIT_TEST_METRICS
    {
        #region Fields
        /// <summary>First text position within the geometry.</summary>
        public UINT32 textPosition;

        /// <summary>Number of text positions within the geometry.</summary>
        public UINT32 length;

        /// <summary>Left position of the top-left coordinate of the geometry.</summary>
        public FLOAT left;

        /// <summary>Top position of the top-left coordinate of the geometry.</summary>
        public FLOAT top;

        /// <summary>Geometry's width.</summary>
        public FLOAT width;

        /// <summary>Geometry's height.</summary>
        public FLOAT height;

        /// <summary>Bidi level of text positions enclosed within the geometry.</summary>
        public UINT32 bidiLevel;

        /// <summary>Geometry encloses text?</summary>
        public BOOL isText;

        /// <summary>Range is trimmed.</summary>
        public BOOL isTrimmed;
        #endregion
    }
}
