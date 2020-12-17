// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Graphics.Geometry3D;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Utilities;

namespace TerraFX.Samples
{
    /// <summary>Defines a ToMesh that is placed and oriented via its ToWorld Transform and can be set to parallel or prespective projection.</summary>
    public static class ToMeshExtensions
    {
        public static Mesh ToMesh(this Box box)
        {
            //  Y
            //  |  g-------h 
            //    /.      /|  Z
            //   / .     / | /
            //  c-------d  | 
            //  |  e....|..f
            //  | .     | /
            //  |.      |/
            //  a-------b   ---X

            var vertices = new List<Vector3>();
            {
                var corners = Box.Corners(box);
                var a = corners[0];
                var b = corners[1];
                var c = corners[2];
                var d = corners[3];
                var e = corners[4];
                var f = corners[5];
                var g = corners[6];
                var h = corners[7];

                // front triangles
                vertices.AddRange(new[] { a, c, d });
                vertices.AddRange(new[] { a, d, b });

                // top triangles
                vertices.AddRange(new[] { c, g, h });
                vertices.AddRange(new[] { c, h, d });

                // right triangles
                vertices.AddRange(new[] { d, h, f });
                vertices.AddRange(new[] { d, f, b });

                // bottom triangles
                vertices.AddRange(new[] { a, b, f });
                vertices.AddRange(new[] { a, f, e });

                // left triangles
                vertices.AddRange(new[] { e, g, c });
                vertices.AddRange(new[] { e, c, a });

                // back triangles
                vertices.AddRange(new[] { e, f, h });
                vertices.AddRange(new[] { e, h, g });
            }

            var vs = vertices.ToArray();

            var indices = new List<TriangleInidices>();
            var count = vs.Length;
            for (uint j = 0; j < count; j += 3)
            {
                indices.Add(new TriangleInidices(j, j + 1, j + 2));
            }

            var normals = new List<Vector3>();
            for (var i = 0; i < vs.Length; i += 3)
            {
                // same normal for all three triangle vertices to ensure flat shaded surfaces
                var a = vertices[i + 0];
                var b = vertices[i + 1];
                var c = vertices[i + 2];
                var n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
                normals.AddRange(new Vector3[] { n, n, n });
            }

            var ns = normals.ToArray();
            var ts = indices.ToArray();
            var mesh = new Mesh(vs, ns, ts);
            return mesh;
        }

        public static Mesh ToMesh(this Tetrahedron tetrahedron)
        {
            var vertices = new List<Vector3>();

            {
                var corners = Tetrahedron.Corners(tetrahedron);
                var a = corners[0];
                var b = corners[1];
                var c = corners[2];
                var d = corners[3];

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

                // Clockwise when looking at the triangle from the outside.
                // Replicate vertices in order for normals be different.
                // In spite of normal interpolation we want a flat surface shading effect
                vertices.AddRange(new[] { a, b, c }); // bottom
                vertices.AddRange(new[] { a, c, d }); // left
                vertices.AddRange(new[] { b, d, c }); // right
                vertices.AddRange(new[] { a, d, b }); // back
            }

            var vs = vertices.ToArray();
            var indices = new List<TriangleInidices>();
            var count = vs.Length;

            for (uint j = 0; j < count; j += 3)
            {
                indices.Add(new TriangleInidices(j, j + 1, j + 2));
            }

            var normals = new List<Vector3>();

            for (var i = 0; i < vs.Length; i += 3)
            {
                // same normal for all three triangle vertices to ensure flat shaded surfaces
                var a = vertices[i + 0];
                var b = vertices[i + 1];
                var c = vertices[i + 2];
                var n = Vector3.Normalize(Vector3.Cross(b - a, c - a));
                normals.AddRange(new Vector3[] { n, n, n });
            }

            var ns = normals.ToArray();
            var ts = indices.ToArray();
            var mesh = new Mesh(vs, ns, ts);
            return mesh;
        }
    }
}
