#version 450

layout(location = 0)
in vec4 fragColor;
layout(location = 1)
in vec2 fragUV;

layout(location = 0)
out vec4 outColor;

layout(binding = 1)
uniform sampler2D uTexture;

void main() {
	outColor = texture(uTexture, fragUV) * fragColor;
}