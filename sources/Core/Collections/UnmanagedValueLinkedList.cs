// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LinkedList<T> class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueLinkedList{T}" /> struct.</summary>
public static class UnmanagedValueLinkedList
{
    /// <summary>Gets an empty linked list.</summary>
    public static UnmanagedValueLinkedList<T> Empty<T>()
        where T : unmanaged => new UnmanagedValueLinkedList<T>();
}