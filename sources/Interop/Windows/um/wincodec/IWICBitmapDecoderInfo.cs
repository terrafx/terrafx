// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("D8CD007F-D08F-4191-9BFC-236EA7F0E4B5")]
    public /* unmanaged */ unsafe struct IWICBitmapDecoderInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICBitmapDecoderInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICBitmapDecoderInfo* This
        );
        #endregion

        #region IWICComponentInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetComponentType(
            [In] IWICBitmapDecoderInfo* This,
            [Out] WICComponentType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetCLSID(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("CLSID")] Guid* pclsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSigningStatus(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("DWORD")] uint* pStatus
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetAuthor(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchAuthor,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzAuthor,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVendorGUID(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidVendor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVersion(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSpecVersion(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchSpecVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzSpecVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFriendlyName(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchFriendlyName,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFriendlyName,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );
        #endregion

        #region IWICBitmapCodecInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetContainerFormat(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPixelFormats(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("GUID[]")] Guid* pguidPixelFormats,
            [Out, ComAliasName("UINT")] uint* pcActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetColorManagementVersion(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchColorManagementVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzColorManagementVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDeviceManufacturer(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchDeviceManufacturer,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzDeviceManufacturer,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDeviceModels(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchDeviceModels,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzDeviceModels,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMimeTypes(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchMimeTypes,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzMimeTypes,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFileExtensions(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchFileExtensions,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFileExtensions,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportAnimation(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportAnimation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportChromakey(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportChromakey
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportLossless(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportLossless
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportMultiframe(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportMultiframe
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _MatchesMimeType(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("LPCWSTR")] char* wzMimeType,
            [Out, ComAliasName("BOOL")] int* pfMatches
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPatterns(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cbSizePatterns,
            [Out, Optional, ComAliasName("WICBitmapPattern[]")] WICBitmapPattern* pPatterns,
            [Out, Optional, ComAliasName("UINT")] uint* pcPatterns,
            [Out, ComAliasName("UINT")] uint* pcbPatternsActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _MatchesPattern(
            [In] IWICBitmapDecoderInfo* This,
            [In, Optional] IStream* pIStream,
            [Out, ComAliasName("BOOL")] int* pfMatches
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateInstance(
            [In] IWICBitmapDecoderInfo* This,
            [Out] IWICBitmapDecoder** ppIBitmapDecoder = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
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
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IWICComponentInfo Methods
        [return: ComAliasName("HRESULT")]
        public int GetComponentType(
            [Out] WICComponentType* pType
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetComponentType>(lpVtbl->GetComponentType)(
                    This,
                    pType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetCLSID(
            [Out, ComAliasName("CLSID")] Guid* pclsid
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetCLSID>(lpVtbl->GetCLSID)(
                    This,
                    pclsid
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSigningStatus(
            [Out, ComAliasName("DWORD")] uint* pStatus
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetSigningStatus>(lpVtbl->GetSigningStatus)(
                    This,
                    pStatus
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetAuthor(
            [In, ComAliasName("UINT")] uint cchAuthor,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzAuthor,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetAuthor>(lpVtbl->GetAuthor)(
                    This,
                    cchAuthor,
                    wzAuthor,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetVendorGUID(
            [Out, ComAliasName("GUID")] Guid* pguidVendor
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetVendorGUID>(lpVtbl->GetVendorGUID)(
                    This,
                    pguidVendor
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetVersion(
            [In, ComAliasName("UINT")] uint cchVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetVersion>(lpVtbl->GetVersion)(
                    This,
                    cchVersion,
                    wzVersion,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSpecVersion(
            [In, ComAliasName("UINT")] uint cchSpecVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzSpecVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetSpecVersion>(lpVtbl->GetSpecVersion)(
                    This,
                    cchSpecVersion,
                    wzSpecVersion,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFriendlyName(
            [In, ComAliasName("UINT")] uint cchFriendlyName,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFriendlyName,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetFriendlyName>(lpVtbl->GetFriendlyName)(
                    This,
                    cchFriendlyName,
                    wzFriendlyName,
                    pcchActual
                );
            }
        }
        #endregion

        #region IWICBitmapCodecInfo Methods
        [return: ComAliasName("HRESULT")]
        public int GetContainerFormat(
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetContainerFormat>(lpVtbl->GetContainerFormat)(
                    This,
                    pguidContainerFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPixelFormats(
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("GUID[]")] Guid* pguidPixelFormats,
            [Out, ComAliasName("UINT")] uint* pcActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetPixelFormats>(lpVtbl->GetPixelFormats)(
                    This,
                    cFormats,
                    pguidPixelFormats,
                    pcActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetColorManagementVersion(
            [In, ComAliasName("UINT")] uint cchColorManagementVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzColorManagementVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetColorManagementVersion>(lpVtbl->GetColorManagementVersion)(
                    This,
                    cchColorManagementVersion,
                    wzColorManagementVersion,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDeviceManufacturer(
            [In, ComAliasName("UINT")] uint cchDeviceManufacturer,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzDeviceManufacturer,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetDeviceManufacturer>(lpVtbl->GetDeviceManufacturer)(
                    This,
                    cchDeviceManufacturer,
                    wzDeviceManufacturer,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDeviceModels(
            [In, ComAliasName("UINT")] uint cchDeviceModels,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzDeviceModels,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetDeviceModels>(lpVtbl->GetDeviceModels)(
                    This,
                    cchDeviceModels,
                    wzDeviceModels,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMimeTypes(
            [In, ComAliasName("UINT")] uint cchMimeTypes,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzMimeTypes,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetMimeTypes>(lpVtbl->GetMimeTypes)(
                    This,
                    cchMimeTypes,
                    wzMimeTypes,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFileExtensions(
            [In, ComAliasName("UINT")] uint cchFileExtensions,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFileExtensions,
            [Out, ComAliasName("UINT")] uint* pcchActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetFileExtensions>(lpVtbl->GetFileExtensions)(
                    This,
                    cchFileExtensions,
                    wzFileExtensions,
                    pcchActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DoesSupportAnimation(
            [Out, ComAliasName("BOOL")] int* pfSupportAnimation
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_DoesSupportAnimation>(lpVtbl->DoesSupportAnimation)(
                    This,
                    pfSupportAnimation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DoesSupportChromakey(
            [Out, ComAliasName("BOOL")] int* pfSupportChromakey
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_DoesSupportChromakey>(lpVtbl->DoesSupportChromakey)(
                    This,
                    pfSupportChromakey
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DoesSupportLossless(
            [Out, ComAliasName("BOOL")] int* pfSupportLossless
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_DoesSupportLossless>(lpVtbl->DoesSupportLossless)(
                    This,
                    pfSupportLossless
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DoesSupportMultiframe(
            [Out, ComAliasName("BOOL")] int* pfSupportMultiframe
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_DoesSupportMultiframe>(lpVtbl->DoesSupportMultiframe)(
                    This,
                    pfSupportMultiframe
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int MatchesMimeType(
            [In, ComAliasName("LPCWSTR")] char* wzMimeType,
            [Out, ComAliasName("BOOL")] int* pfMatches
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_MatchesMimeType>(lpVtbl->MatchesMimeType)(
                    This,
                    wzMimeType,
                    pfMatches
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetPatterns(
            [In, ComAliasName("UINT")] uint cbSizePatterns,
            [Out, Optional, ComAliasName("WICBitmapPattern[]")] WICBitmapPattern* pPatterns,
            [Out, Optional, ComAliasName("UINT")] uint* pcPatterns,
            [Out, ComAliasName("UINT")] uint* pcbPatternsActual
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_GetPatterns>(lpVtbl->GetPatterns)(
                    This,
                    cbSizePatterns,
                    pPatterns,
                    pcPatterns,
                    pcbPatternsActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int MatchesPattern(
            [In, Optional] IStream* pIStream,
            [Out, ComAliasName("BOOL")] int* pfMatches
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_MatchesPattern>(lpVtbl->MatchesPattern)(
                    This,
                    pIStream,
                    pfMatches
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateInstance(
            [Out] IWICBitmapDecoder** ppIBitmapDecoder = null
        )
        {
            fixed (IWICBitmapDecoderInfo* This = &this)
            {
                return MarshalFunction<_CreateInstance>(lpVtbl->CreateInstance)(
                    This,
                    ppIBitmapDecoder
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

            #region IWICComponentInfo Fields
            public IntPtr GetComponentType;

            public IntPtr GetCLSID;

            public IntPtr GetSigningStatus;

            public IntPtr GetAuthor;

            public IntPtr GetVendorGUID;

            public IntPtr GetVersion;

            public IntPtr GetSpecVersion;

            public IntPtr GetFriendlyName;
            #endregion

            #region IWICBitmapCodecInfo Fields
            public IntPtr GetContainerFormat;

            public IntPtr GetPixelFormats;

            public IntPtr GetColorManagementVersion;

            public IntPtr GetDeviceManufacturer;

            public IntPtr GetDeviceModels;

            public IntPtr GetMimeTypes;

            public IntPtr GetFileExtensions;

            public IntPtr DoesSupportAnimation;

            public IntPtr DoesSupportChromakey;

            public IntPtr DoesSupportLossless;

            public IntPtr DoesSupportMultiframe;

            public IntPtr MatchesMimeType;
            #endregion

            #region Fields
            public IntPtr GetPatterns;

            public IntPtr MatchesPattern;

            public IntPtr CreateInstance;
            #endregion
        }
        #endregion
    }
}

