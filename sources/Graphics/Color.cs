// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Graphics
{
    /// <summary>Defines a red-green-blue color triple.</summary>
    public readonly struct Color : IEquatable<Color>, IFormattable
    {
        #region Fields
        private readonly float _red;
        private readonly float _green;
        private readonly float _blue;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Color" /> struct.</summary>
        /// <param name="red">The value of the red component.</param>
        /// <param name="green">The value of the green component.</param>
        /// <param name="blue">The value of the blue component.</param>
        public Color(float red, float green, float blue)
        {
            _red = red;
            _green = green;
            _blue = blue;
        }
        #endregion

        #region Properties
        /// <summary>Gets the value of the blue component.</summary>
        public float Blue
        {
            get
            {
                return _blue;
            }
        }

        /// <summary>Gets the value of the green component.</summary>
        public float Green
        {
            get
            {
                return _green;
            }
        }

        /// <summary>Gets the value of the red component.</summary>
        public float Red
        {
            get
            {
                return _red;
            }
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Color" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Color" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Color" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Color left, Color right)
        {
            return (left.Red == right.Red)
                && (left.Green == right.Green)
                && (left.Blue == right.Blue);
        }

        /// <summary>Compares two <see cref="Color" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Color" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Color" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Color left, Color right)
        {
            return (left.Red != right.Red)
                || (left.Green != right.Green)
                || (left.Blue != right.Blue);
        }
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Color" /> instance with <see cref="Blue" /> set to the specified value.</summary>
        /// <param name="value">The new value of the blue component.</param>
        /// <returns>A new <see cref="Color" /> instance with <see cref="Blue" /> set to <paramref name="value" />.</returns>
        public Color WithBlue(float value)
        {
            return new Color(Red, Green, value);
        }

        /// <summary>Creates a new <see cref="Color" /> instance with <see cref="Green" /> set to the specified value.</summary>
        /// <param name="value">The new value of the green component.</param>
        /// <returns>A new <see cref="Color" /> instance with <see cref="Green" /> set to <paramref name="value" />.</returns>
        public Color WithGreen(float value)
        {
            return new Color(Red, value, Blue);
        }

        /// <summary>Creates a new <see cref="Color" /> instance with <see cref="Red" /> set to the specified value.</summary>
        /// <param name="value">The new value of the red component.</param>
        /// <returns>A new <see cref="Color" /> instance with <see cref="Red" /> set to <paramref name="value" />.</returns>
        public Color WithRed(float value)
        {
            return new Color(value, Green, Blue);
        }
        #endregion

        #region System.IEquatable<Color> Methods
        /// <summary>Compares a <see cref="Color" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Color" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Color other)
        {
            return this == other;
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

            return new StringBuilder(7 + (separator.Length * 2))
                .Append('<')
                .Append(Red.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Green.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Blue.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Color" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Color other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(Red.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Green.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Blue.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, sizeof(float) * 3);
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
