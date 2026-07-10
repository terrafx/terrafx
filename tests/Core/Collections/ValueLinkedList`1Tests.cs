// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValueLinkedList{T}" /> struct.</summary>
internal static class ValueLinkedListTests
{
    /// <summary>Provides validation of the <see cref="ValueLinkedList{T}.ValueLinkedList(IEnumerable{T})" /> constructor.</summary>
    [Test]
    public static void CtorIEnumerableTest()
    {
        Assert.That(() => new ValueLinkedList<int>(new List<int> { 1, 2, 3 }),
            Has.Count.EqualTo(3)
        );

        Assert.That(() => new ValueLinkedList<int>((null as IEnumerable<int>)!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("source")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList{T}.ValueLinkedList(ReadOnlySpan{T})" /> constructor.</summary>
    [Test]
    public static void CtorReadOnlySpanTest()
    {
        var list = new ValueLinkedList<int>((ReadOnlySpan<int>)[1, 2, 3]);

        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(list.First!.Value, Is.EqualTo(1));
        Assert.That(list.Last!.Value, Is.EqualTo(3));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList{T}" /> default state.</summary>
    [Test]
    public static void EmptyTest()
    {
        ValueLinkedList<int> list = [];

        Assert.That(list.Count, Is.EqualTo(0));
        Assert.That(list.IsEmpty, Is.True);
        Assert.That(list.First, Is.Null);
        Assert.That(list.Last, Is.Null);
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.AddLast{T}(ref ValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void AddLastTest()
    {
        ValueLinkedList<int> list = [];

        var first = list.AddLast(1);
        _ = list.AddLast(2);
        var last = list.AddLast(3);

        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(list.First, Is.EqualTo(first));
        Assert.That(list.Last, Is.EqualTo(last));
        Assert.That(first.IsFirstNode, Is.True);
        Assert.That(last.Value, Is.EqualTo(3));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.AddFirst{T}(ref ValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void AddFirstTest()
    {
        ValueLinkedList<int> list = [];

        _ = list.AddFirst(1);
        var newFirst = list.AddFirst(2);

        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list.First, Is.EqualTo(newFirst));
        Assert.That(newFirst.Value, Is.EqualTo(2));
        Assert.That(list.Last!.Value, Is.EqualTo(1));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.AddAfter{T}(ref ValueLinkedList{T}, ValueLinkedListNode{T}, T)" /> method.</summary>
    [Test]
    public static void AddAfterTest()
    {
        ValueLinkedList<int> list = [];

        var node = list.AddLast(1);
        _ = list.AddLast(3);

        var inserted = list.AddAfter(node, 2);

        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(inserted.Value, Is.EqualTo(2));

        Span<int> destination = stackalloc int[3];
        list.CopyTo(destination);

        int[] expected = [1, 2, 3];
        Assert.That(destination.ToArray(), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.AddBefore{T}(ref ValueLinkedList{T}, ValueLinkedListNode{T}, T)" /> method.</summary>
    [Test]
    public static void AddBeforeTest()
    {
        ValueLinkedList<int> list = [];

        var first = list.AddLast(2);
        var inserted = list.AddBefore(first, 1);

        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list.First, Is.EqualTo(inserted));
        Assert.That(inserted.IsFirstNode, Is.True);
        Assert.That(first.IsFirstNode, Is.False);
    }

    /// <summary>Provides validation that adding an already-parented node is rejected.</summary>
    [Test]
    public static void AddAlreadyParentedNodeTest()
    {
        ValueLinkedList<int> list = [];
        var node = list.AddLast(1);

        Assert.That(() => list.AddLast(node),
            Throws.InstanceOf<InvalidOperationException>()
        );

        Assert.That(() => list.AddFirst((null as ValueLinkedListNode<int>)!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("node")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.Contains{T}(ref readonly ValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void ContainsTest()
    {
        ValueLinkedList<int> list = [];
        var node = list.AddLast(1);
        _ = list.AddLast(2);

        Assert.That(list.Contains(1), Is.True);
        Assert.That(list.Contains(3), Is.False);
        Assert.That(list.Contains(node), Is.True);
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.CopyTo{T}(ref readonly ValueLinkedList{T}, Span{T})" /> method.</summary>
    [Test]
    public static void CopyToTest()
    {
        var list = new ValueLinkedList<int>((ReadOnlySpan<int>)[1, 2, 3]);
        int[] expected = [1, 2, 3];

        Span<int> exact = stackalloc int[3];
        list.CopyTo(exact);
        Assert.That(exact.ToArray(), Is.EqualTo(expected));

        // A destination larger than the count is valid; only the first Count elements are written.
        Span<int> larger = stackalloc int[5];
        list.CopyTo(larger);
        Assert.That(larger[..3].ToArray(), Is.EqualTo(expected));

        // A destination smaller than the count is rejected.
        Assert.That(() => list.CopyTo(new int[2]),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.Find{T}(ref readonly ValueLinkedList{T}, T)" /> and <see cref="ValueLinkedList.FindLast{T}(ref readonly ValueLinkedList{T}, T)" /> methods.</summary>
    [Test]
    public static void FindTest()
    {
        ValueLinkedList<int> list = [];

        var firstSeven = list.AddLast(7);
        _ = list.AddLast(8);
        var lastSeven = list.AddLast(7);

        Assert.That(list.Find(7), Is.EqualTo(firstSeven));
        Assert.That(list.FindLast(7), Is.EqualTo(lastSeven));
        Assert.That(list.Find(9), Is.Null);
        Assert.That(list.FindLast(9), Is.Null);
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.Remove{T}(ref ValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void RemoveValueTest()
    {
        ValueLinkedList<int> list = [];
        _ = list.AddLast(1);
        _ = list.AddLast(2);
        _ = list.AddLast(3);

        Assert.That(list.Remove(2), Is.True);
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list.Contains(2), Is.False);

        Assert.That(list.Remove(42), Is.False);
        Assert.That(list.Count, Is.EqualTo(2));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.Remove{T}(ref ValueLinkedList{T}, ValueLinkedListNode{T})" /> method.</summary>
    [Test]
    public static void RemoveNodeTest()
    {
        ValueLinkedList<int> list = [];
        _ = list.AddLast(1);
        var node = list.AddLast(2);

        list.Remove(node);

        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(node.HasParent, Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.RemoveFirst{T}(ref ValueLinkedList{T})" /> and <see cref="ValueLinkedList.RemoveLast{T}(ref ValueLinkedList{T})" /> methods.</summary>
    [Test]
    public static void RemoveFirstLastTest()
    {
        ValueLinkedList<int> list = [];
        _ = list.AddLast(1);
        _ = list.AddLast(2);
        _ = list.AddLast(3);

        list.RemoveFirst();
        list.RemoveLast();

        Assert.That(list.Count, Is.EqualTo(1));
        Assert.That(list.First!.Value, Is.EqualTo(2));
        Assert.That(list.Last!.Value, Is.EqualTo(2));

        ValueLinkedList<int> empty = [];

        Assert.That(() => empty.RemoveFirst(),
            Throws.ArgumentNullException
        );

        Assert.That(() => empty.RemoveLast(),
            Throws.ArgumentNullException
        );
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList.Clear{T}(ref ValueLinkedList{T})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        ValueLinkedList<int> list = [];
        var node = list.AddLast(1);
        _ = list.AddLast(2);

        list.Clear();

        Assert.That(list.Count, Is.EqualTo(0));
        Assert.That(list.IsEmpty, Is.True);
        Assert.That(list.First, Is.Null);
        Assert.That(node.HasParent, Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedListNode{T}" /> node links.</summary>
    [Test]
    public static void NodeLinksTest()
    {
        ValueLinkedList<int> list = [];

        var first = list.AddLast(1);
        var middle = list.AddLast(2);
        var last = list.AddLast(3);

        Assert.That(first.Previous, Is.Null);
        Assert.That(first.Next, Is.EqualTo(middle));

        Assert.That(middle.Previous, Is.EqualTo(first));
        Assert.That(middle.Next, Is.EqualTo(last));

        Assert.That(last.Previous, Is.EqualTo(middle));
        Assert.That(last.Next, Is.Null);

        last.Value = 30;
        Assert.That(last.Value, Is.EqualTo(30));

        last.ValueRef = 300;
        Assert.That(last.Value, Is.EqualTo(300));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedList{T}.op_Equality" /> and <see cref="ValueLinkedList{T}.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        ValueLinkedList<int> left = [];
        ValueLinkedList<int> right = [];

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);

        _ = left.AddLast(1);

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
    }
}
