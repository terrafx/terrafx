// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValueStack{T}" /> struct.</summary>
public static class ValueStackTests
{
    /// <summary>Provides validation of the <see cref="ValueStack{T}.ValueStack()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        Assert.That(() => new ValueStack<int>(),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.ValueStack(int)" /> constructor.</summary>
    [Test]
    public static void CtorInt32Test()
    {
        Assert.That(() => new ValueStack<int>(5),
            Has.Property("Capacity").EqualTo(5)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueStack<int>(-5),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-5)
                  .And.Property("ParamName").EqualTo("capacity")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.ValueStack(IEnumerable{T})" /> constructor.</summary>
    [Test]
    public static void CtorIEnumerableTest()
    {
        Assert.That(() => new ValueStack<int>(Enumerable.Range(0, 3)),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueStack<int>(Enumerable.Empty<int>()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueStack<int>((null as IEnumerable<int>)!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("source")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.ValueStack(ReadOnlySpan{T})" /> constructor.</summary>
    [Test]
    public static void CtorReadOnlySpanTest()
    {
        Assert.That(() => new ValueStack<int>(new int[] { 1, 2, 3 }.AsSpan()),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueStack<int>(Array.Empty<int>().AsSpan()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueStack<int>((null as int[]).AsSpan()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.ValueStack(T[], bool)" /> constructor.</summary>
    [Test]
    public static void CtorArrayBooleanTest()
    {
        Assert.That(() => new ValueStack<int>(new int[] { 1, 2, 3 }, takeOwnership: false),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueStack<int>(new int[] { 1, 2, 3 }, takeOwnership: true),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueStack<int>(Array.Empty<int>(), takeOwnership: false),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueStack<int>(Array.Empty<int>(), takeOwnership: true),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueStack<int>(null!, takeOwnership: false),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("array")
        );

        Assert.That(() => new ValueStack<int>(null!, takeOwnership: true),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("array")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.Clear" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        valueStack.Clear();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        _ = valueStack.Pop();

        valueStack.Push(4);
        valueStack.Clear();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        _ = valueStack.Pop();

        valueStack.Push(4);
        valueStack.Push(5);

        valueStack.Clear();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(0)
        );

        valueStack = new ValueStack<int>();
        valueStack.Clear();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.Contains(T)" /> method.</summary>
    [Test]
    public static void ContainsTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueStack.Contains(1),
            Is.True
        );

        Assert.That(() => valueStack.Contains(4),
            Is.False
        );

        _ = valueStack.Pop();
        valueStack.Push(4);

        Assert.That(() => valueStack.Contains(3),
            Is.False
        );

        Assert.That(() => valueStack.Contains(4),
            Is.True
        );

        _ = valueStack.Pop();
        valueStack.Push(5);
        valueStack.Push(6);

        Assert.That(() => valueStack.Contains(4),
            Is.False
        );

        Assert.That(() => valueStack.Contains(5),
            Is.True
        );

        Assert.That(() => valueStack.Contains(6),
            Is.True
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.Contains(0),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.CopyTo(Span{T})" /> method.</summary>
    [Test]
    public static void CopyToTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        var destination = new int[3];
        valueStack.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 1, 2, 3 })
        );

        _ = valueStack.Pop();
        valueStack.Push(4);

        valueStack.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 1, 2, 4 })
        );

        destination = new int[6];
        valueStack.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 1, 2, 4, 0, 0, 0 })
        );

        _ = valueStack.Pop();
        valueStack.Push(5);
        valueStack.Push(6);

        valueStack.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 1, 2, 5, 6, 0, 0 })
        );

        Assert.That(() => valueStack.CopyTo(Array.Empty<int>()),
            Throws.ArgumentException
                  .And.Property("ParamName").EqualTo("destination")
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.CopyTo(Array.Empty<int>()),
            Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.EnsureCapacity(int)" /> method.</summary>
    [Test]
    public static void EnsureCapacityTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        valueStack.EnsureCapacity(-1);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueStack.EnsureCapacity(0);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueStack.EnsureCapacity(3);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueStack.EnsureCapacity(4);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(3)
        );

        valueStack.EnsureCapacity(16);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(16)
               .And.Count.EqualTo(3)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.Peek()" /> method.</summary>
    [Test]
    public static void PeekTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueStack.Peek(),
            Is.EqualTo(3)
        );

        valueStack.Push(4);

        Assert.That(() => valueStack.Peek(),
            Is.EqualTo(4)
        );

        _ = valueStack.Pop();
        valueStack.Push(5);

        Assert.That(() => valueStack.Peek(),
            Is.EqualTo(5)
        );

        _ = valueStack.Pop();
        valueStack.Push(6);
        valueStack.Push(7);

        Assert.That(() => valueStack.Peek(),
            Is.EqualTo(7)
        );

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.Peek(),
            Throws.InvalidOperationException
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.Peek(int)" /> method.</summary>
    [Test]
    public static void PeekInt32Test()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueStack.Peek(0),
            Is.EqualTo(3)
        );

        valueStack.Push(4);

        Assert.That(() => valueStack.Peek(0),
            Is.EqualTo(4)
        );

        Assert.That(() => valueStack.Peek(3),
            Is.EqualTo(1)
        );

        _ = valueStack.Pop();
        valueStack.Push(5);

        Assert.That(() => valueStack.Peek(0),
            Is.EqualTo(5)
        );

        Assert.That(() => valueStack.Peek(1),
            Is.EqualTo(3)
        );

        _ = valueStack.Pop();
        valueStack.Push(6);
        valueStack.Push(7);

        Assert.That(() => valueStack.Peek(0),
            Is.EqualTo(7)
        );

        Assert.That(() => valueStack.Peek(2),
            Is.EqualTo(3)
        );

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.Peek(0),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(0)
                  .And.Property("ParamName").EqualTo("index")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.Pop()" /> method.</summary>
    [Test]
    public static void PopTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(3)
        );

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(2)
        );

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(1)
        );

        Assert.That(() => valueStack.Pop(),
            Throws.InvalidOperationException
        );

        valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        _ = valueStack.Pop();
        valueStack.Push(4);

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(4)
        );

        _ = valueStack.Pop();
        valueStack.Push(5);
        valueStack.Push(6);

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(6)
        );

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(5)
        );

        Assert.That(() => valueStack.Pop(),
            Is.EqualTo(1)
        );

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.Pop(),
            Throws.InvalidOperationException
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.Push(T)" /> method.</summary>
    [Test]
    public static void PushTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        valueStack.Push(4);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        valueStack.Push(5);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        _ = valueStack.Pop();
        valueStack.Push(5);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueStack = new ValueStack<int>();
        valueStack.Push(6);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(1)
               .And.Count.EqualTo(1)
        );
    }



    /// <summary>Provides validation of the <see cref="ValueStack{T}.TrimExcess(float)" /> method.</summary>
    [Test]
    public static void TrimExcessTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        valueStack.TrimExcess();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        _ = valueStack.Pop();

        valueStack.Push(4);
        valueStack.TrimExcess();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });

        valueStack.Push(4);
        valueStack.Push(5);

        valueStack.TrimExcess();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(5)
               .And.Count.EqualTo(5)
        );

        valueStack.EnsureCapacity(15);
        valueStack.TrimExcess(0.3f);

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(15)
               .And.Count.EqualTo(5)
        );

        valueStack = new ValueStack<int>();
        valueStack.TrimExcess();

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.TryPeek(out T)" /> method.</summary>
    [Test]
    public static void TryPeekTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        var result = valueStack.TryPeek(out var value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(3)
        );

        valueStack.Push(4);
        result = valueStack.TryPeek(out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(4)
        );

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.TryPeek(out _),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.TryPeek(int, out T)" /> method.</summary>
    [Test]
    public static void TryPeekInt32Test()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        var result = valueStack.TryPeek(0, out var value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(3)
        );

        valueStack.Push(4);
        result = valueStack.TryPeek(0, out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(4)
        );

        result = valueStack.TryPeek(3, out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(1)
        );

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.TryPeek(0, out _),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueStack{T}.TryPop(out T)" /> method.</summary>
    [Test]
    public static void TryPopTest()
    {
        var valueStack = new ValueStack<int>(new int[] { 1, 2, 3 });
        var result = valueStack.TryPop(out var value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(3)
        );

        valueStack.Push(4);
        result = valueStack.TryPop(out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(4)
        );

        Assert.That(() => valueStack,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(2)
        );

        valueStack = new ValueStack<int>();

        Assert.That(() => valueStack.TryPop(out _),
            Is.False
        );
    }
}
