// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.Desktop
{
    /// <summary>Provides access to an device context that can accept GDI drawing commands.</summary>
    [Guid("E0DB51C3-6F77-4BAE-B3D5-E47509B35838")]
    unsafe public /* blittable */ struct ID2D1GdiInteropRenderTarget
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDC(
            [In] ID2D1GdiInteropRenderTarget* This,
            [In] D2D1_DC_INITIALIZE_MODE mode,
            [Out, ComAliasName("HDC")] void** hdc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReleaseDC(
            [In] ID2D1GdiInteropRenderTarget* This,
            [In, Optional] /* readonly */ RECT* update
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetDC GetDC;

            public ReleaseDC ReleaseDC;
            #endregion
        }
        #endregion
    }
}
