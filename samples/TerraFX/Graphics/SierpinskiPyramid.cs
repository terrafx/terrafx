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
                                                   
            var a = new Vector3(-r1,    -r6,      r3);    
            var b = new Vector3(+r1,    -r6,      r3);    
            var c = new Vector3(0  ,    -r6, -2 * r3);     
            var d = new Vector3(0  , 3 * r6,       0);     
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            vertices.Add(d);

            var indices = new List<ushort[]>();
            // clockwise when looking at the triangle from the outside
            indices.Add(new ushort[] { 2, 0, 1 }); // bottom
            indices.Add(new ushort[] { 3, 0, 2 }); // left
            indices.Add(new ushort[] { 3, 2, 1 }); // right
            indices.Add(new ushort[] { 3, 1, 0 }); // back

            return (vertices, indices);
        }
    }
}
