// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using TerraFX.Numerics;

namespace TerraFX.Samples.Graphics
{
    internal class SierpinskiPyramid
    {
        internal static (List<Vector3> vertices, List<ushort[]> indices) CreateMesh(int recursionDepth)
        {
            var vertices = new List<Vector3>();

            var a = new Vector3(-0.5f, 0.5f, 0.0f);
            var b = new Vector3(0.5f, 0.5f, 0.0f);
            var c = new Vector3(0.5f, -0.5f, 0.0f);
            var d = new Vector3(-0.5f, -0.5f, 0.0f);
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);

            var indices = new List<ushort[]>();
            indices.Add(new ushort[] { 0, 1, 2 });
            indices.Add(new ushort[] { 0, 2, 3 });

            return (vertices, indices);
        }
    }
}
