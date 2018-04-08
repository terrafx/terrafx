// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct D3D12_QUERY_DATA_PIPELINE_STATISTICS
    {
        #region Fields
        [ComAliasName("UINT64")]
        public ulong IAVertices;

        [ComAliasName("UINT64")]
        public ulong IAPrimitives;

        [ComAliasName("UINT64")]
        public ulong VSInvocations;

        [ComAliasName("UINT64")]
        public ulong GSInvocations;

        [ComAliasName("UINT64")]
        public ulong GSPrimitives;

        [ComAliasName("UINT64")]
        public ulong CInvocations;

        [ComAliasName("UINT64")]
        public ulong CPrimitives;

        [ComAliasName("UINT64")]
        public ulong PSInvocations;

        [ComAliasName("UINT64")]
        public ulong HSInvocations;

        [ComAliasName("UINT64")]
        public ulong DSInvocations;

        [ComAliasName("UINT64")]
        public ulong CSInvocations;
        #endregion
    }
}
