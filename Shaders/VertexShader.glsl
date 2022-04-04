#version 330 core

layout (location = 0) in vec3 _VPOS;
uniform mat4 _MVP;

void main()
{
    gl_Position = _MVP * vec4(_VPOS, 1.0);
}