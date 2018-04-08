// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("A9DB33A2-AF5F-43C7-B679-74F5984B5AA4")]
    public /* unmanaged */ unsafe struct IWICPixelFormatInfo2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICPixelFormatInfo2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICPixelFormatInfo2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICPixelFormatInfo2* This
        );
        #endregion

        #region IWICComponentInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetComponentType(
            [In] IWICPixelFormatInfo2* This,
            [Out] WICComponentType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetCLSID(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("CLSID")] Guid* pclsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSigningStatus(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("DWORD")] uint* pStatus
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetAuthor(
            [In] IWICPixelFormatInfo2* This,
            [In, ComAliasName("UINT")] uint cchAuthor,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzAuthor,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVendorGUID(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("GUID")] Guid* pguidVendor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVersion(
            [In] IWICPixelFormatInfo2* This,
            [In, ComAliasName("UINT")] uint cchVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSpecVersion(
            [In] IWICPixelFormatInfo2* This,
            [In, ComAliasName("UINT")] uint cchSpecVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzSpecVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFriendlyName(
            [In] IWICPixelFormatInfo2* This,
            [In, ComAliasName("UINT")] uint cchFriendlyName,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFriendlyName,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );
        #endregion

        #region IWICPixelFormatInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFormatGUID(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("GUID")] Guid* pFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetColorContext(
            [In] IWICPixelFormatInfo2* This,
            [Out] IWICColorContext** ppIColorContext = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetBitsPerPixel(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("UINT")] uint* puiBitsPerPixel
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetChannelCount(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("UINT")] uint* puiChannelCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetChannelMask(
            [In] IWICPixelFormatInfo2* This,
            [In, ComAliasName("UINT")] uint uiChannelIndex,
            [In, ComAliasName("UINT")] uint cbMaskBuffer,
            [In, Out, Optional, ComAliasName("BYTE[]")] byte* pbMaskBuffer,
            [Out, ComAliasName("UINT")] uint* pcbActual
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SupportsTransparency(
            [In] IWICPixelFormatInfo2* This,
            [Out, ComAliasName("BOOL")] int* pfSupportsTransparency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetNumericRepresentation(
            [In] IWICPixelFormatInfo2* This,
            [Out] WICPixelFormatNumericRepresentation* pNumericRepresentation
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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
            fixed (IWICPixelFormatInfo2* This = &this)
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

        #region IWICPixelFormatInfo Methods
        [return: ComAliasName("HRESULT")]
        public int GetFormatGUID(
            [Out, ComAliasName("GUID")] Guid* pFormat
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_GetFormatGUID>(lpVtbl->GetFormatGUID)(
                    This,
                    pFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetColorContext(
            [Out] IWICColorContext** ppIColorContext = null
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_GetColorContext>(lpVtbl->GetColorContext)(
                    This,
                    ppIColorContext
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetBitsPerPixel(
            [Out, ComAliasName("UINT")] uint* puiBitsPerPixel
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_GetBitsPerPixel>(lpVtbl->GetBitsPerPixel)(
                    This,
                    puiBitsPerPixel
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetChannelCount(
            [Out, ComAliasName("UINT")] uint* puiChannelCount
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_GetChannelCount>(lpVtbl->GetChannelCount)(
                    This,
                    puiChannelCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetChannelMask(
            [In, ComAliasName("UINT")] uint uiChannelIndex,
            [In, ComAliasName("UINT")] uint cbMaskBuffer,
            [In, Out, Optional, ComAliasName("BYTE[]")] byte* pbMaskBuffer,
            [Out, ComAliasName("UINT")] uint* pcbActual
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_GetChannelMask>(lpVtbl->GetChannelMask)(
                    This,
                    uiChannelIndex,
                    cbMaskBuffer,
                    pbMaskBuffer,
                    pcbActual
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int SupportsTransparency(
            [Out, ComAliasName("BOOL")] int* pfSupportsTransparency
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_SupportsTransparency>(lpVtbl->SupportsTransparency)(
                    This,
                    pfSupportsTransparency
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetNumericRepresentation(
            [Out] WICPixelFormatNumericRepresentation* pNumericRepresentation
        )
        {
            fixed (IWICPixelFormatInfo2* This = &this)
            {
                return MarshalFunction<_GetNumericRepresentation>(lpVtbl->GetNumericRepresentation)(
                    This,
                    pNumericRepresentation
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

            #region IWICPixelFormatInfo Fields
            public IntPtr GetFormatGUID;

            public IntPtr GetColorContext;

            public IntPtr GetBitsPerPixel;

            public IntPtr GetChannelCount;

            public IntPtr GetChannelMask;
            #endregion

            #region Fields
            public IntPtr SupportsTransparency;

            public IntPtr GetNumericRepresentation;
            #endregion
        }
        #endregion
    }
}

