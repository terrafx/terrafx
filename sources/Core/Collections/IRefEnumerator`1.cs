// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;

namespace TerraFX.Collections;

/// <summary>Provides methods for iteration by reference.</summary>
/// <typeparam name="T">The type of items being iterated.</typeparam>
public interface IRefEnumerator<T> : IEnumerator<T>
{
    /// <summary>Gets a readonly reference to the item at the current position of the enumerator.</summary>
    ref readonly T CurrentRef { get; }
}
