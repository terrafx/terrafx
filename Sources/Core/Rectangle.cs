// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX
{
    /// <summary>Defines a rectangle.</summary>
    public struct Rectangle : IEquatable<Rectangle>, IFormattable
    {
        #region Fields
        private Point2D _location;

        private Size2D _size;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Rectangle" /> struct.</summary>
        /// <param name="location">The location of the instance.</param>
        /// <param name="size">The size of the instance.</param>
        public Rectangle(Point2D location, Size2D size)
        {
            _location = location;
            _size = size;
        }

        /// <summary>Initializes a new instance of the <see cref="Rectangle" /> struct.</summary>
        /// <param name="x">The x-coordinate of the instance.</param>
        /// <param name="y">The y-coordinate of the instance.</param>
        /// <param name="width">The width of the instance.</param>
        /// <param name="height">The height of the instance.</param>
        public Rectangle(float x, float y, float width, float height)
        {
            _location = new Point2D(x, y);
            _size = new Size2D(width, height);
        }
        #endregion

        #region Properties
        /// <summary>Gets or sets the height of the instance.</summary>
        public float Height
        {
            get
            {
                return _size.Height;
            }

            set
            {
                _size.Height = value;
            }
        }

        /// <summary>Gets or sets the location of the instance.</summary>
        public Point2D Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
            }
        }

        /// <summary>Gets or sets the size of the instance.</summary>
        public Size2D Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }

        /// <summary>Gets or sets the width of the instance.</summary>
        public float Width
        {
            get
            {
                return _size.Width;
            }

            set
            {
                _size.Width = value;
            }
        }

        /// <summary>Gets or sets the value of the x-coordinate.</summary>
        public float X
        {
            get
            {
                return _location.X;
            }

            set
            {
                _location.X = value;
            }
        }

        /// <summary>Gets or sets the value of the x-coordinate.</summary>
        public float Y
        {
            get
            {
                return _location.Y;
            }

            set
            {
                _location.Y = value;
            }
        }
        #endregion

        #region Operators
        /// <summary>Compares two <see cref="Rectangle" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Rectangle" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Rectangle" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return (left.Location == right.Location)
                && (left.Size == right.Size);
        }

        /// <summary>Compares two <see cref="Rectangle" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Rectangle" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Rectangle" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return (left.Location != right.Location)
                || (left.Size != right.Size);
        }
        #endregion

        #region System.IEquatable<Rectangle>
        /// <summary>Compares a <see cref="Rectangle" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Rectangle" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Rectangle other)
        {
            return (this == other);
        }
        #endregion

        #region System.IFormattable
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
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(Width.ToString(format, formatProvider));
                stringBuilder.Append(separator);
                stringBuilder.Append(' ');
                stringBuilder.Append(Height.ToString(format, formatProvider));
                stringBuilder.Append('>');
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region System.Object
        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Rectangle" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Rectangle other)
                && Equals(other);
        }

        /// <summary>Gets a hash code for the current instance.</summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = HashUtilities.CombineValue(X.GetHashCode(), combinedValue);
                combinedValue = HashUtilities.CombineValue(Y.GetHashCode(), combinedValue);
                combinedValue = HashUtilities.CombineValue(Width.GetHashCode(), combinedValue);
                combinedValue = HashUtilities.CombineValue(Height.GetHashCode(), combinedValue);
            }
            return HashUtilities.FinalizeValue(combinedValue, (sizeof(float) * 4));
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
