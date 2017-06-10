// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.DXGI
{
    unsafe public struct DXGI_INFO_QUEUE_MESSAGE
    {
        #region Fields
        public Guid Producer;

        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category;

        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity;

        public DXGI_INFO_QUEUE_MESSAGE_ID ID;

        public byte* pDescription;

        public UIntPtr DescriptionByteLength;
        #endregion
    }
}
