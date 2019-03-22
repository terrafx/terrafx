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
    [Guid("9F34FB65-13F4-4F15-BC57-3726B5E53D9F")]
    [Unmanaged]
    public unsafe struct IWICFormatConverterInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICFormatConverterInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICFormatConverterInfo* This
        );
        #endregion

        #region IWICComponentInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetComponentType(
            [In] IWICFormatConverterInfo* This,
            [Out] WICComponentType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetCLSID(
            [In] IWICFormatConverterInfo* This,
            [Out, ComAliasName("CLSID")] Guid* pclsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSigningStatus(
            [In] IWICFormatConverterInfo* This,
            [Out, ComAliasName("DWORD")] uint* pStatus
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetAuthor(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchAuthor,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzAuthor,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVendorGUID(
            [In] IWICFormatConverterInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidVendor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetVersion(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSpecVersion(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchSpecVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzSpecVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFriendlyName(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchFriendlyName,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFriendlyName,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPixelFormats(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("WICPixelFormatGUID[]")] Guid* pPixelFormatGUIDs,
            [Out, ComAliasName("UINT")] uint* pcActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateInstance(
            [In] IWICFormatConverterInfo* This,
            [Out] IWICFormatConverter** ppIConverter = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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
            fixed (IWICFormatConverterInfo* This = &this)
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

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetPixelFormats(
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("WICPixelFormatGUID[]")] Guid* pPixelFormatGUIDs,
            [Out, ComAliasName("UINT")] uint* pcActual
        )
        {
            fixed (IWICFormatConverterInfo* This = &this)
            {
                return MarshalFunction<_GetPixelFormats>(lpVtbl->GetPixelFormats)(
                    This,
                    cFormats,
                    pPixelFormatGUIDs,
                    pcActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateInstance(
            [Out] IWICFormatConverter** ppIConverter = null
        )
        {
            fixed (IWICFormatConverterInfo* This = &this)
            {
                return MarshalFunction<_CreateInstance>(lpVtbl->CreateInstance)(
                    This,
                    ppIConverter
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

            #region Fields
            public IntPtr GetPixelFormats;

            public IntPtr CreateInstance;
            #endregion
        }
        #endregion
    }
}
