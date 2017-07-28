// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Stores an ordered pair of integers, typically the width and height of a rectangle.</summary>
    public /* blittable */ struct D2D_SIZE_U
    {
        #region Fields
        [ComAliasName("UINT32")]
        public uint width;

        [ComAliasName("UINT32")]
        public uint height;
        #endregion
    }
}
