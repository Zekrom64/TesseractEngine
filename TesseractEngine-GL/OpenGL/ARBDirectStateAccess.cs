using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Native;

namespace Tesseract.OpenGL {

#nullable disable
	public class ARBDirectStateAccessFunctions {

		public delegate void PFN_glCreateTransformFeedbacks(int n, [NativeType("GLuint*")] IntPtr ids);
		[ExternFunction(AltNames = new string[] { "glCreateTransformFeedbacksARB" })]
		public PFN_glCreateTransformFeedbacks glCreateTransformFeedbacks;
		public delegate void PFN_glTransformFeedbackBufferBase(uint xfb, uint index, uint buffer);
		[ExternFunction(AltNames = new string[] { "glTransformFeedbackBufferBaseARB" })]
		public PFN_glTransformFeedbackBufferBase glTransformFeedbackBufferBase;
		public delegate void PFN_glTransformFeedbackBufferRange(uint xfb, uint index, uint buffer, nint offset, nint size);
		[ExternFunction(AltNames = new string[] { "glTransformFeedbackBufferRangeARB" })]
		public PFN_glTransformFeedbackBufferRange glTransformFeedbackBufferRange;
		public delegate void PFN_glGetTransformFeedbackiv(uint xfb, uint pname, [NativeType("GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbackivARB" })]
		public PFN_glGetTransformFeedbackiv glGetTransformFeedbackiv;
		public delegate void PFN_glGetTransformFeedbacki_v(uint xfb, uint pname, uint index, [NativeType("GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbacki_vARB" })]
		public PFN_glGetTransformFeedbacki_v glGetTransformFeedbacki_v;
		public delegate void PFN_glGetTransformFeedbacki64_v(uint xfb, uint pname, uint index, [NativeType("GLint64*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetTransformFeedbacki64_vARB" })]
		public PFN_glGetTransformFeedbacki64_v glGetTransformFeedbacki64_v;

		public delegate void PFN_glCreateBuffers(int n, [NativeType("GLuint*")] IntPtr buffers);
		[ExternFunction(AltNames = new string[] { "glCreateBuffersARB" })]
		public PFN_glCreateBuffers glCreateBuffers;
		public delegate void PFN_glNamedBufferStorage(uint buffer, nint size, IntPtr data, uint flags);
		[ExternFunction(AltNames = new string[] { "glNamedBufferStorageARB" })]
		public PFN_glNamedBufferStorage glNamedBufferStorage;
		public delegate void PFN_glNamedBufferData(uint buffer, nint size, IntPtr data, uint usage);
		[ExternFunction(AltNames = new string[] { "glNamedBufferDataARB" })]
		public PFN_glNamedBufferData glNamedBufferData;
		public delegate void PFN_glNamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glNamedBufferSubDataARB" })]
		public PFN_glNamedBufferSubData glNamedBufferSubData;
		public delegate void PFN_glCopyNamedBufferSubData(uint readBuffer, uint writeBuffer, nint readOffset, nint writeOffset, nint size);
		[ExternFunction(AltNames = new string[] { "glCopyNamedBufferSubDataARB" })]
		public PFN_glCopyNamedBufferSubData glCopyNamedBufferSubData;
		public delegate void PFN_glClearNamedBufferData(uint buffer, uint internalformat, uint format, uint type, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glClearNamedBufferDataARB" })]
		public PFN_glClearNamedBufferData glClearNamedBufferData;
		public delegate void PFN_glClearNamedBufferSubData(uint buffer, uint internalformat, nint offset, nint size, uint format, uint type, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glClearNamedBufferSubDataARB" })]
		public PFN_glClearNamedBufferSubData glClearNamedBufferSubData;
		public delegate IntPtr PFN_glMapNamedBuffer(uint buffer, uint access);
		[ExternFunction(AltNames = new string[] { "glMapNamedBufferARB" })]
		public PFN_glMapNamedBuffer glMapNamedBuffer;
		public delegate IntPtr PFN_glMapNamedBufferRange(uint buffer, nint offset, nint length, uint access);
		[ExternFunction(AltNames = new string[] { "glMapNamedBufferRangeARB" })]
		public PFN_glMapNamedBufferRange glMapNamedBufferRange;
		public delegate byte PFN_glUnmapNamedBuffer(uint buffer);
		[ExternFunction(AltNames = new string[] { "glUnmapNamedBufferARB" })]
		public PFN_glUnmapNamedBuffer glUnmapNamedBuffer;
		public delegate void PFN_glFlushMappedNamedBufferRange(uint buffer, nint offset, nint length);
		[ExternFunction(AltNames = new string[] { "glFlushMappedNamedBufferRangeARB" })]
		public PFN_glFlushMappedNamedBufferRange glFlushMappedNamedBufferRange;
		public delegate void PFN_glGetNamedBufferParameteriv(uint buffer, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferParameterivARB" })]
		public PFN_glGetNamedBufferParameteriv glGetNamedBufferParameteriv;
		public delegate void PFN_glGetNamedBufferParameteri64v(uint buffer, uint pname, [NativeType("GLint64*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferParameteri64vARB" })]
		public PFN_glGetNamedBufferParameteri64v glGetNamedBufferParameteri64v;
		public delegate void PFN_glGetNamedBufferPointerv(uint buffer, uint pname, [NativeType("void**")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferPointervARB" })]
		public PFN_glGetNamedBufferPointerv glGetNamedBufferPointerv;
		public delegate void PFN_glGetNamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glGetNamedBufferSubDataARB" })]
		public PFN_glGetNamedBufferSubData glGetNamedBufferSubData;

		public delegate void PFN_glCreateFramebuffers(int n, [NativeType("GLuint*")] IntPtr framebuffers);
		[ExternFunction(AltNames = new string[] { "glCreateFramebuffersARB" })]
		public PFN_glCreateFramebuffers glCreateFramebuffers;
		public delegate void PFN_glNamedFramebufferRenderbuffer(uint framebuffer, uint attachment, uint renderbuffertarget, uint renderbuffer);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferRenderbufferARB" })]
		public PFN_glNamedFramebufferRenderbuffer glNamedFramebufferRenderbuffer;
		public delegate void PFN_glNamedFramebufferParameteri(uint framebuffer, uint pname, int param);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferParameteriARB" })]
		public PFN_glNamedFramebufferParameteri glNamedFramebufferParameteri;
		public delegate void PFN_glNamedFramebufferTexture(uint framebuffer, uint attachment, uint texture, int level);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferTextureARB" })]
		public PFN_glNamedFramebufferTexture glNamedFramebufferTexture;
		public delegate void PFN_glNamedFramebufferTextureLayer(uint framebuffer, uint attachment, uint texture, int level, int layer);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferTextureLayerARB" })]
		public PFN_glNamedFramebufferTextureLayer glNamedFramebufferTextureLayer;
		public delegate void PFN_glNamedFramebufferDrawBuffer(uint framebuffer, uint mode);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferDrawBufferARB" })]
		public PFN_glNamedFramebufferDrawBuffer glNamedFramebufferDrawBuffer;
		public delegate void PFN_glNamedFramebufferDrawBuffers(uint framebuffer, int n, [NativeType("const GLenum*")] IntPtr bufs);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferDrawBuffersARB" })]
		public PFN_glNamedFramebufferDrawBuffers glNamedFramebufferDrawBuffers;
		public delegate void PFN_glNamedFramebufferReadBuffer(uint framebuffer, uint mode);
		[ExternFunction(AltNames = new string[] { "glNamedFramebufferReadBufferARB" })]
		public PFN_glNamedFramebufferReadBuffer glNamedFramebufferReadBuffer;
		public delegate void PFN_glInvalidateNamedFramebufferData(uint framebuffer, int numAttachments, [NativeType("const GLenum*")] IntPtr attachments);
		[ExternFunction(AltNames = new string[] { "glInvalidateNamedFramebufferDataARB" })]
		public PFN_glInvalidateNamedFramebufferData glInvalidateNamedFramebufferData;
		public delegate void PFN_glInvalidateNamedFramebufferSubData(uint framebuffer, int numAttachments, [NativeType("const GLenum*")] IntPtr attachments, int x, int y, int width, int height);
		[ExternFunction(AltNames = new string[] { "glInvalidateNamedFramebufferSubDataARB" })]
		public PFN_glInvalidateNamedFramebufferSubData glInvalidateNamedFramebufferSubData;
		public delegate void PFN_glClearNamedFramebufferiv(uint framebuffer, uint buffer, int drawbuffer, [NativeType("const GLint*")] IntPtr value);
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferivARB" })]
		public PFN_glClearNamedFramebufferiv glClearNamedFramebufferiv;
		public delegate void PFN_glClearNamedFramebufferuiv(uint framebuffer, uint buffer, int drawbuffer, [NativeType("const GLuint*")] IntPtr value);
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferuivARB" })]
		public PFN_glClearNamedFramebufferuiv glClearNamedFramebufferuiv;
		public delegate void PFN_glClearNamedFramebufferfv(uint framebuffer, uint buffer, int drawbuffer, [NativeType("const GLfloat*")] IntPtr value);
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferfvARB" })]
		public PFN_glClearNamedFramebufferfv glClearNamedFramebufferfv;
		public delegate void PFN_glClearNamedFramebufferfi(uint framebuffer, uint buffer, int drawbuffer, float depth, int stencil);
		[ExternFunction(AltNames = new string[] { "glClearNamedFramebufferfiARB" })]
		public PFN_glClearNamedFramebufferfi glClearNamedFramebufferfi;
		public delegate void PFN_glBlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, uint filter);
		[ExternFunction(AltNames = new string[] { "glBlitNamedFramebufferARB" })]
		public PFN_glBlitNamedFramebuffer glBlitNamedFramebuffer;
		public delegate uint PFN_glCheckNamedFramebufferStatus(uint framebuffer, uint target);
		[ExternFunction(AltNames = new string[] { "glCheckNamedFramebufferStatusARB" })]
		public PFN_glCheckNamedFramebufferStatus glCheckNamedFramebufferStatus;
		public delegate void PFN_glGetNamedFramebufferParameteriv(uint framebuffer, uint pname, [NativeType("GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetNamedFramebufferParameterivARB" })]
		public PFN_glGetNamedFramebufferParameteriv glGetNamedFramebufferParameteriv;
		public delegate void PFN_glGetNamedFramebufferAttachmentParameteriv(uint framebuffer, uint attachment, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetnamedFramebufferAttachmentParameterivARB" })]
		public PFN_glGetNamedFramebufferAttachmentParameteriv glGetNamedFramebufferAttachmentParameteriv;

		public delegate void PFN_glCreateRenderbuffers(int n, [NativeType("GLuint*")] IntPtr renderbuffers);
		[ExternFunction(AltNames = new string[] { "glCreateRenderbuffersARB" })]
		public PFN_glCreateRenderbuffers glCreateRenderbuffers;
		public delegate void PFN_glNamedRenderbufferStorage(uint renderbuffer, uint internalformat, int width, int height);
		[ExternFunction(AltNames = new string[] { "glNamedRenderbufferStorageARB" })]
		public PFN_glNamedRenderbufferStorage glNamedRenderbufferStorage;
		public delegate void PFN_glNamedRenderbufferStorageMultisample(uint renderbuffer, int samples, uint internalformat, int width, int height);
		[ExternFunction(AltNames = new string[] { "glNamedRenderbufferStorageMultisampleARB" })]
		public PFN_glNamedRenderbufferStorageMultisample glNamedRenderbufferStorageMultisample;
		public delegate void PFN_glGetNamedRenderbufferParameteriv(uint renderbuffer, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetNamedRenderbufferParameterivARB" })]
		public PFN_glGetNamedRenderbufferParameteriv glGetNamedRenderbufferParameteriv;

		public delegate void PFN_glCreateTextures(uint target, int n, [NativeType("GLuint*")] IntPtr textures);
		[ExternFunction(AltNames = new string[] { "glCreateTexturesARB" })]
		public PFN_glCreateTextures glCreateTextures;
		public delegate void PFN_glTextureBuffer(uint texture, uint internalformat, uint buffer);
		[ExternFunction(AltNames = new string[] { "glTextureBufferARB" })]
		public PFN_glTextureBuffer glTextureBuffer;
		public delegate void PFN_glTextureBufferRange(uint texture, uint internalformat, uint buffer, nint offset, nint size);
		[ExternFunction(AltNames = new string[] { "glTextureBufferRangeARB" })]
		public PFN_glTextureBufferRange glTextureBufferRange;
		public delegate void PFN_glTextureStorage1D(uint texture, int levels, uint internalformat, int width);
		[ExternFunction(AltNames = new string[] { "glTextureStorage1DARB" })]
		public PFN_glTextureStorage1D glTextureStorage1D;
		public delegate void PFN_glTextureStorage2D(uint texture, int levels, uint internalformat, int width, int height);
		[ExternFunction(AltNames = new string[] { "glTextureStorage2DARB" })]
		public PFN_glTextureStorage2D glTextureStorage2D;
		public delegate void PFN_glTextureStorage3D(uint texture, int levels, uint internalformat, int width, int height, int depth);
		[ExternFunction(AltNames = new string[] { "glTextureStorage3DARB" })]
		public PFN_glTextureStorage3D glTextureStorage3D;
		public delegate void PFN_glTextureStorage2DMultisample(uint texture, int samples, uint internalformat, int width, int height, byte fixedsamplelocations);
		[ExternFunction(AltNames = new string[] { "glTextureStorage2DMultisampleARB" })]
		public PFN_glTextureStorage2DMultisample glTextureStorage2DMultisample;
		public delegate void PFN_glTextureStorage3DMultisample(uint texture, int samples, uint internalformat, int width, int height, int depth, byte fixedsamplelocations);
		[ExternFunction(AltNames = new string[] { "glTextureStorage3DMultisampleARB" })]
		public PFN_glTextureStorage3DMultisample glTextureStorage3DMultisample;
		public delegate void PFN_glTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, uint type, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glTextureSubImage1DARB" })]
		public PFN_glTextureSubImage1D glTextureSubImage1D;
		public delegate void PFN_glTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, uint type, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glTextureSubImage2DARB" })]
		public PFN_glTextureSubImage2D glTextureSubImage2D;
		public delegate void PFN_glTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glTextureSubImage3DARB" })]
		public PFN_glTextureSubImage3D glTextureSubImage3D;
		public delegate void PFN_glCompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glCompressedTextureSubImage1DARB" })]
		public PFN_glCompressedTextureSubImage1D glCompressedTextureSubImage1D;
		public delegate void PFN_glCompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glCompressedTextureSubImage2DARB" })]
		public PFN_glCompressedTextureSubImage2D glCompressedTextureSubImage2D;
		public delegate void PFN_glCompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
		[ExternFunction(AltNames = new string[] { "glCompressedTextureSubImage3DARB" })]
		public PFN_glCompressedTextureSubImage3D glCompressedTextureSubImage3D;
		public delegate void PFN_glCopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width);
		[ExternFunction(AltNames = new string[] { "glCopyTextureSubImage1DARB" })]
		public PFN_glCopyTextureSubImage1D glCopyTextureSubImage1D;
		public delegate void PFN_glCopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height);
		[ExternFunction(AltNames = new string[] { "glCopyTextureSubImage2DARB" })]
		public PFN_glCopyTextureSubImage2D glCopyTextureSubImage2D;
		public delegate void PFN_glCopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
		[ExternFunction(AltNames = new string[] { "glCopyTextureSubImage3DARB" })]
		public PFN_glCopyTextureSubImage3D glCopyTextureSubImage3D;
		public delegate void PFN_glTextureParameterf(uint texture, uint pname, float param);
		[ExternFunction(AltNames = new string[] { "glTextureParameterfARB" })]
		public PFN_glTextureParameterf glTextureParameterf;
		public delegate void PFN_glTextureParameterfv(uint texture, uint pname, [NativeType("const GLfloat*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glTextureParameterfvARB" })]
		public PFN_glTextureParameterfv glTextureParameterfv;
		public delegate void PFN_glTextureParameteri(uint texture, uint pname, int param);
		[ExternFunction(AltNames = new string[] { "glTextureParameteriARB" })]
		public PFN_glTextureParameteri glTextureParameteri;
		public delegate void PFN_glTextureParameteriv(uint texture, uint pname, [NativeType("const GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glTextureParameterivARB" })]
		public PFN_glTextureParameteriv glTextureParameteriv;
		public delegate void PFN_glTextureParameterIiv(uint texture, uint pname, [NativeType("const GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glTextureParameterIivARB" })]
		public PFN_glTextureParameterIiv glTextureParameterIiv;
		public delegate void PFN_glTextureParameterIuiv(uint texture, uint pname, [NativeType("const GLuint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glTextureParameterIuivARB" })]
		public PFN_glTextureParameterIuiv glTextureParameterIuiv;
		public delegate void PFN_glGenerateTextureMipmap(uint texture);
		[ExternFunction(AltNames = new string[] { "glGenerateTextureMipmapARB" })]
		public PFN_glGenerateTextureMipmap glGenerateTextureMipmap;
		public delegate void PFN_glBindTextureUnit(uint unit, uint texture);
		[ExternFunction(AltNames = new string[] { "glBindTextureUnitARB" })]
		public PFN_glBindTextureUnit glBindTextureUnit;
		public delegate void PFN_glGetTextureImage(uint texture, int level, uint format, uint type, int bufSize, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glGetTextureImageARB" })]
		public PFN_glGetTextureImage glGetTextureImage;
		public delegate void PFN_glGetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels);
		[ExternFunction(AltNames = new string[] { "glGetCompressedTextureImageARB" })]
		public PFN_glGetCompressedTextureImage glGetCompressedTextureImage;
		public delegate void PFN_glGetTextureLevelParameterfv(uint texture, int level, uint pname, [NativeType("GLfloat*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTextureLevelParameterfvARB" })]
		public PFN_glGetTextureLevelParameterfv glGetTextureLevelParameterfv;
		public delegate void PFN_glGetTextureLevelParameteriv(uint texture, int level, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTextureLevelParameterivARB" })]
		public PFN_glGetTextureLevelParameteriv glGetTextureLevelParameteriv;
		public delegate void PFN_glGetTextureParameterfv(uint texture, uint pname, [NativeType("GLfloat*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterfvARB" })]
		public PFN_glGetTextureParameterfv glGetTextureParameterfv;
		public delegate void PFN_glGetTextureParameteriv(uint texture, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterivARB" })]
		public PFN_glGetTextureParameteriv glGetTextureParameteriv;
		public delegate void PFN_glGetTextureParameterIiv(uint texture, uint pname, [NativeType("GLint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterIivARB" })]
		public PFN_glGetTextureParameterIiv glGetTextureParameterIiv;
		public delegate void PFN_glGetTextureParameterIuiv(uint texture, uint pname, [NativeType("GLuint*")] IntPtr _params);
		[ExternFunction(AltNames = new string[] { "glGetTextureParameterIuivARB" })]
		public PFN_glGetTextureParameterIuiv glGetTextureParameterIuiv;

		public delegate void PFN_glCreateVertexArrays(int n, [NativeType("GLuint*")] IntPtr arrays);
		[ExternFunction(AltNames = new string[] { "glCreateVertexArraysARB" })]
		public PFN_glCreateVertexArrays glCreateVertexArrays;
		public delegate void PFN_glDisableVertexArrayAttrib(uint vaobj, uint index);
		[ExternFunction(AltNames = new string[] { "glDisableVertexArrayAttribARB" })]
		public PFN_glDisableVertexArrayAttrib glDisableVertexArrayAttrib;
		public delegate void PFN_glEnableVertexArrayAttrib(uint vaobj, uint index);
		[ExternFunction(AltNames = new string[] { "glEnableVertexArrayAttribARB" })]
		public PFN_glEnableVertexArrayAttrib glEnableVertexArrayAttrib;
		public delegate void PFN_glVertexArrayElementBuffer(uint vaobj, uint buffer);
		[ExternFunction(AltNames = new string[] { "glVertexArrayElementBufferARB" })]
		public PFN_glVertexArrayElementBuffer glVertexArrayElementBuffer;
		public delegate void PFN_glVertexArrayVertexBuffer(uint vaobj, uint bindingindex, uint buffer, nint offset, int stride);
		[ExternFunction(AltNames = new string[] { "glVertexArrayVertexBufferARB" })]
		public PFN_glVertexArrayVertexBuffer glVertexArrayVertexBuffer;
		public delegate void PFN_glVertexArrayVertexBuffers(uint vaobj, uint first, int count, [NativeType("const GLuint*")] IntPtr buffers, [NativeType("const GLintptr*")] IntPtr offsets, [NativeType("const GLsizei*")] IntPtr strides);
		[ExternFunction(AltNames = new string[] { "glVertexArrayVertexBuffersARB" })]
		public PFN_glVertexArrayVertexBuffers glVertexArrayVertexBuffers;
		public delegate void PFN_glVertexArrayAttribFormat(uint vaobj, uint attribindex, int size, uint type, byte normalized, uint relativeoffset);
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribFormatARB" })]
		public PFN_glVertexArrayAttribFormat glVertexArrayAttribFormat;
		public delegate void PFN_glVertexArrayAttribIFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribIFormatARB" })]
		public PFN_glVertexArrayAttribIFormat glVertexArrayAttribIFormat;
		public delegate void PFN_glVertexArrayAttribLFormat(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribLFormatARB" })]
		public PFN_glVertexArrayAttribLFormat glVertexArrayAttribLFormat;
		public delegate void PFN_glVertexArrayAttribBinding(uint vaobj, uint attribindex, uint bindingindex);
		[ExternFunction(AltNames = new string[] { "glVertexArrayAttribBindingARB" })]
		public PFN_glVertexArrayAttribBinding glVertexArrayAttribBinding;
		public delegate void PFN_glVertexArrayBindingDivisor(uint vaobj, uint bindingindex, uint divisor);
		[ExternFunction(AltNames = new string[] { "glVertexArrayBindingDivisorARB" })]
		public PFN_glVertexArrayBindingDivisor glVertexArrayBindingDivisor;
		public delegate void PFN_glGetVertexArrayiv(uint vaobj, uint pname, [NativeType("GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetVertexArrayivARB" })]
		public PFN_glGetVertexArrayiv glGetVertexArrayiv;
		public delegate void PFN_glGetVertexArrayIndexediv(uint vaobj, uint index, uint pname, [NativeType("GLint*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetVertexArrayIndexedivARB" })]
		public PFN_glGetVertexArrayIndexediv glGetVertexArrayIndexediv;
		public delegate void PFN_glGetVertexArrayIndexed64iv(uint vaobj, uint index, uint pname, [NativeType("GLint64*")] IntPtr param);
		[ExternFunction(AltNames = new string[] { "glGetVertexArrayIndexed64ivARB" })]
		public PFN_glGetVertexArrayIndexed64iv glGetVertexArrayIndexed64iv;

		public delegate void PFN_glCreateSamplers(int n, [NativeType("GLuint*")] IntPtr samplers);
		[ExternFunction(AltNames = new string[] { "glCreateSamplersARB" })]
		public PFN_glCreateSamplers glCreateSamplers;

		public delegate void PFN_glCreateProgramPipelines(int n, [NativeType("GLuint*")] IntPtr pipelines);
		[ExternFunction(AltNames = new string[] { "glCreateProgramPipelinesARB" })]
		public PFN_glCreateProgramPipelines glCreateProgramPipelines;

		public delegate void PFN_glCreateQueries(uint target, int n, [NativeType("GLuint*")] IntPtr ids);
		[ExternFunction(AltNames = new string[] { "glCreateQueriesARB" })]
		public PFN_glCreateQueries glCreateQueries;
		public delegate void PFN_glGetQueryBufferObjectiv(uint id, uint buffer, uint pname, nint offset);
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjectivARB" })]
		public PFN_glGetQueryBufferObjectiv glGetQueryBufferObjectiv;
		public delegate void PFN_glGetQueryBufferObjectuiv(uint id, uint buffer, uint pname, nint offset);
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjectuivARB" })]
		public PFN_glGetQueryBufferObjectuiv glGetQueryBufferObjectuiv;
		public delegate void PFN_glGetQueryBufferObjecti64v(uint id, uint buffer, uint pname, nint offset);
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjecti64vARB" })]
		public PFN_glGetQueryBufferObjecti64v glGetQueryBufferObjecti64v;
		public delegate void PFN_glGetQueryBufferObjectui64v(uint id, uint buffer, uint pname, nint offset);
		[ExternFunction(AltNames = new string[] { "glGetQueryBufferObjectui64vARB" })]
		public PFN_glGetQueryBufferObjectui64v glGetQueryBufferObjectui64v;

	}
#nullable restore

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
					Functions.glCreateTransformFeedbacks(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateTransformFeedbacks(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTransformFeedbacks(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateTransformFeedbacks() {
			uint id = 0;
			unsafe {
				Functions.glCreateTransformFeedbacks(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackBufferBase(uint xfb, uint index, uint buffer) => Functions.glTransformFeedbackBufferBase(xfb, index, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TransformFeedbackBufferRange(uint xfb, uint index, uint buffer, nint offset, nint size) => Functions.glTransformFeedbackBufferRange(xfb, index, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTransformFeedbackiv(xfb, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, uint index, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTransformFeedbacki_v(xfb, (uint)pname, index, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetTransformFeedback(uint xfb, GLGetTransformFeedback pname, uint index, Span<long> values) {
			unsafe {
				fixed (long* pValues = values) {
					Functions.glGetTransformFeedbacki64_v(xfb, (uint)pname, index, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateBuffers(Span<uint> buffers) {
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glCreateBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateBuffers(int n) {
			uint[] buffers = new uint[n];
			unsafe {
				fixed (uint* pBuffers = buffers) {
					Functions.glCreateBuffers(buffers.Length, (IntPtr)pBuffers);
				}
			}
			return buffers;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateBuffers() {
			uint id = 0;
			unsafe {
				Functions.glCreateBuffers(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage(uint buffer, nint size, IntPtr data, GLBufferStorageFlags flags) => Functions.glNamedBufferStorage(buffer, size, data, (uint)flags);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferStorage(uint buffer, nint size, GLBufferStorageFlags flags) => Functions.glNamedBufferStorage(buffer, size, IntPtr.Zero, (uint)flags);

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
		public void NamedBufferData(uint buffer, nint size, IntPtr data, GLBufferUsage usage) => Functions.glNamedBufferData(buffer, size, data, (uint)usage);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedBufferData(uint buffer, nint size, GLBufferUsage usage) => Functions.glNamedBufferData(buffer, size, IntPtr.Zero, (uint)usage);

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
		public void NamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data) => Functions.glNamedBufferSubData(buffer, offset, size, data);

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
		public void CopyNamedBufferSubData(uint readBuffer, uint writeBuffer, nint readOffset, nint writeOffset, nint size) => Functions.glCopyNamedBufferSubData(readBuffer, writeBuffer, readOffset, writeOffset, size);

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
		public IntPtr MapNamedBuffer(uint buffer, GLAccess access) => Functions.glMapNamedBuffer(buffer, (uint)access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntPtr MapNamedBufferRange(uint buffer, nint offset, nint length, GLMapAccessFlags access) => Functions.glMapNamedBufferRange(buffer, offset, length, (uint)access);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool UnmapNamedBuffer(uint buffer) => Functions.glUnmapNamedBuffer(buffer) != 0;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FlushMappedNamedBufferRange(uint buffer, nint offset, nint length) => Functions.glFlushMappedNamedBufferRange(buffer, offset, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedBufferParameter(uint buffer, GLGetBufferParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedBufferParameteriv(buffer, (uint)pname, (IntPtr)pValues); ;
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetNamedBufferParameter(uint buffer, GLGetBufferParameter pname, Span<long> values) {
			unsafe {
				fixed (long* pValues = values) {
					Functions.glGetNamedBufferParameteri64v(buffer, (uint)pname, (IntPtr)pValues); ;
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<IntPtr> GetNamedBufferPointer(uint buffer, GLGetBufferPointer pname, Span<IntPtr> values) {
			unsafe {
				fixed (IntPtr* pValues = values) {
					Functions.glGetNamedBufferPointerv(buffer, (uint)pname, (IntPtr)pValues); ;
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetNamedBufferSubData(uint buffer, nint offset, nint size, IntPtr data) => Functions.glGetNamedBufferSubData(buffer, offset, size, data);

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
					Functions.glCreateFramebuffers(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateFramebuffers(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateFramebuffers(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateFramebuffers() {
			uint id = 0;
			unsafe {
				Functions.glCreateFramebuffers(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferRenderbuffer(uint framebuffer, GLFramebufferAttachment attachment, GLRenderbufferTarget renderbufferTarget, uint renderbuffer) => Functions.glNamedFramebufferRenderbuffer(framebuffer, (uint)attachment, (uint)renderbufferTarget, renderbuffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferParameter(uint framebuffer, GLFramebufferParameter pname, int param) => Functions.glNamedFramebufferParameteri(framebuffer, (uint)pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferTexture(uint framebuffer, GLFramebufferAttachment attachment, uint texture, int level) => Functions.glNamedFramebufferTexture(framebuffer, (uint)attachment, texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferTextureLayer(uint framebuffer, GLFramebufferAttachment attachment, uint texture, int level, int layer) => Functions.glNamedFramebufferTextureLayer(framebuffer, (uint)attachment, texture, level, layer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffer(uint framebuffer, GLDrawBuffer mode) => Functions.glNamedFramebufferDrawBuffer(framebuffer, (uint)mode);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffers(uint framebuffer, in ReadOnlySpan<GLDrawBuffer> bufs) {
			unsafe {
				fixed (GLDrawBuffer* pBufs = bufs) {
					Functions.glNamedFramebufferDrawBuffers(framebuffer, bufs.Length, (IntPtr)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferDrawBuffers(uint framebuffer, params GLDrawBuffer[] bufs) {
			unsafe {
				fixed (GLDrawBuffer* pBufs = bufs) {
					Functions.glNamedFramebufferDrawBuffers(framebuffer, bufs.Length, (IntPtr)pBufs);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedFramebufferReadBuffer(uint framebuffer, GLDrawBuffer buf) => Functions.glNamedFramebufferReadBuffer(framebuffer, (uint)buf);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferData(uint framebuffer, in ReadOnlySpan<GLFramebufferAttachment> attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferData(framebuffer, attachments.Length, (IntPtr)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferData(uint framebuffer, params GLFramebufferAttachment[] attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferData(framebuffer, attachments.Length, (IntPtr)pAttachments);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferSubData(uint framebuffer, in ReadOnlySpan<GLFramebufferAttachment> attachments, Recti area) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferSubData(framebuffer, attachments.Length, (IntPtr)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateNamedFramebufferSubData(uint framebuffer, Recti area, params GLFramebufferAttachment[] attachments) {
			unsafe {
				fixed (GLFramebufferAttachment* pAttachments = attachments) {
					Functions.glInvalidateNamedFramebufferSubData(framebuffer, attachments.Length, (IntPtr)pAttachments, area.Position.X, area.Position.Y, area.Size.X, area.Size.Y);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<int> value) {
			unsafe {
				fixed (int* pValue = value) {
					Functions.glClearNamedFramebufferiv(framebuffer, (uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, Vector4i value) {
			unsafe {
				Functions.glClearNamedFramebufferiv(framebuffer, (uint)buffer, drawbuffer, (IntPtr)(&value));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<uint> value) {
			unsafe {
				fixed (uint* pValue = value) {
					Functions.glClearNamedFramebufferuiv(framebuffer, (uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, Vector4ui value) {
			unsafe {
				Functions.glClearNamedFramebufferuiv(framebuffer, (uint)buffer, drawbuffer, (IntPtr)(&value));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, in ReadOnlySpan<float> value) {
			unsafe {
				fixed (float* pValue = value) {
					Functions.glClearNamedFramebufferfv(framebuffer, (uint)buffer, drawbuffer, (IntPtr)pValue);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, Vector4 value) {
			unsafe {
				Functions.glClearNamedFramebufferfv(framebuffer, (uint)buffer, drawbuffer, (IntPtr)(&value));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearNamedFramebuffer(uint framebuffer, GLClearBuffer buffer, int drawbuffer, float depth, int stencil) => Functions.glClearNamedFramebufferfi(framebuffer, (uint)buffer, drawbuffer, depth, stencil);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BlitNamedFramebuffer(uint readFramebuffer, uint drawFramebuffer, Recti src, Recti dst, GLBufferMask mask, GLFilter filter) {
			Vector2i srcmin = src.Minimum, srcmax = src.Maximum;
			Vector2i dstmin = dst.Minimum, dstmax = dst.Maximum;
			Functions.glBlitNamedFramebuffer(readFramebuffer, drawFramebuffer, srcmin.X, srcmin.Y, srcmax.X, srcmax.Y, dstmin.X, dstmin.Y, dstmax.X, dstmax.Y, (uint)mask, (uint)filter);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public GLFramebufferStatus CheckNamedFramebufferStatus(uint framebuffer, GLFramebufferTarget target) => (GLFramebufferStatus)Functions.glCheckNamedFramebufferStatus(framebuffer, (uint)target);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedFramebufferParameter(uint framebuffer, GLFramebufferParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedFramebufferParameteriv(framebuffer, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedFramebufferAttachmentParameter(uint framebuffer, GLFramebufferAttachment attachment, GLGetFramebufferAttachment pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedFramebufferAttachmentParameteriv(framebuffer, (uint)attachment, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateRenderbuffers(Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateRenderbuffers(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateRenderbuffers(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateRenderbuffers(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateRenderbuffers() {
			uint id = 0;
			unsafe {
				Functions.glCreateRenderbuffers(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedRenderbufferStorage(uint renderbuffer, GLInternalFormat internalFormat, int width, int height) => Functions.glNamedRenderbufferStorage(renderbuffer, (uint)internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NamedRenderbufferStorageMultisample(uint renderbuffer, int samples, GLInternalFormat internalFormat, int width, int height) => Functions.glNamedRenderbufferStorageMultisample(renderbuffer, samples, (uint)internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetNamedRenderbufferParameter(uint renderbuffer, GLGetRenderbuffer pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetNamedRenderbufferParameteriv(renderbuffer, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateTextures(GLTextureTarget target, Span<uint> ids) {
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTextures((uint)target, ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateTextures(GLTextureTarget target, int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateTextures((uint)target, ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateTextures(GLTextureTarget target) {
			uint id = 0;
			unsafe {
				Functions.glCreateTextures((uint)target, 1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBuffer(uint texture, GLInternalFormat internalFormat, uint buffer) => Functions.glTextureBuffer(texture, (uint)internalFormat, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureBufferRange(uint texture, GLInternalFormat internalFormat, uint buffer, nint offset, nint size) => Functions.glTextureBufferRange(texture, (uint)internalFormat, buffer, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage1D(uint texture, int levels, GLInternalFormat internalFormat, int width) => Functions.glTextureStorage1D(texture, levels, (uint)internalFormat, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage2D(uint texture, int levels, GLInternalFormat internalFormat, int width, int height) => Functions.glTextureStorage2D(texture, levels, (uint)internalFormat, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage3D(uint texture, int levels, GLInternalFormat internalFormat, int width, int height, int depth) => Functions.glTextureStorage3D(texture, levels, (uint)internalFormat, width, height, depth);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage2DMultisample(uint texture, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => Functions.glTextureStorage2DMultisample(texture, samples, (uint)internalFormat, width, height, (byte)(fixedSampleLocations ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureStorage3DMultisample(uint texture, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => Functions.glTextureStorage3DMultisample(texture, samples, (uint)internalFormat, width, height, depth, (byte)(fixedSampleLocations ? 1 : 0));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage1D(uint texture, int level, int xoffset, int width, GLFormat format, GLTextureType type, IntPtr pixels) => Functions.glTextureSubImage1D(texture, level, xoffset, width, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage1D<T>(uint texture, int level, int xoffset, int width, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					Functions.glTextureSubImage1D(texture, level, xoffset, width, (uint)format, (uint)type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, IntPtr pixels) => Functions.glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					Functions.glTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, (uint)type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, IntPtr pixels) => Functions.glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, (uint)type, pixels);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureSubImage3D<T>(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLFormat format, GLTextureType type, in ReadOnlySpan<T> pixels) where T : unmanaged {
			unsafe {
				fixed (T* pPixels = pixels) {
					Functions.glTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, (uint)type, (IntPtr)pPixels);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage1D(uint texture, int level, int xoffset, int width, GLInternalFormat format, int imageSize, IntPtr data) => Functions.glCompressedTextureSubImage1D(texture, level, xoffset, width, (uint)format, imageSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage1D<T>(uint texture, int level, int xoffset, int width, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glCompressedTextureSubImage1D(texture, level, xoffset, width, (uint)format, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int width, int height, GLInternalFormat format, int imageSize, IntPtr data) => Functions.glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, imageSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glCompressedTextureSubImage2D(texture, level, xoffset, yoffset, width, height, (uint)format, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLInternalFormat format, int imageSize, IntPtr data) => Functions.glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, imageSize, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CompressedTextureSubImage3D<T>(uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, GLInternalFormat format, in ReadOnlySpan<T> data) where T : unmanaged {
			unsafe {
				fixed (T* pData = data) {
					Functions.glCompressedTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, width, height, depth, (uint)format, sizeof(T) * data.Length, (IntPtr)pData);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage1D(uint texture, int level, int xoffset, int x, int y, int width) => Functions.glCopyTextureSubImage1D(texture, level, xoffset, x, y, width);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage2D(uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height) => Functions.glCopyTextureSubImage2D(texture, level, xoffset, yoffset, x, y, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyTextureSubImage3D(uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height) => Functions.glCopyTextureSubImage3D(texture, level, xoffset, yoffset, zoffset, x, y, width, height);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, float param) => Functions.glTextureParameterf(texture, (uint)pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, in ReadOnlySpan<float> param) {
			unsafe {
				fixed (float* pParam = param) {
					Functions.glTextureParameterfv(texture, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, int param) => Functions.glTextureParameteri(texture, (uint)pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameter(uint texture, GLTexParamter pname, in ReadOnlySpan<int> param) {
			unsafe {
				fixed (int* pParam = param) {
					Functions.glTextureParameteriv(texture, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameterI(uint texture, GLTexParamter pname, in ReadOnlySpan<int> param) {
			unsafe {
				fixed (int* pParam = param) {
					Functions.glTextureParameterIiv(texture, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureParameterI(uint texture, GLTexParamter pname, in ReadOnlySpan<uint> param) {
			unsafe {
				fixed (uint* pParam = param) {
					Functions.glTextureParameterIuiv(texture, (uint)pname, (IntPtr)pParam);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GenerateTextureMipmap(uint texture) => Functions.glGenerateTextureMipmap(texture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindTextureUnit(uint unit, uint texture) => Functions.glBindTextureUnit(unit, texture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetTextureImage(uint texture, int level, GLFormat format, GLTextureType type, int bufSize, IntPtr pixels) => Functions.glGetTextureImage(texture, level, (uint)format, (uint)type, bufSize, pixels);

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
		public void GetCompressedTextureImage(uint texture, int level, int bufSize, IntPtr pixels) => Functions.glGetCompressedTextureImage(texture, level, bufSize, pixels);

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
					Functions.glGetTextureLevelParameterfv(texture, level, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureLevelParameter(uint texture, int level, GLGetTexLevelParameter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTextureLevelParameteriv(texture, level, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<float> GetTextureParameter(uint texture, GLTexParamter pname, Span<float> values) {
			unsafe {
				fixed(float* pValues = values) {
					Functions.glGetTextureParameterfv(texture, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureParameter(uint texture, GLTexParamter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTextureParameteriv(texture, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetTextureParameterI(uint texture, GLTexParamter pname, Span<int> values) {
			unsafe {
				fixed (int* pValues = values) {
					Functions.glGetTextureParameterIiv(texture, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> GetTextureParameterI(uint texture, GLTexParamter pname, Span<uint> values) {
			unsafe {
				fixed (uint* pValues = values) {
					Functions.glGetTextureParameterIuiv(texture, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateVertexArrays(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateVertexArrays(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateVertexArrays(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateVertexArrays(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateVertexArrays() {
			uint id = 0;
			unsafe {
				Functions.glCreateVertexArrays(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DisableVertexArrayAttrib(uint vaobj, uint index) => Functions.glDisableVertexArrayAttrib(vaobj, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnableVertexArrayAttrib(uint vaobj, uint index) => Functions.glEnableVertexArrayAttrib(vaobj, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayElementBuffer(uint vaobj, uint buffer) => Functions.glVertexArrayElementBuffer(vaobj, buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayVertexBuffer(uint vaobj, uint bindingIndex, uint buffer, nint offset, int stride) => Functions.glVertexArrayVertexBuffer(vaobj, bindingIndex, buffer, offset, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayVertexBuffers(uint vaobj, uint first, in ReadOnlySpan<uint> buffers, in ReadOnlySpan<nint> offsets, in ReadOnlySpan<int> strides) {
			int n = ExMath.Min(buffers.Length, offsets.Length, strides.Length);
			unsafe {
				fixed(uint* pBuffers = buffers) {
					fixed(nint* pOffsets = offsets) {
						fixed(int* pStrides = strides) {
							Functions.glVertexArrayVertexBuffers(vaobj, first, n, (IntPtr)pBuffers, (IntPtr)pOffsets, (IntPtr)pStrides);
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribFormat(uint vaobj, uint attribIndex, int size, GLType type, bool normalized, uint relativeOffset) => Functions.glVertexArrayAttribFormat(vaobj, attribIndex, size, (uint)type, (byte)(normalized ? 1 : 0), relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribIFormat(uint vaobj, uint attribIndex, int size, GLType type, uint relativeOffset) => Functions.glVertexArrayAttribIFormat(vaobj, attribIndex, size, (uint)type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribLFormat(uint vaobj, uint attribIndex, int size, GLType type, uint relativeOffset) => Functions.glVertexArrayAttribLFormat(vaobj, attribIndex, size, (uint)type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayAttribBinding(uint vaobj, uint attribIndex, uint bindingIndex) => Functions.glVertexArrayAttribBinding(vaobj, attribIndex, bindingIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexArrayBindingDivisor(uint vaobj, uint bindingIndex, uint divisor) => Functions.glVertexArrayBindingDivisor(vaobj, bindingIndex, divisor);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexArray(uint vaobj, GLGetVertexArray pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetVertexArrayiv(vaobj, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetVertexArrayIndexed(uint vaobj, uint index, GLGetVertexArrayIndexed pname, Span<int> values) {
			unsafe {
				fixed(int* pValues = values) {
					Functions.glGetVertexArrayIndexediv(vaobj, index, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetVertexArrayIndexed(uint vaobj, uint index, GLGetVertexArrayIndexed pname, Span<long> values) {
			unsafe {
				fixed (long* pValues = values) {
					Functions.glGetVertexArrayIndexed64iv(vaobj, index, (uint)pname, (IntPtr)pValues);
				}
			}
			return values;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateSamplers(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateSamplers(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateSamplers(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateSamplers(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateSamplers() {
			uint id = 0;
			unsafe {
				Functions.glCreateSamplers(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateProgramPipelines(Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateProgramPipelines(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateProgramPipelines(int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateProgramPipelines(ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateProgramPipelines() {
			uint id = 0;
			unsafe {
				Functions.glCreateProgramPipelines(1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<uint> CreateQueries(GLQueryTarget target, Span<uint> ids) {
			unsafe {
				fixed(uint* pIds = ids) {
					Functions.glCreateQueries((uint)target, ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint[] CreateQueries(GLQueryTarget target, int n) {
			uint[] ids = new uint[n];
			unsafe {
				fixed (uint* pIds = ids) {
					Functions.glCreateQueries((uint)target, ids.Length, (IntPtr)pIds);
				}
			}
			return ids;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint CreateQueries(GLQueryTarget target) {
			uint id = 0;
			unsafe {
				Functions.glCreateQueries((uint)target, 1, (IntPtr)(&id));
			}
			return id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectiv(uint id, uint buffer, GLGetQueryObject pname, nint offset) => Functions.glGetQueryBufferObjectiv(id, buffer, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectuiv(uint id, uint buffer, GLGetQueryObject pname, nint offset) => Functions.glGetQueryBufferObjectuiv(id, buffer, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjecti64v(uint id, uint buffer, GLGetQueryObject pname, nint offset) => Functions.glGetQueryBufferObjecti64v(id, buffer, (uint)pname, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetQueryBufferObjectui64v(uint id, uint buffer, GLGetQueryObject pname, nint offset) => Functions.glGetQueryBufferObjectui64v(id, buffer, (uint)pname, offset);

	}
}
