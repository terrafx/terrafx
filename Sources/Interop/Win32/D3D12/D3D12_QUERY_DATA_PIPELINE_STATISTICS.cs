// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_QUERY_DATA_PIPELINE_STATISTICS
    {
        #region Fields
        public ulong IAVertices;

        public ulong IAPrimitives;

        public ulong VSInvocations;

        public ulong GSInvocations;

        public ulong GSPrimitives;

        public ulong CInvocations;

        public ulong CPrimitives;

        public ulong PSInvocations;

        public ulong HSInvocations;

        public ulong DSInvocations;

        public ulong CSInvocations;
        #endregion
    }
}
