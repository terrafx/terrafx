// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("852F2087-802C-4037-AB60-FF2E7EE6FC01")]
    public /* blittable */ unsafe struct ID2D1Device3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID2D1Device3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID2D1Device3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID2D1Device3* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetFactory(
            [In] ID2D1Device3* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1Device Delegates
        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDeviceContext(
            [In] ID2D1Device3* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext** deviceContext
        );

        /// <summary>Creates a D2D print control.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePrintControl(
            [In] ID2D1Device3* This,
            [In] IWICImagingFactory* wicFactory,
            [In] IPrintDocumentPackageTarget* documentTarget,
            [In, Optional] D2D1_PRINT_CONTROL_PROPERTIES* printControlProperties,
            [Out] ID2D1PrintControl** printControl
        );

        /// <summary>Sets the maximum amount of texture memory to maintain before evicting caches.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMaximumTextureMemory(
            [In] ID2D1Device3* This,
            [In, ComAliasName("UINT64")] ulong maximumInBytes
        );

        /// <summary>Gets the maximum amount of texture memory to maintain before evicting caches.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetMaximumTextureMemory(
            [In] ID2D1Device3* This
        );

        /// <summary>Clears all resources that are cached but not held in use by the application through an interface reference.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearResources(
            [In] ID2D1Device3* This,
            [In, ComAliasName("UINT32")] uint millisecondsSinceUse = 0
        );
        #endregion

        #region ID2D1Device1 Delegates
        /// <summary>Retrieves the rendering priority currently set on the device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_RENDERING_PRIORITY GetRenderingPriority(
            [In] ID2D1Device3* This
        );

        /// <summary>Sets the rendering priority of the device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetRenderingPriority(
            [In] ID2D1Device3* This,
            [In] D2D1_RENDERING_PRIORITY renderingPriority
        );

        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDeviceContext1(
            [In] ID2D1Device3* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext1** deviceContext1
        );
        #endregion

        #region ID2D1Device2 Delegates
        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDeviceContext2(
            [In] ID2D1Device3* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext2** deviceContext2
        );

        /// <summary>Flush all device contexts that reference a given bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void FlushDeviceContexts(
            [In] ID2D1Device3* This,
            [In] ID2D1Bitmap* bitmap
        );

        /// <summary>Returns the DXGI device associated with this D2D device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDxgiDevice(
            [In] ID2D1Device3* This,
            [Out] IDXGIDevice** dxgiDevice
        );
        #endregion

        #region Delegates
        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDeviceContext3(
            [In] ID2D1Device3* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext3** deviceContext3
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

            #region ID2D1Resource Fields
            public IntPtr GetFactory;
            #endregion

            #region ID2D1Device Fields
            public IntPtr CreateDeviceContext;

            public IntPtr CreatePrintControl;

            public IntPtr SetMaximumTextureMemory;

            public IntPtr GetMaximumTextureMemory;

            public IntPtr ClearResources;
            #endregion

            #region ID2D1Device1 Fields
            public IntPtr GetRenderingPriority;

            public IntPtr SetRenderingPriority;

            public IntPtr CreateDeviceContext1;
            #endregion

            #region ID2D1Device2 Fields
            public IntPtr CreateDeviceContext2;

            public IntPtr FlushDeviceContexts;

            public IntPtr GetDxgiDevice;
            #endregion

            #region Fields
            public IntPtr CreateDeviceContext3;
            #endregion
        }
        #endregion
    }
}
