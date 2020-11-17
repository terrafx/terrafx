// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics
{
    internal class SierpinskiPyramid
    {
        #region Tetrahedron
        internal static (List<Vector3> vertices, List<uint> indices) CreateMeshTetrahedron(int recursionDepth)
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

            var a = new Vector3(-r1,      r3,    -r6);
            var b = new Vector3(+r1,      r3,    -r6);
            var c = new Vector3(0  , -2 * r3,    -r6);
            var d = new Vector3(0  ,       0, 3 * r6);


            a = new Vector3(-0.25f, +0.25f, 0);
            b = new Vector3(+0.25f, +0.25f, 0);
            c = new Vector3(+0.25f, -0.25f, 0);
            d = new Vector3(-0.25f, -0.25f, 0);

            TetrahedronRecursion(recursionDepth, a, b, c, d, vertices, indices);

            //var dict = new Dictionary<Vector3, int>();
            //foreach (var v in vertices)
            //{
            //    if (dict.ContainsKey(v))
            //        dict[v]++;
            //    else
            //        dict[v] = 1;
            //}
            //foreach (var v in dict.Keys)
            //{
            //    if (dict[v] != 3 && dict[v] != 6)
            //        System.Diagnostics.Debugger.Break();
            //}

            //for (int i = 0; i < vertices.Count; i+=3)
            //{
            //    a = vertices[i];
            //    b = vertices[i+1];
            //    c = vertices[i+2];
            //    float ab = (a - b).Length;
            //    float ac = (a - c).Length;
            //    float bc = (b - c).Length;
            //    if (MathF.Abs((ac / ab) - 1) > 0.1 || MathF.Abs((ac / bc) - 1) > 0.1)
            //    {
            //        System.Diagnostics.Debugger.Break();
            //    }
            //}

            return (vertices, indices);
        }

        private static void TetrahedronRecursion(int recursionDepth
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
                BaseCaseTetrahedron(a, b, c, d, vertices, indices);
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
                TetrahedronRecursion(recursionDepth - 1, a, e, f, h, vertices, indices);
                TetrahedronRecursion(recursionDepth - 1, e, b, g, i, vertices, indices);
                TetrahedronRecursion(recursionDepth - 1, f, g, c, j, vertices, indices);
                TetrahedronRecursion(recursionDepth - 1, h, i, j, d, vertices, indices);
            }
        }

        private static void BaseCaseTetrahedron(
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

            var i = indices.Count;
            for (int j = 0; j < 12; j++)
            {
                indices.Add((uint)(i + j));
            }
        }
        #endregion Tetrahedron

        #region Quad
        internal static (List<Vector3> vertices, List<uint> indices) CreateMeshQuad(int recursionDepth)
        {
            float r = 0.25f;
            var vertices = new List<Vector3>();
            var indices = new List<uint>();

            //
            //  a-------b    y          in this setup 
            //  | \     |    ^     z    the origin o
            //  |   \   |    |   /      is in the middle
            //  |     \ |    | /        of the rendered scene
            //  d-------c    o------>x  (z is into the page, so xyz is left-handed)
            //  

            var a = new Vector3(-r, +r, 0);
            var b = new Vector3(+r, +r, 0);
            var c = new Vector3(+r, -r, 0);
            var d = new Vector3(-r, -r, 0);

            QuadRecursion(recursionDepth, a, b, c, d, vertices, indices);

            return (vertices, indices);
        }

        private static void QuadRecursion(int recursionDepth
            , Vector3 a, Vector3 b, Vector3 c, Vector3 d
            , List<Vector3> vertices, List<uint> indices)
        {
            //
            //  a-e---f-b    y          in this setup 
            //  l m   n g    ^     z    the origin o
            //  |   \   |    |   /      is in the middle
            //  k p   o h    | /        of the rendered scene
            //  d-j---i-c    o------>x  (z is into the page, so xyz is left-handed)
            //  
            if (recursionDepth == 0)
            {
                BaseCaseQuad(a, b, c, d, vertices, indices);
            }
            else
            {
                // subdivide all tetrahedron edges at their midpoints
                // and form four new tetrahedrons from them
                Vector3 e = a * 2 / 3 + b * 1 / 3;
                Vector3 f = a * 1 / 3 + b * 2 / 3;
                Vector3 g = b * 2 / 3 + c * 1 / 3;
                Vector3 h = b * 1 / 3 + c * 2 / 3;
                Vector3 i = c * 2 / 3 + d * 1 / 3;
                Vector3 j = c * 1 / 3 + d * 2 / 3;
                Vector3 k = d * 2 / 3 + a * 1 / 3;
                Vector3 l = d * 1 / 3 + a * 2 / 3;
                Vector3 m = a * 2 / 3 + c * 1 / 3;
                Vector3 o = a * 1 / 3 + c * 2 / 3;
                Vector3 n = b * 2 / 3 + d * 1 / 3;
                Vector3 p = b * 1 / 3 + d * 2 / 3;
                QuadRecursion(recursionDepth - 1, a, e, m, l, vertices, indices);
                QuadRecursion(recursionDepth - 1, f, b, g, n, vertices, indices);
                QuadRecursion(recursionDepth - 1, o, h, c, i, vertices, indices);
                QuadRecursion(recursionDepth - 1, k, p, j, d, vertices, indices);
            }
        }

        private static void BaseCaseQuad(
            Vector3 a, Vector3 b, Vector3 c, Vector3 d
            , List<Vector3> vertices, List<uint> indices)
        {
            //
            //  a-------b    y          in this setup 
            //  | \     |    ^     z    the origin o
            //  |   \   |    |   /      is in the middle
            //  |     \ |    | /        of the rendered scene
            //  d-------c    o------>x  (z is into the page, so xyz is left-handed)
            //  

            // Clockwise when looking at the triangle from the outside.
            // Replicate vertices in order for normals be different.
            // In spite of normal interpolation we want a flat surface shading effect
            vertices.AddRange(new[] { a, b, c }); // both windings
            vertices.AddRange(new[] { a, c, b }); // so we can rotate
            vertices.AddRange(new[] { a, c, d }); // 360 degrees 
            vertices.AddRange(new[] { a, d, c }); // and still see it

            var i = indices.Count;
            for (int j = 0; j < 12; j++)
            {
                indices.Add((uint)(i + j));
            }
        }
        #endregion Quad

        #region Normals
        internal static List<Vector3> MeshNormals(List<Vector3> vertices)
        {
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

            return normals;
        }
        #endregion Normals
    }
}
