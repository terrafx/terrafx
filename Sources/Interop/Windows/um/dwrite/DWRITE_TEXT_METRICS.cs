// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Overall metrics associated with text after layout. All coordinates are in device independent pixels (DIPs).</summary>
    public /* blittable */ struct DWRITE_TEXT_METRICS
    {
        #region Fields
        /// <summary>Left-most point of formatted text relative to layout box (excluding any glyph overhang).</summary>
        public FLOAT left;

        /// <summary>Top-most point of formatted text relative to layout box (excluding any glyph overhang).</summary>
        public FLOAT top;

        /// <summary>The width of the formatted text ignoring trailing whitespace at the end of each line.</summary>
        public FLOAT width;

        /// <summary>The width of the formatted text taking into account the trailing whitespace at the end of each line.</summary>
        public FLOAT widthIncludingTrailingWhitespace;

        /// <summary>The height of the formatted text. The height of an empty string is determined by the size of the default font's line height.</summary>
        public FLOAT height;

        /// <summary>Initial width given to the layout. Depending on whether the text was wrapped or not, it can be either larger or smaller than the text content width.</summary>
        public FLOAT layoutWidth;

        /// <summary>Initial height given to the layout. Depending on the length of the text, it may be larger or smaller than the text content height.</summary>
        public FLOAT layoutHeight;

        /// <summary>The maximum reordering count of any line of text, used to calculate the most number of hit-testing boxes needed. If the layout has no bidirectional text or no text at all, the minimum level is 1.</summary>
        public UINT32 maxBidiReorderingDepth;

        /// <summary>Total number of lines.</summary>
        public UINT32 lineCount;
        #endregion
    }
}
