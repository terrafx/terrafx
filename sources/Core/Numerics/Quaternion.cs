// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Resources;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a quaternion.</summary>
    public readonly struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        /// <summary>Defines a <see cref="Quaternion" /> that represents the Identity mapping.</summary>
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        private readonly Vector4 _q;

        /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
        /// <param name="x">The value of the x-component.</param>
        /// <param name="y">The value of the y-component.</param>
        /// <param name="z">The value of the z-component.</param>
        /// <param name="w">The value of the w-component.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            _q = new Vector4(x, y, z, w);
        }

        /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
        /// <param name="q">The <see cref="Vector4" /> q that should be interpreted as <see cref="Quaternion" />.</param>
        public Quaternion(Vector4 q)
        {
            _q = q;
        }

        /// <summary>Gets the value of the x-component.</summary>
        public float X => _q.X;

        /// <summary>Gets the value of the y-component.</summary>
        public float Y => _q.Y;

        /// <summary>Gets the value of the z-component.</summary>
        public float Z => _q.Z;

        /// <summary>Gets the value of the w-component.</summary>
        public float W => _q.W;

        /// <summary>Computes the length of this <see cref="Quaternion" />.</summary>
        /// <returns>The length of this <see cref="Quaternion" />.</returns>
        public float Length => _q.Length;

        /// <summary>Computes the squared length of this <see cref="Quaternion" />.</summary>
        /// <returns>The squared length of this <see cref="Quaternion" />.</returns>
        public float LengthSquared => _q.LengthSquared;

        /// <summary>Compares two <see cref="Quaternion" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Quaternion" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Quaternion" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Quaternion left, Quaternion right) => left._q == right._q;

        /// <summary>Compares two <see cref="Quaternion" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Quaternion" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Quaternion" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Quaternion left, Quaternion right) => left._q != right._q;

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
        public static Quaternion operator +(Quaternion left, Quaternion right) => new Quaternion(left._q + right._q);

        /// <summary>Subtracts two specified <see cref="Quaternion" /> values.</summary>
        /// <param name="left">The minuend.</param>
        /// <param name="right">The subtrahend.</param>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        public static Quaternion operator -(Quaternion left, Quaternion right) => new Quaternion(left._q - right._q);

        /// <summary>Multiplies two specified <see cref="Quaternion" /> values. Is the same as <seealso cref="Quaternion.Concatenate" />.</summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The result of multiplying <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator *(Quaternion left, Quaternion right) => Quaternion.Concatenate(left, right);

        /// <summary>Divides two specified <see cref="Quaternion" /> values component by component.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor.</param>
        /// <returns>The result of dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator /(Quaternion left, Quaternion right) => new Quaternion(left._q / right._q);

        /// <summary>Multiplies each component of a <see cref="Quaternion" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The value to multiply each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator *(Quaternion left, float right) => new Quaternion(left._q * right);

        /// <summary>Divides each component of a <see cref="Quaternion" /> value by a given <see cref="float" /> value.</summary>
        /// <param name="left">The dividend.</param>
        /// <param name="right">The divisor to divide each component by.</param>
        /// <returns>The result of multiplying each component of <paramref name="left" /> by <paramref name="right" />.</returns>
        public static Quaternion operator /(Quaternion left, float right) => new Quaternion(left._q / right);

        /// <summary>
        /// Creates a new <see cref="Quaternion" /> by concatenating 'this' with the given one.
        /// The result will have the combined effect of the rotations in both source Quaternions.
        /// </summary>
        /// <param name="q">The left Quaternion for this operation.</param>
        /// <param name="r">The right Quaternion to concatenate.</param>
        /// <returns>The combined Quaternion.</returns>
        public static Quaternion Concatenate(Quaternion q, Quaternion r)
        {
            // from XMQuaternionMultiply at https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl#L82
            // (r.x * +q.w) + (r.y * +q.z) + (r.z * -q.y) + (r.w * +q.x),
            // (r.x * -q.z) + (r.y * +q.w) + (r.z * +q.x) + (r.w * +q.y),
            // (r.x * +q.y) + (r.y * -q.x) + (r.z * +q.w) + (r.w * +q.z),
            // (r.x * -q.x) + (r.y * -q.y) + (r.z * -q.z) + (r.w * +q.w)

            var resultQ = new Quaternion(
                Quaternion.Dot(r, new Quaternion(+q.W, +q.Z, -q.Y, +q.X)),
                Quaternion.Dot(r, new Quaternion(-q.Z, +q.W, +q.X, +q.Y)),
                Quaternion.Dot(r, new Quaternion(+q.Y, -q.X, +q.W, +q.Z)),
                Quaternion.Dot(r, new Quaternion(-q.X, -q.Y, -q.Z, +q.W))
            );
            return resultQ;
        }

        /// <summary>The Conjugate of a <see cref="Quaternion" />.</summary>
        /// <param name="q">The Quaternion for this operation.</param>
        /// <returns>The resulting Conjugate.</returns>
        public static Quaternion Conjugate(Quaternion q) => new Quaternion(-q.X, -q.Y, -q.Z, q.W);

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
        /// <param name="q">The left Quaternion for this operation.</param>
        /// <param name="r">The right Quaternion for this operation.</param>
        /// <returns>The resulting dot product <see cref="Quaternion" />.</returns>
        public static float Dot(Quaternion q, Quaternion r) => Vector4.Dot(q._q, r._q);

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
        public static bool EqualEstimate(Quaternion left, Quaternion right, Quaternion epsilon) => Vector4.EqualEstimate(left._q, right._q, epsilon._q);

        /// <summary>The inverse of this <see cref="Quaternion" /> with the given other one.</summary>
        /// <param name="q">The Quaternion for this operation.</param>
        /// <returns>The resulting inverse <see cref="Quaternion" />.</returns>
        public static Quaternion Invert(Quaternion q)
        {
            var l2 = q.LengthSquared;
            return new Quaternion(-q.X / l2, -q.Y / l2, -q.Z / l2, q.W / l2);
        }

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
        /// <param name="q">The <see cref="Quaternion" /> for this operation.</param>
        /// <param name="other">The other <see cref="Quaternion" /> to compute the max with.</param>
        /// <returns>The resulting new instance.</returns>
        public static Quaternion Max(Quaternion q, Quaternion other) => new Quaternion(Vector4.Max(q._q, other._q));

        /// <summary>Computes the <see cref="Quaternion" /> that for each component has the minimum value out of this and v.</summary>
        /// <param name="q">The <see cref="Quaternion" /> for this operation.</param>
        /// <param name="other">The other <see cref="Quaternion" /> to compute the min with.</param>
        /// <returns>The resulting new instance.</returns>
        public static Quaternion Min(Quaternion q, Quaternion other) => new Quaternion(Vector4.Max(q._q, other._q));

        /// <summary>The unit length version of this <see cref="Quaternion" /> with the given other one.</summary>
        /// <param name="q">The Quaternion for this operation.</param>
        /// <returns>The resulting unit length <see cref="Quaternion" />.</returns>
        public static Quaternion Normalize(Quaternion q) => new Quaternion(Vector4.Normalize(q._q));

        /// <summary>The  <see cref="Matrix3x3" /> that corresponds to this <see cref="Quaternion" />.
        /// If this Quaternion is normalized, then the Matrix3x3 is a rotation only matrix,
        /// otherwise the Matrix3x3 is a rotation + scaling matrix.</summary>
        /// <param name="q">The Quaternion for this operation.</param>
        /// <returns>The resulting <see cref="Matrix3x3" />.</returns>
        public static Matrix3x3 ToMatrix3x3(Quaternion q)
        {
            var w2 = q.W * q.W;
            var x2 = q.X * q.X;
            var y2 = q.Y * q.Y;
            var z2 = q.Z * q.Z;
            var wz = 2 * q.W * q.Z;
            var xz = 2 * q.X * q.Z;
            var xy = 2 * q.X * q.Y;
            var wx = 2 * q.W * q.X;
            var wy = 2 * q.W * q.Y;
            var yz = 2 * q.Y * q.Z;
            var x = new Vector3(w2 + x2 - y2 - z2, wz + xy, xz - wy);
            var y = new Vector3(xy - wz, w2 - x2 + y2 - z2, wx + yz);
            var z = new Vector3(wy + xz, yz - wx, w2 - x2 - y2 + z2);
            var m = new Matrix3x3(x, y, z);
            return m;
        }

        /// <summary>The  <see cref="Matrix3x3" /> that corresponds to this <see cref="Quaternion" />.
        /// If this Quaternion is normalized, then the Matrix3x3 is a rotation only matrix,
        /// otherwise the Matrix3x3 is a rotation + scaling matrix.</summary>
        /// <param name="q">The Quaternion for this operation.</param>
        /// <returns>The resulting <see cref="Matrix3x3" />.</returns>
        public static Matrix4x4 ToMatrix4x4(Quaternion q)
        {
            var w2 = q.W * q.W;
            var x2 = q.X * q.X;
            var y2 = q.Y * q.Y;
            var z2 = q.Z * q.Z;
            var wz = 2 * q.W * q.Z;
            var xz = 2 * q.X * q.Z;
            var xy = 2 * q.X * q.Y;
            var wx = 2 * q.W * q.X;
            var wy = 2 * q.W * q.Y;
            var yz = 2 * q.Y * q.Z;
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
