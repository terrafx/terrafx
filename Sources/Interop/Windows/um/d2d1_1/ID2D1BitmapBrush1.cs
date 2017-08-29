// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A bitmap brush allows a bitmap to be used to fill a geometry.  Interpolation mode is specified with D2D1_INTERPOLATION_MODE</summary>
    [Guid("41343A53-E41A-49A2-91CD-21793BBB62E5")]
    public /* blittable */ unsafe struct ID2D1BitmapBrush1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1BitmapBrush1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1BitmapBrush1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1BitmapBrush1* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFactory(
            [In] ID2D1BitmapBrush1* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1Brush Delegates
        /// <summary>Sets the opacity for when the brush is drawn over the entire fill of the brush.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetOpacity(
            [In] ID2D1BitmapBrush1* This,
            [In, ComAliasName("FLOAT")] float opacity
        );

        /// <summary>Sets the transform that applies to everything drawn by the brush.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetTransform(
            [In] ID2D1BitmapBrush1* This,
            [In, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* transform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("FLOAT")]
        public /* static */ delegate float GetOpacity(
            [In] ID2D1BitmapBrush1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetTransform(
            [In] ID2D1BitmapBrush1* This,
            [Out, ComAliasName("D2D1_MATRIX_3X2_F")] D2D_MATRIX_3X2_F* transform
        );
        #endregion

        #region ID2D1BitmapBrush Delegates
        /// <summary>Sets how the bitmap is to be treated outside of its natural extent on the X axis.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetExtendModeX(
            [In] ID2D1BitmapBrush1* This,
            [In] D2D1_EXTEND_MODE extendModeX
        );

        /// <summary>Sets how the bitmap is to be treated outside of its natural extent on the X axis.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetExtendModeY(
            [In] ID2D1BitmapBrush1* This,
            [In] D2D1_EXTEND_MODE extendModeY
        );

        /// <summary>Sets the interpolation mode used when this brush is used.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetInterpolationMode(
            [In] ID2D1BitmapBrush1* This,
            [In] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode
        );

        /// <summary>Sets the bitmap associated as the source of this brush.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetBitmap(
            [In] ID2D1BitmapBrush1* This,
            [In] ID2D1Bitmap* bitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_EXTEND_MODE GetExtendModeX(
            [In] ID2D1BitmapBrush1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_EXTEND_MODE GetExtendModeY(
            [In] ID2D1BitmapBrush1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_BITMAP_INTERPOLATION_MODE GetInterpolationMode(
            [In] ID2D1BitmapBrush1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetBitmap(
            [In] ID2D1BitmapBrush1* This,
            [Out] ID2D1Bitmap** bitmap
        );
        #endregion

        #region Delegates
        /// <summary>Sets the interpolation mode used when this brush is used.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetInterpolationMode1(
            [In] ID2D1BitmapBrush1* This,
            [In] D2D1_INTERPOLATION_MODE interpolationMode
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_INTERPOLATION_MODE GetInterpolationMode1(
            [In] ID2D1BitmapBrush1* This
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

            #region ID2D1Brush Fields
            public IntPtr SetOpacity;

            public IntPtr SetTransform;

            public IntPtr GetOpacity;

            public IntPtr GetTransform;
            #endregion

            #region ID2D1BitmapBrush Fields
            public IntPtr SetExtendModeX;

            public IntPtr SetExtendModeY;

            public IntPtr SetInterpolationMode;

            public IntPtr SetBitmap;

            public IntPtr GetExtendModeX;

            public IntPtr GetExtendModeY;

            public IntPtr GetInterpolationMode;

            public IntPtr GetBitmap;
            #endregion

            #region Fields
            public IntPtr SetInterpolationMode1;

            public IntPtr GetInterpolationMode1;
            #endregion
        }
        #endregion
    }
}
