// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueList{T}" /> struct.</summary>
public static class ValueList
{
    /// <summary>Gets an empty list.</summary>
    public static ValueList<T> Empty<T>() => new ValueList<T>();
}
