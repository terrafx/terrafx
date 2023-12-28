// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="UnmanagedValuePool{T}" /> struct.</summary>
public static class UnmanagedValuePool
{
    /// <summary>Gets an empty pool.</summary>
    public static UnmanagedValuePool<T> Empty<T>()
        where T : unmanaged => new UnmanagedValuePool<T>();
}
