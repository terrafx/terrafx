// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>Represents a bitmap that can be used as a surface for an ID2D1DeviceContext or mapped into system memory, and can contain additional color context information.</summary>
    [Guid("A898A84C-3873-4588-B08B-EBBF978DF041")]
    public /* unmanaged */ unsafe struct ID2D1Bitmap1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1Bitmap1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1Bitmap1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1Bitmap1* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetFactory(
            [In] ID2D1Bitmap1* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1Bitmap Delegates
        /// <summary>Returns the size of the bitmap in resolution independent units.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetSize(
            [In] ID2D1Bitmap1* This,
            [Out, ComAliasName("D2D1_SIZE_F")] D2D_SIZE_F* pSize
        );

        /// <summary>Returns the size of the bitmap in resolution dependent units, (pixels).</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetPixelSize(
            [In] ID2D1Bitmap1* This,
            [Out, ComAliasName("D2D1_SIZE_U")] D2D_SIZE_U pSize
        );

        /// <summary>Retrieve the format of the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetPixelFormat(
            [In] ID2D1Bitmap1* This,
            [Out] D2D1_PIXEL_FORMAT* pPixelFormat
        );

        /// <summary>Return the DPI of the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetDpi(
            [In] ID2D1Bitmap1* This,
            [Out, ComAliasName("FLOAT")] float* dpiX,
            [Out, ComAliasName("FLOAT")] float* dpiY
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyFromBitmap(
            [In] ID2D1Bitmap1* This,
            [In, Optional, ComAliasName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1Bitmap* bitmap,
            [In, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyFromRenderTarget(
            [In] ID2D1Bitmap1* This,
            [In, Optional, ComAliasName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1RenderTarget* renderTarget,
            [In, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyFromMemory(
            [In] ID2D1Bitmap1* This,
            [In, Optional, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* dstRect,
            [In] void* srcData,
            [In, ComAliasName("UINT32")] uint pitch
        );
        #endregion

        #region Delegates
        /// <summary>Retrieves the color context information associated with the bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetColorContext(
            [In] ID2D1Bitmap1* This,
            [Out] ID2D1ColorContext** colorContext
        );

        /// <summary>Retrieves the bitmap options used when creating the API.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_BITMAP_OPTIONS _GetOptions(
            [In] ID2D1Bitmap1* This
        );

        /// <summary>Retrieves the DXGI surface from the corresponding bitmap, if the bitmap was created from a device derived from a D3D device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSurface(
            [In] ID2D1Bitmap1* This,
            [Out] IDXGISurface** dxgiSurface
        );

        /// <summary>Maps the given bitmap into memory. The bitmap must have been created with the D2D1_BITMAP_OPTIONS_CPU_READ flag.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Map(
            [In] ID2D1Bitmap1* This,
            [In] D2D1_MAP_OPTIONS options,
            [Out] D2D1_MAPPED_RECT* mappedRect
        );

        /// <summary>Unmaps the given bitmap from memory.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Unmap(
            [In] ID2D1Bitmap1* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1Bitmap1* This = &this)
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
            fixed (ID2D1Bitmap1* This = &this)
            {
                MarshalFunction<_GetFactory>(lpVtbl->GetFactory)(
                    This,
                    factory
                );
            }
        }
        #endregion

        #region ID2D1Bitmap Methods
        public void GetSize(
            [Out, ComAliasName("D2D1_SIZE_F")] D2D_SIZE_F* pSize
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                MarshalFunction<_GetSize>(lpVtbl->GetSize)(
                    This,
                    pSize
                );
            }
        }

        public void GetPixelSize(
            [Out, ComAliasName("D2D1_SIZE_U")] D2D_SIZE_U pSize
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
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
            fixed (ID2D1Bitmap1* This = &this)
            {
                MarshalFunction<_GetPixelFormat>(lpVtbl->GetPixelFormat)(
                    This,
                    pPixelFormat
                );
            }
        }

        public void GetDpi(
            [Out, ComAliasName("FLOAT")] float* dpiX,
            [Out, ComAliasName("FLOAT")] float* dpiY
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                MarshalFunction<_GetDpi>(lpVtbl->GetDpi)(
                    This,
                    dpiX,
                    dpiY
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyFromBitmap(
            [In, Optional, ComAliasName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1Bitmap* bitmap,
            [In, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_CopyFromBitmap>(lpVtbl->CopyFromBitmap)(
                    This,
                    destPoint,
                    bitmap,
                    srcRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyFromRenderTarget(
            [In, Optional, ComAliasName("D2D1_POINT_2U")] D2D_POINT_2U* destPoint,
            [In] ID2D1RenderTarget* renderTarget,
            [In, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* srcRect = null
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_CopyFromRenderTarget>(lpVtbl->CopyFromRenderTarget)(
                    This,
                    destPoint,
                    renderTarget,
                    srcRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyFromMemory(
            [In, Optional, ComAliasName("D2D1_RECT_U")] D2D_RECT_U* dstRect,
            [In] void* srcData,
            [In, ComAliasName("UINT32")] uint pitch
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
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

        #region Methods
        public void GetColorContext(
            [Out] ID2D1ColorContext** colorContext
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                MarshalFunction<_GetColorContext>(lpVtbl->GetColorContext)(
                    This,
                    colorContext
                );
            }
        }

        public D2D1_BITMAP_OPTIONS GetOptions()
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_GetOptions>(lpVtbl->GetOptions)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSurface(
            [Out] IDXGISurface** dxgiSurface
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_GetSurface>(lpVtbl->GetSurface)(
                    This,
                    dxgiSurface
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Map(
            [In] D2D1_MAP_OPTIONS options,
            [Out] D2D1_MAPPED_RECT* mappedRect
        )
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_Map>(lpVtbl->Map)(
                    This,
                    options,
                    mappedRect
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Unmap()
        {
            fixed (ID2D1Bitmap1* This = &this)
            {
                return MarshalFunction<_Unmap>(lpVtbl->Unmap)(
                    This
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region ID2D1Bitmap Fields
            public IntPtr GetSize;

            public IntPtr GetPixelSize;

            public IntPtr GetPixelFormat;

            public IntPtr GetDpi;

            public IntPtr CopyFromBitmap;

            public IntPtr CopyFromRenderTarget;

            public IntPtr CopyFromMemory;
            #endregion

            #region Fields
            public IntPtr GetColorContext;

            public IntPtr GetOptions;

            public IntPtr GetSurface;

            public IntPtr Map;

            public IntPtr Unmap;
            #endregion
        }
        #endregion
    }
}

