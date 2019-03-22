// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Contains the position and color of a gradient stop.</summary>
    [Unmanaged]
    public struct D2D1_GRADIENT_STOP
    {
        #region Fields
        [ComAliasName("FLOAT")]
        public float position;

        [ComAliasName("D2D1_COLOR_F")]
        public DXGI_RGBA color;
        #endregion
    }
}
