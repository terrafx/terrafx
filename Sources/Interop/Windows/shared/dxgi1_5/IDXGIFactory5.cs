// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("7632E1F5-EE65-4DCA-87FD-84CD75F8838D")]
    public /* blittable */ unsafe struct IDXGIFactory5
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CheckFeatureSupport(
            [In] IDXGIFactory5* This,
            [In] DXGI_FEATURE Feature,
            [In, Out] void* pFeatureSupportData,
            [In, ComAliasName("UINT")] uint FeatureSupportDataSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIFactory4.Vtbl BaseVtbl;

            public IntPtr CheckFeatureSupport;
            #endregion
        }
        #endregion
    }
}
