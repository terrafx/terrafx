// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

namespace TerraFX.Collections;

/// <summary>Provides functionality for the <see cref="ValuePool{T}" /> struct.</summary>
public static class ValuePool
{
    /// <summary>Gets an empty pool.</summary>
    public static ValuePool<T> Empty<T>() => new ValuePool<T>();
}
