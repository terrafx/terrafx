// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Encapsulates a 32-bit device independent bitmap and device context, which can be used for rendering glyphs.</summary>
    [Guid("5E5A32A3-8DFF-4773-9FF6-0696EAB77267")]
    unsafe public /* blittable */ struct IDWriteBitmapRenderTarget
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Draws a run of glyphs to the bitmap.</summary>
        /// <param name="baselineOriginX">Horizontal position of the baseline origin, in DIPs, relative to the upper-left corner of the DIB.</param>
        /// <param name="baselineOriginY">Vertical position of the baseline origin, in DIPs, relative to the upper-left corner of the DIB.</param>
        /// <param name="measuringMode">Specifies measuring mode for glyphs in the run. Renderer implementations may choose different rendering modes for different measuring modes, for example DWRITE_RENDERING_MODE_CLEARTYPE_NATURAL for DWRITE_MEASURING_MODE_NATURAL, DWRITE_RENDERING_MODE_CLEARTYPE_GDI_CLASSIC for DWRITE_MEASURING_MODE_GDI_CLASSIC, and DWRITE_RENDERING_MODE_CLEARTYPE_GDI_NATURAL for DWRITE_MEASURING_MODE_GDI_NATURAL.</param>
        /// <param name="glyphRun">Structure containing the properties of the glyph run.</param>
        /// <param name="renderingParams">Object that controls rendering behavior.</param>
        /// <param name="textColor">Specifies the foreground color of the text.</param>
        /// <param name="blackBoxRect">Optional rectangle that receives the bounding box (in pixels not DIPs) of all the pixels affected by drawing the glyph run. The black box rectangle may extend beyond the dimensions of the bitmap.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawGlyphRun(
            [In] IDWriteBitmapRenderTarget* This,
            [In, ComAliasName("FLOAT")] float baselineOriginX,
            [In, ComAliasName("FLOAT")] float baselineOriginY,
            [In] DWRITE_MEASURING_MODE measuringMode,
            [In] /* readonly */ DWRITE_GLYPH_RUN* glyphRun,
            [In] IDWriteRenderingParams* renderingParams,
            [In, ComAliasName("COLORREF")] uint textColor,
            [Out] RECT* blackBoxRect = null
        );

        /// <summary>Gets a handle to the memory device context.</summary>
        /// <returns>Returns the device context handle.</returns>
        /// <remarks> An application can use the device context to draw using GDI functions. An application can obtain the bitmap handle (HBITMAP) by calling GetCurrentObject. An application that wants information about the underlying bitmap, including a pointer to the pixel data, can call GetObject to fill in a DIBSECTION public structure. The bitmap is always a 32-bit top-down DIB.</remarks>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HDC")]
        public /* static */ delegate void* GetMemoryDC(
            [In] IDWriteBitmapRenderTarget* This
        );

        /// <summary>Gets the number of bitmap pixels per DIP. A DIP (device-independent pixel) is 1/96 inch so this value is the number if pixels per inch divided by 96.</summary>
        /// <returns>Returns the number of bitmap pixels per DIP.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetPixelsPerDip(
            [In] IDWriteBitmapRenderTarget* This
        );

        /// <summary>Sets the number of bitmap pixels per DIP. A DIP (device-independent pixel) is 1/96 inch so this value is the number if pixels per inch divided by 96.</summary>
        /// <param name="pixelsPerDip">Specifies the number of pixels per DIP.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPixelsPerDip(
            [In] IDWriteBitmapRenderTarget* This,
            [In, ComAliasName("FLOAT")] float pixelsPerDip
        );

        /// <summary>Gets the transform that maps abstract coordinate to DIPs. By default this is the identity transform. Note that this is unrelated to the world transform of the underlying device context.</summary>
        /// <param name="transform">Receives the transform.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCurrentTransform(
            [In] IDWriteBitmapRenderTarget* This,
            [Out] DWRITE_MATRIX* transform
        );

        /// <summary>Sets the transform that maps abstract coordinate to DIPs. This does not affect the world transform of the underlying device context.</summary>
        /// <param name="transform">Specifies the new transform. This parameter can be NULL, in which case the identity transform is implied.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetCurrentTransform(
            [In] IDWriteBitmapRenderTarget* This,
            [In, Optional] /* readonly */ DWRITE_MATRIX* transform
        );

        /// <summary>Gets the dimensions of the bitmap.</summary>
        /// <param name="size">Receives the size of the bitmap in pixels.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSize(
            [In] IDWriteBitmapRenderTarget* This,
            [Out] SIZE* size
        );

        /// <summary>Resizes the bitmap.</summary>
        /// <param name="width">New bitmap width, in pixels.</param>
        /// <param name="height">New bitmap height, in pixels.</param>
        /// <returns>Standard HRESULT error code.</returns>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Resize(
            [In] IDWriteBitmapRenderTarget* This,
            [In, ComAliasName("UINT32")] uint width,
            [In, ComAliasName("UINT32")] uint height
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public DrawGlyphRun DrawGlyphRun;

            public GetMemoryDC GetMemoryDC;

            public GetPixelsPerDip GetPixelsPerDip;

            public SetPixelsPerDip SetPixelsPerDip;

            public GetCurrentTransform GetCurrentTransform;

            public SetCurrentTransform SetCurrentTransform;

            public GetSize GetSize;

            public Resize Resize;
            #endregion
        }
        #endregion
    }
}
