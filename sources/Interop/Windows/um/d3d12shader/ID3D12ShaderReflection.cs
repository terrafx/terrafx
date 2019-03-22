// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("5A58797D-A72C-478D-8BA2-EFC6B0EFE88E")]
    [Unmanaged]
    public unsafe struct ID3D12ShaderReflection
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3D12ShaderReflection* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc(
            [In] ID3D12ShaderReflection* This,
            [Out] D3D12_SHADER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* _GetConstantBufferByIndex(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("UINT")] uint Index
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* _GetConstantBufferByName(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("LPCSTR")] sbyte* Name
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetResourceBindingDesc(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("UINT")] uint ResourceIndex,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetInputParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("UINT")] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetOutputParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("UINT")] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPatchConstantParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("UINT")] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionVariable* _GetVariableByName(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("LPCSTR")] sbyte* Name
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetResourceBindingDescByName(
            [In] ID3D12ShaderReflection* This,
            [In, ComAliasName("LPCSTR")] sbyte* Name,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetMovInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetMovcInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetConversionInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetBitwiseInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D3D_PRIMITIVE _GetGSInputPrimitive(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _IsSampleFrequencyShader(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetNumInterfaceSlots(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMinFeatureLevel(
            [In] ID3D12ShaderReflection* This,
            [Out] D3D_FEATURE_LEVEL* pLevel
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetThreadGroupSize(
            [In] ID3D12ShaderReflection* This,
            [Out, ComAliasName("UINT")] uint* pSizeX = null,
            [Out, ComAliasName("UINT")] uint* pSizeY = null,
            [Out, ComAliasName("UINT")] uint* pSizeZ = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetRequiresFlags(
            [In] ID3D12ShaderReflection* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
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
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetDesc(
            [Out] D3D12_SHADER_DESC* pDesc
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }

        public ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByIndex(
            [In, ComAliasName("UINT")] uint Index
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetConstantBufferByIndex>(lpVtbl->GetConstantBufferByIndex)(
                    This,
                    Index
                );
            }
        }

        public ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByName(
            [In, ComAliasName("LPCSTR")] sbyte* Name
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetConstantBufferByName>(lpVtbl->GetConstantBufferByName)(
                    This,
                    Name
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetResourceBindingDesc(
            [In, ComAliasName("UINT")] uint ResourceIndex,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetResourceBindingDesc>(lpVtbl->GetResourceBindingDesc)(
                    This,
                    ResourceIndex,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetInputParameterDesc(
            [In, ComAliasName("UINT")] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetInputParameterDesc>(lpVtbl->GetInputParameterDesc)(
                    This,
                    ParameterIndex,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetOutputParameterDesc(
            [In, ComAliasName("UINT")] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetOutputParameterDesc>(lpVtbl->GetOutputParameterDesc)(
                    This,
                    ParameterIndex,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPatchConstantParameterDesc(
            [In, ComAliasName("UINT")] uint ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetPatchConstantParameterDesc>(lpVtbl->GetPatchConstantParameterDesc)(
                    This,
                    ParameterIndex,
                    pDesc
                );
            }
        }

        public ID3D12ShaderReflectionVariable* GetVariableByName(
            [In, ComAliasName("LPCSTR")] sbyte* Name
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetVariableByName>(lpVtbl->GetVariableByName)(
                    This,
                    Name
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetResourceBindingDescByName(
            [In, ComAliasName("LPCSTR")] sbyte* Name,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetResourceBindingDescByName>(lpVtbl->GetResourceBindingDescByName)(
                    This,
                    Name,
                    pDesc
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetMovInstructionCount()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetMovInstructionCount>(lpVtbl->GetMovInstructionCount)(
                    This
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetMovcInstructionCount()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetMovcInstructionCount>(lpVtbl->GetMovcInstructionCount)(
                    This
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetConversionInstructionCount()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetConversionInstructionCount>(lpVtbl->GetConversionInstructionCount)(
                    This
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetBitwiseInstructionCount()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetBitwiseInstructionCount>(lpVtbl->GetBitwiseInstructionCount)(
                    This
                );
            }
        }

        public D3D_PRIMITIVE GetGSInputPrimitive()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetGSInputPrimitive>(lpVtbl->GetGSInputPrimitive)(
                    This
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int IsSampleFrequencyShader()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_IsSampleFrequencyShader>(lpVtbl->IsSampleFrequencyShader)(
                    This
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetNumInterfaceSlots()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetNumInterfaceSlots>(lpVtbl->GetNumInterfaceSlots)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMinFeatureLevel(
            [Out] D3D_FEATURE_LEVEL* pLevel
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetMinFeatureLevel>(lpVtbl->GetMinFeatureLevel)(
                    This,
                    pLevel
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetThreadGroupSize(
            [Out, ComAliasName("UINT")] uint* pSizeX = null,
            [Out, ComAliasName("UINT")] uint* pSizeY = null,
            [Out, ComAliasName("UINT")] uint* pSizeZ = null
        )
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetThreadGroupSize>(lpVtbl->GetThreadGroupSize)(
                    This,
                    pSizeX,
                    pSizeY,
                    pSizeZ
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetRequiresFlags()
        {
            fixed (ID3D12ShaderReflection* This = &this)
            {
                return MarshalFunction<_GetRequiresFlags>(lpVtbl->GetRequiresFlags)(
                    This
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

            #region Fields
            public IntPtr GetDesc;

            public IntPtr GetConstantBufferByIndex;

            public IntPtr GetConstantBufferByName;

            public IntPtr GetResourceBindingDesc;

            public IntPtr GetInputParameterDesc;

            public IntPtr GetOutputParameterDesc;

            public IntPtr GetPatchConstantParameterDesc;

            public IntPtr GetVariableByName;

            public IntPtr GetResourceBindingDescByName;

            public IntPtr GetMovInstructionCount;

            public IntPtr GetMovcInstructionCount;

            public IntPtr GetConversionInstructionCount;

            public IntPtr GetBitwiseInstructionCount;

            public IntPtr GetGSInputPrimitive;

            public IntPtr IsSampleFrequencyShader;

            public IntPtr GetNumInterfaceSlots;

            public IntPtr GetMinFeatureLevel;

            public IntPtr GetThreadGroupSize;

            public IntPtr GetRequiresFlags;
            #endregion
        }
        #endregion
    }
}

