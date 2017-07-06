// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public  /* blittable */ struct TYPEATTR
    {
        #region Fields
        public GUID guid;

        public LCID lcid;

        public DWORD dwReserved;

        public MEMBERID memidConstructor;

        public MEMBERID memidDestructor;

        public LPOLESTR lpstrSchema;

        public ULONG cbSizeInstance;

        public TYPEKIND typekind;

        public WORD cFuncs;

        public WORD cVars;

        public WORD cImplTypes;

        public WORD cbSizeVft;

        public WORD cbAlignment;

        public WORD wTypeFlags;

        public WORD wMajorVerNum;

        public WORD wMinorVerNum;

        public TYPEDESC tdescAlias;

        public IDLDESC idldescType;
        #endregion
    }
}
