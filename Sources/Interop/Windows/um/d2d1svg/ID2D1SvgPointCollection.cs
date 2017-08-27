// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1svg.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface describing an SVG 'points' value in a 'polyline' or 'polygon' element.</summary>
    [Guid("9DBE4C0D-3572-4DD9-9825-5530813BB712")]
    public /* blittable */ unsafe struct ID2D1SvgPointCollection
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Removes points from the end of the array.</summary>
        /// <param name="pointsCount">Specifies how many points to remove.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RemovePointsAtEnd(
            [In] ID2D1SvgPointCollection* This,
            [In, ComAliasName("UINT32")] uint pointsCount
        );

        /// <summary>Updates the points array. Existing points not updated by this method are preserved. The array is resized larger if necessary to accomodate the new points.</summary>
        /// <param name="points">The points array.</param>
        /// <param name="pointsCount">The number of points to update.</param>
        /// <param name="startIndex">The index at which to begin updating points. Must be less than or equal to the size of the array.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UpdatePoints(
            [In] ID2D1SvgPointCollection* This,
            [In, ComAliasName("D2D1_POINT_2F[]")] D2D_POINT_2F* points,
            [In, ComAliasName("UINT32")] uint pointsCount,
            [In, ComAliasName("UINT32")] uint startIndex = 0
        );

        /// <summary>Gets points from the points array.</summary>
        /// <param name="points">Buffer to contain the points.</param>
        /// <param name="pointsCount">The element count of the buffer.</param>
        /// <param name="startIndex">The index of the first point to retrieve.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPoints(
            [In] ID2D1SvgPointCollection* This,
            [Out, ComAliasName("D2D1_POINT_2F[]")] D2D_POINT_2F* points,
            [In, ComAliasName("UINT32")] uint pointsCount,
            [In, ComAliasName("UINT32")] uint startIndex = 0
        );

        /// <summary>Gets the number of points in the array.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPointsCount(
            [In] ID2D1SvgPointCollection* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1SvgAttribute.Vtbl BaseVtbl;

            public IntPtr RemovePointsAtEnd;

            public IntPtr UpdatePoints;

            public IntPtr GetPoints;

            public IntPtr GetPointsCount;
            #endregion
        }
        #endregion
    }
}
