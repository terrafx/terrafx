// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("77DB970F-6276-48BA-BA28-070143B4392C")]
    unsafe public /* blittable */ struct IDXGIDevice1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetMaximumFrameLatency(
            [In] IDXGIDevice1* This,
            [In] UINT MaxLatency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMaximumFrameLatency(
            [In] IDXGIDevice1* This,
            [Out] UINT* pMaxLatency
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIDevice.Vtbl BaseVtbl;

            public SetMaximumFrameLatency SetMaximumFrameLatency;

            public GetMaximumFrameLatency GetMaximumFrameLatency;
            #endregion
        }
        #endregion
    }
}
