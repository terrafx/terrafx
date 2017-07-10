// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Allows the drawing state to be atomically created. This also specifies the drawing state that is saved into an IDrawingStateBlock object.</summary>
    public /* blittable */ struct D2D1_DRAWING_STATE_DESCRIPTION
    {
        #region Fields
        public D2D1_ANTIALIAS_MODE antialiasMode;

        public D2D1_TEXT_ANTIALIAS_MODE textAntialiasMode;

        public D2D1_TAG tag1;

        public D2D1_TAG tag2;

        public D2D1_MATRIX_3X2_F transform;
        #endregion
    }
}
