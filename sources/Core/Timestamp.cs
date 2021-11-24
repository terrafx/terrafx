// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX;

/// <summary>Defines a timestamp.</summary>
public readonly struct Timestamp : IComparable<Timestamp>, IEquatable<Timestamp>, IComparable, IFormattable
{
    /// <summary>The number of ticks that occur per day.</summary>
    public const long TicksPerDay = TimeSpan.TicksPerDay;

    /// <summary>The number of ticks that occur per hour.</summary>
    public const long TicksPerHour = TimeSpan.TicksPerHour;

    /// <summary>The number of ticks that occur per millisecond.</summary>
    public const long TicksPerMillisecond = TimeSpan.TicksPerMillisecond;

    /// <summary>The number of ticks that occur per minute.</summary>
    public const long TicksPerMinute = TimeSpan.TicksPerMinute;

    /// <summary>The number of ticks that occur per second.</summary>
    public const long TicksPerSecond = TimeSpan.TicksPerSecond;

    private readonly long _ticks;

    /// <summary>Initializes a new instance of the <see cref="Timestamp" /> struct.</summary>
    /// <param name="ticks">The number of 100-nanosecond ticks represented by the instance.</param>
    public Timestamp(long ticks)
    {
        _ticks = ticks;
    }

    /// <summary>Gets the number of 100-nanosecond ticks represented by the current instance.</summary>
    public long Ticks => _ticks;

    /// <summary>Compares two timestamps to determine equality.</summary>
    /// <param name="left">The timestamp to compare with <paramref name="right" />.</param>
    /// <param name="right">The timestamp to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Timestamp left, Timestamp right) => left._ticks == right._ticks;

    /// <summary>Compares two timestamps to determine inequality.</summary>
    /// <param name="left">The timestamp to compare with <paramref name="right" />.</param>
    /// <param name="right">The timestamp to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Timestamp left, Timestamp right) => left._ticks == right._ticks;

    /// <summary>Compares two timestamps to determine relative sort-order.</summary>
    /// <param name="left">The timestamp to compare with <paramref name="right" />.</param>
    /// <param name="right">The timestamp to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
    public static bool operator >(Timestamp left, Timestamp right) => left._ticks > right._ticks;

    /// <summary>Compares two timestamps to determine relative sort-order.</summary>
    /// <param name="left">The timestamp to compare with <paramref name="right" />.</param>
    /// <param name="right">The timestamp to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
    public static bool operator >=(Timestamp left, Timestamp right) => left._ticks >= right._ticks;

    /// <summary>Compares two timestamps to determine relative sort-order.</summary>
    /// <param name="left">The timestamp to compare with <paramref name="right" />.</param>
    /// <param name="right">The timestamp to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
    public static bool operator <(Timestamp left, Timestamp right) => left._ticks < right._ticks;

    /// <summary>Compares two timestamps to determine relative sort-order.</summary>
    /// <param name="left">The timestamp to compare with <paramref name="right" />.</param>
    /// <param name="right">The timestamp to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
    public static bool operator <=(Timestamp left, Timestamp right) => left._ticks <= right._ticks;

    /// <summary>Computes the value of a quaternion.</summary>
    /// <param name="value">The quaternion.</param>
    /// <returns><paramref name="value" /></returns>
    public static Timestamp operator +(Timestamp value) => value;

    /// <summary>Computes the negation of a timestamp.</summary>
    /// <param name="value">The timestamp to negate.</param>
    /// <returns>The negation of <paramref name="value" />.</returns>
    public static Timestamp operator -(Timestamp value) => new Timestamp(-value._ticks);

    /// <summary>Computes the sum of two timestamps.</summary>
    /// <param name="left">The timestamp to which to add <paramref name="right" />.</param>
    /// <param name="right">The timestamp which is added to <paramref name="left" />.</param>
    /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
    public static TimeSpan operator +(Timestamp left, Timestamp right)
    {
        var deltaTicks = unchecked(left._ticks + right._ticks);
        return new TimeSpan(deltaTicks);
    }

    /// <summary>Computes the difference of two timestamps.</summary>
    /// <param name="left">The timestamp from which to subtract <paramref name="right" />.</param>
    /// <param name="right">The timestamp which is subtracted from <paramref name="left" />.</param>
    /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
    public static TimeSpan operator -(Timestamp left, Timestamp right)
    {
        var deltaTicks = unchecked(left._ticks - right._ticks);
        return new TimeSpan(deltaTicks);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is Timestamp other)
        {
            return CompareTo(other);
        }
        else
        {
            if (obj is not null)
            {
                ThrowForInvalidType(obj.GetType(), nameof(obj), typeof(Timestamp));
            }
            return 1;
        }
    }

    /// <inheritdoc />
    public int CompareTo(Timestamp other) => _ticks.CompareTo(other._ticks);

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Timestamp other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Timestamp other) => _ticks.Equals(other._ticks);

    /// <inheritdoc />
    public override int GetHashCode() => _ticks.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => _ticks.ToString();

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider) => _ticks.ToString(format, formatProvider);
}
