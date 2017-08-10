// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dwrite_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>The IDWriteAsyncResult interface represents the result of an asynchronous operation. A client can use the interface to wait for the operation to complete and to get the result.</summary>
    [Guid("CE25F8FD-863B-4D13-9651-C1F88DC73FE2")]
    unsafe public /* blittable */ struct IDWriteAsyncResult
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>The GetWaitHandleMethod method returns a handle that can be used to wait for the asynchronous operation to complete. The handle remains valid until the interface is released.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HANDLE")]
        public /* static */ delegate void* GetWaitHandle(
            [In] IDWriteAsyncResult* This
        );

        /// <summary>The GetResult method returns the result of the asynchronous operation. The return value is E_PENDING if the operation has not yet completed.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetResult(
            [In] IDWriteAsyncResult* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetWaitHandle;

            public IntPtr GetResult;
            #endregion
        }
        #endregion
    }
}
