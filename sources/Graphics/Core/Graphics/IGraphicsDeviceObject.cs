// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics device.</summary>
public interface IGraphicsDeviceObject : IGraphicsAdapterObject
{
    /// <summary>Gets the device for which the object was created.</summary>
    GraphicsDevice Device { get; }
}
