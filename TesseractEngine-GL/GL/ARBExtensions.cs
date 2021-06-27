using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.GL.Native;

namespace Tesseract.GL {
	
	public enum GLDebugSourceARB : uint {
		API = GLEnums.GL_DEBUG_SOURCE_API_ARB,
		WindowSystem = GLEnums.GL_DEBUG_SOURCE_WINDOW_SYSTEM_ARB,
		ShaderCompiler = GLEnums.GL_DEBUG_SOURCE_SHADER_COMPILER_ARB,
		ThirdParty = GLEnums.GL_DEBUG_SOURCE_THIRD_PARTY_ARB,
		Application = GLEnums.GL_DEBUG_SOURCE_APPLICATION_ARB,
		Other = GLEnums.GL_DEBUG_SOURCE_OTHER
	}

	public enum GLDebugTypeARB : uint {
		Error = GLEnums.GL_DEBUG_TYPE_ERROR_ARB,
		DeprecatedBehavior = GLEnums.GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR_ARB,
		UndefinedBehavior = GLEnums.GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR_ARB,
		Portability = GLEnums.GL_DEBUG_TYPE_PORTABILITY_ARB,
		Performance = GLEnums.GL_DEBUG_TYPE_PERFORMANCE_ARB,
		Other = GLEnums.GL_DEBUG_TYPE_OTHER_ARB
	}

	public enum GLDebugSeverityARB : uint {
		High = GLEnums.GL_DEBUG_SEVERITY_HIGH_ARB,
		Medium = GLEnums.GL_DEBUG_SEVERITY_MEDIUM_ARB,
		Low = GLEnums.GL_DEBUG_SEVERITY_LOW_ARB
	}

	public delegate void GLDebugProcARB(GLDebugSourceARB source, GLDebugTypeARB type, uint id, GLDebugSeverityARB severity, int length, [MarshalAs(UnmanagedType.LPStr)] string message, IntPtr userParam);

}
