// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="UnmanagedValuePool{T}" /> struct.</summary>
internal static unsafe class UnmanagedValuePoolTests
{
    /// <summary>Provides validation of the <see cref="UnmanagedValuePool{T}.UnmanagedValuePool()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        var pool = new UnmanagedValuePool<int>();

        Assert.That(pool.Capacity, Is.EqualTo((nuint)0));
        Assert.That(pool.Count, Is.EqualTo((nuint)0));
        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)0));
        Assert.That(pool.IsEmpty, Is.True);

        pool.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValuePool{T}.UnmanagedValuePool(nuint)" /> constructor.</summary>
    [Test]
    public static void CtorNUIntTest()
    {
        var pool = new UnmanagedValuePool<int>(4);

        Assert.That(pool.Capacity, Is.EqualTo((nuint)4));
        Assert.That(pool.Count, Is.EqualTo((nuint)0));

        pool.Dispose();
    }

    /// <summary>Provides validation that renting from an empty pool creates a new item.</summary>
    [Test]
    public static void RentCreatesWhenEmptyTest()
    {
        var pool = new UnmanagedValuePool<int>();

        _ = pool.Rent(&CreateItem);

        Assert.That(pool.Count, Is.EqualTo((nuint)1));
        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)0));

        pool.Dispose();
    }

    /// <summary>Provides validation that a returned item is reused by a subsequent rent.</summary>
    [Test]
    public static void RentReusesReturnedItemTest()
    {
        var pool = new UnmanagedValuePool<int>();

        var first = pool.Rent(&CreateItem);
        pool.Return(first);

        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)1));

        var second = pool.Rent(&CreateItem);

        Assert.That(second, Is.EqualTo(first));
        Assert.That(pool.Count, Is.EqualTo((nuint)1));
        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)0));

        pool.Dispose();
    }

    /// <summary>Provides validation that renting with an argument forwards it to the create function.</summary>
    [Test]
    public static void RentWithArgTest()
    {
        var pool = new UnmanagedValuePool<int>();

        var item = pool.Rent(&CreateItemFromArg, 42);

        Assert.That(item, Is.EqualTo(42));
        Assert.That(pool.Count, Is.EqualTo((nuint)1));

        pool.Dispose();
    }

    /// <summary>Provides validation that renting with a <c>null</c> create function is rejected.</summary>
    [Test]
    public static void RentNullCreateItemThrowsTest()
    {
        var pool = new UnmanagedValuePool<int>();

        Assert.That(() => pool.Rent((delegate*<int>)null),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("createItem")
        );

        pool.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValuePool.Return{T}(ref UnmanagedValuePool{T}, T)" /> method.</summary>
    [Test]
    public static void ReturnTest()
    {
        var pool = new UnmanagedValuePool<int>();

        pool.Return(5);

        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)1));

        pool.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValuePool.Remove{T}(ref UnmanagedValuePool{T}, T)" /> method.</summary>
    [Test]
    public static void RemoveTest()
    {
        var pool = new UnmanagedValuePool<int>();
        var item = pool.Rent(&CreateItem);

        Assert.That(pool.Remove(item), Is.True);
        Assert.That(pool.Count, Is.EqualTo((nuint)0));

        Assert.That(pool.Remove(item + 1), Is.False);

        pool.Dispose();
    }

    /// <summary>Provides validation that removing an item also removes it from the available items.</summary>
    [Test]
    public static void RemoveAlsoRemovesAvailableTest()
    {
        var pool = new UnmanagedValuePool<int>();

        var item = pool.Rent(&CreateItem);
        pool.Return(item);

        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)1));
        Assert.That(pool.Remove(item), Is.True);

        Assert.That(pool.Count, Is.EqualTo((nuint)0));
        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)0));

        pool.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValuePool.Clear{T}(ref UnmanagedValuePool{T})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        var pool = new UnmanagedValuePool<int>();

        var item = pool.Rent(&CreateItem);
        pool.Return(item);

        pool.Clear();

        Assert.That(pool.Count, Is.EqualTo((nuint)0));
        Assert.That(pool.AvailableCount, Is.EqualTo((nuint)0));
        Assert.That(pool.IsEmpty, Is.True);

        pool.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValuePool{T}.AvailableItems" /> enumerator.</summary>
    [Test]
    public static void AvailableItemsTest()
    {
        var pool = new UnmanagedValuePool<int>();

        var first = pool.Rent(&CreateItem);
        var second = pool.Rent(&CreateItem);

        pool.Return(first);
        pool.Return(second);

        var count = 0;
        var enumerator = pool.AvailableItems;

        while (enumerator.MoveNext())
        {
            count++;
        }

        Assert.That(count, Is.EqualTo(2));

        pool.Dispose();
    }

    /// <summary>Provides validation that the pool enumerates the items it contains.</summary>
    [Test]
    public static void GetEnumeratorTest()
    {
        var pool = new UnmanagedValuePool<int>();

        _ = pool.Rent(&CreateItem);
        _ = pool.Rent(&CreateItem);

        var count = 0;

        foreach (var item in pool)
        {
            count++;
        }

        Assert.That(count, Is.EqualTo(2));

        pool.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValuePool{T}.op_Equality" /> and <see cref="UnmanagedValuePool{T}.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = new UnmanagedValuePool<int>();
        var right = new UnmanagedValuePool<int>();

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);

        _ = left.Rent(&CreateItem);

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);

        left.Dispose();
        right.Dispose();
    }

    private static int s_nextValue;

    private static int CreateItem() => ++s_nextValue;

    private static int CreateItemFromArg(int arg) => arg;
}
