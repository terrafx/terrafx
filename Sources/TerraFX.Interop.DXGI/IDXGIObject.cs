// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("AEC22FB8-76F3-4639-9BE0-28EB43A67A2E")]
    unsafe public struct IDXGIObject
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetPrivateData(
            [In] IDXGIObject* This,
            [In] Guid* Name,
            [In] uint DataSize,
            [In] void* pData
        );

        public /* static */ delegate HRESULT SetPrivateDataInterface(
            [In] IDXGIObject* This,
            [In] Guid* Name,
            [In, Optional] IUnknown* pUnknown
        );

        public /* static */ delegate HRESULT GetPrivateData(
            [In] IDXGIObject* This,
            [In] Guid* Name,
            [In, Out] uint* pDataSize,
            [Out] void* pData
        );

        public /* static */ delegate HRESULT GetParent(
            [In] IDXGIObject* This,
            [In] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.QueryInterface QueryInterface;

            public IUnknown.AddRef AddRef;

            public IUnknown.Release Release;

            public SetPrivateData SetPrivateData;

            public SetPrivateDataInterface SetPrivateDataInterface;

            public GetPrivateData GetPrivateData;

            public GetParent GetParent;
            #endregion
        }
        #endregion
    }
}
