// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>An effect uses this interface to alter the image rectangle of its input.</summary>
    [Guid("90F732E2-5092-4606-A819-8651970BACCD")]
    public /* blittable */ unsafe struct ID2D1BoundsAdjustmentTransform
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetOutputBounds(
            [In] ID2D1BoundsAdjustmentTransform* This,
            [In, ComAliasName("D2D1_RECT_L")] /* readonly */ RECT* outputBounds
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetOutputBounds(
            [In] ID2D1BoundsAdjustmentTransform* This,
            [Out, ComAliasName("D2D1_RECT_L")] RECT* outputBounds
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1TransformNode.Vtbl BaseVtbl;

            public IntPtr SetOutputBounds;

            public IntPtr GetOutputBounds;
            #endregion
        }
        #endregion
    }
}
