// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using NUnit.Framework;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="MemoryUtilities" /> static class.</summary>
[TestFixture(TestOf = typeof(MemoryUtilities))]
internal static unsafe class MemoryUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="MemoryUtilities.CopyUnsafe(void*, void*, nuint)" /> method.</summary>
    /// <remarks>
    ///     The lengths intentionally cover every copy path: the <c>SmallCopy</c> switch (0-32), the
    ///     trailing block copies (multiples of 32) plus a sub-32 remainder, and the 128-byte block loop
    ///     with a trailing remainder. The <c>32 * n + r</c> sizes (e.g. 40, 48, 80) are the regression
    ///     cases where the trailing remainder was previously copied from the wrong offset.
    /// </remarks>
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(8)]
    [TestCase(15)]
    [TestCase(16)]
    [TestCase(17)]
    [TestCase(31)]
    [TestCase(32)]
    [TestCase(33)]
    [TestCase(40)]
    [TestCase(47)]
    [TestCase(48)]
    [TestCase(63)]
    [TestCase(64)]
    [TestCase(65)]
    [TestCase(80)]
    [TestCase(95)]
    [TestCase(96)]
    [TestCase(97)]
    [TestCase(111)]
    [TestCase(112)]
    [TestCase(127)]
    [TestCase(128)]
    [TestCase(129)]
    [TestCase(160)]
    [TestCase(176)]
    [TestCase(192)]
    [TestCase(255)]
    public static void CopyUnsafeTest(int length)
    {
        const int MaxLength = 256;

        var source = stackalloc byte[MaxLength];
        var destination = stackalloc byte[MaxLength];

        for (var i = 0; i < MaxLength; i++)
        {
            source[i] = unchecked((byte)(i + 1));
            destination[i] = 0;
        }

        MemoryUtilities.CopyUnsafe(destination, source, (nuint)length);

        for (var i = 0; i < length; i++)
        {
            Assert.That(destination[i], Is.EqualTo(source[i]), $"byte {i} was not copied");
        }

        for (var i = length; i < MaxLength; i++)
        {
            Assert.That(destination[i], Is.EqualTo((byte)0), $"byte {i} was written past the requested length");
        }
    }
}
