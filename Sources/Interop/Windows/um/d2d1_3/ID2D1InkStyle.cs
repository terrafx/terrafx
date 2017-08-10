// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a collection of style properties to be used by methods like ID2D1DeviceContext2::DrawInk when rendering ink. The ink style defines the nib (pen tip) shape and transform.</summary>
    [Guid("BAE8B344-23FC-4071-8CB5-D05D6F073848")]
    unsafe public /* blittable */ struct ID2D1InkStyle
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetNibTransform(
            [In] ID2D1InkStyle* This,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] /* readonly */ D2D_MATRIX_3X2_F* transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetNibTransform(
            [In] ID2D1InkStyle* This,
            [Out, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetNibShape(
            [In] ID2D1InkStyle* This,
            [In] D2D1_INK_NIB_SHAPE nibShape
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_INK_NIB_SHAPE GetNibShape(
            [In] ID2D1InkStyle* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public IntPtr SetNibTransform;

            public IntPtr GetNibTransform;

            public IntPtr SetNibShape;

            public IntPtr GetNibShape;
            #endregion
        }
        #endregion
    }
}
