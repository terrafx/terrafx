// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX;

/// <summary>Provides functionality for the <see cref="UnmanagedSpan{T}" /> struct.</summary>
public static class UnmanagedSpan
{
    /// <summary>Gets an empty span.</summary>
    public static UnmanagedSpan<T> Empty<T>()
        where T : unmanaged => new UnmanagedSpan<T>();
}
