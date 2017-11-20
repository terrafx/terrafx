// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections
{
    /// <summary>Defines a <see cref="string" />-<see cref="object" /> dictionary that provides notifications when its contents are changed.</summary>
    public interface IPropertySet : IObservableDictionary<string, object>
    {
    }
}
