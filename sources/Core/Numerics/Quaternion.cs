// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;
using static TerraFX.Utilities.AssertionUtilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a quaternion whch encodes a rotation as an axis-angle.</summary>
    public readonly struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        /// <summary>Defines the identity quaternion.</summary>
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>Defines the all zeros quaternion.</summary>
        public static readonly Quaternion Zero = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Defines the all ones quaternion.</summary>
        public static readonly Quaternion One = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f);

        private readonly Vector4 _value;

        /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
        /// <param name="x">The value of the x-component.</param>
        /// <param name="y">The value of the y-component.</param>
        /// <param name="z">The value of the z-component.</param>
        /// <param name="w">The value of the w-component.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            _value = new Vector4(x, y, z, w);
        }

        /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
        /// <param name="value">The <see cref="Vector4" /> q that should be interpreted as <see cref="Quaternion" />.</param>
        public Quaternion(Vector4 value)
        {
            _value = value;
        }

        /// <summary>Gets the value of the x-component.</summary>
        public float X => _value.X;

        /// <summary>Gets the value of the y-component.</summary>
        public float Y => _value.Y;

        /// <summary>Gets the value of the z-component.</summary>
        public float Z => _value.Z;

        /// <summary>Gets the value of the w-component.</summary>
        public float W => _value.W;

        /// <summary>Computes the length of this <see cref="Quaternion" />.</summary>
        /// <returns>The length of this <see cref="Quaternion" />.</returns>
        public float Length => _value.Length;

        /// <summary>Computes the squared length of this <see cref="Quaternion" />.</summary>
        /// <returns>The squared length of this <see cref="Quaternion" />.</returns>
        public float LengthSquared => _value.LengthSquared;

        /// <summary>Compares two quaternions to determine equality.</summary>
        /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
        /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Quaternion left, Quaternion right) => left._value == right._value;

        /// <summary>Compares two quaternions to determine inequality.</summary>
        /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
        /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Quaternion left, Quaternion right) => left._value != right._value;

        /// <summary>Computes the value of a quaternion.</summary>
        /// <param name="value">The quaternion.</param>
        /// <returns><paramref name="value" /></returns>
        public static Quaternion operator +(Quaternion value) => value;

        /// <summary>Negates a quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>The negation of <paramref name="value" />.</returns>
        public static Quaternion operator -(Quaternion value) => value * -1;

        /// <summary>Computes the sum of two quaternions.</summary>
        /// <param name="left">The quaternion to which to add <paramref name="right" />.</param>
        /// <param name="right">The quaternion which is added to <paramref name="left" />.</param>
        /// <returns>The sum of <paramref name="right" /> added to <paramref name="left" />.</returns>
        public static Quaternion operator +(Quaternion left, Quaternion right) => new Quaternion(left._value + right._value);

        /// <summary>Computes the difference of two quaternions.</summary>
        /// <param name="left">The quaternion from which to subtract <paramref name="right" />.</param>
        /// <param name="right">The quaternion which is subtracted from <paramref name="left" />.</param>
        /// <returns>The difference of <paramref name="right" /> subtracted from <paramref name="left" />.</returns>
        public static Quaternion operator -(Quaternion left, Quaternion right) => new Quaternion(left._value - right._value);

        /// <summary>Computes the product of a quaternion and a float.</summary>
        /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
        /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Quaternion operator *(Quaternion left, float right) => new Quaternion(left._value * right);

        /// <summary>Computes the product of two quaternions.</summary>
        /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
        /// <param name="right">The quaternion which is used to multiply <paramref name="left" />.</param>
        /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new Quaternion(
                Dot(left, new Quaternion(+right.W, +right.Z, -right.Y, +right.X)),
                Dot(left, new Quaternion(-right.Z, +right.W, +right.X, +right.Y)),
                Dot(left, new Quaternion(+right.Y, -right.X, +right.W, +right.Z)),
                Dot(left, new Quaternion(-right.X, -right.Y, -right.Z, +right.W))
            );
        }

        /// <summary>Computes the quotient of a quaternion and a float.</summary>
        /// <param name="left">The quaternion which is divied by <paramref name="right" />.</param>
        /// <param name="right">The float which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Quaternion operator /(Quaternion left, float right) => new Quaternion(left._value / right);

        /// <summary>Computes the quotient of two quaternions.</summary>
        /// <param name="left">The quaternion which is divied by <paramref name="right" />.</param>
        /// <param name="right">The quaternion which divides <paramref name="left" />.</param>
        /// <returns>The quotient of <paramref name="left" /> divided by <paramref name="right" />.</returns>
        public static Quaternion operator /(Quaternion left, Quaternion right) => new Quaternion(left._value / right._value);

        /// <summary>Computes the concatenation of two quaternions.</summary>
        /// <param name="left">The quaternion on which to concatenate <paramref name="right" />.</param>
        /// <param name="right">The quaternion which is concatenated onto <paramref name="left" />.</param>
        /// <returns>The concatenation of <paramref name="right" /> onto <paramref name="left" />.</returns>
        /// <remarks>This methods is the same as computing <c><paramref name="right" /> * <paramref name="left" /></c>.</remarks>
        public static Quaternion Concatenate(Quaternion left, Quaternion right) => right * left;

        /// <summary>Computes the conjugate of a quaternion.</summary>
        /// <param name="value">The quaternion for which to get its conjugate.</param>
        /// <returns>The conjugate of <paramref name="value" />.</returns>
        public static Quaternion Conjugate(Quaternion value) => new Quaternion(-value.X, -value.Y, -value.Z, value.W);

        /// <summary>Creates a quaternion from an axis and angle.</summary>
        /// <param name="axis">The axis of the quaternion.</param>
        /// <param name="angle">The angle, in radians, of the quaternion.</param>
        /// <returns>A quaternion that represents <paramref name="axis" /> and <paramref name="angle" />.</returns>
        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
        {
            var normal = Vector3.Normalize(axis);
            (var sin, var cos) = MathUtilities.SinCos(angle * 0.5f);

            return new Quaternion(
                sin * normal.X,
                sin * normal.Y,
                sin * normal.Z,
                cos
            );
        }

        /// <summary>Computes the dot product of two quaternions.</summary>
        /// <param name="left">The quaternion to multiply by <paramref name="right" />.</param>
        /// <param name="right">The quatnerion which is used to multiply <paramref name="left" />.</param>
        /// <returns>The dot product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
        public static float Dot(Quaternion left, Quaternion right) => Vector4.Dot(left._value, right._value);

        /// <summary>Compares two quaternions to determine approximate equality.</summary>
        /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
        /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
        /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
        public static bool EqualsEstimate(Quaternion left, Quaternion right, Quaternion epsilon)
            => Vector4.EqualsEstimate(left._value, right._value, epsilon._value);

        /// <summary>Computes the inverse of a quaternion.</summary>
        /// <param name="value">The quaternion to invert.</param>
        /// <returns>The inverse of <paramref name="value" />.</returns>
        public static Quaternion Invert(Quaternion value) => Conjugate(value) / value.LengthSquared;

        /// <summary>Compares two quaternions to determine the combined maximum.</summary>
        /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
        /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
        /// <returns>The combined maximum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Quaternion Max(Quaternion left, Quaternion right) => new Quaternion(
            Vector4.Max(left._value, right._value)
        );

        /// <summary>Compares two quaternions to determine the combined minimum.</summary>
        /// <param name="left">The quaternion to compare with <paramref name="right" />.</param>
        /// <param name="right">The quaternion to compare with <paramref name="left" />.</param>
        /// <returns>The combined minimum of <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Quaternion Min(Quaternion left, Quaternion right) => new Quaternion(
            Vector4.Max(left._value, right._value)
        );

        /// <summary>Computes the normalized form of a quaternion.</summary>
        /// <param name="value">The quaternion to normalized.</param>
        /// <returns>The normalized form of <paramref name="value" />.</returns>
        public static Quaternion Normalize(Quaternion value) => new Quaternion(
            Vector4.Normalize(value._value)
        );

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Quaternion other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Quaternion other) => this == other;

        /// <inheritdoc />
        public override int GetHashCode() => _value.GetHashCode();

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

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="x">The new value of the x-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
        public Quaternion WithX(float x) => new Quaternion(x, Y, Z, W);

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="y">The new value of the y-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
        public Quaternion WithY(float y) => new Quaternion(X, y, Z, W);

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="z">The new value of the z-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
        public Quaternion WithZ(float z) => new Quaternion(X, Y, z, W);

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="w">The new value of the w-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="W" /> set to <paramref name="w" />.</returns>
        public Quaternion WithW(float w) => new Quaternion(X, Y, Z, w);
    }
}
