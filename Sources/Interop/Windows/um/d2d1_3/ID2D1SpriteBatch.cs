// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("4DC583BF-3A10-438A-8722-E9765224F1F1")]
    unsafe public /* blittable */ struct ID2D1SpriteBatch
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Adds sprites to the end of the sprite batch.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddSprites(
            [In] ID2D1SpriteBatch* This,
            [In, ComAliasName("UINT32")] uint spriteCount,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* destinationRectangles,
            [In, ComAliasName("D2D1_RECT_U")] /* readonly */ D2D_RECT_U* sourceRectangles = null,
            [In, ComAliasName("D2D1_COLOR_F")] /* readonly */ DXGI_RGBA* colors = null,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] /* readonly */ D2D_MATRIX_3X2_F* transforms = null,
            [In, ComAliasName("UINT32")] uint destinationRectanglesStride = 16,
            [In, ComAliasName("UINT32")] uint sourceRectanglesStride = 16,
            [In, ComAliasName("UINT32")] uint colorsStride = 16,
            [In, ComAliasName("UINT32")] uint transformsStride = 24
        );

        /// <summary>Set properties for existing sprites. All properties not specified are unmodified.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetSprites(
            [In] ID2D1SpriteBatch* This,
            [In, ComAliasName("UINT32")] uint startIndex,
            [In, ComAliasName("UINT32")] uint spriteCount,
            [In, ComAliasName("D2D1_RECT_F")] /* readonly */ D2D_RECT_F* destinationRectangles = null,
            [In, ComAliasName("D2D1_RECT_U")] /* readonly */ D2D_RECT_U* sourceRectangles = null,
            [In, ComAliasName("D2D1_COLOR_F")] /* readonly */ DXGI_RGBA* colors = null,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] /* readonly */ D2D_MATRIX_3X2_F* transforms = null,
            [In, ComAliasName("UINT32")] uint destinationRectanglesStride = 16,
            [In, ComAliasName("UINT32")] uint sourceRectanglesStride = 16,
            [In, ComAliasName("UINT32")] uint colorsStride = 16,
            [In, ComAliasName("UINT32")] uint transformsStride = 24
        );

        /// <summary>Retrieves sprite properties.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSprites(
            [In] ID2D1SpriteBatch* This,
            [In, ComAliasName("UINT32")] uint startIndex,
            [In, ComAliasName("UINT32")] uint spriteCount,
            [Out, ComAliasName("D2D1_RECT_F")] D2D_RECT_F* destinationRectangles = null,
            [Out, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* sourceRectangles = null,
            [Out, ComAliasName("D2D1_COLOR_F")] DXGI_RGBA* colors = null,
            [Out, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* transforms = null
        );

        /// <summary>Retrieves the number of sprites in the sprite batch.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetSpriteCount(
            [In] ID2D1SpriteBatch* This
        );

        /// <summary>Removes all sprites from the sprite batch.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void Clear(
            [In] ID2D1SpriteBatch* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Resource.Vtbl BaseVtbl;

            public IntPtr AddSprites;

            public IntPtr SetSprites;

            public IntPtr GetSprites;

            public IntPtr GetSpriteCount;

            public IntPtr Clear;
            #endregion
        }
        #endregion
    }
}
