// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValueQueue{T}" /> struct.</summary>
public static class ValueQueueTests
{
    /// <summary>Provides validation of the <see cref="ValueQueue{T}.ValueQueue()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        Assert.That(() => new ValueQueue<int>(),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.ValueQueue(int)" /> constructor.</summary>
    [Test]
    public static void CtorInt32Test()
    {
        Assert.That(() => new ValueQueue<int>(5),
            Has.Property("Capacity").EqualTo(5)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueQueue<int>(-5),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-5)
                  .And.Property("ParamName").EqualTo("capacity")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.ValueQueue(IEnumerable{T})" /> constructor.</summary>
    [Test]
    public static void CtorIEnumerableTest()
    {
        Assert.That(() => new ValueQueue<int>(Enumerable.Range(0, 3)),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueQueue<int>(Enumerable.Empty<int>()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueQueue<int>((null as IEnumerable<int>)!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("source")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.ValueQueue(ReadOnlySpan{T})" /> constructor.</summary>
    [Test]
    public static void CtorReadOnlySpanTest()
    {
        Assert.That(() => new ValueQueue<int>(new int[] { 1, 2, 3 }.AsSpan()),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueQueue<int>(Array.Empty<int>().AsSpan()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueQueue<int>((null as int[]).AsSpan()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.ValueQueue(T[], bool)" /> constructor.</summary>
    [Test]
    public static void CtorArrayBooleanTest()
    {
        Assert.That(() => new ValueQueue<int>(new int[] { 1, 2, 3 }, takeOwnership: false),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueQueue<int>(new int[] { 1, 2, 3 }, takeOwnership: true),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueQueue<int>(Array.Empty<int>(), takeOwnership: false),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueQueue<int>(Array.Empty<int>(), takeOwnership: true),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueQueue<int>(null!, takeOwnership: false),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("array")
        );

        Assert.That(() => new ValueQueue<int>(null!, takeOwnership: true),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("array")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.Clear" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        valueQueue.Clear();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        _ = valueQueue.Dequeue();

        valueQueue.Enqueue(4);
        valueQueue.Clear();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        _ = valueQueue.Dequeue();

        valueQueue.Enqueue(4);
        valueQueue.Enqueue(5);

        valueQueue.Clear();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(0)
        );

        valueQueue = new ValueQueue<int>();
        valueQueue.Clear();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.Contains(T)" /> method.</summary>
    [Test]
    public static void ContainsTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueQueue.Contains(1),
            Is.True
        );

        Assert.That(() => valueQueue.Contains(4),
            Is.False
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(4);

        Assert.That(() => valueQueue.Contains(1),
            Is.False
        );

        Assert.That(() => valueQueue.Contains(4),
            Is.True
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(5);
        valueQueue.Enqueue(6);

        Assert.That(() => valueQueue.Contains(2),
            Is.False
        );

        Assert.That(() => valueQueue.Contains(5),
            Is.True
        );

        Assert.That(() => valueQueue.Contains(6),
            Is.True
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.Contains(0),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.CopyTo(Span{T})" /> method.</summary>
    [Test]
    public static void CopyToTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        var destination = new int[3];
        valueQueue.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 1, 2, 3 })
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(4);

        valueQueue.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 2, 3, 4 })
        );

        destination = new int[6];
        valueQueue.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 2, 3, 4, 0, 0, 0 })
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(5);
        valueQueue.Enqueue(6);

        valueQueue.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo(new int[] { 3, 4, 5, 6, 0, 0 })
        );

        Assert.That(() => valueQueue.CopyTo(Array.Empty<int>()),
            Throws.ArgumentException
                  .And.Property("ParamName").EqualTo("destination")
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.CopyTo(Array.Empty<int>()),
            Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.Dequeue()" /> method.</summary>
    [Test]
    public static void DequeueTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(1)
        );

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(2)
        );

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(3)
        );

        Assert.That(() => valueQueue.Dequeue(),
            Throws.InvalidOperationException
        );

        valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(4);

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(2)
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(5);
        valueQueue.Enqueue(6);

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(4)
        );

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(5)
        );

        Assert.That(() => valueQueue.Dequeue(),
            Is.EqualTo(6)
        );

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.Dequeue(),
            Throws.InvalidOperationException
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.Enqueue(T)" /> method.</summary>
    [Test]
    public static void EnqueueTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        valueQueue.Enqueue(4);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        valueQueue.Enqueue(5);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(5);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueQueue = new ValueQueue<int>();
        valueQueue.Enqueue(6);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(1)
               .And.Count.EqualTo(1)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.EnsureCapacity(int)" /> method.</summary>
    [Test]
    public static void EnsureCapacityTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        valueQueue.EnsureCapacity(-1);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueQueue.EnsureCapacity(0);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueQueue.EnsureCapacity(3);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueQueue.EnsureCapacity(4);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(3)
        );

        valueQueue.EnsureCapacity(16);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(16)
               .And.Count.EqualTo(3)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.Peek()" /> method.</summary>
    [Test]
    public static void PeekTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueQueue.Peek(),
            Is.EqualTo(1)
        );

        valueQueue.Enqueue(4);

        Assert.That(() => valueQueue.Peek(),
            Is.EqualTo(1)
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(5);

        Assert.That(() => valueQueue.Peek(),
            Is.EqualTo(2)
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(6);
        valueQueue.Enqueue(7);

        Assert.That(() => valueQueue.Peek(),
            Is.EqualTo(3)
        );

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.Peek(),
            Throws.InvalidOperationException
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.Peek(int)" /> method.</summary>
    [Test]
    public static void PeekInt32Test()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueQueue.Peek(0),
            Is.EqualTo(1)
        );

        valueQueue.Enqueue(4);

        Assert.That(() => valueQueue.Peek(0),
            Is.EqualTo(1)
        );

        Assert.That(() => valueQueue.Peek(3),
            Is.EqualTo(4)
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(5);

        Assert.That(() => valueQueue.Peek(0),
            Is.EqualTo(2)
        );

        Assert.That(() => valueQueue.Peek(1),
            Is.EqualTo(3)
        );

        _ = valueQueue.Dequeue();
        valueQueue.Enqueue(6);
        valueQueue.Enqueue(7);

        Assert.That(() => valueQueue.Peek(0),
            Is.EqualTo(3)
        );

        Assert.That(() => valueQueue.Peek(2),
            Is.EqualTo(5)
        );

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.Peek(0),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(0)
                  .And.Property("ParamName").EqualTo("index")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.TrimExcess(float)" /> method.</summary>
    [Test]
    public static void TrimExcessTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        valueQueue.TrimExcess();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        _ = valueQueue.Dequeue();

        valueQueue.Enqueue(4);
        valueQueue.TrimExcess();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });

        valueQueue.Enqueue(4);
        valueQueue.Enqueue(5);

        valueQueue.TrimExcess();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(5)
               .And.Count.EqualTo(5)
        );

        valueQueue.EnsureCapacity(15);
        valueQueue.TrimExcess(0.3f);

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(15)
               .And.Count.EqualTo(5)
        );

        valueQueue = new ValueQueue<int>();
        valueQueue.TrimExcess();

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.TryDequeue(out T)" /> method.</summary>
    [Test]
    public static void TryDequeueTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        var result = valueQueue.TryDequeue(out var value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(1)
        );

        valueQueue.Enqueue(4);
        result = valueQueue.TryDequeue(out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(2)
        );

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(2)
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.TryDequeue(out _),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.TryPeek(out T)" /> method.</summary>
    [Test]
    public static void TryPeekTest()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        var result = valueQueue.TryPeek(out var value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(1)
        );

        valueQueue.Enqueue(4);
        result = valueQueue.TryPeek(out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(1)
        );

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.TryPeek(out _),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueQueue{T}.TryPeek(int, out T)" /> method.</summary>
    [Test]
    public static void TryPeekInt32Test()
    {
        var valueQueue = new ValueQueue<int>(new int[] { 1, 2, 3 });
        var result = valueQueue.TryPeek(0, out var value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(1)
        );

        valueQueue.Enqueue(4);
        result = valueQueue.TryPeek(0, out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(1)
        );

        result = valueQueue.TryPeek(3, out value);

        Assert.That(() => result,
            Is.True
        );

        Assert.That(() => value,
            Is.EqualTo(4)
        );

        Assert.That(() => valueQueue,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        valueQueue = new ValueQueue<int>();

        Assert.That(() => valueQueue.TryPeek(0, out _),
            Is.False
        );
    }
}
