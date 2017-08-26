// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A bitmap brush allows a bitmap to be used to fill a geometry.  Interpolation mode is specified with D2D1_INTERPOLATION_MODE</summary>
    [Guid("41343A53-E41A-49A2-91CD-21793BBB62E5")]
    public /* blittable */ unsafe struct ID2D1BitmapBrush1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Sets the interpolation mode used when this brush is used.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetInterpolationMode1(
            [In] ID2D1BitmapBrush1* This,
            [In] D2D1_INTERPOLATION_MODE interpolationMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_INTERPOLATION_MODE GetInterpolationMode1(
            [In] ID2D1BitmapBrush1* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1BitmapBrush.Vtbl BaseVtbl;

            public IntPtr SetInterpolationMode1;

            public IntPtr GetInterpolationMode1;
            #endregion
        }
        #endregion
    }
}
