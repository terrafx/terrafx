// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Creates Direct2D resources. This interface also enables the creation of ID2D1Device4 objects.</summary>
    [Guid("C4349994-838E-4B0F-8CAB-44997D9EEACC")]
    public /* blittable */ unsafe struct ID2D1Factory5
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>This creates a new Direct2D device from the given IDXGIDevice.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDevice(
            [In] ID2D1Factory5* This,
            [In] IDXGIDevice* dxgiDevice,
            [Out] ID2D1Device4** d2dDevice4
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Factory4.Vtbl BaseVtbl;

            public IntPtr CreateDevice;
            #endregion
        }
        #endregion
    }
}
