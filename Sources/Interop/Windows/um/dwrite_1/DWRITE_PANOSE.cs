// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Typeface classification values, used for font selection and matching.</summary>
    /// <remarks> Note the family type (index 0) is the only stable entry in the 10-byte array, as all the following entries can change dynamically depending on context of the first field.</remarks>
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ unsafe struct DWRITE_PANOSE
    {
        #region Fields
        [FieldOffset(0)]
        [ComAliasName("UINT8[10]")]
        public fixed byte values[10];

        [FieldOffset(0)]
        [ComAliasName("UINT8")]
        public byte familyKind;

        [FieldOffset(0)]
        public _text_e__Struct text;

        [FieldOffset(0)]
        public _script_e__Struct script;

        [FieldOffset(0)]
        public _decorative_e__Struct decorative;

        [FieldOffset(0)]
        public _symbol_e__Struct symbol;
        #endregion

        #region Structs
        public /* blittable */ struct _text_e__Struct
        {
            #region Fields
            [ComAliasName("UINT8")]
            public byte familyKind;

            [ComAliasName("UINT8")]
            public byte serifStyle;

            [ComAliasName("UINT8")]
            public byte weight;

            [ComAliasName("UINT8")]
            public byte proportion;

            [ComAliasName("UINT8")]
            public byte contrast;

            [ComAliasName("UINT8")]
            public byte strokeVariation;

            [ComAliasName("UINT8")]
            public byte armStyle;

            [ComAliasName("UINT8")]
            public byte letterform;

            [ComAliasName("UINT8")]
            public byte midline;

            [ComAliasName("UINT8")]
            public byte xHeight;
            #endregion
        };

        public /* blittable */ struct _script_e__Struct
        {
            #region Fields
            [ComAliasName("UINT8")]
            public byte familyKind;

            [ComAliasName("UINT8")]
            public byte toolKind;

            [ComAliasName("UINT8")]
            public byte weight;

            [ComAliasName("UINT8")]
            public byte spacing;

            [ComAliasName("UINT8")]
            public byte aspectRatio;

            [ComAliasName("UINT8")]
            public byte contrast;

            [ComAliasName("UINT8")]
            public byte scriptTopology;

            [ComAliasName("UINT8")]
            public byte scriptForm;

            [ComAliasName("UINT8")]
            public byte finials;

            [ComAliasName("UINT8")]
            public byte xAscent;
            #endregion
        }

        public /* blittable */ struct _decorative_e__Struct
        {
            #region Fields
            [ComAliasName("UINT8")]
            public byte familyKind;

            [ComAliasName("UINT8")]
            public byte decorativeClass;

            [ComAliasName("UINT8")]
            public byte weight;

            [ComAliasName("UINT8")]
            public byte aspect;

            [ComAliasName("UINT8")]
            public byte contrast;

            [ComAliasName("UINT8")]
            public byte serifVariant;

            [ComAliasName("UINT8")]
            public byte fill;

            [ComAliasName("UINT8")]
            public byte lining;

            [ComAliasName("UINT8")]
            public byte decorativeTopology;

            [ComAliasName("UINT8")]
            public byte characterRange;
            #endregion
        }

        public /* blittable */ struct _symbol_e__Struct
        {
            #region Fields
            [ComAliasName("UINT8")]
            public byte familyKind;

            [ComAliasName("UINT8")]
            public byte symbolKind;

            [ComAliasName("UINT8")]
            public byte weight;

            [ComAliasName("UINT8")]
            public byte spacing;

            [ComAliasName("UINT8")]
            public byte aspectRatioAndContrast;

            [ComAliasName("UINT8")]
            public byte aspectRatio94;

            [ComAliasName("UINT8")]
            public byte aspectRatio119;

            [ComAliasName("UINT8")]
            public byte aspectRatio157;

            [ComAliasName("UINT8")]
            public byte aspectRatio163;

            [ComAliasName("UINT8")]
            public byte aspectRatio211;
            #endregion
        }
        #endregion
    }
}
