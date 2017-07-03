// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum TYPEKIND
    {
        ENUM = 0,

        RECORD = (ENUM + 1),

        MODULE = (RECORD + 1),

        INTERFACE = (MODULE + 1),

        DISPATCH = (INTERFACE + 1),

        COCLASS = (DISPATCH + 1),

        ALIAS = (COCLASS + 1),

        UNION = (ALIAS + 1),

        MAX = (UNION + 1)
    }
}
