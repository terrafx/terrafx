// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="UnmanagedValueDictionary{TKey, TValue}" /> struct.</summary>
internal static class UnmanagedValueDictionaryTests
{
    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary{TKey, TValue}.UnmanagedValueDictionary()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];

        Assert.That(() => dictionary,
            Has.Property("Capacity").EqualTo((nuint)0)
               .And.Count.EqualTo((nuint)0)
        );

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary{TKey, TValue}.UnmanagedValueDictionary(nuint)" /> constructor.</summary>
    [Test]
    public static void CtorNUIntTest()
    {
        var dictionary = new UnmanagedValueDictionary<int, int>(7);

        Assert.That(() => dictionary,
            Has.Property("Capacity").GreaterThanOrEqualTo((nuint)7)
               .And.Count.EqualTo((nuint)0)
        );

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary{TKey, TValue}" /> indexer.</summary>
    [Test]
    public static void IndexerTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];

        dictionary[1] = 10;
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary.Count, Is.EqualTo((nuint)1));

        // Setting an existing key overwrites the value rather than adding a new entry.
        dictionary[1] = 100;
        Assert.That(dictionary[1], Is.EqualTo(100));
        Assert.That(dictionary.Count, Is.EqualTo((nuint)1));

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.Add{TKey, TValue}(ref UnmanagedValueDictionary{TKey, TValue}, TKey, TValue)" /> method.</summary>
    [Test]
    public static void AddTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];

        dictionary.Add(1, 10);
        dictionary.Add(2, 20);

        Assert.That(dictionary.Count, Is.EqualTo((nuint)2));
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary[2], Is.EqualTo(20));

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.ContainsKey{TKey, TValue}(ref readonly UnmanagedValueDictionary{TKey, TValue}, TKey)" /> method.</summary>
    [Test]
    public static void ContainsKeyTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.ContainsKey(1), Is.True);
        Assert.That(dictionary.ContainsKey(2), Is.False);

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.ContainsValue{TKey, TValue}(ref readonly UnmanagedValueDictionary{TKey, TValue}, TValue)" /> method.</summary>
    [Test]
    public static void ContainsValueTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.ContainsValue(10), Is.True);
        Assert.That(dictionary.ContainsValue(20), Is.False);

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.TryGetValue{TKey, TValue}(ref readonly UnmanagedValueDictionary{TKey, TValue}, TKey, out TValue)" /> method.</summary>
    [Test]
    public static void TryGetValueTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.TryGetValue(1, out var value), Is.True);
        Assert.That(value, Is.EqualTo(10));

        Assert.That(dictionary.TryGetValue(2, out var missing), Is.False);
        Assert.That(missing, Is.EqualTo(0));

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.TryAdd{TKey, TValue}(ref UnmanagedValueDictionary{TKey, TValue}, TKey, TValue)" /> method.</summary>
    [Test]
    public static void TryAddTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];

        Assert.That(dictionary.TryAdd(1, 10), Is.True);
        Assert.That(dictionary.Count, Is.EqualTo((nuint)1));

        // A duplicate key is rejected and the original value is left untouched.
        Assert.That(dictionary.TryAdd(1, 20), Is.False);
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary.Count, Is.EqualTo((nuint)1));

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.Remove{TKey, TValue}(ref UnmanagedValueDictionary{TKey, TValue}, TKey)" /> method.</summary>
    [Test]
    public static void RemoveTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);
        dictionary.Add(2, 20);

        Assert.That(dictionary.Remove(1), Is.True);
        Assert.That(dictionary.Count, Is.EqualTo((nuint)1));
        Assert.That(dictionary.ContainsKey(1), Is.False);

        Assert.That(dictionary.Remove(42), Is.False);
        Assert.That(dictionary.Count, Is.EqualTo((nuint)1));

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.Remove{TKey, TValue}(ref UnmanagedValueDictionary{TKey, TValue}, TKey, out TValue)" /> method.</summary>
    [Test]
    public static void RemoveOutValueTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.Remove(1, out var value), Is.True);
        Assert.That(value, Is.EqualTo(10));

        Assert.That(dictionary.Remove(1, out var missing), Is.False);
        Assert.That(missing, Is.EqualTo(0));

        dictionary.Dispose();
    }

    /// <summary>Provides validation that a removed slot is reused by a subsequent add.</summary>
    [Test]
    public static void RemoveThenAddReusesSlotTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);
        dictionary.Add(2, 20);
        dictionary.Add(3, 30);

        _ = dictionary.Remove(2);
        dictionary.Add(4, 40);

        Assert.That(dictionary.Count, Is.EqualTo((nuint)3));
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary[3], Is.EqualTo(30));
        Assert.That(dictionary[4], Is.EqualTo(40));
        Assert.That(dictionary.ContainsKey(2), Is.False);

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.Clear{TKey, TValue}(ref UnmanagedValueDictionary{TKey, TValue})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);
        dictionary.Add(2, 20);

        dictionary.Clear();

        Assert.That(dictionary.Count, Is.EqualTo((nuint)0));
        Assert.That(dictionary.IsEmpty, Is.True);
        Assert.That(dictionary.ContainsKey(1), Is.False);

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary.EnsureCapacity{TKey, TValue}(ref UnmanagedValueDictionary{TKey, TValue}, nuint)" /> method.</summary>
    [Test]
    public static void EnsureCapacityTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];

        dictionary.EnsureCapacity(32);

        Assert.That(dictionary.Capacity, Is.GreaterThanOrEqualTo((nuint)32));
        Assert.That(dictionary.Count, Is.EqualTo((nuint)0));

        dictionary.Dispose();
    }

    /// <summary>Provides validation that adding more items than the initial capacity grows the dictionary correctly.</summary>
    [Test]
    public static void GrowthTest()
    {
        UnmanagedValueDictionary<int, int> dictionary = [];

        for (var i = 0; i < 100; i++)
        {
            dictionary.Add(i, i * 10);
        }

        Assert.That(dictionary.Count, Is.EqualTo((nuint)100));

        for (var i = 0; i < 100; i++)
        {
            Assert.That(dictionary[i], Is.EqualTo(i * 10));
        }

        dictionary.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueDictionary{TKey, TValue}.op_Equality" /> and <see cref="UnmanagedValueDictionary{TKey, TValue}.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = new UnmanagedValueDictionary<int, int>();
        var alias = left;
        var other = new UnmanagedValueDictionary<int, int>(7);

        Assert.That(left == alias, Is.True);
        Assert.That(left != alias, Is.False);

        Assert.That(left == other, Is.False);
        Assert.That(left != other, Is.True);

        left.Dispose();
        other.Dispose();
    }
}
