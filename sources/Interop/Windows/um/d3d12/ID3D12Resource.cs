// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("696442BE-A72E-4059-BC79-5B5C98040FAD")]
    public /* unmanaged */ unsafe struct ID3D12Resource
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3D12Resource* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3D12Resource* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3D12Resource* This
        );
        #endregion

        #region ID3D12Object Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] ID3D12Resource* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] ID3D12Resource* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        );


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] ID3D12Resource* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetName(
            [In] ID3D12Resource* This,
            [In, ComAliasName("LPCWSTR")] char* Name
        );
        #endregion

        #region ID3D12DeviceChild Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDevice(
            [In] ID3D12Resource* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvDevice = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Map(
            [In] ID3D12Resource* This,
            [In, ComAliasName("UINT")] uint Subresource,
            [In] D3D12_RANGE* pReadRange = null,
            [Out] void** ppData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _Unmap(
            [In] ID3D12Resource* This,
            [In, ComAliasName("UINT")] uint Subresource,
            [In] D3D12_RANGE* pWrittenRange = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetDesc(
            [In] ID3D12Resource* This,
            [Out] D3D12_RESOURCE_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")]
        public /* static */ delegate ulong _GetGPUVirtualAddress(
            [In] ID3D12Resource* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _WriteToSubresource(
            [In] ID3D12Resource* This,
            [In, ComAliasName("UINT")] uint DstSubresource,
            [In, Optional] D3D12_BOX* pDstBox,
            [In] void* pSrcData,
            [In, ComAliasName("UINT")] uint SrcRowPitch,
            [In, ComAliasName("UINT")] uint SrcDepthPitch
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ReadFromSubresource(
            [In] ID3D12Resource* This,
            [Out] void* pDstData,
            [In, ComAliasName("UINT")] uint DstRowPitch,
            [In, ComAliasName("UINT")] uint DstDepthPitch,
            [In, ComAliasName("UINT")] uint SrcSubresource,
            [In] D3D12_BOX* pSrcBox = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetHeapProperties(
            [In] ID3D12Resource* This,
            [Out] D3D12_HEAP_PROPERTIES* pHeapProperties = null,
            [Out] D3D12_HEAP_FLAGS* pHeapFlags = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3D12Resource* This = &this)
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
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3D12Resource* This = &this)
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
            fixed (ID3D12Resource* This = &this)
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
            fixed (ID3D12Resource* This = &this)
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
            fixed (ID3D12Resource* This = &this)
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
            fixed (ID3D12Resource* This = &this)
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
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_GetDevice>(lpVtbl->GetDevice)(
                    This,
                    riid,
                    ppvDevice
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int Map(
            [In, ComAliasName("UINT")] uint Subresource,
            [In] D3D12_RANGE* pReadRange = null,
            [Out] void** ppData = null
        )
        {
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_Map>(lpVtbl->Map)(
                    This,
                    Subresource,
                    pReadRange,
                    ppData
                );
            }
        }

        public void Unmap(
            [In, ComAliasName("UINT")] uint Subresource,
            [In] D3D12_RANGE* pWrittenRange = null
        )
        {
            fixed (ID3D12Resource* This = &this)
            {
                MarshalFunction<_Unmap>(lpVtbl->Unmap)(
                    This,
                    Subresource,
                    pWrittenRange
                );
            }
        }

        public void GetDesc(
            [Out] D3D12_RESOURCE_DESC* pDesc
        )
        {
            fixed (ID3D12Resource* This = &this)
            {
                MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")]
        public ulong GetGPUVirtualAddress()
        {
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_GetGPUVirtualAddress>(lpVtbl->GetGPUVirtualAddress)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int WriteToSubresource(
            [In, ComAliasName("UINT")] uint DstSubresource,
            [In, Optional] D3D12_BOX* pDstBox,
            [In] void* pSrcData,
            [In, ComAliasName("UINT")] uint SrcRowPitch,
            [In, ComAliasName("UINT")] uint SrcDepthPitch
        )
        {
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_WriteToSubresource>(lpVtbl->WriteToSubresource)(
                    This,
                    DstSubresource,
                    pDstBox,
                    pSrcData,
                    SrcRowPitch,
                    SrcDepthPitch
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ReadFromSubresource(
            [Out] void* pDstData,
            [In, ComAliasName("UINT")] uint DstRowPitch,
            [In, ComAliasName("UINT")] uint DstDepthPitch,
            [In, ComAliasName("UINT")] uint SrcSubresource,
            [In] D3D12_BOX* pSrcBox = null
        )
        {
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_ReadFromSubresource>(lpVtbl->ReadFromSubresource)(
                    This,
                    pDstData,
                    DstRowPitch,
                    DstDepthPitch,
                    SrcSubresource,
                    pSrcBox
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetHeapProperties(
            [Out] D3D12_HEAP_PROPERTIES* pHeapProperties = null,
            [Out] D3D12_HEAP_FLAGS* pHeapFlags = null
        )
        {
            fixed (ID3D12Resource* This = &this)
            {
                return MarshalFunction<_GetHeapProperties>(lpVtbl->GetHeapProperties)(
                    This,
                    pHeapProperties,
                    pHeapFlags
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

            #region ID3D12Object Fields
            public IntPtr GetPrivateData;

            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr SetName;
            #endregion

            #region ID3D12DeviceChild Fields
            public IntPtr GetDevice;
            #endregion

            #region Fields
            public IntPtr Map;

            public IntPtr Unmap;

            public IntPtr GetDesc;

            public IntPtr GetGPUVirtualAddress;

            public IntPtr WriteToSubresource;

            public IntPtr ReadFromSubresource;

            public IntPtr GetHeapProperties;
            #endregion
        }
        #endregion
    }
}

