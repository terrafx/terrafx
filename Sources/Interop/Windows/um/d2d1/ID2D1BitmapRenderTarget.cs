// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Renders to an intermediate texture created by the CreateCompatibleRenderTarget method.</summary>
    [Guid("2CD90695-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1BitmapRenderTarget
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetBitmap(
            [In] ID2D1BitmapRenderTarget* This,
            [Out] ID2D1Bitmap** bitmap
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1RenderTarget.Vtbl BaseVtbl;

            public IntPtr GetBitmap;
            #endregion
        }
        #endregion
    }
}
