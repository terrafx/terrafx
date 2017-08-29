// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface that represents text rendering settings for glyph rasterization and filtering.</summary>
    [Guid("B7924BAA-391B-412A-8C5C-E44CC2D867DC")]
    public /* blittable */ unsafe struct IDWriteRenderingParams3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDWriteRenderingParams3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDWriteRenderingParams3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDWriteRenderingParams3* This
        );
        #endregion

        #region IDWriteRenderingParams Delegates
        /// <summary>Gets the gamma value used for gamma correction. Valid values must be greater than zero and cannot exceed 256.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetGamma(
            [In] IDWriteRenderingParams3* This
        );

        /// <summary>Gets the amount of contrast enhancement. Valid values are greater than or equal to zero.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetEnhancedContrast(
            [In] IDWriteRenderingParams3* This
        );

        /// <summary>Gets the ClearType level. Valid values range from 0.0f (no ClearType) to 1.0f (full ClearType).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate float GetClearTypeLevel(
            [In] IDWriteRenderingParams3* This
        );

        /// <summary>Gets the pixel geometry.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_PIXEL_GEOMETRY GetPixelGeometry(
            [In] IDWriteRenderingParams3* This
        );

        /// <summary>Gets the rendering mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_RENDERING_MODE GetRenderingMode(
            [In] IDWriteRenderingParams3* This
        );
        #endregion

        #region IDWriteRenderingParams1 Delegates
        /// <summary>Gets the amount of contrast enhancement to use for grayscale antialiasing. Valid values are greater than or equal to zero.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetGrayscaleEnhancedContrast(
            [In] IDWriteRenderingParams3* This
        );
        #endregion

        #region IDWriteRenderingParams2 Delegates
        /// <summary>Gets the grid fitting mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_GRID_FIT_MODE GetGridFitMode(
            [In] IDWriteRenderingParams3* This
        );
        #endregion

        #region Delegates
        /// <summary>Gets the rendering mode.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate DWRITE_RENDERING_MODE1 GetRenderingMode1(
            [In] IDWriteRenderingParams3* This
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

            #region IDWriteRenderingParams Fields
            public IntPtr GetGamma;

            public IntPtr GetEnhancedContrast;

            public IntPtr GetClearTypeLevel;

            public IntPtr GetPixelGeometry;

            public IntPtr GetRenderingMode;
            #endregion

            #region IDWriteRenderingParams1 Fields
            public IntPtr GetGrayscaleEnhancedContrast;
            #endregion

            #region IDWriteRenderingParams2 Fields
            public IntPtr GetGridFitMode;
            #endregion

            #region Fields
            public IntPtr GetRenderingMode1;
            #endregion
        }
        #endregion
    }
}
