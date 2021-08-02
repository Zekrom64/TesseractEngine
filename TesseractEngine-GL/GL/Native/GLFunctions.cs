using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Native;

namespace Tesseract.GL.Native {

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

	public class GL11Functions {

		public delegate void PFN_glAccum(GLenum op, GLfloat value);
		public delegate void PFN_glAlphaFunc(GLenum func, GLclampf _ref);
		public delegate GLboolean PFN_glAreTexturesResident(GLsizei n, [NativeType("const GLuint*")] IntPtr textures, [NativeType("GLboolean*")] IntPtr residences);
		public delegate void PFN_glArrayElement(GLint i);
		public delegate void PFN_glBegin(GLenum mode);
		public delegate void PFN_glBindTexture(GLenum target, GLuint texture);
		public delegate void PFN_glBitmap(GLsizei width, GLsizei height, GLfloat xorig, GLfloat yorig, GLfloat xmove, GLfloat ymove, [NativeType("const GLubyte*")] IntPtr bitmap);
		public delegate void PFN_glBlendFunc(GLenum sfactor, GLenum dfactor);
		public delegate void PFN_glCallList(GLuint list);
		public delegate void PFN_glCallLists(GLsizei n, GLenum type, IntPtr lists);
		public delegate void PFN_glClear(GLbitfield mask);
		public delegate void PFN_glClearAccum(GLfloat red, GLfloat green, GLfloat blue, GLfloat alpha);
		public delegate void PFN_glClearColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha);
		public delegate void PFN_glClearDepth(GLclampd depth);
		public delegate void PFN_glClearIndex(GLfloat c);
		public delegate void PFN_glClearStencil(GLint s);
		public delegate void PFN_glClipPlane(GLenum plane, [NativeType("const GLdouble*")] IntPtr equation);
		// glColor3/4*
		/*
		public delegate void PFN_glColor3b(GLbyte red, GLbyte green, GLbyte blue);
		public delegate void PFN_glColor3bv([NativeType("const GLbyte*")] IntPtr v);
		public delegate void PFN_glColor3d(GLdouble red, GLdouble green, GLdouble blue);
		public delegate void PFN_glColor3dv([NativeType("const GLdouble*")] IntPtr v);
		public delegate void PFN_glColor3f(GLfloat red, GLfloat green, GLfloat blue);
		public delegate void PFN_glColor3fv([NativeType("const GLfloat*")] IntPtr v);
		public delegate void PFN_glColor3i(GLint red, GLint green, GLint blue);
		public delegate void PFN_glColor3iv([NativeType("const GLint*")] IntPtr v);
		*/
		public delegate void PFN_glColorMask(GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha);
		public delegate void PFN_glColorMaterial(GLenum face, GLenum mode);
		public delegate void PFN_glColorPointer(GLint size, GLenum type, GLsizei stride, IntPtr pointer);
		public delegate void PFN_glCopyPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum type);
		public delegate void PFN_glCopyTexImage1D(GLenum target, GLint level, GLenum internalFormat, GLint x, GLint y, GLsizei width, GLsizei border);
		public delegate void PFN_glCopyTexImage2D(GLenum target, GLint level, GLenum internalFormat, GLint x, GLint y, GLsizei width, GLsizei height, GLsizei border);
		public delegate void PFN_glCopyTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLint x, GLint y, GLsizei width);
		public delegate void PFN_glCopyTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height);
		public delegate void PFN_glCullFace(GLenum mode);
		public delegate void PFN_glDeleteLists(GLuint list, GLsizei range);
		public delegate void PFN_glDeleteTextures(GLsizei n, [NativeType("const GLuint*")] IntPtr textures);
		public delegate void PFN_glDepthFunc(GLenum func);
		public delegate void PFN_glDepthMask(GLboolean flag);
		public delegate void PFN_glDepthRange(GLclampd zNear, GLclampd zFar);
		public delegate void PFN_glDisable(GLenum cap);
		public delegate void PFN_glDisableClientState(GLenum array);
		public delegate void PFN_glDrawArrays(GLenum mode, GLint first, GLsizei count);
		public delegate void PFN_glDrawBuffer(GLenum mode);
		public delegate void PFN_glDrawElements(GLenum mode, GLsizei count, GLenum type, IntPtr indices);
		public delegate void PFN_glDrawPixels(GLsizei width, GLsizei height, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glEdgeFlag(GLboolean flag);
		public delegate void PFN_glEdgeFlagPointer(GLsizei stride, IntPtr pointer);
		public delegate void PFN_glEnable(GLenum cap);
		public delegate void PFN_glEnableClientState(GLenum array);
		public delegate void PFN_glEnd();
		public delegate void PFN_glEndList();
		// glEvalCoord1/2*
		public delegate void PFN_glEvalMesh1(GLenum mode, GLint i1, GLint i2);
		public delegate void PFN_glEvalMesh2(GLenum mode, GLint i1, GLint i2, GLint j1, GLint j2);
		public delegate void PFN_glEvalPoint1(GLint i);
		public delegate void PFN_glEvalPoint2(GLint i, GLint j);
		public delegate void PFN_glFeedbackBuffer(GLsizei size, GLenum type, [NativeType("GLfloat*")] IntPtr buffer);
		public delegate void PFN_glFinish();
		public delegate void PFN_glFlush();
		public delegate void PFN_glFogf(GLenum pname, GLfloat param);
		public delegate void PFN_glFogfv(GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glFogi(GLenum pname, GLint param);
		public delegate void PFN_glFogiv(GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glFrontFace(GLenum mode);
		public delegate void PFN_glFrustum(GLdouble left, GLdouble right, GLdouble bottom, GLdouble top, GLdouble zNear, GLdouble zFar);
		public delegate GLuint PFN_glGenLists(GLsizei range);
		public delegate void PFN_glGenTextures(GLsizei n, [NativeType("GLuint*")] IntPtr textures);
		public delegate void PFN_glGetBooleanv(GLenum pname, [NativeType("GLboolean*")] IntPtr _params);
		public delegate void PFN_glGetClipPlane(GLenum plane, [NativeType("GLdouble*")] IntPtr equation);
		public delegate void PFN_glGetDoublev(GLenum pname, [NativeType("GLdouble*")] IntPtr _params);
		public delegate GLenum PFN_glGetError();
		public delegate void PFN_glGetFloatv(GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetIntegerv(GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetLightfv(GLenum light, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetLightiv(GLenum light, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetMapdv(GLenum target, GLenum query, [NativeType("GLdouble*")] IntPtr v);
		public delegate void PFN_glGetMapfv(GLenum target, GLenum query, [NativeType("GLfloat*")] IntPtr v);
		public delegate void PFN_glGetMapiv(GLenum target, GLenum query, [NativeType("GLint*")] IntPtr v);
		public delegate void PFN_glGetMaterialfv(GLenum face, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetMaterialiv(GLenum face, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetPixelMapfv(GLenum map, [NativeType("GLfloat*")] IntPtr values);
		public delegate void PFN_glGetPixelMapuiv(GLenum map, [NativeType("GLuint*")] IntPtr values);
		public delegate void PFN_glGetPixelMapusv(GLenum map, [NativeType("GLushort*")] IntPtr values);
		public delegate void PFN_glGetPointerv(GLenum pname, [NativeType("void**")] IntPtr _params);
		public delegate void PFN_glGetPolygonStipple([NativeType("GLubyte*")] IntPtr mask);
		[return: NativeType("const GLubyte*")]
		public delegate IntPtr PFN_glGetString(GLenum name);
		public delegate void PFN_glGetTexEnvfv(GLenum target, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetTexEnviv(GLenum target, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetTexGendv(GLenum coord, GLenum pname, [NativeType("GLdouble*")] IntPtr _params);
		public delegate void PFN_glGetTexGenfv(GLenum coord, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetTexGeniv(GLenum coord, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetTexImage(GLenum target, GLint level, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glGetTexLevelParameterfv(GLenum target, GLint level, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetTexLevelParameteriv(GLenum target, GLint level, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetTexParameterf(GLenum target, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetTexParameteri(GLenum target, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glHint(GLenum target, GLenum mode);
		public delegate void PFN_glIndexMask(GLuint mask);
		public delegate void PFN_glIndexPointer(GLenum type, GLsizei stride, IntPtr pointer);
		// glIndex*
		public delegate void PFN_glInitNames();
		public delegate void PFN_glInterleavedArrays(GLenum format, GLsizei stride, IntPtr pointer);
		public delegate GLboolean PFN_glIsEnabled(GLenum cap);
		public delegate GLboolean PFN_glIsList(GLuint list);
		public delegate GLboolean PFN_glIsTexture(GLuint texture);
		public delegate void PFN_glLightModelf(GLenum pname, GLfloat param);
		public delegate void PFN_glLightModelfv(GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glLightModeli(GLenum pname, GLint param);
		public delegate void PFN_glLightModeliv(GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glLightf(GLenum light, GLenum pname, GLfloat param);
		public delegate void PFN_glLightfv(GLenum light, GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glLighti(GLenum light, GLenum pname, GLint param);
		public delegate void PFN_glLightiv(GLenum light, GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glLineStipple(GLint factor, GLushort pattern);
		public delegate void PFN_glLineWidth(GLfloat width);
		public delegate void PFN_glListBase(GLuint _base);
		public delegate void PFN_glLoadIdentity();
		public delegate void PFN_glLoadMatrixd([NativeType("const GLdouble*")] IntPtr m);
		public delegate void PFN_glLoadMatrixf([NativeType("const GLfloat*")] IntPtr m);
		public delegate void PFN_glLoadName(GLuint name);
		public delegate void PFN_glLogicOp(GLenum opcode);
		// glMap1/2*
		// glMapGrid1/2
		public delegate void PFN_glMaterialf(GLenum face, GLenum pname, GLfloat param);
		public delegate void PFN_glMaterialfv(GLenum face, GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glMateriali(GLenum face, GLenum pname, GLint param);
		public delegate void PFN_glMaterialiv(GLenum face, GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glMatrixMode(GLenum mode);
		public delegate void PFN_glMultMatrixd([NativeType("const GLdouble*")] IntPtr m);
		public delegate void PFN_glMultMatrixf([NativeType("const GLfloat*")] IntPtr m);
		public delegate void PFN_glNewList(GLuint list, GLenum mode);
		// glNormal3*
		public delegate void PFN_glNormalPointer(GLenum type, GLsizei stride, IntPtr pointer);
		public delegate void PFN_glOrtho(GLdouble left, GLdouble right, GLdouble bottom, GLdouble top, GLdouble zNear, GLdouble zFar);
		public delegate void PFN_glPassThrough(GLfloat token);
		public delegate void PFN_glPixelMapfv(GLenum map, GLsizei mapsize, [NativeType("GLfloat*")] IntPtr values);
		public delegate void PFN_glPixelMapuiv(GLenum map, GLsizei mapsize, [NativeType("GLuint*")] IntPtr values);
		public delegate void PFN_glPixelMapusv(GLenum map, GLsizei mapsize, [NativeType("GLushort*")] IntPtr values);
		public delegate void PFN_glPixelStoref(GLenum pname, GLfloat param);
		public delegate void PFN_glPixelStorei(GLenum pname, GLint param);
		public delegate void PFN_glPixelTransferf(GLenum pname, GLfloat param);
		public delegate void PFN_glPixelTransferi(GLenum pname, GLint param);
		public delegate void PFN_glPixelZoom(GLfloat xfactor, GLfloat yfactor);
		public delegate void PFN_glPointSize(GLfloat size);
		public delegate void PFN_glPolygonMode(GLenum face, GLenum mode);
		public delegate void PFN_glPolygonOffset(GLfloat factor, GLfloat units);
		public delegate void PFN_glPolygonStipple([NativeType("const GLubyte*")] IntPtr mask);
		public delegate void PFN_glPopAttrib();
		public delegate void PFN_glPopClientAttrib();
		public delegate void PFN_glPopMatrix();
		public delegate void PFN_glPopName();
		public delegate void PFN_glPrioritizeTextures(GLsizei n, [NativeType("const GLuint*")] IntPtr textures, [NativeType("const GLclampf*")] IntPtr priorities);
		public delegate void PFN_glPushAttrib(GLbitfield mask);
		public delegate void PFN_glPushClientAttrib(GLbitfield mask);
		public delegate void PFN_glPushMatrix();
		public delegate void PFN_glPushName(GLuint name);
		// glRasterPos2/3/4*
		public delegate void PFN_glReadBuffer(GLenum mode);
		public delegate void PFN_glReadPixels(GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glRectd(GLdouble x1, GLdouble y1, GLdouble x2, GLdouble y2);
		public delegate void PFN_glRectf(GLfloat x1, GLfloat y1, GLfloat x2, GLfloat y2);
		public delegate void PFN_glRecti(GLint x1, GLint y1, GLint x2, GLint y2);
		public delegate void PFN_glRects(GLshort x1, GLshort y1, GLshort x2, GLshort y2);
		public delegate GLint PFN_glRenderMode(GLenum mode);
		public delegate void PFN_glRotated(GLdouble angle, GLdouble x, GLdouble y, GLdouble z);
		public delegate void PFN_glRotatef(GLfloat angle, GLfloat x, GLfloat y, GLfloat z);
		public delegate void PFN_glScaled(GLdouble x, GLdouble y, GLdouble z);
		public delegate void PFN_glScalef(GLfloat x, GLfloat y, GLfloat z);
		public delegate void PFN_glScissor(GLint x, GLint y, GLsizei width, GLsizei height);
		public delegate void PFN_glSelectBuffer(GLsizei size, [NativeType("GLuint*")] IntPtr buffer);
		public delegate void PFN_glShadeModel(GLenum mode);
		public delegate void PFN_glStencilFunc(GLenum func, GLint _ref, GLuint mask);
		public delegate void PFN_glStencilMask(GLuint mask);
		public delegate void PFN_glStencilOp(GLenum fail, GLenum zfail, GLenum zpass);
		// glTexCoord1/2/3/4*
		public delegate void PFN_glTexCoordPointer(GLint size, GLenum type, GLsizei stride, IntPtr pointer);
		public delegate void PFN_glTexEnvf(GLenum target, GLenum pname, GLfloat param);
		public delegate void PFN_glTexEnvfv(GLenum target, GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glTexEnvi(GLenum target, GLenum pname, GLint param);
		public delegate void PFN_glTexEnviv(GLenum target, GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glTexGend(GLenum target, GLenum pname, GLdouble param);
		public delegate void PFN_glTexGendv(GLenum target, GLenum pname, [NativeType("const GLdouble*")] IntPtr _params);
		public delegate void PFN_glTexGenf(GLenum target, GLenum pname, GLfloat param);
		public delegate void PFN_glTexGenfv(GLenum target, GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glTexGeni(GLenum target, GLenum pname, GLint param);
		public delegate void PFN_glTexGeniv(GLenum target, GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glTexImage1D(GLenum target, GLint level, GLint internalFormat, GLsizei width, GLint border, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glTexImage2D(GLenum target, GLint level, GLint internalFormat, GLsizei width, GLsizei height, GLint border, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glTexParameterf(GLenum target, GLenum pname, GLfloat param);
		public delegate void PFN_glTexParameterfv(GLenum target, GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glTexParameteri(GLenum target, GLenum pname, GLint param);
		public delegate void PFN_glTexParameteriv(GLenum target, GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		public delegate void PFN_glTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glTranslated(GLdouble x, GLdouble y, GLdouble z);
		public delegate void PFN_glTranslatef(GLfloat x, GLfloat y, GLfloat z);
		// glVertex2/3/4*
		public delegate void PFN_glVertexPointer(GLint size, GLenum type, GLsizei stride, IntPtr pointer);
		public delegate void PFN_glViewport(GLint x, GLint y, GLsizei width, GLsizei height);

		public PFN_glAccum glAccum;
		public PFN_glAlphaFunc glAlphaFunc;
		public PFN_glAreTexturesResident glAreTexturesResident;
		public PFN_glArrayElement glArrayElement;
		public PFN_glBegin glBegin;
		public PFN_glBindTexture glBindTexture;
		public PFN_glBitmap glBitmap;
		public PFN_glBlendFunc glBlendFunc;
		public PFN_glCallList glCallList;
		public PFN_glCallLists glCallLists;
		public PFN_glClear glClear;
		public PFN_glClearAccum glClearAccum;
		public PFN_glClearColor glClearColor;
		public PFN_glClearDepth glClearDepth;
		public PFN_glClearIndex glClearIndex;
		public PFN_glClearStencil glClearStencil;
		public PFN_glClipPlane glClipPlane;
		public PFN_glColorMask glColorMask;
		public PFN_glColorMaterial glColorMaterial;
		public PFN_glColorPointer glColorPointer;
		public PFN_glCopyPixels glCopyPixels;
		public PFN_glCopyTexImage1D glCopyTexImage1D;
		public PFN_glCopyTexImage2D glCopyTexImage2D;
		public PFN_glCopyTexSubImage1D glCopyTexSubImage1D;
		public PFN_glCopyTexSubImage2D glCopyTexSubImage2D;
		public PFN_glCullFace glCullFace;
		public PFN_glDeleteLists glDeleteLists;
		public PFN_glDeleteTextures glDeleteTextures;
		public PFN_glDepthFunc glDepthFunc;
		public PFN_glDepthMask glDepthMask;
		public PFN_glDepthRange glDepthRange;
		public PFN_glDisable glDisable;
		public PFN_glDisableClientState glDisableClientState;
		public PFN_glDrawArrays glDrawArrays;
		public PFN_glDrawBuffer glDrawBuffer;
		public PFN_glDrawElements glDrawElements;
		public PFN_glDrawPixels glDrawPixels;
		public PFN_glEdgeFlag glEdgeFlag;
		public PFN_glEdgeFlagPointer glEdgeFlagPointer;
		public PFN_glEnable glEnable;
		public PFN_glEnableClientState glEnableClientState;
		public PFN_glEnd glEnd;
		public PFN_glEndList glEndList;
		public PFN_glEvalMesh1 glEvalMesh1;
		public PFN_glEvalMesh2 glEvalMesh2;
		public PFN_glEvalPoint1 glEvalPoint1;
		public PFN_glEvalPoint2 glEvalPoint2;
		public PFN_glFeedbackBuffer glFeedbackBuffer;
		public PFN_glFinish glFinish;
		public PFN_glFlush glFlush;
		public PFN_glFogf glFogf;
		public PFN_glFogfv glFogfv;
		public PFN_glFogi glFogi;
		public PFN_glFogiv glFogiv;
		public PFN_glFrontFace glFrontFace;
		public PFN_glFrustum glFrustum;
		public PFN_glGenLists glGenLists;
		public PFN_glGenTextures glGenTextures;
		public PFN_glGetBooleanv glGetBooleanv;
		public PFN_glGetClipPlane glGetClipPlane;
		public PFN_glGetDoublev glGetDoublev;
		public PFN_glGetError glGetError;
		public PFN_glGetFloatv glGetFloatv;
		public PFN_glGetIntegerv glGetIntegerv;
		public PFN_glGetLightfv glGetLightfv;
		public PFN_glGetLightiv glGetLightiv;
		public PFN_glGetMapdv glGetMapdv;
		public PFN_glGetMapfv glGetMapfv;
		public PFN_glGetMapiv glGetMapiv;
		public PFN_glGetMaterialfv glGetMaterialfv;
		public PFN_glGetMaterialiv glGetMaterialiv;
		public PFN_glGetPixelMapfv glGetPixelMapfv;
		public PFN_glGetPixelMapuiv glGetPixelMapuiv;
		public PFN_glGetPixelMapusv glGetPixelMapusv;
		public PFN_glGetPointerv glGetPointerv;
		public PFN_glGetPolygonStipple glGetPolygonStipple;
		public PFN_glGetString glGetString;
		public PFN_glGetTexEnvfv glGetTexEnvfv;
		public PFN_glGetTexEnviv glGetTexEnviv;
		public PFN_glGetTexGendv glGetTexGendv;
		public PFN_glGetTexGenfv glGetTexGenfv;
		public PFN_glGetTexGeniv glGetTexGeniv;
		public PFN_glGetTexImage glGetTexImage;
		public PFN_glGetTexLevelParameterfv glGetTexLevelParameterfv;
		public PFN_glGetTexLevelParameteriv glGetTexLevelParameteriv;
		public PFN_glGetTexParameterf glGetTexParameterf;
		public PFN_glGetTexParameteri glGetTexParameteri;
		public PFN_glHint glHint;
		public PFN_glIndexMask glIndexMask;
		public PFN_glIndexPointer glIndexPointer;
		public PFN_glInitNames glInitNames;
		public PFN_glInterleavedArrays glInterleavedArrays;
		public PFN_glIsEnabled glIsEnabled;
		public PFN_glIsList glIsList;
		public PFN_glIsTexture glIsTexture;
		public PFN_glLightModelf glLightModelf;
		public PFN_glLightModelfv glLightModelfv;
		public PFN_glLightModeli glLightModeli;
		public PFN_glLightModeliv glLightModeliv;
		public PFN_glLightf glLightf;
		public PFN_glLightfv glLightfv;
		public PFN_glLighti glLighti;
		public PFN_glLightiv glLightiv;
		public PFN_glLineStipple glLineStipple;
		public PFN_glLineWidth glLineWidth;
		public PFN_glListBase glListBase;
		public PFN_glLoadIdentity glLoadIdentity;
		public PFN_glLoadMatrixd glLoadMatrixd;
		public PFN_glLoadMatrixf glLoadMatrixf;
		public PFN_glLoadName glLoadName;
		public PFN_glLogicOp glLogicOp;
		public PFN_glMaterialf glMaterialf;
		public PFN_glMaterialfv glMaterialfv;
		public PFN_glMateriali glMateriali;
		public PFN_glMaterialiv glMaterialiv;
		public PFN_glMatrixMode glMatrixMode;
		public PFN_glMultMatrixd glMultMatrixd;
		public PFN_glMultMatrixf glMultMatrixf;
		public PFN_glNewList glNewList;
		public PFN_glNormalPointer glNormalPointer;
		public PFN_glOrtho glOrtho;
		public PFN_glPassThrough glPassThrough;
		public PFN_glPixelMapfv glPixelMapfv;
		public PFN_glPixelMapuiv glPixelMapuiv;
		public PFN_glPixelMapusv glPixelMapusv;
		public PFN_glPixelStoref glPixelStoref;
		public PFN_glPixelStorei glPixelStorei;
		public PFN_glPixelTransferf glPixelTransferf;
		public PFN_glPixelTransferi glPixelTransferi;
		public PFN_glPixelZoom glPixelZoom;
		public PFN_glPointSize glPointSize;
		public PFN_glPolygonMode glPolygonMode;
		public PFN_glPolygonOffset glPolygonOffset;
		public PFN_glPolygonStipple glPolygonStipple;
		public PFN_glPopAttrib glPopAttrib;
		public PFN_glPopClientAttrib glPopClientAttrib;
		public PFN_glPopMatrix glPopMatrix;
		public PFN_glPopName glPopName;
		public PFN_glPrioritizeTextures glPrioritizeTextures;
		public PFN_glPushAttrib glPushAttrib;
		public PFN_glPushClientAttrib glPushClientAttrib;
		public PFN_glPushMatrix glPushMatrix;
		public PFN_glPushName glPushName;
		public PFN_glReadBuffer glReadBuffer;
		public PFN_glReadPixels glReadPixels;
		public PFN_glRectd glRectd;
		public PFN_glRectf glRectf;
		public PFN_glRecti glRecti;
		public PFN_glRects glRects;
		public PFN_glRenderMode glRenderMode;
		public PFN_glRotated glRotated;
		public PFN_glRotatef glRotatef;
		public PFN_glScaled glScaled;
		public PFN_glScalef glScalef;
		public PFN_glScissor glScissor;
		public PFN_glSelectBuffer glSelectBuffer;
		public PFN_glShadeModel glShadeModel;
		public PFN_glStencilFunc glStencilFunc;
		public PFN_glStencilMask glStencilMask;
		public PFN_glStencilOp glStencilOp;
		public PFN_glTexCoordPointer glTexCoordPointer;
		public PFN_glTexEnvf glTexEnvf;
		public PFN_glTexEnvfv glTexEnvfv;
		public PFN_glTexEnvi glTexEnvi;
		public PFN_glTexEnviv glTexEnviv;
		public PFN_glTexGend glTexGend;
		public PFN_glTexGendv glTexGendv;
		public PFN_glTexGenf glTexGenf;
		public PFN_glTexGenfv glTexGenfv;
		public PFN_glTexGeni glTexGeni;
		public PFN_glTexGeniv glTexGeniv;
		public PFN_glTexImage1D glTexImage1D;
		public PFN_glTexImage2D glTexImage2D;
		public PFN_glTexParameterf glTexParameterf;
		public PFN_glTexParameterfv glTexParameterfv;
		public PFN_glTexParameteri glTexParameteri;
		public PFN_glTexParameteriv glTexParameteriv;
		public PFN_glTexSubImage1D glTexSubImage1D;
		public PFN_glTexSubImage2D glTexSubImage2D;
		public PFN_glTranslated glTranslated;
		public PFN_glTranslatef glTranslatef;
		public PFN_glVertexPointer glVertexPointer;
		public PFN_glViewport glViewport;

	}

	public class GL12Functions {

		public delegate void PFN_glCopyTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLint x, GLint y, GLsizei width, GLsizei height);
		public delegate void PFN_glDrawRangeElements(GLenum mode, GLuint start, GLuint end, GLsizei count, GLenum type, IntPtr indices);
		public delegate void PFN_glTexImage3D(GLenum target, GLint level, GLint internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLint border, GLenum format, GLenum type, IntPtr pixels);
		public delegate void PFN_glTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLenum type, IntPtr pixels);

		public PFN_glCopyTexSubImage3D glCopyTexSubImage3D;
		public PFN_glDrawRangeElements glDrawRangeElements;
		public PFN_glTexImage3D glTexImage3D;
		public PFN_glTexSubImage3D glTexSubImage3D;

	}

	public class GL13Functions {

		public delegate void PFN_glActiveTexture(GLenum texture);
		public delegate void PFN_glClientActiveTexture(GLenum texture);
		public delegate void PFN_glCompressedTexImage1D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLint border, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexImage2D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLint border, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexImage3D(GLenum target, GLint level, GLenum internalFormat, GLsizei width, GLsizei height, GLsizei depth, GLint border, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexSubImage1D(GLenum target, GLint level, GLint xoffset, GLsizei width, GLenum format, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexSubImage2D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glCompressedTexSubImage3D(GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint zoffset, GLsizei width, GLsizei height, GLsizei depth, GLenum format, GLsizei imageSize, IntPtr data);
		public delegate void PFN_glGetCompressedTexImage(GLenum target, GLint lod, IntPtr img);
		public delegate void PFN_glLoadTransposeMatrixd([NativeType("const GLdouble[16]")] IntPtr m);
		public delegate void PFN_glLoadTransposeMatrixf([NativeType("const GLfloat[16]")] IntPtr m);
		public delegate void PFN_glMultTransposeMatrixd([NativeType("const GLdouble[16]")] IntPtr m);
		public delegate void PFN_glMultTransposeMatrixf([NativeType("const GLfloat[16]")] IntPtr m);
		// glMultiTexCoord1/2/3/4*
		public delegate void PFN_glSampleCoverage(GLclampf value, GLboolean invert);

		public PFN_glActiveTexture glActiveTexture;
		public PFN_glClientActiveTexture glClientActiveTexture;
		public PFN_glCompressedTexImage1D glCompressedTexImage1D;
		public PFN_glCompressedTexImage2D glCompressedTexImage2D;
		public PFN_glCompressedTexImage3D glCompressedTexImage3D;
		public PFN_glCompressedTexSubImage1D glCompressedTexSubImage1D;
		public PFN_glCompressedTexSubImage2D glCompressedTexSubImage2D;
		public PFN_glCompressedTexSubImage3D glCompressedTexSubImage3D;
		public PFN_glGetCompressedTexImage glGetCompressedTexImage;
		public PFN_glLoadTransposeMatrixd glLoadTransposeMatrixd;
		public PFN_glLoadTransposeMatrixf glLoadTransposeMatrixf;
		public PFN_glMultTransposeMatrixd glMultTransposeMatrixd;
		public PFN_glMultTransposeMatrixf glMultTransposeMatrixf;
		public PFN_glSampleCoverage glSampleCoverage;

	}

	public class GL14Functions {

		public delegate void PFN_glBlendColor(GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha);
		public delegate void PFN_glBlendEquation(GLenum mode);
		public delegate void PFN_glBlendFuncSeparate(GLenum sfactorRGB, GLenum dfactorRGB, GLenum sfactorAlpha, GLenum dfactorAlpha);
		public delegate void PFN_glFogCoordPointer(GLenum type, GLsizei stride, IntPtr pointer);
		public delegate void PFN_glFogCoordd(GLdouble coord);
		public delegate void PFN_glFogCoordf(GLfloat coord);
		public delegate void PFN_glMultiDrawArrays(GLenum mode, [NativeType("const GLint*")] IntPtr first, [NativeType("const GLint*")] IntPtr count, GLsizei drawcount);
		public delegate void PFN_glMultiDrawElements(GLenum mode, [NativeType("const GLsizei*")] IntPtr count, GLenum type, [NativeType("const void* const*")] IntPtr indices, GLsizei drawcount);
		public delegate void PFN_glPointParameterf(GLenum pname, GLfloat param);
		public delegate void PFN_glPointParameterfv(GLenum pname, [NativeType("const GLfloat*")] IntPtr _params);
		public delegate void PFN_glPointParameteri(GLenum pname, GLint param);
		public delegate void PFN_glPointParameteriv(GLenum pname, [NativeType("const GLint*")] IntPtr _params);
		// glSecondaryColor3*
		public delegate void PFN_glSecondaryColorPointer(GLint size, GLenum type, GLsizei stride, IntPtr pointer);
		// glWindowPos2/3*

		public PFN_glBlendColor glBlendColor;
		public PFN_glBlendEquation glBlendEquation;
		public PFN_glBlendFuncSeparate glBlendFuncSeparate;
		public PFN_glFogCoordPointer glFogCoordPointer;
		public PFN_glFogCoordd glFogCoordd;
		public PFN_glFogCoordf glFogCoordf;
		public PFN_glMultiDrawArrays glMultiDrawArrays;
		public PFN_glMultiDrawElements glMultiDrawElements;
		public PFN_glPointParameterf glPointParameterf;
		public PFN_glPointParameterfv glPointParameterfv;
		public PFN_glPointParameteri glPointParameteri;
		public PFN_glPointParameteriv glPointParameteriv;
		public PFN_glSecondaryColorPointer glSecondaryColorPointer;

	}

	public class GL15Functions {

		public delegate void PFN_glBeginQuery(GLenum target, GLuint id);
		public delegate void PFN_glBindBuffer(GLenum target, GLuint buffer);
		public delegate void PFN_glBufferData(GLenum target, GLsizeiptr size, IntPtr data, GLenum usage);
		public delegate void PFN_glBufferSubData(GLenum target, GLintptr offset, GLsizeiptr size, IntPtr data);
		public delegate void PFN_glDeleteBuffers(GLsizei n, [NativeType("const GLuint*")] IntPtr buffers);
		public delegate void PFN_glDeleteQueries(GLsizei n, [NativeType("const GLuint*")] IntPtr ids);
		public delegate void PFN_glEndQuery(GLenum target);
		public delegate void PFN_glGenBuffers(GLsizei n, [NativeType("GLuint*")] IntPtr buffers);
		public delegate void PFN_glGenQueries(GLsizei n, [NativeType("GLuint*")] IntPtr ids);
		public delegate void PFN_glGetBufferParamteriv(GLenum target, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetBufferPointerv(GLenum target, GLenum pname, [NativeType("void**")] IntPtr _params);
		public delegate void PFN_glGetBufferSubData(GLenum target, GLintptr offset, GLsizeiptr size, IntPtr data);
		public delegate void PFN_glGetQueryObjectiv(GLuint id, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetQueryObjectuiv(GLuint id, GLenum pname, [NativeType("GLuint*")] IntPtr _params);
		public delegate void PFN_glGetQueryiv(GLenum target, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate GLboolean PFN_glIsBuffer(GLuint buffer);
		public delegate GLboolean PFN_glIsQuery(GLuint id);
		public delegate IntPtr PFN_glMapBuffer(GLenum target, GLenum access);
		public delegate GLboolean PFN_glUnmapBuffer(GLenum target);

		public PFN_glBeginQuery glBeginQuery;
		public PFN_glBindBuffer glBindBuffer;
		public PFN_glBufferData glBufferData;
		public PFN_glBufferSubData glBufferSubData;
		public PFN_glDeleteBuffers glDeleteBuffers;
		public PFN_glDeleteQueries glDeleteQueries;
		public PFN_glEndQuery glEndQuery;
		public PFN_glGenBuffers glGenBuffers;
		public PFN_glGenQueries glGenQueries;
		public PFN_glGetBufferParamteriv glGetBufferParamteriv;
		public PFN_glGetBufferPointerv glGetBufferPointerv;
		public PFN_glGetBufferSubData glGetBufferSubData;
		public PFN_glGetQueryObjectiv glGetQueryObjectiv;
		public PFN_glGetQueryObjectuiv glGetQueryObjectuiv;
		public PFN_glGetQueryiv glGetQueryiv;
		public PFN_glIsBuffer glIsBuffer;
		public PFN_glIsQuery glIsQuery;
		public PFN_glMapBuffer glMapBuffer;
		public PFN_glUnmapBuffer glUnmapBuffer;

	}

	public class GL20Functions {

		public delegate void PFN_glAttachShader(GLuint program, GLuint shader);
		public delegate void PFN_glBindAttribLocation(GLuint program, GLuint index, [MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate void PFN_glBlendEquationSeparate(GLenum modeRGB, GLenum modeAlpha);
		public delegate void PFN_glCompileShader(GLuint shader);
		public delegate GLuint PFN_glCreateProgram();
		public delegate GLuint PFN_glCreateShader(GLenum type);
		public delegate void PFN_glDeleteProgram(GLuint program);
		public delegate void PFN_glDeleteShader(GLuint shader);
		public delegate void PFN_glDetachShader(GLuint program, GLuint shader);
		public delegate void PFN_glDisableVertexAttribArray(GLuint index);
		public delegate void PFN_glDrawBuffers(GLsizei n, [NativeType("const GLenum*")] IntPtr bufs);
		public delegate void PFN_glEnableVertexAttribArray(GLuint index);
		public delegate void PFN_glGetActiveAttrib(GLuint program, GLuint index, GLsizei maxLength, out GLsizei length, out GLint size, out GLenum type, [NativeType("GLchar*")] IntPtr name);
		public delegate void PFN_glGetActiveUniform(GLuint program, GLuint index, GLsizei maxLength, out GLsizei length, out GLint size, out GLenum type, [NativeType("GLchar*")] IntPtr name);
		public delegate void PFN_glGetAttachedShaders(GLuint program, GLsizei maxCount, out GLsizei count, [NativeType("GLuint*")] IntPtr shaders);
		public delegate GLint PFN_glGetAttribLocation(GLuint program, [MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate void PFN_glGetProgramInfoLog(GLuint program, GLsizei bufSize, out GLsizei length, [NativeType("GLchar*")] IntPtr infoLog);
		public delegate void PFN_glGetProgramiv(GLuint program, GLenum pname, out GLint param);
		public delegate void PFN_glGetShaderInfoLog(GLuint shader, GLsizei bufSize, out GLsizei length, [NativeType("GLchar*")] IntPtr infoLog);
		public delegate void PFN_glGetShaderSource(GLuint obj, GLsizei maxLength, out GLsizei length, [NativeType("GLchar*")] IntPtr source);
		public delegate void PFN_glGetShaderiv(GLuint shader, GLenum pname, out GLint param);
		public delegate GLint PFN_glGetUniformLocation(GLuint program, [MarshalAs(UnmanagedType.LPStr)] string name);
		public delegate void PFN_glGetUniformfv(GLuint program, GLint location, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetUniformiv(GLuint program, GLint location, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glGetVertexAttribPointerv(GLuint index, GLenum name, out IntPtr pointer);
		public delegate void PFN_glGetVertexAttribdv(GLuint index, GLenum pname, [NativeType("GLdouble*")] IntPtr _params);
		public delegate void PFN_glGetVertexAttribfv(GLuint index, GLenum pname, [NativeType("GLfloat*")] IntPtr _params);
		public delegate void PFN_glGetVertexAttribiv(GLuint index, GLenum pname, [NativeType("GLint*")] IntPtr _params);
		public delegate void PFN_glIsProgram(GLuint program);
		public delegate void PFN_glIsShader(GLuint shader);
		public delegate void PFN_glLinkProgram(GLuint program);
		public delegate void PFN_glShaderSource(GLuint shader, GLsizei count, [NativeType("const GLchar* const*")] IntPtr _string, [NativeType("const GLint*")] IntPtr length);
		public delegate void PFN_glStencilFuncSeparate(GLenum frontfunc, GLenum backfunc, GLint _ref, GLuint mask);
		public delegate void PFN_glStencilMasksSeparate(GLenum face, GLuint mask);
		public delegate void PFN_glStencilOpSeparate(GLenum face, GLenum sfail, GLenum dpfail, GLenum dppass);
		public delegate void PFN_glUniform1f(GLint location, GLfloat v0);
		public delegate void PFN_glUniform1i(GLint location, GLint v0);
		public delegate void PFN_glUniform2f(GLint location, GLfloat v0, GLfloat v1);
		public delegate void PFN_glUniform2i(GLint location, GLint v0, GLint v1);
		public delegate void PFN_glUniform3f(GLint location, GLfloat v0, GLfloat v1, GLfloat v2);
		public delegate void PFN_glUniform3i(GLint location, GLint v0, GLint v1, GLint v2);
		public delegate void PFN_glUniform4f(GLint location, GLfloat v0, GLfloat v1, GLfloat v2, GLfloat v3);
		public delegate void PFN_glUniform4i(GLint location, GLint v0, GLint v1, GLint v2, GLint v3);
		public delegate void PFN_glUniformMatrix2fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix3fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix4fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUseProgram(GLuint program);
		public delegate void PFN_glValidateProgram(GLuint program);
		public delegate void PFN_glVertexAttribPointer(GLuint index, GLint size, GLenum type, GLboolean normalized, GLsizei stride, IntPtr pointer);

		public PFN_glAttachShader glAttachShader;
		public PFN_glBindAttribLocation glBindAttribLocation;
		public PFN_glBlendEquationSeparate glBlendEquationSeparate;
		public PFN_glCompileShader glCompileShader;
		public PFN_glCreateProgram glCreateProgram;
		public PFN_glCreateShader glCreateShader;
		public PFN_glDeleteProgram glDeleteProgram;
		public PFN_glDeleteShader glDeleteShader;
		public PFN_glDetachShader glDetachShader;
		public PFN_glDisableVertexAttribArray glDisableVertexAttribArray;
		public PFN_glDrawBuffers glDrawBuffers;
		public PFN_glEnableVertexAttribArray glEnableVertexAttribArray;
		public PFN_glGetActiveAttrib glGetActiveAttrib;
		public PFN_glGetActiveUniform glGetActiveUniform;
		public PFN_glGetAttachedShaders glGetAttachedShaders;
		public PFN_glGetAttribLocation glGetAttribLocation;
		public PFN_glGetProgramInfoLog glGetProgramInfoLog;
		public PFN_glGetProgramiv glGetProgramiv;
		public PFN_glGetShaderInfoLog glGetShaderInfoLog;
		public PFN_glGetShaderSource glGetShaderSource;
		public PFN_glGetShaderiv glGetShaderiv;
		public PFN_glGetUniformLocation glGetUniformLocation;
		public PFN_glGetUniformfv glGetUniformfv;
		public PFN_glGetUniformiv glGetUniformiv;
		public PFN_glGetVertexAttribPointerv glGetVertexAttribPointerv;
		public PFN_glGetVertexAttribdv glGetVertexAttribdv;
		public PFN_glGetVertexAttribfv glGetVertexAttribfv;
		public PFN_glGetVertexAttribiv glGetVertexAttribiv;
		public PFN_glIsProgram glIsProgram;
		public PFN_glIsShader glIsShader;
		public PFN_glLinkProgram glLinkProgram;
		public PFN_glShaderSource glShaderSource;
		public PFN_glStencilFuncSeparate glStencilFuncSeparate;
		public PFN_glStencilMasksSeparate glStencilMasksSeparate;
		public PFN_glStencilOpSeparate glStencilOpSeparate;
		public PFN_glUniform1f glUniform1f;
		public PFN_glUniform1i glUniform1i;
		public PFN_glUniform2f glUniform2f;
		public PFN_glUniform2i glUniform2i;
		public PFN_glUniform3f glUniform3f;
		public PFN_glUniform3i glUniform3i;
		public PFN_glUniform4f glUniform4f;
		public PFN_glUniform4i glUniform4i;
		public PFN_glUniformMatrix2fv glUniformMatrix2fv;
		public PFN_glUniformMatrix3fv glUniformMatrix3fv;
		public PFN_glUniformMatrix4fv glUniformMatrix4fv;
		public PFN_glUseProgram glUseProgram;
		public PFN_glValidateProgram glValidateProgram;
		public PFN_glVertexAttribPointer glVertexAttribPointer;
	}

	public class GL21Functions {

		public delegate void PFN_glUniformMatrix2x3fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix2x4fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix3x2fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix3x4fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix4x2fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);
		public delegate void PFN_glUniformMatrix4x3fv(GLint location, GLsizei count, GLboolean transpose, [NativeType("const GLfloat*")] IntPtr value);

		public PFN_glUniformMatrix2x3fv glUniformMatrix2x3fv;
		public PFN_glUniformMatrix2x4fv glUniformMatrix2x4fv;
		public PFN_glUniformMatrix3x2fv glUniformMatrix3x2fv;
		public PFN_glUniformMatrix3x4fv glUniformMatrix3x4fv;
		public PFN_glUniformMatrix4x2fv glUniformMatrix4x2fv;
		public PFN_glUniformMatrix4x3fv glUniformMatrix4x3fv;

	}

	public class GL45Functions {

		public PFN_glGetGraphicsResetStatus glGetGraphicsResetStatus;
		public PFN_glGetnCompressedTexImage glGetnCompressedTexImage;
		public PFN_glGetnTexImage glGetnTexImage;
		public PFN_glGetnUniformdv glGetnUniformdv;

	}

	public delegate GLenum PFN_glGetGraphicsResetStatus();
	public delegate void PFN_glGetnCompressedTexImage(GLenum target, GLint lod, GLsizei bufSize, IntPtr pixels);
	public delegate void PFN_glGetnTexImage(GLenum tex, GLint level, GLenum format, GLenum type, GLsizei bufSize, IntPtr pixels);
	public delegate void PFN_glGetnUniformdv(GLuint program, GLint location, GLsizei bufSize, [NativeType("GLdouble*")] IntPtr _params);

	public class GL46Functions {

		public PFN_glMultiDrawArraysIndirectCount glMultiDrawArraysIndirectCount;
		public PFN_glMultiDrawElementsIndirectCount glMultiDrawElementsIndirectCount;
		public PFN_glSpecializeShader glSpecializeShader;

	}

	public delegate void PFN_glMultiDrawArraysIndirectCount(GLenum mode, IntPtr indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride);
	public delegate void PFN_glMultiDrawElementsIndirectCount(GLenum mode, GLenum type, IntPtr indirect, GLintptr drawcount, GLsizei maxdrawcount, GLsizei stride);
	public delegate void PFN_glSpecializeShader(GLuint shader, [MarshalAs(UnmanagedType.LPStr)] string entryPoint, GLuint numSpecializationConstants, [NativeType("const GLuint*")] IntPtr pConstantIndex, [NativeType("const GLuint*")] IntPtr pConstantValue);

}
