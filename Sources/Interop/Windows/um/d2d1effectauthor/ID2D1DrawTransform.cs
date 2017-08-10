// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The interface implemented by a transform author to provide a GPU-based effect.</summary>
    [Guid("36BFDCB6-9739-435D-A30D-A653BEFF6A6F")]
    unsafe public /* blittable */ struct ID2D1DrawTransform
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetDrawInfo(
            [In] ID2D1DrawTransform* This,
            [In] ID2D1DrawInfo* drawInfo
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Transform.Vtbl BaseVtbl;

            public IntPtr SetDrawInfo;
            #endregion
        }
        #endregion
    }
}
