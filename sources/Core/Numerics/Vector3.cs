// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using static TerraFX.Utilities.HashUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a three-dimensional Euclidean vector.</summary>
    public readonly struct Vector3 : IEquatable<Vector3>, IFormattable
    {
        /// <summary>Defines a <see cref="Vector3" /> where all components are zero.</summary>
        public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector3" /> whose x-component is one and whose remaining components are zero.</summary>
        public static readonly Vector3 UnitX = new Vector3(1.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector3" /> whose y-component is one and whose remaining components are zero.</summary>
        public static readonly Vector3 UnitY = new Vector3(0.0f, 1.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector3" /> whose z-component is one and whose remaining components are zero.</summary>
        public static readonly Vector3 UnitZ = new Vector3(0.0f, 0.0f, 1.0f);

        /// <summary>Defines a <see cref="Vector3" /> where all components are one.</summary>
        public static readonly Vector3 One = new Vector3(1.0f, 1.0f, 1.0f);

        private readonly float _x;
        private readonly float _y;
        private readonly float _z;

        /// <summary>Initializes a new instance of the <see cref="Vector3" /> struct.</summary>
        /// <param name="x">The value of the x-dimension.</param>
        /// <param name="y">The value of the y-dimension.</param>
        /// <param name="z">The value of the z-dimension.</param>
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>Gets the value of the x-dimension.</summary>
        public float X => _x;

        /// <summary>Gets the value of the y-dimension.</summary>
        public float Y => _y;

        /// <summary>Gets the value of the z-dimension.</summary>
        public float Z => _z;

        /// <summary>Compares two <see cref="Vector3" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Vector3" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector3" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector3 left, Vector3 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z);
        }

        /// <summary>Compares two <see cref="Vector3" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Vector3" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector3" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector3 left, Vector3 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y)
                || (left.Z != right.Z);
        }

        /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-dimension.</param>
        /// <returns>A new <see cref="Vector3" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector3 WithX(float value) => new Vector3(value, Y, Z);

        /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-dimension.</param>
        /// <returns>A new <see cref="Vector3" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector3 WithY(float value) => new Vector3(X, value, Z);

        /// <summary>Creates a new <see cref="Vector3" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-dimension.</param>
        /// <returns>A new <see cref="Vector3" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Vector3 WithZ(float value) => new Vector3(X, Y, value);

        /// <summary>Compares a <see cref="Vector3" /> with the current instance to determine equality.</summary>
        /// <param name="other">The <see cref="Vector3" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other" /> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector3 other) => this == other;

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <param name="format">The format to use or <c>null</c> to use the default format.</param>
        /// <param name="formatProvider">The provider to use when formatting the current instance or <c>null</c> to use the default provider.</param>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(7 + (separator.Length * 2))
                .Append('<')
                .Append(X.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Y.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Z.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }

        /// <summary>Compares a <see cref="object" /> with the current instance to determine equality.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj" /> is an instance of <see cref="Vector3" /> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            return (obj is Vector3 other)
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
                combinedValue = CombineValue(Z.GetHashCode(), combinedValue);
            }
            return FinalizeValue(combinedValue, sizeof(float) * 3);
        }

        /// <summary>Converts the current instance to an equivalent <see cref="string" /> value.</summary>
        /// <returns>An equivalent <see cref="string" /> value for the current instance.</returns>
        public override string ToString() => ToString(format: null, formatProvider: null);
    }
}
