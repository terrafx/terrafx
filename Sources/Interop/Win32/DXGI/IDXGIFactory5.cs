// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_5.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("7632E1F5-EE65-4DCA-87FD-84CD75F8838D")]
    unsafe public struct IDXGIFactory5
    {
        #region Constants
        public static readonly Guid IID = typeof(IDXGIFactory5).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CheckFeatureSupport(
            [In] IDXGIFactory5* This,
            [In] DXGI_FEATURE Feature,
            [In, Out] void* pFeatureSupportData,
            [In] uint FeatureSupportDataSize
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IDXGIFactory4.Vtbl BaseVtbl;

            public CheckFeatureSupport CheckFeatureSupport;
            #endregion
        }
        #endregion
    }
}
