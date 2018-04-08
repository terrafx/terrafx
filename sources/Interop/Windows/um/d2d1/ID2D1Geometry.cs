// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>Represents a geometry resource and defines a set of helper methods for manipulating and measuring geometric shapes. Interfaces that inherit from ID2D1Geometry define specific shapes.</summary>
    [Guid("2CD906A1-12E2-11DC-9FED-001143A055F9")]
    public /* unmanaged */ unsafe struct ID2D1Geometry
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1Geometry* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1Geometry* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1Geometry* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetFactory(
            [In] ID2D1Geometry* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region Delegates
        /// <summary>Retrieve the bounds of the geometry, with an optional applied transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetBounds(
            [In] ID2D1Geometry* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        );

        /// <summary>Get the bounds of the corresponding geometry after it has been widened or have an optional pen style applied.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetWidenedBounds(
            [In] ID2D1Geometry* This,
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
        public /* static */ delegate int _StrokeContainsPoint(
            [In] ID2D1Geometry* This,
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
        public /* static */ delegate int _FillContainsPoint(
            [In] ID2D1Geometry* This,
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("BOOL")] int* contains
        );

        /// <summary>Compare how one geometry intersects or contains another geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CompareWithGeometry(
            [In] ID2D1Geometry* This,
            [In] ID2D1Geometry* inputGeometry,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* inputGeometryTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out] D2D1_GEOMETRY_RELATION* relation
        );

        /// <summary>Converts a geometry to a simplified geometry that has arcs and quadratic beziers removed.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Simplify(
            [In] ID2D1Geometry* This,
            [In] D2D1_GEOMETRY_SIMPLIFICATION_OPTION simplificationOption,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Tessellates a geometry into triangles.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Tessellate(
            [In] ID2D1Geometry* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1TessellationSink* tessellationSink
        );

        /// <summary>Performs a combine operation between the two geometries to produce a resulting geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CombineWithGeometry(
            [In] ID2D1Geometry* This,
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
        public /* static */ delegate int _Outline(
            [In] ID2D1Geometry* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Computes the area of the geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ComputeArea(
            [In] ID2D1Geometry* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("FLOAT")] float* area
        );

        /// <summary>Computes the length of the geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ComputeLength(
            [In] ID2D1Geometry* This,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("FLOAT")] float* length
        );

        /// <summary>Computes the point and tangent a given distance along the path.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ComputePointAtLength(
            [In] ID2D1Geometry* This,
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
        public /* static */ delegate int _Widen(
            [In] ID2D1Geometry* This,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID2D1Resource Methods
        public void GetFactory(
            [Out] ID2D1Factory** factory
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                MarshalFunction<_GetFactory>(lpVtbl->GetFactory)(
                    This,
                    factory
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetBounds(
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_GetBounds>(lpVtbl->GetBounds)(
                    This,
                    worldTransform,
                    bounds
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetWidenedBounds(
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* bounds
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_GetWidenedBounds>(lpVtbl->GetWidenedBounds)(
                    This,
                    strokeWidth,
                    strokeStyle,
                    worldTransform,
                    flatteningTolerance,
                    bounds
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int StrokeContainsPoint(
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point,
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("BOOL")] int* contains
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_StrokeContainsPoint>(lpVtbl->StrokeContainsPoint)(
                    This,
                    point,
                    strokeWidth,
                    strokeStyle,
                    worldTransform,
                    flatteningTolerance,
                    contains
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int FillContainsPoint(
            [In, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F point,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("BOOL")] int* contains
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_FillContainsPoint>(lpVtbl->FillContainsPoint)(
                    This,
                    point,
                    worldTransform,
                    flatteningTolerance,
                    contains
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CompareWithGeometry(
            [In] ID2D1Geometry* inputGeometry,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* inputGeometryTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out] D2D1_GEOMETRY_RELATION* relation
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_CompareWithGeometry>(lpVtbl->CompareWithGeometry)(
                    This,
                    inputGeometry,
                    inputGeometryTransform,
                    flatteningTolerance,
                    relation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Simplify(
            [In] D2D1_GEOMETRY_SIMPLIFICATION_OPTION simplificationOption,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_Simplify>(lpVtbl->Simplify)(
                    This,
                    simplificationOption,
                    worldTransform,
                    flatteningTolerance,
                    geometrySink
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Tessellate(
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1TessellationSink* tessellationSink
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_Tessellate>(lpVtbl->Tessellate)(
                    This,
                    worldTransform,
                    flatteningTolerance,
                    tessellationSink
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CombineWithGeometry(
            [In] ID2D1Geometry* inputGeometry,
            [In] D2D1_COMBINE_MODE combineMode,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* inputGeometryTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_CombineWithGeometry>(lpVtbl->CombineWithGeometry)(
                    This,
                    inputGeometry,
                    combineMode,
                    inputGeometryTransform,
                    flatteningTolerance,
                    geometrySink
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Outline(
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_Outline>(lpVtbl->Outline)(
                    This,
                    worldTransform,
                    flatteningTolerance,
                    geometrySink
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ComputeArea(
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("FLOAT")] float* area
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_ComputeArea>(lpVtbl->ComputeArea)(
                    This,
                    worldTransform,
                    flatteningTolerance,
                    area
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ComputeLength(
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("FLOAT")] float* length
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_ComputeLength>(lpVtbl->ComputeLength)(
                    This,
                    worldTransform,
                    flatteningTolerance,
                    length
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ComputePointAtLength(
            [In, ComAliasName("FLOAT")] float length,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* point = null,
            [Out, ComAliasName("D2D1_POINT_2F")] D2D_POINT_2F* unitTangentVector = null
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_ComputePointAtLength>(lpVtbl->ComputePointAtLength)(
                    This,
                    length,
                    worldTransform,
                    flatteningTolerance,
                    point,
                    unitTangentVector
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Widen(
            [In, ComAliasName("FLOAT")] float strokeWidth,
            [In, Optional] ID2D1StrokeStyle* strokeStyle,
            [In, Optional, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* worldTransform,
            [In, ComAliasName("FLOAT")] float flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        )
        {
            fixed (ID2D1Geometry* This = &this)
            {
                return MarshalFunction<_Widen>(lpVtbl->Widen)(
                    This,
                    strokeWidth,
                    strokeStyle,
                    worldTransform,
                    flatteningTolerance,
                    geometrySink
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region Fields
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
        }
        #endregion
    }
}

