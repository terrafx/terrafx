// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Graphics.Geometry3D
{
    /// <summary>The three triangle indices into the vertex array.</summary>
    public struct TriangleInidices {
        /// <summary>The three triangle indices into the vertex array.
        /// NOTE: to be converted to Vector3 of uint.</summary>
        public (uint, uint, uint) Indices;

        /// <summary>Constructs the triangle indices tuple from the three indices into the vertex array.</summary>
        public TriangleInidices(uint a, uint b, uint c)
        {
            Indices = (a, b, c);
        }
    }

    /// <summary>Defines a 3D Mesh via its size along x, y, z, meaning width, height and depth.</summary>
    public readonly struct Mesh : IEquatable<Mesh>, IFormattable
    {
        /// <summary>Defines a <see cref="Mesh" /> of unit length.</summary>
        public static readonly Mesh Triangle = new Mesh(
            new Vector3[] { new Vector3(-1, -1, 0), new Vector3(1, -1, 0), new Vector3(0, 1, 0) },
            new Vector3[] { new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1) },
            new TriangleInidices[] { new TriangleInidices(0,1,2) });

        private readonly Vector3[] _vertices;

        private readonly Vector3[] _normals;

        private readonly TriangleInidices[] _triangleIndices;

        /// <summary>Initializes a new instance of the <see cref="Mesh" /> struct.</summary>
        /// <param name="vertices"></param>
        /// <param name="normals"></param>
        /// <param name="triangleIndices"></param>
        public Mesh(Vector3[] vertices, Vector3[] normals, TriangleInidices[] triangleIndices)
        {
            _vertices = vertices;
            _normals = normals;
            _triangleIndices = triangleIndices;
        }

        /// <summary>Initializes a new instance of the <see cref="Mesh" /> struct.</summary>
        /// <param name="other">The other mesh to clone.</param>
        public Mesh(Mesh other)
        {
            _vertices = other.Vertices;
            _normals = other.Normals;
            _triangleIndices = other.TriangleIndices;
        }

        /// <summary>Gets the Vertex array.</summary>
        public Vector3[] Vertices => _vertices;

        /// <summary>Gets the Normals array.</summary>
        public Vector3[] Normals => _normals;

        /// <summary>Gets the TriangleIdices array.</summary>
        public TriangleInidices[] TriangleIndices => _triangleIndices;

        /// <summary>Compares two <see cref="Mesh" /> instances to determine equality.</summary>
        /// <param name="left">The <see cref="Mesh" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Mesh" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Mesh left, Mesh right) => left.Vertices == right.Vertices && left.Normals == right.Normals && left.TriangleIndices == right.TriangleIndices;

        /// <summary>Compares two <see cref="Mesh" /> instances to determine inequality.</summary>
        /// <param name="left">The <see cref="Mesh" /> to compare with <paramref name="right" />.</param>
        /// <param name="right">The <see cref="Mesh" /> to compare with <paramref name="left" />.</param>
        /// <returns><c>true</c> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Mesh left, Mesh right) => !(left == right);

        // ----- methods.public -----

        // -- equality and similarity --

        /// <inheritdoc />
        public override bool Equals(object? obj) => (obj is Mesh other) && Equals(other);

        /// <inheritdoc />
        public bool Equals(Mesh other) => this == other;

        // -- state reporting (GetHashCode, ToString) --

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            {
                hashCode.Add(Vertices);
                hashCode.Add(Normals);
                hashCode.Add(TriangleIndices);
            }
            return hashCode.ToHashCode();
        }

        /// <inheritdoc />
        public override string ToString() => ToString(format: null, formatProvider: null);

        /// <inheritdoc />
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            var separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return new StringBuilder(5 + separator.Length)
                .Append('<')
                .Append("Vs" + Vertices.Length.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append("Ns" + Normals.Length.ToString(format, formatProvider))
                .Append(separator)
                .Append(' ')
                .Append("Ts" + TriangleIndices.Length.ToString(format, formatProvider))
                .Append('>')
                .ToString();
        }

        // -- new instance with some member changed (With*) --

        /// <summary>Creates a new <see cref="Mesh" /> instance with <see cref="Size" /> set to the specified values.
        /// Requires all three arrays, since they need to be consistent with each other.</summary>
        /// <param name="vertices">The new vertex array to use.</param>
        /// <param name="normals">The new normals array to use.</param>
        /// <param name="triangleIndices">The new triangle indices array to use.</param>
        /// <returns>A new <see cref="Mesh" /> instance with the new values.</returns>
        public Mesh WithVsNsTs(Vector3[] vertices, Vector3[] normals, TriangleInidices[] triangleIndices) => new Mesh(vertices, normals, triangleIndices);
    }
}
