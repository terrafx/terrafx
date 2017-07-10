// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Issues drawing commands to a GDI device context.</summary>
    [Guid("1C51BC64-DE61-46FD-9899-63A5D8F03950")]
    unsafe public /* blittable */ struct ID2D1DCRenderTarget
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT BindDC(
            [In] ID2D1DCRenderTarget* This,
            [In] /* readonly */ HDC hDC,
            [In] /* readonly */ RECT* pSubRect
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1RenderTarget.Vtbl BaseVtbl;

            public BindDC BindDC;
            #endregion
        }
        #endregion
    }
}
