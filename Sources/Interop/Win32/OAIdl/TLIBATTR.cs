// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    public struct TLIBATTR
    {
        #region Fields
        public Guid guid;

        public LCID lcid;

        public SYSKIND syskind;

        public ushort wMajorVerNum;

        public ushort wMinorVerNum;

        public ushort wLibFlags;
        #endregion
    }
}
