// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a two-dimensional Euclidean vector.</summary>
    public readonly struct Vector2 : IEquatable<Vector2>, IFormattable
    {
        /// <summary>Defines a <see cref="Vector2" /> where all components are zero.</summary>
        public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector2" /> whose x-component is one and whose remaining components are zero.</summary>
        public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector2" /> whose y-component is one and whose remaining components are zero.</summary>
        public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

        /// <summary>Defines a <see cref="Vector2" /> where all components are one.</summary>
        public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

        private readonly float _x;
        private readonly float _y;

        /// <summary>Initializes a new instance of the <see cref="Vector2" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>Gets the value of the x-dimension.</summary>
        public float X => _x;

        /// <summary>Gets the value of the y-dimension.</summary>
        public float Y => _y;

        /// <summary>Compares two <see cref="Vector2" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Vector2" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector2" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y);
        }

        /// <summary>Compares two <see cref="Vector2" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Vector2" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector2" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y);
        }

        /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Vector2" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector2 WithX(float value) => new Vector2(value, Y);

        /// <summary>Creates a new <see cref="Vector2" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector2" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector2 WithY(float value) => new Vector2(X, value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Vector2 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Vector2 other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var combinedValue = 0;
            {
                combinedValue = CombineValue(X.GetHashCode(), combinedValue);
                combinedValue = CombineValue(Y.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, sizeof(float) * 2);
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(5 + separator.Length)
                .Append('<')
                .Append(X.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Y.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }
    }
}
