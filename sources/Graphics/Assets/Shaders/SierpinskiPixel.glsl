// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#version 450 core

layout(binding = 2) uniform sampler3D input_textureSampler;

layout(location = 0) in vec3 input_normal;
layout(location = 1) in vec3 input_uvw;
layout(location = 0) out vec4 output_color;

void main()
{
    vec4 color = texture(input_textureSampler, input_uvw);
    float brightness = 0.1 + 0.9 * clamp(input_normal[2], 0, 1);
    output_color = color * brightness;
}
