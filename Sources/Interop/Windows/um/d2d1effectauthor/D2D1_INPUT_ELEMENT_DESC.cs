// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d2d1effectauthor.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    /// <summary>This defines a single element of the vertex layout.</summary>
    public /* blittable */ struct D2D1_INPUT_ELEMENT_DESC
    {
        #region Fields
        public PCSTR semanticName;

        public UINT32 semanticIndex;

        public DXGI_FORMAT format;

        public UINT32 inputSlot;

        public UINT32 alignedByteOffset;
        #endregion
    }
}
