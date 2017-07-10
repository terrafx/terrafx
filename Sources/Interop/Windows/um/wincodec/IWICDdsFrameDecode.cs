// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3D4C0C61-18A4-41E4-BD80-481A4FC9F464")]
    unsafe public /* blittable */ struct IWICDdsFrameDecode
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSizeInBlocks(
            [In] IWICDdsFrameDecode* This,
            [Out] UINT* pWidthInBlocks,
            [Out] UINT* pHeightInBlocks
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFormatInfo(
            [In] IWICDdsFrameDecode* This,
            [Out] WICDdsFormatInfo* pFormatInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyBlocks(
            [In] IWICDdsFrameDecode* This,
            [In] /* readonly */ WICRect* prcBoundsInBlocks,
            [In] UINT cbStride,
            [In] UINT cbBufferSize,
            [Out] BYTE* pbBuffer
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetSizeInBlocks GetSizeInBlocks;

            public GetFormatInfo GetFormatInfo;

            public CopyBlocks CopyBlocks;
            #endregion
        }
        #endregion
    }
}
