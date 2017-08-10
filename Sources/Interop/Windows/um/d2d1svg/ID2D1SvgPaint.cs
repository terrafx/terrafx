// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1svg.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Interface describing an SVG 'fill' or 'stroke' value.</summary>
    [Guid("D59BAB0A-68A2-455B-A5DC-9EB2854E2490")]
    unsafe public /* blittable */ struct ID2D1SvgPaint
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Sets the paint type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPaintType(
            [In] ID2D1SvgPaint* This,
            [In] D2D1_SVG_PAINT_TYPE paintType
        );

        /// <summary>Gets the paint type.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_SVG_PAINT_TYPE GetPaintType(
            [In] ID2D1SvgPaint* This
        );

        /// <summary>Sets the paint color that is used if the paint type is D2D1_SVG_PAINT_TYPE_COLOR.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetColor(
            [In] ID2D1SvgPaint* This,
            [In, ComAliasName("D2D1_COLOR_F")] /* readonly */ DXGI_RGBA* color
        );

        /// <summary>Gets the paint color that is used if the paint type is D2D1_SVG_PAINT_TYPE_COLOR.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetColor(
            [In] ID2D1SvgPaint* This,
            [Out, ComAliasName("D2D1_COLOR_F")] DXGI_RGBA* color
        );

        /// <summary>Sets the element id which acts as the paint server. This id is used if the paint type is D2D1_SVG_PAINT_TYPE_URI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetId(
            [In] ID2D1SvgPaint* This,
            [In, ComAliasName("PCWSTR")] /* readonly */ char* id
        );

        /// <summary>Gets the element id which acts as the paint server. This id is used if the paint type is D2D1_SVG_PAINT_TYPE_URI.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetId(
            [In] ID2D1SvgPaint* This,
            [Out, ComAliasName("PWSTR")] char* id,
            [In, ComAliasName("UINT32")] uint idCount
        );

        /// <summary>Gets the string length of the element id which acts as the paint server. This id is used if the paint type is D2D1_SVG_PAINT_TYPE_URI. The returned string length does not include room for the null terminator.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetIdLength(
            [In] ID2D1SvgPaint* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1SvgAttribute.Vtbl BaseVtbl;

            public IntPtr SetPaintType;

            public IntPtr GetPaintType;

            public IntPtr SetColor;

            public IntPtr GetColor;

            public IntPtr SetId;

            public IntPtr GetId;

            public IntPtr GetIdLength;
            #endregion
        }
        #endregion
    }
}
