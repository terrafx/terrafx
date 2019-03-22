// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("30961379-4609-4A41-998E-54FE567EE0C1")]
    [Unmanaged]
    public unsafe struct IDXGIResource1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIResource1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIResource1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIResource1* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] IDXGIResource1* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] IDXGIResource1* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] IDXGIResource1* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetParent(
            [In] IDXGIResource1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region IDXGIDeviceSubObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDevice(
            [In] IDXGIResource1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppDevice
        );
        #endregion

        #region IDXGIResource Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSharedHandle(
            [In] IDXGIResource1* This,
            [Out, ComAliasName("HANDLE")] IntPtr* pSharedHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetUsage(
            [In] IDXGIResource1* This,
            [Out, ComAliasName("DXGI_USAGE")] uint* pUsage
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetEvictionPriority(
            [In] IDXGIResource1* This,
            [In, ComAliasName("UINT")] uint EvictionPriority
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetEvictionPriority(
            [In] IDXGIResource1* This,
            [Out, ComAliasName("UINT")] uint* pEvictionPriority
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSubresourceSurface(
            [In] IDXGIResource1* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] IDXGISurface2** ppSurface
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSharedHandle(
            [In] IDXGIResource1* This,
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In, ComAliasName("DWORD")] uint dwAccess,
            [In, Optional, ComAliasName("LPCWSTR")] char* lpName,
            [Out, ComAliasName("HANDLE")] IntPtr* pHandle
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIResource1* This = &this)
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
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IDXGIObject Methods
        [return: ComAliasName("HRESULT")]
        public int SetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_SetPrivateData>(lpVtbl->SetPrivateData)(
                    This,
                    Name,
                    DataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateDataInterface(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_SetPrivateDataInterface>(lpVtbl->SetPrivateDataInterface)(
                    This,
                    Name,
                    pUnknown
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_GetPrivateData>(lpVtbl->GetPrivateData)(
                    This,
                    Name,
                    pDataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetParent(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_GetParent>(lpVtbl->GetParent)(
                    This,
                    riid,
                    ppParent
                );
            }
        }
        #endregion

        #region IDXGIDeviceSubObject Methods
        [return: ComAliasName("HRESULT")]
        public int GetDevice(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppDevice
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_GetDevice>(lpVtbl->GetDevice)(
                    This,
                    riid,
                    ppDevice
                );
            }
        }
        #endregion

        #region IDXGIResource Methods
        [return: ComAliasName("HRESULT")]
        public int GetSharedHandle(
            [Out, ComAliasName("HANDLE")] IntPtr* pSharedHandle
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_GetSharedHandle>(lpVtbl->GetSharedHandle)(
                    This,
                    pSharedHandle
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetUsage(
            [Out, ComAliasName("DXGI_USAGE")] uint* pUsage
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_GetUsage>(lpVtbl->GetUsage)(
                    This,
                    pUsage
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetEvictionPriority(
            [In, ComAliasName("UINT")] uint EvictionPriority
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_SetEvictionPriority>(lpVtbl->SetEvictionPriority)(
                    This,
                    EvictionPriority
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetEvictionPriority(
            [Out, ComAliasName("UINT")] uint* pEvictionPriority
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_GetEvictionPriority>(lpVtbl->GetEvictionPriority)(
                    This,
                    pEvictionPriority
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreateSubresourceSurface(
            [In, ComAliasName("UINT")] uint index,
            [Out] IDXGISurface2** ppSurface
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_CreateSubresourceSurface>(lpVtbl->CreateSubresourceSurface)(
                    This,
                    index,
                    ppSurface
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSharedHandle(
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In, ComAliasName("DWORD")] uint dwAccess,
            [In, Optional, ComAliasName("LPCWSTR")] char* lpName,
            [Out, ComAliasName("HANDLE")] IntPtr* pHandle
        )
        {
            fixed (IDXGIResource1* This = &this)
            {
                return MarshalFunction<_CreateSharedHandle>(lpVtbl->CreateSharedHandle)(
                    This,
                    pAttributes,
                    dwAccess,
                    lpName,
                    pHandle
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

            #region IDXGIObject Fields
            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr GetPrivateData;

            public IntPtr GetParent;
            #endregion

            #region IDXGIDeviceSubObject Fields
            public IntPtr GetDevice;
            #endregion

            #region IDXGIResource Fields
            public IntPtr GetSharedHandle;

            public IntPtr GetUsage;

            public IntPtr SetEvictionPriority;

            public IntPtr GetEvictionPriority;
            #endregion

            #region Fields
            public IntPtr CreateSubresourceSurface;

            public IntPtr CreateSharedHandle;
            #endregion
        }
        #endregion
    }
}

