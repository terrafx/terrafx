// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>The DWRITE_LINE_METRICS public structure contains information about a formatted line of text.</summary>
    public /* blittable */ struct DWRITE_LINE_METRICS
    {
        #region Fields
        /// <summary>The number of total text positions in the line. This includes any trailing whitespace and newline characters.</summary>>
        public UINT32 length;

        /// <summary>The number of whitespace positions at the end of the line. Newline sequences are considered whitespace.</summary>>
        public UINT32 trailingWhitespaceLength;

        /// <summary>The number of characters in the newline sequence at the end of the line. If the count is zero, then the line was either wrapped or it is the end of the text.</summary>>
        public UINT32 newlineLength;

        /// <summary>Height of the line as measured from top to bottom.</summary>>
        public FLOAT height;

        /// <summary>Distance from the top of the line to its baseline.</summary>>
        public FLOAT baseline;

        /// <summary>The line is trimmed.</summary>>
        public BOOL isTrimmed;
        #endregion
    }
}
