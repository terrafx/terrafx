// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Encapsulates a device- and transform-dependent representation of a filled or stroked geometry.</summary>
    [Guid("A16907D7-BC02-4801-99E8-8CF7F485F774")]
    unsafe public /* blittable */ struct ID2D1GeometryRealization
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        // ID2D1GeometryRealization declares no new members
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
