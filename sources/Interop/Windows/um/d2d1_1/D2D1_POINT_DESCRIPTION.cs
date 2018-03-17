// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Describes a point along a path.</summary>
    public /* blittable */ struct D2D1_POINT_DESCRIPTION
    {
        #region Fields
        [ComAliasName("D2D1_POINT_2F")]
        public D2D_POINT_2F point;

        [ComAliasName("D2D1_POINT_2F")]
        public D2D_POINT_2F unitTangentVector;

        [ComAliasName("UINT32")]
        public uint endSegment;

        [ComAliasName("UINT32")]
        public uint endFigure;

        [ComAliasName("FLOAT")]
        public float lengthToEndSegment;
        #endregion
    }
}