#version 330

in vec4 fragColor;
in vec2 fragUV;

layout(location = 0)
out vec4 outColor;

uniform sampler2D uTexture;

void main() {
	outColor = texture(uTexture, fragUV) * fragColor;
}