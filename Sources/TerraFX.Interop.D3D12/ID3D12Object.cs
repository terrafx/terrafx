// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("C4FEC28F-7966-4E95-9F94-F431CB56C3B8")]
    unsafe public struct ID3D12Object
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Object).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetPrivateData(
            [In] ID3D12Object* This,
            [In] Guid* guid,
            [In, Out] uint* pDataSize,
            [Out, Optional] void* pData
        );

        public /* static */ delegate HRESULT SetPrivateData(
            [In] ID3D12Object* This,
            [In] Guid* guid,
            [In] uint DataSize,
            [In, Optional] void* pData);


        public /* static */ delegate HRESULT SetPrivateDataInterface(
            [In] ID3D12Object* This,
            [In] Guid* guid,
            [In, Optional] IUnknown* pData
        );

        public /* static */ delegate HRESULT SetName(
            [In] ID3D12Object* This,
            [In] LPWSTR Name
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetPrivateData GetPrivateData;

            public SetPrivateData SetPrivateData;

            public SetPrivateDataInterface SetPrivateDataInterface;

            public SetName SetName;
            #endregion
        }
        #endregion
    }
}
