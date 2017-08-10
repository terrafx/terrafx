// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3B16811B-6A43-4EC9-B713-3D5A0C13B940")]
    unsafe public /* blittable */ struct IWICBitmapSourceTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CopyPixels(
            [In] IWICBitmapSourceTransform* This,
            [In, Optional] /* readonly */ WICRect* prc,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, Optional, ComAliasName("WICPixelFormatGUID")] Guid* pguidDstFormat,
            [In] WICBitmapTransformOptions dstTransform,
            [In, ComAliasName("UINT")] uint nStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [Out, ComAliasName("BYTE")] byte* pbBuffer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetClosestSize(
            [In] IWICBitmapSourceTransform* This,
            [In, Out, ComAliasName("UINT")] uint* puiWidth,
            [In, Out, ComAliasName("UINT")] uint* puiHeight
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetClosestPixelFormat(
            [In] IWICBitmapSourceTransform* This,
            [In, Out, ComAliasName("WICPixelFormatGUID")] Guid* pguidDstFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportTransform(
            [In] IWICBitmapSourceTransform* This,
            [In] WICBitmapTransformOptions dstTransform,
            [Out, ComAliasName("BOOL")] int* pfIsSupported
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr CopyPixels;

            public IntPtr GetClosestSize;

            public IntPtr GetClosestPixelFormat;

            public IntPtr DoesSupportTransform;
            #endregion
        }
        #endregion
    }
}
