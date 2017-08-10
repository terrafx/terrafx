// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1svg.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface describing an SVG 'stroke-dasharray' value.</summary>
    [Guid("F1C0CA52-92A3-4F00-B4CE-F35691EFD9D9")]
    unsafe public /* blittable */ struct ID2D1SvgStrokeDashArray
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Removes dashes from the end of the array.</summary>
        /// <param name="dashesCount">Specifies how many dashes to remove.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RemoveDashesAtEnd(
            [In] ID2D1SvgStrokeDashArray* This,
            [In, ComAliasName("UINT32")] uint dashesCount
        );

        /// <summary>Updates the array. Existing dashes not updated by this method are preserved. The array is resized larger if necessary to accomodate the new dashes.</summary>
        /// <param name="dashes">The dashes array.</param>
        /// <param name="dashesCount">The number of dashes to update.</param>
        /// <param name="startIndex">The index at which to begin updating dashes. Must be less than or equal to the size of the array.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UpdateDashes(
            [In] ID2D1SvgStrokeDashArray* This,
            [In, ComAliasName("FLOAT")] /* readonly */ float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint startIndex
        );

        /// <summary>Updates the array. Existing dashes not updated by this method are preserved. The array is resized larger if necessary to accomodate the new dashes.</summary>
        /// <param name="dashes">The dashes array.</param>
        /// <param name="dashesCount">The number of dashes to update.</param>
        /// <param name="startIndex">The index at which to begin updating dashes. Must be less than or equal to the size of the array.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int UpdateDashes1(
            [In] ID2D1SvgStrokeDashArray* This,
            [In] /* readonly */ D2D1_SVG_LENGTH* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint startIndex
        );

        /// <summary>Gets dashes from the array.</summary>
        /// <param name="dashes">Buffer to contain the dashes.</param>
        /// <param name="dashesCount">The element count of buffer.</param>
        /// <param name="startIndex">The index of the first dash to retrieve.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDashes(
            [In] ID2D1SvgStrokeDashArray* This,
            [Out, ComAliasName("FLOAT")] float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint startIndex
        );

        /// <summary>Gets dashes from the array.</summary>
        /// <param name="dashes">Buffer to contain the dashes.</param>
        /// <param name="dashesCount">The element count of buffer.</param>
        /// <param name="startIndex">The index of the first dash to retrieve.</param>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDashes1(
            [In] ID2D1SvgStrokeDashArray* This,
            [Out] D2D1_SVG_LENGTH* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, DefaultParameterValue(0u), ComAliasName("UINT32")] uint startIndex
        );

        /// <summary>Gets the number of the dashes in the array.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetDashesCount(
            [In] ID2D1SvgStrokeDashArray* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1SvgAttribute.Vtbl BaseVtbl;

            public IntPtr RemoveDashesAtEnd;

            public IntPtr UpdateDashes;

            public IntPtr UpdateDashes1;

            public IntPtr GetDashes;

            public IntPtr GetDashes1;

            public IntPtr GetDashesCount;
            #endregion
        }
        #endregion
    }
}
