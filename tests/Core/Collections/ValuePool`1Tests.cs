// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValuePool{T}" /> struct.</summary>
internal static unsafe class ValuePoolTests
{
    /// <summary>Provides validation of the <see cref="ValuePool{T}.ValuePool()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        Assert.That(() => new ValuePool<Item>(),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
               .And.Property("AvailableItemCount").EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValuePool{T}.ValuePool(int)" /> constructor.</summary>
    [Test]
    public static void CtorInt32Test()
    {
        Assert.That(() => new ValuePool<Item>(4),
            Has.Property("Capacity").EqualTo(4)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValuePool<Item>(-1),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ParamName").EqualTo("capacity")
        );
    }

    /// <summary>Provides validation that renting from an empty pool creates a new item.</summary>
    [Test]
    public static void RentCreatesWhenEmptyTest()
    {
        ValuePool<Item> pool = [];

        var item = pool.Rent(&CreateItem);

        Assert.That(item, Is.Not.Null);
        Assert.That(pool.Count, Is.EqualTo(1));
        Assert.That(pool.AvailableItemCount, Is.EqualTo(0));
    }

    /// <summary>Provides validation that a returned item is reused by a subsequent rent.</summary>
    [Test]
    public static void RentReusesReturnedItemTest()
    {
        ValuePool<Item> pool = [];

        var first = pool.Rent(&CreateItem);
        pool.Return(first);

        Assert.That(pool.AvailableItemCount, Is.EqualTo(1));

        var second = pool.Rent(&CreateItem);

        Assert.That(second, Is.SameAs(first));
        Assert.That(pool.Count, Is.EqualTo(1));
        Assert.That(pool.AvailableItemCount, Is.EqualTo(0));
    }

    /// <summary>Provides validation that renting with an argument forwards it to the create function.</summary>
    [Test]
    public static void RentWithArgTest()
    {
        ValuePool<Item> pool = [];

        var item = pool.Rent(&CreateItemFromArg, 42);

        Assert.That(item.Value, Is.EqualTo(42));
        Assert.That(pool.Count, Is.EqualTo(1));
    }

    /// <summary>Provides validation that renting with a <c>null</c> create function is rejected.</summary>
    [Test]
    public static void RentNullCreateItemThrowsTest()
    {
        ValuePool<Item> pool = [];

        Assert.That(() => pool.Rent((delegate*<Item>)null),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("createItem")
        );
    }

    /// <summary>Provides validation of the <see cref="ValuePool.Return{T}(ref ValuePool{T}, T)" /> method.</summary>
    [Test]
    public static void ReturnTest()
    {
        ValuePool<Item> pool = [];

        pool.Return(new());
        Assert.That(pool.AvailableItemCount, Is.EqualTo(1));

        Assert.That(() => pool.Return(null!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("item")
        );
    }

    /// <summary>Provides validation of the <see cref="ValuePool.Remove{T}(ref ValuePool{T}, T)" /> method.</summary>
    [Test]
    public static void RemoveTest()
    {
        ValuePool<Item> pool = [];
        var item = pool.Rent(&CreateItem);

        Assert.That(pool.Remove(item), Is.True);
        Assert.That(pool.Count, Is.EqualTo(0));

        Assert.That(pool.Remove(new()), Is.False);
    }

    /// <summary>Provides validation that removing an item also removes it from the available items.</summary>
    [Test]
    public static void RemoveAlsoRemovesAvailableTest()
    {
        ValuePool<Item> pool = [];

        var item = pool.Rent(&CreateItem);
        pool.Return(item);

        Assert.That(pool.AvailableItemCount, Is.EqualTo(1));
        Assert.That(pool.Remove(item), Is.True);

        Assert.That(pool.Count, Is.EqualTo(0));
        Assert.That(pool.AvailableItemCount, Is.EqualTo(0));
    }

    /// <summary>Provides validation of the <see cref="ValuePool.Clear{T}(ref ValuePool{T})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        ValuePool<Item> pool = [];

        var item = pool.Rent(&CreateItem);
        pool.Return(item);

        pool.Clear();

        Assert.That(pool.Count, Is.EqualTo(0));
        Assert.That(pool.AvailableItemCount, Is.EqualTo(0));
        Assert.That(pool.IsEmpty, Is.True);
    }

    /// <summary>Provides validation of the <see cref="ValuePool{T}.AvailableItems" /> enumerator.</summary>
    [Test]
    public static void AvailableItemsTest()
    {
        ValuePool<Item> pool = [];

        var first = pool.Rent(&CreateItem);
        var second = pool.Rent(&CreateItem);

        pool.Return(first);
        pool.Return(second);

        var count = 0;
        var enumerator = pool.AvailableItems;

        while (enumerator.MoveNext())
        {
            Assert.That(enumerator.Current, Is.Not.Null);
            count++;
        }

        Assert.That(count, Is.EqualTo(2));
    }

    /// <summary>Provides validation that the pool enumerates the items it contains.</summary>
    [Test]
    public static void GetEnumeratorTest()
    {
        ValuePool<Item> pool = [];

        _ = pool.Rent(&CreateItem);
        _ = pool.Rent(&CreateItem);

        var count = 0;

        foreach (var item in pool)
        {
            Assert.That(item, Is.Not.Null);
            count++;
        }

        Assert.That(count, Is.EqualTo(2));
    }

    /// <summary>Provides validation of the <see cref="ValuePool{T}.op_Equality" /> and <see cref="ValuePool{T}.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        ValuePool<Item> left = [];
        ValuePool<Item> right = [];

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);

        _ = left.Rent(&CreateItem);

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
    }

    private static Item CreateItem() => new();

    private static Item CreateItemFromArg(int arg) => new() { Value = arg };

    private sealed class Item
    {
        public int Value;
    }
}
