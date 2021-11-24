// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on code from https://github.com/microsoft/DirectXMath
// The original code is Copyright © Microsoft. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Globalization;
using System.Text;

namespace TerraFX.Numerics;

/// <summary>Defines a 4x4 row-major matrix.</summary>
public readonly struct Matrix4x4 : IEquatable<Matrix4x4>, IFormattable
{
    /// <summary>Defines the identity matrix.</summary>
    public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

    /// <summary>Defines the all zeros matrix.</summary>
    public static readonly Matrix4x4 Zero = new Matrix4x4(Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero);

    /// <summary>Defines the all ones matrix.</summary>
    public static readonly Matrix4x4 One = new Matrix4x4(Vector4.One, Vector4.One, Vector4.One, Vector4.One);

    private readonly Vector4 _x;
    private readonly Vector4 _y;
    private readonly Vector4 _z;
    private readonly Vector4 _w;

    /// <summary>Initializes a new instance of the <see cref="Matrix4x4" /> struct.</summary>
    /// <param name="x">The value of the x-dimension.</param>
    /// <param name="y">The value of the y-dimension.</param>
    /// <param name="z">The value of the z-dimension.</param>
    /// <param name="w">The value of the w-dimension.</param>
    public Matrix4x4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    /// <summary>Gets the value of the x-dimension.</summary>
    public Vector4 X => _x;

    /// <summary>Gets the value of the y-dimension.</summary>
    public Vector4 Y => _y;

    /// <summary>Gets the value of the z-dimension.</summary>
    public Vector4 Z => _z;

    /// <summary>Gets the value of the w-dimension.</summary>
    public Vector4 W => _w;

    /// <summary>Compares two matrices to determine equality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
        => (left.X == right.X)
        && (left.Y == right.Y)
        && (left.Z == right.Z)
        && (left.W == right.W);

    /// <summary>Compares two matrices to determine inequality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
        => (left.X != right.X)
        || (left.Y != right.Y)
        || (left.Z != right.Z)
        || (left.W != right.W);

    /// <summary>Computes the product of a matrix and a float.</summary>
    /// <param name="left">The matrix to multiply by <paramref name="right" />.</param>
    /// <param name="right">The float which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Matrix4x4 operator *(Matrix4x4 left, float right) => new Matrix4x4(
        left.X * right,
        left.Y * right,
        left.Z * right,
        left.W * right
    );

    /// <summary>Computes the product of two matrices.</summary>
    /// <param name="left">The matrix to multiply by <paramref name="right" />.</param>
    /// <param name="right">The matrix which is used to multiply <paramref name="left" />.</param>
    /// <returns>The product of <paramref name="left" /> multipled by <paramref name="right" />.</returns>
    public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
    {
        var transposed = Transpose(right);

        return new Matrix4x4(
            DotRows(left, transposed.X),
            DotRows(left, transposed.Y),
            DotRows(left, transposed.Z),
            DotRows(left, transposed.W)
        );

        static Vector4 DotRows(Matrix4x4 left, Vector4 right)
        {
            return new Vector4(
                Vector4.Dot(left.X, right),
                Vector4.Dot(left.Y, right),
                Vector4.Dot(left.Z, right),
                Vector4.Dot(left.W, right)
            );
        }
    }

    /// <summary>Creates a matrix from a rotation.</summary>
    /// <param name="rotation">The rotation of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="rotation" />.</returns>
    public static Matrix4x4 CreateFromRotation(Quaternion rotation)
    {
        var w2 = rotation.W * rotation.W;
        var x2 = rotation.X * rotation.X;
        var y2 = rotation.Y * rotation.Y;
        var z2 = rotation.Z * rotation.Z;

        var wz = 2 * rotation.W * rotation.Z;
        var xz = 2 * rotation.X * rotation.Z;
        var xy = 2 * rotation.X * rotation.Y;
        var wx = 2 * rotation.W * rotation.X;
        var wy = 2 * rotation.W * rotation.Y;
        var yz = 2 * rotation.Y * rotation.Z;

        return new Matrix4x4(
            new Vector4(w2 + x2 - y2 - z2, wz + xy, xz - wy, 0),
            new Vector4(xy - wz, w2 - x2 + y2 - z2, wx + yz, 0),
            new Vector4(wy + xz, yz - wx, w2 - x2 - y2 + z2, 0),
            Vector4.UnitW
        );
    }

    /// <summary>Creates a matrix from a transform.</summary>
    /// <param name="transform">The transform of the matrix.</param>
    /// <returns>A matrix that represents <paramref name="transform" />.</returns>
    public static Matrix4x4 CreateFromTransform(Transform transform)
    {
        var rotation3x3 = Matrix3x3.CreateFromRotation(transform.Rotation);

        var x = new Vector4(rotation3x3.X * transform.Scale.X, 0.0f);
        var y = new Vector4(rotation3x3.Y * transform.Scale.Y, 0.0f);
        var z = new Vector4(rotation3x3.Z * transform.Scale.Z, 0.0f);
        var w = new Vector4(transform.Translation.X, transform.Translation.Y, transform.Translation.Z, 1.0f);

        return new Matrix4x4(x, y, z, w);
    }

    /// <summary>Compares two matrices to determine approximate equality.</summary>
    /// <param name="left">The matrix to compare with <paramref name="right" />.</param>
    /// <param name="right">The matrix to compare with <paramref name="left" />.</param>
    /// <param name="epsilon">The maximum (exclusive) difference between <paramref name="left" /> and <paramref name="right" /> for which they should be considered equivalent.</param>
    /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> differ by no more than <paramref name="epsilon" />; otherwise, <c>false</c>.</returns>
    public static bool EqualsEstimate(Matrix4x4 left, Matrix4x4 right, Matrix4x4 epsilon)
        => Vector4.EqualsEstimate(left.X, right.X, epsilon.X)
        && Vector4.EqualsEstimate(left.Y, right.Y, epsilon.Y)
        && Vector4.EqualsEstimate(left.Z, right.Z, epsilon.Z)
        && Vector4.EqualsEstimate(left.W, right.W, epsilon.W);

    /// <summary>Transposes a matrix.</summary>
    /// <param name="value">The matrix to transpose.</param>
    /// <returns>The transposition of <paramref name="value" />.</returns>
    public static Matrix4x4 Transpose(Matrix4x4 value) => new Matrix4x4(
        new Vector4(value.X.X, value.Y.X, value.Z.X, value.W.X),
        new Vector4(value.X.Y, value.Y.Y, value.Z.Y, value.W.Y),
        new Vector4(value.X.Z, value.Y.Z, value.Z.Z, value.W.Z),
        new Vector4(value.X.W, value.Y.W, value.Z.W, value.W.W)
    );

    /// <inheritdoc />
    public override bool Equals(object? obj) => (obj is Matrix4x4 other) && Equals(other);

    /// <inheritdoc />
    public bool Equals(Matrix4x4 other) => this == other;

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

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

    /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="X" /> set to the specified value.</summary>
    /// <param name="x">The new value of the x-dimension.</param>
    /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="X" /> set to <paramref name="x" />.</returns>
    public Matrix4x4 WithX(Vector4 x) => new Matrix4x4(x, Y, Z, W);

    /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="Y" /> set to the specified value.</summary>
    /// <param name="y">The new value of the y-dimension.</param>
    /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="Y" /> set to <paramref name="y" />.</returns>
    public Matrix4x4 WithY(Vector4 y) => new Matrix4x4(X, y, Z, W);

    /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="Z" /> set to the specified value.</summary>
    /// <param name="z">The new value of the z-dimension.</param>
    /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="Z" /> set to <paramref name="z" />.</returns>
    public Matrix4x4 WithZ(Vector4 z) => new Matrix4x4(X, Y, z, W);

    /// <summary>Creates a new <see cref="Matrix4x4" /> instance with <see cref="W" /> set to the specified value.</summary>
    /// <param name="w">The new value of the w-dimension.</param>
    /// <returns>A new <see cref="Matrix4x4" /> instance with <see cref="W" /> set to <paramref name="w" />.</returns>
    public Matrix4x4 WithW(Vector4 w) => new Matrix4x4(X, Y, Z, w);
}
