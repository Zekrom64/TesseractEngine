using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Tesseract.Core.Math;

namespace Tesseract.OpenGL {

	public class GL43 : GL42 {

		public GL43(GL gl, IGLContext context) : base(gl, context) { }

		// ARB_arrays_of_arrays

		// ARB_ES3_compatibility

		// ARB_clear_buffer_object

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBufferData<T>(GLBufferTarget target, GLInternalFormat internalFormat, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged =>
			GL.ARBClearBufferObject.ClearBufferData(target, internalFormat, format, type, data);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearBufferSubData<T>(GLBufferTarget target, GLInternalFormat internalFormat, nint offset, nint size, GLFormat format, GLType type, in ReadOnlySpan<T> data) where T : unmanaged =>
			GL.ARBClearBufferObject.ClearBufferSubData(target, internalFormat, offset, size, format, type, data);

		// ARB_compute_shader

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchCompute(Vector3ui numGroups) => GL.ARBComputeShader.DispatchCompute(numGroups);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DispatchComputeIndirect(nint offset) => GL.ARBComputeShader.DispatchComputeIndirect(offset);

		// ARB_copy_image

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void CopyImageSubData(uint srcName, GLTextureTarget srcTarget, int srcLevel, Vector3i src, uint dstName, GLTextureTarget dstTarget, int dstLevel, Vector3i dst, Vector3i size) =>
			GL.ARBCopyImage.CopyImageSubData(srcName, srcTarget, srcLevel, src, dstName, dstTarget, dstLevel, dst, size);

		// ARB_explicit_uniform_location

		// ARB_fragment_layer_viewport

		// ARB_framebuffer_no_attachments

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void FramebufferParameter(GLFramebufferTarget target, GLFramebufferParameter pname, int param) => GL.ARBFramebufferNoAttachments.FramebufferParameter(target, pname, param);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetFramebufferParameteri(GLFramebufferTarget target, GLFramebufferParameter pname) => GL.ARBFramebufferNoAttachments.GetFramebufferParameteri(target, pname);

		// ARB_internalformat_query2

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<long> GetInternalFormat(GLInternalFormatTarget target, GLInternalFormat internalFormat, GLGetInternalFormat pname, Span<long> values) => GL.ARBInternalFormatQuery2.GetInternalFormat(target, internalFormat, pname, values);

		// ARB_invalidate_subdata

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateTexSubImage(uint texture, int level, Vector3i offset, Vector3i size) => GL.ARBInvalidateSubdata.InvalidateTexSubImage(texture, level, offset, size);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateTexImage(uint texture, int level) => GL.ARBInvalidateSubdata.InvalidateTexImage(texture, level);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateBufferSubData(uint buffer, nint offset, nint length) => GL.ARBInvalidateSubdata.InvalidateBufferSubData(buffer, offset, length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateBufferData(uint buffer) => GL.ARBInvalidateSubdata.InvalidateBufferData(buffer);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateFramebuffer(GLFramebufferTarget target, in ReadOnlySpan<GLFramebufferAttachment> attachments) => GL.ARBInvalidateSubdata.InvalidateFramebuffer(target, attachments);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvalidateSubFramebuffer(GLFramebufferTarget target, in ReadOnlySpan<GLFramebufferAttachment> attachments, Recti area) => GL.ARBInvalidateSubdata.InvalidateSubFramebuffer(target, attachments, area);

		// ARB_multi_draw_indirect

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawArraysIndirect(GLDrawMode mode, nint offset, int primcount, int stride) => GL.ARBMultiDrawIndirect.MultiDrawArraysIndirect(mode, offset, primcount, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void MultiDrawElementsIndirect(GLDrawMode mode, GLIndexType type, nint offset, int primcount, int stride) => GL.ARBMultiDrawIndirect.MultiDrawElementsIndirect(mode, type, offset, primcount, stride);

		// ARB_program_interface_query

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetProgramInterface(uint program, GLProgramInterface programInterface, GLGetProgramInterface pname, Span<int> values) => GL.ARBProgramInterfaceQuery.GetProgramInterface(program, programInterface, pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint GetProgramResourceIndex(uint program, GLProgramInterface programInterface, string name) => GL.ARBProgramInterfaceQuery.GetProgramResourceIndex(program, programInterface, name);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetProgramResourceName(uint program, GLProgramInterface programInterface, uint index) => GL.ARBProgramInterfaceQuery.GetProgramResourceName(program, programInterface, index);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<int> GetProgramResource(uint program, GLProgramInterface programInterface, uint index, in ReadOnlySpan<GLGetProgramResource> props, Span<int> values) => GL.ARBProgramInterfaceQuery.GetProgramResource(program, programInterface, index, props, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramResourceLocation(uint program, GLProgramInterface programInterface, string name) => GL.ARBProgramInterfaceQuery.GetProgramResourceLocation(program, programInterface, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetProgramResourceLocationIndex(uint program, GLProgramInterface programInterface, string name) => GL.ARBProgramInterfaceQuery.GetProgramResourceLocationIndex(program, programInterface, name);

		// ARB_robust_buffer_access_behavior

		// ARB_shader_image_size

		// ARB_shader_storage_buffer_object

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding) => GL.ARBShaderStorageBufferObject.ShaderStorageBlockBinding(program, storageBlockIndex, storageBlockBinding);

		// ARB_stencil_texturing

		// ARB_texture_buffer_range

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexBufferRange(GLBufferTarget target, GLInternalFormat internalFormat, uint buffer, nint offset, nint size) => GL.ARBTextureBufferRange.TexBufferRange(target, internalFormat, buffer, offset, size);

		// ARB_texture_query_levels

		// ARB_texture_storage_multisample

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage2DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, bool fixedSampleLocations) => GL.ARBTextureStorageMultisample.TexStorage2DMultisample(target, samples, internalFormat, width, height, fixedSampleLocations);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TexStorage3DMultisample(GLTextureTarget target, int samples, GLInternalFormat internalFormat, int width, int height, int depth, bool fixedSampleLocations) => GL.ARBTextureStorageMultisample.TexStorage3DMultisample(target, samples, internalFormat, width, height, depth, fixedSampleLocations);

		// ARB_texture_view

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TextureView(uint texture, GLTextureTarget target, uint origTexture, GLInternalFormat internalFormat, uint minLevel, uint numLevels, uint minLayer, uint numLayers) => GL.ARBTextureView.TextureView(texture, target, origTexture, internalFormat, minLevel, numLevels, minLayer, numLayers);

		// ARB_vertex_attrib_binding

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void BindVertexBuffer(uint bindingIndex, uint buffer, nint offset, int stride) => GL.ARBVertexAttribBinding.BindVertexBuffer(bindingIndex, buffer, offset, stride);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribFormat(uint attribIndex, int size, GLType type, bool normalized, uint relativeOffset) => GL.ARBVertexAttribBinding.VertexAttribFormat(attribIndex, size, type, normalized, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribIFormat(uint attribIndex, int size, GLType type, uint relativeOffset) => GL.ARBVertexAttribBinding.VertexAttribIFormat(attribIndex, size, type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribLFormat(uint attribIndex, int size, GLType type, uint relativeOffset) => GL.ARBVertexAttribBinding.VertexAttribLFormat(attribIndex, size, type, relativeOffset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexAttribBinding(uint attribIndex, uint bindingIndex) => GL.ARBVertexAttribBinding.VertexAttribBinding(attribIndex, bindingIndex);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void VertexBindingDivisor(uint bindingIndex, uint divisor) => GL.ARBVertexAttribBinding.VertexBindingDivisor(bindingIndex, divisor);

		// KHR_debug / ARB_debug_output

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugMessageControl(GLDebugSource source, GLDebugType type, GLDebugSeverity severity, in ReadOnlySpan<uint> ids, bool enabled) => GL.KHRDebug.DebugMessageControl(source, type, severity, ids, enabled);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugMessageInsert(GLDebugSource source, GLDebugType type, uint id, GLDebugSeverity severity, string message) => GL.KHRDebug.DebugMessageInsert(source, type, id, severity, message);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void DebugMessageCallback(GLDebugProc callback, IntPtr userParam) => GL.KHRDebug.DebugMessageCallback(callback, userParam);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<IntPtr> GetPointer(uint pname, Span<IntPtr> values) => GL.KHRDebug.GetPointer(pname, values);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PushDebugGroup(GLDebugSource source, uint id, string message) => GL.KHRDebug.PushDebugGroup(source, id, message);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void PopDebugGroup() => GL.KHRDebug.PopDebugGroup();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ObjectLabel(GLIdentifier identifier, uint name, string label) => GL.KHRDebug.ObjectLabel(identifier, name, label);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetObjectLabel(GLIdentifier identifier, uint name) => GL.KHRDebug.GetObjectLabel(identifier, name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ObjectPtrLabel(IntPtr ptr, string label) => GL.KHRDebug.ObjectPtrLabel(ptr, label);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetObjectPtrLabel(IntPtr ptr) => GL.KHRDebug.GetObjectPtrLabel(ptr);

	}

}
