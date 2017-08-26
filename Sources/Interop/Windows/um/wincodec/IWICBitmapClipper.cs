// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("E4FBCF03-223D-4E81-9333-D635556DD1B5")]
    public /* blittable */ unsafe struct IWICBitmapClipper
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Initialize(
            [In] IWICBitmapClipper* This,
            [In, Optional] IWICBitmapSource* pISource,
            [In] /* readonly */ WICRect* prc
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICBitmapSource.Vtbl BaseVtbl;

            public IntPtr Initialize;
            #endregion
        }
        #endregion
    }
}
