// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.D2D1_PIXEL_OPTIONS;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    /// <summary>A transform uses this interface to specify how to render a particular pass using pixel and vertex shaders.</summary>
    [Guid("693CE632-7F2F-45DE-93FE-18D88B37AA21")]
    public /* unmanaged */ unsafe struct ID2D1DrawInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1DrawInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1DrawInfo* This
        );
        #endregion

        #region ID2D1RenderInfo Delegates
        /// <summary>Sets options for sampling the specified image input</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetInputDescription(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("UINT32")] uint inputIndex,
            [In] D2D1_INPUT_DESCRIPTION inputDescription
        );

        /// <summary>Controls the output precision and channel-depth for the associated transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetOutputBuffer(
            [In] ID2D1DrawInfo* This,
            [In] D2D1_BUFFER_PRECISION bufferPrecision,
            [In] D2D1_CHANNEL_DEPTH channelDepth
        );

        /// <summary>Controls whether the output of the associated transform is cached.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetCached(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("BOOL")] int isCached
        );

        /// <summary>Provides a hint of the approximate shader instruction count per pixel.  If provided, it may improve performance when processing large images.  Instructions should be counted multiple times if occurring within loops.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetInstructionCountHint(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("UINT32")] uint instructionCount
        );
        #endregion

        #region Delegates
        /// <summary>Set the constant buffer for this transform's pixel shader.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPixelShaderConstantBuffer(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("BYTE[]")] byte* buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        );

        /// <summary>Sets the resource texture corresponding to the given shader texture index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetResourceTexture(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("UINT32")] uint textureIndex,
            [In] ID2D1ResourceTexture* resourceTexture
        );

        /// <summary>Set the constant buffer for this transform's vertex shader.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetVertexShaderConstantBuffer(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("BYTE[]")] byte* buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        );

        /// <summary>Set the shader instructions for this transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPixelShader(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("REFGUID")] Guid* shaderId,
            [In] D2D1_PIXEL_OPTIONS pixelOptions = D2D1_PIXEL_OPTIONS_NONE
        );

        /// <summary>Set custom vertices for the associated transform.  A blend mode if foreground-over will be used if blendDescription is NULL.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetVertexProcessing(
            [In] ID2D1DrawInfo* This,
            [In, Optional] ID2D1VertexBuffer* vertexBuffer,
            [In] D2D1_VERTEX_OPTIONS vertexOptions,
            [In] D2D1_BLEND_DESCRIPTION* blendDescription = null,
            [In] D2D1_VERTEX_RANGE* vertexRange = null,
            [In, ComAliasName("GUID")] Guid* vertexShader = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
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
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID2D1RenderInfo Methods
        [return: ComAliasName("HRESULT")]
        public int SetInputDescription(
            [In, ComAliasName("UINT32")] uint inputIndex,
            [In] D2D1_INPUT_DESCRIPTION inputDescription
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetInputDescription>(lpVtbl->SetInputDescription)(
                    This,
                    inputIndex,
                    inputDescription
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetOutputBuffer(
            [In] D2D1_BUFFER_PRECISION bufferPrecision,
            [In] D2D1_CHANNEL_DEPTH channelDepth
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetOutputBuffer>(lpVtbl->SetOutputBuffer)(
                    This,
                    bufferPrecision,
                    channelDepth
                );
            }
        }

        public void SetCached(
            [In, ComAliasName("BOOL")] int isCached
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                MarshalFunction<_SetCached>(lpVtbl->SetCached)(
                    This,
                    isCached
                );
            }
        }

        public void SetInstructionCountHint(
            [In, ComAliasName("UINT32")] uint instructionCount
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                MarshalFunction<_SetInstructionCountHint>(lpVtbl->SetInstructionCountHint)(
                    This,
                    instructionCount
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int SetPixelShaderConstantBuffer(
            [In, ComAliasName("BYTE[]")] byte* buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetPixelShaderConstantBuffer>(lpVtbl->SetPixelShaderConstantBuffer)(
                    This,
                    buffer,
                    bufferCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetResourceTexture(
            [In, ComAliasName("UINT32")] uint textureIndex,
            [In] ID2D1ResourceTexture* resourceTexture
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetResourceTexture>(lpVtbl->SetResourceTexture)(
                    This,
                    textureIndex,
                    resourceTexture
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetVertexShaderConstantBuffer(
            [In, ComAliasName("BYTE[]")] byte* buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetVertexShaderConstantBuffer>(lpVtbl->SetVertexShaderConstantBuffer)(
                    This,
                    buffer,
                    bufferCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPixelShader(
            [In, ComAliasName("REFGUID")] Guid* shaderId,
            [In] D2D1_PIXEL_OPTIONS pixelOptions = D2D1_PIXEL_OPTIONS_NONE
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetPixelShader>(lpVtbl->SetPixelShader)(
                    This,
                    shaderId,
                    pixelOptions
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetVertexProcessing(
            [In, Optional] ID2D1VertexBuffer* vertexBuffer,
            [In] D2D1_VERTEX_OPTIONS vertexOptions,
            [In] D2D1_BLEND_DESCRIPTION* blendDescription = null,
            [In] D2D1_VERTEX_RANGE* vertexRange = null,
            [In, ComAliasName("GUID")] Guid* vertexShader = null
        )
        {
            fixed (ID2D1DrawInfo* This = &this)
            {
                return MarshalFunction<_SetVertexProcessing>(lpVtbl->SetVertexProcessing)(
                    This,
                    vertexBuffer,
                    vertexOptions,
                    blendDescription,
                    vertexRange,
                    vertexShader
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

            #region ID2D1RenderInfo Fields
            public IntPtr SetInputDescription;

            public IntPtr SetOutputBuffer;

            public IntPtr SetCached;

            public IntPtr SetInstructionCountHint;
            #endregion

            #region Fields
            public IntPtr SetPixelShaderConstantBuffer;

            public IntPtr SetResourceTexture;

            public IntPtr SetVertexShaderConstantBuffer;

            public IntPtr SetPixelShader;

            public IntPtr SetVertexProcessing;
            #endregion
        }
        #endregion
    }
}

