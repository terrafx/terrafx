// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using TerraFX.Advanced;
using TerraFX.Graphics.Advanced;

namespace TerraFX.UnitTests.Graphics.Advanced;

/// <summary>Provides a set of tests covering the <see cref="GraphicsMemoryAllocator" /> class.</summary>
internal static unsafe class GraphicsMemoryAllocatorTests
{
    // Work in units well above MinimumFreeMemoryRegionByteLengthToRegister (4096) so that
    // free regions are always registered and reuse/coalescing behave predictably.
    private const uint SixtyFourKB = 64 * 1024;

    private static int s_onFreeCount;

    /// <summary>A trivial device object used to satisfy the allocator's opaque token requirement.</summary>
    private sealed class DummyDeviceObject : IDisposable
    {
        public void Dispose() { }
    }

    private static GraphicsMemoryAllocator CreateAllocator(nuint byteLength, bool isDedicated = false, GraphicsMemoryAllocatorOnFreeCallback onFree = default)
    {
        var createOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = byteLength,
            IsDedicated = isDedicated,
            OnFree = onFree,
        };
        return GraphicsMemoryAllocator.CreateDefault(new DummyDeviceObject(), in createOptions);
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryAllocator.CreateDefault(IDisposable, in GraphicsMemoryAllocatorCreateOptions)" /> establishes the expected initial state.</summary>
    [Test]
    public static void CreateDefaultEstablishesInitialStateTest()
    {
        var byteLength = (nuint)(4 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength, isDedicated: true);

        Assert.That(allocator.ByteLength, Is.EqualTo(byteLength));
        Assert.That(allocator.IsEmpty, Is.True);
        Assert.That(allocator.IsDedicated, Is.True);
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo(byteLength));
        Assert.That(allocator.TotalAllocatedMemoryRegionByteLength, Is.EqualTo((nuint)0));
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo(byteLength));
        Assert.That(allocator.DeviceObject, Is.Not.Null);
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryAllocator.IsDedicated" /> reflects the create option.</summary>
    [Test]
    public static void CreateDefaultNonDedicatedTest()
    {
        var allocator = CreateAllocator(4 * SixtyFourKB, isDedicated: false);
        Assert.That(allocator.IsDedicated, Is.False);
    }

    /// <summary>Provides validation that creating an allocator with a zero <see cref="GraphicsMemoryAllocatorCreateOptions.ByteLength" /> throws.</summary>
    [Test]
    public static void CreateDefaultZeroByteLengthThrowsTest()
    {
        var createOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = 0,
        };

        Assert.That(() => GraphicsMemoryAllocator.CreateDefault(new DummyDeviceObject(), in createOptions),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    /// <summary>Provides validation that a simple allocation updates the region and allocator totals.</summary>
    [Test]
    public static void AllocateUpdatesStateTest()
    {
        var byteLength = (nuint)(4 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength);

        var region = allocator.Allocate(SixtyFourKB);

        Assert.That(region.IsAllocated, Is.True);
        Assert.That(region.ByteLength, Is.EqualTo((nuint)SixtyFourKB));
        Assert.That(region.MemoryAllocator, Is.SameAs(allocator));
        Assert.That(allocator.TotalAllocatedMemoryRegionByteLength, Is.EqualTo((nuint)SixtyFourKB));
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo(byteLength - SixtyFourKB));
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo(byteLength - SixtyFourKB));
    }

    /// <summary>Provides validation that an aligned allocation produces an aligned offset, exercising the padding-begin split path.</summary>
    [Test]
    public static void AllocateWithAlignmentProducesAlignedOffsetTest()
    {
        var allocator = CreateAllocator(8 * SixtyFourKB);

        // Allocate a small region first so the following free region no longer starts on the requested alignment boundary.
        _ = allocator.Allocate(4096);

        var alignment = (nuint)SixtyFourKB;
        var region = allocator.Allocate(SixtyFourKB, alignment);

        Assert.That(region.ByteAlignment, Is.EqualTo(alignment));
        Assert.That(region.ByteOffset % alignment, Is.EqualTo((nuint)0));
        Assert.That(region.ByteOffset, Is.GreaterThanOrEqualTo((nuint)4096));
    }

    /// <summary>Provides validation that the default alignment is applied when <c>zero</c> is requested.</summary>
    [Test]
    public static void AllocateWithDefaultAlignmentTest()
    {
        var allocator = CreateAllocator(4 * SixtyFourKB);

        var region = allocator.Allocate(SixtyFourKB);

        // Runtime.Configuration.DefaultAlignment defaults to 16.
        Assert.That(region.ByteAlignment, Is.EqualTo((nuint)16));
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryAllocator.TryAllocate(nuint, nuint, out GraphicsMemoryRegion)" /> fails cleanly when the request exceeds the managed length.</summary>
    [Test]
    public static void TryAllocateLargerThanByteLengthReturnsFalseTest()
    {
        var byteLength = (nuint)(2 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength);

        var result = allocator.TryAllocate(4 * SixtyFourKB, 0, out var region);

        Assert.That(result, Is.False);
        Assert.That(region == default, Is.True);
        Assert.That(allocator.IsEmpty, Is.True);
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo(byteLength));
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryAllocator.Allocate(nuint, nuint)" /> throws when the request exceeds the managed length.</summary>
    [Test]
    public static void AllocateLargerThanByteLengthThrowsTest()
    {
        var allocator = CreateAllocator(2 * SixtyFourKB);

        Assert.That(() => allocator.Allocate(4 * SixtyFourKB),
            Throws.InstanceOf<OutOfMemoryException>()
        );
    }

    /// <summary>Provides validation that a zero byte length throws.</summary>
    [Test]
    public static void TryAllocateZeroByteLengthThrowsTest()
    {
        var allocator = CreateAllocator(4 * SixtyFourKB);

        Assert.That(() => allocator.TryAllocate(0, 0, out _),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    /// <summary>Provides validation that a non-power-of-two alignment throws.</summary>
    [Test]
    public static void TryAllocateNonPow2AlignmentThrowsTest()
    {
        var allocator = CreateAllocator(4 * SixtyFourKB);

        Assert.That(() => allocator.TryAllocate(SixtyFourKB, 3, out _),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    /// <summary>Provides validation that freeing an allocation restores the allocator totals and leaves a single reusable region.</summary>
    [Test]
    public static void AllocateThenFreeRestoresStateTest()
    {
        var byteLength = (nuint)(4 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength);

        var region = allocator.Allocate(SixtyFourKB);
        allocator.Free(in region);

        Assert.That(allocator.IsEmpty, Is.True);
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo(byteLength));
        Assert.That(allocator.TotalAllocatedMemoryRegionByteLength, Is.EqualTo((nuint)0));
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo(byteLength));

        // The whole range is available again as a single contiguous region.
        Assert.That(allocator.TryAllocate(byteLength, 0, out _), Is.True);
    }

    /// <summary>Provides validation that freeing neighbors coalesces free regions back into a single span.</summary>
    [Test]
    public static void FreeCoalescesAdjacentRegionsTest()
    {
        var byteLength = (nuint)(3 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength);

        var regionA = allocator.Allocate(SixtyFourKB);
        var regionB = allocator.Allocate(SixtyFourKB);
        var regionC = allocator.Allocate(SixtyFourKB);

        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo((nuint)0));

        // Free the middle first, then the two neighbors; all three should merge back into one region.
        allocator.Free(in regionB);
        allocator.Free(in regionA);
        allocator.Free(in regionC);

        Assert.That(allocator.IsEmpty, Is.True);
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo(byteLength));
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo(byteLength));

        // The three freed regions coalesced: a single allocation spanning the entire range now succeeds.
        Assert.That(allocator.TryAllocate(byteLength, 0, out _), Is.True);
    }

    /// <summary>Provides validation that non-adjacent free regions remain distinct and are not treated as a single span.</summary>
    [Test]
    public static void FreeNonAdjacentRegionsRemainFragmentedTest()
    {
        var byteLength = (nuint)(4 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength);

        var regionA = allocator.Allocate(SixtyFourKB);          // [0, 64K)
        var regionB = allocator.Allocate(SixtyFourKB);          // [64K, 128K)
        var regionC = allocator.Allocate(2 * SixtyFourKB);      // [128K, 256K)

        // Free the first (64K) and the last (128K), leaving B allocated between them.
        allocator.Free(in regionA);
        allocator.Free(in regionC);

        Assert.That(allocator.IsEmpty, Is.False);
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo((nuint)(3 * SixtyFourKB)));

        // The gaps are distinct (64K and 128K) and must not be treated as one 192K span: the largest
        // allocatable region is the larger gap, not the combined free length.
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo((nuint)(2 * SixtyFourKB)));
        Assert.That(allocator.TryAllocate(3 * SixtyFourKB, 0, out _), Is.False);
        Assert.That(allocator.TryAllocate(2 * SixtyFourKB, 0, out _), Is.True);

        GC.KeepAlive(regionB);
    }

    /// <summary>Provides validation that freeing a region not produced by the allocator throws.</summary>
    [Test]
    public static void FreeUnknownRegionThrowsTest()
    {
        var allocator = CreateAllocator(4 * SixtyFourKB);

        var foreignRegion = new GraphicsMemoryRegion {
            ByteAlignment = 1,
            ByteLength = SixtyFourKB,
            ByteOffset = 16 * SixtyFourKB,
            IsAllocated = true,
            MemoryAllocator = allocator,
        };

        Assert.That(() => allocator.Free(in foreignRegion),
            Throws.InstanceOf<KeyNotFoundException>()
        );
    }

    /// <summary>Provides validation that <see cref="GraphicsMemoryAllocator.Clear" /> resets the allocator to a single free region.</summary>
    [Test]
    public static void ClearResetsAllocatorTest()
    {
        var byteLength = (nuint)(4 * SixtyFourKB);
        var allocator = CreateAllocator(byteLength);

        _ = allocator.Allocate(SixtyFourKB);
        _ = allocator.Allocate(SixtyFourKB);

        allocator.Clear();

        Assert.That(allocator.IsEmpty, Is.True);
        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo(byteLength));
        Assert.That(allocator.TotalAllocatedMemoryRegionByteLength, Is.EqualTo((nuint)0));
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo(byteLength));

        // The entire range is once again available as a single contiguous region.
        Assert.That(allocator.TryAllocate(byteLength, 0, out _), Is.True);
    }

    /// <summary>Provides validation that an exhausted allocator rejects further allocations.</summary>
    [Test]
    public static void ExhaustionRejectsFurtherAllocationsTest()
    {
        var allocator = CreateAllocator(2 * SixtyFourKB);

        Assert.That(allocator.TryAllocate(SixtyFourKB, 0, out _), Is.True);
        Assert.That(allocator.TryAllocate(SixtyFourKB, 0, out _), Is.True);

        Assert.That(allocator.TotalFreeMemoryRegionByteLength, Is.EqualTo((nuint)0));
        Assert.That(allocator.LargestFreeMemoryRegionByteLength, Is.EqualTo((nuint)0));
        Assert.That(allocator.TryAllocate(SixtyFourKB, 0, out var region), Is.False);
        Assert.That(region == default, Is.True);
    }

    /// <summary>Provides validation that the <see cref="GraphicsMemoryAllocatorCreateOptions.OnFree" /> callback is invoked when a region is freed.</summary>
    [Test]
    public static void OnFreeCallbackIsInvokedTest()
    {
        s_onFreeCount = 0;

        var onFree = new GraphicsMemoryAllocatorOnFreeCallback(&OnFreeHandler);
        var allocator = CreateAllocator(4 * SixtyFourKB, onFree: onFree);

        var region = allocator.Allocate(SixtyFourKB);
        Assert.That(s_onFreeCount, Is.EqualTo(0));

        allocator.Free(in region);
        Assert.That(s_onFreeCount, Is.EqualTo(1));
    }

    private static void OnFreeHandler(in GraphicsMemoryRegion memoryRegion)
    {
        s_onFreeCount++;
    }
}
