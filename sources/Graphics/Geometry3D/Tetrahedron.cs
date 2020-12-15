// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Graphics.Geometry3D
{
    /// <summary>Defines a 3D Tetrahedron via its size along x, y, z, meaning width, height and depth.
    /// Specifically, those x,y,z sizes refer to a box into which the Tetrahedron is inscribed.
    /// Only the back bottom edge of the box is also an edge of the Tetrahedron.
    /// The other two corners are at the midpoint of the bottom front edge and the center of the top face.
    /// Note that this makes very intuitive use of the size of the bounding box.
    /// The box is shifted up such that the tetrahedron center of mass at the origin.</summary>
    public readonly struct Tetrahedron : IEquatable<Tetrahedron>, IFormattable
    {
        /// <summary>Defines an equal sided <see cref="Tetrahedron" /> with edges that are unit length.</summary>
        public static readonly Tetrahedron One = new Tetrahedron(new Vector3(1, 0.86602544f, 0.75f));

        private readonly Vector3 _size;

        /// <summary>Initializes a new instance of the <see cref="Tetrahedron" /> struct.</summary>
        /// <param name="size">The size of the instance.</param>
        public Tetrahedron(Vector3 size = default)
        {
            _size = size;
        }

        /// <summary>Initializes a new instance of the <see cref="Tetrahedron" /> struct.</summary>
        /// <param name="sizeX">The x-coordinate of the instance.</param>
        /// <param name="sizeY">The y-coordinate of the instance.</param>
        /// <param name="sizeZ">The y-coordinate of the instance.</param>
        public Tetrahedron(float sizeX, float sizeY, float sizeZ)
        {
            _size = new Vector3(sizeX, sizeY, sizeZ);
        }

        /// <summary>Gets the size of the instance.</summary>
        public Vector3 Size => _size;

        /// <summary>Compares two <see cref="Tetrahedron" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Tetrahedron" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Tetrahedron" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Tetrahedron left, Tetrahedron right) => left.Size == right.Size;

        /// <summary>Compares two <see cref="Tetrahedron" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Tetrahedron" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Tetrahedron" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Tetrahedron left, Tetrahedron right) => left.Size != right.Size;

        /// <summary>Creates an array of the 8 corners of the given <see cref="Tetrahedron" /> instance.</summary>
        /// <param name="tetrahedron">The <see cref="Tetrahedron" /> for which to compute the corners.</param>
        /// <returns>The array of corners, each of type <see cref="Vector3" />.</returns>
        public static Vector3[] Corners(Tetrahedron tetrahedron)
        {
            var nx = -0.5f * tetrahedron.Size.X;
            var px = +0.5f * tetrahedron.Size.X;
            var ny = -0.5f * tetrahedron.Size.Y;
            var py = +0.5f * tetrahedron.Size.Y;
            var nz = -0.5f * tetrahedron.Size.Z;
            var pz = +0.5f * tetrahedron.Size.Z;

            //         d
            //         .
            //        /|\
            //       / | \        y          in this setup
            //      /  |  \       ^     z    the origin o
            //     /   |   \      |   /      is in the middle
            //    /    |    \     | /        of the rendered scene
            //  a'\''''|''''/'b   o------>x  (z is into the page, so xyz is left-handed)
            //      \  |  /
            //        \|/
            //         '
            //         c

            var shiftCenterToOrigin = new Vector3(0, 0.25f * ny, 0.5f * pz);
            var a = shiftCenterToOrigin + new Vector3(nx, py, nz);
            var b = shiftCenterToOrigin + new Vector3(px, py, nz);
            var c = shiftCenterToOrigin + new Vector3(0, ny, nz);
            var d = shiftCenterToOrigin + new Vector3(0, 0, pz);

            var corners = new Vector3[4] { a, b, c, d };
            return corners;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Tetrahedron other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Tetrahedron other) => this == other;

        /// <summary>Tests if two <see cref="Tetrahedron" /> instances have sufficiently similar values to see them as equivalent.
        /// Use this to compare values that might be affected by differences in rounding the least significant bits.</summary>
        /// <param name="left">The left instance to compare.</param>
        /// <param name="right">The right instance to compare.</param>
        /// <param name="epsilon">The threshold below which they are sufficiently similar.</param>
        /// <returns>True if similar, false otherwise.</returns>
        public static bool EqualEstimate(Tetrahedron left, Tetrahedron right, Tetrahedron epsilon) => Vector3.EqualEstimate(left._size, right._size, epsilon._size);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Size);
            }
            return hashCode.ToHashCode();
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

        /// <summary>Creates a new <see cref="Tetrahedron" /> instance with <see cref="Size" /> set to the specified value.</summary>
        /// <param name="value">The new size of the instance.</param>
        /// <returns>A new <see cref="Tetrahedron" /> instance with <see cref="Size" /> set to <paramref name="value" />.</returns>
        public Tetrahedron WithSize(Vector3 value) => new Tetrahedron(value);
    }
}
