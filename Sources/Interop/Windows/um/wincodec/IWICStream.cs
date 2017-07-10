// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("135FF860-22B7-4DDF-B0F6-218F4F299A43")]
    unsafe public /* blittable */ struct IWICStream
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeFromIStream(
            [In] IWICStream* This,
            [In, Optional] IStream* pIStream
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeFromFilename(
            [In] IWICStream* This,
            [In] LPCWSTR wzFileName,
            [In] DWORD dwDesiredAccess
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeFromMemory(
            [In] IWICStream* This,
            [In] WICInProcPointer pbBuffer,
            [In] DWORD cbBufferSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT InitializeFromIStreamRegion(
            [In] IWICStream* This,
            [In, Optional] IStream* pIStream,
            [In] ULARGE_INTEGER ulOffset,
            [In] ULARGE_INTEGER ulMaxSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IStream.Vtbl BaseVtbl;

            public InitializeFromIStream InitializeFromIStream;

            public InitializeFromFilename InitializeFromFilename;

            public InitializeFromMemory InitializeFromMemory;

            public InitializeFromIStreamRegion InitializeFromIStreamRegion;
            #endregion
        }
        #endregion
    }
}
