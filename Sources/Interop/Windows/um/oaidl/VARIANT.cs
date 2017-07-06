// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    unsafe public  /* blittable */ struct VARIANT
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        public VARTYPE vt;

        [FieldOffset(2)]
        public WORD wReserved1;

        [FieldOffset(4)]
        public WORD wReserved2;

        [FieldOffset(6)]
        public WORD wReserved3;

        #region union
        [FieldOffset(8)]
        public LONGLONG llVal;

        [FieldOffset(8)]
        public LONG lVal;

        [FieldOffset(8)]
        public BYTE bVal;

        [FieldOffset(8)]
        public SHORT iVal;

        [FieldOffset(8)]
        public FLOAT fltVal;

        [FieldOffset(8)]
        public DOUBLE dblVal;

        [FieldOffset(8)]
        public VARIANT_BOOL boolVal;

        [FieldOffset(8)]
        public SCODE scode;

        [FieldOffset(8)]
        public CY cyVal;

        [FieldOffset(8)]
        public DATE date;

        [FieldOffset(8)]
        public BSTR bstrVal;

        [FieldOffset(8)]
        public IUnknown* punkVal;

        [FieldOffset(8)]
        public IDispatch* pdispVal;

        [FieldOffset(8)]
        public SAFEARRAY* parray;

        [FieldOffset(8)]
        public BYTE* pbVal;

        [FieldOffset(8)]
        public SHORT* piVal;

        [FieldOffset(8)]
        public LONG* plVal;

        [FieldOffset(8)]
        public LONGLONG* pllVal;

        [FieldOffset(8)]
        public FLOAT* pfltVal;

        [FieldOffset(8)]
        public DOUBLE* pdblVal;

        [FieldOffset(8)]
        public VARIANT_BOOL* pboolVal;

        [FieldOffset(8)]
        public SCODE* pscode;

        [FieldOffset(8)]
        public CY* pcyVal;

        [FieldOffset(8)]
        public DATE* pdate;

        [FieldOffset(8)]
        public BSTR* pbstrVal;

        [FieldOffset(8)]
        public IUnknown** ppunkVal;

        [FieldOffset(8)]
        public IDispatch** ppdispVal;

        [FieldOffset(8)]
        public SAFEARRAY** pparray;

        [FieldOffset(8)]
        public VARIANT* pvarVal;

        [FieldOffset(8)]
        public PVOID byref;

        [FieldOffset(8)]
        public CHAR cVal;

        [FieldOffset(8)]
        public USHORT uiVal;

        [FieldOffset(8)]
        public ULONG ulVal;

        [FieldOffset(8)]
        public ULONGLONG ullVal;

        [FieldOffset(8)]
        public INT intVal;

        [FieldOffset(8)]
        public UINT uintVal;

        [FieldOffset(8)]
        public DECIMAL* pdecVal;

        [FieldOffset(8)]
        public CHAR* pcVal;

        [FieldOffset(8)]
        public USHORT* puiVal;

        [FieldOffset(8)]
        public ULONG* pulVal;

        [FieldOffset(8)]
        public ULONGLONG* pullVal;

        [FieldOffset(8)]
        public INT* pintVal;

        [FieldOffset(8)]
        public UINT* puintVal;

        [FieldOffset(8)]
        public BRECORD brecVal;
        #endregion
        #endregion

        [FieldOffset(0)]
        public DECIMAL decVal;
        #endregion
    }
}
