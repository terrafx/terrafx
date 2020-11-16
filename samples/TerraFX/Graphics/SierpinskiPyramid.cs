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
            float scale = 0.5f;
            var vertices = new List<Vector3>();
            var indices = new List<ushort[]>();

            //         d
            //         .
            //        /|\
            //       / | \
            //      /  |  \
            //     /   |   \
            //    /    |    \
            //  a'\''''|''''/'b
            //      \  |  /   
            //        \|/
            //         '
            //         c

            float r1 = scale;
            float r3 = scale / MathF.Sqrt(3);
            float r6 = scale / MathF.Sqrt(6);

            var a = new Vector3(-r1, -r6, r3);
            var b = new Vector3(+r1, -r6, r3);
            var c = new Vector3(0, -r6, -2 * r3);
            var d = new Vector3(0, 3 * r6, 0);

            PyramidRecursion(recursionDepth, a, b, c, d, scale, vertices, indices);

            return (vertices, indices);
        }

        private static void PyramidRecursion(int recursionDepth
            , Vector3 a, Vector3 b, Vector3 c, Vector3 d, float scale
            , List<Vector3> vertices, List<ushort[]> indices)
        {
            //         d
            //         .
            //        /|\
            //       / | \
            //      /h |  \i
            //     /   |   \
            //    /    |j    \
            //  a'\'''e|''''/'b
            //     f\  |  /g    
            //        \|/
            //         '
            //         c 

            BaseCaseAdd(a, b, c, d, vertices, indices);
        }

        private static void BaseCaseAdd(
            Vector3 a, Vector3 b, Vector3 c, Vector3 d
            , List<Vector3> vertices, List<ushort[]> indices)
        {
            //         d
            //         .
            //        /|\
            //       / | \
            //      /  |  \
            //     /   |   \
            //    /    |    \
            //  a'\''''|''''/'b
            //      \  |  /   
            //        \|/
            //         '
            //         c

            // Clockwise when looking at the triangle from the inside.
            // Replicate vertices in order for normals be different.
            // In spite of normal interpolation we want a flat surface shading effect
            vertices.AddRange(new[] { a, b, c }); // bottom
            vertices.AddRange(new[] { a, c, d }); // left
            vertices.AddRange(new[] { b, d, c }); // right
            vertices.AddRange(new[] { a, d, b }); // back

            var i = indices.Count;
            for (int j = 0; j < 12; j += 3)
            {
                indices.Add(new ushort[] {
                    (ushort)(i + j),
                    (ushort)(i + j + 1),
                    (ushort)(i + j + 2) });
            }
        }

        internal static (List<Vector3> vertices, List<Vector3> normals, List<ushort[]> indices) CreateMeshWithNormals(int recursionDepth)
        {
            (List<Vector3> vertices, List<ushort[]> indices)
                = CreateMeshPlain(recursionDepth);

            var n4 = new Vector3[4];
            for (int i = 0; i < 12; i += 3)
            {
                Vector3 a = vertices[i + 0];
                Vector3 b = vertices[i + 1];
                Vector3 c = vertices[i + 2];
                n4[i/3] = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            }
            var normals = new List<Vector3>();
            for (int i = 0; i < vertices.Count; i += 3)
            {
                // same normal for all three triangle vertices to ensure flat shaded surfaces
                var n = n4[i % 4];
                normals.AddRange(new Vector3[] { n, n, n });
            }

            return (vertices, normals, indices);
        }
    }
}
