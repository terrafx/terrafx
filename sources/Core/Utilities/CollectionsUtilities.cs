// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Collections;

namespace TerraFX.Utilities;

/// <summary>Provides a set of methods to supplement the collections namespace.</summary>
public static unsafe class CollectionsUtilities
{
    /// <summary>Disposes the items in an array.</summary>
    /// <typeparam name="TDisposable">The type of the items in the array.</typeparam>
    /// <param name="array">The array whose items are to be disposed.</param>
    public static void Dispose<TDisposable>(this TDisposable[] array)
        where TDisposable : IDisposable => array.AsSpan().Dispose();

    /// <summary>Disposes the items in a span.</summary>
    /// <typeparam name="TDisposable">The type of the items in the span.</typeparam>
    /// <param name="span">The span whose items are to be disposed.</param>
    public static void Dispose<TDisposable>(this Span<TDisposable> span)
        where TDisposable : IDisposable
    {
        for (var index = span.Length - 1; index >= 0; index--)
        {
            var item = span.GetReferenceUnsafe(index);
            item.Dispose();
        }
        span.Clear();
    }

    /// <summary>Disposes the items in a list.</summary>
    /// <typeparam name="TDisposable">The type of the items in the list.</typeparam>
    /// <param name="list">The list whose items are to be disposed.</param>
    public static void Dispose<TDisposable>(ref this ValueList<TDisposable> list)
        where TDisposable : IDisposable
    {
        for (var index = list.Count - 1; index >= 0; index--)
        {
            var item = list.GetReferenceUnsafe(index);
            item.Dispose();
        }
        list.Clear();
    }

    /// <summary>Disposes the items in a pool.</summary>
    /// <typeparam name="TDisposable">The type of the items in the pool.</typeparam>
    /// <param name="pool">The pool whose items are to be disposed.</param>
    public static void Dispose<TDisposable>(ref this ValuePool<TDisposable> pool)
        where TDisposable : IDisposable
    {
        for (var index = pool.Count - 1; index >= 0; index--)
        {
            var item = pool.GetReferenceUnsafe(index);
            item.Dispose();
        }
        pool.Clear();
    }

    /// <summary>Disposes the items in a list.</summary>
    /// <typeparam name="TDisposable">The type of the items in the list.</typeparam>
    /// <param name="list">The list whose items are to be disposed.</param>
    public static void Dispose<TDisposable>(ref this UnmanagedValueList<TDisposable> list)
        where TDisposable : unmanaged, IDisposable
    {
        for (var index = list.Count - 1; index >= 0; index--)
        {
            var item = list.GetReferenceUnsafe(index);
            item.Dispose();
        }
        list.Clear();
    }

    /// <summary>Disposes the items in a pool.</summary>
    /// <typeparam name="TDisposable">The type of the items in the pool.</typeparam>
    /// <param name="pool">The pool whose items are to be disposed.</param>
    public static void Dispose<TDisposable>(ref this UnmanagedValuePool<TDisposable> pool)
        where TDisposable : unmanaged, IDisposable
    {
        for (var index = pool.Count - 1; index >= 0; index--)
        {
            var item = pool.GetReferenceUnsafe(index);
            item.Dispose();
        }
        pool.Clear();
    }
}
