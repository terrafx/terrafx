// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TerraFX.Collections;

namespace TerraFX.UnitTests.Collections;

/// <summary>Provides a set of tests covering the <see cref="ValueList{T}" /> struct.</summary>
internal static class ValueListTests
{
    /// <summary>Provides validation of the <see cref="ValueList{T}.ValueList()" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        Assert.That(() => new ValueList<int>(),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList{T}.ValueList(int)" /> constructor.</summary>
    [Test]
    public static void CtorInt32Test()
    {
        Assert.That(() => new ValueList<int>(5),
            Has.Property("Capacity").EqualTo(5)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueList<int>(-5),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-5)
                  .And.Property("ParamName").EqualTo("capacity")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList{T}.ValueList(IEnumerable{T})" /> constructor.</summary>
    [Test]
    public static void CtorIEnumerableTest()
    {
        Assert.That(() => new ValueList<int>(Enumerable.Range(0, 3)),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueList<int>(Enumerable.Empty<int>()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueList<int>((null as IEnumerable<int>)!),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("source")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList{T}.ValueList(ReadOnlySpan{T})" /> constructor.</summary>
    [Test]
    public static void CtorReadOnlySpanTest()
    {
        Assert.That(() => new ValueList<int>([1, 2, 3]),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueList<int>([]),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueList<int>((null as int[]).AsSpan()),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList{T}.ValueList(T[], bool)" /> constructor.</summary>
    [Test]
    public static void CtorArrayBooleanTest()
    {
        Assert.That(() => new ValueList<int>([1, 2, 3], takeOwnership: false),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueList<int>([1, 2, 3], takeOwnership: true),
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        Assert.That(() => new ValueList<int>([], takeOwnership: false),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueList<int>([], takeOwnership: true),
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );

        Assert.That(() => new ValueList<int>(null!, takeOwnership: false),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("array")
        );

        Assert.That(() => new ValueList<int>(null!, takeOwnership: true),
            Throws.ArgumentNullException
                  .And.Property("ParamName").EqualTo("array")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList{T}" /> indexer.</summary>
    [Test]
    public static void GetItemTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueList[1],
            Is.EqualTo(2)
        );

        Assert.That(() => valueList[-1],
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-1)
                  .And.Property("ParamName").EqualTo("index")
        );

        Assert.That(() => valueList[3],
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(3)
                  .And.Property("ParamName").EqualTo("index")
        );

        Assert.That(() => new ValueList<int>()[0],
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(0)
                  .And.Property("ParamName").EqualTo("index")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList{T}" /> indexer.</summary>
    [Test]
    public static void SetItemTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueList[1] = 4,
            Is.EqualTo(4)
        );

        Assert.That(() => valueList[-1] = 4,
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-1)
                  .And.Property("ParamName").EqualTo("index")
        );

        Assert.That(() => valueList[3] = 4,
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(3)
                  .And.Property("ParamName").EqualTo("index")
        );

        valueList = [];

        Assert.That(() => valueList[0] = 4,
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(0)
                  .And.Property("ParamName").EqualTo("index")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.Add{T}(ref ValueList{T}, T)" /> method.</summary>
    [Test]
    public static void AddTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 }) {
            4
        };

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        Assert.That(() => valueList[3],
            Is.EqualTo(4)
        );

        valueList.Add(5);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(5)
        );

        Assert.That(() => valueList[4],
            Is.EqualTo(5)
        );

        valueList = [
            6
        ];

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(1)
               .And.Count.EqualTo(1)
        );

        Assert.That(() => valueList[0],
            Is.EqualTo(6)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.Clear{T}(ref ValueList{T})" /> method.</summary>
    [Test]
    public static void ClearTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });
        valueList.Clear();

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(0)
        );

        valueList = [];
        valueList.Clear();

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.Contains{T}(ref readonly ValueList{T}, T)" /> method.</summary>
    [Test]
    public static void ContainsTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueList.Contains(1),
            Is.True
        );

        Assert.That(() => valueList.Contains(4),
            Is.False
        );

        valueList = [];

        Assert.That(() => valueList.Contains(0),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.CopyTo{T}(ref readonly ValueList{T}, Span{T})" /> method.</summary>
    [Test]
    public static void CopyToTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });

        var destination = new int[3];
        valueList.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo([1, 2, 3])
        );

        destination = new int[6];
        valueList.CopyTo(destination);

        Assert.That(() => destination,
            Is.EquivalentTo([1, 2, 3, 0, 0, 0])
        );

        Assert.That(() => valueList.CopyTo([]),
            Throws.ArgumentException
                  .And.Property("ParamName").EqualTo("destination")
        );

        valueList = [];

        Assert.That(() => valueList.CopyTo([]),
            Throws.Nothing
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.EnsureCapacity{T}(ref ValueList{T}, int)" /> method.</summary>
    [Test]
    public static void EnsureCapacityTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });
        valueList.EnsureCapacity(-1);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueList.EnsureCapacity(0);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueList.EnsureCapacity(3);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueList.EnsureCapacity(4);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(3)
        );

        valueList.EnsureCapacity(16);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(16)
               .And.Count.EqualTo(3)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.IndexOf{T}(ref readonly ValueList{T}, T)" /> method.</summary>
    [Test]
    public static void IndexOfTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueList.IndexOf(1),
            Is.EqualTo(0)
        );

        Assert.That(() => valueList.IndexOf(4),
            Is.EqualTo(-1)
        );

        valueList = [];

        Assert.That(() => valueList.IndexOf(0),
            Is.EqualTo(-1)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.Insert{T}(ref ValueList{T}, int, T)" /> method.</summary>
    [Test]
    public static void InsertTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });
        valueList.Insert(3, 4);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(6)
               .And.Count.EqualTo(4)
        );

        Assert.That(() => valueList[3],
            Is.EqualTo(4)
        );

        Assert.That(() => valueList.Insert(5, 5),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(5)
                  .And.Property("ParamName").EqualTo("index")
        );

        valueList = [];
        valueList.Insert(0, 1);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(1)
               .And.Count.EqualTo(1)
        );

        Assert.That(() => valueList[0],
            Is.EqualTo(1)
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.Remove{T}(ref ValueList{T}, T)" /> method.</summary>
    [Test]
    public static void RemoveTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });

        Assert.That(() => valueList.Remove(1),
            Is.True
        );

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(2)
        );

        Assert.That(() => valueList[0],
            Is.EqualTo(2)
        );

        Assert.That(() => valueList[1],
            Is.EqualTo(3)
        );

        Assert.That(() => valueList.Remove(0),
            Is.False
        );

        valueList = [];

        Assert.That(() => valueList.Remove(0),
            Is.False
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.RemoveAt{T}(ref ValueList{T}, int)" /> method.</summary>
    [Test]
    public static void RemoveAtTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });
        valueList.RemoveAt(0);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(2)
        );

        Assert.That(() => valueList[0],
            Is.EqualTo(2)
        );

        Assert.That(() => valueList[1],
            Is.EqualTo(3)
        );

        Assert.That(() => valueList.RemoveAt(-1),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(-1)
                  .And.Property("ParamName").EqualTo("index")
        );

        Assert.That(() => valueList.RemoveAt(2),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(2)
                  .And.Property("ParamName").EqualTo("index")
        );

        valueList = [];

        Assert.That(() => valueList.RemoveAt(0),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ActualValue").EqualTo(0)
                  .And.Property("ParamName").EqualTo("index")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueList.TrimExcess{T}(ref ValueList{T}, float)" /> method.</summary>
    [Test]
    public static void TrimExcessTest()
    {
        var valueList = new ValueList<int>(new int[] { 1, 2, 3 });
        valueList.TrimExcess();

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(3)
               .And.Count.EqualTo(3)
        );

        valueList.Add(4);
        valueList.Add(5);

        valueList.TrimExcess();

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(5)
               .And.Count.EqualTo(5)
        );

        valueList.EnsureCapacity(15);
        valueList.TrimExcess(0.3f);

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(15)
               .And.Count.EqualTo(5)
        );

        valueList = [];
        valueList.TrimExcess();

        Assert.That(() => valueList,
            Has.Property("Capacity").EqualTo(0)
               .And.Count.EqualTo(0)
        );
    }
}
