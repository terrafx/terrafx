// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("93A665C4-A3B2-4E5D-B692-A26AE14E3374")]
    unsafe public struct ID3D12Debug2
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Debug2).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void SetGPUBasedValidationFlags(
            [In] ID3D12Debug2* This,
            [In] D3D12_GPU_BASED_VALIDATION_FLAGS Flags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public SetGPUBasedValidationFlags SetGPUBasedValidationFlags;
            #endregion
        }
        #endregion
    }
}
