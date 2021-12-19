// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX.UI;

/// <summary>An object which is created for a UI service.</summary>
public interface IUIServiceObject : IDisposable
{
    /// <summary>Gets the service for which the object was created.</summary>
    UIService Service { get; }
}
