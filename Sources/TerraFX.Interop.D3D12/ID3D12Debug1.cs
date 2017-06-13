// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("AFFAA4CA-63FE-4D8E-B8AD-159000AF4304")]
    unsafe public struct ID3D12Debug1
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Debug1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void EnableDebugLayer(
            [In] ID3D12Debug1* This
        );

        public /* static */ delegate void SetEnableGPUBasedValidation(
            [In] ID3D12Debug1* This,
            [In] BOOL Enable
        );

        public /* static */ delegate void SetEnableSynchronizedCommandQueueValidation(
            [In] ID3D12Debug1* This,
            [In] BOOL Enable
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public EnableDebugLayer EnableDebugLayer;

            public SetEnableGPUBasedValidation SetEnableGPUBasedValidation;

            public SetEnableSynchronizedCommandQueueValidation SetEnableSynchronizedCommandQueueValidation;
            #endregion
        }
        #endregion
    }
}
