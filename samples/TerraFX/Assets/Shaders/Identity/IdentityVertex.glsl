// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#version 450 core

layout(location = 0) in vec4 input_color;
layout(location = 1) in vec3 input_position;

layout(location = 0) out vec4 output_color;

out gl_PerVertex
{
    vec4 gl_Position;
};

void main()
{
    output_color = input_color;
    gl_Position = vec4(input_position, 1.0);
}
