// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Enables creation and drawing of geometry realization objects.</summary>
    [Guid("D37F57E4-6908-459F-A199-E72F24F79987")]
    public /* blittable */ unsafe struct ID2D1DeviceContext1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFilledGeometryRealization(
            [In] ID2D1DeviceContext1* This,
            [In] ID2D1Geometry* geometry,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out] ID2D1GeometryRealization** geometryRealization
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStrokedGeometryRealization(
            [In] ID2D1DeviceContext1* This,
            [In] ID2D1Geometry* geometry,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [Out] ID2D1GeometryRealization** geometryRealization
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawGeometryRealization(
            [In] ID2D1DeviceContext1* This,
            [In] ID2D1GeometryRealization* geometryRealization,
            [In] ID2D1Brush* brush
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1DeviceContext.Vtbl BaseVtbl;

            public IntPtr CreateFilledGeometryRealization;

            public IntPtr CreateStrokedGeometryRealization;

            public IntPtr DrawGeometryRealization;
            #endregion
        }
        #endregion
    }
}
