// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Contains the gradient origin offset and the size and position of the gradient ellipse for an ID2D1RadialGradientBrush.</summary>
    public /* blittable */ struct D2D1_RADIAL_GRADIENT_BRUSH_PROPERTIES
    {
        #region Fields
        public D2D1_POINT_2F center;

        public D2D1_POINT_2F gradientOriginOffset;

        public FLOAT radiusX;

        public FLOAT radiusY;
        #endregion
    }
}
