// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using NUnit.Framework;

namespace TerraFX.UnitTests;

/// <summary>Provides a set of tests covering the <see cref="Timestamp" /> struct.</summary>
[TestFixture(TestOf = typeof(Timestamp))]
internal static class TimestampTests
{
    /// <summary>Provides validation of the <see cref="Timestamp" /> <c>TicksPer*</c> constants.</summary>
    [Test]
    public static void ConstantsTest()
    {
        Assert.That(() => Timestamp.TicksPerDay, Is.EqualTo(TimeSpan.TicksPerDay));
        Assert.That(() => Timestamp.TicksPerHour, Is.EqualTo(TimeSpan.TicksPerHour));
        Assert.That(() => Timestamp.TicksPerMillisecond, Is.EqualTo(TimeSpan.TicksPerMillisecond));
        Assert.That(() => Timestamp.TicksPerMinute, Is.EqualTo(TimeSpan.TicksPerMinute));
        Assert.That(() => Timestamp.TicksPerSecond, Is.EqualTo(TimeSpan.TicksPerSecond));
    }

    /// <summary>Provides validation of the <see cref="Timestamp(long)" /> constructor and the <see cref="Timestamp.Ticks" /> property.</summary>
    [Test]
    public static void TicksTest()
    {
        Assert.That(() => new Timestamp(0).Ticks, Is.EqualTo(0L));
        Assert.That(() => new Timestamp(1234).Ticks, Is.EqualTo(1234L));
        Assert.That(() => new Timestamp(-1234).Ticks, Is.EqualTo(-1234L));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_Equality" /> method.</summary>
    [Test]
    public static void OpEqualityTest()
    {
        Assert.That(() => new Timestamp(1234) == new Timestamp(1234), Is.True);
        Assert.That(() => new Timestamp(1234) == new Timestamp(5678), Is.False);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_Inequality" /> method.</summary>
    [Test]
    public static void OpInequalityTest()
    {
        // Regression: op_Inequality previously delegated to == and always returned equality.
        Assert.That(() => new Timestamp(1234) != new Timestamp(1234), Is.False);
        Assert.That(() => new Timestamp(1234) != new Timestamp(5678), Is.True);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_GreaterThan" /> method.</summary>
    [Test]
    public static void OpGreaterThanTest()
    {
        Assert.That(() => new Timestamp(5678) > new Timestamp(1234), Is.True);
        Assert.That(() => new Timestamp(1234) > new Timestamp(1234), Is.False);
        Assert.That(() => new Timestamp(1234) > new Timestamp(5678), Is.False);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_GreaterThanOrEqual" /> method.</summary>
    [Test]
    public static void OpGreaterThanOrEqualTest()
    {
        Assert.That(() => new Timestamp(5678) >= new Timestamp(1234), Is.True);
        Assert.That(() => new Timestamp(1234) >= new Timestamp(1234), Is.True);
        Assert.That(() => new Timestamp(1234) >= new Timestamp(5678), Is.False);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_LessThan" /> method.</summary>
    [Test]
    public static void OpLessThanTest()
    {
        Assert.That(() => new Timestamp(1234) < new Timestamp(5678), Is.True);
        Assert.That(() => new Timestamp(1234) < new Timestamp(1234), Is.False);
        Assert.That(() => new Timestamp(5678) < new Timestamp(1234), Is.False);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_LessThanOrEqual" /> method.</summary>
    [Test]
    public static void OpLessThanOrEqualTest()
    {
        Assert.That(() => new Timestamp(1234) <= new Timestamp(5678), Is.True);
        Assert.That(() => new Timestamp(1234) <= new Timestamp(1234), Is.True);
        Assert.That(() => new Timestamp(5678) <= new Timestamp(1234), Is.False);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_UnaryPlus" /> method.</summary>
    [Test]
    public static void OpUnaryPlusTest()
    {
        Assert.That(() => +new Timestamp(1234), Is.EqualTo(new Timestamp(1234)));
        Assert.That(() => +new Timestamp(-1234), Is.EqualTo(new Timestamp(-1234)));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_UnaryNegation" /> method.</summary>
    [Test]
    public static void OpUnaryNegationTest()
    {
        Assert.That(() => -new Timestamp(1234), Is.EqualTo(new Timestamp(-1234)));
        Assert.That(() => -new Timestamp(-1234), Is.EqualTo(new Timestamp(1234)));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.op_Subtraction" /> method.</summary>
    [Test]
    public static void OpSubtractionTest()
    {
        Assert.That(() => new Timestamp(5678) - new Timestamp(1234), Is.EqualTo(new TimeSpan(4444)));
        Assert.That(() => new Timestamp(1234) - new Timestamp(5678), Is.EqualTo(new TimeSpan(-4444)));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.CompareTo(Timestamp)" /> method.</summary>
    [Test]
    public static void CompareToTimestampTest()
    {
        Assert.That(() => new Timestamp(1234).CompareTo(new Timestamp(5678)), Is.Negative);
        Assert.That(() => new Timestamp(1234).CompareTo(new Timestamp(1234)), Is.Zero);
        Assert.That(() => new Timestamp(5678).CompareTo(new Timestamp(1234)), Is.Positive);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.CompareTo(object)" /> method.</summary>
    [Test]
    public static void CompareToObjectTest()
    {
        Assert.That(() => new Timestamp(1234).CompareTo(null), Is.EqualTo(1));
        Assert.That(() => new Timestamp(1234).CompareTo((object)new Timestamp(5678)), Is.Negative);
        Assert.That(() => new Timestamp(1234).CompareTo("not a timestamp"),
            Throws.InstanceOf<ArgumentOutOfRangeException>()
        );
    }

    /// <summary>Provides validation of the <see cref="Timestamp.Equals(Timestamp)" /> and <see cref="Timestamp.Equals(object)" /> methods.</summary>
    [Test]
    public static void EqualsTest()
    {
        Assert.That(() => new Timestamp(1234).Equals(new Timestamp(1234)), Is.True);
        Assert.That(() => new Timestamp(1234).Equals(new Timestamp(5678)), Is.False);

        Assert.That(() => new Timestamp(1234).Equals((object)new Timestamp(1234)), Is.True);
        Assert.That(() => new Timestamp(1234).Equals((object)new Timestamp(5678)), Is.False);
        Assert.That(() => new Timestamp(1234).Equals(null), Is.False);
        Assert.That(() => new Timestamp(1234).Equals("not a timestamp"), Is.False);
    }

    /// <summary>Provides validation of the <see cref="Timestamp.GetHashCode()" /> method.</summary>
    [Test]
    public static void GetHashCodeTest()
    {
        Assert.That(() => new Timestamp(1234).GetHashCode(), Is.EqualTo(new Timestamp(1234).GetHashCode()));
        Assert.That(() => new Timestamp(1234).GetHashCode(), Is.EqualTo(1234L.GetHashCode()));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.ToString(string, IFormatProvider)" /> method.</summary>
    [Test]
    public static void ToStringTest()
    {
        Assert.That(() => new Timestamp(1234).ToString(null, CultureInfo.InvariantCulture), Is.EqualTo("1234"));
        Assert.That(() => new Timestamp(1234).ToString("X", CultureInfo.InvariantCulture), Is.EqualTo("4D2"));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf16Test()
    {
        Span<char> destination = stackalloc char[32];

        var result = new Timestamp(1234).TryFormat(destination, out var charsWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(charsWritten, Is.EqualTo(4));
        Assert.That(destination.Slice(0, charsWritten).ToString(), Is.EqualTo("1234"));
    }

    /// <summary>Provides validation of the <see cref="Timestamp.TryFormat(Span{byte}, out int, ReadOnlySpan{char}, IFormatProvider)" /> method.</summary>
    [Test]
    public static void TryFormatUtf8Test()
    {
        Span<byte> utf8Destination = stackalloc byte[32];

        var result = new Timestamp(1234).TryFormat(utf8Destination, out var bytesWritten, provider: CultureInfo.InvariantCulture);

        Assert.That(result, Is.True);
        Assert.That(bytesWritten, Is.EqualTo(4));
        Assert.That(System.Text.Encoding.UTF8.GetString(utf8Destination.Slice(0, bytesWritten)), Is.EqualTo("1234"));
    }
}
