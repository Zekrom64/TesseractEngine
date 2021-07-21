using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.GL.Native {

	/* 
	 * 
	 */

	// Typedefs from OpenGL headers
	using GLenum = UInt32;
	using GLbitfield = UInt32;
	using GLuint = UInt32;
	using GLint = Int32;
	using GLsizei = Int32;
	using GLboolean = Byte;
	using GLbyte = SByte;
	using GLshort = Int16;
	using GLubyte = Byte;
	using GLushort = UInt16;
	using GLulong = UInt64;
	using GLfloat = Single;
	using GLclampf = Single;
	using GLdouble = Double;
	using GLclampd = Double;
	using GLint64 = Int64;
	using GLuint64 = UInt64;
	using GLintptr = IntPtr;
	using GLsizeiptr = IntPtr;

	using GLfixed = Int32;
	using GLsync = IntPtr;

	using cl_context = IntPtr;
	using cl_event = IntPtr;

	public class ARBES2CompatibilityFunctions {

		public PFN_glClearDepthfARB glClearDepthfARB;
		public PFN_glDepthRangefARB glDepthRangefARB;
		public PFN_glGetShaderPrecisionFormatARB glGetShaderPrecisionFormatARB;
		public PFN_glReleaseShaderCompilerARB glReleaseShaderCompilerARB;
		public PFN_glShaderBinaryARB glShaderBinaryARB;

	}

	public delegate void PFN_glClearDepthfARB(GLclampf d);
	public delegate void PFN_glDepthRangefARB(GLclampf n, GLclampf f);
	public delegate void PFN_glGetShaderPrecisionFormatARB(GLenum shaderType, GLenum precisionType, out GLint range, out GLint precision);
	public delegate void PFN_glReleaseShaderCompilerARB();
	public delegate void PFN_glShaderBinaryARB(GLsizei count, [NativeType("const GLuint*")] IntPtr shaders, GLenum binaryformat, IntPtr binary, GLsizei length);

	public class ARBES31CompatibilityFunctions {

		public PFN_glMemoryBarrierByRegionARB glMemoryBarrierByRegionARB;

	}

	public delegate void PFN_glMemoryBarrierByRegionARB(GLbitfield barriers);

	public class ARBES32CompatibilityFunctions {

		public PFN_glPrimitiveBoundingBoxARB glPrimitiveBoundingBoxARB;

	}

	public delegate void PFN_glPrimitiveBoundingBoxARB(GLfloat minX, GLfloat minY, GLfloat minZ, GLfloat minW, GLfloat maxX, GLfloat maxY, GLfloat maxZ, GLfloat maxW);

	public class ARBBaseInstanceFunctions {

		public PFN_glDrawArraysInstancedBaseInstanceARB glDrawArraysInstancedBaseInstanceARB;
		public PFN_glDrawElementsInstancedBaseInstanceARB glDrawElementsInstancedBaseInstanceARB;
		public PFN_glDrawElementsInstancedBaseVertexBaseInstanceARB glDrawElementsInstancedBaseVertexBaseInstanceARB;

	}

	public delegate void PFN_glDrawArraysInstancedBaseInstanceARB(GLenum mode, GLint first, GLsizei count, GLsizei primcount, GLuint baseinstance);
	public delegate void PFN_glDrawElementsInstancedBaseInstanceARB(GLenum mode, GLsizei count, GLenum type, IntPtr indices, GLsizei primcount, GLuint baseinstance);
	public delegate void PFN_glDrawElementsInstancedBaseVertexBaseInstanceARB(GLenum mode, GLsizei count, GLenum type, IntPtr indices, GLsizei primcount, GLint basevertex, GLuint baseinstance);

	public class ARBBindlessTextureFunctions {

		public PFN_glGetImageHandleARB glGetImageHandleARB;
		public PFN_glGetTextureHandleARB glGetTextureHandleARB;
		public PFN_glGetTextureSamplerHandleARB glGetTextureSamplerHandleARB;
		public PFN_glGetVertexAttribLui64vARB glGetVertexAttribLui64VARB;
		public PFN_glIsImageHandleResidentARB glIsImageHandleResidentARB;
		public PFN_glIsTextureHandleResidentARB glIsTextureHandleResidentARB;
		public PFN_glMakeImageHandleNonResidentARB glMakeImageHandleNonResidentARB;
		public PFN_glMakeImageHandleReisdentARB glMakeImageHandleReisdentARB;
		public PFN_glMakeTextureHandleNonResidentARB glMakeTextureHandleNonResidentARB;
		public PFN_glMakeTextureHandleResidentARB glMakeTextureHandleResidentARB;
		public PFN_glProgramUniformHandleui64ARB glProgramUniformHandleui64ARB;
		public PFN_glProgramUniformHandleui64vARB glProgramUniformHandleui64VARB;
		public PFN_glUniformHandleui64ARB glUniformHandleui64ARB;
		public PFN_glUniformHandleui64vARB glUniformHandleui64VARB;
		public PFN_glVertexAttribL1ui64ARB glVertexAttribL1Ui64ARB;

	}

	public delegate GLuint64 PFN_glGetImageHandleARB(GLuint texture, GLint level, GLboolean layered, GLint layer, GLenum format);
	public delegate GLuint64 PFN_glGetTextureHandleARB(GLuint texture);
	public delegate GLuint64 PFN_glGetTextureSamplerHandleARB(GLuint texture, GLuint sampler);
	public delegate void PFN_glGetVertexAttribLui64vARB(GLuint index, GLenum pname, [NativeType("GLuint64*")] IntPtr _params);
	public delegate GLboolean PFN_glIsImageHandleResidentARB(GLuint64 handle);
	public delegate GLboolean PFN_glIsTextureHandleResidentARB(GLuint64 handle);
	public delegate void PFN_glMakeImageHandleNonResidentARB(GLuint64 handle);
	public delegate void PFN_glMakeImageHandleReisdentARB(GLuint64 handle, GLenum access);
	public delegate void PFN_glMakeTextureHandleNonResidentARB(GLuint64 handle);
	public delegate void PFN_glMakeTextureHandleResidentARB(GLuint64 handle);
	public delegate void PFN_glProgramUniformHandleui64ARB(GLuint program, GLint location, GLuint64 value);
	public delegate void PFN_glProgramUniformHandleui64vARB(GLuint program, GLint location, GLsizei count, [NativeType("const GLuint64*")] IntPtr values);
	public delegate void PFN_glUniformHandleui64ARB(GLint location, GLuint64 value);
	public delegate void PFN_glUniformHandleui64vARB(GLint location, GLsizei count, [NativeType("const GLuint64*")] IntPtr values);
	public delegate void PFN_glVertexAttribL1ui64ARB(GLuint index, GLuint64 x);

	public class ARBBlendFuncExtendedFunctions {

		public PFN_glBindFragDataLocationIndexedARB glBindFragDataLocationIndexedARB;
		public PFN_glGetFragDataIndexARB glGetFragDataIndexARB;

	}

	public delegate void PFN_glBindFragDataLocationIndexedARB(GLuint program, GLuint colorNumber, GLuint index, [MarshalAs(UnmanagedType.LPStr)] string name);
	public delegate GLint PFN_glGetFragDataIndexARB(GLuint program, [MarshalAs(UnmanagedType.LPStr)] string name);

	public class ARBBufferStorageFunctions {

		public PFN_glBufferStorageARB glBufferStorageARB;

	}

	public delegate void PFN_glBufferStorageARB(GLenum target, GLsizeiptr size, IntPtr data, GLbitfield flags);

	public class ARBCLEventFunctions {

		public PFN_glCreateSyncFromCLEventARB glCreateSyncFromCLEventARB;

	}

	public delegate GLsync PFN_glCreateSyncFromCLEventARB(cl_context context, cl_event _event, GLbitfield flags);

	public class ARBClearBufferObjectFunctions {

		public PFN_glClearBufferDataARB glClearBufferDataARB;
		public PFN_glClearBufferSubDataARB glClearBufferSubDataARB;

	}

	public delegate void PFN_glClearBufferDataARB(GLenum target, GLenum internalformat, GLenum format, GLenum type, IntPtr data);
	public delegate void PFN_glClearBufferSubDataARB(GLenum target, GLenum internalformat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, IntPtr data);

	public class ARBClearTextureFunctions {

		public PFN_glClearTexImageARB glClearTexImageARB;
		public PFN_glClearTexSubImageARB glClearTexSubImageARB;

	}

	public delegate void PFN_glClearTexImageARB(GLuint texture, GLint level, GLenum format, GLenum type, IntPtr data);
	public delegate void PFN_glClearTexSubImageARB(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, IntPtr data);

	public class ARBClipControlFunctions {

		public PFN_glClipControlARB glClipControlARB;

	}

	public delegate void PFN_glClipControlARB(GLenum origin, GLenum depth);

	public class ARBColorBufferFloatFunctions {

		public PFN_glClampColorARB glClampColorARB;

	}

	public delegate void PFN_glClampColorARB(GLenum target, GLenum clamp);

	public class ARBComputeShaderFunctions {

		public PFN_glDispatchComputeARB glDispatchComputeARB;
		public PFN_glDispatchComputeIndirectARB glDispatchComputeIndirectARB;

	}

	public delegate void PFN_glDispatchComputeARB(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z);
	public delegate void PFN_glDispatchComputeIndirectARB(GLintptr indirect);

	public class ARBComputeVariableGroupSizeFunctions {

		public PFN_glDispatchComputeGroupSizeARB glDispatchComputeGroupSizeARB;

	}

	public delegate void PFN_glDispatchComputeGroupSizeARB(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z, GLuint group_size_x, GLuint group_size_y, GLuint group_size_z);

	public class ARBCopyBufferFunctions {

		public PFN_glCopyBufferSubDataARB glCopyBufferSubDataARB;

	}

	public delegate void PFN_glCopyBufferSubDataARB(GLenum readTarget, GLenum writeTarget, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size);

	public class ARBCopyImageFunctions {

		public PFN_glCopyImageSubDataARB glCopyImageSubDataARB;

	}

	public delegate void PFN_glCopyImageSubDataARB(GLuint srcName, GLenum srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, GLenum dstTarget, GLint dstLevel, GLint dstX, GLint dstY, GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth);

	public class ARBDebugOutputFunctions {

		public PFN_glDebugMessageCallbackARB glDebugMessageCallbackARB;
		public PFN_glDebugMessageControlARB glDebugMessageControlARB;
		public PFN_glDebugMessageInsertARB glDebugMessageInsertARB;
		public PFN_glGetDebugMessageLogARB glGetDebugMessageLogARB;

	}

	public delegate void PFN_glDebugMessageCallbackARB([MarshalAs(UnmanagedType.FunctionPtr)] GLDebugProc callback, IntPtr userParam);
	public delegate void PFN_glDebugMessageControlARB(GLenum source, GLenum type, GLenum severity, GLsizei count, [NativeType("const GLuint*")] IntPtr ids, GLboolean enabled);
	public delegate void PFN_glDebugMessageInsertARB(GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, [NativeType("const GLchar*")] IntPtr buf);
	public delegate void PFN_glGetDebugMessageLogARB(GLuint count, GLsizei bufSize, [NativeType("GLenum*")] IntPtr sources, [NativeType("GLenum*")] IntPtr types, [NativeType("GLuint*")] IntPtr ids, [NativeType("GLenum*")] IntPtr severities, [NativeType("GLsizei*")] IntPtr lengths, [NativeType("GLchar*")] IntPtr messageLog);

	public class ARBDirectStateAccessFunctions {

	}

	/*
	public class ARBDrawBuffersFunctions {

		public PFN_glDrawBuffersARB glDrawBuffersARB;

	}

	public delegate void PFN_glDrawBuffersARB(GLsizei n, [NativeType("const GLenum*")] IntPtr bufs);
	*/

	public class ARBDrawElementsBaseVertexFunctions {

		public PFN_glDrawElementsBaseVertexARB glDrawElementsBaseVertexARB;
		public PFN_glDrawElementsInstancedBaseVertexARB glDrawElementsInstancedBaseVertexARB;
		public PFN_glDrawRangeElementsBaseVertexARB glDrawRangeElementsBaseVertexARB;
		public PFN_glMultiDrawElementsBaseVertexARB glMultiDrawElementsBaseVertexARB;

	}

	public delegate void PFN_glDrawElementsBaseVertexARB(GLenum mode, GLsizei count, GLenum type, IntPtr indices, GLint basevertex);
	public delegate void PFN_glDrawElementsInstancedBaseVertexARB(GLenum mode, GLsizei count, GLenum type, IntPtr indices, GLsizei primcount, GLint basevertex);
	public delegate void PFN_glDrawRangeElementsBaseVertexARB(GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, IntPtr indices, GLint basevertex);
	public delegate void PFN_glMultiDrawElementsBaseVertexARB(GLenum mode, [NativeType("GLsizei*")] IntPtr count, GLenum type, [NativeType("void**")] IntPtr indices, GLsizei primcount, [NativeType("GLint*")] IntPtr basevertex);

	public class ARBDrawIndirectFunctions {

		public PFN_glDrawArraysIndirectARB glDrawArraysIndirectARB;
		public PFN_glDrawElementsIndirectARB glDrawElementsIndirectARB;

	}

	public delegate void PFN_glDrawArraysIndirectARB(GLenum mode, IntPtr indirect);
	public delegate void PFN_glDrawElementsIndirectARB(GLenum mode, GLenum type, IntPtr indirect);

	public class ARBDrawInstancedFunctions {

		public PFN_glDrawArraysInstancedARB glDrawArraysInstancedARB;
		public PFN_glDrawElementsInstancedARB glDrawElementsInstancedARB;

	}

	public delegate void PFN_glDrawArraysInstancedARB(GLenum mode, GLint first, GLsizei count, GLsizei primcount);
	public delegate void PFN_glDrawElementsInstancedARB(GLenum mode, GLsizei count, GLenum type, IntPtr indices, GLsizei primcount);

}
