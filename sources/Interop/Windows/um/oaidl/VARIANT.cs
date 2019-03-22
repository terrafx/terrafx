// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    [Unmanaged]
    public unsafe struct VARIANT
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        [NativeTypeName("VARTYPE")]
        public ushort vt;

        [FieldOffset(2)]
        [NativeTypeName("WORD")]
        public ushort wReserved1;

        [FieldOffset(4)]
        [NativeTypeName("WORD")]
        public ushort wReserved2;

        [FieldOffset(6)]
        [NativeTypeName("WORD")]
        public ushort wReserved3;

        #region union
        [FieldOffset(8)]
        [NativeTypeName("LONGLONG")]
        public long llVal;

        [FieldOffset(8)]
        [NativeTypeName("LONG")]
        public int lVal;

        [FieldOffset(8)]
        [NativeTypeName("BYTE")]
        public byte bVal;

        [FieldOffset(8)]
        [NativeTypeName("SHORT")]
        public short iVal;

        [FieldOffset(8)]
        [NativeTypeName("FLOAT")]
        public float fltVal;

        [FieldOffset(8)]
        [NativeTypeName("DOUBLE")]
        public double dblVal;

        [FieldOffset(8)]
        [NativeTypeName("VARIANT_BOOL")]
        public short boolVal;

        [FieldOffset(8)]
        [NativeTypeName("SCODE")]
        public int scode;

        [FieldOffset(8)]
        public CY cyVal;

        [FieldOffset(8)]
        [NativeTypeName("DATE")]
        public double date;

        [FieldOffset(8)]
        [NativeTypeName("BSTR")]
        public char* bstrVal;

        [FieldOffset(8)]
        public IUnknown* punkVal;

        [FieldOffset(8)]
        public IDispatch* pdispVal;

        [FieldOffset(8)]
        public SAFEARRAY* parray;

        [FieldOffset(8)]
        [NativeTypeName("BYTE")]
        public byte* pbVal;

        [FieldOffset(8)]
        [NativeTypeName("SHORT")]
        public short* piVal;

        [FieldOffset(8)]
        [NativeTypeName("LONG")]
        public int* plVal;

        [FieldOffset(8)]
        [NativeTypeName("LONGLONG")]
        public long* pllVal;

        [FieldOffset(8)]
        [NativeTypeName("FLOAT")]
        public float* pfltVal;

        [FieldOffset(8)]
        [NativeTypeName("DOUBLE")]
        public double* pdblVal;

        [FieldOffset(8)]
        [NativeTypeName("VARIANT_BOOL")]
        public short* pboolVal;

        [FieldOffset(8)]
        [NativeTypeName("SCODE")]
        public int* pscode;

        [FieldOffset(8)]
        public CY* pcyVal;

        [FieldOffset(8)]
        [NativeTypeName("DATE")]
        public double* pdate;

        [FieldOffset(8)]
        [NativeTypeName("BSTR")]
        public char** pbstrVal;

        [FieldOffset(8)]
        public IUnknown** ppunkVal;

        [FieldOffset(8)]
        public IDispatch** ppdispVal;

        [FieldOffset(8)]
        public SAFEARRAY** pparray;

        [FieldOffset(8)]
        public VARIANT* pvarVal;

        [FieldOffset(8)]
        [NativeTypeName("PVOID")]
        public void* byref;

        [FieldOffset(8)]
        [NativeTypeName("CHAR")]
        public sbyte cVal;

        [FieldOffset(8)]
        [NativeTypeName("USHORT")]
        public ushort uiVal;

        [FieldOffset(8)]
        [NativeTypeName("ULONG")]
        public uint ulVal;

        [FieldOffset(8)]
        [NativeTypeName("ULONGLONG")]
        public ulong ullVal;

        [FieldOffset(8)]
        [NativeTypeName("INT")]
        public int intVal;

        [FieldOffset(8)]
        [NativeTypeName("UINT")]
        public uint uintVal;

        [FieldOffset(8)]
        public DECIMAL* pdecVal;

        [FieldOffset(8)]
        [NativeTypeName("CHAR")]
        public sbyte* pcVal;

        [FieldOffset(8)]
        [NativeTypeName("USHORT")]
        public ushort* puiVal;

        [FieldOffset(8)]
        [NativeTypeName("ULONG")]
        public uint* pulVal;

        [FieldOffset(8)]
        [NativeTypeName("ULONGLONG")]
        public ulong* pullVal;

        [FieldOffset(8)]
        [NativeTypeName("INT")]
        public int* pintVal;

        [FieldOffset(8)]
        [NativeTypeName("UINT")]
        public uint* puintVal;

        [FieldOffset(8)]
        public BRECORD brecVal;
        #endregion
        #endregion

        [FieldOffset(0)]
        public DECIMAL decVal;
        #endregion
    }
}
