// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>An effect uses this interface to offset an image without inserting a rendering pass.</summary>
    [Guid("3FE6ADEA-7643-4F53-BD14-A0CE63F24042")]
    public /* blittable */ unsafe struct ID2D1OffsetTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetOffset(
            [In] ID2D1OffsetTransform* This,
            [In, ComAliasName("D2D1_POINT_2L")] POINT offset
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D2D1_POINT_2L")]
        public /* static */ delegate POINT GetOffset(
            [In] ID2D1OffsetTransform* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1TransformNode.Vtbl BaseVtbl;

            public IntPtr SetOffset;

            public IntPtr GetOffset;
            #endregion
        }
        #endregion
    }
}
