// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("3FEBD6DD-4973-4787-8194-E45F9E28923E")]
    unsafe public struct ID3D12DebugDevice
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DebugDevice).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetFeatureMask(
            [In] ID3D12DebugDevice* This,
            [In] D3D12_DEBUG_FEATURE Mask
        );

        public /* static */ delegate D3D12_DEBUG_FEATURE GetFeatureMask(
            [In] ID3D12DebugDevice* This
        );

        public /* static */ delegate HRESULT ReportLiveDeviceObjects(
            [In] ID3D12DebugDevice* This,
            [In] D3D12_RLDO_FLAGS Flags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public SetFeatureMask SetFeatureMask;

            public GetFeatureMask GetFeatureMask;

            public ReportLiveDeviceObjects ReportLiveDeviceObjects;
            #endregion
        }
        #endregion
    }
}
