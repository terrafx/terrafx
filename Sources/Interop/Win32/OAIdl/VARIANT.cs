// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public struct VARIANT
    {
        #region Fields
        public _Anonymous_e__Union Anonymous;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public _Anonymous2_e__Struct Anyonmous2;

            [FieldOffset(0)]
            public DECIMAL decVal;
            #endregion

            #region Structs
            public struct _Anonymous2_e__Struct
            {
                #region Fields
                public VARTYPE vt;

                public ushort wReserved1;

                public ushort wReserved2;

                public ushort wReserved3;

                public _Anonymous3_e__Union Anonymous3;
                #endregion

                #region Structs
                [StructLayout(LayoutKind.Explicit)]
                public struct _Anonymous3_e__Union
                {
                    #region Fields
                    [FieldOffset(8)]
                    public long llVal;

                    [FieldOffset(8)]
                    public int lVal;

                    [FieldOffset(8)]
                    public byte bVal;

                    [FieldOffset(8)]
                    public short iVal;

                    [FieldOffset(8)]
                    public float fltVal;

                    [FieldOffset(8)]
                    public double dblVal;

                    [FieldOffset(8)]
                    public VARIANT_BOOL boolVal;

                    [FieldOffset(8)]
                    public VARIANT_BOOL @bool;

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
                    public byte* pbVal;

                    [FieldOffset(8)]
                    public short* piVal;

                    [FieldOffset(8)]
                    public int* plVal;

                    [FieldOffset(8)]
                    public long* pllVal;

                    [FieldOffset(8)]
                    public float* pfltVal;

                    [FieldOffset(8)]
                    public double* pdblVal;

                    [FieldOffset(8)]
                    public VARIANT_BOOL* pboolVal;

                    [FieldOffset(8)]
                    public VARIANT_BOOL* pbool;

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
                    public void* byref;

                    [FieldOffset(8)]
                    public sbyte cVal;

                    [FieldOffset(8)]
                    public ushort uiVal;

                    [FieldOffset(8)]
                    public uint ulVal;

                    [FieldOffset(8)]
                    public ulong ullVal;

                    [FieldOffset(8)]
                    public int intVal;

                    [FieldOffset(8)]
                    public uint uintVal;

                    [FieldOffset(8)]
                    public DECIMAL* pdecVal;

                    [FieldOffset(8)]
                    public sbyte* pcVal;

                    [FieldOffset(8)]
                    public ushort* puiVal;

                    [FieldOffset(8)]
                    public uint* pulVal;

                    [FieldOffset(8)]
                    public ulong* pullVal;

                    [FieldOffset(8)]
                    public int* pintVal;

                    [FieldOffset(8)]
                    public uint* puintVal;

                    [FieldOffset(0)]
                    public _Anonymous4_e__Struct Anonymous4;
                    #endregion

                    #region Structs
                    public struct _Anonymous4_e__Struct
                    {
                        #region Fields
                        public void* pvRecord;

                        public IRecordInfo* pRecInfo;
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }
}
