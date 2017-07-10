// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a geometry resource and defines a set of helper methods for manipulating and measuring geometric shapes. Interfaces that inherit from ID2D1Geometry define specific shapes.</summary>
    [Guid("2CD906A1-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1Geometry
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Retrieve the bounds of the geometry, with an optional applied transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetBounds(
            [In] ID2D1Geometry* This,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [Out] D2D1_RECT_F* bounds
        );

        /// <summary>Get the bounds of the corresponding geometry after it has been widened or have an optional pen style applied.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetWidenedBounds(
            [In] ID2D1Geometry* This,
            [In] FLOAT strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out] D2D1_RECT_F* bounds
        );

        /// <summary>Checks to see whether the corresponding penned and widened geometry contains the given point.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT StrokeContainsPoint(
            [In] ID2D1Geometry* This,
            [In] D2D1_POINT_2F point,
            [In] FLOAT strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out] BOOL* contains
        );

        /// <summary>Test whether the given fill of this geometry would contain this point.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT FillContainsPoint(
            [In] ID2D1Geometry* This,
            [In] D2D1_POINT_2F point,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out] BOOL* contains
        );

        /// <summary>Compare how one geometry intersects or contains another geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CompareWithGeometry(
            [In] ID2D1Geometry* This,
            [In] ID2D1Geometry* inputGeometry,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* inputGeometryTransform,
            [In] FLOAT flatteningTolerance,
            [Out] D2D1_GEOMETRY_RELATION* relation
        );

        /// <summary>Converts a geometry to a simplified geometry that has arcs and quadratic beziers removed.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Simplify(
            [In] ID2D1Geometry* This,
            [In] D2D1_GEOMETRY_SIMPLIFICATION_OPTION simplificationOption,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Tessellates a geometry into triangles.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Tessellate(
            [In] ID2D1Geometry* This,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [In] ID2D1TessellationSink* tessellationSink
        );

        /// <summary>Performs a combine operation between the two geometries to produce a resulting geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CombineWithGeometry(
            [In] ID2D1Geometry* This,
            [In] ID2D1Geometry* inputGeometry,
            [In] D2D1_COMBINE_MODE combineMode,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* inputGeometryTransform,
            [In] FLOAT flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Computes the outline of the geometry. The result is written back into a simplified geometry sink.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Outline(
            [In] ID2D1Geometry* This,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Computes the area of the geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ComputeArea(
            [In] ID2D1Geometry* This,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out] FLOAT* area
        );

        /// <summary>Computes the length of the geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ComputeLength(
            [In] ID2D1Geometry* This,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out] FLOAT* length
        );

        /// <summary>Computes the point and tangent a given distance along the path.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ComputePointAtLength(
            [In] ID2D1Geometry* This,
            [In] FLOAT length,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [Out, Optional] D2D1_POINT_2F* point,
            [Out, Optional] D2D1_POINT_2F* unitTangentVector
        );

        /// <summary>Get the geometry and widen it as well as apply an optional pen style.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Widen(
            [In] ID2D1Geometry* This,
            [In] FLOAT strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public GetBounds GetBounds;

            public GetWidenedBounds GetWidenedBounds;

            public StrokeContainsPoint StrokeContainsPoint;

            public FillContainsPoint FillContainsPoint;

            public CompareWithGeometry CompareWithGeometry;

            public Simplify Simplify;

            public Tessellate Tessellate;

            public CombineWithGeometry CombineWithGeometry;

            public Outline Outline;

            public ComputeArea ComputeArea;

            public ComputeLength ComputeLength;

            public ComputePointAtLength ComputePointAtLength;

            public Widen Widen;
            #endregion
        }
        #endregion
    }
}
