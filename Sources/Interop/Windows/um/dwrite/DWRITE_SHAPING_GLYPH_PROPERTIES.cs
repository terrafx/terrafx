// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Shaping output properties per output glyph.</summary>
    public /* blittable */ struct DWRITE_SHAPING_GLYPH_PROPERTIES
    {
        #region Fields
        internal UINT16 _bitField;
        #endregion

        #region Properties
        /// <summary>Justification class, whether to use spacing, kashidas, or another method. This exists for backwards compatibility with Uniscribe's SCRIPT_JUSTIFY enum.</summary>
        public UINT16 justification
        {
            get
            {
                return (ushort)(_bitField & 0b0000_0000_0000_1111);
            }

            set
            {
                _bitField = (ushort)((_bitField & 0b1111_1111_1111_0000) | (value & 0b0000_0000_0000_1111));
            }
        }

        /// <summary>Indicates glyph is the first of a cluster.</summary>
        public UINT16 isClusterStart
        {
            get
            {
                return (ushort)((_bitField & 0b0000_0000_0001_0000) >> 4);
            }

            set
            {
                _bitField = (ushort)((_bitField & 0b1111_1111_1110_1111) | ((value << 4) & 0b0000_0000_0001_0000));
            }
        }

        /// <summary>Glyph is a diacritic.</summary>
        public UINT16 isDiacritic
        {
            get
            {
                return (ushort)((_bitField & 0b0000_0000_0010_0000) >> 5);
            }

            set
            {
                _bitField = (ushort)((_bitField & 0b1111_1111_1101_1111) | ((value << 5) & 0b0000_0000_0010_0000));
            }
        }

        /// <summary>Glyph has no width, blank, ZWJ, ZWNJ etc.</summary>
        public UINT16 isZeroWidthSpace
        {
            get
            {
                return (ushort)((_bitField & 0b0000_0000_0100_0000) >> 6);
            }

            set
            {
                _bitField = (ushort)((_bitField & 0b1111_1111_1011_1111) | ((value << 6) & 0b0000_0000_0100_0000));
            }
        }

        /// <summary>Reserved for use by shaping engine.</summary>
        public UINT16 reserved
        {
            get
            {
                return (ushort)((_bitField & 0b1111_1111_1000_0000) >> 7);
            }

            set
            {
                _bitField = (ushort)((_bitField & 0b0000_0000_0111_1111) | ((value << 7) & 0b1111_1111_1000_0000));
            }
        }
        #endregion
    }
}
