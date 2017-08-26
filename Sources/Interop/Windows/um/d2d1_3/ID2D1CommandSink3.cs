// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("18079135-4CF3-4868-BC8E-06067E6D242D")]
    public /* blittable */ unsafe struct ID2D1CommandSink3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DrawSpriteBatch(
            [In] ID2D1CommandSink3* This,
            [In] ID2D1SpriteBatch* spriteBatch,
            [In, ComAliasName("UINT32")] uint startIndex,
            [In, ComAliasName("UINT32")] uint spriteCount,
            [In] ID2D1Bitmap* bitmap,
            [In] D2D1_BITMAP_INTERPOLATION_MODE interpolationMode,
            [In] D2D1_SPRITE_OPTIONS spriteOptions
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1CommandSink2.Vtbl BaseVtbl;

            public IntPtr DrawSpriteBatch;
            #endregion
        }
        #endregion
    }
}
