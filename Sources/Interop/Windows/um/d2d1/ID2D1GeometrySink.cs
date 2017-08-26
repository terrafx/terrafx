// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Describes a geometric path that can contain lines, arcs, cubic Bezier curves, and quadratic Bezier curves.</summary>
    [Guid("2CD9069F-12E2-11DC-9FED-001143A055F9")]
    public /* blittable */ unsafe struct ID2D1GeometrySink
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddLine(
            [In] ID2D1GeometrySink* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddBezier(
            [In] ID2D1GeometrySink* This,
            [In] /* readonly */ D2D1_BEZIER_SEGMENT* bezier
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddQuadraticBezier(
            [In] ID2D1GeometrySink* This,
            [In] /* readonly */ D2D1_QUADRATIC_BEZIER_SEGMENT* bezier
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddQuadraticBeziers(
            [In] ID2D1GeometrySink* This,
            [In] /* readonly */ D2D1_QUADRATIC_BEZIER_SEGMENT* beziers,
            [In, ComAliasName("UINT32")] uint beziersCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddArc(
            [In] ID2D1GeometrySink* This,
            [In] /* readonly */ D2D1_ARC_SEGMENT* arc
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1SimplifiedGeometrySink.Vtbl BaseVtbl;

            public IntPtr AddLine;

            public IntPtr AddBezier;

            public IntPtr AddQuadraticBezier;

            public IntPtr AddQuadraticBeziers;

            public IntPtr AddArc;
            #endregion
        }
        #endregion
    }
}
