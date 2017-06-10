// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("9D8E1289-D7B3-465F-8126-250E349AF85D")]
    unsafe public struct IDXGIKeyedMutex
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT AcquireSync(
            [In] IDXGIKeyedMutex* This,
            [In] ulong Key,
            [In] uint dwMilliseconds
        );

        public /* static */ delegate HRESULT ReleaseSync(
            [In] IDXGIKeyedMutex* This,
            [In] ulong Key
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.QueryInterface QueryInterface;

            public IUnknown.AddRef AddRef;

            public IUnknown.Release Release;

            public IDXGIObject.SetPrivateData SetPrivateData;

            public IDXGIObject.SetPrivateDataInterface SetPrivateDataInterface;

            public IDXGIObject.GetPrivateData GetPrivateData;

            public IDXGIObject.GetParent GetParent;

            public IDXGIDeviceSubObject.GetDevice GetDevice;

            public AcquireSync AcquireSync;

            public ReleaseSync ReleaseSync;
            #endregion
        }
        #endregion
    }
}
