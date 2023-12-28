// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX;

/// <summary>Provides functionality for the <see cref="UnmanagedArray{T}" /> struct.</summary>
public static class UnmanagedArray
{
    /// <summary>Gets an empty array.</summary>
    public static UnmanagedArray<T> Empty<T>()
        where T : unmanaged => UnmanagedArray<T>.s_empty;
}
