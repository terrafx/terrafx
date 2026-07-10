// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValueDictionary{TKey, TValue}" /> struct.</summary>
internal static class ValueDictionaryTests
{
    /// <summary>Provides validation of the <see cref="ValueDictionary{TKey, TValue}.ValueDictionary()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        Assert.That(() => new ValueDictionary<int, int>(),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary{TKey, TValue}.ValueDictionary(int)" /> constructor.</summary>
    [Test]
    public static void CtorInt32Test()
    {
        Assert.That(() => new ValueDictionary<int, int>(7),
            Has.Property("Capacity").GreaterThanOrEqualTo(7)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueDictionary<int, int>(-5),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-5)
                  .And.Property("ParamName").EqualTo("capacity")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary{TKey, TValue}.ValueDictionary(IEnumerable{KeyValuePair{TKey, TValue}})" /> constructor.</summary>
    [Test]
    public static void CtorIEnumerableTest()
    {
        var source = new Dictionary<int, int> {
            [1] = 10,
            [2] = 20,
            [3] = 30,
        };

        Assert.That(() => new ValueDictionary<int, int>(source),
            Has.Count.EqualTo(3)
        );

        Assert.That(() => new ValueDictionary<int, int>((null as IEnumerable<KeyValuePair<int, int>>)!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("source")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary{TKey, TValue}" /> indexer.</summary>
    [Test]
    public static void IndexerTest()
    {
        ValueDictionary<int, int> dictionary = [];

        dictionary[1] = 10;
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary.Count, Is.EqualTo(1));

        // Setting an existing key overwrites the value rather than adding a new entry.
        dictionary[1] = 100;
        Assert.That(dictionary[1], Is.EqualTo(100));
        Assert.That(dictionary.Count, Is.EqualTo(1));

        Assert.That(() => new ValueDictionary<int, int>()[42],
            Throws.InstanceOf<KeyNotFoundException>()
        );
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.Add{TKey, TValue}(ref ValueDictionary{TKey, TValue}, TKey, TValue)" /> method.</summary>
    [Test]
    public static void AddTest()
    {
        ValueDictionary<int, int> dictionary = [];

        dictionary.Add(1, 10);
        dictionary.Add(2, 20);

        Assert.That(dictionary.Count, Is.EqualTo(2));
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary[2], Is.EqualTo(20));

        var duplicate = dictionary;

        Assert.That(() => duplicate.Add(1, 30),
            Throws.ArgumentException
                  .And.Property("ParamName").EqualTo("key")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.ContainsKey{TKey, TValue}(ref readonly ValueDictionary{TKey, TValue}, TKey)" /> method.</summary>
    [Test]
    public static void ContainsKeyTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.ContainsKey(1), Is.True);
        Assert.That(dictionary.ContainsKey(2), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.ContainsValue{TKey, TValue}(ref readonly ValueDictionary{TKey, TValue}, TValue)" /> method.</summary>
    [Test]
    public static void ContainsValueTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.ContainsValue(10), Is.True);
        Assert.That(dictionary.ContainsValue(20), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.TryGetValue{TKey, TValue}(ref readonly ValueDictionary{TKey, TValue}, TKey, out TValue)" /> method.</summary>
    [Test]
    public static void TryGetValueTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.TryGetValue(1, out var value), Is.True);
        Assert.That(value, Is.EqualTo(10));

        Assert.That(dictionary.TryGetValue(2, out var missing), Is.False);
        Assert.That(missing, Is.EqualTo(0));
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.TryAdd{TKey, TValue}(ref ValueDictionary{TKey, TValue}, TKey, TValue)" /> method.</summary>
    [Test]
    public static void TryAddTest()
    {
        ValueDictionary<int, int> dictionary = [];

        Assert.That(dictionary.TryAdd(1, 10), Is.True);
        Assert.That(dictionary.Count, Is.EqualTo(1));

        // A duplicate key is rejected and the original value is left untouched.
        Assert.That(dictionary.TryAdd(1, 20), Is.False);
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary.Count, Is.EqualTo(1));
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.Remove{TKey, TValue}(ref ValueDictionary{TKey, TValue}, TKey)" /> method.</summary>
    [Test]
    public static void RemoveTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);
        dictionary.Add(2, 20);

        Assert.That(dictionary.Remove(1), Is.True);
        Assert.That(dictionary.Count, Is.EqualTo(1));
        Assert.That(dictionary.ContainsKey(1), Is.False);

        Assert.That(dictionary.Remove(42), Is.False);
        Assert.That(dictionary.Count, Is.EqualTo(1));
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.Remove{TKey, TValue}(ref ValueDictionary{TKey, TValue}, TKey, out TValue)" /> method.</summary>
    [Test]
    public static void RemoveOutValueTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);

        Assert.That(dictionary.Remove(1, out var value), Is.True);
        Assert.That(value, Is.EqualTo(10));

        Assert.That(dictionary.Remove(1, out var missing), Is.False);
        Assert.That(missing, Is.EqualTo(0));
    }

    /// <summary>Provides validation that a removed slot is reused by a subsequent add.</summary>
    [Test]
    public static void RemoveThenAddReusesSlotTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);
        dictionary.Add(2, 20);
        dictionary.Add(3, 30);

        _ = dictionary.Remove(2);
        dictionary.Add(4, 40);

        Assert.That(dictionary.Count, Is.EqualTo(3));
        Assert.That(dictionary[1], Is.EqualTo(10));
        Assert.That(dictionary[3], Is.EqualTo(30));
        Assert.That(dictionary[4], Is.EqualTo(40));
        Assert.That(dictionary.ContainsKey(2), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.Clear{TKey, TValue}(ref ValueDictionary{TKey, TValue})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        ValueDictionary<int, int> dictionary = [];
        dictionary.Add(1, 10);
        dictionary.Add(2, 20);

        dictionary.Clear();

        Assert.That(dictionary.Count, Is.EqualTo(0));
        Assert.That(dictionary.IsEmpty, Is.True);
        Assert.That(dictionary.ContainsKey(1), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary.EnsureCapacity{TKey, TValue}(ref ValueDictionary{TKey, TValue}, int)" /> method.</summary>
    [Test]
    public static void EnsureCapacityTest()
    {
        ValueDictionary<int, int> dictionary = [];

        dictionary.EnsureCapacity(32);

        Assert.That(dictionary.Capacity, Is.GreaterThanOrEqualTo(32));
        Assert.That(dictionary.Count, Is.EqualTo(0));
    }

    /// <summary>Provides validation that adding more items than the initial capacity grows the dictionary correctly.</summary>
    [Test]
    public static void GrowthTest()
    {
        ValueDictionary<int, int> dictionary = [];

        for (var i = 0; i < 100; i++)
        {
            dictionary.Add(i, i * 10);
        }

        Assert.That(dictionary.Count, Is.EqualTo(100));

        for (var i = 0; i < 100; i++)
        {
            Assert.That(dictionary[i], Is.EqualTo(i * 10));
        }
    }

    /// <summary>Provides validation of the <see cref="ValueDictionary{TKey, TValue}.op_Equality" /> and <see cref="ValueDictionary{TKey, TValue}.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        ValueDictionary<int, int> left = [];
        ValueDictionary<int, int> right = [];
        var other = new ValueDictionary<int, int>(7);

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);

        Assert.That(left == other, Is.False);
        Assert.That(left != other, Is.True);
    }
}
