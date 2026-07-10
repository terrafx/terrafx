// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;

namespace TerraFX.UnitTests;

/// <summary>Provides a set of tests covering the <see cref="ValueLazy{T}" /> struct.</summary>
internal static class ValueLazyTests
{
    /// <summary>Provides validation of the <see cref="ValueLazy{T}.ValueLazy(Func{T})" /> constructor.</summary>
    [Test]
    public static void CtorTest()
    {
        var lazy = new ValueLazy<int>(() => 42);

        Assert.That(lazy.IsValueCreated, Is.False);
        Assert.That(lazy.IsValueFaulted, Is.False);

        Assert.That(() => new ValueLazy<int>(null!),
            Throws.InstanceOf<ArgumentNullException>()
                  .And.Property("ParamName").EqualTo("factory")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.Value" /> property.</summary>
    [Test]
    public static void ValueTest()
    {
        var count = 0;

        var lazy = new ValueLazy<int>(() =>
        {
            count++;
            return 42;
        });

        Assert.That(lazy.IsValueCreated, Is.False);

        Assert.That(lazy.Value, Is.EqualTo(42));
        Assert.That(lazy.IsValueCreated, Is.True);

        // Accessing the value again should not invoke the factory a second time.
        Assert.That(lazy.Value, Is.EqualTo(42));
        Assert.That(count, Is.EqualTo(1));
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.ValueOrDefault" /> property.</summary>
    [Test]
    public static void ValueOrDefaultTest()
    {
        var lazy = new ValueLazy<int>(() => 42);

        Assert.That(lazy.ValueOrDefault, Is.EqualTo(0));

        _ = lazy.Value;

        Assert.That(lazy.ValueOrDefault, Is.EqualTo(42));
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.ValueRef" /> property.</summary>
    [Test]
    public static void ValueRefTest()
    {
        var lazy = new ValueLazy<int>(() => 42);

        ref var value = ref lazy.ValueRef;
        Assert.That(lazy.IsValueCreated, Is.True);
        Assert.That(value, Is.EqualTo(42));

        value = 100;
        Assert.That(lazy.Value, Is.EqualTo(100));
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.Reset(Func{T})" /> method.</summary>
    [Test]
    public static void ResetTest()
    {
        var lazy = new ValueLazy<int>(() => 42);
        Assert.That(lazy.Value, Is.EqualTo(42));

        lazy.Reset(() => 100);

        Assert.That(lazy.IsValueCreated, Is.False);
        Assert.That(lazy.Value, Is.EqualTo(100));

        Assert.That(() => lazy.Reset(null!),
            Throws.InstanceOf<ArgumentNullException>()
                  .And.Property("ParamName").EqualTo("factory")
        );
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.Dispose(Action{T})" /> method.</summary>
    [Test]
    public static void DisposeTest()
    {
        var disposed = 0;

        var lazy = new ValueLazy<int>(() => 42);
        _ = lazy.Value;

        lazy.Dispose((value) => disposed = value);
        Assert.That(disposed, Is.EqualTo(42));

        // A value that was never created should not invoke the disposal action.
        var uncreated = new ValueLazy<int>(() => 42);
        var called = false;

        uncreated.Dispose((value) => called = true);
        Assert.That(called, Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        static int factory()
        {
            return 42;
        }

        var left = new ValueLazy<int>(factory);
        var right = new ValueLazy<int>(factory);
        var other = new ValueLazy<int>(() => 42);

        Assert.That(left == right, Is.True);
        Assert.That(left == other, Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        static int factory()
        {
            return 42;
        }

        var left = new ValueLazy<int>(factory);
        var right = new ValueLazy<int>(factory);
        var other = new ValueLazy<int>(() => 42);

        Assert.That(left != right, Is.False);
        Assert.That(left != other, Is.True);
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.Equals(ValueLazy{T})" /> and <see cref="ValueLazy{T}.Equals(object)" /> methods.</summary>
    [Test]
    public static void EqualsTest()
    {
        static int factory()
        {
            return 42;
        }

        var left = new ValueLazy<int>(factory);
        var right = new ValueLazy<int>(factory);

        Assert.That(left.Equals(right), Is.True);
        Assert.That(left.Equals((object)right), Is.True);
        Assert.That(left.Equals(null), Is.False);
        Assert.That(left.Equals("not a lazy"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.GetHashCode()" /> method.</summary>
    [Test]
    public static void GetHashCodeTest()
    {
        static int factory()
        {
            return 42;
        }

        var left = new ValueLazy<int>(factory);
        var right = new ValueLazy<int>(factory);

        Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()));
    }

    /// <summary>Provides validation of the <see cref="ValueLazy{T}.ToString()" /> method.</summary>
    [Test]
    public static void ToStringTest()
    {
        var lazy = new ValueLazy<int>(() => 42);

        Assert.That(lazy.ToString(), Is.EqualTo(string.Empty));

        _ = lazy.Value;

        Assert.That(lazy.ToString(), Is.EqualTo("42"));
    }
}
