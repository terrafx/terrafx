// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    /// <summary>Typeface classification values, used for font selection and matching.</summary>
    /// <remarks> Note the family type (index 0) is the only stable entry in the 10-byte array, as all the following entries can change dynamically depending on context of the first field.</remarks>
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct DWRITE_PANOSE
    {
        #region Fields
        [FieldOffset(0)]
        public _values_e__FixedBuffer values;

        [FieldOffset(0)]
        public UINT8 familyKind;

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
        unsafe public /* blittable */ struct _values_e__FixedBuffer
        {
            #region Fields
            public UINT8 e0;

            public UINT8 e1;

            public UINT8 e2;

            public UINT8 e3;

            public UINT8 e4;

            public UINT8 e5;

            public UINT8 e6;

            public UINT8 e7;

            public UINT8 e8;

            public UINT8 e9;
            #endregion

            #region Properties
            public UINT8 this[int index]
            {
                get
                {
                    if ((uint)(index) > 9) // (index < 0) || (index > 9)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index), index);
                    }

                    fixed (UINT8* e = &e0)
                    {
                        return e[index];
                    }
                }
            }
            #endregion
        }

        public /* blittable */ struct _text_e__Struct
        {
            #region Fields
            public UINT8 familyKind;

            public UINT8 serifStyle;

            public UINT8 weight;

            public UINT8 proportion;

            public UINT8 contrast;

            public UINT8 strokeVariation;

            public UINT8 armStyle;

            public UINT8 letterform;

            public UINT8 midline;

            public UINT8 xHeight;
            #endregion
        };

        public /* blittable */ struct _script_e__Struct
        {
            #region Fields
            public UINT8 familyKind;

            public UINT8 toolKind;

            public UINT8 weight;

            public UINT8 spacing;

            public UINT8 aspectRatio;

            public UINT8 contrast;

            public UINT8 scriptTopology;

            public UINT8 scriptForm;

            public UINT8 finials;

            public UINT8 xAscent;
            #endregion
        }

        public /* blittable */ struct _decorative_e__Struct
        {
            #region Fields
            public UINT8 familyKind;

            public UINT8 decorativeClass;

            public UINT8 weight;

            public UINT8 aspect;

            public UINT8 contrast;

            public UINT8 serifVariant;

            public UINT8 fill;

            public UINT8 lining;

            public UINT8 decorativeTopology;

            public UINT8 characterRange;
            #endregion
        }

        public /* blittable */ struct _symbol_e__Struct
        {
            #region Fields
            public UINT8 familyKind;

            public UINT8 symbolKind;

            public UINT8 weight;

            public UINT8 spacing;

            public UINT8 aspectRatioAndContrast;

            public UINT8 aspectRatio94;

            public UINT8 aspectRatio119;

            public UINT8 aspectRatio157;

            public UINT8 aspectRatio163;

            public UINT8 aspectRatio211;
            #endregion
        }
        #endregion
    }
}
