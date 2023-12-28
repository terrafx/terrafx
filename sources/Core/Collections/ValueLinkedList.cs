// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValueLinkedList{T}" /> struct.</summary>
public static class ValueLinkedList
{
    /// <summary>Gets an empty linked list.</summary>
    public static ValueLinkedList<T> Empty<T>() => new ValueLinkedList<T>();
}
