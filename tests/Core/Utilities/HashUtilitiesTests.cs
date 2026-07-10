// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="HashUtilities" /> static class.</summary>
internal static class HashUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="HashUtilities.IsPrime(int)" /> method.</summary>
    [TestCase(2, true)]
    [TestCase(3, true)]
    [TestCase(5, true)]
    [TestCase(7, true)]
    [TestCase(11, true)]
    [TestCase(101, true)]
    [TestCase(4, false)]
    [TestCase(6, false)]
    [TestCase(8, false)]
    [TestCase(9, false)]
    [TestCase(15, false)]
    [TestCase(100, false)]
    public static void IsPrimeTest(int candidate, bool expected)
    {
        Assert.That(HashUtilities.IsPrime(candidate), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="HashUtilities.GetPrime(int)" /> method.</summary>
    [TestCase(0, 3)]
    [TestCase(3, 3)]
    [TestCase(4, 7)]
    [TestCase(8, 11)]
    [TestCase(12, 17)]
    public static void GetPrimeTest(int min, int expected)
    {
        Assert.That(HashUtilities.GetPrime(min), Is.EqualTo(expected));
    }

    /// <summary>Provides validation that <see cref="HashUtilities.GetPrime(int)" /> rejects a negative minimum.</summary>
    [Test]
    public static void GetPrimeNegativeThrowsTest()
    {
        Assert.That(() => HashUtilities.GetPrime(-1),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
                  .And.Property("ParamName").EqualTo("min")
        );
    }

    /// <summary>Provides validation of the <see cref="HashUtilities.ExpandPrime(int)" /> method.</summary>
    [TestCase(3, 7)]
    [TestCase(7, 17)]
    [TestCase(11, 23)]
    public static void ExpandPrimeTest(int oldSize, int expected)
    {
        // ExpandPrime doubles the size then rounds up to the next prime.
        Assert.That(HashUtilities.ExpandPrime(oldSize), Is.EqualTo(expected));
    }

    /// <summary>Provides validation of the <see cref="HashUtilities.GetFastModMultiplier(uint)" /> method.</summary>
    [TestCase(3u)]
    [TestCase(7u)]
    [TestCase(101u)]
    public static void GetFastModMultiplierTest(uint divisor)
    {
        Assert.That(HashUtilities.GetFastModMultiplier(divisor), Is.EqualTo((ulong.MaxValue / divisor) + 1));
    }

    /// <summary>Provides validation that <see cref="HashUtilities.FastMod(uint, uint, ulong)" /> matches the built-in modulo operator.</summary>
    [TestCase(0u, 7u)]
    [TestCase(1u, 7u)]
    [TestCase(6u, 7u)]
    [TestCase(7u, 7u)]
    [TestCase(8u, 7u)]
    [TestCase(100u, 11u)]
    [TestCase(1000u, 13u)]
    [TestCase(uint.MaxValue, 97u)]
    public static void FastModTest(uint value, uint divisor)
    {
        var multiplier = HashUtilities.GetFastModMultiplier(divisor);
        Assert.That(HashUtilities.FastMod(value, divisor, multiplier), Is.EqualTo(value % divisor));
    }
}
