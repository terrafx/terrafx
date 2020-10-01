// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Graphics
{
    /// <summary>Defines a red-green-blue-alpha color triple.</summary>
    public readonly struct ColorRgba : IEquatable<ColorRgba>, IFormattable
    {
        private readonly float _red;
        private readonly float _green;
        private readonly float _blue;
        private readonly float _alpha;

        /// <summary>Initializes a new instance of the <see cref="ColorRgba" /> struct.</summary>
        /// <param name="red">The value of the red component.</param>
        /// <param name="green">The value of the green component.</param>
        /// <param name="blue">The value of the blue component.</param>
        /// <param name="alpha">The value of the alpha component.</param>
        public ColorRgba(float red, float green, float blue, float alpha)
        {
            _red = red;
            _green = green;
            _blue = blue;
            _alpha = alpha;
        }

        /// <summary>Gets the value of the alpha component.</summary>
        public float Alpha => _alpha;

        /// <summary>Gets the value of the blue component.</summary>
        public float Blue => _blue;

        /// <summary>Gets the value of the green component.</summary>
        public float Green => _green;

        /// <summary>Gets the value of the red component.</summary>
        public float Red => _red;

        /// <summary>Compares two <see cref="ColorRgba" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="ColorRgba" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ColorRgba" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ColorRgba left, ColorRgba right)
        {
            return (left.Red == right.Red)
                && (left.Green == right.Green)
                && (left.Blue == right.Blue)
                && (left.Alpha == right.Alpha);
        }

        /// <summary>Compares two <see cref="ColorRgba" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="ColorRgba" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="ColorRgba" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ColorRgba left, ColorRgba right)
        {
            return (left.Red != right.Red)
                || (left.Green != right.Green)
                || (left.Blue != right.Blue)
                || (left.Alpha != right.Alpha);
        }

        /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Alpha" /> set to the specified value.</summary>
        /// <param name="value">The new value of the alpha component.</param>
        /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Alpha" /> set to <paramref name="value" />.</returns>
        public ColorRgba WithAlpha(float value) => new ColorRgba(Red, Green, Blue, value);

        /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Blue" /> set to the specified value.</summary>
        /// <param name="value">The new value of the blue component.</param>
        /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Blue" /> set to <paramref name="value" />.</returns>
        public ColorRgba WithBlue(float value) => new ColorRgba(Red, Green, value, Alpha);

        /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Green" /> set to the specified value.</summary>
        /// <param name="value">The new value of the green component.</param>
        /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Green" /> set to <paramref name="value" />.</returns>
        public ColorRgba WithGreen(float value) => new ColorRgba(Red, value, Blue, Alpha);

        /// <summary>Creates a new <see cref="ColorRgba" /> instance with <see cref="Red" /> set to the specified value.</summary>
        /// <param name="value">The new value of the red component.</param>
        /// <returns>A new <see cref="ColorRgba" /> instance with <see cref="Red" /> set to <paramref name="value" />.</returns>
        public ColorRgba WithRed(float value) => new ColorRgba(value, Green, Blue, Alpha);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is ColorRgba other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(ColorRgba other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Red);
                hashCode.Add(Green);
                hashCode.Add(Blue);
                hashCode.Add(Alpha);
            }
            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(9 + (separator.Length * 3))
                .Append('<')
                .Append(Red.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Green.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Blue.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Alpha.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }
    }
}
