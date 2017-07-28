// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Geometry enclosing of text positions.</summary>
    public /* blittable */ struct DWRITE_HIT_TEST_METRICS
    {
        #region Fields
        /// <summary>First text position within the geometry.</summary>
        [ComAliasName("UINT32")]
        public uint textPosition;

        /// <summary>Number of text positions within the geometry.</summary>
        [ComAliasName("UINT32")]
        public uint length;

        /// <summary>Left position of the top-left coordinate of the geometry.</summary>
        [ComAliasName("FLOAT")]
        public float left;

        /// <summary>Top position of the top-left coordinate of the geometry.</summary>
        [ComAliasName("FLOAT")]
        public float top;

        /// <summary>Geometry's width.</summary>
        [ComAliasName("FLOAT")]
        public float width;

        /// <summary>Geometry's height.</summary>
        [ComAliasName("FLOAT")]
        public float height;

        /// <summary>Bidi level of text positions enclosed within the geometry.</summary>
        [ComAliasName("UINT32")]
        public uint bidiLevel;

        /// <summary>Geometry encloses text?</summary>
        [ComAliasName("BOOL")]
        public int isText;

        /// <summary>Range is trimmed.</summary>
        [ComAliasName("BOOL")]
        public int isTrimmed;
        #endregion
    }
}
