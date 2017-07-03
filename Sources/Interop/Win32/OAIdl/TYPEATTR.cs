// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public struct TYPEATTR
    {
        #region Fields
        public Guid guid;

        public LCID lcid;

        public uint dwReserved;

        public MEMBERID memidConstructor;

        public MEMBERID memidDestructor;

        public LPOLESTR lpstrSchema;

        public uint cbSizeInstance;

        public TYPEKIND typekind;

        public ushort cFuncs;


        public ushort cVars;

        public ushort cImplTypes;

        public ushort cbSizeVft;

        public ushort cbAlignment;

        public ushort wTypeFlags;

        public ushort wMajorVerNum;

        public ushort wMinorVerNum;

        public TYPEDESC tdescAlias;

        public IDLDESC idldescType;
        #endregion
    }
}
