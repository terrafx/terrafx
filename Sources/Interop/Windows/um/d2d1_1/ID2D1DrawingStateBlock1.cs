// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents drawing state.</summary>
    [Guid("689F1F85-C72E-4E33-8F19-85754EFD5ACE")]
    public /* blittable */ unsafe struct ID2D1DrawingStateBlock1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Retrieves the state currently contained within this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDescription(
            [In] ID2D1DrawingStateBlock1* This,
            [Out] D2D1_DRAWING_STATE_DESCRIPTION1* stateDescription
        );

        /// <summary>Sets the state description of this state block resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetDescription(
            [In] ID2D1DrawingStateBlock1* This,
            [In] /* readonly */ D2D1_DRAWING_STATE_DESCRIPTION1* stateDescription
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1DrawingStateBlock.Vtbl BaseVtbl;

            public IntPtr GetDescription;

            public IntPtr SetDescription;
            #endregion
        }
        #endregion
    }
}
