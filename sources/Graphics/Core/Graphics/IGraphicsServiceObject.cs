// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.Graphics;

/// <summary>An object which is created for a graphics service.</summary>
public interface IGraphicsServiceObject : IDisposable
{
    /// <summary>Gets the service for which the object was created.</summary>
    GraphicsService Service { get; }
}
