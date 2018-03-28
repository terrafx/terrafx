// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX
{
    /// <summary>Defines a size in two-dimensional space.</summary>
    public readonly struct Size2D : IEquatable<Size2D>, IFormattable
    {
        #region Fields
        /// <summary>The width of the instance.</summary>
        internal readonly float _width;

        /// <summary>The height of the instance.</summary>
        internal readonly float _height;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Size2D" /> struct.</summary>
        /// <param name="width">The width of the instance.</param>
        /// <param name="height">The height of the instance.</param>
        public Size2D(float width, float height)
        {
            _width = width;
            _height = height;
        }
        #endregion

        #region Properties
        /// <summary>Gets or sets the width of the instance.</summary>
        public float Width
        {
            get
            {
                return _width;
            }
        }

        /// <summary>Gets or sets the height of the instance.</summary>
        public float Height
        {
            get
            {
                return _height;
            }
        }
        #endregion

        #region Comparison Operators
        /// <summary>Compares two <see cref="Size2D" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Size2D" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Size2D" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Size2D left, Size2D right)
        {
            return (left.Width == right.Width)
                && (left.Height == right.Height);
        }

        /// <summary>Compares two <see cref="Size2D" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Size2D" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Size2D" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Size2D left, Size2D right)
        {
            return (left.Width != right.Width)
                || (left.Height != right.Height);
        }
        #endregion

        #region Methods
        /// <summary>Creates a new <see cref="Rectangle" /> instance with <see cref="Height" /> set to the specified value.</summary>
        /// <param name="value">The new height of the instance.</param>
        /// <returns>A new <see cref="Rectangle" /> instance with <see cref="Height" /> set to <paramref name="value" />.</returns>
        public Size2D WithHeight(float value)
        {
            return new Size2D(Width, value);
        }

        /// <summary>Creates a new <see cref="Size2D" /> instance with <see cref="Width" /> set to the specified value.</summary>
        /// <param name="value">The new width of the instance.</param>
        /// <returns>A new <see cref="Size2D" /> instance with <see cref="Width" /> set to <paramref name="value" />.</returns>
        public Size2D WithWidth(float value)
        {
            return new Size2D(value, Height);
        }
        #endregion

        #region System.IEquatable<Size2D> Methods
        /// <summary>Compares a <see cref="Size2D" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Size2D" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Size2D other)
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
                stringBuilder.Append(Width.ToString(format, formatProvider));
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(Height.ToString(format, formatProvider));
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region System.Object Methods
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Size2D" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Size2D other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(Width.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Height.GetHashCode(), combinedValue);
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
