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
    /// It is assumed to be axis aligned and centered on Location,
    /// such that if Location is (0,0,0) the Box covers all eight quadrants equally.</summary>
    public readonly struct Box : IEquatable<Box>, IFormattable
    {
        /// <summary>Defines a <see cref="Box" /> of unit length.</summary>
        public static readonly Box One = new Box(Vector3.One);

        private readonly Vector3 _location;
        private readonly Vector3 _size;

        /// <summary>Initializes a new instance of the <see cref="Box" /> struct.</summary>
        /// <param name="size">The size for this instance.</param>
        /// <param name="location">The location for this instance.</param>
        public Box(Vector3 size = default, Vector3 location = default)
        {
            _location = location;
            _size = size;
        }

        /// <summary>Gets the location of the instance.
        /// Note that it is assumed that Location is also the center of the Box.</summary>
        public Vector3 Location => _location;

        /// <summary>Gets the size of the instance.</summary>
        public Vector3 Size => _size;

        /// <summary>Compares two <see cref="Box" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Box" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Box" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Box left, Box right) => left.Size == right.Size && left.Location == right.Location;

        /// <summary>Compares two <see cref="Box" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Box" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Box" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Box left, Box right) => !(left == right);

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

            var a = new Vector3(nx, ny, nz) + box.Location;
            var b = new Vector3(px, ny, nz) + box.Location;
            var c = new Vector3(nx, py, nz) + box.Location;
            var d = new Vector3(px, py, nz) + box.Location;
            var e = new Vector3(nx, ny, pz) + box.Location;
            var f = new Vector3(px, ny, pz) + box.Location;
            var g = new Vector3(nx, py, pz) + box.Location;
            var h = new Vector3(px, py, pz) + box.Location;

            var corners = new Vector3[8] { a, b, c, d, e, f, g, h };
            return corners;
        }

        /// <summary>
        /// The minimum coordinate value corner of the box (left, bottom, front).
        /// </summary>
        /// <returns>The position of the minimum corner.</returns>
        public Vector3 CornerMin() => Location - (Size / 2);
        /// <summary>
        /// The maximum coordinate value corner of the box (right, top, back).
        /// </summary>
        /// <returns>The position of the maximum corner.</returns>
        public Vector3 CornerMax() => Location + (Size / 2);

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
        public static bool EqualEstimate(Box left, Box right, Box epsilon) => Vector3.EqualEstimate(left._size, right._size, epsilon._size) && Vector3.EqualEstimate(left._location, right._location, epsilon._location);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Location);
                hashCode.Add(Size);
            }
            return hashCode.ToHashCode();
        }

        /// <summary>Computes the largest <see cref="Box" /> that is fully contained in both, 'this' and 'other'.</summary>
        /// <param name="other">The other <see cref="Box" /> to consider.</param>
        /// <returns>The resulting new instance.</returns>
        public Box Intersection(Box other)
        {
            Box intersectionBox = default;
            if (Location.X + (Size.X / 2) < other.Location.X - (Size.X / 2) ||
                Location.X - (Size.X / 2) > other.Location.X + (Size.X / 2) ||
                Location.Y + (Size.Y / 2) < other.Location.Y - (Size.Y / 2) ||
                Location.Y - (Size.Y / 2) > other.Location.Y + (Size.Y / 2) ||
                Location.Z + (Size.Z / 2) < other.Location.Z - (Size.Z / 2) ||
                Location.Z - (Size.Z / 2) > other.Location.Z + (Size.Z / 2))
            {
                return intersectionBox;
            }

            var cornerMin = Vector3.Max(CornerMin(), other.CornerMin());
            var cornerMax = Vector3.Min(CornerMax(), other.CornerMax());
            intersectionBox = new Box(cornerMax - cornerMin, (cornerMax + cornerMin) / 2);
            return intersectionBox;
        }

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
        public Box Union(Box other)
        {
            var cornerMin = Vector3.Min(CornerMin(), other.CornerMin());
            var cornerMax = Vector3.Max(CornerMax(), other.CornerMax());
            var intersectionBox = new Box(cornerMax - cornerMin, (cornerMax + cornerMin) / 2);
            return intersectionBox;
        }

        /// <summary>Creates a new <see cref="Box" /> instance with <see cref="Location" /> set to the specified value.</summary>
        /// <param name="value">The new location of the instance.</param>
        /// <returns>A new <see cref="Box" /> instance with <see cref="Location" /> set to <paramref name="value" />.</returns>
        public Box WithLocation(Vector3 value) => new Box(Size, value);

        /// <summary>Creates a new <see cref="Box" /> instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new <see cref="Box" /> instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Box WithSize(Vector3 value) => new Box(value, Location);
    }
}
