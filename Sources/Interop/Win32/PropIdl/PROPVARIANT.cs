// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\PropIdlBase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
#pragma warning disable CS1591
    unsafe public struct PROPVARIANT
    {
        #region Fields
        public VARTYPE vt;

        public byte wReserved1;

        public byte wReserved2;

        public uint wReserved3;

        public _Anonymous_e__Union Anonymous;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Explicit)]
        public struct _Anonymous_e__Union
        {
            #region Fields
            [FieldOffset(0)]
            public sbyte cVal;

            [FieldOffset(0)]
            public byte bVal;

            [FieldOffset(0)]
            public short iVal;

            [FieldOffset(0)]
            public ushort uiVal;

            [FieldOffset(0)]
            public int lVal;

            [FieldOffset(0)]
            public uint ulVal;

            [FieldOffset(0)]
            public int intVal;

            [FieldOffset(0)]
            public uint uintVal;

            [FieldOffset(0)]
            public long hVal;

            [FieldOffset(0)]
            public ulong uhVal;

            [FieldOffset(0)]
            public float fltVal;

            [FieldOffset(0)]
            public double dblVal;

            [FieldOffset(0)]
            public VARIANT_BOOL boolVal;

            [FieldOffset(0)]
            public VARIANT_BOOL @bool;

            [FieldOffset(0)]
            public SCODE scode;

            [FieldOffset(0)]
            public CY cyVal;

            [FieldOffset(0)]
            public DATE date;

            [FieldOffset(0)]
            public FILETIME filetime;

            [FieldOffset(0)]
            public Guid* puuid;

            [FieldOffset(0)]
            public CLIPDATA* pclipdata;

            [FieldOffset(0)]
            public BSTR bstrVal;

            [FieldOffset(0)]
            public BSTRBLOB bstrblobVal;

            [FieldOffset(0)]
            public BLOB blob;

            [FieldOffset(0)]
            public LPSTR pszVal;

            [FieldOffset(0)]
            public LPWSTR pwszVal;

            [FieldOffset(0)]
            public IUnknown* punkVal;

            [FieldOffset(0)]
            public IDispatch* pdispVal;

            [FieldOffset(0)]
            public IStream* pStream;

            [FieldOffset(0)]
            public IStorage* pSTorage;

            [FieldOffset(0)]
            public VERSIONEDSTREAM* pVersionedStream;

            [FieldOffset(0)]
            public SAFEARRAY* parray;

            [FieldOffset(0)]
            public CAC cac;

            [FieldOffset(0)]
            public CAUB caub;

            [FieldOffset(0)]
            public CAI cai;

            [FieldOffset(0)]
            public CAUI caui;

            [FieldOffset(0)]
            public CAL cal;

            [FieldOffset(0)]
            public CAUL caul;

            [FieldOffset(0)]
            public CAH cah;

            [FieldOffset(0)]
            public CAUH cauh;

            [FieldOffset(0)]
            public CAFLT caflt;

            [FieldOffset(0)]
            public CADBL cadbl;

            [FieldOffset(0)]
            public CABOOL cabool;

            [FieldOffset(0)]
            public CASCODE cascode;

            [FieldOffset(0)]
            public CACY cacy;

            [FieldOffset(0)]
            public CADATE cadata;

            [FieldOffset(0)]
            public CAFILETIME cafiletime;

            [FieldOffset(0)]
            public CACLSID cauuid;

            [FieldOffset(0)]
            public CACLIPDATA caclipdata;

            [FieldOffset(0)]
            public CABSTR cabstr;

            [FieldOffset(0)]
            public CABSTRBLOB cabstrblob;

            [FieldOffset(0)]
            public CALPSTR calpstr;

            [FieldOffset(0)]
            public CALPWSTR calpwstr;

            [FieldOffset(0)]
            public CAPROPVARIANT capropvar;

            [FieldOffset(0)]
            public sbyte* pcVal;

            [FieldOffset(0)]
            public byte* pbVal;

            [FieldOffset(0)]
            public short* piVal;

            [FieldOffset(0)]
            public ushort* puiVal;

            [FieldOffset(0)]
            public int* plVal;

            [FieldOffset(0)]
            public uint* pulVal;

            [FieldOffset(0)]
            public int* pintVal;

            [FieldOffset(0)]
            public uint* puintVal;

            [FieldOffset(0)]
            public float* pfltVal;

            [FieldOffset(0)]
            public double* pdblVal;

            [FieldOffset(0)]
            public VARIANT_BOOL* pboolVal;

            [FieldOffset(0)]
            public DECIMAL* pdecVal;

            [FieldOffset(0)]
            public SCODE* pscode;

            [FieldOffset(0)]
            public CY* pcy;

            [FieldOffset(0)]
            public DATE* pdate;

            [FieldOffset(0)]
            public BSTR* pbstrVal;

            [FieldOffset(0)]
            public IUnknown** ppunkVal;

            [FieldOffset(0)]
            public IDispatch** ppdispVal;

            [FieldOffset(0)]
            public SAFEARRAY** pparray;

            [FieldOffset(0)]
            public PROPVARIANT* pvarVal;
            #endregion
        }
        #endregion
    }
#pragma warning restore CS1591
}
