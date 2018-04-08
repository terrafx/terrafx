// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\documenttarget.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("1B8EFEC4-3019-4C27-964E-367202156906")]
    public /* unmanaged */ unsafe struct IPrintDocumentPackageTarget
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IPrintDocumentPackageTarget* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IPrintDocumentPackageTarget* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IPrintDocumentPackageTarget* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPackageTargetTypes(
            [In] IPrintDocumentPackageTarget* This,
            [Out, ComAliasName("UINT32")] uint* targetCount,
            [Out, ComAliasName("GUID[]")] Guid** targetTypes = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPackageTarget(
            [In] IPrintDocumentPackageTarget* This,
            [In, ComAliasName("REFGUID")] Guid* guidTargetType,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvTarget = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Cancel(
            [In] IPrintDocumentPackageTarget* This
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IPrintDocumentPackageTarget* This = &this)
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
            fixed (IPrintDocumentPackageTarget* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IPrintDocumentPackageTarget* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int GetPackageTargetTypes(
            [Out, ComAliasName("UINT32")] uint* targetCount,
            [Out, ComAliasName("GUID[]")] Guid** targetTypes = null
        )
        {
            fixed (IPrintDocumentPackageTarget* This = &this)
            {
                return MarshalFunction<_GetPackageTargetTypes>(lpVtbl->GetPackageTargetTypes)(
                    This,
                    targetCount,
                    targetTypes
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPackageTarget(
            [In, ComAliasName("REFGUID")] Guid* guidTargetType,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvTarget = null
        )
        {
            fixed (IPrintDocumentPackageTarget* This = &this)
            {
                return MarshalFunction<_GetPackageTarget>(lpVtbl->GetPackageTarget)(
                    This,
                    guidTargetType,
                    riid,
                    ppvTarget
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Cancel()
        {
            fixed (IPrintDocumentPackageTarget* This = &this)
            {
                return MarshalFunction<_Cancel>(lpVtbl->Cancel)(
                    This
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
            public IntPtr GetPackageTargetTypes;

            public IntPtr GetPackageTarget;

            public IntPtr Cancel;
            #endregion
        }
        #endregion
    }
}

