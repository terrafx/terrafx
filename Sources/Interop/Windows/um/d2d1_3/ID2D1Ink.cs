// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a single continuous stroke of variable-width ink, as defined by a series of Bezier segments and widths.</summary>
    [Guid("B499923B-7029-478F-A8B3-432C7C5F5312")]
    unsafe public /* blittable */ struct ID2D1Ink
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Resets the ink start point.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetStartPoint(
            [In] ID2D1Ink* This,
            [In] /* readonly */ D2D1_INK_POINT* startPoint
        );

        /// <summary>Retrieve the start point with which the ink was initialized.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_INK_POINT GetStartPoint(
            [In] ID2D1Ink* This
        );

        /// <summary>Add one or more segments to the end of the ink.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddSegments(
            [In] ID2D1Ink* This,
            [In] /* readonly */ D2D1_INK_BEZIER_SEGMENT* segments,
            [In] UINT32 segmentsCount
        );

        /// <summary>Remove one or more segments from the end of the ink.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT RemoveSegmentsAtEnd(
            [In] ID2D1Ink* This,
            [In] UINT32 segmentsCount
        );

        /// <summary>Updates the specified segments with new control points.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetSegments(
            [In] ID2D1Ink* This,
            [In] UINT32 startSegment,
            [In] /* readonly */ D2D1_INK_BEZIER_SEGMENT* segments,
            [In] UINT32 segmentsCount
        );

        /// <summary>Update the last segment with new control points.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetSegmentAtEnd(
            [In] ID2D1Ink* This,
            [In] /* readonly */ D2D1_INK_BEZIER_SEGMENT* segment
        );

        /// <summary>Returns the number of segments the ink is composed of.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetSegmentCount(
            [In] ID2D1Ink* This
        );

        /// <summary>Retrieve the segments stored in the ink.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSegments(
            [In] ID2D1Ink* This,
            [In] UINT32 startSegment,
            [Out] D2D1_INK_BEZIER_SEGMENT* segments,
            [In] UINT32 segmentsCount
        );

        /// <summary>Construct a geometric representation of the ink.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT StreamAsGeometry(
            [In] ID2D1Ink* This,
            [In, Optional] ID2D1InkStyle* inkStyle,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [In] FLOAT flatteningTolerance,
            [In] ID2D1SimplifiedGeometrySink* geometrySink
        );

        /// <summary>Retrieve the bounds of the ink, with an optional applied transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetBounds(
            [In] ID2D1Ink* This,
            [In, Optional] ID2D1InkStyle* inkStyle,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* worldTransform,
            [Out] D2D1_RECT_F* bounds
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public SetStartPoint SetStartPoint;

            public GetStartPoint GetStartPoint;

            public AddSegments AddSegments;

            public RemoveSegmentsAtEnd RemoveSegmentsAtEnd;

            public SetSegments SetSegments;

            public SetSegmentAtEnd SetSegmentAtEnd;

            public GetSegmentCount GetSegmentCount;

            public GetSegments GetSegments;

            public StreamAsGeometry StreamAsGeometry;

            public GetBounds GetBounds;
            #endregion
        }
        #endregion
    }
}
