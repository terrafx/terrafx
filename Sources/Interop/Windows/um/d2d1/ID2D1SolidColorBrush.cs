// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Paints an area with a solid color.</summary>
    [Guid("2CD906A9-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1SolidColorBrush
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetColor(
            [In] ID2D1SolidColorBrush* This,
            [In, ComAliasName("D2D1_COLOR_F")] /* readonly */ DXGI_RGBA* color
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D2D1_COLOR_F")]
        public /* static */ delegate DXGI_RGBA GetColor(
            [In] ID2D1SolidColorBrush* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Brush.Vtbl BaseVtbl;

            public SetColor SetColor;

            public GetColor GetColor;
            #endregion
        }
        #endregion
    }
}
