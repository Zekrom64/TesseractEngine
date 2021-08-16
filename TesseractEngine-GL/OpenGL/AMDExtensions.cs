using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {
	
	public enum GLDebugCategoryAMD : uint {
		ApiError = GLEnums.GL_DEBUG_CATEGORY_API_ERROR_AMD,
		WindowSystem = GLEnums.GL_DEBUG_CATEGORY_WINDOW_SYSTEM_AMD,
		Deprecation = GLEnums.GL_DEBUG_CATEGORY_DEPRECATION_AMD,
		UndefinedBehavior = GLEnums.GL_DEBUG_CATEGORY_UNDEFINED_BEHAVIOR_AMD,
		Performance = GLEnums.GL_DEBUG_CATEGORY_PERFORMANCE_AMD,
		ShaderCompiler = GLEnums.GL_DEBUG_CATEGORY_SHADER_COMPILER_AMD,
		Application = GLEnums.GL_DEBUG_CATEGORY_APPLICATION_AMD,
		Other = GLEnums.GL_DEBUG_CATEGORY_OTHER_AMD
	}

	public enum GLDebugSeverityAMD : uint {
		High = GLEnums.GL_DEBUG_SEVERITY_HIGH_AMD,
		Medium = GLEnums.GL_DEBUG_SEVERITY_MEDIUM_AMD,
		Low = GLEnums.GL_DEBUG_SEVERITY_LOW_AMD
	}

	public delegate void GLDebugProcAMD(uint id, GLDebugCategoryAMD category, GLDebugSeverityAMD severity, int length, [MarshalAs(UnmanagedType.LPStr)] string message, IntPtr userParam);

}
