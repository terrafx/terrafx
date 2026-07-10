// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Advanced;
using TerraFX.Graphics.Advanced;

namespace TerraFX.UnitTests.Graphics.Advanced;

/// <summary>Provides a set of tests covering the <see cref="GraphicsMemoryRegion" /> struct.</summary>
internal static class GraphicsMemoryRegionTests
{
    /// <summary>A trivial device object used to satisfy the allocator's opaque token requirement.</summary>
    private sealed class DummyDeviceObject : IDisposable
    {
        public void Dispose() { }
    }

    private static GraphicsMemoryAllocator CreateAllocator()
    {
        var createOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = 64 * 1024,
        };
        return GraphicsMemoryAllocator.CreateDefault(new DummyDeviceObject(), in createOptions);
    }

    /// <summary>Provides validation that two equal memory regions compare equal.</summary>
    [Test]
    public static void EqualsReturnsTrueForEqualRegionsTest()
    {
        var allocator = CreateAllocator();

        var left = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = allocator,
        };

        var right = left;

        Assert.That(left == right, Is.True);
        Assert.That(left != right, Is.False);
        Assert.That(left.Equals(right), Is.True);
        Assert.That(left.Equals((object)right), Is.True);
        Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()));
    }

    /// <summary>Provides validation that regions differing by <see cref="GraphicsMemoryRegion.ByteAlignment" /> compare not equal.</summary>
    [Test]
    public static void EqualsReturnsFalseForDifferingByteAlignmentTest()
    {
        var allocator = CreateAllocator();

        var left = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = allocator,
        };

        var right = left with { ByteAlignment = 32 };

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
        Assert.That(left.Equals(right), Is.False);
    }

    /// <summary>Provides validation that regions differing by <see cref="GraphicsMemoryRegion.ByteLength" /> compare not equal.</summary>
    [Test]
    public static void EqualsReturnsFalseForDifferingByteLengthTest()
    {
        var allocator = CreateAllocator();

        var left = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = allocator,
        };

        var right = left with { ByteLength = 2048 };

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
        Assert.That(left.Equals(right), Is.False);
    }

    /// <summary>Provides validation that regions differing by <see cref="GraphicsMemoryRegion.ByteOffset" /> compare not equal.</summary>
    [Test]
    public static void EqualsReturnsFalseForDifferingByteOffsetTest()
    {
        var allocator = CreateAllocator();

        var left = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = allocator,
        };

        var right = left with { ByteOffset = 0 };

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
        Assert.That(left.Equals(right), Is.False);
    }

    /// <summary>Provides validation that regions differing by <see cref="GraphicsMemoryRegion.IsAllocated" /> compare not equal.</summary>
    [Test]
    public static void EqualsReturnsFalseForDifferingIsAllocatedTest()
    {
        var allocator = CreateAllocator();

        var left = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = allocator,
        };

        var right = left with { IsAllocated = false };

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
        Assert.That(left.Equals(right), Is.False);
    }

    /// <summary>Provides validation that regions differing by <see cref="GraphicsMemoryRegion.MemoryAllocator" /> compare not equal.</summary>
    [Test]
    public static void EqualsReturnsFalseForDifferingMemoryAllocatorTest()
    {
        var left = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = CreateAllocator(),
        };

        var right = left with { MemoryAllocator = CreateAllocator() };

        Assert.That(left == right, Is.False);
        Assert.That(left != right, Is.True);
        Assert.That(left.Equals(right), Is.False);
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryRegion.Equals(object)" /> returns <c>false</c> for a non-region value.</summary>
    [Test]
    public static void EqualsReturnsFalseForNonRegionTest()
    {
        var region = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = CreateAllocator(),
        };

        Assert.That(region.Equals(null), Is.False);
        Assert.That(region.Equals("not a region"), Is.False);
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryRegion.Equals(GraphicsMemoryRegion)" /> is null-safe for regions with a null allocator (e.g. the <c>default</c> sentinel).</summary>
    [Test]
    public static void EqualsIsNullSafeForDefaultRegionTest()
    {
        var region = new GraphicsMemoryRegion {
            ByteAlignment = 16,
            ByteLength = 1024,
            ByteOffset = 512,
            IsAllocated = true,
            MemoryAllocator = CreateAllocator(),
        };

        // A default region has a null MemoryAllocator; comparing it must not throw.
        Assert.That(default(GraphicsMemoryRegion).Equals(default), Is.True);
        Assert.That(default(GraphicsMemoryRegion).Equals(region), Is.False);
        Assert.That(region.Equals(default), Is.False);
    }
}
