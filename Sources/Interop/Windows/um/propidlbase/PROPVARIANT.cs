// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\propidlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    unsafe public /* blittable */ struct PROPVARIANT
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        public VARTYPE vt;

        [FieldOffset(2)]
        public byte wReserved1;

        [FieldOffset(3)]
        public byte wReserved2;

        [FieldOffset(4)]
        public uint wReserved3;

        #region union
        [FieldOffset(8)]
        public CHAR cVal;

        [FieldOffset(8)]
        public UCHAR bVal;

        [FieldOffset(8)]
        public SHORT iVal;

        [FieldOffset(8)]
        public USHORT uiVal;

        [FieldOffset(8)]
        public LONG lVal;

        [FieldOffset(8)]
        public ULONG ulVal;

        [FieldOffset(8)]
        public INT intVal;

        [FieldOffset(8)]
        public UINT uintVal;

        [FieldOffset(8)]
        public LARGE_INTEGER hVal;

        [FieldOffset(8)]
        public ULARGE_INTEGER uhVal;

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
        public FILETIME filetime;

        [FieldOffset(8)]
        public CLSID* puuid;

        [FieldOffset(8)]
        public CLIPDATA* pclipdata;

        [FieldOffset(8)]
        public BSTR bstrVal;

        [FieldOffset(8)]
        public BSTRBLOB bstrblobVal;

        [FieldOffset(8)]
        public BLOB blob;

        [FieldOffset(8)]
        public LPSTR pszVal;

        [FieldOffset(8)]
        public LPWSTR pwszVal;

        [FieldOffset(8)]
        public IUnknown* punkVal;

        [FieldOffset(8)]
        public IDispatch* pdispVal;

        [FieldOffset(8)]
        public IStream* pStream;

        [FieldOffset(8)]
        public IStorage* pStorage;

        [FieldOffset(8)]
        public LPVERSIONEDSTREAM pVersionedStream;

        [FieldOffset(8)]
        public LPSAFEARRAY parray;

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
        public CHAR* pcVal;

        [FieldOffset(8)]
        public UCHAR* pbVal;

        [FieldOffset(8)]
        public SHORT* piVal;

        [FieldOffset(8)]
        public USHORT* puiVal;

        [FieldOffset(8)]
        public LONG* plVal;

        [FieldOffset(8)]
        public ULONG* pulVal;

        [FieldOffset(8)]
        public INT* pintVal;

        [FieldOffset(8)]
        public UINT* puintVal;

        [FieldOffset(8)]
        public FLOAT* pfltVal;

        [FieldOffset(8)]
        public DOUBLE* pdblVal;

        [FieldOffset(8)]
        public VARIANT_BOOL* pboolVal;

        [FieldOffset(8)]
        public DECIMAL* pdecVal;

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
        public LPSAFEARRAY* pparray;

        [FieldOffset(8)]
        public PROPVARIANT* pvarVal;
        #endregion
        #endregion

        [FieldOffset(0)]
        public DECIMAL decVal;
        #endregion
    }
}
