// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_QUERY_DATA_PIPELINE_STATISTICS
    {
        #region Fields
        public UINT64 IAVertices;

        public UINT64 IAPrimitives;

        public UINT64 VSInvocations;

        public UINT64 GSInvocations;

        public UINT64 GSPrimitives;

        public UINT64 CInvocations;

        public UINT64 CPrimitives;

        public UINT64 PSInvocations;

        public UINT64 HSInvocations;

        public UINT64 DSInvocations;

        public UINT64 CSInvocations;
        #endregion
    }
}
