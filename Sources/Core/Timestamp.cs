// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX
{
    /// <summary>Defines a timestamp.</summary>
    public /* blittable */ struct Timestamp : IComparable, IComparable<Timestamp>, IEquatable<Timestamp>, IFormattable
    {
        #region Constants
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
        #endregion

        #region Fields
        internal readonly long _ticks;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Timestamp" /> struct.</summary>
        /// <param name="ticks">The number of 100-nanosecond ticks that will be represented by the instance.</param>
        public Timestamp(long ticks)
        {
            _ticks = ticks;
        }
        #endregion

        #region Properties
        /// <summary>Gets the number of 100-nanosecond ticks represented by the current instance.</summary>
        public long Ticks
        {
            get
            {
                return _ticks;
            }
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Timestamp" /> values to determine equality.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Timestamp left, Timestamp right)
        {
            return (left._ticks == right._ticks);
        }

        /// <summary>Compares two <see cref="Timestamp" /> values to determine inequality.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Timestamp left, Timestamp right)
        {
            return (left._ticks == right._ticks);
        }

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >(Timestamp left, Timestamp right)
        {
            return (left._ticks > right._ticks);
        }

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is greater than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Timestamp left, Timestamp right)
        {
            return (left._ticks >= right._ticks);
        }

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <(Timestamp left, Timestamp right)
        {
            return (left._ticks < right._ticks);
        }

        /// <summary>Compares two <see cref="Timestamp" /> values to determine relative sort-order.</summary>
        /// <param name="left">The <see cref="Timestamp" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Timestamp" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> is less than or equal to <paramref name="right" />; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Timestamp left, Timestamp right)
        {
            return (left._ticks <= right._ticks);
        }
        #endregion

        #region Binary Operators
        /// <summary>Subtracts two <see cref="Timestamp" /> values to determine their delta.</summary>
        /// <param name="left">The <see cref="Timestamp" /> from which <paramref name="right" /> will be subtracted.</param>
        /// <param name="right">The <see cref="Timestamp" /> to subtract from <paramref name="left" />.</param>
        /// <returns>The delta of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static TimeSpan operator -(Timestamp left, Timestamp right)
        {
            Debug.Assert(left > right);
            var deltaTicks = unchecked(left._ticks - right._ticks);
            return new TimeSpan(deltaTicks);
        }
        #endregion

        #region System.IComparable Methods
        /// <summary>Compares an <see cref="object" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="obj" /> is greater than the current instance, <c>zero</c> if <paramref name="obj"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="obj" /> is <c>null</c> or greater than the current instance.</returns>
        /// <exception cref="ArgumentException"><paramref name="obj" /> is not <c>null</c> and is not an instance of <see cref="Timestamp" />.</exception>
        public int CompareTo(object obj)
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
                throw NewArgumentExceptionForInvalidType(nameof(obj), obj.GetType());
            }
        }
        #endregion

        #region System.IComparable<Timestamp> Methods
        /// <summary>Compares a <see cref="Timestamp" /> with the current instance to determine relative sort-order.</summary>
        /// <param name="other">The <see cref="Timestamp" /> to compare with the current instance.</param>
        /// <returns>A value <c>less than zero</c> if <paramref name="other" /> is greater than the current instance, <c>zero</c> if <paramref name="other"/> is equal to the current instance; and <c>greater than zero</c> if <paramref name="other" /> is greater than the current instance.</returns>
        public int CompareTo(Timestamp other)
        {
            // We have to actually compare because subtraction
            // causes wrapping for very large negative numbers.

            if (this < other)
            {
                return -1;
            }
            else if (this > other)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region System.IEquatable<Timestamp> Methods
        /// <summary>Compares a <see cref="Timestamp" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Timestamp" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Timestamp other)
        {
            return (this == other);
        }
        #endregion

        #region System.IFormattable Methods
        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _ticks.ToString(format, formatProvider);
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares an <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Timestamp" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Timestamp other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return _ticks.GetHashCode();
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString()
        {
            return ToString(format: null, formatProvider: null);
        }
        #endregion
    }
}
