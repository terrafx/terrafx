// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX
{
    /// <summary>Defines a timestamp.</summary>
    public readonly struct Timestamp : IComparable, IComparable<Timestamp>, IEquatable<Timestamp>, IFormattable
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

        /// <summary>Compares two <see cref="Timestamp" /> values to determine equality.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Timestamp left, Timestamp right) => left._ticks == right._ticks;

        /// <summary>Compares two <see cref="Timestamp" /> values to determine inequality.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Timestamp left, Timestamp right) => left._ticks == right._ticks;

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(Timestamp left, Timestamp right) => left._ticks > right._ticks;

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Timestamp left, Timestamp right) => left._ticks >= right._ticks;

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(Timestamp left, Timestamp right) => left._ticks < right._ticks;

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Timestamp left, Timestamp right) => left._ticks <= right._ticks;

        /// <summary>Subtracts two <see cref="Timestamp" /> values to determine their delta.</summary>
        /// <param name="left">The <see cref="Timestamp" /> from which <paramref name="right" /> will be subtracted.</param>
        /// <param name="right">The <see cref="Timestamp" /> to subtract from <paramref name="left" />.</param>
        /// <returns>The delta of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static TimeSpan operator -(Timestamp left, Timestamp right)
        {
            Assert(left > right, Resources.ArgumentOutOfRangeExceptionMessage, nameof(left), left);
            var deltaTicks = unchecked(left._ticks - right._ticks);
            return new TimeSpan(deltaTicks);
        }

        /// <inheritdoc />
        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }
            else if (obj is Timestamp other)
            {
                return CompareTo(other);
            }
            else
            {
                ThrowArgumentExceptionForInvalidType(obj.GetType(), nameof(obj));
                return 0;
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
}
