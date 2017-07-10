// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>A container for 3D lookup table data that can be passed to the LookupTable3D effect.</summary>
    [Guid("53DD9855-A3B0-4D5B-82E1-26E25C5E5797")]
    unsafe public /* blittable */ struct ID2D1LookupTable3D
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        // ID2D1LookupTable3D declares no new members
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;
            #endregion
        }
        #endregion
    }
}
