// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValueStack{T}" /> struct.</summary>
public static class UnmanagedValueStack
{
    /// <summary>Gets an empty stack.</summary>
    public static UnmanagedValueStack<T> Empty<T>()
        where T : unmanaged => new UnmanagedValueStack<T>();
}
