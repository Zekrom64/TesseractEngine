#version 450
layout(location = 0)
in vec2 inPosition;
layout(location = 1)
in vec4 inColor;
layout(location = 2)
in vec2 inUV;

layout(location = 0)
out vec4 fragColor;
layout(location = 1)
out vec2 fragUV;

layout(binding = 0)
uniform UGlobals {
	mat4 mProj;
} uGlobals;

void main() {
	fragUV = inUV;
	fragColor = inColor;
	gl_Position = vec4(inPosition, 0, 1) * uGlobals.mProj;
}