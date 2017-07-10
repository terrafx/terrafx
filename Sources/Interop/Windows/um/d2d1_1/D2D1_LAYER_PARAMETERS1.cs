// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>All parameters related to pushing a layer.</summary>
    unsafe public /* blittable */ struct D2D1_LAYER_PARAMETERS1
    {
        #region Fields
        public D2D1_RECT_F contentBounds;

        public ID2D1Geometry* geometricMask;

        public D2D1_ANTIALIAS_MODE maskAntialiasMode;

        public D2D1_MATRIX_3X2_F maskTransform;

        public FLOAT opacity;

        public ID2D1Brush* opacityBrush;

        public D2D1_LAYER_OPTIONS1 layerOptions;
        #endregion
    }
}
