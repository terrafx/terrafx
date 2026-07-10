// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="PropertySet" /> class.</summary>
internal static class PropertySetTests
{
    /// <summary>Provides validation of the <see cref="PropertySet.PropertySet()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        var propertySet = new PropertySet();

        Assert.That(propertySet.Count, Is.EqualTo(0));
        Assert.That(propertySet.IsEmpty, Is.True);
    }

    /// <summary>Provides validation of the <see cref="PropertySet.PropertySet(int)" /> constructor.</summary>
    [Test]
    public static void CtorInt32Test()
    {
        var propertySet = new PropertySet(4);

        Assert.That(propertySet.Count, Is.EqualTo(0));

        Assert.That(() => new PropertySet(-1),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ParamName").EqualTo("capacity")
        );
    }

    /// <summary>Provides validation of the <see cref="PropertySet.PropertySet(IEnumerable{KeyValuePair{string, object}})" /> constructor.</summary>
    [Test]
    public static void CtorIEnumerableTest()
    {
        var items = new List<KeyValuePair<string, object>>
        {
            new("a", 1),
            new("b", 2),
        };

        var propertySet = new PropertySet(items);

        Assert.That(propertySet.Count, Is.EqualTo(2));
        Assert.That(propertySet["a"], Is.EqualTo(1));
        Assert.That(propertySet["b"], Is.EqualTo(2));

        Assert.That(() => new PropertySet((IEnumerable<KeyValuePair<string, object>>)null!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("items")
        );
    }

    /// <summary>Provides validation of the <see cref="PropertySet.PropertySet(IDictionary{string, object})" /> constructor.</summary>
    [Test]
    public static void CtorIDictionaryTest()
    {
        var items = new Dictionary<string, object>
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        var propertySet = new PropertySet(items);

        Assert.That(propertySet.Count, Is.EqualTo(2));

        Assert.That(() => new PropertySet((IDictionary<string, object>)null!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("items")
        );
    }

    /// <summary>Provides validation that the indexer adds a new item and raises the add notification.</summary>
    [Test]
    public static void IndexerAddsTest()
    {
        var propertySet = new PropertySet();
        var eventArgs = CaptureNext(propertySet);

        propertySet["a"] = 1;

        Assert.That(propertySet["a"], Is.EqualTo(1));
        Assert.That(eventArgs.Value, Is.Not.Null);
        Assert.That(eventArgs.Value!.Action, Is.EqualTo(NotifyDictionaryChangedAction.Add));
        Assert.That(eventArgs.Value.Key, Is.EqualTo("a"));
    }

    /// <summary>Provides validation that the indexer updates an existing item and raises the value-changed notification.</summary>
    [Test]
    public static void IndexerUpdatesTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
        };

        var eventArgs = CaptureNext(propertySet);

        propertySet["a"] = 2;

        Assert.That(propertySet["a"], Is.EqualTo(2));
        Assert.That(eventArgs.Value, Is.Not.Null);
        Assert.That(eventArgs.Value!.Action, Is.EqualTo(NotifyDictionaryChangedAction.ValueChanged));
        Assert.That(eventArgs.Value.Key, Is.EqualTo("a"));
        Assert.That(eventArgs.Value.OldValue, Is.EqualTo(1));
        Assert.That(eventArgs.Value.NewValue, Is.EqualTo(2));
    }

    /// <summary>Provides validation of the <see cref="PropertySet.Add(string, object)" /> method.</summary>
    [Test]
    public static void AddTest()
    {
        var propertySet = new PropertySet();
        var eventArgs = CaptureNext(propertySet);

        propertySet.Add("a", 1);

        Assert.That(propertySet.Count, Is.EqualTo(1));
        Assert.That(eventArgs.Value!.Action, Is.EqualTo(NotifyDictionaryChangedAction.Add));

        Assert.That(() => propertySet.Add("a", 2), Throws.ArgumentException);
    }

    /// <summary>Provides validation of the <see cref="PropertySet.TryAdd(string, object)" /> method.</summary>
    [Test]
    public static void TryAddTest()
    {
        var propertySet = new PropertySet();

        Assert.That(propertySet.TryAdd("a", 1), Is.True);
        Assert.That(propertySet.TryAdd("a", 2), Is.False);
        Assert.That(propertySet["a"], Is.EqualTo(1));
    }

    /// <summary>Provides validation of the <see cref="PropertySet.Remove(string)" /> method.</summary>
    [Test]
    public static void RemoveTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
        };

        var eventArgs = CaptureNext(propertySet);

        Assert.That(propertySet.Remove("a"), Is.True);
        Assert.That(propertySet.Count, Is.EqualTo(0));
        Assert.That(eventArgs.Value!.Action, Is.EqualTo(NotifyDictionaryChangedAction.Remove));
        Assert.That(eventArgs.Value.Key, Is.EqualTo("a"));

        Assert.That(propertySet.Remove("a"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="PropertySet.Remove(string, out object)" /> method.</summary>
    [Test]
    public static void RemoveOutValueTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
        };

        Assert.That(propertySet.Remove("a", out var value), Is.True);
        Assert.That(value, Is.EqualTo(1));

        Assert.That(propertySet.Remove("b", out _), Is.False);
    }

    /// <summary>Provides validation of the <see cref="PropertySet.Clear()" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        var eventArgs = CaptureNext(propertySet);

        propertySet.Clear();

        Assert.That(propertySet.Count, Is.EqualTo(0));
        Assert.That(propertySet.IsEmpty, Is.True);
        Assert.That(eventArgs.Value!.Action, Is.EqualTo(NotifyDictionaryChangedAction.Reset));
    }

    /// <summary>Provides validation of the <see cref="PropertySet.ContainsKey(string)" />, <see cref="PropertySet.ContainsValue(object)" />, and <see cref="PropertySet.Contains(KeyValuePair{string, object})" /> methods.</summary>
    [Test]
    public static void ContainsTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
        };

        Assert.That(propertySet.ContainsKey("a"), Is.True);
        Assert.That(propertySet.ContainsKey("b"), Is.False);

        Assert.That(propertySet.ContainsValue(1), Is.True);
        Assert.That(propertySet.ContainsValue(2), Is.False);

        Assert.That(propertySet.Contains(new KeyValuePair<string, object>("a", 1)), Is.True);
        Assert.That(propertySet.Contains(new KeyValuePair<string, object>("a", 2)), Is.False);
    }

    /// <summary>Provides validation of the <see cref="PropertySet.TryGetValue(string, out object)" /> method.</summary>
    [Test]
    public static void TryGetValueTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
        };

        Assert.That(propertySet.TryGetValue("a", out var value), Is.True);
        Assert.That(value, Is.EqualTo(1));

        Assert.That(propertySet.TryGetValue("b", out _), Is.False);
    }

    /// <summary>Provides validation of the <see cref="PropertySet.CopyTo(Span{KeyValuePair{string, object}})" /> method.</summary>
    [Test]
    public static void CopyToTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        var destination = new KeyValuePair<string, object>[2];
        propertySet.CopyTo(destination);

        Assert.That(destination, Has.Member(new KeyValuePair<string, object>("a", 1)));
        Assert.That(destination, Has.Member(new KeyValuePair<string, object>("b", 2)));
    }

    /// <summary>Provides validation of the <see cref="PropertySet.Keys" /> and <see cref="PropertySet.Values" /> collections.</summary>
    [Test]
    public static void KeysAndValuesTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        var keys = propertySet.Keys;
        Assert.That(keys.Count, Is.EqualTo(2));
        Assert.That(keys.Contains("a"), Is.True);

        var values = propertySet.Values;
        Assert.That(values.Count, Is.EqualTo(2));
        Assert.That(values.Contains(1), Is.True);
    }

    /// <summary>Provides validation that the property set enumerates the items it contains.</summary>
    [Test]
    public static void GetEnumeratorTest()
    {
        var propertySet = new PropertySet
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        var count = 0;

        foreach (var item in propertySet)
        {
            count++;
        }

        Assert.That(count, Is.EqualTo(2));
    }

    /// <summary>Provides validation that the missing-key indexer getter throws.</summary>
    [Test]
    public static void IndexerMissingKeyThrowsTest()
    {
        var propertySet = new PropertySet();

        Assert.That(() => propertySet["missing"], Throws.InstanceOf<KeyNotFoundException>());
    }

    private static StrongBox<NotifyDictionaryChangedEventArgs<string, object>?> CaptureNext(PropertySet propertySet)
    {
        var box = new StrongBox<NotifyDictionaryChangedEventArgs<string, object>?>(null);
        propertySet.DictionaryChanged += (sender, eventArgs) => box.Value = eventArgs;
        return box;
    }
}
