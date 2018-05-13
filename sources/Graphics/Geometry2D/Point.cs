// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Graphics.Geometry2D
{
    /// <summary>Defines a point in two-dimensional space.</summary>
    public readonly struct Point : IEquatable<Point>, IFormattable
    {
        #region Fields
        private readonly float _x;
        private readonly float _y;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Point" /> struct.</summary>
        /// <param name="x">The value of the x-coordinate.</param>
        /// <param name="y">The value of the y-coordinate.</param>
        public Point(float x, float y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region Properties
        /// <summary>Gets the value of the x-coordinate.</summary>
        public float X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>Gets the value of the y-coordinate.</summary>
        public float Y
        {
            get
            {
                return _y;
            }
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Point" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Point" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Point" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y);
        }

        /// <summary>Compares two <see cref="Point" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Point" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Point" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y);
        }
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Point" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-coordinate.</param>
        /// <returns>A new <see cref="Point" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Point WithX(float value)
        {
            return new Point(value, Y);
        }

        /// <summary>Creates a new <see cref="Point" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-coordinate.</param>
        /// <returns>A new <see cref="Point" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Point WithY(float value)
        {
            return new Point(X, value);
        }
        #endregion

        #region System.IEquatable<Point2D> Methods
        /// <summary>Compares a <see cref="Point" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Point" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Point other)
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
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            var stringBuilder = new StringBuilder(5 + separator.Length);
            {
                stringBuilder.Append('<');
                stringBuilder.Append(X.ToString(format, formatProvider));
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(Y.ToString(format, formatProvider));
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Point" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Point other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(X.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Y.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, (sizeof(float) * 2));
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
