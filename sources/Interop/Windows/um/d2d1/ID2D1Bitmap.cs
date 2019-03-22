// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>Root bitmap resource, linearly scaled on a draw call.</summary>
    [Guid("A2296057-EA42-4099-983B-539FB6505426")]
    [Unmanaged]
    public unsafe struct ID2D1Bitmap
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: NativeTypeName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1Bitmap* This,
            [In, NativeTypeName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: NativeTypeName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1Bitmap* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: NativeTypeName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1Bitmap* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetFactory(
            [In] ID2D1Bitmap* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region Delegates
        /// <summary>Returns the size of the bitmap in resolution independent units.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetSize(
            [In] ID2D1Bitmap* This,
            [Out, NativeTypeName("D2D1_SIZE_F")] D2D_SIZE_F* pSize
        );

        /// <summary>Returns the size of the bitmap in resolution dependent units, (pixels).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetPixelSize(
            [In] ID2D1Bitmap* This,
            [Out, NativeTypeName("D2D1_SIZE_U")] D2D_SIZE_U pSize
        );

        /// <summary>Retrieve the format of the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetPixelFormat(
            [In] ID2D1Bitmap* This,
            [Out] D2D1_PIXEL_FORMAT* pPixelFormat
        );

        /// <summary>Return the DPI of the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetDpi(
            [In] ID2D1Bitmap* This,
            [Out, NativeTypeName("FLOAT")] float* dpiX,
            [Out, NativeTypeName("FLOAT")] float* dpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: NativeTypeName("HRESULT")]
        public /* static */ delegate int _CopyFromBitmap(
            [In] ID2D1Bitmap* This,
            [In, Optional, NativeTypeName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1Bitmap* bitmap,
            [In, NativeTypeName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: NativeTypeName("HRESULT")]
        public /* static */ delegate int _CopyFromRenderTarget(
            [In] ID2D1Bitmap* This,
            [In, Optional, NativeTypeName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1RenderTarget* renderTarget,
            [In, NativeTypeName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: NativeTypeName("HRESULT")]
        public /* static */ delegate int _CopyFromMemory(
            [In] ID2D1Bitmap* This,
            [In, Optional, NativeTypeName("D2D1_RECT_U")] D2D_RECT_U* dstRect,
            [In] void* srcData,
            [In, NativeTypeName("UINT32")] uint pitch
        );
        #endregion

        #region IUnknown Methods
        [return: NativeTypeName("HRESULT")]
        public int QueryInterface(
            [In, NativeTypeName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: NativeTypeName("ULONG")]
        public uint AddRef()
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: NativeTypeName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID2D1Resource Methods
        public void GetFactory(
            [Out] ID2D1Factory** factory
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                MarshalFunction<_GetFactory>(lpVtbl->GetFactory)(
                    This,
                    factory
                );
            }
        }
        #endregion

        #region Methods
        public void GetSize(
            [Out, NativeTypeName("D2D1_SIZE_F")] D2D_SIZE_F* pSize
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                MarshalFunction<_GetSize>(lpVtbl->GetSize)(
                    This,
                    pSize
                );
            }
        }

        public void GetPixelSize(
            [Out, NativeTypeName("D2D1_SIZE_U")] D2D_SIZE_U pSize
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                MarshalFunction<_GetPixelSize>(lpVtbl->GetPixelSize)(
                    This,
                    pSize
                );
            }
        }

        public void GetPixelFormat(
            [Out] D2D1_PIXEL_FORMAT* pPixelFormat
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                MarshalFunction<_GetPixelFormat>(lpVtbl->GetPixelFormat)(
                    This,
                    pPixelFormat
                );
            }
        }

        public void GetDpi(
            [Out, NativeTypeName("FLOAT")] float* dpiX,
            [Out, NativeTypeName("FLOAT")] float* dpiY
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                MarshalFunction<_GetDpi>(lpVtbl->GetDpi)(
                    This,
                    dpiX,
                    dpiY
                );
            }
        }

        [return: NativeTypeName("HRESULT")]
        public int CopyFromBitmap(
            [In, Optional, NativeTypeName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1Bitmap* bitmap,
            [In, NativeTypeName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                return MarshalFunction<_CopyFromBitmap>(lpVtbl->CopyFromBitmap)(
                    This,
                    destPoint,
                    bitmap,
                    srcRect
                );
            }
        }

        [return: NativeTypeName("HRESULT")]
        public int CopyFromRenderTarget(
            [In, Optional, NativeTypeName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1RenderTarget* renderTarget,
            [In, NativeTypeName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                return MarshalFunction<_CopyFromRenderTarget>(lpVtbl->CopyFromRenderTarget)(
                    This,
                    destPoint,
                    renderTarget,
                    srcRect
                );
            }
        }

        [return: NativeTypeName("HRESULT")]
        public int CopyFromMemory(
            [In, Optional, NativeTypeName("D2D1_RECT_U")] D2D_RECT_U* dstRect,
            [In] void* srcData,
            [In, NativeTypeName("UINT32")] uint pitch
        )
        {
            fixed (ID2D1Bitmap* This = &this)
            {
                return MarshalFunction<_CopyFromMemory>(lpVtbl->CopyFromMemory)(
                    This,
                    dstRect,
                    srcData,
                    pitch
                );
            }
        }
        #endregion

        #region Structs
        [Unmanaged]
        public struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region Fields
            public IntPtr GetSize;

            public IntPtr GetPixelSize;

            public IntPtr GetPixelFormat;

            public IntPtr GetDpi;

            public IntPtr CopyFromBitmap;

            public IntPtr CopyFromRenderTarget;

            public IntPtr CopyFromMemory;
            #endregion
        }
        #endregion
    }
}
