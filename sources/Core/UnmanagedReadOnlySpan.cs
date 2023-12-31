// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;

namespace TerraFX;

/// <summary>Provides functionality for the <see cref="UnmanagedReadOnlySpan{T}" /> struct.</summary>
public static unsafe class UnmanagedReadOnlySpan
{
    /// <summary>Gets an empty span.</summary>
    public static UnmanagedReadOnlySpan<T> Empty<T>()
        where T : unmanaged => new UnmanagedReadOnlySpan<T>();

    /// <inheritdoc cref="UnmanagedSpan.AsSpan{T}(UnmanagedSpan{T})" />
    public static ReadOnlySpan<T> AsSpan<T>(this UnmanagedReadOnlySpan<T> span)
        where T : unmanaged => span._value.AsSpan();

    /// <inheritdoc cref="UnmanagedSpan.AsSpan{T}(UnmanagedSpan{T}, nuint)" />
    public static ReadOnlySpan<T> AsSpan<T>(this UnmanagedReadOnlySpan<T> span, nuint start)
        where T : unmanaged => span._value.AsSpan(start);

    /// <inheritdoc cref="UnmanagedSpan.AsSpan{T}(UnmanagedSpan{T}, nuint, nuint)" />
    public static ReadOnlySpan<T> AsSpan<T>(this UnmanagedReadOnlySpan<T> span, nuint start, nuint length)
        where T : unmanaged => span._value.AsSpan(start, length);

    /// <inheritdoc cref="UnmanagedSpan.CopyTo{T}(UnmanagedSpan{T}, UnmanagedArray{T})" />
    public static void CopyTo<T>(this UnmanagedReadOnlySpan<T> span, UnmanagedArray<T> destination)
        where T : unmanaged => span._value.CopyTo(destination);

    /// <inheritdoc cref="UnmanagedSpan.CopyTo{T, TData}(UnmanagedSpan{T}, UnmanagedArray{T, TData})" />
    public static void CopyTo<T, TData>(this UnmanagedReadOnlySpan<T> span, UnmanagedArray<T, TData> destination)
        where T : unmanaged
        where TData : unmanaged => span._value.CopyTo(destination);

    /// <inheritdoc cref="UnmanagedSpan.CopyTo{T}(UnmanagedSpan{T}, UnmanagedSpan{T})" />
    public static void CopyTo<T>(this UnmanagedReadOnlySpan<T> span, UnmanagedSpan<T> destination)
        where T : unmanaged => span._value.CopyTo(destination);

    /// <inheritdoc cref="UnmanagedSpan.GetPointer{T}(UnmanagedSpan{T}, nuint)" />
    public static T* GetPointer<T>(this UnmanagedReadOnlySpan<T> span, nuint index = 0)
        where T : unmanaged => span._value.GetPointer(index);

    /// <inheritdoc cref="UnmanagedSpan.GetPointerUnsafe{T}(UnmanagedSpan{T}, nuint)" />
    public static T* GetPointerUnsafe<T>(this UnmanagedReadOnlySpan<T> span, nuint index = 0)
        where T : unmanaged => span._value.GetPointerUnsafe(index);

    /// <inheritdoc cref="UnmanagedSpan.GetReference{T}(UnmanagedSpan{T}, nuint)" />
    public static ref readonly T GetReference<T>(this UnmanagedReadOnlySpan<T> span, nuint index = 0)
        where T : unmanaged => ref span._value.GetReference(index);

    /// <inheritdoc cref="UnmanagedSpan.GetReferenceUnsafe{T}(UnmanagedSpan{T}, nuint)" />
    public static ref readonly T GetReferenceUnsafe<T>(this UnmanagedReadOnlySpan<T> span, nuint index = 0)
        where T : unmanaged => ref span._value.GetReferenceUnsafe(index);

    /// <inheritdoc cref="UnmanagedSpan.Slice{T}(UnmanagedSpan{T}, nuint)" />
    public static UnmanagedReadOnlySpan<T> Slice<T>(this UnmanagedReadOnlySpan<T> span, nuint start)
        where T : unmanaged => span._value.Slice(start);

    /// <inheritdoc cref="UnmanagedSpan.Slice{T}(UnmanagedSpan{T}, nuint, nuint)" />
    public static UnmanagedReadOnlySpan<T> Slice<T>(this UnmanagedReadOnlySpan<T> span, nuint start, nuint length)
        where T : unmanaged => span._value.Slice(start, length);
}
