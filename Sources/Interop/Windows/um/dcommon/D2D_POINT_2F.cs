// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Represents an x-coordinate and y-coordinate pair in two-dimensional space.</summary>
    public /* blittable */ struct D2D_POINT_2F
    {
        #region Fields
        [ComAliasName("FLOAT")]
        public float x;

        [ComAliasName("FLOAT")]
        public float y;
        #endregion
    }
}
