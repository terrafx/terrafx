// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Interop.Desktop;

namespace TerraFX.Interop
{
    [Guid("595E39D1-2724-4663-99B1-DA969DE28364")]
    public /* blittable */ unsafe struct IDXGIOutput2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IDXGIOutput2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IDXGIOutput2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IDXGIOutput2* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrivateData(
            [In] IDXGIOutput2* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrivateDataInterface(
            [In] IDXGIOutput2* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPrivateData(
            [In] IDXGIOutput2* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetParent(
            [In] IDXGIOutput2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region IDXGIOutput Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDesc(
            [In] IDXGIOutput2* This,
            [Out] DXGI_OUTPUT_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDisplayModeList(
            [In] IDXGIOutput2* This,
            [In] DXGI_FORMAT EnumFormat,
            [In, ComAliasName("UINT")] uint Flags,
            [In, Out, ComAliasName("UINT")] uint* pNumModes,
            [Out, ComAliasName("DXGI_MODE_DESC[]")] DXGI_MODE_DESC* pDesc = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindClosestMatchingMode(
            [In] IDXGIOutput2* This,
            [In] DXGI_MODE_DESC* pModeToMatch,
            [Out] DXGI_MODE_DESC* pClosestMatch,
            [In] IUnknown* pConcernedDevice = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WaitForVBlank(
            [In] IDXGIOutput2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int TakeOwnership(
            [In] IDXGIOutput2* This,
            [In] IUnknown* pDevice,
            [In, ComAliasName("BOOL")] int Exclusive
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ReleaseOwnership(
            [In] IDXGIOutput2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGammaControlCapabilities(
            [In] IDXGIOutput2* This,
            [Out] DXGI_GAMMA_CONTROL_CAPABILITIES* pGammaCaps
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetGammaControl(
            [In] IDXGIOutput2* This,
            [In] DXGI_GAMMA_CONTROL* pArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetGammaControl(
            [In] IDXGIOutput2* This,
            [Out] DXGI_GAMMA_CONTROL* pArray
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetDisplaySurface(
            [In] IDXGIOutput2* This,
            [In] IDXGISurface* pScanoutSurface
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDisplaySurfaceData(
            [In] IDXGIOutput2* This,
            [In] IDXGISurface* pDestination
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFrameStatistics(
            [In] IDXGIOutput2* This,
            [Out] DXGI_FRAME_STATISTICS* pStats
        );
        #endregion

        #region IDXGIOutput1 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDisplayModeList1(
            [In] IDXGIOutput2* This,
            [In] DXGI_FORMAT EnumFormat,
            [In, ComAliasName("UINT")] uint Flags,
            [In, Out, ComAliasName("UINT")] uint* pNumModes,
            [Out] DXGI_MODE_DESC1* pDesc = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int FindClosestMatchingMode1(
            [In] IDXGIOutput2* This,
            [In] DXGI_MODE_DESC1* pModeToMatch,
            [Out] DXGI_MODE_DESC1* pClosestMatch,
            [In] IUnknown* pConcernedDevice = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDisplaySurfaceData1(
            [In] IDXGIOutput2* This,
            [In] IDXGIResource* pDestination
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DuplicateOutput(
            [In] IDXGIOutput2* This,
            [In] IUnknown* pDevice,
            [Out] IDXGIOutputDuplication** ppOutputDuplication
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int SupportsOverlays(
            [In] IDXGIOutput2* This
        );
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

            #region Fields
            public IntPtr SupportsOverlays;
            #endregion
        }
        #endregion
    }
}
