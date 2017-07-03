// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    unsafe public struct DXGI_INFO_QUEUE_MESSAGE
    {
        #region Fields
        public DXGI_DEBUG_ID Producer;

        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category;

        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity;

        public DXGI_INFO_QUEUE_MESSAGE_ID ID;

        public /* const */ CHAR* pDescription;

        public nuint DescriptionByteLength;
        #endregion
    }
}
