// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Graphics.Geometry3D
{
    /// <summary>Defines a 3D Box via its size along x, y, z, meaning width, height and depth.
    /// It is assumed to be centered on the origin, such that it covers all eight quadrants equally.</summary>
    public readonly struct Box : IEquatable<Box>, IFormattable
    {
        /// <summary>Defines a <see cref="Box" /> of unit length.</summary>
        public static readonly Box One = new Box(Vector3.One);

        private readonly Vector3 _size;

        /// <summary>Initializes a new instance of the <see cref="Box" /> struct.</summary>
        /// <param name="size">The size of the instance.</param>
        public Box(Vector3 size = default)
        {
            _size = size;
        }

        /// <summary>Initializes a new instance of the <see cref="Box" /> struct.</summary>
        /// <param name="sizeX">The x-coordinate of the instance.</param>
        /// <param name="sizeY">The y-coordinate of the instance.</param>
        /// <param name="sizeZ">The y-coordinate of the instance.</param>
        public Box(float sizeX, float sizeY, float sizeZ)
        {
            _size = new Vector3(sizeX, sizeY, sizeZ);
        }

        /// <summary>Gets the size of the instance.</summary>
        public Vector3 Size => _size;

        /// <summary>Compares two <see cref="Box" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Box" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Box" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Box left, Box right) => left.Size == right.Size;

        /// <summary>Compares two <see cref="Box" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Box" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Box" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Box left, Box right) => left.Size != right.Size;

        /// <summary>Computes the center of mass of the given <see cref="Box" /> instance.</summary>
        /// <param name="box">The <see cref="Box" /> for which to compute the corners.</param>
        /// <returns>The center of mass of the <see cref="Box" />.</returns>
        public static Vector3 Center(Box box) => Vector3.Zero;

        /// <summary>Creates an array of the 8 corners of the given <see cref="Box" /> instance.</summary>
        /// <param name="box">The box for which to compute the corners.</param>
        /// <returns>The array of corners, each of type <see cref="Vector3" />.</returns>
        public static Vector3[] Corners(Box box)
        {
            var nx = -0.5f * box.Size.X;
            var px = +0.5f * box.Size.X;
            var ny = -0.5f * box.Size.Y;
            var py = +0.5f * box.Size.Y;
            var nz = -0.5f * box.Size.Z;
            var pz = +0.5f * box.Size.Z;

            //  Y
            //  |  g-------h 
            //    /.      /|  Z
            //   / .     / | /
            //  c-------d  | 
            //  |  e....|..f
            //  | .     | /
            //  |.      |/
            //  a-------b   ---X

            var a = new Vector3(nx, ny, nz);
            var b = new Vector3(px, ny, nz);
            var c = new Vector3(nx, py, nz);
            var d = new Vector3(px, py, nz);
            var e = new Vector3(nx, ny, pz);
            var f = new Vector3(px, ny, pz);
            var g = new Vector3(nx, py, pz);
            var h = new Vector3(px, py, pz);

            var corners = new Vector3[8] { a, b, c, d, e, f, g, h };
            return corners;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Box other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Box other) => this == other;

        /// <summary>Tests if two <see cref="Box" /> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="left">The left instance to compare.</param>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns>True if similar, false otherwise.</returns>
        public static bool EqualEstimate(Box left, Box right, Box epsilon) => Vector3.EqualEstimate(left._size, right._size, epsilon._size);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Size);
            }
            return hashCode.ToHashCode();
        }

        /// <summary>Computes the largest <see cref="Box" /> that is fully contained in both, 'this' and 'box'.</summary>
        /// <param name="other">The other <see cref="Box" /> to consider.</param>
        /// <returns>The resulting new instance.</returns>
        public Box Intersection(Box other) => new Box(Vector3.Min(Size, other.Size));

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return new StringBuilder(3)
                .Append('<')
                .Append("Size" + Size.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }

        /// <summary>Computes the smallest <see cref="Box" /> that can fit both, 'this' and 'box'.</summary>
        /// <param name="other">The other <see cref="Box" /> to fit.</param>
        /// <returns>The resulting new instance.</returns>
        public Box Union(Box other) => new Box(Vector3.Max(Size, other.Size));

        /// <summary>Creates a new <see cref="Box" /> instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new <see cref="Box" /> instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Box WithSize(Vector3 value) => new Box(value);
    }
}
