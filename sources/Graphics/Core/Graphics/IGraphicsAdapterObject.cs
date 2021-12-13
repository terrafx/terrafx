// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics adapter.</summary>
public interface IGraphicsAdapterObject : IGraphicsServiceObject
{
    /// <summary>Gets the adapter for which the object was created.</summary>
    GraphicsAdapter Adapter { get; }
}
