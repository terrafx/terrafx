// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("94D99BDB-F1F8-4AB0-B236-7DA0170EDAB1")]
    [Unmanaged]
    public unsafe struct IDXGISwapChain3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGISwapChain3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGISwapChain3* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetParent(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region IDXGIDeviceSubObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDevice(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppDevice
        );
        #endregion

        #region IDXGISwapChain Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Present(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint SyncInterval,
            [In, ComAliasName("UINT")] uint Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetBuffer(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint Buffer,
            [In, ComAliasName("REFIID")] Guid* riid,
            [In, Out] void** ppSurface
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetFullscreenState(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("BOOL")] int Fullscreen,
            [In] IDXGIOutput* pTarget = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFullscreenState(
            [In] IDXGISwapChain3* This,
            [Out, ComAliasName("BOOL")] int* pFullscreen = null,
            [Out] IDXGIOutput** ppTarget = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_SWAP_CHAIN_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ResizeBuffers(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint BufferCount,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height,
            [In] DXGI_FORMAT NewFormat,
            [In, ComAliasName("UINT")] uint SwapChainFlags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ResizeTarget(
            [In] IDXGISwapChain3* This,
            [In] DXGI_MODE_DESC* pNewTargetParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetContainingOutput(
            [In] IDXGISwapChain3* This,
            [Out] IDXGIOutput** ppOutput
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameStatistics(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_FRAME_STATISTICS* pStats
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetLastPresentCount(
            [In] IDXGISwapChain3* This,
            [Out, ComAliasName("UINT")] uint* pLastPresentCount
        );
        #endregion

        #region IDXGISwapChain1 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc1(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_SWAP_CHAIN_DESC1* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFullscreenDesc(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetHwnd(
            [In] IDXGISwapChain3* This,
            [Out, ComAliasName("HWND")] IntPtr* pHwnd
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetCoreWindow(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("REFIID")] Guid* refiid,
            [Out] void** ppUnk
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Present1(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint SyncInterval,
            [In, ComAliasName("UINT")] uint PresentFlags,
            [In] DXGI_PRESENT_PARAMETERS* pPresentParameters
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsTemporaryMonoSupported(
            [In] IDXGISwapChain3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetRestrictToOutput(
            [In] IDXGISwapChain3* This,
            [Out] IDXGIOutput** ppRestrictToOutput
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetBackgroundColor(
            [In] IDXGISwapChain3* This,
            [In] DXGI_RGBA* pColor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetBackgroundColor(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_RGBA* pColor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetRotation(
            [In] IDXGISwapChain3* This,
            [In] DXGI_MODE_ROTATION Rotation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetRotation(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_MODE_ROTATION* pRotation
        );
        #endregion

        #region IDXGISwapChain2 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetSourceSize(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetSourceSize(
            [In] IDXGISwapChain3* This,
            [Out, ComAliasName("UINT")] uint* pWidth,
            [Out, ComAliasName("UINT")] uint* pHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetMaximumFrameLatency(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint MaxLatency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMaximumFrameLatency(
            [In] IDXGISwapChain3* This,
            [Out, ComAliasName("UINT")] uint* pMaxLatency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HANDLE")]
        public /* static */ delegate IntPtr _GetFrameLatencyWaitableObject(
            [In] IDXGISwapChain3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetMatrixTransform(
            [In] IDXGISwapChain3* This,
            [In] DXGI_MATRIX_3X2_F* pMatrix
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMatrixTransform(
            [In] IDXGISwapChain3* This,
            [Out] DXGI_MATRIX_3X2_F* pMatrix
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetCurrentBackBufferIndex(
            [In] IDXGISwapChain3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CheckColorSpaceSupport(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [Out, ComAliasName("UINT")] uint* pColorSpaceSupport
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetColorSpace1(
            [In] IDXGISwapChain3* This,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ResizeBuffers1(
            [In] IDXGISwapChain3* This,
            [In, ComAliasName("UINT")] uint BufferCount,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height,
            [In] DXGI_FORMAT Format,
            [In, ComAliasName("UINT")] uint SwapChainFlags,
            [In, ComAliasName("UINT[]")] uint* pCreationNodeMask,
            [In, ComAliasName("IUnknown*[]")] IUnknown** ppPresentQueue
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGISwapChain3* This = &this)
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
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGISwapChain3* This = &this)
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
            fixed (IDXGISwapChain3* This = &this)
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
            fixed (IDXGISwapChain3* This = &this)
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
            fixed (IDXGISwapChain3* This = &this)
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
            fixed (IDXGISwapChain3* This = &this)
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
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetDevice>(lpVtbl->GetDevice)(
                    This,
                    riid,
                    ppDevice
                );
            }
        }
        #endregion

        #region IDXGISwapChain Methods
        [return: ComAliasName("HRESULT")]
        public int Present(
            [In, ComAliasName("UINT")] uint SyncInterval,
            [In, ComAliasName("UINT")] uint Flags
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_Present>(lpVtbl->Present)(
                    This,
                    SyncInterval,
                    Flags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetBuffer(
            [In, ComAliasName("UINT")] uint Buffer,
            [In, ComAliasName("REFIID")] Guid* riid,
            [In, Out] void** ppSurface
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetBuffer>(lpVtbl->GetBuffer)(
                    This,
                    Buffer,
                    riid,
                    ppSurface
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetFullscreenState(
            [In, ComAliasName("BOOL")] int Fullscreen,
            [In] IDXGIOutput* pTarget = null
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetFullscreenState>(lpVtbl->SetFullscreenState)(
                    This,
                    Fullscreen,
                    pTarget
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFullscreenState(
            [Out, ComAliasName("BOOL")] int* pFullscreen = null,
            [Out] IDXGIOutput** ppTarget = null
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetFullscreenState>(lpVtbl->GetFullscreenState)(
                    This,
                    pFullscreen,
                    ppTarget
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDesc(
            [Out] DXGI_SWAP_CHAIN_DESC* pDesc
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ResizeBuffers(
            [In, ComAliasName("UINT")] uint BufferCount,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height,
            [In] DXGI_FORMAT NewFormat,
            [In, ComAliasName("UINT")] uint SwapChainFlags
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_ResizeBuffers>(lpVtbl->ResizeBuffers)(
                    This,
                    BufferCount,
                    Width,
                    Height,
                    NewFormat,
                    SwapChainFlags
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ResizeTarget(
            [In] DXGI_MODE_DESC* pNewTargetParameters
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_ResizeTarget>(lpVtbl->ResizeTarget)(
                    This,
                    pNewTargetParameters
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetContainingOutput(
            [Out] IDXGIOutput** ppOutput
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetContainingOutput>(lpVtbl->GetContainingOutput)(
                    This,
                    ppOutput
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrameStatistics(
            [Out] DXGI_FRAME_STATISTICS* pStats
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetFrameStatistics>(lpVtbl->GetFrameStatistics)(
                    This,
                    pStats
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetLastPresentCount(
            [Out, ComAliasName("UINT")] uint* pLastPresentCount
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetLastPresentCount>(lpVtbl->GetLastPresentCount)(
                    This,
                    pLastPresentCount
                );
            }
        }
        #endregion

        #region IDXGISwapChain1 Methods
        [return: ComAliasName("HRESULT")]
        public int GetDesc1(
            [Out] DXGI_SWAP_CHAIN_DESC1* pDesc
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetDesc1>(lpVtbl->GetDesc1)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFullscreenDesc(
            [Out] DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pDesc
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetFullscreenDesc>(lpVtbl->GetFullscreenDesc)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetHwnd(
            [Out, ComAliasName("HWND")] IntPtr* pHwnd
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetHwnd>(lpVtbl->GetHwnd)(
                    This,
                    pHwnd
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetCoreWindow(
            [In, ComAliasName("REFIID")] Guid* refiid,
            [Out] void** ppUnk
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetCoreWindow>(lpVtbl->GetCoreWindow)(
                    This,
                    refiid,
                    ppUnk
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Present1(
            [In, ComAliasName("UINT")] uint SyncInterval,
            [In, ComAliasName("UINT")] uint PresentFlags,
            [In] DXGI_PRESENT_PARAMETERS* pPresentParameters
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_Present1>(lpVtbl->Present1)(
                    This,
                    SyncInterval,
                    PresentFlags,
                    pPresentParameters
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int IsTemporaryMonoSupported()
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_IsTemporaryMonoSupported>(lpVtbl->IsTemporaryMonoSupported)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRestrictToOutput(
            [Out] IDXGIOutput** ppRestrictToOutput
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetRestrictToOutput>(lpVtbl->GetRestrictToOutput)(
                    This,
                    ppRestrictToOutput
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetBackgroundColor(
            [In] DXGI_RGBA* pColor
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetBackgroundColor>(lpVtbl->SetBackgroundColor)(
                    This,
                    pColor
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetBackgroundColor(
            [Out] DXGI_RGBA* pColor
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetBackgroundColor>(lpVtbl->GetBackgroundColor)(
                    This,
                    pColor
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetRotation(
            [In] DXGI_MODE_ROTATION Rotation
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetRotation>(lpVtbl->SetRotation)(
                    This,
                    Rotation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRotation(
            [Out] DXGI_MODE_ROTATION* pRotation
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetRotation>(lpVtbl->GetRotation)(
                    This,
                    pRotation
                );
            }
        }
        #endregion

        #region IDXGISwapChain2 Methods
        [return: ComAliasName("HRESULT")]
        public int SetSourceSize(
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetSourceSize>(lpVtbl->SetSourceSize)(
                    This,
                    Width,
                    Height
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetSourceSize(
            [Out, ComAliasName("UINT")] uint* pWidth,
            [Out, ComAliasName("UINT")] uint* pHeight
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetSourceSize>(lpVtbl->GetSourceSize)(
                    This,
                    pWidth,
                    pHeight
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetMaximumFrameLatency(
            [In, ComAliasName("UINT")] uint MaxLatency
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetMaximumFrameLatency>(lpVtbl->SetMaximumFrameLatency)(
                    This,
                    MaxLatency
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMaximumFrameLatency(
            [Out, ComAliasName("UINT")] uint* pMaxLatency
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetMaximumFrameLatency>(lpVtbl->GetMaximumFrameLatency)(
                    This,
                    pMaxLatency
                );
            }
        }

        [return: ComAliasName("HANDLE")]
        public IntPtr GetFrameLatencyWaitableObject()
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetFrameLatencyWaitableObject>(lpVtbl->GetFrameLatencyWaitableObject)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetMatrixTransform(
            [In] DXGI_MATRIX_3X2_F* pMatrix
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetMatrixTransform>(lpVtbl->SetMatrixTransform)(
                    This,
                    pMatrix
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMatrixTransform(
            [Out] DXGI_MATRIX_3X2_F* pMatrix
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetMatrixTransform>(lpVtbl->GetMatrixTransform)(
                    This,
                    pMatrix
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("UINT")]
        public uint GetCurrentBackBufferIndex()
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_GetCurrentBackBufferIndex>(lpVtbl->GetCurrentBackBufferIndex)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CheckColorSpaceSupport(
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [Out, ComAliasName("UINT")] uint* pColorSpaceSupport
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_CheckColorSpaceSupport>(lpVtbl->CheckColorSpaceSupport)(
                    This,
                    ColorSpace,
                    pColorSpaceSupport
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetColorSpace1(
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_SetColorSpace1>(lpVtbl->SetColorSpace1)(
                    This,
                    ColorSpace
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ResizeBuffers1(
            [In, ComAliasName("UINT")] uint BufferCount,
            [In, ComAliasName("UINT")] uint Width,
            [In, ComAliasName("UINT")] uint Height,
            [In] DXGI_FORMAT Format,
            [In, ComAliasName("UINT")] uint SwapChainFlags,
            [In, ComAliasName("UINT[]")] uint* pCreationNodeMask,
            [In, ComAliasName("IUnknown*[]")] IUnknown** ppPresentQueue
        )
        {
            fixed (IDXGISwapChain3* This = &this)
            {
                return MarshalFunction<_ResizeBuffers1>(lpVtbl->ResizeBuffers1)(
                    This,
                    BufferCount,
                    Width,
                    Height,
                    Format,
                    SwapChainFlags,
                    pCreationNodeMask,
                    ppPresentQueue
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

            #region IDXGISwapChain Fields
            public IntPtr Present;

            public IntPtr GetBuffer;

            public IntPtr SetFullscreenState;

            public IntPtr GetFullscreenState;

            public IntPtr GetDesc;

            public IntPtr ResizeBuffers;

            public IntPtr ResizeTarget;

            public IntPtr GetContainingOutput;

            public IntPtr GetFrameStatistics;

            public IntPtr GetLastPresentCount;
            #endregion

            #region IDXGISwapChain1 Fields
            public IntPtr GetDesc1;

            public IntPtr GetFullscreenDesc;

            public IntPtr GetHwnd;

            public IntPtr GetCoreWindow;

            public IntPtr Present1;

            public IntPtr IsTemporaryMonoSupported;

            public IntPtr GetRestrictToOutput;

            public IntPtr SetBackgroundColor;

            public IntPtr GetBackgroundColor;

            public IntPtr SetRotation;

            public IntPtr GetRotation;
            #endregion

            #region IDXGISwapChain2 Fields
            public IntPtr SetSourceSize;

            public IntPtr GetSourceSize;

            public IntPtr SetMaximumFrameLatency;

            public IntPtr GetMaximumFrameLatency;

            public IntPtr GetFrameLatencyWaitableObject;

            public IntPtr SetMatrixTransform;

            public IntPtr GetMatrixTransform;
            #endregion

            #region Fields
            public IntPtr GetCurrentBackBufferIndex;

            public IntPtr CheckColorSpaceSupport;

            public IntPtr SetColorSpace1;

            public IntPtr ResizeBuffers1;
            #endregion
        }
        #endregion
    }
}

