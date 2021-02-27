// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics
{
    /// <summary>Defines the kind of a graphics pipeline resource.</summary>
    public enum GraphicsPipelineResourceKind
    {
        /// <summary>Defines an unknown graphics pipeline resource kind.</summary>
        Unknown,

        /// <summary>Defines a constant buffer graphics pipeline resource.</summary>
        ConstantBuffer,

        /// <summary>Defines a texture graphics pipeline resource.</summary>
        Texture,
    }
}
