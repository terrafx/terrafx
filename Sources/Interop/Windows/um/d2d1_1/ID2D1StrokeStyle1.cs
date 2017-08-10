// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Extends a stroke style to allow nominal width strokes.</summary>
    [Guid("10A72A66-E91C-43F4-993F-DDF4B82B0B4A")]
    unsafe public /* blittable */ struct ID2D1StrokeStyle1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_STROKE_TRANSFORM_TYPE GetStrokeTransformType(
            [In] ID2D1StrokeStyle1* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1StrokeStyle.Vtbl BaseVtbl;

            public IntPtr GetStrokeTransformType;
            #endregion
        }
        #endregion
    }
}
