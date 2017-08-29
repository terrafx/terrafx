// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The ID2D1PathGeometry1 interface adds functionality to ID2D1PathGeometry. In particular, it provides the path geometry-specific ComputePointAndSegmentAtLength method.</summary>
    [Guid("62BAA2D2-AB54-41B7-B872-787E0106A421")]
    public /* blittable */ unsafe struct ID2D1PathGeometry1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1PathGeometry1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1PathGeometry1* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFactory(
            [In] ID2D1PathGeometry1* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1Geometry Delegates
        /// <summary>Retrieve the bounds of the geometry, with an optional applied transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetBounds(
            [In] ID2D1PathGeometry1* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        );

        /// <summary>Get the bounds of the corresponding geometry after it has been widened or have an optional pen style applied.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetWidenedBounds(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        );

        /// <summary>Checks to see whether the corresponding penned and widened geometry contains the given point.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int StrokeContainsPoint(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("BOOL")] int* contains
        );

        /// <summary>Test whether the given fill of this geometry would contain this point.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FillContainsPoint(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("BOOL")] int* contains
        );

        /// <summary>Compare how one geometry intersects or contains another geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CompareWithGeometry(
            [In] ID2D1PathGeometry1* This,
            [In] ID2D1Geometry* inputGeometry,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* inputGeometryTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out] D2D1_GEOMETRY_RELATION* relation
        );

        /// <summary>Converts a geometry to a simplified geometry that has arcs and quadratic beziers removed.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Simplify(
            [In] ID2D1PathGeometry1* This,
            [In] D2D1_GEOMETRY_SIMPLIFICATION_OPTION simplificationOption,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Tessellates a geometry into triangles.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Tessellate(
            [In] ID2D1PathGeometry1* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1TessellationSink* tessellationSink
        );

        /// <summary>Performs a combine operation between the two geometries to produce a resulting geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CombineWithGeometry(
            [In] ID2D1PathGeometry1* This,
            [In] ID2D1Geometry* inputGeometry,
            [In] D2D1_COMBINE_MODE combineMode,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* inputGeometryTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Computes the outline of the geometry. The result is written back into a simplified geometry sink.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Outline(
            [In] ID2D1PathGeometry1* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Computes the area of the geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputeArea(
            [In] ID2D1PathGeometry1* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("FLOAT")] float* area
        );

        /// <summary>Computes the length of the geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputeLength(
            [In] ID2D1PathGeometry1* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("FLOAT")] float* length
        );

        /// <summary>Computes the point and tangent a given distance along the path.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputePointAtLength(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("FLOAT")] float length,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* point = null,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* unitTangentVector = null
        );

        /// <summary>Get the geometry and widen it as well as apply an optional pen style.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Widen(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );
        #endregion

        #region ID2D1PathGeometry Delegates
        /// <summary>Opens a geometry sink that will be used to create this path geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Open(
            [In] ID2D1PathGeometry1* This,
            [Out] ID2D1GeometrySink** geometrySink
        );

        /// <summary>Retrieve the contents of this geometry. The caller passes an implementation of a
        /// ID2D1GeometrySink interface to receive the data.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Stream(
            [In] ID2D1PathGeometry1* This,
            [In] ID2D1GeometrySink* geometrySink
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSegmentCount(
            [In] ID2D1PathGeometry1* This,
            [Out, ComAliasName("UINT32")] uint* count
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFigureCount(
            [In] ID2D1PathGeometry1* This,
            [Out, ComAliasName("UINT32")] uint* count
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ComputePointAndSegmentAtLength(
            [In] ID2D1PathGeometry1* This,
            [In, ComAliasName("FLOAT")] float length,
            [In, ComAliasName("UINT32")] uint startSegment,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out] D2D1_POINT_DESCRIPTION* pointDescription
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

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region ID2D1Geometry Fields
            public IntPtr GetBounds;

            public IntPtr GetWidenedBounds;

            public IntPtr StrokeContainsPoint;

            public IntPtr FillContainsPoint;

            public IntPtr CompareWithGeometry;

            public IntPtr Simplify;

            public IntPtr Tessellate;

            public IntPtr CombineWithGeometry;

            public IntPtr Outline;

            public IntPtr ComputeArea;

            public IntPtr ComputeLength;

            public IntPtr ComputePointAtLength;

            public IntPtr Widen;
            #endregion

            #region ID2D1PathGeometry Fields
            public IntPtr Open;

            public IntPtr Stream;

            public IntPtr GetSegmentCount;

            public IntPtr GetFigureCount;
            #endregion

            #region Fields
            public IntPtr ComputePointAndSegmentAtLength;
            #endregion
        }
        #endregion
    }
}
