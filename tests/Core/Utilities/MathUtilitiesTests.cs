// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using NUnit.Framework;
using TerraFX.Utilities;

namespace TerraFX.UnitTests.Utilities;

/// <summary>Provides a set of tests covering the <see cref="MathUtilities" /> static class.</summary>
[TestFixture(TestOf = typeof(MathUtilities))]
public static class MathUtilitiesTests
{
    /// <summary>Provides validation of the <see cref="MathUtilities.Max(double, double)" /> method.</summary>
    [TestCase(-5.0, +5.0, +5.0)]
    [TestCase(+5.0, -5.0, +5.0)]
    [TestCase(-0.0, +0.0, +0.0)]
    [TestCase(double.NaN, 1.0, double.NaN)]
    [TestCase(1.0, double.NaN, double.NaN)]
    public static void MaxTest(double left, double right, double expected)
    {
        Assert.That(() => BitConverter.DoubleToUInt64Bits(MathUtilities.Max(left, right)),
            Is.EqualTo(BitConverter.DoubleToUInt64Bits(expected))
        );
    }

    /// <summary>Provides validation of the <see cref="MathUtilities.Max(float, float)" /> method.</summary>
    [TestCase(-5.0f, +5.0f, +5.0f)]
    [TestCase(+5.0f, -5.0f, +5.0f)]
    [TestCase(-0.0f, +0.0f, +0.0f)]
    [TestCase(float.NaN, 1.0f, float.NaN)]
    [TestCase(1.0f, float.NaN, float.NaN)]
    public static void MaxTest(float left, float right, float expected)
    {
        Assert.That(() => BitConverter.SingleToUInt32Bits(MathUtilities.Max(left, right)),
            Is.EqualTo(BitConverter.SingleToUInt32Bits(expected))
        );
    }

    /// <summary>Provides validation of the <see cref="MathUtilities.Min(double, double)" /> method.</summary>
    [TestCase(-5.0, +5.0, -5.0)]
    [TestCase(+5.0, -5.0, -5.0)]
    [TestCase(-0.0, +0.0, -0.0)]
    [TestCase(double.NaN, 1.0, double.NaN)]
    [TestCase(1.0, double.NaN, double.NaN)]
    public static void MinTest(double left, double right, double expected)
    {
        Assert.That(() => BitConverter.DoubleToUInt64Bits(MathUtilities.Min(left, right)),
            Is.EqualTo(BitConverter.DoubleToUInt64Bits(expected))
        );
    }

    /// <summary>Provides validation of the <see cref="MathUtilities.Min(float, float)" /> method.</summary>
    [TestCase(-5.0f, +5.0f, -5.0f)]
    [TestCase(+5.0f, -5.0f, -5.0f)]
    [TestCase(-0.0f, +0.0f, -0.0f)]
    [TestCase(float.NaN, 1.0f, float.NaN)]
    [TestCase(1.0f, float.NaN, float.NaN)]
    public static void MinTest(float left, float right, float expected)
    {
        Assert.That(() => BitConverter.SingleToUInt32Bits(MathUtilities.Min(left, right)),
            Is.EqualTo(BitConverter.SingleToUInt32Bits(expected))
        );
    }
}
