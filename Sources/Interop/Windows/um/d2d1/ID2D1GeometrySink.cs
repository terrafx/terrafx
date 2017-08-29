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

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1GeometrySink* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1GeometrySink* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1GeometrySink* This
        );
        #endregion

        #region ID2D1SimplifiedGeometrySink Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetFillMode(
            [In] ID2D1GeometrySink* This,
            [In] D2D1_FILL_MODE fillMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetSegmentFlags(
            [In] ID2D1GeometrySink* This,
            [In] D2D1_PATH_SEGMENT vertexFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void BeginFigure(
            [In] ID2D1GeometrySink* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F startPoint,
            [In] D2D1_FIGURE_BEGIN figureBegin
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddLines(
            [In] ID2D1GeometrySink* This,
            [In, ComAliasName("D2D1_POINT_2F[]")] D2D_POINT_2F* points,
            [In, ComAliasName("UINT32")] uint pointsCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddBeziers(
            [In] ID2D1GeometrySink* This,
            [In, ComAliasName("D2D1_BEZIER_SEGMENT[]")] D2D1_BEZIER_SEGMENT* beziers,
            [In, ComAliasName("UINT32")] uint beziersCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void EndFigure(
            [In] ID2D1GeometrySink* This,
            [In] D2D1_FIGURE_END figureEnd
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Close(
            [In] ID2D1GeometrySink* This
        );
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
            [In] D2D1_BEZIER_SEGMENT* bezier
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddQuadraticBezier(
            [In] ID2D1GeometrySink* This,
            [In] D2D1_QUADRATIC_BEZIER_SEGMENT* bezier
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddQuadraticBeziers(
            [In] ID2D1GeometrySink* This,
            [In, ComAliasName("D2D1_QUADRATIC_BEZIER_SEGMENT[]")] D2D1_QUADRATIC_BEZIER_SEGMENT* beziers,
            [In, ComAliasName("UINT32")] uint beziersCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void AddArc(
            [In] ID2D1GeometrySink* This,
            [In] D2D1_ARC_SEGMENT* arc
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1SimplifiedGeometrySink Fields
            public IntPtr SetFillMode;

            public IntPtr SetSegmentFlags;

            public IntPtr BeginFigure;

            public IntPtr AddLines;

            public IntPtr AddBeziers;

            public IntPtr EndFigure;

            public IntPtr Close;
            #endregion

            #region Fields
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
