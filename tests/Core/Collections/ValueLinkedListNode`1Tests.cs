// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValueLinkedListNode{T}" /> class.</summary>
internal static class ValueLinkedListNodeTests
{
    /// <summary>Provides validation of the <see cref="ValueLinkedListNode{T}.ValueLinkedListNode(T)" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        var node = new ValueLinkedListNode<int>(5);

        Assert.That(node.Value, Is.EqualTo(5));
        Assert.That(node.HasParent, Is.False);
        Assert.That(node.IsFirstNode, Is.False);
        Assert.That(node.Next, Is.Null);
        Assert.That(node.Previous, Is.Null);
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedListNode{T}.Value" /> property.</summary>
    [Test]
    public static void ValueTest()
    {
        var node = new ValueLinkedListNode<int>(5)
        {
            Value = 10,
        };

        Assert.That(node.Value, Is.EqualTo(10));
    }

    /// <summary>Provides validation of the <see cref="ValueLinkedListNode{T}.ValueRef" /> property.</summary>
    [Test]
    public static void ValueRefTest()
    {
        var node = new ValueLinkedListNode<int>(5);

        ref var valueRef = ref node.ValueRef;
        valueRef = 10;

        Assert.That(node.Value, Is.EqualTo(10));
        Assert.That(node.ValueRef, Is.EqualTo(10));
    }

    /// <summary>Provides validation that a node reflects its links once added to a list.</summary>
    [Test]
    public static void LinksTest()
    {
        var list = new ValueLinkedList<int>();

        var first = list.AddLast(1);
        var middle = list.AddLast(2);
        var last = list.AddLast(3);

        Assert.That(first.HasParent, Is.True);
        Assert.That(first.IsFirstNode, Is.True);
        Assert.That(first.Previous, Is.Null);
        Assert.That(first.Next, Is.SameAs(middle));

        Assert.That(middle.IsFirstNode, Is.False);
        Assert.That(middle.Previous, Is.SameAs(first));
        Assert.That(middle.Next, Is.SameAs(last));

        Assert.That(last.IsFirstNode, Is.False);
        Assert.That(last.Previous, Is.SameAs(middle));
        Assert.That(last.Next, Is.Null);
    }
}
