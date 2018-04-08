// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public /* unmanaged */ unsafe struct VARIANT
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        [ComAliasName("VARTYPE")]
        public ushort vt;

        [FieldOffset(2)]
        [ComAliasName("WORD")]
        public ushort wReserved1;

        [FieldOffset(4)]
        [ComAliasName("WORD")]
        public ushort wReserved2;

        [FieldOffset(6)]
        [ComAliasName("WORD")]
        public ushort wReserved3;

        #region union
        [FieldOffset(8)]
        [ComAliasName("LONGLONG")]
        public long llVal;

        [FieldOffset(8)]
        [ComAliasName("LONG")]
        public int lVal;

        [FieldOffset(8)]
        [ComAliasName("BYTE")]
        public byte bVal;

        [FieldOffset(8)]
        [ComAliasName("SHORT")]
        public short iVal;

        [FieldOffset(8)]
        [ComAliasName("FLOAT")]
        public float fltVal;

        [FieldOffset(8)]
        [ComAliasName("DOUBLE")]
        public double dblVal;

        [FieldOffset(8)]
        [ComAliasName("VARIANT_BOOL")]
        public short boolVal;

        [FieldOffset(8)]
        [ComAliasName("SCODE")]
        public int scode;

        [FieldOffset(8)]
        public CY cyVal;

        [FieldOffset(8)]
        [ComAliasName("DATE")]
        public double date;

        [FieldOffset(8)]
        [ComAliasName("BSTR")]
        public char* bstrVal;

        [FieldOffset(8)]
        public IUnknown* punkVal;

        [FieldOffset(8)]
        public IDispatch* pdispVal;

        [FieldOffset(8)]
        public SAFEARRAY* parray;

        [FieldOffset(8)]
        [ComAliasName("BYTE")]
        public byte* pbVal;

        [FieldOffset(8)]
        [ComAliasName("SHORT")]
        public short* piVal;

        [FieldOffset(8)]
        [ComAliasName("LONG")]
        public int* plVal;

        [FieldOffset(8)]
        [ComAliasName("LONGLONG")]
        public long* pllVal;

        [FieldOffset(8)]
        [ComAliasName("FLOAT")]
        public float* pfltVal;

        [FieldOffset(8)]
        [ComAliasName("DOUBLE")]
        public double* pdblVal;

        [FieldOffset(8)]
        [ComAliasName("VARIANT_BOOL")]
        public short* pboolVal;

        [FieldOffset(8)]
        [ComAliasName("SCODE")]
        public int* pscode;

        [FieldOffset(8)]
        public CY* pcyVal;

        [FieldOffset(8)]
        [ComAliasName("DATE")]
        public double* pdate;

        [FieldOffset(8)]
        [ComAliasName("BSTR")]
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
        [ComAliasName("PVOID")]
        public void* byref;

        [FieldOffset(8)]
        [ComAliasName("CHAR")]
        public sbyte cVal;

        [FieldOffset(8)]
        [ComAliasName("USHORT")]
        public ushort uiVal;

        [FieldOffset(8)]
        [ComAliasName("ULONG")]
        public uint ulVal;

        [FieldOffset(8)]
        [ComAliasName("ULONGLONG")]
        public ulong ullVal;

        [FieldOffset(8)]
        [ComAliasName("INT")]
        public int intVal;

        [FieldOffset(8)]
        [ComAliasName("UINT")]
        public uint uintVal;

        [FieldOffset(8)]
        public DECIMAL* pdecVal;

        [FieldOffset(8)]
        [ComAliasName("CHAR")]
        public sbyte* pcVal;

        [FieldOffset(8)]
        [ComAliasName("USHORT")]
        public ushort* puiVal;

        [FieldOffset(8)]
        [ComAliasName("ULONG")]
        public uint* pulVal;

        [FieldOffset(8)]
        [ComAliasName("ULONGLONG")]
        public ulong* pullVal;

        [FieldOffset(8)]
        [ComAliasName("INT")]
        public int* pintVal;

        [FieldOffset(8)]
        [ComAliasName("UINT")]
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
