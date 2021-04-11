// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#version 450 core

layout(binding = 2) uniform sampler1D input_texture1DSampler;
layout(binding = 3) uniform sampler2D input_texture2DSampler;
layout(binding = 4) uniform sampler3D input_texture3DSampler;

layout(location = 0) in vec3 input_uvw;
layout(location = 0) out vec4 output_color;

void main()
{
    vec4 texel1D = texture(input_texture1DSampler, input.uvw[0]);
    vec4 texel2D = texture(input_texture2DSampler, vec2(input.uvw[0], input.uvw[1]));
    vec4 texel3D = texture(input_texture3DSampler, input.uvw);

    vec4 texel = texel3D * (1-texel2D[3]) + texel2D * texel2D[3];
    float d = abs(input.uvw[0] - input.uvw[1]);

    if (d < 0.1) {
        texel = texel * (10 * d) + texel1D * (1 - 10 * d);
    }

    output_color = texel;
}
