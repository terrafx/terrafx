// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("25483823-CD46-4C7D-86CA-47AA95B837BD")]
    public /* blittable */ unsafe struct IDXGIFactory3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIFactory3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIFactory3* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetParent(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region IDXGIFactory Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _EnumAdapters(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("UINT")] uint Adapter,
            [Out] IDXGIAdapter** ppAdapter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _MakeWindowAssociation(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HWND")] IntPtr WindowHandle,
            [In, ComAliasName("UINT")] uint Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetWindowAssociation(
            [In] IDXGIFactory3* This,
            [Out, ComAliasName("HWND")] IntPtr* pWindowHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSwapChain(
            [In] IDXGIFactory3* This,
            [In] IUnknown* pDevice,
            [In] DXGI_SWAP_CHAIN_DESC* pDesc,
            [Out] IDXGISwapChain** ppSwapChain
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSoftwareAdapter(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HMODULE")] IntPtr Module,
            [Out] IDXGIAdapter** ppAdapter
        );
        #endregion

        #region IDXGIFactory1 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _EnumAdapters1(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("UINT")] uint Adapter,
            [Out] IDXGIAdapter1** ppAdapter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsCurrent(
            [In] IDXGIFactory3* This
        );
        #endregion

        #region IDXGIFactory2 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsWindowedStereoEnabled(
            [In] IDXGIFactory3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSwapChainForHwnd(
            [In] IDXGIFactory3* This,
            [In] IUnknown* pDevice,
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pFullscreenDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSwapChainForCoreWindow(
            [In] IDXGIFactory3* This,
            [In] IUnknown* pDevice,
            [In] IUnknown* pWindow,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSharedResourceAdapterLuid(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HANDLE")] IntPtr hResource,
            [Out] LUID* pLuid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterStereoStatusWindow(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HWND")] IntPtr WindowHandle,
            [In, ComAliasName("UINT")] uint wMsg,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterStereoStatusEvent(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _UnregisterStereoStatus(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("DWORD")] uint dwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterOcclusionStatusWindow(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HWND")] IntPtr WindowHandle,
            [In, ComAliasName("UINT")] uint wMsg,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterOcclusionStatusEvent(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _UnregisterOcclusionStatus(
            [In] IDXGIFactory3* This,
            [In, ComAliasName("UINT")] uint dwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSwapChainForComposition(
            [In] IDXGIFactory3* This,
            [In] IUnknown* pDevice,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetCreationFlags(
            [In] IDXGIFactory3* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIFactory3* This = &this)
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
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIFactory3* This = &this)
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
            fixed (IDXGIFactory3* This = &this)
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
            fixed (IDXGIFactory3* This = &this)
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
            fixed (IDXGIFactory3* This = &this)
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
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_GetParent>(lpVtbl->GetParent)(
                    This,
                    riid,
                    ppParent
                );
            }
        }
        #endregion

        #region IDXGIFactory Methods
        [return: ComAliasName("HRESULT")]
        public int EnumAdapters(
            [In, ComAliasName("UINT")] uint Adapter,
            [Out] IDXGIAdapter** ppAdapter
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_EnumAdapters>(lpVtbl->EnumAdapters)(
                    This,
                    Adapter,
                    ppAdapter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int MakeWindowAssociation(
            [In, ComAliasName("HWND")] IntPtr WindowHandle,
            [In, ComAliasName("UINT")] uint Flags
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_MakeWindowAssociation>(lpVtbl->MakeWindowAssociation)(
                    This,
                    WindowHandle,
                    Flags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetWindowAssociation(
            [Out, ComAliasName("HWND")] IntPtr* pWindowHandle
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_GetWindowAssociation>(lpVtbl->GetWindowAssociation)(
                    This,
                    pWindowHandle
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSwapChain(
            [In] IUnknown* pDevice,
            [In] DXGI_SWAP_CHAIN_DESC* pDesc,
            [Out] IDXGISwapChain** ppSwapChain
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_CreateSwapChain>(lpVtbl->CreateSwapChain)(
                    This,
                    pDevice,
                    pDesc,
                    ppSwapChain
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSoftwareAdapter(
            [In, ComAliasName("HMODULE")] IntPtr Module,
            [Out] IDXGIAdapter** ppAdapter
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_CreateSoftwareAdapter>(lpVtbl->CreateSoftwareAdapter)(
                    This,
                    Module,
                    ppAdapter
                );
            }
        }
        #endregion

        #region IDXGIFactory1 Methods
        [return: ComAliasName("HRESULT")]
        public int EnumAdapters1(
            [In, ComAliasName("UINT")] uint Adapter,
            [Out] IDXGIAdapter1** ppAdapter
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_EnumAdapters1>(lpVtbl->EnumAdapters1)(
                    This,
                    Adapter,
                    ppAdapter
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int IsCurrent()
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_IsCurrent>(lpVtbl->IsCurrent)(
                    This
                );
            }
        }
        #endregion

        #region IDXGIFactory2 Methods
        [return: ComAliasName("BOOL")]
        public int IsWindowedStereoEnabled()
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_IsWindowedStereoEnabled>(lpVtbl->IsWindowedStereoEnabled)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSwapChainForHwnd(
            [In] IUnknown* pDevice,
            [In, ComAliasName("HWND")] IntPtr hWnd,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pFullscreenDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_CreateSwapChainForHwnd>(lpVtbl->CreateSwapChainForHwnd)(
                    This,
                    pDevice,
                    hWnd,
                    pDesc,
                    pFullscreenDesc,
                    pRestrictToOutput,
                    ppSwapChain
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSwapChainForCoreWindow(
            [In] IUnknown* pDevice,
            [In] IUnknown* pWindow,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_CreateSwapChainForCoreWindow>(lpVtbl->CreateSwapChainForCoreWindow)(
                    This,
                    pDevice,
                    pWindow,
                    pDesc,
                    pRestrictToOutput,
                    ppSwapChain
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSharedResourceAdapterLuid(
            [In, ComAliasName("HANDLE")] IntPtr hResource,
            [Out] LUID* pLuid
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_GetSharedResourceAdapterLuid>(lpVtbl->GetSharedResourceAdapterLuid)(
                    This,
                    hResource,
                    pLuid
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RegisterStereoStatusWindow(
            [In, ComAliasName("HWND")] IntPtr WindowHandle,
            [In, ComAliasName("UINT")] uint wMsg,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_RegisterStereoStatusWindow>(lpVtbl->RegisterStereoStatusWindow)(
                    This,
                    WindowHandle,
                    wMsg,
                    pdwCookie
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RegisterStereoStatusEvent(
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_RegisterStereoStatusEvent>(lpVtbl->RegisterStereoStatusEvent)(
                    This,
                    hEvent,
                    pdwCookie
                );
            }
        }

        public void UnregisterStereoStatus(
            [In, ComAliasName("DWORD")] uint dwCookie
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                MarshalFunction<_UnregisterStereoStatus>(lpVtbl->UnregisterStereoStatus)(
                    This,
                    dwCookie
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RegisterOcclusionStatusWindow(
            [In, ComAliasName("HWND")] IntPtr WindowHandle,
            [In, ComAliasName("UINT")] uint wMsg,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_RegisterOcclusionStatusWindow>(lpVtbl->RegisterOcclusionStatusWindow)(
                    This,
                    WindowHandle,
                    wMsg,
                    pdwCookie
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RegisterOcclusionStatusEvent(
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_RegisterOcclusionStatusEvent>(lpVtbl->RegisterOcclusionStatusEvent)(
                    This,
                    hEvent,
                    pdwCookie
                );
            }
        }

        public void UnregisterOcclusionStatus(
            [In, ComAliasName("UINT")] uint dwCookie
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                MarshalFunction<_UnregisterOcclusionStatus>(lpVtbl->UnregisterOcclusionStatus)(
                    This,
                    dwCookie
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSwapChainForComposition(
            [In] IUnknown* pDevice,
            [In] DXGI_SWAP_CHAIN_DESC1* pDesc,
            [In, Optional] IDXGIOutput* pRestrictToOutput,
            [Out] IDXGISwapChain1** ppSwapChain
        )
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_CreateSwapChainForComposition>(lpVtbl->CreateSwapChainForComposition)(
                    This,
                    pDevice,
                    pDesc,
                    pRestrictToOutput,
                    ppSwapChain
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("UINT")]
        public uint GetCreationFlags()
        {
            fixed (IDXGIFactory3* This = &this)
            {
                return MarshalFunction<_GetCreationFlags>(lpVtbl->GetCreationFlags)(
                    This
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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

            #region IDXGIFactory Fields
            public IntPtr EnumAdapters;

            public IntPtr MakeWindowAssociation;

            public IntPtr GetWindowAssociation;

            public IntPtr CreateSwapChain;

            public IntPtr CreateSoftwareAdapter;
            #endregion

            #region IDXGIFactory1 Fields
            public IntPtr EnumAdapters1;

            public IntPtr IsCurrent;
            #endregion

            #region IDXGIFactory2 Fields
            public IntPtr IsWindowedStereoEnabled;

            public IntPtr CreateSwapChainForHwnd;

            public IntPtr CreateSwapChainForCoreWindow;

            public IntPtr GetSharedResourceAdapterLuid;

            public IntPtr RegisterStereoStatusWindow;

            public IntPtr RegisterStereoStatusEvent;

            public IntPtr UnregisterStereoStatus;

            public IntPtr RegisterOcclusionStatusWindow;

            public IntPtr RegisterOcclusionStatusEvent;

            public IntPtr UnregisterOcclusionStatus;

            public IntPtr CreateSwapChainForComposition;
            #endregion

            #region Fields
            public IntPtr GetCreationFlags;
            #endregion
        }
        #endregion
    }
}

