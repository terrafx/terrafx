// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics
{
    /// <summary>Defines a four-dimensional Euclidean vector.</summary>
    public readonly struct Vector4 : IEquatable<Vector4>, IFormattable
    {
        /// <summary>Defines a <see cref="Vector4" /> where all components are zero.</summary>
        public static readonly Vector4 Zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose x-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitX = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose y-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitY = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose z-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);

        /// <summary>Defines a <see cref="Vector4" /> whose w-component is one and whose remaining components are zero.</summary>
        public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>Defines a <see cref="Vector4" /> where all components are one.</summary>
        public static readonly Vector4 One = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        private readonly float _x;
        private readonly float _y;
        private readonly float _z;
        private readonly float _w;

        /// <summary>Initializes a new instance of the <see cref="Vector4" /> struct.</summary>
        /// <param name="x">The value of the x-component.</param>
        /// <param name="y">The value of the y-component.</param>
        /// <param name="z">The value of the z-component.</param>
        /// <param name="w">The value of the w-component.</param>
        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>Gets the value of the x-component.</summary>
        public float X => _x;

        /// <summary>Gets the value of the y-component.</summary>
        public float Y => _y;

        /// <summary>Gets the value of the z-component.</summary>
        public float Z => _z;

        /// <summary>Gets the value of the w-component.</summary>
        public float W => _w;

        /// <summary>Compares two <see cref="Vector4" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Vector4" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector4" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.W == right.W);
        }

        /// <summary>Compares two <see cref="Vector4" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Vector4" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Vector4" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4 left, Vector4 right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y)
                || (left.Z != right.Z)
                || (left.W != right.W);
        }

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Vector4 WithX(float value) => new Vector4(value, Y, Z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Vector4 WithY(float value) => new Vector4(X, value, Z, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Vector4 WithZ(float value) => new Vector4(X, Y, value, W);

        /// <summary>Creates a new <see cref="Vector4" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="value">The new value of the w-component.</param>
        /// <returns>A new <see cref="Vector4" /> instance with <see cref="W" /> set to <paramref name="value" />.</returns>
        public Vector4 WithW(float value) => new Vector4(X, Y, Z, value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Vector4 other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Vector4 other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(X);
                hashCode.Add(Y);
                hashCode.Add(Z);
                hashCode.Add(W);
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
                .Append(X.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Y.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(Z.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append(W.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }
    }
}
