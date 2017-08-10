// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("E8EDA601-3D48-431A-AB44-69059BE88BBE")]
    unsafe public /* blittable */ struct IWICPixelFormatInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFormatGUID(
            [In] IWICPixelFormatInfo* This,
            [Out, ComAliasName("GUID")] Guid* pFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetColorContext(
            [In] IWICPixelFormatInfo* This,
            [Out, Optional] IWICColorContext** ppIColorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetBitsPerPixel(
            [In] IWICPixelFormatInfo* This,
            [Out, ComAliasName("UINT")] uint* puiBitsPerPixel
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetChannelCount(
            [In] IWICPixelFormatInfo* This,
            [Out, ComAliasName("UINT")] uint* puiChannelCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetChannelMask(
            [In] IWICPixelFormatInfo* This,
            [In, ComAliasName("UINT")] uint uiChannelIndex,
            [In, ComAliasName("UINT")] uint cbMaskBuffer,
            [In, Out, Optional, ComAliasName("BYTE")] byte* pbMaskBuffer,
            [Out, ComAliasName("UINT")] uint* pcbActual
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICComponentInfo.Vtbl BaseVtbl;

            public IntPtr GetFormatGUID;

            public IntPtr GetColorContext;

            public IntPtr GetBitsPerPixel;

            public IntPtr GetChannelCount;

            public IntPtr GetChannelMask;
            #endregion
        }
        #endregion
    }
}
