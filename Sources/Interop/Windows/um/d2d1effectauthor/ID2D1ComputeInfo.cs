// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>A transform uses this interface to specify how to render a particular pass using compute shader.</summary>
    [Guid("5598B14B-9FD7-48B7-9BDB-8F0964EB38BC")]
    public /* blittable */ unsafe struct ID2D1ComputeInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Set the constant buffer for this transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetComputeShaderConstantBuffer(
            [In] ID2D1ComputeInfo* This,
            [In, ComAliasName("BYTE[]")] byte *buffer,
            [In, ComAliasName("UINT32")] uint bufferCount
        );

        /// <summary>Set the shader instructions for this transform.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetComputeShader(
            [In] ID2D1ComputeInfo* This,
            [In, ComAliasName("REFGUID")] Guid* shaderId
        );

        /// <summary>Sets the resource texture corresponding to the given shader texture index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetResourceTexture(
            [In] ID2D1ComputeInfo* This,
            [In, ComAliasName("UINT32")] uint textureIndex,
            [In] ID2D1ResourceTexture* resourceTexture
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1RenderInfo.Vtbl BaseVtbl;

            public IntPtr SetComputeShaderConstantBuffer;

            public IntPtr SetComputeShader;

            public IntPtr SetResourceTexture;
            #endregion
        }
        #endregion
    }
}
