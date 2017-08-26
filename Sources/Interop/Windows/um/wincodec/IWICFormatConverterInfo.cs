// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

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
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPixelFormats(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("WICPixelFormatGUID")] Guid* pPixelFormatGUIDs,
            [Out, ComAliasName("UINT")] uint* pcActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInstance(
            [In] IWICFormatConverterInfo* This,
            [Out] IWICFormatConverter** ppIConverter = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICComponentInfo.Vtbl BaseVtbl;

            public IntPtr GetPixelFormats;

            public IntPtr CreateInstance;
            #endregion
        }
        #endregion
    }
}
