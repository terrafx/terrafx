// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum INVOKEKIND
    {
        FUNC = 1,

        PROPERTYGET = 2,

        PROPERTYPUT = 4,

        PROPERTYPUTREF = 8
    }
}
