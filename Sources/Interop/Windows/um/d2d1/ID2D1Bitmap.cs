// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Root bitmap resource, linearly scaled on a draw call.</summary>
    [Guid("A2296057-EA42-4099-983B-539FB6505426")]
    unsafe public /* blittable */ struct ID2D1Bitmap
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns the size of the bitmap in resolution independent units.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_SIZE_F GetSize(
            [In] ID2D1Bitmap* This
        );

        /// <summary>Returns the size of the bitmap in resolution dependent units, (pixels).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_SIZE_U GetPixelSize(
            [In] ID2D1Bitmap* This
        );

        /// <summary>Retrieve the format of the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_PIXEL_FORMAT GetPixelFormat(
            [In] ID2D1Bitmap* This
        );

        /// <summary>Return the DPI of the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDpi(
            [In] ID2D1Bitmap* This,
            [Out] FLOAT* dpiX,
            [Out] FLOAT* dpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyFromBitmap(
            [In] ID2D1Bitmap* This,
            [In, Optional] /* readonly */ D2D1_POINT_2U* destPoint,
            [In] ID2D1Bitmap* bitmap,
            [In, Optional] /* readonly */ D2D1_RECT_U* srcRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyFromRenderTarget(
            [In] ID2D1Bitmap* This,
            [In, Optional] /* readonly */ D2D1_POINT_2U* destPoint,
            [In] ID2D1RenderTarget* renderTarget,
            [In, Optional] /* readonly */ D2D1_RECT_U* srcRect
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyFromMemory(
            [In] ID2D1Bitmap* This,
            [In, Optional] /* readonly */ D2D1_RECT_U* dstRect,
            [In] /* readonly */ void* srcData,
            [In] UINT32 pitch
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Image.Vtbl BaseVtbl;

            public GetSize GetSize;

            public GetPixelSize GetPixelSize;

            public GetPixelFormat GetPixelFormat;

            public GetDpi GetDpi;

            public CopyFromBitmap CopyFromBitmap;

            public CopyFromRenderTarget CopyFromRenderTarget;

            public CopyFromMemory CopyFromMemory;
            #endregion
        }
        #endregion
    }
}
