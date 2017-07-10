// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("4DC583BF-3A10-438A-8722-E9765224F1F1")]
    unsafe public /* blittable */ struct ID2D1SpriteBatch
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Adds sprites to the end of the sprite batch.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddSprites(
            [In] ID2D1SpriteBatch* This,
            [In] UINT32 spriteCount,
            [In] /* readonly */ D2D1_RECT_F* destinationRectangles,
            [In, Optional] /* readonly */ D2D1_RECT_U* sourceRectangles,
            [In, Optional] /* readonly */ D2D1_COLOR_F* colors,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* transforms,
            [In, DefaultParameterValue(16u /* sizeof(D2D1_RECT_F) */)] UINT32 destinationRectanglesStride,
            [In, DefaultParameterValue(16u /* sizeof(D2D1_RECT_U) */)] UINT32 sourceRectanglesStride,
            [In, DefaultParameterValue(16u /* sizeof(D2D1_COLOR_F) */)] UINT32 colorsStride,
            [In, DefaultParameterValue(24u /* sizeof(D2D1_MATRIX_3X2_F) */)] UINT32 transformsStride
        );

        /// <summary>Set properties for existing sprites. All properties not specified are unmodified.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetSprites(
            [In] ID2D1SpriteBatch* This,
            [In] UINT32 startIndex,
            [In] UINT32 spriteCount,
            [In, Optional] /* readonly */ D2D1_RECT_F* destinationRectangles,
            [In, Optional] /* readonly */ D2D1_RECT_U* sourceRectangles,
            [In, Optional] /* readonly */ D2D1_COLOR_F* colors,
            [In, Optional] /* readonly */ D2D1_MATRIX_3X2_F* transforms,
            [In, DefaultParameterValue(16u /* sizeof(D2D1_RECT_F) */)] UINT32 destinationRectanglesStride,
            [In, DefaultParameterValue(16u /* sizeof(D2D1_RECT_U) */)] UINT32 sourceRectanglesStride,
            [In, DefaultParameterValue(16u /* sizeof(D2D1_COLOR_F) */)] UINT32 colorsStride,
            [In, DefaultParameterValue(24u /* sizeof(D2D1_MATRIX_3X2_F) */)] UINT32 transformsStride
        );

        /// <summary>Retrieves sprite properties.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSprites(
            [In] ID2D1SpriteBatch* This,
            [In] UINT32 startIndex,
            [In] UINT32 spriteCount,
            [Out] D2D1_RECT_F* destinationRectangles = null,
            [Out] D2D1_RECT_U* sourceRectangles = null,
            [Out] D2D1_COLOR_F* colors = null,
            [Out] D2D1_MATRIX_3X2_F* transforms = null
        );

        /// <summary>Retrieves the number of sprites in the sprite batch.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetSpriteCount(
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

            public AddSprites AddSprites;

            public SetSprites SetSprites;

            public GetSprites GetSprites;

            public GetSpriteCount GetSpriteCount;

            public Clear Clear;
            #endregion
        }
        #endregion
    }
}
