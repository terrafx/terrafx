// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueQueue{T}" /> struct.</summary>
public static class ValueQueue
{
    /// <summary>Gets an empty queue.</summary>
    public static ValueQueue<T> Empty<T>() => new ValueQueue<T>();
}
