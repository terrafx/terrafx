// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Holds the appropriate digits and numeric punctuation for a given locale.</summary>
    [Guid("14885CC9-BAB0-4F90-B6ED-5C366A2CD03D")]
    unsafe public /* blittable */ struct IDWriteNumberSubstitution
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        // IDWriteNumberSubstitution declares no new members
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;
            #endregion
        }
        #endregion
    }
}
