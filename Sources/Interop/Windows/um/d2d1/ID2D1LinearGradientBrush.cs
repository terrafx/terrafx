// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Paints an area with a linear gradient.</summary>
    [Guid("2CD906AB-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1LinearGradientBrush
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetStartPoint(
            [In] ID2D1SolidColorBrush* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F startPoint
        );

        /// <summary>Sets the end point of the gradient in local coordinate space. This is not
        /// influenced by the geometry being filled.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetEndPoint(
            [In] ID2D1SolidColorBrush* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F endPoint
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D2D1_POINT_2F")]
        public /* static */ delegate D2D_POINT_2F GetStartPoint(
            [In] ID2D1SolidColorBrush* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D2D1_POINT_2F")]
        public /* static */ delegate D2D_POINT_2F GetEndPoint(
            [In] ID2D1SolidColorBrush* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetGradientStopCollection(
            [In] ID2D1SolidColorBrush* This,
            [Out] ID2D1GradientStopCollection** gradientStopCollection
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Brush.Vtbl BaseVtbl;

            public IntPtr SetStartPoint;

            public IntPtr SetEndPoint;

            public IntPtr GetStartPoint;

            public IntPtr GetEndPoint;

            public IntPtr GetGradientStopCollection;
            #endregion
        }
        #endregion
    }
}
