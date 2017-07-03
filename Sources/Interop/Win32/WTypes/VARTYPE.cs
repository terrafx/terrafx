// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from shared\wtypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
#pragma warning disable CS1591
    public enum VARTYPE : ushort
    {
        EMPTY = 0,

        NULL = 1,

        I2 = 2,

        I4 = 3,

        R4 = 4,

        R8 = 5,

        CY = 6,

        DATE = 7,

        BSTR = 8,

        DISPATCH = 9,

        ERROR = 10,

        BOOL = 11,

        VARIANT = 12,

        UNKNOWN = 13,

        DECIMAL = 14,

        // VBA reserves 15 for future use

        I1 = 16,

        UI1 = 17,

        UI2 = 18,

        UI4 = 19,

        I8 = 20,

        UI8 = 21,

        INT = 22,

        UINT = 23,

        VOID = 24,

        HRESULT = 25,

        PTR = 26,

        SAFEARRAY = 27,

        CARRAY = 28,

        USERDEFINED = 29,

        LPSTR = 30,

        LPWSTR = 31,

        // VBA reserves 32-35 for future use

        RECORD = 36,

        INT_PTR = 37,

        UINT_PTR = 38,

        FILETIME = 64,

        BLOB = 65,

        STREAM = 66,

        STORAGE = 67,

        STREAMED_OBJECT = 68,

        STORED_OBJECT = 69,

        BLOB_OBJECT = 70,

        CF = 71,

        CLSID = 72,

        VERSIONED_STREAM = 73,

        BSTR_BLOB = 0x0FFF,

        VECTOR = 0x1000,

        ARRAY = 0x2000,

        BYREF = 0x4000,

        RESERVED = 0x8000,

        ILLEGAL = 0xFFFF,

        ILLEGALMASKED = 0x0FFF,

        TYPEMASK = 0x0FFF
    }
#pragma warning restore CS1591
}
