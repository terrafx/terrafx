// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueStack{T}" /> struct.</summary>
public static class ValueStack
{
    /// <summary>Gets an empty stack.</summary>
    public static ValueStack<T> Empty<T>() => new ValueStack<T>();
}
