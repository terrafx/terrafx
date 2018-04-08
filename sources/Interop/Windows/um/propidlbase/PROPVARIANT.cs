// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\propidlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public /* unmanaged */ unsafe struct PROPVARIANT
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        [ComAliasName("VARTYPE")]
        public ushort vt;

        [FieldOffset(2)]
        public byte wReserved1;

        [FieldOffset(3)]
        public byte wReserved2;

        [FieldOffset(4)]
        public uint wReserved3;

        #region union
        [FieldOffset(8)]
        [ComAliasName("CHAR")]
        public sbyte cVal;

        [FieldOffset(8)]
        [ComAliasName("UCHAR")]
        public byte bVal;

        [FieldOffset(8)]
        [ComAliasName("SHORT")]
        public short iVal;

        [FieldOffset(8)]
        [ComAliasName("USHORT")]
        public ushort uiVal;

        [FieldOffset(8)]
        [ComAliasName("LONG")]
        public int lVal;

        [FieldOffset(8)]
        [ComAliasName("ULONG")]
        public uint ulVal;

        [FieldOffset(8)]
        [ComAliasName("INT")]
        public int intVal;

        [FieldOffset(8)]
        [ComAliasName("UINT")]
        public uint uintVal;

        [FieldOffset(8)]
        public LARGE_INTEGER hVal;

        [FieldOffset(8)]
        public ULARGE_INTEGER uhVal;

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
        public FILETIME filetime;

        [FieldOffset(8)]
        [ComAliasName("CLSID")]
        public Guid* puuid;

        [FieldOffset(8)]
        public CLIPDATA* pclipdata;

        [FieldOffset(8)]
        [ComAliasName("BSTR")]
        public char* bstrVal;

        [FieldOffset(8)]
        public BSTRBLOB bstrblobVal;

        [FieldOffset(8)]
        public BLOB blob;

        [FieldOffset(8)]
        [ComAliasName("LPSTR")]
        public sbyte* pszVal;

        [FieldOffset(8)]
        [ComAliasName("LPWSTR")]
        public char* pwszVal;

        [FieldOffset(8)]
        public IUnknown* punkVal;

        [FieldOffset(8)]
        public IDispatch* pdispVal;

        [FieldOffset(8)]
        public IStream* pStream;

        [FieldOffset(8)]
        public IStorage* pStorage;

        [FieldOffset(8)]
        [ComAliasName("LPVERSIONEDSTREAM")]
        public VERSIONEDSTREAM* pVersionedStream;

        [FieldOffset(8)]
        [ComAliasName("LPSAFEARRAY")]
        public SAFEARRAY* parray;

        [FieldOffset(8)]
        public CAC cac;

        [FieldOffset(8)]
        public CAUB caub;

        [FieldOffset(8)]
        public CAI cai;

        [FieldOffset(8)]
        public CAUI caui;

        [FieldOffset(8)]
        public CAL cal;

        [FieldOffset(8)]
        public CAUL caul;

        [FieldOffset(8)]
        public CAH cah;

        [FieldOffset(8)]
        public CAUH cauh;

        [FieldOffset(8)]
        public CAFLT caflt;

        [FieldOffset(8)]
        public CADBL cadbl;

        [FieldOffset(8)]
        public CABOOL cabool;

        [FieldOffset(8)]
        public CASCODE cascode;

        [FieldOffset(8)]
        public CACY cacy;

        [FieldOffset(8)]
        public CADATE cadate;

        [FieldOffset(8)]
        public CAFILETIME cafiletime;

        [FieldOffset(8)]
        public CACLSID cauuid;

        [FieldOffset(8)]
        public CACLIPDATA caclipdata;

        [FieldOffset(8)]
        public CABSTR cabstr;

        [FieldOffset(8)]
        public CABSTRBLOB cabstrblob;

        [FieldOffset(8)]
        public CALPSTR calpstr;

        [FieldOffset(8)]
        public CALPWSTR calpwstr;

        [FieldOffset(8)]
        public CAPROPVARIANT capropvar;

        [FieldOffset(8)]
        [ComAliasName("CHAR")]
        public sbyte* pcVal;

        [FieldOffset(8)]
        [ComAliasName("UCHAR")]
        public byte* pbVal;

        [FieldOffset(8)]
        [ComAliasName("SHORT")]
        public short* piVal;

        [FieldOffset(8)]
        [ComAliasName("USHORT")]
        public ushort* puiVal;

        [FieldOffset(8)]
        [ComAliasName("LONG")]
        public int* plVal;

        [FieldOffset(8)]
        [ComAliasName("ULONG")]
        public uint* pulVal;

        [FieldOffset(8)]
        [ComAliasName("INT")]
        public int* pintVal;

        [FieldOffset(8)]
        [ComAliasName("UINT")]
        public uint* puintVal;

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
        public DECIMAL* pdecVal;

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
        [ComAliasName("LPSAFEARRAY")]
        public SAFEARRAY** pparray;

        [FieldOffset(8)]
        public PROPVARIANT* pvarVal;
        #endregion
        #endregion

        [FieldOffset(0)]
        public DECIMAL decVal;
        #endregion
    }
}
