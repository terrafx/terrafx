// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("068346E8-AAEC-4B84-ADD7-137F513F77A1")]
    unsafe public /* blittable */ struct IDXGIOutput6
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDesc1(
            [In] IDXGIOutput6* This,
            [Out] DXGI_OUTPUT_DESC1* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CheckHardwareCompositionSupport(
            [In] IDXGIOutput6* This,
            [Out, ComAliasName("UINT")] uint* pFlags
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIOutput5.Vtbl BaseVtbl;

            public GetDesc1 GetDesc1;

            public CheckHardwareCompositionSupport CheckHardwareCompositionSupport;
            #endregion
        }
        #endregion
    }
}
