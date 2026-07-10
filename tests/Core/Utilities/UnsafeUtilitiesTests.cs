// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="UnsafeUtilities" /> static class.</summary>
internal static unsafe class UnsafeUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="UnsafeUtilities.SizeOf{T}()" /> method.</summary>
    [Test]
    public static void SizeOfTest()
    {
        Assert.That(UnsafeUtilities.SizeOf<byte>(), Is.EqualTo(1u));
        Assert.That(UnsafeUtilities.SizeOf<int>(), Is.EqualTo(4u));
        Assert.That(UnsafeUtilities.SizeOf<double>(), Is.EqualTo(8u));
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.As{T}(object)" /> method.</summary>
    [Test]
    public static void AsObjectTest()
    {
        object value = "hello";

        Assert.That(value.As<string>(), Is.SameAs(value));
        Assert.That(((object?)null).As<string>(), Is.Null);
    }

    /// <summary>Provides validation that <see cref="UnsafeUtilities.As{TFrom, TTo}(Span{TFrom})" /> reinterprets while preserving the element count.</summary>
    [Test]
    public static void AsSpanReinterpretTest()
    {
        Span<int> source = [1, 2, 3, 4];

        var reinterpreted = source.As<int, uint>();

        Assert.That(reinterpreted.Length, Is.EqualTo(source.Length));
        Assert.That(reinterpreted[0], Is.EqualTo(1u));

        reinterpreted[0] = 42;
        Assert.That(source[0], Is.EqualTo(42));
    }

    /// <summary>Provides validation that <see cref="UnsafeUtilities.Cast{TFrom, TTo}(Span{TFrom})" /> adjusts the element count by the size ratio.</summary>
    [Test]
    public static void CastTest()
    {
        Span<int> source = [1, 2, 3, 4];

        var bytes = source.Cast<int, byte>();

        Assert.That(bytes.Length, Is.EqualTo(source.Length * sizeof(int)));
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.AsSpan{T}(ReadOnlySpan{T})" /> method.</summary>
    [Test]
    public static void AsSpanTest()
    {
        Span<int> backing = [1, 2, 3];
        ReadOnlySpan<int> readOnly = backing;

        var writable = readOnly.AsSpan();
        writable[0] = 42;

        Assert.That(backing[0], Is.EqualTo(42));
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.NullRef{T}()" />, <see cref="UnsafeUtilities.IsNullRef{T}(ref readonly T)" />, and <see cref="UnsafeUtilities.IsNotNullRef{T}(in T)" /> methods.</summary>
    [Test]
    public static void NullRefTest()
    {
        Assert.That(UnsafeUtilities.IsNullRef(in UnsafeUtilities.NullRef<int>()), Is.True);

        var value = 5;
        Assert.That(UnsafeUtilities.IsNullRef(in value), Is.False);
        Assert.That(UnsafeUtilities.IsNotNullRef(in value), Is.True);
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.CreateSpan{T}(ref T, int)" /> method.</summary>
    [Test]
    public static void CreateSpanTest()
    {
        var value = 5;

        var span = UnsafeUtilities.CreateSpan(ref value, 1);

        Assert.That(span.Length, Is.EqualTo(1));
        Assert.That(span[0], Is.EqualTo(5));

        span[0] = 9;
        Assert.That(value, Is.EqualTo(9));
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.GetReferenceUnsafe{T}(T[], int)" /> method.</summary>
    [Test]
    public static void GetReferenceUnsafeArrayTest()
    {
        var array = new[] { 1, 2, 3 };

        ref var reference = ref array.GetReferenceUnsafe(1);

        Assert.That(reference, Is.EqualTo(2));

        reference = 9;
        Assert.That(array[1], Is.EqualTo(9));
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.ReadUnaligned{T}(void*)" /> and <see cref="UnsafeUtilities.WriteUnaligned{T}(void*, T)" /> methods.</summary>
    [Test]
    public static void ReadWriteUnalignedTest()
    {
        var buffer = stackalloc byte[8];

        UnsafeUtilities.WriteUnaligned<int>(buffer, 42);
        Assert.That(UnsafeUtilities.ReadUnaligned<int>(buffer), Is.EqualTo(42));

        UnsafeUtilities.WriteUnaligned<int>(buffer, (nuint)sizeof(int), 99);
        Assert.That(UnsafeUtilities.ReadUnaligned<int>(buffer, (nuint)sizeof(int)), Is.EqualTo(99));
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.CopyBlock{TDestination, TSource}(ref TDestination, ref readonly TSource, uint)" /> method.</summary>
    [Test]
    public static void CopyBlockTest()
    {
        Span<int> source = [1, 2, 3, 4];
        Span<int> destination = stackalloc int[4];

        UnsafeUtilities.CopyBlock(ref destination.GetReferenceUnsafe(), in source.GetReferenceUnsafe(), UnsafeUtilities.SizeOf<int>() * 4);

        Assert.That(destination.SequenceEqual(source), Is.True);
    }

    /// <summary>Provides validation of the <see cref="UnsafeUtilities.ToUnmanagedArray{T}(Span{T}, nuint)" /> method.</summary>
    [Test]
    public static void ToUnmanagedArrayTest()
    {
        Span<int> source = [1, 2, 3];

        var array = source.ToUnmanagedArray();

        Assert.That(array.Length, Is.EqualTo((nuint)3));
        Assert.That(array[0], Is.EqualTo(1));
        Assert.That(array[1], Is.EqualTo(2));
        Assert.That(array[2], Is.EqualTo(3));

        array.Dispose();
    }
}
