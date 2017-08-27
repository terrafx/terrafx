// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a set of run-time bindable and discoverable properties that allow a data-driven application to modify the state of a Direct2D effect.</summary>
    [Guid("483473D7-CD46-4F9D-9D3A-3112AA80159D")]
    public /* blittable */ unsafe struct ID2D1Properties
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns the total number of custom properties in this interface.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPropertyCount(
            [In] ID2D1Properties* This
        );

        /// <summary>Retrieves the property name from the given property index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPropertyName(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out, ComAliasName("PWSTR")] char* name,
            [In, ComAliasName("UINT32")] uint nameCount
        );

        /// <summary>Returns the length of the property name from the given index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPropertyNameLength(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index
        );

        /// <summary>Retrieves the type of the given property.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_PROPERTY_TYPE _GetType(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index
        );

        /// <summary>Retrieves the property index for the given property name.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetPropertyIndex(
            [In] ID2D1Properties* This,
            [In, ComAliasName("PCWSTR")] char* name
        );

        /// <summary>Sets the value of the given property using its name.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetValueByName(
            [In] ID2D1Properties* This,
            [In, ComAliasName("PCWSTR")] char* name,
            [In] D2D1_PROPERTY_TYPE type,
            [In, ComAliasName("BYTE[]")] byte* data,
            [In, ComAliasName("UINT32")] uint dataSize
        );

        /// <summary>Sets the given value using the property index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetValue(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index,
            [In] D2D1_PROPERTY_TYPE type,
            [In, ComAliasName("BYTE[]")] byte* data,
            [In, ComAliasName("UINT32")] uint dataSize
        );

        /// <summary>Retrieves the given property or sub-property by name. '.' is the delimiter for sub-properties.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetValueByName(
            [In] ID2D1Properties* This,
            [In, ComAliasName("PCWSTR")] char* name,
            [In] D2D1_PROPERTY_TYPE type,
            [Out, ComAliasName("BYTE[]")] byte* data,
            [In, ComAliasName("UINT32")] uint dataSize
        );

        /// <summary>Retrieves the given value by index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetValue(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index,
            [In] D2D1_PROPERTY_TYPE type,
            [Out, ComAliasName("BYTE[]")] byte* data,
            [In, ComAliasName("UINT32")] uint dataSize
        );

        /// <summary>Returns the value size for the given property index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT32")]
        public /* static */ delegate uint GetValueSize(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index
        );

        /// <summary>Retrieves the sub-properties of the given property by index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSubProperties(
            [In] ID2D1Properties* This,
            [In, ComAliasName("UINT32")] uint index,
            [Out] ID2D1Properties** subProperties
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr GetPropertyCount;

            public IntPtr GetPropertyName;

            public IntPtr GetPropertyNameLength;

            public IntPtr _GetType;

            public IntPtr GetPropertyIndex;

            public IntPtr SetValueByName;

            public IntPtr SetValue;

            public IntPtr GetValueByName;

            public IntPtr GetValue;

            public IntPtr GetValueSize;

            public IntPtr GetSubProperties;
            #endregion
        }
        #endregion
    }
}
