// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Creates Direct2D resources. This interface also enables the creation of ID2D1Device5 objects.</summary>
    [Guid("F9976F46-F642-44C1-97CA-DA32EA2A2635")]
    unsafe public /* blittable */ struct ID2D1Factory6
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>This creates a new Direct2D device from the given IDXGIDevice.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDevice(
            [In] ID2D1Factory6* This,
            [In] IDXGIDevice* dxgiDevice,
            [Out] ID2D1Device5** d2dDevice5
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID2D1Factory5.Vtbl BaseVtbl;

            public CreateDevice CreateDevice;
            #endregion
        }
        #endregion
    }
}
