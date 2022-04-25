// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.UI.Advanced;

/// <summary>An object which is created for a UI service.</summary>
public abstract class UIServiceObject : DisposableObject
{
    private readonly UIService _service;

    private protected UIServiceObject(UIService service, string? name = null) : base(name)
    {
        AssertNotNull(service);
        _service = service;
    }

    /// <summary>Gets the service for which the object was created.</summary>
    public UIService Service => _service;
}
