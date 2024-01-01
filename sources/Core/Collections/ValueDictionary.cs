// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
public static class ValueDictionary
{
    /// <summary>Gets an empty dictionary.</summary>
    public static ValueDictionary<TKey, TValue> Empty<TKey, TValue>()
        where TKey : notnull => new ValueDictionary<TKey, TValue>();
}