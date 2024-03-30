#version 330 core

uniform mat4 projection;
uniform mat4 view;

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec3 aColor;

out vec3 ourColor;

void main()
{
    gl_Position = projection * view * vec4(aPosition, 1.0);
    ourColor = aColor;
}