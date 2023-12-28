// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Collections;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics;

internal static class SierpinskiPyramid
{
    internal static (UnmanagedValueList<Vector3> vertices, UnmanagedValueList<uint> indices) CreateMeshTetrahedron(int recursionDepth)
    {
        var scale = 1.0f;

        var vertices = UnmanagedValueList.Empty<Vector3>();
        var indices = UnmanagedValueList.Empty<uint>();

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

        var r1 = scale;
        var r3 = scale / MathF.Sqrt(3);
        var r6 = scale / MathF.Sqrt(6);

        var a = Vector3.Create(-r1, -r6 - 0.4f, r3);
        var b = Vector3.Create(+r1, -r6 - 0.4f, r3);
        var c = Vector3.Create(0, -r6 - 0.4f, -2 * r3);
        var d = Vector3.Create(0, (3 * r6) - 0.4f, 0);

        TetrahedronRecursion(recursionDepth, a, b, c, d, ref vertices, ref indices);

        return (vertices, indices);
    }

    private static void TetrahedronRecursion(int recursionDepth, Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref UnmanagedValueList<Vector3> vertices, ref UnmanagedValueList<uint> indices)
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
            BaseCaseTetrahedron(a, b, c, d, ref vertices, ref indices);
        }
        else
        {
            // subdivide all tetrahedron edges at their midpoints
            // and form four new tetrahedrons from them
            var e = (a + b) / 2;
            var f = (a + c) / 2;
            var g = (b + c) / 2;
            var h = (a + d) / 2;
            var i = (b + d) / 2;
            var j = (c + d) / 2;
            TetrahedronRecursion(recursionDepth - 1, a, e, f, h, ref vertices, ref indices);
            TetrahedronRecursion(recursionDepth - 1, e, b, g, i, ref vertices, ref indices);
            TetrahedronRecursion(recursionDepth - 1, f, g, c, j, ref vertices, ref indices);
            TetrahedronRecursion(recursionDepth - 1, h, i, j, d, ref vertices, ref indices);
        }
    }

    private static void BaseCaseTetrahedron(Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref UnmanagedValueList<Vector3> vertices, ref UnmanagedValueList<uint> indices)
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

        vertices.EnsureCapacity(vertices.Count + 12);

        // bottom
        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);

        // left
        vertices.Add(a);
        vertices.Add(c);
        vertices.Add(d);

        // right
        vertices.Add(b);
        vertices.Add(d);
        vertices.Add(c);

        // back
        vertices.Add(a);
        vertices.Add(d);
        vertices.Add(b);

        var i = indices.Count;

        for (nuint j = 0; j < 12; j++)
        {
            indices.Add((uint)(i + j));
        }
    }

    internal static (UnmanagedValueList<Vector3> vertices, UnmanagedValueList<uint> indices) CreateMeshQuad(int recursionDepth)
    {
        var r = 0.99f;

        var vertices = UnmanagedValueList.Empty<Vector3>();
        var indices = UnmanagedValueList.Empty<uint>();

        //
        //  a-------b    y          in this setup
        //  | \     |    ^     z    the origin o
        //  |   \   |    |   /      is in the middle
        //  |     \ |    | /        of the rendered scene
        //  d-------c    o------>x  (z is into the page, so xyz is left-handed)
        //

        var a = Vector3.Create(-r, +r, 0f);
        var b = Vector3.Create(+r, +r, 0f);
        var c = Vector3.Create(+r, -r, 0f);
        var d = Vector3.Create(-r, -r, 0f);

        QuadRecursion(recursionDepth, a, b, c, d, ref vertices, ref indices);

        return (vertices, indices);
    }

    private static void QuadRecursion(int recursionDepth, Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref UnmanagedValueList<Vector3> vertices, ref UnmanagedValueList<uint> indices)
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
            BaseCaseQuad(a, b, c, d, ref vertices, ref indices);
        }
        else
        {
            // subdivide all tetrahedron edges at their midpoints
            // and form four new tetrahedrons from them
            var s = 5 / 11f;
            var t = 1 - s;
            var e = (a * t) + (b * s);
            var f = (a * s) + (b * t);
            var g = (b * t) + (c * s);
            var h = (b * s) + (c * t);
            var i = (c * t) + (d * s);
            var j = (c * s) + (d * t);
            var k = (d * t) + (a * s);
            var l = (d * s) + (a * t);
            var m = (a * t) + (c * s);
            var o = (a * s) + (c * t);
            var n = (b * t) + (d * s);
            var p = (b * s) + (d * t);
            QuadRecursion(recursionDepth - 1, a, e, m, l, ref vertices, ref indices);
            QuadRecursion(recursionDepth - 1, f, b, g, n, ref vertices, ref indices);
            QuadRecursion(recursionDepth - 1, o, h, c, i, ref vertices, ref indices);
            QuadRecursion(recursionDepth - 1, k, p, j, d, ref vertices, ref indices);
        }
    }

    private static void BaseCaseQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, ref UnmanagedValueList<Vector3> vertices, ref UnmanagedValueList<uint> indices)
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

        vertices.EnsureCapacity(vertices.Count + 12);

        // Add both windings so we can rotate 360 degrees and still see it

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);

        vertices.Add(a);
        vertices.Add(c);
        vertices.Add(b);

        vertices.Add(a);
        vertices.Add(c);
        vertices.Add(d);

        vertices.Add(a);
        vertices.Add(d);
        vertices.Add(c);

        var i = indices.Count;

        for (nuint j = 0; j < 12; j++)
        {
            indices.Add((uint)(i + j));
        }
    }

    internal static UnmanagedValueList<Vector3> MeshNormals(in UnmanagedValueList<Vector3> vertices)
    {
        var n4 = new Vector3[4];

        for (nuint i = 0; i < 12; i += 3)
        {
            var a = vertices[i + 0];
            var b = vertices[i + 1];
            var c = vertices[i + 2];
            n4[i / 3] = Vector3.Normalize(Vector3.CrossProduct(b - a, c - a));
        }

        var normals = UnmanagedValueList.Empty<Vector3>();

        for (nuint i = 0; i < vertices.Count; i += 3)
        {
            // same normal for all three triangle vertices to ensure flat shaded surfaces

            var n = n4[i % 4];
            normals.EnsureCapacity(normals.Count + 3);

            normals.Add(n);
            normals.Add(n);
            normals.Add(n);
        }

        return normals;
    }
}
