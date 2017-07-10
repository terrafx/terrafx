// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("9F34FB65-13F4-4F15-BC57-3726B5E53D9F")]
    unsafe public /* blittable */ struct IWICFormatConverterInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPixelFormats(
            [In] IWICFormatConverterInfo* This,
            [In] UINT cFormats,
            [In, Out, Optional] WICPixelFormatGUID* pPixelFormatGUIDs,
            [Out] UINT* pcActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateInstance(
            [In] IWICFormatConverterInfo* This,
            [Out, Optional] IWICFormatConverter** ppIConverter
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICComponentInfo.Vtbl BaseVtbl;

            public GetPixelFormats GetPixelFormats;

            public CreateInstance CreateInstance;
            #endregion
        }
        #endregion
    }
}
