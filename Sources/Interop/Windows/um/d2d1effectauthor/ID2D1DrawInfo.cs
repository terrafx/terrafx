// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Interop.D2D1_PIXEL_OPTIONS;

namespace TerraFX.Interop
{
    /// <summary>A transform uses this interface to specify how to render a particular pass using pixel and vertex shaders.</summary>
    [Guid("693CE632-7F2F-45DE-93FE-18D88B37AA21")]
    public /* blittable */ unsafe struct ID2D1DrawInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Set the constant buffer for this transform's pixel shader.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPixelShaderConstantBuffer(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("BYTE")] /* readonly */ byte* buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        );

        /// <summary>Sets the resource texture corresponding to the given shader texture index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetResourceTexture(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("UINT32")] uint textureIndex,
            [In] ID2D1ResourceTexture* resourceTexture
        );

        /// <summary>Set the constant buffer for this transform's vertex shader.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetVertexShaderConstantBuffer(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("BYTE")] /* readonly */ byte* buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        );

        /// <summary>Set the shader instructions for this transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPixelShader(
            [In] ID2D1DrawInfo* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* shaderId,
            [In] D2D1_PIXEL_OPTIONS pixelOptions = D2D1_PIXEL_OPTIONS_NONE
        );

        /// <summary>Set custom vertices for the associated transform.  A blend mode if foreground-over will be used if blendDescription is NULL.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetVertexProcessing(
            [In] ID2D1DrawInfo* This,
            [In, Optional] ID2D1VertexBuffer* vertexBuffer,
            [In] D2D1_VERTEX_OPTIONS vertexOptions,
            [In] /* readonly */ D2D1_BLEND_DESCRIPTION* blendDescription = null,
            [In] /* readonly */ D2D1_VERTEX_RANGE* vertexRange = null,
            [In, ComAliasName("GUID")] /* readonly */ Guid* vertexShader = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1RenderInfo.Vtbl BaseVtbl;

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
