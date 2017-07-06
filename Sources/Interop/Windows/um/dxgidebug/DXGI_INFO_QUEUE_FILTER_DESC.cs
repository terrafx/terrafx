// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct DXGI_INFO_QUEUE_FILTER_DESC
    {
        #region Fields
        public UINT NumCategories;

        public DXGI_INFO_QUEUE_MESSAGE_CATEGORY* pCategoryList;

        public UINT NumSeverities;

        public DXGI_INFO_QUEUE_MESSAGE_SEVERITY* pSeverityList;

        public UINT NumIDs;

        public DXGI_INFO_QUEUE_MESSAGE_ID* pIDList;
        #endregion
    }
}
