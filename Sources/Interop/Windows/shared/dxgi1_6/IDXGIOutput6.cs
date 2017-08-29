// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Interop.Desktop;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("068346E8-AAEC-4B84-ADD7-137F513F77A1")]
    public /* blittable */ unsafe struct IDXGIOutput6
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIOutput6* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIOutput6* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIOutput6* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] IDXGIOutput6* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] IDXGIOutput6* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] IDXGIOutput6* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetParent(
            [In] IDXGIOutput6* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region IDXGIOutput Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc(
            [In] IDXGIOutput6* This,
            [Out] DXGI_OUTPUT_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDisplayModeList(
            [In] IDXGIOutput6* This,
            [In] DXGI_FORMAT EnumFormat,
            [In, ComAliasName("UINT")] uint Flags,
            [In, Out, ComAliasName("UINT")] uint* pNumModes,
            [Out, ComAliasName("DXGI_MODE_DESC[]")] DXGI_MODE_DESC* pDesc = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _FindClosestMatchingMode(
            [In] IDXGIOutput6* This,
            [In] DXGI_MODE_DESC* pModeToMatch,
            [Out] DXGI_MODE_DESC* pClosestMatch,
            [In] IUnknown* pConcernedDevice = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WaitForVBlank(
            [In] IDXGIOutput6* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _TakeOwnership(
            [In] IDXGIOutput6* This,
            [In] IUnknown* pDevice,
            [In, ComAliasName("BOOL")] int Exclusive
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ReleaseOwnership(
            [In] IDXGIOutput6* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetGammaControlCapabilities(
            [In] IDXGIOutput6* This,
            [Out] DXGI_GAMMA_CONTROL_CAPABILITIES* pGammaCaps
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetGammaControl(
            [In] IDXGIOutput6* This,
            [In] DXGI_GAMMA_CONTROL* pArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetGammaControl(
            [In] IDXGIOutput6* This,
            [Out] DXGI_GAMMA_CONTROL* pArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetDisplaySurface(
            [In] IDXGIOutput6* This,
            [In] IDXGISurface* pScanoutSurface
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDisplaySurfaceData(
            [In] IDXGIOutput6* This,
            [In] IDXGISurface* pDestination
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameStatistics(
            [In] IDXGIOutput6* This,
            [Out] DXGI_FRAME_STATISTICS* pStats
        );
        #endregion

        #region IDXGIOutput1 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDisplayModeList1(
            [In] IDXGIOutput6* This,
            [In] DXGI_FORMAT EnumFormat,
            [In, ComAliasName("UINT")] uint Flags,
            [In, Out, ComAliasName("UINT")] uint* pNumModes,
            [Out] DXGI_MODE_DESC1* pDesc = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _FindClosestMatchingMode1(
            [In] IDXGIOutput6* This,
            [In] DXGI_MODE_DESC1* pModeToMatch,
            [Out] DXGI_MODE_DESC1* pClosestMatch,
            [In] IUnknown* pConcernedDevice = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDisplaySurfaceData1(
            [In] IDXGIOutput6* This,
            [In] IDXGIResource* pDestination
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DuplicateOutput(
            [In] IDXGIOutput6* This,
            [In] IUnknown* pDevice,
            [Out] IDXGIOutputDuplication** ppOutputDuplication
        );
        #endregion

        #region IDXGIOutput2 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _SupportsOverlays(
            [In] IDXGIOutput6* This
        );
        #endregion

        #region IDXGIOutput3 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CheckOverlaySupport(
            [In] IDXGIOutput6* This,
            [In] DXGI_FORMAT EnumFormat,
            [In] IUnknown* pConcernedDevice,
            [Out, ComAliasName("UINT")] uint* pFlags
        );
        #endregion

        #region IDXGIOutput4 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CheckOverlayColorSpaceSupport(
            [In] IDXGIOutput6* This,
            [In] DXGI_FORMAT Format,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [In] IUnknown* pConcernedDevice,
            [Out, ComAliasName("UINT")] uint* pFlags
        );
        #endregion

        #region IDXGIOutput5 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DuplicateOutput1(
            [In] IDXGIOutput6* This,
            [In] IUnknown* pDevice,
            [In, ComAliasName("UINT")] uint Flags,
            [In, ComAliasName("UINT")] uint SupportedFormatsCount,
            [In, ComAliasName("DXGI_FORMAT[]")] DXGI_FORMAT* pSupportedFormats,
            [Out] IDXGIOutputDuplication** ppOutputDuplication
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc1(
            [In] IDXGIOutput6* This,
            [Out] DXGI_OUTPUT_DESC1* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CheckHardwareCompositionSupport(
            [In] IDXGIOutput6* This,
            [Out, ComAliasName("UINT")] uint* pFlags
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIOutput6* This = &this)
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
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIOutput6* This = &this)
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
            fixed (IDXGIOutput6* This = &this)
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
            fixed (IDXGIOutput6* This = &this)
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
            fixed (IDXGIOutput6* This = &this)
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
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetParent>(lpVtbl->GetParent)(
                    This,
                    riid,
                    ppParent
                );
            }
        }
        #endregion

        #region IDXGIOutput Methods
        [return: ComAliasName("HRESULT")]
        public int GetDesc(
            [Out] DXGI_OUTPUT_DESC* pDesc
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDisplayModeList(
            [In] DXGI_FORMAT EnumFormat,
            [In, ComAliasName("UINT")] uint Flags,
            [In, Out, ComAliasName("UINT")] uint* pNumModes,
            [Out, ComAliasName("DXGI_MODE_DESC[]")] DXGI_MODE_DESC* pDesc = null
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetDisplayModeList>(lpVtbl->GetDisplayModeList)(
                    This,
                    EnumFormat,
                    Flags,
                    pNumModes,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int FindClosestMatchingMode(
            [In] DXGI_MODE_DESC* pModeToMatch,
            [Out] DXGI_MODE_DESC* pClosestMatch,
            [In] IUnknown* pConcernedDevice = null
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_FindClosestMatchingMode>(lpVtbl->FindClosestMatchingMode)(
                    This,
                    pModeToMatch,
                    pClosestMatch,
                    pConcernedDevice
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int WaitForVBlank()
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_WaitForVBlank>(lpVtbl->WaitForVBlank)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int TakeOwnership(
            [In] IUnknown* pDevice,
            [In, ComAliasName("BOOL")] int Exclusive
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_TakeOwnership>(lpVtbl->TakeOwnership)(
                    This,
                    pDevice,
                    Exclusive
                );
            }
        }

        public void ReleaseOwnership()
        {
            fixed (IDXGIOutput6* This = &this)
            {
                MarshalFunction<_ReleaseOwnership>(lpVtbl->ReleaseOwnership)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetGammaControlCapabilities(
            [Out] DXGI_GAMMA_CONTROL_CAPABILITIES* pGammaCaps
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetGammaControlCapabilities>(lpVtbl->GetGammaControlCapabilities)(
                    This,
                    pGammaCaps
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetGammaControl(
            [In] DXGI_GAMMA_CONTROL* pArray
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_SetGammaControl>(lpVtbl->SetGammaControl)(
                    This,
                    pArray
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetGammaControl(
            [Out] DXGI_GAMMA_CONTROL* pArray
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetGammaControl>(lpVtbl->GetGammaControl)(
                    This,
                    pArray
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetDisplaySurface(
            [In] IDXGISurface* pScanoutSurface
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_SetDisplaySurface>(lpVtbl->SetDisplaySurface)(
                    This,
                    pScanoutSurface
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDisplaySurfaceData(
            [In] IDXGISurface* pDestination
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetDisplaySurfaceData>(lpVtbl->GetDisplaySurfaceData)(
                    This,
                    pDestination
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrameStatistics(
            [Out] DXGI_FRAME_STATISTICS* pStats
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetFrameStatistics>(lpVtbl->GetFrameStatistics)(
                    This,
                    pStats
                );
            }
        }
        #endregion

        #region IDXGIOutput1 Methods
        [return: ComAliasName("HRESULT")]
        public int GetDisplayModeList1(
            [In] DXGI_FORMAT EnumFormat,
            [In, ComAliasName("UINT")] uint Flags,
            [In, Out, ComAliasName("UINT")] uint* pNumModes,
            [Out] DXGI_MODE_DESC1* pDesc = null
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetDisplayModeList1>(lpVtbl->GetDisplayModeList1)(
                    This,
                    EnumFormat,
                    Flags,
                    pNumModes,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int FindClosestMatchingMode1(
            [In] DXGI_MODE_DESC1* pModeToMatch,
            [Out] DXGI_MODE_DESC1* pClosestMatch,
            [In] IUnknown* pConcernedDevice = null
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_FindClosestMatchingMode1>(lpVtbl->FindClosestMatchingMode1)(
                    This,
                    pModeToMatch,
                    pClosestMatch,
                    pConcernedDevice
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDisplaySurfaceData1(
            [In] IDXGIResource* pDestination
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetDisplaySurfaceData1>(lpVtbl->GetDisplaySurfaceData1)(
                    This,
                    pDestination
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int DuplicateOutput(
            [In] IUnknown* pDevice,
            [Out] IDXGIOutputDuplication** ppOutputDuplication
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_DuplicateOutput>(lpVtbl->DuplicateOutput)(
                    This,
                    pDevice,
                    ppOutputDuplication
                );
            }
        }
        #endregion

        #region IDXGIOutput2 Methods
        [return: ComAliasName("BOOL")]
        public int SupportsOverlays()
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_SupportsOverlays>(lpVtbl->SupportsOverlays)(
                    This
                );
            }
        }
        #endregion

        #region IDXGIOutput3 Methods
        [return: ComAliasName("HRESULT")]
        public int CheckOverlaySupport(
            [In] DXGI_FORMAT EnumFormat,
            [In] IUnknown* pConcernedDevice,
            [Out, ComAliasName("UINT")] uint* pFlags
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_CheckOverlaySupport>(lpVtbl->CheckOverlaySupport)(
                    This,
                    EnumFormat,
                    pConcernedDevice,
                    pFlags
                );
            }
        }
        #endregion

        #region IDXGIOutput4 Methods
        [return: ComAliasName("HRESULT")]
        public int CheckOverlayColorSpaceSupport(
            [In] DXGI_FORMAT Format,
            [In] DXGI_COLOR_SPACE_TYPE ColorSpace,
            [In] IUnknown* pConcernedDevice,
            [Out, ComAliasName("UINT")] uint* pFlags
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_CheckOverlayColorSpaceSupport>(lpVtbl->CheckOverlayColorSpaceSupport)(
                    This,
                    Format,
                    ColorSpace,
                    pConcernedDevice,
                    pFlags
                );
            }
        }
        #endregion

        #region IDXGIOutput5 Methods
        [return: ComAliasName("HRESULT")]
        public int DuplicateOutput1(
            [In] IUnknown* pDevice,
            [In, ComAliasName("UINT")] uint Flags,
            [In, ComAliasName("UINT")] uint SupportedFormatsCount,
            [In, ComAliasName("DXGI_FORMAT[]")] DXGI_FORMAT* pSupportedFormats,
            [Out] IDXGIOutputDuplication** ppOutputDuplication
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_DuplicateOutput1>(lpVtbl->DuplicateOutput1)(
                    This,
                    pDevice,
                    Flags,
                    SupportedFormatsCount,
                    pSupportedFormats,
                    ppOutputDuplication
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetDesc1(
            [Out] DXGI_OUTPUT_DESC1* pDesc
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_GetDesc1>(lpVtbl->GetDesc1)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CheckHardwareCompositionSupport(
            [Out, ComAliasName("UINT")] uint* pFlags
        )
        {
            fixed (IDXGIOutput6* This = &this)
            {
                return MarshalFunction<_CheckHardwareCompositionSupport>(lpVtbl->CheckHardwareCompositionSupport)(
                    This,
                    pFlags
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

            #region IDXGIOutput Fields
            public IntPtr GetDesc;

            public IntPtr GetDisplayModeList;

            public IntPtr FindClosestMatchingMode;

            public IntPtr WaitForVBlank;

            public IntPtr TakeOwnership;

            public IntPtr ReleaseOwnership;

            public IntPtr GetGammaControlCapabilities;

            public IntPtr SetGammaControl;

            public IntPtr GetGammaControl;

            public IntPtr SetDisplaySurface;

            public IntPtr GetDisplaySurfaceData;

            public IntPtr GetFrameStatistics;
            #endregion

            #region IDXGIOutput1 Fields
            public IntPtr GetDisplayModeList1;

            public IntPtr FindClosestMatchingMode1;

            public IntPtr GetDisplaySurfaceData1;

            public IntPtr DuplicateOutput;
            #endregion

            #region IDXGIOutput2 Fields
            public IntPtr SupportsOverlays;
            #endregion

            #region IDXGIOutput3 Fields
            public IntPtr CheckOverlaySupport;
            #endregion

            #region IDXGIOutput4 Fields
            public IntPtr CheckOverlayColorSpaceSupport;
            #endregion

            #region IDXGIOutput5 Fields
            public IntPtr DuplicateOutput1;
            #endregion

            #region Fields
            public IntPtr GetDesc1;

            public IntPtr CheckHardwareCompositionSupport;
            #endregion
        }
        #endregion
    }
}

