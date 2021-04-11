// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#version 450 core

layout(binding = 0, std140) uniform PerFrameInput
{
    layout(column_major) mat4 frameTransform;
};

layout(binding = 1, std140) uniform PerPrimitiveInput
{
    layout(column_major) mat4 primitiveTransform;
};

layout(location = 0) in vec3 input_position;
layout(location = 1) in vec3 input_uvw;

out gl_PerVertex
{
    vec4 gl_Position;
};

layout(location = 0) out vec3 output_uvw;

void main()
{
    vec4 v4 = vec4(input_position, 1.0);
    gl_Position = v4;

    v4 = v4 * primitiveTransform;
    v4 = v4 * frameTransform;

    output_uvw = vec3(v4);

}
