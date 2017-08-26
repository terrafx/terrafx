// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Represents a producer of pixels that can fill an arbitrary 2D plane.</summary>
    [Guid("65019F75-8DA2-497C-B32C-DFA34E48EDE6")]
    unsafe public /* blittable */ struct ID2D1Image
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        // ID2D1Image declares no new members
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
