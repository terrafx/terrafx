// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("3AFF9CCE-BE95-4303-B927-E7D16FF4A613")]
    [Unmanaged]
    public unsafe struct IWICPlanarBitmapSourceTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICPlanarBitmapSourceTransform* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICPlanarBitmapSourceTransform* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportTransform(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, Out, ComAliasName("UINT")] uint* puiWidth,
            [In, Out, ComAliasName("UINT")] uint* puiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In, ComAliasName("WICPixelFormatGUID[]")] Guid* pguidDstFormats,
            [Out, ComAliasName("WICBitmapPlaneDescription[]")] WICBitmapPlaneDescription* pPlaneDescriptions,
            [In, ComAliasName("UINT")] uint cPlanes,
            [Out, ComAliasName("BOOL")] int* pfIsSupported
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPixels(
            [In] IWICPlanarBitmapSourceTransform* This,
            [In, Optional] WICRect* prcSource,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In, ComAliasName("WICBitmapPlane[]")] WICBitmapPlane* pDstPlanes,
            [In, ComAliasName("UINT")] uint cPlanes
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICPlanarBitmapSourceTransform* This = &this)
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
            fixed (IWICPlanarBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICPlanarBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int DoesSupportTransform(
            [In, Out, ComAliasName("UINT")] uint* puiWidth,
            [In, Out, ComAliasName("UINT")] uint* puiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In, ComAliasName("WICPixelFormatGUID[]")] Guid* pguidDstFormats,
            [Out, ComAliasName("WICBitmapPlaneDescription[]")] WICBitmapPlaneDescription* pPlaneDescriptions,
            [In, ComAliasName("UINT")] uint cPlanes,
            [Out, ComAliasName("BOOL")] int* pfIsSupported
        )
        {
            fixed (IWICPlanarBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_DoesSupportTransform>(lpVtbl->DoesSupportTransform)(
                    This,
                    puiWidth,
                    puiHeight,
                    dstTransform,
                    dstPlanarOptions,
                    pguidDstFormats,
                    pPlaneDescriptions,
                    cPlanes,
                    pfIsSupported
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyPixels(
            [In, Optional] WICRect* prcSource,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In] WICBitmapTransformOptions dstTransform,
            [In] WICPlanarOptions dstPlanarOptions,
            [In, ComAliasName("WICBitmapPlane[]")] WICBitmapPlane* pDstPlanes,
            [In, ComAliasName("UINT")] uint cPlanes
        )
        {
            fixed (IWICPlanarBitmapSourceTransform* This = &this)
            {
                return MarshalFunction<_CopyPixels>(lpVtbl->CopyPixels)(
                    This,
                    prcSource,
                    uiWidth,
                    uiHeight,
                    dstTransform,
                    dstPlanarOptions,
                    pDstPlanes,
                    cPlanes
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

            #region Fields
            public IntPtr DoesSupportTransform;

            public IntPtr CopyPixels;
            #endregion
        }
        #endregion
    }
}
