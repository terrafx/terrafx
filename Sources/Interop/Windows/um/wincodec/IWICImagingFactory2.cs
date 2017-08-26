// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("7B816B45-1996-4476-B132-DE9E247C8AF0")]
    unsafe public /* blittable */ struct IWICImagingFactory2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateImageEncoder(
            [In] IWICImagingFactory2* This,
            [In] ID2D1Device* pD2DDevice,
            [Out] IWICImageEncoder** ppWICImageEncoder
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICImagingFactory.Vtbl BaseVtbl;

            public IntPtr CreateImageEncoder;
            #endregion
        }
        #endregion
    }
}
