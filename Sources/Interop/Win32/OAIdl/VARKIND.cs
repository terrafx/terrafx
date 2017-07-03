// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\OAIdl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum VARKIND
    {
        PERINSTANCE = 0,

        STATIC = (PERINSTANCE + 1),

        CONST = (STATIC + 1),

        DISPATCH = (CONST + 1)
    }
}
