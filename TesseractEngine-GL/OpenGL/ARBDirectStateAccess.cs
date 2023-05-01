using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

	public unsafe class ARBDirectStateAccessFunctions {

		[ExternFunction(AltNames = new string[] { "glCreateTransformFeedbacksARB" })]
		[NativeType("void glCreateTransformFeedbacks(GLsizei n, GLuint* pIDs)")]
		public delegate* unmanaged<int, uint*, void> glCreateTransformFeedbacks;
		[ExternFunction(AltNames = new string[] { "glTransformFeedbackBufferBaseARB" })]
		[NativeType("void glTransformFeedbackBufferBase(GLuint feedback, GLuint index, GLuint buffer)")]
		public delegate* unmanaged<uint, uint, uint, void> glTransformFeedbackBufferBase;
		[ExternFunction(AltNames = new string[] { "glTransformFeedbackBufferRangeARB" })]
		[NativeType("void glTransformFeedbackBufferRange(GLuint feedback, GLuint index, GLuint buffer, GLintptr offset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, uint, nint, nint, void> glTransformFeedbackBufferRange;
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbackivARB" })]
		[NativeType("void glGetTransformFeedbackiv(GLuint feedback, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetTransformFeedbackiv;
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbacki_vARB" })]
		[NativeType("void glGetTransformFeedbacki_v(GLuint feedback, GLenum pname, GLuint index, GLint* pParam)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetTransformFeedbacki_v;
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbacki64_vARB" })]
		[NativeType("void glGetTransformFeedbacki64_v(GLuint feedback, GLenum pname, GLuint index, GLint64* pParam)")]
		public delegate* unmanaged<uint, uint, uint, long*, void> glGetTransformFeedbacki64_v;

		[ExternFunction(AltNames = new string[] { "glCreateBuffersARB" })]
		[NativeType("void glCreateBuffers(GLsizei n, GLuint* pBuffers)")]
		public delegate* unmanaged<int, uint*, void> glCreateBuffers;
		[ExternFunction(AltNames = new string[] { "glNamedBufferStorageARB" })]
		[NativeType("void glNamedBufferStorage(GLuint buffer, GLsizeiptr size, void* pData, GLbitfield flags)")]
		public delegate* unmanaged<uint, nint, IntPtr, uint, void> glNamedBufferStorage;
		[ExternFunction(AltNames = new string[] { "glNamedBufferDataARB" })]
		[NativeType("void glNamedBufferData(GLuint buffer, GLsizeiptr size, void* pData, GLenum usage)")]
		public delegate* unmanaged<uint, nint, IntPtr, uint, void> glNamedBufferData;
		[ExternFunction(AltNames = new string[] { "glNamedBufferSubDataARB" })]
		[NativeType("void glNamedBufferSubData(GLuint buffer, GLintptr offset, GLsizeiptr size, void* pData)")]
		public delegate* unmanaged<uint, nint, nint, IntPtr, void> glNamedBufferSubData;
		[ExternFunction(AltNames = new string[] { "glCopyNamedBufferSubDataARB" })]
		[NativeType("void glCopyNamedBufferSubData(GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, nint, nint, nint, void> glCopyNamedBufferSubData;
		[ExternFunction(AltNames = new string[] { "glClearNamedBufferDataARB" })]
		[NativeType("void glClearNamedBufferData(GLuint buffer, GLenum internalFormat, GLenum format, GLenum type, void* pData)")]
		public delegate* unmanaged<uint, uint, uint, uint, IntPtr, void> glClearNamedBufferData;
		[ExternFunction(AltNames = new string[] { "glClearNamedBufferSubDataARB" })]
		[NativeType("void glClearNamedBufferSubData(GLuint buffer, GLenum internalFormat, GLintptr offset, GLsizeiptr size, GLenum format, GLenum type, void* pData)")]
		public delegate* unmanaged<uint, uint, nint, nint, uint, uint, IntPtr, void> glClearNamedBufferSubData;
		[ExternFunction(AltNames = new string[] { "glMapNamedBufferARB" })]
		[NativeType("void* glMapNamedBuffer(GLuint buffer, GLenum access)")]
		public delegate* unmanaged<uint, uint, IntPtr> glMapNamedBuffer;
		[ExternFunction(AltNames = new string[] { "glMapNamedBufferRangeARB" })]
		[NativeType("void* glMapNamedBufferRange(GLuint buffer, GLintptr offset, GLsizeiptr length, GLbitfield access)")]
		public delegate* unmanaged<uint, nint, nint, uint, IntPtr> glMapNamedBufferRange;
		[ExternFunction(AltNames = new string[] { "glUnmapNamedBufferARB" })]
		[NativeType("GLboolean glUnmapNamedBuffer(GLuint buffer)")]
		public delegate* unmanaged<uint, byte> glUnmapNamedBuffer;
		[ExternFunction(AltNames = new string[] { "glFlushMappedNamedBufferRangeARB" })]
		[NativeType("void glFlushMappedNamedBufferRange(GLuint buffer, GLintptr offset, GLsizeiptr length)")]
		public delegate* unmanaged<uint, nint, nint, void> glFlushMappedNamedBufferRange;
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferParameterivARB" })]
		[NativeType("void glGetNamedBufferParameteriv(GLuint buffer, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetNamedBufferParameteriv;
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferParameteri64vARB" })]
		[NativeType("void glGetNamedBufferParameteri64v(GLuint buffer, GLenum pname, GLint64* pParams)")]
		public delegate* unmanaged<uint, uint, long*, void> glGetNamedBufferParameteri64v;
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferPointervARB" })]
		[NativeType("void glGetNamedBufferPointerv(GLuint buffer, GLenum pname, void** pParams)")]
		public delegate* unmanaged<uint, uint, IntPtr*, void> glGetNamedBufferPointerv;
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferSubDataARB" })]
		[NativeType("void glGetNamedBufferSubData(GLuint buffer, GLintptr offset, GLsizeiptr size, void* pData)")]
		public delegate* unmanaged<uint, nint, nint, IntPtr, void> glGetNamedBufferSubData;

		[ExternFunction(AltNames = new string[] { "glCreateFramebuffersARB" })]
		[NativeType("void glCreateFramebuffers(GLsizei n, GLuint* pFramebuffers)")]
		public delegate* unmanaged<int, uint*, void> glCreateFramebuffers;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferRenderbufferARB" })]
		[NativeType("void glNamedFramebufferRenderbuffer(GLuint framebuffer, GLenum attachment, GLenum renderbufferTarget, GLuint renderbuffer)")]
		public delegate* unmanaged<uint, uint, uint, uint, void> glNamedFramebufferRenderbuffer;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferParameteriARB" })]
		[NativeType("void glNamedFramebufferParameteri(GLuint framebuffer, GLenum pname, GLint param)")]
		public delegate* unmanaged<uint, uint, int, void> glNamedFramebufferParameteri;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferTextureARB" })]
		[NativeType("void glNamedFramebufferTexture(GLuint framebuffer, GLenum attachment, GLuint texture, GLint level)")]
		public delegate* unmanaged<uint, uint, uint, int, void> glNamedFramebufferTexture;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferTextureLayerARB" })]
		[NativeType("void glNamedFramebufferTextureLayer(GLuint framebuffer, GLenum attachment, GLuint texture, GLint level, GLint layer)")]
		public delegate* unmanaged<uint, uint, uint, int, int, void> glNamedFramebufferTextureLayer;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferDrawBufferARB" })]
		[NativeType("void glNamedFramebufferDrawBuffer(GLuint framebuffer, GLenum mode)")]
		public delegate* unmanaged<uint, uint, void> glNamedFramebufferDrawBuffer;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferDrawBuffersARB" })]
		[NativeType("void glNamedFramebufferDrawBuffers(GLuint framebuffer, GLsizei n, const GLenum* pBufs)")]
		public delegate* unmanaged<uint, int, uint*, void> glNamedFramebufferDrawBuffers;
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferReadBufferARB" })]
		[NativeType("void glNamedFramebufferReadBuffer(GLuint framebuffer, GLenum mode)")]
		public delegate* unmanaged<uint, uint, void> glNamedFramebufferReadBuffer;
		[ExternFunction(AltNames = new string[] { "glInvalidateNamedFramebufferDataARB" })]
		[NativeType("void glInvalidateNamedFramebufferData(GLuint framebuffer, GLsizei numAttachments, const GLenum* pAttachments)")]
		public delegate* unmanaged<uint, int, uint*, void> glInvalidateNamedFramebufferData;
		[ExternFunction(AltNames = new string[] { "glInvalidateNamedFramebufferSubDataARB" })]
		[NativeType("void glInvalidateNamedFramebufferSubData(GLuint framebuffer, GLsizei numAttachments, const GLenum* pAttachments, GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, uint*, int, int, int, int, void> glInvalidateNamedFramebufferSubData;
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferivARB" })]
		[NativeType("void glClearNamedFramebufferiv(GLuint framebuffer, GLenum buffer, GLint drawBuffer, const GLint* pValue)")]
		public delegate* unmanaged<uint, uint, int, int*, void> glClearNamedFramebufferiv;
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferuivARB" })]
		[NativeType("void glClearNamedFramebufferuiv(GLuint framebuffer, GLenum buffer, GLint drawBuffer, const GLuint* pValue)")]
		public delegate* unmanaged<uint, uint, int, uint*, void> glClearNamedFramebufferuiv;
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferfvARB" })]
		[NativeType("void glClearNamedFramebufferfv(GLuint framebuffer, GLenum buffer, GLint drawBuffer, const GLfloat* pValue)")]
		public delegate* unmanaged<uint, uint, int, float*, void> glClearNamedFramebufferfv;
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferfiARB" })]
		[NativeType("void glClearNamedFramebufferfi(GLuint framebuffer, GLenum buffer, GLint drawBuffer, GLfloat depth, GLint stencil)")]
		public delegate* unmanaged<uint, uint, int, float, int, void> glClearNamedFramebufferfi;
		[ExternFunction(AltNames = new string[] { "glBlitNamedFramebufferARB" })]
		[NativeType("void glBlitNamedFramebuffer(GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GLbitfield mask, GLenum filter)")]
		public delegate* unmanaged<uint, uint, int, int, int, int, int, int, int, int, uint, uint, void> glBlitNamedFramebuffer;
		[ExternFunction(AltNames = new string[] { "glCheckNamedFramebufferStatusARB" })]
		[NativeType("GLenum glCheckNamedFramebufferStatus(GLuint framebuffer, GLenum target)")]
		public delegate* unmanaged<uint, uint, uint> glCheckNamedFramebufferStatus;
		[ExternFunction(AltNames = new string[] { "glGetNamedFramebufferParameterivARB" })]
		[NativeType("void glGetNamedFramebufferParameteriv(GLuint framebuffer, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetNamedFramebufferParameteriv;
		[ExternFunction(AltNames = new string[] { "glGetnamedFramebufferAttachmentParameterivARB" })]
		[NativeType("void glGetNamedFramebufferAttachmentParameteriv(GLuint framebuffer, GLenum attachment, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetNamedFramebufferAttachmentParameteriv;

		[ExternFunction(AltNames = new string[] { "glCreateRenderbuffersARB" })]
		[NativeType("void glCreateRenderbuffers(GLsizei n, GLuint* pRenderbuffers)")]
		public delegate* unmanaged<int, uint*, void> glCreateRenderbuffers;
		[ExternFunction(AltNames = new string[] { "glNamedRenderbufferStorageARB" })]
		[NativeType("void glNamedRenderbufferStorage(GLuint renderbuffer, GLenum internalFormat, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, uint, int, int, void> glNamedRenderbufferStorage;
		[ExternFunction(AltNames = new string[] { "glNamedRenderbufferStorageMultisampleARB" })]
		[NativeType("void glNamedRenderbufferStorageMultisample(GLuint renderbuffer, GLint samples, GLenum internalFormat, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, uint, int, int, void> glNamedRenderbufferStorageMultisample;
		[ExternFunction(AltNames = new string[] { "glGetNamedRenderbufferParameterivARB" })]
		[NativeType("void glGetNamedRenderbufferParameteriv(GLuint renderbuffer, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetNamedRenderbufferParameteriv;
		
		[ExternFunction(AltNames = new string[] { "glCreateTexturesARB" })]
		[NativeType("void glCreateTextures(GLenum target, GLsizei n, GLuint* pTextures)")]
		public delegate* unmanaged<uint, int, uint*, void> glCreateTextures;
		[ExternFunction(AltNames = new string[] { "glTextureBufferARB" })]
		[NativeType("void glTextureBuffer(GLuint texture, GLenum internalFormat, GLuint buffer)")]
		public delegate* unmanaged<uint, uint, uint, void> glTextureBuffer;
		[ExternFunction(AltNames = new string[] { "glTextureBufferRangeARB" })]
		[NativeType("void glTextureBufferRange(GLuint texture, GLenum internalFormat, GLuint buffer, GLintptr offset, GLsizeiptr size)")]
		public delegate* unmanaged<uint, uint, uint, nint, nint, void> glTextureBufferRange;
		[ExternFunction(AltNames = new string[] { "glTextureStorage1DARB" })]
		[NativeType("void glTextureStorage1D(GLuint texture, GLint level, GLenum internalFormat, GLsizei width)")]
		public delegate* unmanaged<uint, int, uint, int, void> glTextureStorage1D;
		[ExternFunction(AltNames = new string[] { "glTextureStorage2DARB" })]
		[NativeType("void glTextureStorage2D(GLuint texture, GLint level, GLenum internalFormat, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, uint, int, int, void> glTextureStorage2D;
		[ExternFunction(AltNames = new string[] { "glTextureStorage3DARB" })]
		[NativeType("void glTextureStorage3D(GLuint texture, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth)")]
		public delegate* unmanaged<uint, int, uint, int, int, int, void> glTextureStorage3D;
		[ExternFunction(AltNames = new string[] { "glTextureStorage2DMultisampleARB" })]
		[NativeType("void glTextureStorage2DMultisample(GLuint texture, GLint samples, GLenum internalFormat, GLsizei width, GLsizei height, GLboolean fixedSampleLocations)")]
		public delegate* unmanaged<uint, int, uint, int, int, byte, void> glTextureStorage2DMultisample;
		[ExternFunction(AltNames = new string[] { "glTextureStorage3DMultisampleARB" })]
		[NativeType("void glTextureStorage3DMultisample(GLuint texture, GLint samples, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLboolean fixedSampleLocations)")]
		public delegate* unmanaged<uint, int, uint, int, int, int, byte, void> glTextureStorage3DMultisample;
		[ExternFunction(AltNames = new string[] { "glTextureSubImage1DARB" })]
		[NativeType("void glTextureSubImage1D(GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, void* pPixels)")]
		public delegate* unmanaged<uint, int, int, int, uint, uint, IntPtr, void> glTextureSubImage1D;
		[ExternFunction(AltNames = new string[] { "glTextureSubImage2DARB" })]
		[NativeType("void glTextureSubImage2D(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pPixels)")]
		public delegate* unmanaged<uint, int, int, int, int, int, uint, uint, IntPtr, void> glTextureSubImage2D;
		[ExternFunction(AltNames = new string[] { "glTextureSubImage3DARB" })]
		[NativeType("void glTextureSubImage3D(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, void* pPixels)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, uint, uint, IntPtr, void> glTextureSubImage3D;
		[ExternFunction(AltNames = new string[] { "glCompressedTextureSubImage1DARB" })]
		[NativeType("void glCompressedTextureSubImage1D(GLuint texture, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<uint, int, int, int, uint, int, IntPtr, void> glCompressedTextureSubImage1D;
		[ExternFunction(AltNames = new string[] { "glCompressedTextureSubImage2DARB" })]
		[NativeType("void glCompressedTextureSubImage2D(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<uint, int, int, int, int, int, uint, int, IntPtr, void> glCompressedTextureSubImage2D;
		[ExternFunction(AltNames = new string[] { "glCompressedTextureSubImage3DARB" })]
		[NativeType("void glCompressedTextureSubImage2D(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize, void* pData)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, uint, int, IntPtr, void> glCompressedTextureSubImage3D;
		[ExternFunction(AltNames = new string[] { "glCopyTextureSubImage1DARB" })]
		[NativeType("void glCopyTextureSubImage1D(GLuint texture, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width)")]
		public delegate* unmanaged<uint, int, int, int, int, int, void> glCopyTextureSubImage1D;
		[ExternFunction(AltNames = new string[] { "glCopyTextureSubImage2DARB" })]
		[NativeType("void glCopyTextureSubImage2D(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, void> glCopyTextureSubImage2D;
		[ExternFunction(AltNames = new string[] { "glCopyTextureSubImage3DARB" })]
		[NativeType("void glCopyTextureSubImage3D(GLuint texture, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height)")]
		public delegate* unmanaged<uint, int, int, int, int, int, int, int, int, void> glCopyTextureSubImage3D;
		[ExternFunction(AltNames = new string[] { "glTextureParameterfARB" })]
		[NativeType("void glTextureParameterf(GLuint texture, GLenum pname, GLfloat param)")]
		public delegate* unmanaged<uint, uint, float, void> glTextureParameterf;
		[ExternFunction(AltNames = new string[] { "glTextureParameterfvARB" })]
		[NativeType("void glTextureParameterfv(GLuint texture, GLenum pname, const GLfloat* pParams)")]
		public delegate* unmanaged<uint, uint, float*, void> glTextureParameterfv;
		[ExternFunction(AltNames = new string[] { "glTextureParameteriARB" })]
		[NativeType("void glTextureParameteri(GLuint texture, GLenum pname, GLint param)")]
		public delegate* unmanaged<uint, uint, int, void> glTextureParameteri;
		[ExternFunction(AltNames = new string[] { "glTextureParameterivARB" })]
		[NativeType("void glTextureParameteriv(GLuint texture, GLenum pname, const GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glTextureParameteriv;
		[ExternFunction(AltNames = new string[] { "glTextureParameterIivARB" })]
		[NativeType("void glTextureParameterIiv(GLuint texture, GLenum pname, const GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glTextureParameterIiv;
		[ExternFunction(AltNames = new string[] { "glTextureParameterIuivARB" })]
		[NativeType("void glTextureParameterIuiv(GLuint texture, GLenum pname, const GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glTextureParameterIuiv;
		[ExternFunction(AltNames = new string[] { "glGenerateTextureMipmapARB" })]
		[NativeType("void glGenerateTextureMipmap(GLuint texture)")]
		public delegate* unmanaged<uint, void> glGenerateTextureMipmap;
		[ExternFunction(AltNames = new string[] { "glBindTextureUnitARB" })]
		[NativeType("void glBindTextureUnit(GLuint unit, GLuint texture)")]
		public delegate* unmanaged<uint, uint, void> glBindTextureUnit;
		[ExternFunction(AltNames = new string[] { "glGetTextureImageARB" })]
		[NativeType("void glGetTextureImage(GLuint texture, GLint level, GLenum format, GLenum type, GLsizei bufSize, void* pPixels)")]
		public delegate* unmanaged<uint, int, uint, uint, int, IntPtr, void> glGetTextureImage;
		[ExternFunction(AltNames = new string[] { "glGetCompressedTextureImageARB" })]
		[NativeType("void glGetCompressedTextureImage(GLuint texture, GLint level, GLsizei bufSize, void* pPixels)")]
		public delegate* unmanaged<uint, int, int, IntPtr, void> glGetCompressedTextureImage;
		[ExternFunction(AltNames = new string[] { "glGetTextureLevelParameterfvARB" })]
		[NativeType("void glGetTextureLevelParameterfv(GLuint texture, GLint level, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<uint, int, uint, float*, void> glGetTextureLevelParameterfv;
		[ExternFunction(AltNames = new string[] { "glGetTextureLevelParameterivARB" })]
		[NativeType("void glGetTextureLevelParameteriv(GLuint texture, GLint level, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, int, uint, int*, void> glGetTextureLevelParameteriv;
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterfvARB" })]
		[NativeType("void glGetTextureParameterfv(GLuint texture, GLenum pname, GLfloat* pParams)")]
		public delegate* unmanaged<uint, uint, float*, void> glGetTextureParameterfv;
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterivARB" })]
		[NativeType("void glGetTextureParameteriv(GLuint texture, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetTextureParameteriv;
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterIivARB" })]
		[NativeType("void glGetTextureParameterIiv(GLuint texture, GLenum pname, GLint* pParams)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetTextureParameterIiv;
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterIuivARB" })]
		[NativeType("void glGetTextureParameterIuiv(GLuint texture, GLenum pname, GLuint* pParams)")]
		public delegate* unmanaged<uint, uint, uint*, void> glGetTextureParameterIuiv;

		[ExternFunction(AltNames = new string[] { "glCreateVertexArraysARB" })]
		[NativeType("void glCreateVertexArrays(GLsizei n, GLuint* pArrays)")]
		public delegate* unmanaged<int, uint*, void> glCreateVertexArrays;
		[ExternFunction(AltNames = new string[] { "glDisableVertexArrayAttribARB" })]
		[NativeType("void glDisableVertexArrayAttrib(GLuint vertexArray, GLuint index)")]
		public delegate* unmanaged<uint, uint, void> glDisableVertexArrayAttrib;
		[ExternFunction(AltNames = new string[] { "glEnableVertexArrayAttribARB" })]
		[NativeType("void glEnableVertexArrayAttrib(GLuint vertexArray, GLuint index)")]
		public delegate* unmanaged<uint, uint, void> glEnableVertexArrayAttrib;
		[ExternFunction(AltNames = new string[] { "glVertexArrayElementBufferARB" })]
		[NativeType("void glVertexArrayElementBuffer(GLuint vertexArray, GLuint buffer)")]
		public delegate* unmanaged<uint, uint, void> glVertexArrayElementBuffer;
		[ExternFunction(AltNames = new string[] { "glVertexArrayVertexBufferARB" })]
		[NativeType("void glVertexArrayVertexBuffer(GLuint vertexArray, GLuint bindingIndex, GLuint buffer, GLintptr offset, GLsizei stride)")]
		public delegate* unmanaged<uint, uint, uint, nint, int, void> glVertexArrayVertexBuffer;
		[ExternFunction(AltNames = new string[] { "glVertexArrayVertexBuffersARB" })]
		[NativeType("void glVertexArrayVertexBuffers(GLuint vertexArray, GLuint first, GLsizei count, const GLuint* pBuffers, const GLintptr* pOffsets, const GLsizei* pStrides)")]
		public delegate* unmanaged<uint, uint, int, uint*, nint*, int*, void> glVertexArrayVertexBuffers;
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribFormatARB" })]
		[NativeType("void glVertexArrayAttribFormat(GLuint vertexArray, GLuint attribIndex, GLsizei size, GLenum type, GLboolean normalized, GLuint relativeOffset)")]
		public delegate* unmanaged<uint, uint, int, uint, byte, uint, void> glVertexArrayAttribFormat;
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribIFormatARB" })]
		[NativeType("void glVertexArrayAttribIFormat(GLuint vertexArray, GLuint attribIndex, GLsizei size, GLenum type, GLuint relativeOffset)")]
		public delegate* unmanaged<uint, uint, int, uint, uint, void> glVertexArrayAttribIFormat;
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribLFormatARB" })]
		[NativeType("void glVertexArrayAttribLFormat(GLuint vertexArray, GLuint attribIndex, GLsizei size, GLenum type, GLuint relativeOffset)")]
		public delegate* unmanaged<uint, uint, int, uint, uint, void> glVertexArrayAttribLFormat;
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribBindingARB" })]
		[NativeType("void glVertexArrayAttribBinding(GLuint vertexArray, GLuint attribIndex, GLuint bindingIndex)")]
		public delegate* unmanaged<uint, uint, uint, void> glVertexArrayAttribBinding;
		[ExternFunction(AltNames = new string[] { "glVertexArrayBindingDivisorARB" })]
		[NativeType("void glVertexArrayBindingDivisor(GLuint vertexArray, GLuint bindingIndex, GLuint divisor)")]
		public delegate* unmanaged<uint, uint, uint, void> glVertexArrayBindingDivisor;
		[ExternFunction(AltNames = new string[] { "glGetVertexArrayivARB" })]
		[NativeType("void glGetVertexArrayiv(GLuint vertexArray, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<uint, uint, int*, void> glGetVertexArrayiv;
		[ExternFunction(AltNames = new string[] { "glGetVertexArrayIndexedivARB" })]
		[NativeType("void glGetVertexArrayIndexediv(GLuint vertexArray, GLuint index, GLenum pname, GLint* pParam)")]
		public delegate* unmanaged<uint, uint, uint, int*, void> glGetVertexArrayIndexediv;
		[ExternFunction(AltNames = new string[] { "glGetVertexArrayIndexed64ivARB" })]
		[NativeType("void glGetVertexArrayIndexed64iv(GLuint vertexArray, GLuint index, GLenum pname, GLint64* pParam)")]
		public delegate* unmanaged<uint, uint, uint, long*, void> glGetVertexArrayIndexed64iv;

		[ExternFunction(AltNames = new string[] { "glCreateSamplersARB" })]
		[NativeType("void glCreateSamplers(GLsizei n, GLuint* pSamplers)")]
		public delegate* unmanaged<int, uint*, void> glCreateSamplers;

		[ExternFunction(AltNames = new string[] { "glCreateProgramPipelinesARB" })]
		[NativeType("void glCraeteProgramPipelines(GLsizei n, GLuint* pPipelines)")]
		public delegate* unmanaged<int, uint*, void> glCreateProgramPipelines;

		[ExternFunction(AltNames = new string[] { "glCreateQueriesARB" })]
		[NativeType("void glCreateQueries(GLenum target, GLsizei n, GLuint* pIDs)")]
		public delegate* unmanaged<uint, int, uint*, void> glCreateQueries;
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjectivARB" })]
		[NativeType("void glGetQueryBufferObjectiv(GLuint id, GLuint buffer, GLenum pname, GLintptr offset)")]
		public delegate* unmanaged<uint, uint, uint, nint, void> glGetQueryBufferObjectiv;
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjectuivARB" })]
		[NativeType("void glGetQueryBufferObjectuiv(GLuint id, GLuint buffer, GLenum pname, GLintptr offset)")]
		public delegate* unmanaged<uint, uint, uint, nint, void> glGetQueryBufferObjectuiv;
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjecti64vARB" })]
		[NativeType("void glGetQueryBufferObjecti64v(GLuint id, GLuint buffer, GLenum pname, GLintptr offset)")]
		public delegate* unmanaged<uint, uint, uint, nint, void> glGetQueryBufferObjecti64v;
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjectui64vARB" })]
		[NativeType("void glGetQueryBufferObjectui64v(GLuint id, GLuint buffer, GLenum pname, GLintptr offset)")]
		public delegate* unmanaged<uint, uint, uint, nint, void> glGetQueryBufferObjectui64v;

	}

	public class ARBDirectStateAccess : IGLObject {

		public GL GL { get; }
		public ARBDirectStateAccessFunctions Functions { get; } = new();

		public ARBDirectStateAccess(GL gl, IGLContext context) {
			GL = gl;
			Library.LoadFunctions(context.GetGLProcAddress, Functions);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateTransformFeedbacks(Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTransformFeedbacks(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateTransformFeedbacks(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTransformFeedbacks(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateTransformFeedbacks() {
			uint id = 0;
			unsafe {
				Functions.glCreateTransformFeedbacks(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer) {
			unsafe {
				Functions.glTransformFeedbackBufferBase(xfb, index, buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, nint offset, nint size) {
			unsafe {
				Functions.glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTransformFeedbackiv(xfb, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, uint index, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTransformFeedbacki_v(xfb, (uint)pname, index, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, uint index, Span<long> values) {
			unsafe {
				fixed (long* pValues = values) {
					Functions.glGetTransformFeedbacki64_v(xfb, (uint)pname, index, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateBuffers(Span<uint> buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glCreateBuffers(buffers.Length, pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateBuffers(int n) {
			uint[] buffers = new uint[n];
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glCreateBuffers(buffers.Length, pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateBuffers() {
			uint id = 0;
			unsafe {
				Functions.glCreateBuffers(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage(uint buffer, nint size, IntPtr data, GLBufferStorageFlags flags) {
			unsafe {
				Functions.glNamedBufferStorage(buffer, size, data, (uint)flags);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage(uint buffer, nint size, GLBufferStorageFlags flags) {
			unsafe {
				Functions.glNamedBufferStorage(buffer, size, IntPtr.Zero, (uint)flags);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage<T>(uint buffer, in ReadOnlySpan<T> data, GLBufferStorageFlags flags) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glNamedBufferStorage(buffer, sizeof(T) * data.Length, (IntPtr)pData, (uint)flags);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage<T>(uint buffer, GLBufferStorageFlags flags, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glNamedBufferStorage(buffer, sizeof(T) * data.Length, (IntPtr)pData, (uint)flags);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData(uint buffer, nint size, IntPtr data, GLBufferUsage usage) {
			unsafe {
				Functions.glNamedBufferData(buffer, size, data, (uint)usage);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData(uint buffer, nint size, GLBufferUsage usage) {
			unsafe {
				Functions.glNamedBufferData(buffer, size, IntPtr.Zero, (uint)usage);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData<T>(uint buffer, in ReadOnlySpan<T> data, GLBufferUsage usage) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glNamedBufferData(buffer, sizeof(T) * data.Length, (IntPtr)pData, (uint)usage);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData<T>(uint buffer, GLBufferUsage usage, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glNamedBufferData(buffer, sizeof(T) * data.Length, (IntPtr)pData, (uint)usage);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data) {
			unsafe {
				Functions.glNamedBufferSubData(buffer, offset, size, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferSubData<T>(uint buffer, nint offset, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glNamedBufferSubData(buffer, offset, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferSubData<T>(uint buffer, nint offset, params T[] data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glNamedBufferSubData(buffer, offset, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, nint readOffset, nint writeOffset, nint size) {
			unsafe {
				Functions.glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedBufferData<T>(uint buffer, GLInternalFormat internalFormat, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glClearNamedBufferData(buffer, (uint)internalFormat, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedBufferSubData<T>(uint buffer, nint offset, nint size, GLInternalFormat internalFormat, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glClearNamedBufferSubData(buffer, (uint)internalFormat, offset, size, (uint)format, (uint)type, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapNamedBuffer(uint buffer, GLAccess access) {
			unsafe {
				return Functions.glMapNamedBuffer(buffer, (uint)access);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapNamedBufferRange(uint buffer, nint offset, nint length, GLMapAccessFlags access) {
			unsafe {
				return Functions.glMapNamedBufferRange(buffer, offset, length, (uint)access);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool UnmapNamedBuffer(uint buffer) {
			unsafe {
				return Functions.glUnmapNamedBuffer(buffer) != 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedNamedBufferRange(uint buffer, nint offset, nint length) {
			unsafe {
				Functions.glFlushMappedNamedBufferRange(buffer, offset, length);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedBufferParameter(uint buffer, GLGetBufferParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedBufferParameteriv(buffer, (uint)pname, pValues); ;
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetNamedBufferParameter(uint buffer, GLGetBufferParameter pname, Span<long> values) {
			unsafe {
				fixed (long* pValues = values) {
					Functions.glGetNamedBufferParameteri64v(buffer, (uint)pname, pValues); ;
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<IntPtr> GetNamedBufferPointer(uint buffer, GLGetBufferPointer pname, Span<IntPtr> values) {
			unsafe {
				fixed (IntPtr* pValues = values) {
					Functions.glGetNamedBufferPointerv(buffer, (uint)pname, pValues); ;
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetNamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data) {
			unsafe {
				Functions.glGetNamedBufferSubData(buffer, offset, size, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetNamedBufferSubData<T>(uint buffer, nint offset, Span<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glGetNamedBufferSubData(buffer, offset, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
			return data;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateFramebuffers(Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateFramebuffers(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateFramebuffers(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateFramebuffers(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateFramebuffers() {
			uint id = 0;
			unsafe {
				Functions.glCreateFramebuffers(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferRenderbuffer(uint framebuffer, GLFramebufferAttachment attachment, GLRenderbufferTarget renderbufferTarget, uint renderbuffer) {
			unsafe {
				Functions.glNamedFramebufferRenderbuffer(framebuffer, (uint)attachment, (uint)renderbufferTarget, renderbuffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferParameter(uint framebuffer, GLFramebufferParameter pname, int param) {
			unsafe {
				Functions.glNamedFramebufferParameteri(framebuffer, (uint)pname, param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferTexture(uint framebuffer, GLFramebufferAttachment attachment, uint texture, int level) {
			unsafe {
				Functions.glNamedFramebufferTexture(framebuffer, (uint)attachment, texture, level);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferTextureLayer(uint framebuffer, GLFramebufferAttachment attachment, uint texture, int level, int layer) {
			unsafe {
				Functions.glNamedFramebufferTextureLayer(framebuffer, (uint)attachment, texture, level, layer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffer(uint framebuffer, GLDrawBuffer mode) {
			unsafe {
				Functions.glNamedFramebufferDrawBuffer(framebuffer, (uint)mode);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffers(uint framebuffer, in ReadOnlySpan<GLDrawBuffer> bufs) {
			unsafe {
				fixed (GLDrawBuffer* pBufs = bufs) {
					Functions.glNamedFramebufferDrawBuffers(framebuffer, bufs.Length, (uint*)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffers(uint framebuffer, params GLDrawBuffer[] bufs) {
			unsafe {
				fixed (GLDrawBuffer* pBufs = bufs) {
					Functions.glNamedFramebufferDrawBuffers(framebuffer, bufs.Length, (uint*)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferReadBuffer(uint framebuffer, GLDrawBuffer buf) {
			unsafe {
				Functions.glNamedFramebufferReadBuffer(framebuffer, (uint)buf);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferData(uint framebuffer, in ReadOnlySpan<GLFramebufferAttachment> attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferData(framebuffer, attachments.Length, (uint*)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferData(uint framebuffer, params GLFramebufferAttachment[] attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferData(framebuffer, attachments.Length, (uint*)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferSubData(uint framebuffer, in ReadOnlySpan<GLFramebufferAttachment> attachments, Recti area) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferSubData(framebuffer, attachments.Length, (uint*)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferSubData(uint framebuffer, Recti area, params GLFramebufferAttachment[] attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferSubData(framebuffer, attachments.Length, (uint*)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<int> value) {
			unsafe {
				fixed (int* pValue = value) {
					Functions.glClearNamedFramebufferiv(framebuffer, (uint)buffer, drawbuffer, pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, Vector4i value) {
			unsafe {
				Functions.glClearNamedFramebufferiv(framebuffer, (uint)buffer, drawbuffer, (int*)(&value));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<uint> value) {
			unsafe {
				fixed (uint* pValue = value) {
					Functions.glClearNamedFramebufferuiv(framebuffer, (uint)buffer, drawbuffer, pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, Vector4ui value) {
			unsafe {
				Functions.glClearNamedFramebufferuiv(framebuffer, (uint)buffer, drawbuffer, (uint*)(&value));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<float> value) {
			unsafe {
				fixed (float* pValue = value) {
					Functions.glClearNamedFramebufferfv(framebuffer, (uint)buffer, drawbuffer, pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, Vector4 value) {
			unsafe {
				Functions.glClearNamedFramebufferfv(framebuffer, (uint)buffer, drawbuffer, (float*)(&value));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, float depth, int stencil) {
			unsafe {
				Functions.glClearNamedFramebufferfi(framebuffer, (uint)buffer, drawbuffer, depth, stencil);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, Recti src, Recti dst, GLBufferMask mask, GLFilter filter) {
			Vector2i srcmin = src.Minimum, srcmax = src.Maximum;
			Vector2i dstmin = dst.Minimum, dstmax = dst.Maximum;
			unsafe {
				Functions.glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcmin.X, srcmin.Y, srcmax.X, srcmax.Y, dstmin.X, dstmin.Y, dstmax.X, dstmax.Y, (uint)mask, (uint)filter);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebufferStatus CheckNamedFramebufferStatus(uint framebuffer, GLFramebufferTarget target) {
			unsafe {
				return (GLFramebufferStatus)Functions.glCheckNamedFramebufferStatus(framebuffer, (uint)target);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedFramebufferParameter(uint framebuffer, GLFramebufferParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedFramebufferParameteriv(framebuffer, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedFramebufferAttachmentParameter(uint framebuffer, GLFramebufferAttachment attachment, GLGetFramebufferAttachment pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedFramebufferAttachmentParameteriv(framebuffer, (uint)attachment, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateRenderbuffers(Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateRenderbuffers(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateRenderbuffers(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateRenderbuffers(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateRenderbuffers() {
			uint id = 0;
			unsafe {
				Functions.glCreateRenderbuffers(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedRenderbufferStorage(uint renderbuffer, GLInternalFormat internalFormat, int width, int height) {
			unsafe {
				Functions.glNamedRenderbufferStorage(renderbuffer, (uint)internalFormat, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, GLInternalFormat internalFormat, int width, int height) {
			unsafe {
				Functions.glNamedRenderbufferStorageMultisample(renderbuffer, samples, (uint)internalFormat, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedRenderbufferParameter(uint renderbuffer, GLGetRenderbuffer pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedRenderbufferParameteriv(renderbuffer, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateTextures(GLTextureTarget target, Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTextures((uint)target, ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateTextures(GLTextureTarget target, int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTextures((uint)target, ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateTextures(GLTextureTarget target) {
			uint id = 0;
			unsafe {
				Functions.glCreateTextures((uint)target, 1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBuffer(uint texture, GLInternalFormat internalFormat, uint buffer) {
			unsafe {
				Functions.glTextureBuffer(texture, (uint)internalFormat, buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBufferRange(uint texture, GLInternalFormat internalFormat, uint buffer, nint offset, nint size) {
			unsafe {
				Functions.glTextureBufferRange(texture, (uint)internalFormat, buffer, offset, size);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage1D(uint texture, int levels, GLInternalFormat internalFormat, int width) {
			unsafe {
				Functions.glTextureStorage1D(texture, levels, (uint)internalFormat, width);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage2D(uint texture, int levels, GLInternalFormat internalFormat, int width, int height) {
			unsafe {
				Functions.glTextureStorage2D(texture, levels, (uint)internalFormat, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage3D(uint texture, int levels, GLInternalFormat internalFormat, int width, int height, int depth) {
			unsafe {
				Functions.glTextureStorage3D(texture, levels, (uint)internalFormat, width, height, depth);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage2DMultisample(uint texture, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) {
			unsafe {
				Functions.glTextureStorage2DMultisample(texture, samples, (uint)internalFormat, width, height, (byte)(fixedSampleLocations ? 1 : 0));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage3DMultisample(uint texture, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) {
			unsafe {
				Functions.glTextureStorage3DMultisample(texture, samples, (uint)internalFormat, width, height, depth, (byte)(fixedSampleLocations ? 1 : 0));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage1D(uint texture, int level, int xoffset, int width, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				Functions.glTextureSubImage1D(texture, level, xoffset, width, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage1D<T>(uint texture, int level, int xoffset, int width, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					Functions.glTextureSubImage1D(texture, level, xoffset, width, (uint)format, (uint)type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				Functions.glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					Functions.glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, (uint)type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, IntPtr pixels) {
			unsafe {
				Functions.glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, (uint)type, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage3D<T>(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					Functions.glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, (uint)type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, GLInternalFormat format, int imageSize, IntPtr data) {
			unsafe {
				Functions.glCompressedTextureSubImage1D(texture, level, xoffset, width, (uint)format, imageSize, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage1D<T>(uint texture, int level, int xoffset, int width, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glCompressedTextureSubImage1D(texture, level, xoffset, width, (uint)format, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, GLInternalFormat format, int imageSize, IntPtr data) {
			unsafe {
				Functions.glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, imageSize, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLInternalFormat format, int imageSize, IntPtr data) {
			unsafe {
				Functions.glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, imageSize, data);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage3D<T>(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) {
			unsafe {
				Functions.glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) {
			unsafe {
				Functions.glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) {
			unsafe {
				Functions.glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, float param) {
			unsafe {
				Functions.glTextureParameterf(texture, (uint)pname, param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, in ReadOnlySpan<float> param) {
			unsafe {
				fixed (float* pParam = param) {
					Functions.glTextureParameterfv(texture, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, int param) {
			unsafe {
				Functions.glTextureParameteri(texture, (uint)pname, param);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, in ReadOnlySpan<int> param) {
			unsafe {
				fixed (int* pParam = param) {
					Functions.glTextureParameteriv(texture, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameterI(uint texture, GLTexParamter pname, in ReadOnlySpan<int> param) {
			unsafe {
				fixed (int* pParam = param) {
					Functions.glTextureParameterIiv(texture, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameterI(uint texture, GLTexParamter pname, in ReadOnlySpan<uint> param) {
			unsafe {
				fixed (uint* pParam = param) {
					Functions.glTextureParameterIuiv(texture, (uint)pname, pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenerateTextureMipmap(uint texture) {
			unsafe {
				Functions.glGenerateTextureMipmap(texture);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextureUnit(uint unit, uint texture) {
			unsafe {
				Functions.glBindTextureUnit(unit, texture);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTextureImage(uint texture, int level, GLFormat format, GLTextureType type, int bufSize, IntPtr pixels) {
			unsafe {
				Functions.glGetTextureImage(texture, level, (uint)format, (uint)type, bufSize, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetTextureImage<T>(uint texture, int level, GLFormat format, GLTextureType type, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					Functions.glGetTextureImage(texture, level, (uint)format, (uint)type, sizeof(T) * pixels.Length, (IntPtr)pPixels);
				}
			}
			return pixels;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels) {
			unsafe {
				Functions.glGetCompressedTextureImage(texture, level, bufSize, pixels);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<T> GetCompressedTextureImage<T>(uint texture, int level, Span<T> pixels) where T : unmanaged {
			unsafe {
				fixed(T* pPixels = pixels) {
					Functions.glGetCompressedTextureImage(texture, level, sizeof(T) * pixels.Length, (IntPtr)pPixels);
				}
			}
			return pixels;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTextureLevelParameter(uint texture, int level, GLGetTexLevelParameter pname, Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					Functions.glGetTextureLevelParameterfv(texture, level, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureLevelParameter(uint texture, int level, GLGetTexLevelParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTextureLevelParameteriv(texture, level, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTextureParameter(uint texture, GLTexParamter pname, Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					Functions.glGetTextureParameterfv(texture, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureParameter(uint texture, GLTexParamter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTextureParameteriv(texture, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureParameterI(uint texture, GLTexParamter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTextureParameterIiv(texture, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTextureParameterI(uint texture, GLTexParamter pname, Span<uint> values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glGetTextureParameterIuiv(texture, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateVertexArrays(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateVertexArrays(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateVertexArrays(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateVertexArrays(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateVertexArrays() {
			uint id = 0;
			unsafe {
				Functions.glCreateVertexArrays(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DisableVertexArrayAttrib(uint vaobj, uint index) {
			unsafe {
				Functions.glDisableVertexArrayAttrib(vaobj, index);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnableVertexArrayAttrib(uint vaobj, uint index) {
			unsafe {
				Functions.glEnableVertexArrayAttrib(vaobj, index);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayElementBuffer(uint vaobj, uint buffer) {
			unsafe {
				Functions.glVertexArrayElementBuffer(vaobj, buffer);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayVertexBuffer(uint vaobj, uint bindingIndex, uint buffer, nint offset, int stride) {
			unsafe {
				Functions.glVertexArrayVertexBuffer(vaobj, bindingIndex, buffer, offset, stride);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayVertexBuffers(uint vaobj, uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<int> strides) {
			int n = ExMath.Min(buffers.Length, offsets.Length, strides.Length);
			unsafe {
				fixed(uint* pBuffers = buffers) {
					fixed(nint* pOffsets = offsets) {
						fixed(int* pStrides = strides) {
							Functions.glVertexArrayVertexBuffers(vaobj, first, n, pBuffers, pOffsets, pStrides);
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribFormat(uint vaobj, uint attribIndex, int size, GLTextureType type, bool normalized, uint relativeOffset) {
			unsafe {
				Functions.glVertexArrayAttribFormat(vaobj, attribIndex, size, (uint)type, (byte)(normalized ? 1 : 0), relativeOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribIFormat(uint vaobj, uint attribIndex, int size, GLTextureType type, uint relativeOffset) {
			unsafe {
				Functions.glVertexArrayAttribIFormat(vaobj, attribIndex, size, (uint)type, relativeOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribLFormat(uint vaobj, uint attribIndex, int size, GLTextureType type, uint relativeOffset) {
			unsafe {
				Functions.glVertexArrayAttribLFormat(vaobj, attribIndex, size, (uint)type, relativeOffset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribBinding(uint vaobj, uint attribIndex, uint bindingIndex) {
			unsafe {
				Functions.glVertexArrayAttribBinding(vaobj, attribIndex, bindingIndex);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayBindingDivisor(uint vaobj, uint bindingIndex, uint divisor) {
			unsafe {
				Functions.glVertexArrayBindingDivisor(vaobj, bindingIndex, divisor);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexArray(uint vaobj, GLGetVertexArray pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetVertexArrayiv(vaobj, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexArrayIndexed(uint vaobj, uint index, GLGetVertexArrayIndexed pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetVertexArrayIndexediv(vaobj, index, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetVertexArrayIndexed(uint vaobj, uint index, GLGetVertexArrayIndexed pname, Span<long> values) {
			unsafe {
				fixed (long* pValues = values) {
					Functions.glGetVertexArrayIndexed64iv(vaobj, index, (uint)pname, pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateSamplers(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateSamplers(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateSamplers(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateSamplers(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateSamplers() {
			uint id = 0;
			unsafe {
				Functions.glCreateSamplers(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateProgramPipelines(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateProgramPipelines(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateProgramPipelines(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateProgramPipelines(ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateProgramPipelines() {
			uint id = 0;
			unsafe {
				Functions.glCreateProgramPipelines(1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateQueries(GLQueryTarget target, Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateQueries((uint)target, ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateQueries(GLQueryTarget target, int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateQueries((uint)target, ids.Length, pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateQueries(GLQueryTarget target) {
			uint id = 0;
			unsafe {
				Functions.glCreateQueries((uint)target, 1, &id);
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectiv(uint id, uint buffer, GLGetQueryObject pname, nint offset) {
			unsafe {
				Functions.glGetQueryBufferObjectiv(id, buffer, (uint)pname, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectuiv(uint id, uint buffer, GLGetQueryObject pname, nint offset) {
			unsafe {
				Functions.glGetQueryBufferObjectuiv(id, buffer, (uint)pname, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjecti64v(uint id, uint buffer, GLGetQueryObject pname, nint offset) {
			unsafe {
				Functions.glGetQueryBufferObjecti64v(id, buffer, (uint)pname, offset);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectui64v(uint id, uint buffer, GLGetQueryObject pname, nint offset) {
			unsafe {
				Functions.glGetQueryBufferObjectui64v(id, buffer, (uint)pname, offset);
			}
		}
	}
}
