// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("688D15C3-02B0-438D-B13A-D1B44C32C39A")]
    public /* blittable */ unsafe struct ID2D1ResourceTexture
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID2D1ResourceTexture* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID2D1ResourceTexture* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID2D1ResourceTexture* This
        );
        #endregion

        #region Delegates
        /// <summary>Update the vertex text.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Update(
            [In] ID2D1ResourceTexture* This,
            [In, Optional, ComAliasName("UINT32[]")] uint* minimumExtents,
            [In, Optional, ComAliasName("UINT32[]")] uint* maximimumExtents,
            [In, Optional, ComAliasName("UINT32[]")] uint* strides,
            [In, ComAliasName("UINT32")] uint dimensions,
            [In, ComAliasName("BYTE[]")]  byte* data,
            [In, ComAliasName("UINT32")] uint dataCount
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID2D1ResourceTexture* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (ID2D1ResourceTexture* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID2D1ResourceTexture* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int Update(
            [In, Optional, ComAliasName("UINT32[]")] uint* minimumExtents,
            [In, Optional, ComAliasName("UINT32[]")] uint* maximimumExtents,
            [In, Optional, ComAliasName("UINT32[]")] uint* strides,
            [In, ComAliasName("UINT32")] uint dimensions,
            [In, ComAliasName("BYTE[]")]  byte* data,
            [In, ComAliasName("UINT32")] uint dataCount
        )
        {
            fixed (ID2D1ResourceTexture* This = &this)
            {
                return MarshalFunction<_Update>(lpVtbl->Update)(
                    This,
                    minimumExtents,
                    maximimumExtents,
                    strides,
                    dimensions,
                    data,
                    dataCount
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr Update;
            #endregion
        }
        #endregion
    }
}

