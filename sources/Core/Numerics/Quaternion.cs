// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Resources;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a Quaternion that can be used as efficient means to represent a rotation in 3D.</summary>
    public readonly struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        /// <summary>Defines a <see cref="Quaternion" /> that represents the Identity mapping.</summary>
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>Defines a <see cref="Quaternion" /> that has zeros for all components.</summary>
        public static readonly Quaternion Zero = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Defines a <see cref="Quaternion" /> that has ones for all components.</summary>
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

        /// <summary>Compares two <see cref="Quaternion" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Quaternion" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Quaternion" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Quaternion left, Quaternion right) => left._value == right._value;

        /// <summary>Compares two <see cref="Quaternion" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Quaternion" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Quaternion" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Quaternion left, Quaternion right) => left._value != right._value;

        /// <summary>Returns the value of the <see cref="Quaternion" /> operand (the sign of the operand is unchanged).</summary>
        /// <param name="value">The operand to return</param>
        /// <returns>The value of the operand, <paramref name="value" />.</returns>
        public static Quaternion operator +(Quaternion value) => value;

        /// <summary>Negates the value of the specified <see cref="Quaternion" /> operand.</summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of <paramref name="value" /> multiplied by negative one (-1).</returns>
        public static Quaternion operator -(Quaternion value) => value * -1;

        /// <summary>Adds two specified <see cref="Quaternion" /> values.</summary>
        /// <param name="left">The first value to add.</param>
        /// <param name="right">The second value to add.</param>
        /// <returns>The result of adding <paramref name="left" /> and <paramref name="right" />.</returns>
        public static Quaternion operator +(Quaternion left, Quaternion right) => new Quaternion(left._value + right._value);

        /// <summary>Subtracts two specified <see cref="Quaternion" /> values.</summary>
        /// <param name="left">The minuend.</param>
        /// <param name="right">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        public static Quaternion operator -(Quaternion left, Quaternion right) => new Quaternion(left._value - right._value);

        /// <summary>Multiplies two specified <see cref="Quaternion" /> values.</summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            // from XMQuaternionMultiply at https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl#L82
            // (r.x * +l.w) + (r.y * +l.z) + (r.z * -l.y) + (r.w * +l.x),
            // (r.x * -l.z) + (r.y * +l.w) + (r.z * +l.x) + (r.w * +l.y),
            // (r.x * +l.y) + (r.y * -l.x) + (r.z * +l.w) + (r.w * +l.z),
            // (r.x * -l.x) + (r.y * -l.y) + (r.z * -l.z) + (r.w * +l.w)

            var resultQ = new Quaternion(
                Quaternion.Dot(left, new Quaternion(+right.W, +right.Z, -right.Y, +right.X)),
                Quaternion.Dot(left, new Quaternion(-right.Z, +right.W, +right.X, +right.Y)),
                Quaternion.Dot(left, new Quaternion(+right.Y, -right.X, +right.W, +right.Z)),
                Quaternion.Dot(left, new Quaternion(-right.X, -right.Y, -right.Z, +right.W))
            );
            return resultQ;
        }

        /// <summary>Divides two specified <see cref="Quaternion" /> values component by component.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The result of dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator /(Quaternion left, Quaternion right) => new Quaternion(left._value / right._value);

        /// <summary>Multiplies each component of a <see cref="Quaternion" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The value to multiply each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator *(Quaternion left, float right) => new Quaternion(left._value * right);

        /// <summary>Divides each component of a <see cref="Quaternion" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor to divide each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator /(Quaternion left, float right) => new Quaternion(left._value / right);

        /// <summary>
        /// Creates a new <see cref="Quaternion" /> by concatenating 'this' with the given one.
        /// The result will have the combined effect of the rotations in both source Quaternions.
        /// Is the same as <seealso cref="Quaternion.operator *(Quaternion, Quaternion)" /> with reversed operand order.
        /// </summary>
        /// <param name="left">The left Quaternion for this operation.</param>
        /// <param name="right">The right Quaternion to concatenate.</param>
        /// <returns>The combined Quaternion.</returns>
        public static Quaternion Concatenate(Quaternion left, Quaternion right) => right * left;

        /// <summary>The Conjugate of a <see cref="Quaternion" />.</summary>
        /// <param name="value">The Quaternion for this operation.</param>
        /// <returns>The resulting Conjugate.</returns>
        public static Quaternion Conjugate(Quaternion value) => new Quaternion(-value.X, -value.Y, -value.Z, value.W);

        /// <summary>A new  <see cref="Quaternion" /> that embodies rotation about the given axis by the given angle in radians.</summary>
        /// <param name="axis">The rotation axis. It will be normalized before use.</param>
        /// <param name="radians">The rotation angle in radians.</param>
        /// <returns></returns>
        public static Quaternion CreateFromAxisAngle(Vector3 axis, float radians)
        {
            var unitAxis = Vector3.Normalize(axis);
            var scale = MathF.Sin(radians / 2.0f);
            var q = new Quaternion(
                scale * unitAxis.X,
                scale * unitAxis.Y,
                scale * unitAxis.Z,
                MathF.Cos(radians / 2)
            );
            return q;
        }

        /// <summary>The dot product of this <see cref="Quaternion" /> with the given other one.</summary>
        /// <param name="left">The left Quaternion for this operation.</param>
        /// <param name="right">The right Quaternion for this operation.</param>
        /// <returns>The resulting dot product <see cref="Quaternion" />.</returns>
        public static float Dot(Quaternion left, Quaternion right) => Vector4.Dot(left._value, right._value);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Quaternion other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Quaternion other) => this == other;

        /// <summary>Tests if two <see cref="Quaternion" /> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="left">The left instance to compare.</param>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns>True if similar, false otherwise.</returns>
        public static bool EqualEstimate(Quaternion left, Quaternion right, Quaternion epsilon) => Vector4.EqualEstimate(left._value, right._value, epsilon._value);

        /// <summary>The inverse of this <see cref="Quaternion" /> with the given other one.</summary>
        /// <param name="value">The Quaternion for this operation.</param>
        /// <returns>The resulting inverse <see cref="Quaternion" />.</returns>
        public static Quaternion Invert(Quaternion value) => Quaternion.Conjugate(value) / value.LengthSquared;

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

        /// <summary>Computes the <see cref="Quaternion" /> that for each component has the maximum value out of this and v.</summary>
        /// <param name="left">The <see cref="Quaternion" /> for this operation.</param>
        /// <param name="right">The other <see cref="Quaternion" /> to compute the max with.</param>
        /// <returns>The resulting new instance.</returns>
        public static Quaternion Max(Quaternion left, Quaternion right) => new Quaternion(Vector4.Max(left._value, right._value));

        /// <summary>Computes the <see cref="Quaternion" /> that for each component has the minimum value out of this and v.</summary>
        /// <param name="left">The <see cref="Quaternion" /> for this operation.</param>
        /// <param name="right">The other <see cref="Quaternion" /> to compute the min with.</param>
        /// <returns>The resulting new instance.</returns>
        public static Quaternion Min(Quaternion left, Quaternion right) => new Quaternion(Vector4.Max(left._value, right._value));

        /// <summary>The unit length version of this <see cref="Quaternion" /> with the given other one.</summary>
        /// <param name="value">The Quaternion for this operation.</param>
        /// <returns>The resulting unit length <see cref="Quaternion" />.</returns>
        public static Quaternion Normalize(Quaternion value) => new Quaternion(Vector4.Normalize(value._value));

        /// <summary>The  <see cref="Matrix3x3" /> that corresponds to this <see cref="Quaternion" />.
        /// If this Quaternion is normalized, then the Matrix3x3 is a rotation only matrix,
        /// otherwise the Matrix3x3 is a rotation + scaling matrix.</summary>
        /// <param name="value">The Quaternion for this operation.</param>
        /// <returns>The resulting <see cref="Matrix3x3" />.</returns>
        public static Matrix3x3 ToMatrix3x3(Quaternion value)
        {
            var w2 = value.W * value.W;
            var x2 = value.X * value.X;
            var y2 = value.Y * value.Y;
            var z2 = value.Z * value.Z;
            var wz = 2 * value.W * value.Z;
            var xz = 2 * value.X * value.Z;
            var xy = 2 * value.X * value.Y;
            var wx = 2 * value.W * value.X;
            var wy = 2 * value.W * value.Y;
            var yz = 2 * value.Y * value.Z;
            var x = new Vector3(w2 + x2 - y2 - z2, wz + xy, xz - wy);
            var y = new Vector3(xy - wz, w2 - x2 + y2 - z2, wx + yz);
            var z = new Vector3(wy + xz, yz - wx, w2 - x2 - y2 + z2);
            var m = new Matrix3x3(x, y, z);
            return m;
        }

        /// <summary>The  <see cref="Matrix3x3" /> that corresponds to this <see cref="Quaternion" />.
        /// If this Quaternion is normalized, then the Matrix3x3 is a rotation only matrix,
        /// otherwise the Matrix3x3 is a rotation + scaling matrix.</summary>
        /// <param name="value">The Quaternion for this operation.</param>
        /// <returns>The resulting <see cref="Matrix3x3" />.</returns>
        public static Matrix4x4 ToMatrix4x4(Quaternion value)
        {
            var w2 = value.W * value.W;
            var x2 = value.X * value.X;
            var y2 = value.Y * value.Y;
            var z2 = value.Z * value.Z;
            var wz = 2 * value.W * value.Z;
            var xz = 2 * value.X * value.Z;
            var xy = 2 * value.X * value.Y;
            var wx = 2 * value.W * value.X;
            var wy = 2 * value.W * value.Y;
            var yz = 2 * value.Y * value.Z;
            var x = new Vector4(w2 + x2 - y2 - z2, wz + xy, xz - wy, 0);
            var y = new Vector4(xy - wz, w2 - x2 + y2 - z2, wx + yz, 0);
            var z = new Vector4(wy + xz, yz - wx, w2 - x2 - y2 + z2, 0);
            var w = new Vector4(0, 0, 0, 1);
            var m = new Matrix4x4(x, y, z, w);
            return m;
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

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="X" /> set to the specified value.</summary>
        /// <param name="value">The new value of the x-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="X" /> set to <paramref name="value" />.</returns>
        public Quaternion WithX(float value) => new Quaternion(value, Y, Z, W);

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="Y" /> set to the specified value.</summary>
        /// <param name="value">The new value of the y-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="Y" /> set to <paramref name="value" />.</returns>
        public Quaternion WithY(float value) => new Quaternion(X, value, Z, W);

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="Z" /> set to the specified value.</summary>
        /// <param name="value">The new value of the z-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="Z" /> set to <paramref name="value" />.</returns>
        public Quaternion WithZ(float value) => new Quaternion(X, Y, value, W);

        /// <summary>Creates a new <see cref="Quaternion" /> instance with <see cref="W" /> set to the specified value.</summary>
        /// <param name="value">The new value of the w-component.</param>
        /// <returns>A new <see cref="Quaternion" /> instance with <see cref="W" /> set to <paramref name="value" />.</returns>
        public Quaternion WithW(float value) => new Quaternion(X, Y, Z, value);
    }
}
