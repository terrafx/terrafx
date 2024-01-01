// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the LinkedList<T>.Node class from https://github.com/dotnet/runtime/
// The original code is Copyright © .NET Foundation and Contributors. All rights reserved. Licensed under the MIT License (MIT).

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueLinkedListNode{T}" /> struct.</summary>
public static unsafe class UnmanagedValueLinkedListNode
{
    internal static void Invalidate<T>(this ref UnmanagedValueLinkedListNode<T> node)
        where T : unmanaged
    {
        node._next = null;
        node._previous = null;
        node._isFirstNode = false;
    }
}
