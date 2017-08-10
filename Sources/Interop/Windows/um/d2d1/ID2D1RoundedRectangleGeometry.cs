// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Describes a rounded rectangle.</summary>
    [Guid("2CD906A3-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct ID2D1RoundedRectangleGeometry
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetRoundedRect(
            [In] ID2D1RoundedRectangleGeometry* This,
            [Out] D2D1_ROUNDED_RECT* roundedRect
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Geometry.Vtbl BaseVtbl;

            public IntPtr GetRoundedRect;
            #endregion
        }
        #endregion
    }
}
