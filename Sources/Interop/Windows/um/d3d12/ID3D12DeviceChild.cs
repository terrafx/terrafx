// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("905DB94B-A00C-4140-9DF5-2B64CA9EA357")]
    unsafe public /* blittable */ struct ID3D12DeviceChild
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDevice(
            [In] ID3D12DeviceChild* This,
            [In, ComAliasName("REFIID")] /* readonly */ Guid* riid,
            [Out, Optional] void** ppvDevice
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Object.Vtbl BaseVtbl;

            public IntPtr GetDevice;
            #endregion
        }
        #endregion
    }
}
