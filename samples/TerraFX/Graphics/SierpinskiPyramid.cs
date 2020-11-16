// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics
{
    internal class SierpinskiPyramid
    {
        internal static (List<Vector3> vertices, List<ushort[]> indices) CreateMeshPlain(int recursionDepth)
        {
            //         d3
            //         .
            //        /|\
            //       / | \
            //      /  |  \
            //     /   |   \
            //    /    |    \
            //  a'\    |    /'b
            //  0   \  |  /   1
            //        \|/
            //         '
            //         c2

            float scale = 0.5f;
            float r1 = scale;
            float r3 = scale / MathF.Sqrt(3);
            float r6 = scale / MathF.Sqrt(6);
            var vertices = new List<Vector3>();

            var a = new Vector3(-r1, -r6, r3);
            var b = new Vector3(+r1, -r6, r3);
            var c = new Vector3(0, -r6, -2 * r3);
            var d = new Vector3(0, 3 * r6, 0);
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);

            var indices = new List<ushort[]>();
            // clockwise when looking at the triangle from the inside
            indices.Add(new ushort[] { 2, 0, 1 }); // bottom
            indices.Add(new ushort[] { 3, 0, 2 }); // left
            indices.Add(new ushort[] { 3, 2, 1 }); // right
            indices.Add(new ushort[] { 3, 1, 0 }); // back

            return (vertices, indices);
        }
        internal static (List<Vector3> vertices, List<Vector3> normals, List<ushort[]> indices) CreateMeshWithNormals(int recursionDepth)
        {
            //         d3
            //         .
            //        /|\
            //       / | \
            //      /  |  \
            //     /   |   \
            //    /    |    \
            //  a'\    |    /'b
            //  0   \  |  /   1
            //        \|/
            //         '
            //         c2

            float scale = 0.5f;
            float r1 = scale;
            float r3 = scale / MathF.Sqrt(3);
            float r6 = scale / MathF.Sqrt(6);
            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();

            var a = new Vector3(-r1, -r6, r3);
            var b = new Vector3(+r1, -r6, r3);
            var c = new Vector3(0, -r6, -2 * r3);
            var d = new Vector3(0, 3 * r6, 0);
            vertices.AddRange(new[] { a, b, c }); // replicate vertices in order for 
            vertices.AddRange(new[] { a, c, d }); // normals be different
            vertices.AddRange(new[] { b, d, c }); // in spite of normal interpolation
            vertices.AddRange(new[] { a, d, b }); // we want a flat surface shading effect
            var abcNormal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            var acdNormal = Vector3.Normalize(Vector3.Cross(c - a, d - a));
            var bdcNormal = Vector3.Normalize(Vector3.Cross(d - b, c - b));
            var adbNormal = Vector3.Normalize(Vector3.Cross(d - a, b - a));
            normals.AddRange(new Vector3[] { abcNormal, abcNormal, abcNormal });
            normals.AddRange(new Vector3[] { acdNormal, acdNormal, acdNormal });
            normals.AddRange(new Vector3[] { bdcNormal, bdcNormal, bdcNormal });
            normals.AddRange(new Vector3[] { adbNormal, adbNormal, adbNormal });

            var indices = new List<ushort[]>();
            // clockwise when looking at the triangle from the inside
            indices.Add(new ushort[] { 0, 1, 2 }); // bottom
            indices.Add(new ushort[] { 3, 4, 5 }); // left
            indices.Add(new ushort[] { 6, 7, 8 }); // right
            indices.Add(new ushort[] { 9, 10, 11 }); // back

            return (vertices, normals, indices);
        }
    }
}
