// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct D3D12_INFO_QUEUE_FILTER_DESC
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint NumCategories;

        public D3D12_MESSAGE_CATEGORY* pCategoryList;

        [ComAliasName("UINT")]
        public uint NumSeverities;

        public D3D12_MESSAGE_SEVERITY* pSeverityList;

        [ComAliasName("UINT")]
        public uint NumIDs;

        public D3D12_MESSAGE_ID* pIDList;
        #endregion
    }
}
