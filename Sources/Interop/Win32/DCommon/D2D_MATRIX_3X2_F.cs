// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Represents a 3-by-2 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    unsafe public struct D2D_MATRIX_3X2_F
    {
        #region Fields
        [FieldOffset(0)]
        public _Anonymous1_e__Union Anonymous1;

        [FieldOffset(0)]
        public _Anonymous2_e__Union Anonymous2;

        [FieldOffset(0)]
        public fixed float m[3 * 2];
        #endregion

        #region Structs
        public struct _Anonymous1_e__Union
        {
            #region Fields
            /// <summary>Horizontal scaling / cosine of rotation</summary>
            public float m11;

            /// <summary>Vertical shear / sine of rotation</summary>
            public float m12;

            /// <summary>Horizontal shear / negative sine of rotation</summary>
            public float m21;

            /// <summary>Vertical scaling / cosine of rotation</summary>
            public float m22;

            /// <summary>Horizontal shift (always orthogonal regardless of rotation)</summary>
            public float dx;

            /// <summary>Vertical shift (always orthogonal regardless of rotation)</summary>
            public float dy;
            #endregion
        }

        public struct _Anonymous2_e__Union
        {
            #region Fields
            public float _11, _12;

            public float _21, _22;

            public float _31, _32;
            #endregion
        }
        #endregion
    }
}
