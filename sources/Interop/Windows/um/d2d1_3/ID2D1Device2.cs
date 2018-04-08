// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>Represents a resource domain whose objects and device contexts can be used together. This interface performs all the same functions as the existing ID2D1Device1 interface. It also enables the creation of ID2D1DeviceContext2 objects.</summary>
    [Guid("A44472E1-8DFB-4E60-8492-6E2861C9CA8B")]
    public /* unmanaged */ unsafe struct ID2D1Device2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1Device2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1Device2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1Device2* This
        );
        #endregion

        #region ID2D1Resource Delegates
        /// <summary>Retrieve the factory associated with this resource.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetFactory(
            [In] ID2D1Device2* This,
            [Out] ID2D1Factory** factory
        );
        #endregion

        #region ID2D1Device Delegates
        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDeviceContext(
            [In] ID2D1Device2* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext** deviceContext
        );

        /// <summary>Creates a D2D print control.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreatePrintControl(
            [In] ID2D1Device2* This,
            [In] IWICImagingFactory* wicFactory,
            [In] IPrintDocumentPackageTarget* documentTarget,
            [In, Optional] D2D1_PRINT_CONTROL_PROPERTIES* printControlProperties,
            [Out] ID2D1PrintControl** printControl
        );

        /// <summary>Sets the maximum amount of texture memory to maintain before evicting caches.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetMaximumTextureMemory(
            [In] ID2D1Device2* This,
            [In, ComAliasName("UINT64")] ulong maximumInBytes
        );

        /// <summary>Gets the maximum amount of texture memory to maintain before evicting caches.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetMaximumTextureMemory(
            [In] ID2D1Device2* This
        );

        /// <summary>Clears all resources that are cached but not held in use by the application through an interface reference.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ClearResources(
            [In] ID2D1Device2* This,
            [In, ComAliasName("UINT32")] uint millisecondsSinceUse = 0
        );
        #endregion

        #region ID2D1Device1 Delegates
        /// <summary>Retrieves the rendering priority currently set on the device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_RENDERING_PRIORITY _GetRenderingPriority(
            [In] ID2D1Device2* This
        );

        /// <summary>Sets the rendering priority of the device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetRenderingPriority(
            [In] ID2D1Device2* This,
            [In] D2D1_RENDERING_PRIORITY renderingPriority
        );

        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDeviceContext1(
            [In] ID2D1Device2* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext1** deviceContext1
        );
        #endregion

        #region Delegates
        /// <summary>Creates a new device context with no initially assigned target.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDeviceContext2(
            [In] ID2D1Device2* This,
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext2** deviceContext2
        );

        /// <summary>Flush all device contexts that reference a given bitmap.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _FlushDeviceContexts(
            [In] ID2D1Device2* This,
            [In] ID2D1Bitmap* bitmap
        );

        /// <summary>Returns the DXGI device associated with this D2D device.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDxgiDevice(
            [In] ID2D1Device2* This,
            [Out] IDXGIDevice** dxgiDevice
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1Device2* This = &this)
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
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID2D1Resource Methods
        public void GetFactory(
            [Out] ID2D1Factory** factory
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                MarshalFunction<_GetFactory>(lpVtbl->GetFactory)(
                    This,
                    factory
                );
            }
        }
        #endregion

        #region ID2D1Device Methods
        [return: ComAliasName("HRESULT")]
        public int CreateDeviceContext(
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext** deviceContext
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_CreateDeviceContext>(lpVtbl->CreateDeviceContext)(
                    This,
                    options,
                    deviceContext
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreatePrintControl(
            [In] IWICImagingFactory* wicFactory,
            [In] IPrintDocumentPackageTarget* documentTarget,
            [In, Optional] D2D1_PRINT_CONTROL_PROPERTIES* printControlProperties,
            [Out] ID2D1PrintControl** printControl
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_CreatePrintControl>(lpVtbl->CreatePrintControl)(
                    This,
                    wicFactory,
                    documentTarget,
                    printControlProperties,
                    printControl
                );
            }
        }

        public void SetMaximumTextureMemory(
            [In, ComAliasName("UINT64")] ulong maximumInBytes
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                MarshalFunction<_SetMaximumTextureMemory>(lpVtbl->SetMaximumTextureMemory)(
                    This,
                    maximumInBytes
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetMaximumTextureMemory()
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_GetMaximumTextureMemory>(lpVtbl->GetMaximumTextureMemory)(
                    This
                );
            }
        }

        public void ClearResources(
            [In, ComAliasName("UINT32")] uint millisecondsSinceUse = 0
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                MarshalFunction<_ClearResources>(lpVtbl->ClearResources)(
                    This,
                    millisecondsSinceUse
                );
            }
        }
        #endregion

        #region ID2D1Device1 Methods
        public D2D1_RENDERING_PRIORITY GetRenderingPriority()
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_GetRenderingPriority>(lpVtbl->GetRenderingPriority)(
                    This
                );
            }
        }

        public void SetRenderingPriority(
            [In] D2D1_RENDERING_PRIORITY renderingPriority
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                MarshalFunction<_SetRenderingPriority>(lpVtbl->SetRenderingPriority)(
                    This,
                    renderingPriority
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateDeviceContext1(
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext1** deviceContext1
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_CreateDeviceContext1>(lpVtbl->CreateDeviceContext1)(
                    This,
                    options,
                    deviceContext1
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreateDeviceContext2(
            [In] D2D1_DEVICE_CONTEXT_OPTIONS options,
            [Out] ID2D1DeviceContext2** deviceContext2
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_CreateDeviceContext2>(lpVtbl->CreateDeviceContext2)(
                    This,
                    options,
                    deviceContext2
                );
            }
        }

        public void FlushDeviceContexts(
            [In] ID2D1Bitmap* bitmap
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                MarshalFunction<_FlushDeviceContexts>(lpVtbl->FlushDeviceContexts)(
                    This,
                    bitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDxgiDevice(
            [Out] IDXGIDevice** dxgiDevice
        )
        {
            fixed (ID2D1Device2* This = &this)
            {
                return MarshalFunction<_GetDxgiDevice>(lpVtbl->GetDxgiDevice)(
                    This,
                    dxgiDevice
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

            #region Fields
            public IntPtr CreateDeviceContext2;

            public IntPtr FlushDeviceContexts;

            public IntPtr GetDxgiDevice;
            #endregion
        }
        #endregion
    }
}

