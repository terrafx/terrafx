// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct FUNCDESC
    {
        #region Fields
        [ComAliasName("MEMBERID")]
        public int memid;

        [ComAliasName("SCODE")]
        public int* lprgscode;

        public ELEMDESC* lprgelemdescParam;

        public FUNCKIND funckind;

        public INVOKEKIND invkind;

        public CALLCONV callconv;

        [ComAliasName("SHORT")]
        public short cParams;

        [ComAliasName("SHORT")]
        public short cParamsOpt;

        [ComAliasName("SHORT")]
        public short oVft;

        [ComAliasName("SHORT")]
        public short cScodes;

        public ELEMDESC elemdescFunc;

        [ComAliasName("WORD")]
        public ushort wFuncFlags;
        #endregion
    }
}
