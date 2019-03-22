// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("80EABF42-2568-4E5E-BD82-C37F86961DC3")]
    [Unmanaged]
    public unsafe struct ID3D12PipelineLibrary1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3D12PipelineLibrary1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3D12PipelineLibrary1* This
        );
        #endregion

        #region ID3D12Object Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        );


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetName(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("LPCWSTR")] char* Name
        );
        #endregion

        #region ID3D12DeviceChild Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDevice(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvDevice = null
        );
        #endregion

        #region ID3D12PipelineLibrary Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _StorePipeline(
            [In] ID3D12PipelineLibrary1* This,
            [In, Optional, ComAliasName("LPCWSTR")] char* pName,
            [In] ID3D12PipelineState* pPipeline
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LoadGraphicsPipeline(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LoadComputePipeline(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("SIZE_T")]
        public /* static */ delegate nuint _GetSerializedSize(
            [In] ID3D12PipelineLibrary1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Serialize(
            [In] ID3D12PipelineLibrary1* This,
            [Out] void* pData,
            [In, ComAliasName("SIZE_T")] nuint DataSizeInBytes
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _LoadPipeline(
            [In] ID3D12PipelineLibrary1* This,
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_PIPELINE_STATE_STREAM_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
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
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID3D12Object Methods
        [return: ComAliasName("HRESULT")]
        public int GetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_GetPrivateData>(lpVtbl->GetPrivateData)(
                    This,
                    guid,
                    pDataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_SetPrivateData>(lpVtbl->SetPrivateData)(
                    This,
                    guid,
                    DataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateDataInterface(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_SetPrivateDataInterface>(lpVtbl->SetPrivateDataInterface)(
                    This,
                    guid,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetName(
            [In, ComAliasName("LPCWSTR")] char* Name
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_SetName>(lpVtbl->SetName)(
                    This,
                    Name
                );
            }
        }
        #endregion

        #region ID3D12DeviceChild Methods
        [return: ComAliasName("HRESULT")]
        public int GetDevice(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvDevice = null
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_GetDevice>(lpVtbl->GetDevice)(
                    This,
                    riid,
                    ppvDevice
                );
            }
        }
        #endregion

        #region ID3D12PipelineLibrary Methods
        [return: ComAliasName("HRESULT")]
        public int StorePipeline(
            [In, Optional, ComAliasName("LPCWSTR")] char* pName,
            [In] ID3D12PipelineState* pPipeline
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_StorePipeline>(lpVtbl->StorePipeline)(
                    This,
                    pName,
                    pPipeline
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int LoadGraphicsPipeline(
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_LoadGraphicsPipeline>(lpVtbl->LoadGraphicsPipeline)(
                    This,
                    pName,
                    pDesc,
                    riid,
                    ppPipelineState
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int LoadComputePipeline(
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_LoadComputePipeline>(lpVtbl->LoadComputePipeline)(
                    This,
                    pName,
                    pDesc,
                    riid,
                    ppPipelineState
                );
            }
        }

        [return: ComAliasName("SIZE_T")]
        public nuint GetSerializedSize()
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_GetSerializedSize>(lpVtbl->GetSerializedSize)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Serialize(
            [Out] void* pData,
            [In, ComAliasName("SIZE_T")] nuint DataSizeInBytes
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_Serialize>(lpVtbl->Serialize)(
                    This,
                    pData,
                    DataSizeInBytes
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int LoadPipeline(
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_PIPELINE_STATE_STREAM_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        )
        {
            fixed (ID3D12PipelineLibrary1* This = &this)
            {
                return MarshalFunction<_LoadPipeline>(lpVtbl->LoadPipeline)(
                    This,
                    pName,
                    pDesc,
                    riid,
                    ppPipelineState
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

            #region ID3D12Object Fields
            public IntPtr GetPrivateData;

            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr SetName;
            #endregion

            #region ID3D12DeviceChild Fields
            public IntPtr GetDevice;
            #endregion

            #region ID3D12PipelineLibrary Fields
            public IntPtr StorePipeline;

            public IntPtr LoadGraphicsPipeline;

            public IntPtr LoadComputePipeline;

            public IntPtr GetSerializedSize;

            public IntPtr Serialize;
            #endregion

            #region Fields
            public IntPtr LoadPipeline;
            #endregion
        }
        #endregion
    }
}

