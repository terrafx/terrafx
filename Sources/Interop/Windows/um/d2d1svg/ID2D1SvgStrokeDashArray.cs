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
    public /* blittable */ unsafe struct ID2D1SvgStrokeDashArray
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1SvgStrokeDashArray* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1SvgStrokeDashArray* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1SvgStrokeDashArray* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFactory(
            [In] ID2D1SvgStrokeDashArray* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1SvgAttribute Delegates
        /// <summary>Returns the element on which this attribute is set. Returns null if the attribute is not set on any element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetElement(
            [In] ID2D1SvgStrokeDashArray* This,
            [Out] ID2D1SvgElement** element
        );

        /// <summary>Creates a clone of this attribute value. On creation, the cloned attribute is not set on any element.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Clone(
            [In] ID2D1SvgStrokeDashArray* This,
            [Out] ID2D1SvgAttribute** attribute
        );
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
            [In, ComAliasName("FLOAT[]")] float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, ComAliasName("UINT32")] uint startIndex = 0
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
            [In, ComAliasName("D2D1_SVG_LENGTH[]")] D2D1_SVG_LENGTH* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, ComAliasName("UINT32")] uint startIndex = 0
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
            [Out, ComAliasName("FLOAT[]")] float* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, ComAliasName("UINT32")] uint startIndex = 0
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
            [Out, ComAliasName("D2D1_SVG_LENGTH[]")] D2D1_SVG_LENGTH* dashes,
            [In, ComAliasName("UINT32")] uint dashesCount,
            [In, ComAliasName("UINT32")] uint startIndex = 0
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
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region ID2D1SvgAttribute Fields
            public IntPtr GetElement;

            public IntPtr Clone;
            #endregion

            #region Fields
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
