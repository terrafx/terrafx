// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public struct FUNCDESC
    {
        #region Fields
        public MEMBERID memid;

        public SCODE* lprgscode;

        public ELEMDESC* lprgelemdescParam;

        public FUNCKIND funckind;

        public INVOKEKIND invkind;

        public CALLCONV callconv;

        public short cParams;

        public short cParamsOpt;

        public short oVft;

        public short cScodes;

        public ELEMDESC elemdescFunc;

        public ushort wFuncFlags;
        #endregion
    }
}
