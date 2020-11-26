// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Globalization;
using System.Text;
using TerraFX.Utilities;

namespace TerraFX.Numerics
{
    /// <summary>Defines a quaternion.</summary>
    public readonly struct Quaternion : IEquatable<Quaternion>, IFormattable
    {
        private readonly float _x;
        private readonly float _y;
        private readonly float _z;
        private readonly float _w;

        /// <summary>Initializes a new instance of the <see cref="Quaternion" /> struct.</summary>
        /// <param name="x">The value of the x-component.</param>
        /// <param name="y">The value of the y-component.</param>
        /// <param name="z">The value of the z-component.</param>
        /// <param name="w">The value of the w-component.</param>
        public Quaternion(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="Quaternion"/> struct from a <see cref="ReadOnlySpan{T}"/>
        /// </summary>
        /// <param name="value"></param>
        public Quaternion(ReadOnlySpan<float> value)
        {
            if (value.Length < 4)
            {
                ExceptionUtilities.ThrowArgumentOutOfRangeException(value.ToArray(), nameof(value));
            }
            _x = value[0];
            _y = value[1];
            _z = value[2];
            _w = value[3];
        }

        /// <summary>Gets the value of the x-component.</summary>
        public float X => _x;

        /// <summary>Gets the value of the y-component.</summary>
        public float Y => _y;

        /// <summary>Gets the value of the z-component.</summary>
        public float Z => _z;

        /// <summary>Gets the value of the w-component.</summary>
        public float W => _w;

        /// <summary>Gets or sets the component at the specified index.</summary>
        /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException">Thrown when the <paramref name="index" /> is out of the range [0, 3].</exception>
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return _x;
                    case 1:
                        return _y;
                    case 2:
                        return _z;
                    case 3:
                        return _w;
                }
                ExceptionUtilities.ThrowArgumentOutOfRangeException(index, nameof(index));
                return 0f;
            }
        }

        /// <summary>
        /// Determine if the Quaternion is an Identity one.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is an identity quaternion; otherwise <c>false</c>.
        /// </returns>
        public bool IsIdentity => Equals(Quaternion.Identity);

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        /// <returns><c>true</c> if the Quaternion is normalized; otherwise, <c>false</c>.</returns>
        public bool IsNormalized => (double)Math.Abs((float)(((double)_x * (double)_x) + ((double)_y * (double)_y) + ((double)_z * (double)_z) + ((double)_w * (double)_w) - 1.0)) < 9.99999997475243E-07;

        /// <summary>Gets the angle of the Quaternion.</summary>
        /// <returns>The quaternion angle.</returns>
        public float Angle => ((double)_x * (double)_x) + ((double)_y * (double)_y) + ((double)_z * (double)_z) < 9.99999997475243E-07 ? 0.0f : (float)(2.0 * Math.Acos((double)_w));

        /// <summary>Gets the axis components of the quaternion.</summary>
        /// <returns>The axis components of the quaternion as a <see cref="Vector3"/>.</returns>
        public Vector3 Axis
        {
            get
            {
                var num1 = (float)(((double)_x * (double)_x) + ((double)_y * (double)_y) + ((double)_z * (double)_z));
                if ((double)num1 < 9.99999997475243E-07)
                {
                    return Vector3.UnitX;
                }
                var num2 = 1f / num1;
                return new Vector3(_x * num2, _y * num2, _z * num2);
            }
        }

        /// <summary>Calculates the length of the quaternion.</summary>
        /// <returns>The length of the quaternion.</returns>
        public float Length => (float)Math.Sqrt(((double)_x * (double)_x) + ((double)_y * (double)_y) + ((double)_z * (double)_z) + ((double)_w * (double)_w));

        /// <summary>Calculates the squared length of the quaternion.</summary>
        /// <returns>The squared length of the quaternion.</returns>
        public float LengthSquared => (float)(((double)_x * (double)_x) + ((double)_y * (double)_y) + ((double)_z * (double)_z) + ((double)_w * (double)_w));


        /// <summary>Compares two <see cref="Quaternion" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Quaternion" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Quaternion" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Quaternion left, Quaternion right)
        {
            return (left.X == right.X)
                && (left.Y == right.Y)
                && (left.Z == right.Z)
                && (left.W == right.W);
        }

        /// <summary>Compares two <see cref="Quaternion" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Quaternion" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Quaternion" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Quaternion left, Quaternion right)
        {
            return (left.X != right.X)
                || (left.Y != right.Y)
                || (left.Z != right.Z)
                || (left.W != right.W);
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

        /// <summary>
        /// A <see cref="Quaternion"/> with all its components set to zero.
        /// </summary>
        public static readonly Quaternion Zero = new Quaternion();

        /// <summary>
        /// A <see cref="Quaternion"/> with all its components set to one.
        /// </summary>
        public static readonly Quaternion One = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f);

        /// <summary>
        /// An Identity <see cref="Quaternion"/> (0, 0, 0, 1)
        /// </summary>
        public static readonly Quaternion Identity = new Quaternion(0.0f, 0.0f, 0.0f, 1f);

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Quaternion other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Quaternion other) => this == other;

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

        /// <summary>Conjugates the quaternion.</summary>
        public Quaternion Conjugate() => new Quaternion(-_x, -_y, -_z, _w);

        /// <summary>Conjugates and re-normalizes the Quaternion.</summary>
        public Quaternion Inverse()
        {
            var ls = LengthSquared;

            if ((double)ls <= 9.99999997475243E-07)
            {
                return Zero;
            }
            var ils = 1f / ls;

            return new Quaternion(-_x * ils, -_y * ils, -_z * ils, _w * ils);
        }

        /// <summary>Converts the quaternion into a unit quaternion.</summary>
        public Quaternion Normalize()
        {
            var l = Length;
            if ((double)l <= 9.99999997475243E-07)
            {
                return Zero;
            }

            var il = 1f / l;

            return new Quaternion(_x * il, _y * il, _z * il, _w * il);
        }

        /// <summary>Calculates the dot product of two quaternions.</summary>
        /// <param name="left">First source quaternion.</param>
        /// <param name="right">Second source quaternion.</param>
        /// <returns>The dot product of the two quaternions.</returns>
        public static float Dot(Quaternion left, Quaternion right) => (float)(((double)left.X * (double)right.X) + ((double)left.Y * (double)right.Y) + ((double)left.Z * (double)right.Z) + ((double)left.W * (double)right.W));

        /// <summary>Adds two quaternions.</summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two quaternions.</param>
        public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result) => result = new Quaternion(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);

        /// <summary>Adds two quaternions.</summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <returns>The sum of the two quaternions.</returns>
        public static Quaternion Add(Quaternion left, Quaternion right)
        {
            Quaternion.Add(ref left, ref right, out var result);
            return result;
        }

        /// <summary>Subtracts two quaternions.</summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two quaternions.</param>
        public static void Subtract(ref Quaternion left, ref Quaternion right, out Quaternion result) => result = new Quaternion(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);

        /// <summary>Subtracts two quaternions.</summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <returns>The difference of the two quaternions.</returns>
        public static Quaternion Subtract(Quaternion left, Quaternion right)
        {
            Quaternion.Subtract(ref left, ref right, out var result);
            return result;
        }

        /// <summary>Scales a quaternion by the given value.</summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <param name="result">When the method completes, contains the scaled quaternion.</param>
        public static void Multiply(ref Quaternion value, float scale, out Quaternion result) => result = new Quaternion(value.X * scale, value.Y * scale, value.Z * scale, value.W * scale);

        /// <summary>Scales a quaternion by the given value.</summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static Quaternion Multiply(Quaternion value, float scale)
        {
            Quaternion.Multiply(ref value, scale, out var result);
            return result;
        }

        /// <summary>Modulates a quaternion by another.</summary>
        /// <param name="left">The first quaternion to modulate.</param>
        /// <param name="right">The second quaternion to modulate.</param>
        /// <param name="result">Contains the modulated quaternion.</param>
        public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result)
        {
            var x1 = left.X;
            var y1 = left.Y;
            var z1 = left.Z;
            var w1 = left.W;
            var x2 = right.X;
            var y2 = right.Y;
            var z2 = right.Z;
            var w2 = right.W;
            result = new Quaternion(
                (float)(((double)x2 * (double)w1) + ((double)x1 * (double)w2) + ((double)y2 * (double)z1) - ((double)z2 * (double)y1)),
                (float)(((double)y2 * (double)w1) + ((double)y1 * (double)w2) + ((double)z2 * (double)x1) - ((double)x2 * (double)z1)),
                (float)(((double)z2 * (double)w1) + ((double)z1 * (double)w2) + ((double)x2 * (double)y1) - ((double)y2 * (double)x1)),
                (float)(((double)w2 * (double)w1) - (((double)x2 * (double)x1) + ((double)y2 * (double)y1) + ((double)z2 * (double)z1))));
        }

        /// <summary>Modulates a quaternion by another.</summary>
        /// <param name="left">The first quaternion to modulate.</param>
        /// <param name="right">The second quaternion to modulate.</param>
        /// <returns>The modulated quaternion.</returns>
        public static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            Quaternion.Multiply(ref left, ref right, out var result);
            return result;
        }

        /// <summary>Reverses the direction of a given quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <param name="result">When the method completes, contains a quaternion facing in the opposite direction.</param>
        public static Quaternion Negate(ref Quaternion value, out Quaternion result) => result = new Quaternion(-value.X, -value.Y, -value.Z, -value.W);

        /// <summary>Reverses the direction of a given quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static Quaternion Negate(Quaternion value)
        {
            Quaternion.Negate(ref value, out var result);
            return result;
        }

        /// <summary>Adds two quaternions.</summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <returns>The sum of the two quaternions.</returns>
        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            Quaternion.Add(ref left, ref right, out var result);
            return result;
        }

        /// <summary>Subtracts two quaternions.</summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <returns>The difference of the two quaternions.</returns>
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            Quaternion.Subtract(ref left, ref right, out var result);
            return result;
        }

        /// <summary>Reverses the direction of a given quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static Quaternion operator -(Quaternion value)
        {
            Quaternion.Negate(ref value, out var result);
            return result;
        }

        /// <summary>Scales a quaternion by the given value.</summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static Quaternion operator *(float scale, Quaternion value)
        {
            Quaternion.Multiply(ref value, scale, out var result);
            return result;
        }

        /// <summary>Scales a quaternion by the given value.</summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static Quaternion operator *(Quaternion value, float scale)
        {
            Quaternion.Multiply(ref value, scale, out var result);
            return result;
        }

        /// <summary>Multiplies a quaternion by another.</summary>
        /// <param name="left">The first quaternion to multiply.</param>
        /// <param name="right">The second quaternion to multiply.</param>
        /// <returns>The multiplied quaternion.</returns>
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion.Multiply(ref left, ref right, out var result);
            return result;
        }
    }
}
