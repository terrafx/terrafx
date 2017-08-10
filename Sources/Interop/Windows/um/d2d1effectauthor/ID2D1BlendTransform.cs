// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>An effect uses this interface to configure a blending operation.</summary>
    [Guid("63AC0B32-BA44-450F-8806-7F4CA1FF2F1B")]
    unsafe public /* blittable */ struct ID2D1BlendTransform
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetDescription(
            [In] ID2D1BlendTransform* This,
            [In] /* readonly */ D2D1_BLEND_DESCRIPTION* description
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDescription(
            [In] ID2D1BlendTransform* This,
            [Out] D2D1_BLEND_DESCRIPTION* description
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1ConcreteTransform.Vtbl BaseVtbl;

            public IntPtr SetDescription;

            public IntPtr GetDescription;
            #endregion
        }
        #endregion
    }
}
