// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.ApplicationModel;

namespace TerraFX.Samples.Graphics
{
    public class HelloSierpinskiQuad : HelloSierpinski
    {
        public HelloSierpinskiQuad(string name, int recursionDepth, ApplicationServiceProvider serviceProvider)
            : base(name, recursionDepth, SierpinskiShape.Quad, serviceProvider)
        {
        }
    }
}
