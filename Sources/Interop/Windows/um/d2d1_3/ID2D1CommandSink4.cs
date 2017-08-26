// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("C78A6519-40D6-4218-B2DE-BEEEB744BB3E")]
    public /* blittable */ unsafe struct ID2D1CommandSink4
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>A new function to set blend mode that respects the new MAX blend. Implementers of SetPrimitiveBlend2 should expect and handle blend mode: D2D1_PRIMITIVE_BLEND_MAX Implementers of SetPrimitiveBlend1 should expect and handle blend modes: D2D1_PRIMITIVE_BLEND_MIN and D2D1_PRIMITIVE_BLEND_ADD Implementers of SetPrimitiveBlend should expect and handle blend modes: D2D1_PRIMITIVE_BLEND_SOURCE_OVER and D2D1_PRIMITIVE_BLEND_COPY</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrimitiveBlend2(
            [In] ID2D1CommandSink4* This,
            [In] D2D1_PRIMITIVE_BLEND primitiveBlend
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1CommandSink3.Vtbl BaseVtbl;

            public IntPtr SetPrimitiveBlend2;
            #endregion
        }
        #endregion
    }
}
