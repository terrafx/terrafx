// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct DXGI_INFO_QUEUE_FILTER_DESC
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint NumCategories;

        [ComAliasName("DXGI_INFO_QUEUE_MESSAGE_CATEGORY[]")]
        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY* pCategoryList;

        [ComAliasName("UINT")]
        public uint NumSeverities;

        [ComAliasName("DXGI_INFO_QUEUE_MESSAGE_SEVERITY[]")]
        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY* pSeverityList;

        [ComAliasName("UINT")]
        public uint NumIDs;

        [ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID[]")]
        public int* pIDList;
        #endregion
    }
}
