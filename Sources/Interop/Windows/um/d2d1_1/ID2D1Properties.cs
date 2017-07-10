// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    /// <summary>Represents a set of run-time bindable and discoverable properties that allow a data-driven application to modify the state of a Direct2D effect.</summary>
    [Guid("483473D7-CD46-4F9D-9D3A-3112AA80159D")]
    unsafe public /* blittable */ struct ID2D1Properties
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        /// <summary>Returns the total number of custom properties in this interface.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetPropertyCount(
            [In] ID2D1Properties* This
        );

        /// <summary>Retrieves the property name from the given property index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPropertyName(
            [In] ID2D1Properties* This,
            [In] UINT32 index,
            [Out] PWSTR name,
            [In] UINT32 nameCount
        );

        /// <summary>Returns the length of the property name from the given index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetPropertyNameLength(
            [In] ID2D1Properties* This,
            [In] UINT32 index
        );

        /// <summary>Retrieves the type of the given property.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D2D1_PROPERTY_TYPE _GetType(
            [In] ID2D1Properties* This,
            [In] UINT32 index
        );

        /// <summary>Retrieves the property index for the given property name.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetPropertyIndex(
            [In] ID2D1Properties* This,
            [In] PCWSTR name
        );

        /// <summary>Sets the value of the given property using its name.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetValueByName(
            [In] ID2D1Properties* This,
            [In] PCWSTR name,
            [In] D2D1_PROPERTY_TYPE type,
            [In] /* readonly */ BYTE* data,
            [In] UINT32 dataSize
        );

        /// <summary>Sets the given value using the property index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetValue(
            [In] ID2D1Properties* This,
            [In] UINT32 index,
            [In] D2D1_PROPERTY_TYPE type,
            [In]  /* readonly */ BYTE* data,
            [In] UINT32 dataSize
        );

        /// <summary>Retrieves the given property or sub-property by name. '.' is the delimiter for sub-properties.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetValueByName(
            [In] ID2D1Properties* This,
            [In] PCWSTR name,
            [In] D2D1_PROPERTY_TYPE type,
            [Out] BYTE* data,
            [In] UINT32 dataSize
        );

        /// <summary>Retrieves the given value by index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetValue(
            [In] ID2D1Properties* This,
            [In] UINT32 index,
            [In] D2D1_PROPERTY_TYPE type,
            [Out] BYTE* data,
            [In] UINT32 dataSize
        );

        /// <summary>Returns the value size for the given property index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT32 GetValueSize(
            [In] ID2D1Properties* This,
            [In] UINT32 index
        );

        /// <summary>Retrieves the sub-properties of the given property by index.</summary>
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSubProperties(
            [In] ID2D1Properties* This,
            [In] UINT32 index,
            [Out] ID2D1Properties** subProperties
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetPropertyCount GetPropertyCount;

            public GetPropertyName GetPropertyName;

            public GetPropertyNameLength GetPropertyNameLength;

            public _GetType _GetType;

            public GetPropertyIndex GetPropertyIndex;

            public SetValueByName SetValueByName;

            public SetValue SetValue;

            public GetValueByName GetValueByName;

            public GetValue GetValue;

            public GetValueSize GetValueSize;

            public GetSubProperties GetSubProperties;
            #endregion
        }
        #endregion
    }
}
