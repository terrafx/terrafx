// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics
{
    internal class SierpinskiPyramid
    {
        internal static (List<Vector3> vertices, List<uint> indices) CreateMeshPlain(int recursionDepth)
        {
            float scale = 0.5f;
            var vertices = new List<Vector3>();
            var indices = new List<uint>();

            //         d               
            //         .            
            //        /|\         y          in this setup 
            //       / | \        ^     z    the origin o
            //      /  |  \       |   /      is in the middle
            //     /   |   \      | /        of the rendered scene
            //    /    |    \     o------>x  (z is into the page, so xyz is left-handed)
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

            PyramidRecursion(recursionDepth, a, b, c, d, vertices, indices);

            var dict = new Dictionary<Vector3, int>();
            foreach (var v in vertices)
            {
                if (dict.ContainsKey(v))
                    dict[v]++;
                else
                    dict[v] = 1;
            }
            foreach (var v in dict.Keys)
            {
                if (dict[v] != 3 && dict[v] != 6)
                    System.Diagnostics.Debugger.Break();
            }

            for (int i = 0; i < vertices.Count; i+=3)
            {
                a = vertices[i];
                b = vertices[i+1];
                c = vertices[i+2];
                float ab = (a - b).Length;
                float ac = (a - c).Length;
                float bc = (b - c).Length;
                if (MathF.Abs((ac / ab) - 1) > 0.1 || MathF.Abs((ac / bc) - 1) > 0.1)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }

            return (vertices, indices);
        }

        private static void PyramidRecursion(int recursionDepth
            , Vector3 a, Vector3 b, Vector3 c, Vector3 d
            , List<Vector3> vertices, List<uint> indices)
        {
            //         d
            //         .
            //        /|\         y          in this setup 
            //       / | \        ^     z    the origin o
            //      /h |  \i      |   /      is in the middle
            //     /   |   \      | /        of the rendered scene
            //    /    |j    \    o------>x  (z is into the page, so xyz is left-handed)
            //  a'\'''e|''''/'b
            //     f\  |  /g    
            //        \|/
            //         '
            //         c 
            if (recursionDepth == 0)
            {
                BaseCaseAdd(a, b, c, d, vertices, indices);
            }
            else
            {
                // subdivide all tetrahedron edges at their midpoints
                // and form four new tetrahedrons from them
                Vector3 e = (a + b) / 2;
                Vector3 f = (a + c) / 2;
                Vector3 g = (b + c) / 2;
                Vector3 h = (a + d) / 2;
                Vector3 i = (b + d) / 2;
                Vector3 j = (c + d) / 2;
                PyramidRecursion(recursionDepth - 1, a, e, f, h, vertices, indices);
                PyramidRecursion(recursionDepth - 1, e, b, g, i, vertices, indices);
                PyramidRecursion(recursionDepth - 1, f, g, c, j, vertices, indices);
                PyramidRecursion(recursionDepth - 1, h, i, j, d, vertices, indices);
            }
        }

        private static void BaseCaseAdd(
            Vector3 a, Vector3 b, Vector3 c, Vector3 d
            , List<Vector3> vertices, List<uint> indices)
        {
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
            vertices.AddRange(new[] { a, c, b }); // bottom
            vertices.AddRange(new[] { a, d, c }); // left
            vertices.AddRange(new[] { b, c, d }); // right
            vertices.AddRange(new[] { a, b, d }); // back
            ////// same again in reverse winding order
            //vertices.AddRange(new[] { a, b, c }); // bottom
            //vertices.AddRange(new[] { a, c, d }); // left
            //vertices.AddRange(new[] { b, d, c }); // right
            //vertices.AddRange(new[] { a, d, b }); // back

            var i = indices.Count;
            for (int j = 0; j < 12; j++)
            {
                indices.Add((uint)(i + j));
            }
        }

        internal static (List<Vector3> vertices, List<Vector3> normals, List<uint> indices) CreateMeshWithNormals(int recursionDepth)
        {
            (List<Vector3> vertices, List<uint> indices)
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
