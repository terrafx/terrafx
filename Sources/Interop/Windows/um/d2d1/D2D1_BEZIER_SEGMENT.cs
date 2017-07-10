// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>Describes a cubic bezier in a path.</summary>
    public /* blittable */ struct D2D1_BEZIER_SEGMENT
    {
        #region Fields
        public D2D1_POINT_2F point1;

        public D2D1_POINT_2F point2;

        public D2D1_POINT_2F point3;
        #endregion
    }
}
