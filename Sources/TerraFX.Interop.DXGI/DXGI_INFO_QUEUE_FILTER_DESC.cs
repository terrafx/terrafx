// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)] // Size = 24 or 48
    unsafe public struct DXGI_INFO_QUEUE_FILTER_DESC
    {
        #region Fields
        public uint NumCategories;

        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY* pCategoryList;

        public uint NumSeverities;

        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY* pSeverityList;

        public uint NumIDs;

        public DXGI_INFO_QUEUE_MESSAGE_ID* pIDList;
        #endregion
    }
}
