// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct DXGI_INFO_QUEUE_MESSAGE
    {
        #region Fields
        [ComAliasName("DXGI_DEBUG_ID")]
        public Guid Producer;

        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category;

        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity;

        [ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")]
        public int ID;

        [ComAliasName("CHAR")]
        public sbyte* pDescription;

        [ComAliasName("SIZE_T")]
        public nuint DescriptionByteLength;
        #endregion
    }
}
