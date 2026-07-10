// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="UnmanagedValueLinkedList{T}" /> struct.</summary>
internal static unsafe class UnmanagedValueLinkedListTests
{
    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList{T}" /> default state.</summary>
    [Test]
    public static void EmptyTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        Assert.That(list.Count, Is.EqualTo((nuint)0));
        Assert.That(list.IsEmpty, Is.True);
        Assert.That(list.First is null, Is.True);
        Assert.That(list.Last is null, Is.True);

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList{T}.UnmanagedValueLinkedList(UnmanagedReadOnlySpan{T})" /> constructor.</summary>
    [Test]
    public static void CtorSpanTest()
    {
        var source = new UnmanagedArray<int>(3);
        source[0] = 1;
        source[1] = 2;
        source[2] = 3;

        var list = new UnmanagedValueLinkedList<int>(source);

        Assert.That(list.Count, Is.EqualTo((nuint)3));
        Assert.That(list.First->Value, Is.EqualTo(1));
        Assert.That(list.Last->Value, Is.EqualTo(3));

        list.Dispose();
        source.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.AddLast{T}(ref UnmanagedValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void AddLastTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        var first = list.AddLast(1);
        _ = list.AddLast(2);
        var last = list.AddLast(3);

        Assert.That(list.Count, Is.EqualTo((nuint)3));
        Assert.That(list.First == first, Is.True);
        Assert.That(list.Last == last, Is.True);
        Assert.That(first->IsFirstNode, Is.True);
        Assert.That(last->Value, Is.EqualTo(3));

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.AddFirst{T}(ref UnmanagedValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void AddFirstTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        _ = list.AddFirst(1);
        var newFirst = list.AddFirst(2);

        Assert.That(list.Count, Is.EqualTo((nuint)2));
        Assert.That(list.First == newFirst, Is.True);
        Assert.That(newFirst->Value, Is.EqualTo(2));
        Assert.That(list.Last->Value, Is.EqualTo(1));

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.AddAfter{T}(ref UnmanagedValueLinkedList{T}, UnmanagedValueLinkedListNode{T}*, T)" /> method.</summary>
    [Test]
    public static void AddAfterTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        var node = list.AddLast(1);
        _ = list.AddLast(3);

        var inserted = list.AddAfter(node, 2);

        Assert.That(list.Count, Is.EqualTo((nuint)3));
        Assert.That(inserted->Value, Is.EqualTo(2));

        var destination = new UnmanagedArray<int>(3);
        list.CopyTo(destination.AsUnmanagedSpan());

        Assert.That(destination[0], Is.EqualTo(1));
        Assert.That(destination[1], Is.EqualTo(2));
        Assert.That(destination[2], Is.EqualTo(3));

        destination.Dispose();
        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.AddBefore{T}(ref UnmanagedValueLinkedList{T}, UnmanagedValueLinkedListNode{T}*, T)" /> method.</summary>
    [Test]
    public static void AddBeforeTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        var first = list.AddLast(2);
        var inserted = list.AddBefore(first, 1);

        Assert.That(list.Count, Is.EqualTo((nuint)2));
        Assert.That(list.First == inserted, Is.True);
        Assert.That(inserted->IsFirstNode, Is.True);
        Assert.That(first->IsFirstNode, Is.False);

        list.Dispose();
    }

    /// <summary>Provides validation that adding an already-parented node is rejected.</summary>
    [Test]
    public static void AddAlreadyParentedNodeTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        var node = list.AddLast(1);

        Assert.That(() => list.AddLast(node),
            Throws.InstanceOf<InvalidOperationException>()
        );

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.Contains{T}(ref readonly UnmanagedValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void ContainsTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        var node = list.AddLast(1);
        _ = list.AddLast(2);

        Assert.That(list.Contains(1), Is.True);
        Assert.That(list.Contains(3), Is.False);
        Assert.That(list.Contains(node), Is.True);

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.CopyTo{T}(ref readonly UnmanagedValueLinkedList{T}, UnmanagedSpan{T})" /> method.</summary>
    [Test]
    public static void CopyToTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        _ = list.AddLast(1);
        _ = list.AddLast(2);
        _ = list.AddLast(3);

        var exact = new UnmanagedArray<int>(3);
        list.CopyTo(exact.AsUnmanagedSpan());

        Assert.That(exact[0], Is.EqualTo(1));
        Assert.That(exact[1], Is.EqualTo(2));
        Assert.That(exact[2], Is.EqualTo(3));

        // A destination larger than the count is valid; only the first Count elements are written.
        var larger = new UnmanagedArray<int>(5);
        list.CopyTo(larger.AsUnmanagedSpan());

        Assert.That(larger[0], Is.EqualTo(1));
        Assert.That(larger[1], Is.EqualTo(2));
        Assert.That(larger[2], Is.EqualTo(3));

        // A destination smaller than the count is rejected.
        var smaller = new UnmanagedArray<int>(2);

        Assert.That(() => list.CopyTo(smaller.AsUnmanagedSpan()),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
        );

        smaller.Dispose();
        larger.Dispose();
        exact.Dispose();
        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.Find{T}(ref readonly UnmanagedValueLinkedList{T}, T)" /> and <see cref="UnmanagedValueLinkedList.FindLast{T}(ref readonly UnmanagedValueLinkedList{T}, T)" /> methods.</summary>
    [Test]
    public static void FindTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        var firstSeven = list.AddLast(7);
        _ = list.AddLast(8);
        var lastSeven = list.AddLast(7);

        Assert.That(list.Find(7) == firstSeven, Is.True);
        Assert.That(list.FindLast(7) == lastSeven, Is.True);
        Assert.That(list.Find(9) is null, Is.True);
        Assert.That(list.FindLast(9) is null, Is.True);

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.Remove{T}(ref UnmanagedValueLinkedList{T}, T)" /> method.</summary>
    [Test]
    public static void RemoveValueTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        _ = list.AddLast(1);
        _ = list.AddLast(2);
        _ = list.AddLast(3);

        Assert.That(list.Remove(2), Is.True);
        Assert.That(list.Count, Is.EqualTo((nuint)2));
        Assert.That(list.Contains(2), Is.False);

        Assert.That(list.Remove(42), Is.False);
        Assert.That(list.Count, Is.EqualTo((nuint)2));

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.Remove{T}(ref UnmanagedValueLinkedList{T}, UnmanagedValueLinkedListNode{T}*)" /> method.</summary>
    [Test]
    public static void RemoveNodeTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        _ = list.AddLast(1);
        var node = list.AddLast(2);

        list.Remove(node);

        Assert.That(list.Count, Is.EqualTo((nuint)1));
        Assert.That(node->HasParent, Is.False);

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.RemoveFirst{T}(ref UnmanagedValueLinkedList{T})" /> and <see cref="UnmanagedValueLinkedList.RemoveLast{T}(ref UnmanagedValueLinkedList{T})" /> methods.</summary>
    [Test]
    public static void RemoveFirstLastTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        _ = list.AddLast(1);
        _ = list.AddLast(2);
        _ = list.AddLast(3);

        list.RemoveFirst();
        list.RemoveLast();

        Assert.That(list.Count, Is.EqualTo((nuint)1));
        Assert.That(list.First->Value, Is.EqualTo(2));
        Assert.That(list.Last->Value, Is.EqualTo(2));

        list.Dispose();

        var empty = new UnmanagedValueLinkedList<int>();

        Assert.That(() => empty.RemoveFirst(),
            Throws.ArgumentNullException
        );

        Assert.That(() => empty.RemoveLast(),
            Throws.ArgumentNullException
        );

        empty.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList.Clear{T}(ref UnmanagedValueLinkedList{T})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        var list = new UnmanagedValueLinkedList<int>();
        var node = list.AddLast(1);
        _ = list.AddLast(2);

        list.Clear();

        Assert.That(list.Count, Is.EqualTo((nuint)0));
        Assert.That(list.IsEmpty, Is.True);
        Assert.That(list.First is null, Is.True);
        Assert.That(node->HasParent, Is.False);

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedListNode{T}" /> node links.</summary>
    [Test]
    public static void NodeLinksTest()
    {
        var list = new UnmanagedValueLinkedList<int>();

        var first = list.AddLast(1);
        var middle = list.AddLast(2);
        var last = list.AddLast(3);

        Assert.That(first->Previous is null, Is.True);
        Assert.That(first->Next == middle, Is.True);

        Assert.That(middle->Previous == first, Is.True);
        Assert.That(middle->Next == last, Is.True);

        Assert.That(last->Previous == middle, Is.True);
        Assert.That(last->Next is null, Is.True);

        last->Value = 30;
        Assert.That(last->Value, Is.EqualTo(30));

        last->ValueRef = 300;
        Assert.That(last->Value, Is.EqualTo(300));

        list.Dispose();
    }

    /// <summary>Provides validation of the <see cref="UnmanagedValueLinkedList{T}.op_Equality" /> and <see cref="UnmanagedValueLinkedList{T}.op_Inequality" /> methods.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        var left = new UnmanagedValueLinkedList<int>();
        var right = new UnmanagedValueLinkedList<int>();

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);

        _ = left.AddLast(1);

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);

        left.Dispose();
        right.Dispose();
    }
}
