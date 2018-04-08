// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct ID3DInclude
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Open(
            [In] ID3DInclude* This,
            [In] D3D_INCLUDE_TYPE IncludeType,
            [In, ComAliasName("LPCSTR")] sbyte* pFileName,
            [In, ComAliasName("LPCVOID")] void* pParentData,
            [Out, ComAliasName("LPCVOID")] void** ppData,
            [Out, ComAliasName("UINT")] uint* pBytes
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Close(
            [In] ID3DInclude* This,
            [In, ComAliasName("LPCVOID")] void* pData
        );
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int Open(
            [In] D3D_INCLUDE_TYPE IncludeType,
            [In, ComAliasName("LPCSTR")] sbyte* pFileName,
            [In, ComAliasName("LPCVOID")] void* pParentData,
            [Out, ComAliasName("LPCVOID")] void** ppData,
            [Out, ComAliasName("UINT")] uint* pBytes
        )
        {
            fixed (ID3DInclude* This = &this)
            {
                return MarshalFunction<_Open>(lpVtbl->Open)(
                    This,
                    IncludeType,
                    pFileName,
                    pParentData,
                    ppData,
                    pBytes
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Close(
            [In, ComAliasName("LPCVOID")] void* pData
        )
        {
            fixed (ID3DInclude* This = &this)
            {
                return MarshalFunction<_Close>(lpVtbl->Close)(
                    This,
                    pData
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region Fields
            public IntPtr Open;

            public IntPtr Close;
            #endregion
        }
        #endregion
    }
}

