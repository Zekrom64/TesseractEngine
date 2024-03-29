﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core;
using Tesseract.Core.Native;
using Tesseract.OpenGL.Native;

namespace Tesseract.OpenGL {

	using GLenum = UInt32;
	using GLbitfield = UInt32;
	using GLuint = UInt32;
	using GLint = Int32;
	using GLsizei = Int32;
	using GLboolean = Boolean;
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

	public class GLException : Exception {
	
		public GLException(string msg) : base(msg) { }

	}


	/// <summary>
	/// A GL object is an object that stores a reference to an instance of the OpenGL API.
	/// </summary>
	public interface IGLObject {

		/// <summary>
		/// The OpenGL API this object references.
		/// </summary>
		public GL? GL { get; }

	}

	/// <summary>
	/// <para>
	/// The GL class provides a whollistic view of an OpenGL context, determining which extensions
	/// and OpenGL versions are supported and loading the appropriate functions.
	/// </para>
	/// <para>
	///	Extensions which add new functions are stored as objects with these functions if present, otherwise
	///	the value of these objects will be null. OpenGL versions will also have their own objects following
	///	the same pattern. Newer OpenGL versions will only be registered if all their required extensions
	///	are present.
	/// </para>
	/// </summary>
	public class GL : IGLContextObject {

		/// <summary>
		/// The OpenGL context linked to the GL object.
		/// </summary>
		public IGLContext Context { get; }

		public GL11 GL11 { get; }
		public GL12? GL12 { get; }
		public GL13? GL13 { get; }
		public GL14? GL14 { get; }
		public GL15? GL15 { get; }
		public GL20? GL20 { get; }
		public GL30? GL30 { get; }
		public GL31? GL31 { get; }
		public GL32? GL32 { get; }
		public GL33? GL33 { get; }
		public GL40? GL40 { get; }
		public GL41? GL41 { get; }
		public GL42? GL42 { get; }
		public GL43? GL43 { get; }
		public GL44? GL44 { get; }
		public GL45? GL45 { get; }
		public GL46? GL46 { get; }

		private readonly HashSet<string> extensions = new();
		/// <summary>
		/// The set of extensions supported by this OpenGL instance.
		/// </summary>
		public IReadOnlySet<string> Extensions => extensions;

		// OpenGL 3.0 Extensions
		// EXT_gpu_shader4
		public EXTGPUShader4? EXTGPUShader4 { get; }
		// NV_conditional_render
		public NVConditionalRender? NVConditionalRender { get; }
		// ARB_map_buffer_range - Note: This borrows from APPLE_flush_mapped_buffer BUT unlike what the spec might lead you to believe this is not part of the core
		public ARBMapBufferRange? ARBMapBufferRange { get; }
		// ARB_color_buffer_float
		public ARBColorBufferFloat? ARBColorBufferFloat { get; }
		// ARB_depth_buffer_float
		public bool ARBDepthBufferFloat { get; }
		// ARB_texture_float
		public bool ARBTextureFloat { get; }
		// EXT_packed_float
		public bool EXTPackedFloat { get; }
		// EXT_texture_shared_exponent
		public bool EXTTextureSharedExponent { get; }
		// ARB_framebuffer_object - Note: This combines EXT_framebuffer_object, EXT_framebuffer_blit, EXT_framebuffer_multisample, and EXT_packed_depth_stencil, and EXT_texture_array
		public ARBFramebufferObject? ARBFramebufferObject { get; }
		// ARB_half_float_pixel
		public bool ARBHalfFloatPixel { get; }
		// EXT_texture_integer
		public EXTTextureInteger? EXTTextureInteger { get; }
		// EXT_draw_buffers2
		public EXTDrawBuffers2? EXTDrawBuffers2 { get; }
		// EXT_texture_compression_rgtc
		public bool EXTTextureCompressionRGTC { get; }
		// ARB_transform_feedback - Note: *Should* be the same as EXT_transform_feedback
		public ARBTransformFeedback? ARBTransformFeedback { get; }
		// ARB_vertex_array_object
		public ARBVertexArrayObject? ARBVertexArrayObject { get; }

		// OpenGL 3.1 Extensions
		// ARB_draw_instanced
		public ARBDrawInstanced? ARBDrawInstanced { get; }
		// ARB_copy_buffer
		public ARBCopyBuffer? ARBCopyBuffer { get; }
		// NV_primitive_restart
		public NVPrimitiveRestart? NVPrimitiveRestart { get; }
		// ARB_texture_buffer_object
		public ARBTextureBufferObject? ARBTextureBufferObject { get; }
		// ARB_texture_rectangle
		public bool ARBTextureRectangle { get; }
		// ARB_uniform_buffer_object
		public ARBUniformBufferObject? ARBUniformBufferObject { get; }

		// OpenGL 3.2 Extensions
		// ARB_compatibility
		public bool ARBCompatibility { get; }
		// ARB_vertex_array_bgra
		public bool ARBVertexArrayBGRA { get; }
		// ARB_draw_elements_base_vertex
		public ARBDrawElementsBaseVertex? ARBDrawElementsBaseVertex { get; }
		// ARB_fragment_coord_conventions
		public bool ARBFragmentCoordConventions { get; }
		// ARB_provoking_vertex
		public ARBProvokingVertex? ARBProvokingVertex { get; }
		// ARB_seamless_cube_map
		public bool ARBSeamlessCubeMap { get; }
		// ARB_texture_multisample
		public ARBTextureMultisample? ARBTextureMultisample { get; }
		// ARB_depth_clamp
		public bool ARBDepthClamp { get; }
		// ARB_geometry_shader4
		public ARBGeometryShader4? ARBGeometryShader4 { get; }
		// ARB_sync
		public ARBSync? ARBSync { get; }

		// OpenGL 3.3 Extensions
		// ARB_shader_bit_encoding
		public bool ARBShaderBitEncoding { get; }
		// ARB_blend_func_extended
		public ARBBlendFuncExtended? ARBBlendFuncExtended { get; }
		// ARB_explicit_attrib_location
		public bool ARBExplicitAttribLocation { get; }
		// ARB_occlusion_query2
		public bool ARBOcclusionQuery2 { get; }
		// ARB_sampler_objects
		public ARBSamplerObjects? ARBSamplerObjects { get; }
		// ARB_texture_rgb10_a2ui
		public bool ARBTextureRGB10A2UI { get; }
		// ARB_texture_swizzle
		public bool ARBTextureSwizzle { get; }
		// ARB_timer_query
		public ARBTimerQuery? ARBTimerQuery { get; }
		// ARB_instanced_arrays
		public ARBInstancedArrays? ARBInstancedArrays { get; }
		// ARB_vertex_type_2_10_10_10_rev
		public bool ARBVertexType2_10_10_10Rev { get; }

		// OpenGL 4.0 Extensions
		// ARB_texture_query_lod
		public bool ARBTextureQueryLOD { get; }
		// ARB_draw_buffers_blend
		public ARBDrawBuffersBlend? ARBDrawBuffersBlend { get; }
		// ARB_draw_indirect
		public ARBDrawIndirect? ARBDrawIndirect { get; }
		// ARB_gpu_shader5
		public bool ARBGPUShader5 { get; }
		// ARB_gpu_shader_fp64
		public bool ARBGPUShaderFP64 { get; }
		// ARB_sample_shading
		public ARBSampleShading? ARBSampleShading { get; }
		// ARB_shader_subroutine
		public ARBShaderSubroutine? ARBShaderSubroutine { get; }
		// ARB_tessellation_shader
		public ARBTessellationShader? ARBTessellationShader { get; }
		// ARB_texture_buffer_object_rgb32
		public bool ARBTextureBufferObjectRGB32 { get; }
		// ARB_texture_cube_map_array
		public bool ARBTextureCubeMapArray { get; }
		// ARB_texture_gather
		public bool ARBTextureGather { get; }
		// ARB_transform_feedback2
		public ARBTransformFeedback2? ARBTransformFeedback2 { get; }
		// ARB_transform_feedback3
		public ARBTransformFeedback3? ARBTransformFeedback3 { get; }

		// OpenGL 4.1 Extensions
		// ARB_ES2_compatibility
		public ARBES2Compatbility? ARBES2Compatbility { get; }
		// ARB_get_program_binary
		public ARBGetProgramBinary? ARBGetProgramBinary { get; }
		// ARB_separate_shader_objects
		public ARBSeparateShaderObjects? ARBSeparateShaderObjects { get; }
		// ARB_shader_precision
		public bool ARBShaderPrecision { get; }
		// ARB_vertex_attrib_64bit
		public ARBVertexAttrib64Bit? ARBVertexAttrib64Bit { get; }
		// ARB_viewport_array
		public ARBViewportArray? ARBViewportArray { get; }

		// OpenGL 4.2 Extensions
		// ARB_texture_compression_bptc
		public bool ARBTextureCompressionBPTC { get; }
		// ARB_compressed_texture_pixel_storage
		public bool ARBCompressedTexturePixelStorage { get; }
		// ARB_shader_atomic_counters
		public ARBShaderAtomicCounters? ARBShaderAtomicCounters { get; }
		// ARB_texture_storage
		public ARBTextureStorage? ARBTextureStorage { get; }
		// ARB_transform_feedback_instanced
		public ARBTransformFeedbackInstanced? ARBTransformFeedbackInstanced { get; }
		// ARB_base_instance
		public ARBBaseInstance? ARBBaseInstance { get; }
		// ARB_shader_image_load_store
		public ARBShaderImageLoadStore? ARBShaderImageLoadStore { get; }
		// ARB_conservative_depth
		public bool ARBConservativeDepth { get; }
		// ARB_shading_language_420pack
		public bool ARBShadingLanguage420Pack { get; }
		// ARB_internalformat_query
		public ARBInternalFormatQuery? ARBInternalFormatQuery { get; }
		// ARB_map_buffer_alignment
		public bool ARBMapBufferAlignment { get; }
		// ARB_shading_language_packing
		public bool ARBShadingLanguagePacking { get; }

		// OpenGL 4.3 Extensions
		// ARB_arrays_of_arrays
		public bool ARBArraysOfArrays { get; }
		// ARB_ES3_compatibility
		public bool ARBES3Compatibility { get; }
		// ARB_clear_buffer_object
		public ARBClearBufferObject? ARBClearBufferObject { get; }
		// ARB_compute_shader
		public ARBComputeShader? ARBComputeShader { get; }
		// ARB_copy_image
		public ARBCopyImage? ARBCopyImage { get; }
		// ARB_explicit_uniform_location
		public bool ARBExplicitUniformLocation { get; }
		// ARB_fragment_layer_viewport
		public bool ARBFragmentLayerViewport { get; }
		// ARB_framebuffer_no_attachments
		public ARBFramebufferNoAttachments? ARBFramebufferNoAttachments { get; }
		// ARB_internalformat_query2
		public ARBInternalFormatQuery2? ARBInternalFormatQuery2 { get; }
		// ARB_invalidate_subdata
		public ARBInvalidateSubdata? ARBInvalidateSubdata { get; }
		// ARB_multi_draw_indirect
		public ARBMultiDrawIndirect? ARBMultiDrawIndirect { get; }
		// ARB_program_interface_query
		public ARBProgramInterfaceQuery? ARBProgramInterfaceQuery { get; }
		// ARB_robust_buffer_access_behavior
		public bool ARBRobustBufferAccessBehavior { get; }
		// ARB_shader_image_size
		public bool ARBShaderImageSize { get; }
		// ARB_shader_storage_buffer_object
		public ARBShaderStorageBufferObject? ARBShaderStorageBufferObject { get; }
		// ARB_stencil_texturing
		public bool ARBStencilTexturing { get; }
		// ARB_texture_buffer_range
		public ARBTextureBufferRange? ARBTextureBufferRange { get; }
		// ARB_texture_query_levels
		public bool ARBTextureQueryLevels { get; }
		// ARB_texture_storage_multisample
		public ARBTextureStorageMultisample? ARBTextureStorageMultisample { get; }
		// ARB_texture_view
		public ARBTextureView? ARBTextureView { get; }
		// ARB_vertex_attrib_binding
		public ARBVertexAttribBinding? ARBVertexAttribBinding { get; }
		// KHR_debug / ARB_debug_output
		public KHRDebug? KHRDebug { get; }

		// OpenGL 4.4 Extensions
		// ARB_buffer_storage
		public ARBBufferStorage? ARBBufferStorage { get; }
		// ARB_clear_texture
		public ARBClearTexture? ARBClearTexture { get; }
		// ARB_enhanced_layouts
		public bool ARBEnhancedLayouts { get; }
		// ARB_multi_bind
		public ARBMultiBind? ARBMultiBind { get; }
		// ARB_query_buffer_object
		public bool ARBQueryBufferObject { get; }
		// ARB_texture_mirror_clamp_to_edge
		public bool ARBTextureMirrorClampToEdge { get; }
		// ARB_texture_stencil8
		public bool ARBTextureStencil8 { get; }
		// ARB_vertex_type_10f_11f_11f_rev
		public bool ARBVertexType10f11f11fRev { get; }

		// OpenGL 4.5 Extensions
		// ARB_clip_control
		public ARBClipControl? ARBClipControl { get; }
		// ARB_cull_distance
		public bool ARBCullDistance { get; }
		// ARB_ES3_1_compatibility
		public ARBES31Compatibility? ARBES31Compatibility { get; }
		// ARB_conditional_render_inverted
		public bool ARBConditionalRenderInverted { get; }
		// KHR_context_flush_control
		public bool KHRContextFlushControl { get; }
		// ARB_derivative_control
		public bool ARBDerivativeControl { get; }
		// ARB_direct_state_access
		public ARBDirectStateAccess? ARBDirectStateAccess { get; }
		// ARB_get_texture_sub_image
		public ARBGetTextureSubImage? ARBGetTextureSubImage { get; }
		// KHR_robustness
		public KHRRobustness? KHRRobustness { get; }
		// ARB_shader_texture_image_samples
		public bool ARBShaderTextureImageSamples { get; }
		// ARB_texture_barrier
		public ARBTextureBarrier? ARBTextureBarrier { get; }

		// OpenGL 4.6 Extensions
		// ARB_indirect_parameters
		public ARBIndirectParameters? ARBIndirectParameters { get; }
		// ARB_pipeline_statistics_query
		public bool ARBPipelineStatisticsQuery { get; }
		// ARB_polygon_offset_clamp
		public ARBPolygonOffsetClamp? ARBPolygonOffsetClamp { get; }
		// KHR_no_error
		public bool KHRNoError { get; }
		// ARB_shader_atomic_counter_ops
		public bool ARBShaderAtomicCounterOps { get; }
		// ARB_shader_draw_parameters
		public bool ARBShaderDrawParameters { get; }
		// ARB_shader_group_vote
		public bool ARBShaderGroupVote { get; }
		// ARB_gl_spirv
		public ARBGLSPIRV? ARBGLSPIRV { get; }
		// ARB_spirv_extensions
		public bool ARBSPIRVExtensions { get; }
		// ARB_texture_filter_anisotropic
		public bool ARBTextureFilterAnisotropic { get; }
		// ARB_transform_feedback_overflow_query
		public bool ARBTransformFeedbackOverflowQuery { get; }

		// Common Extensions
		// EXT_texture_filter_anisotopic - Note: While anisotropic filtering was blessed with ARB status in GL 4.6, *many* vendors support the EXT form in earlier versions
		public bool EXTTextureFilterAnisotropic { get; }
		// EXT_direct_state_access - Note: May be more supported than ARB_direct_state_access on old hardware, but only supports DSA based on OpenGL version and extensions
		// EXT_depth_bounds_test - Note: Don't know why this isn't core
		public EXTDepthBoundsTest? EXTDepthBoundsTest { get; }
		// KHR_texture_compression_astc_hdr
		public bool KHRTextureCompressionASTC_HDR { get; }
		// KHR_texture_compression_astc_ldr
		public bool KHRTextureCompressionASTC_LDR { get; }
		// EXT_texture_compression_s3tc - Note: Not technically core, but pretty much everyone implements it
		public bool EXTTextureCompressionS3TC { get; }
		// ARB_gpu_shader_int64 - Note: While an ARB extension, it is not in any OpenGL version
		public ARBGPUShaderInt64? ARBGPUShaderInt64 { get; }
		// ARB_bindless_texture - Note: This has been around formally since 2013 and is *very* useful for side-stepping texture unit nonsense
		public ARBBindlessTexture? ARBBindlessTexture { get; }

		// AMD Extensions
		public bool AMDGPUShaderInt16 { get; }

		// NVidia Extensions
		public bool NVXGPUMemoryInfo { get; }

		// WGL & Extensions
		public WGL? WGL { get; }
		// WGL_AMD_gpu_association - Note: Really only used to estimate GPU memory
		public WGLAMDGPUAssociation? WGLAMDGPUAssociation { get; }

		public GL(IGLContext context) {
			Context = context;
			// Initialize OpenGL support based on context version
			int major = Context.MajorVersion;
			int minor = Context.MinorVersion;
			bool hasGLXX = major >= 5;
			bool hasGL4X = hasGLXX || major == 4;
			bool hasGL46 = hasGLXX || (hasGL4X && minor >= 6);
			bool hasGL45 = hasGL46 || (hasGL4X && minor >= 5);
			bool hasGL44 = hasGL45 || (hasGL4X && minor >= 4);
			bool hasGL43 = hasGL44 || (hasGL4X && minor >= 3);
			bool hasGL42 = hasGL43 || (hasGL4X && minor >= 2);
			bool hasGL41 = hasGL42 || (hasGL4X && minor >= 1);
			bool hasGL40 = hasGL41 || hasGL4X;
			bool hasGL3X = hasGL4X || major == 3;
			bool hasGL33 = hasGL40 || (hasGL3X && minor >= 3);
			bool hasGL32 = hasGL33 || (hasGL3X && minor >= 2);
			bool hasGL31 = hasGL32 || (hasGL3X && minor >= 1);
			bool hasGL30 = hasGL31 || hasGL3X;
			bool hasGL2X = hasGL3X || major == 2;
			bool hasGL21 = hasGL30 || (hasGL2X && minor >= 1);
			bool hasGL20 = hasGL21 || hasGL2X;
			bool hasGL15 = hasGL20 || minor >= 5;
			bool hasGL14 = hasGL15 || minor >= 4;
			bool hasGL13 = hasGL14 || minor >= 3;
			bool hasGL12 = hasGL13 || minor >= 2;

			// Gather extensions based on known functions
			if (hasGL30) {
				var glGetIntegerv = Marshal.GetDelegateForFunctionPointer<PFN_glGetInteger>(context.GetGLProcAddress("glGetIntegerv"));
				var glGetStringi = Marshal.GetDelegateForFunctionPointer<PFN_glGetStringi>(context.GetGLProcAddress("glGetStringi"));
				glGetIntegerv(GLEnums.GL_NUM_EXTENSIONS, out int nexts);
				for (int i = 0; i < nexts; i++) extensions.Add(MemoryUtil.GetUTF8(glGetStringi(GLEnums.GL_EXTENSIONS, (uint)i))!);
			} else {
				var glGetString = Marshal.GetDelegateForFunctionPointer<PFN_glGetString>(context.GetGLProcAddress("glGetString"));
				string extstr = MemoryUtil.GetUTF8(glGetString(GLEnums.GL_EXTENSIONS))!;
				foreach (string ext in extstr.Split(' ')) if (ext.Length != 0) extensions.Add(ext);
			}

			// GL 3.0
			if (hasGL30) {
				EXTGPUShader4 = new(this, context);
				NVConditionalRender = new(this, context);
				ARBMapBufferRange = new(this, context);
				ARBColorBufferFloat = new(this, context);
				ARBDepthBufferFloat = true;
				ARBTextureFloat = true;
				EXTPackedFloat = true;
				EXTTextureSharedExponent = true;
				ARBFramebufferObject = new(this, context);
				ARBHalfFloatPixel = true;
				EXTTextureInteger = new(this, context);
				EXTDrawBuffers2 = new(this, context);
				EXTTextureCompressionRGTC = true;
				ARBTransformFeedback = new(this, context);
				ARBVertexArrayObject = new(this, context);
			} else {
				if (Extensions.Contains("GL_EXT_gpu_shader4")) EXTGPUShader4 = new(this, context);
				if (Extensions.Contains("GL_NV_conditional_render")) NVConditionalRender = new(this, context);
				if (Extensions.Contains("GL_ARB_map_buffer_range")) ARBMapBufferRange = new(this, context);
				if (Extensions.Contains("GL_ARB_color_buffer_float")) ARBColorBufferFloat = new(this, context);
				if (Extensions.Contains("GL_ARB_depth_buffer_float")) ARBDepthBufferFloat = true;
				if (Extensions.Contains("GL_ARB_texture_float")) ARBTextureFloat = true;
				if (Extensions.Contains("GL_EXT_packed_float")) EXTPackedFloat = true;
				if (Extensions.Contains("GL_EXT_texture_shared_exponent")) EXTTextureSharedExponent = true;
				if (Extensions.Contains("GL_ARB_framebuffer_object") || (
					Extensions.Contains("GL_EXT_framebuffer_object") &&
					Extensions.Contains("GL_EXT_framebuffer_blit") &&
					Extensions.Contains("GL_EXT_framebuffer_multisample") &&
					Extensions.Contains("GL_EXT_packed_depth_stencil") &&
					Extensions.Contains("GL_EXT_texture_array")
				)) ARBFramebufferObject = new(this, context);
				if (Extensions.Contains("GL_ARB_half_float_pixel")) ARBHalfFloatPixel = true;
				if (Extensions.Contains("GL_EXT_texture_integer")) EXTTextureInteger = new(this, context);
				if (Extensions.Contains("GL_EXT_draw_buffers2")) EXTDrawBuffers2 = new(this, context);
				if (Extensions.Contains("GL_EXT_texture_compression_rgtc") || Extensions.Contains("GL_ARB_texture_compression_rgtc")) EXTTextureCompressionRGTC = true;
				// ARB_transform_feedback and EXT_transform_feedback are interchangeable for the spec ¯\_(ツ)_/¯
				if (Extensions.Contains("GL_ARB_transform_feedback") || Extensions.Contains("GL_EXT_transform_feedback")) ARBTransformFeedback = new(this, context);
				if (Extensions.Contains("GL_ARB_vertex_array_object")) ARBVertexArrayObject = new(this, context);
				hasGL30 = hasGL21 &&
					EXTGPUShader4 != null &&
					NVConditionalRender != null && 
					ARBMapBufferRange != null &&
					ARBColorBufferFloat != null &&
					ARBDepthBufferFloat &&
					ARBTextureFloat &&
					EXTPackedFloat &&
					EXTTextureSharedExponent &&
					ARBFramebufferObject != null &&
					ARBHalfFloatPixel &&
					EXTTextureInteger != null &&
					EXTDrawBuffers2 != null &&
					EXTTextureCompressionRGTC &&
					ARBTransformFeedback != null &&
					ARBVertexArrayObject != null;
			}

			// GL 3.1
			if (hasGL31) {
				ARBDrawInstanced = new(this, context);
				ARBCopyBuffer = new(this, context);
				NVPrimitiveRestart = new(this, context);
				ARBTextureBufferObject = new(this, context);
				ARBTextureRectangle = true;
				ARBUniformBufferObject = new(this, context);
			} else {
				if (Extensions.Contains("GL_ARB_draw_instanced")) ARBDrawInstanced = new(this, context);
				// ARB_copy_buffer and EXT_copy_buffer are interchangeable for the spec ¯\_(ツ)_/¯
				if (Extensions.Contains("GL_ARB_copy_buffer") || Extensions.Contains("GL_EXT_copy_buffer")) ARBCopyBuffer = new(this, context);
				if (Extensions.Contains("GL_NV_primitive_restart")) NVPrimitiveRestart = new(this, context);
				if (Extensions.Contains("GL_ARB_texture_buffer_object")) ARBTextureBufferObject = new(this, context);
				if (Extensions.Contains("GL_ARB_texture_rectangle")) ARBTextureRectangle = true;
				if (Extensions.Contains("GL_ARB_uniform_buffer_object")) ARBUniformBufferObject = new(this, context);
				hasGL31 = hasGL30 &&
					ARBDrawInstanced != null &&
					ARBCopyBuffer != null &&
					NVPrimitiveRestart != null &&
					ARBTextureBufferObject != null &&
					ARBTextureRectangle &&
					ARBUniformBufferObject != null;
			}

			// GL 3.2
			if (hasGL32) {
				ARBCompatibility = true;
				ARBVertexArrayBGRA = true;
				ARBDrawElementsBaseVertex = new(this, context);
				ARBFragmentCoordConventions = true;
				ARBProvokingVertex = new(this, context);
				ARBSeamlessCubeMap = true;
				ARBTextureMultisample = new(this, context);
				ARBDepthClamp = true;
				ARBSync = new(this, context);
			} else {
				if (Extensions.Contains("GL_ARB_compatibility")) ARBCompatibility = true;
				if (Extensions.Contains("GL_ARB_vertex_array_bgra")) ARBVertexArrayBGRA = true;
				if (Extensions.Contains("GL_ARB_draw_elements_base_vertex")) ARBDrawElementsBaseVertex = new(this, context);
				if (Extensions.Contains("GL_ARB_fragment_coord_conventions")) ARBFragmentCoordConventions = true;
				if (Extensions.Contains("GL_ARB_provoking_vertex")) ARBProvokingVertex = new(this, context);
				if (Extensions.Contains("GL_ARB_seamless_cube_map")) ARBSeamlessCubeMap = true;
				if (Extensions.Contains("GL_ARB_texture_multisample")) ARBTextureMultisample = new(this, context);
				if (Extensions.Contains("GL_ARB_depth_clamp")) ARBDepthClamp = true;
				if (Extensions.Contains("GL_ARB_sync")) ARBSync = new(this, context);
				hasGL32 = hasGL31 &&
					ARBCompatibility &&
					ARBVertexArrayBGRA &&
					ARBDrawElementsBaseVertex != null &&
					ARBFragmentCoordConventions &&
					ARBProvokingVertex != null &&
					ARBSeamlessCubeMap &&
					ARBTextureMultisample != null &&
					ARBDepthClamp &&
					ARBSync != null &&
					context.GetGLProcAddress("glGetBufferParameteri64v") != IntPtr.Zero;
			}

			// GL 3.3
			if (hasGL33) {
				ARBShaderBitEncoding = true;
				ARBBlendFuncExtended = new(this, context);
				ARBExplicitAttribLocation = true;
				ARBOcclusionQuery2 = true;
				ARBSamplerObjects = new(this, context);
				ARBTextureRGB10A2UI = true;
				ARBTextureSwizzle = true;
				ARBTimerQuery = new(this, context);
				ARBInstancedArrays = new(this, context);
				ARBVertexType2_10_10_10Rev = true;
			} else {
				if (Extensions.Contains("GL_ARB_shader_bit_encoding")) ARBShaderBitEncoding = true;
				if (Extensions.Contains("GL_ARB_blend_func_extended")) ARBBlendFuncExtended = new(this, context);
				if (Extensions.Contains("GL_ARB_explicit_attrib_location")) ARBExplicitAttribLocation = true;
				if (Extensions.Contains("GL_ARB_occlusion_query2")) ARBOcclusionQuery2 = true;
				if (Extensions.Contains("GL_ARB_sampler_objects")) ARBSamplerObjects = new(this, context);
				if (Extensions.Contains("GL_ARB_texture_rgb10_a2ui")) ARBTextureRGB10A2UI = true;
				if (Extensions.Contains("GL_ARB_texture_swizzle")) ARBTextureSwizzle = true;
				if (Extensions.Contains("GL_ARB_timer_query")) ARBTimerQuery = new(this, context);
				if (Extensions.Contains("GL_ARB_instanced_arrays")) ARBInstancedArrays = new(this, context);
				if (Extensions.Contains("GL_ARB_vertex_type_2_10_10_10_rev")) ARBVertexType2_10_10_10Rev = true;
				hasGL33 = hasGL32 &&
					ARBShaderBitEncoding &&
					ARBBlendFuncExtended != null &&
					ARBExplicitAttribLocation &&
					ARBOcclusionQuery2 &&
					ARBSamplerObjects != null &&
					ARBTextureRGB10A2UI &&
					ARBTextureSwizzle &&
					ARBTimerQuery != null &&
					ARBInstancedArrays != null &&
					ARBVertexType2_10_10_10Rev;
			}

			// GL 4.0
			if (hasGL40) {
				ARBTextureQueryLOD = true;
				ARBGPUShader5 = true;
				ARBGPUShaderFP64 = true;
				ARBShaderSubroutine = new(this, context);
				ARBTextureGather = true;
				ARBDrawIndirect = new(this, context);
				ARBSampleShading = new(this, context);
				ARBTessellationShader = new(this, context);
				ARBTextureBufferObjectRGB32 = true;
				ARBTextureCubeMapArray = true;
				ARBTransformFeedback2 = new(this, context);
				ARBTransformFeedback3 = new(this, context);
				ARBDrawBuffersBlend = new(this, context);
			} else {
				if (Extensions.Contains("GL_ARB_texture_query_lod")) ARBTextureQueryLOD = true;
				if (Extensions.Contains("GL_ARB_gpu_shader5")) ARBGPUShader5 = true;
				if (Extensions.Contains("GL_ARB_gpu_shader_fp64")) ARBGPUShaderFP64 = true;
				if (Extensions.Contains("GL_ARB_shader_subroutine")) {
					// Note: The forward compatible Nvidia driver lies and says it supports ARB_shader_subroutine but does not return any function pointers
					if (context.GetGLProcAddress("glGetSubroutineUniformLocation") != 0) ARBShaderSubroutine = new(this, context);
				}
				if (Extensions.Contains("GL_ARB_texture_gather")) ARBTextureGather = true;
				if (Extensions.Contains("GL_ARB_draw_indirect")) ARBDrawIndirect = new(this, context);
				if (Extensions.Contains("GL_ARB_sample_shading")) ARBSampleShading = new(this, context);
				if (Extensions.Contains("GL_ARB_tessellation_shader")) ARBTessellationShader = new(this, context);
				if (Extensions.Contains("GL_ARB_texture_buffer_object_rgb32")) ARBTextureBufferObjectRGB32 = true;
				if (Extensions.Contains("GL_ARB_texture_cube_map_array")) ARBTextureCubeMapArray = true;
				if (Extensions.Contains("GL_ARB_transform_feedback2")) ARBTransformFeedback2 = new(this, context);
				if (Extensions.Contains("GL_ARB_transform_feedback3")) ARBTransformFeedback3 = new(this, context);
				if (Extensions.Contains("GL_ARB_draw_buffers_blend")) ARBDrawBuffersBlend = new(this, context);
				hasGL40 = hasGL33 &&
					ARBTextureQueryLOD &&
					ARBGPUShader5 &&
					ARBGPUShaderFP64 &&
					ARBShaderSubroutine != null &&
					ARBTextureGather &&
					ARBDrawIndirect != null &&
					ARBSampleShading != null &&
					ARBTessellationShader != null &&
					ARBTextureBufferObjectRGB32 &&
					ARBTextureCubeMapArray &&
					ARBTransformFeedback2 != null &&
					ARBTransformFeedback3 != null &&
					ARBDrawBuffersBlend != null;
			}

			// GL 4.1
			if (hasGL41) {
				ARBGetProgramBinary = new(this, context);
				ARBSeparateShaderObjects = new(this, context);
				ARBES2Compatbility = new(this, context);
				ARBShaderPrecision = true;
				ARBVertexAttrib64Bit = new(this, context);
				ARBViewportArray = new(this, context);
			} else {
				if (Extensions.Contains("GL_ARB_get_program_binary")) ARBGetProgramBinary = new(this, context);
				if (Extensions.Contains("GL_ARB_separate_shader_objects")) ARBSeparateShaderObjects = new(this, context);
				if (Extensions.Contains("GL_ARB_ES2_compatibility")) ARBES2Compatbility = new(this, context);
				if (Extensions.Contains("GL_ARB_shader_precision")) ARBShaderPrecision = true;
				if (Extensions.Contains("GL_ARB_vertex_attrib_64bit")) ARBVertexAttrib64Bit = new(this, context);
				if (Extensions.Contains("GL_ARB_viewport_array")) ARBViewportArray = new(this, context);
				hasGL41 = hasGL40 &&
					ARBGetProgramBinary != null &&
					ARBSeparateShaderObjects != null &&
					ARBES2Compatbility != null &&
					ARBShaderPrecision &&
					ARBVertexAttrib64Bit != null &&
					ARBViewportArray != null;
			}

			// GL 4.2
			if (hasGL42) {
				ARBShaderAtomicCounters = new(this, context);
				ARBShaderImageLoadStore = new(this, context);
				ARBTextureStorage = new(this, context);
				ARBTransformFeedbackInstanced = new(this, context);
				ARBShadingLanguage420Pack = true;
				ARBBaseInstance = new(this, context);
				ARBInternalFormatQuery = new(this, context);
				ARBCompressedTexturePixelStorage = true;
				ARBShadingLanguagePacking = true;
				ARBMapBufferAlignment = true;
				ARBConservativeDepth = true;
				ARBTextureCompressionBPTC = true;
			} else {
				if (Extensions.Contains("GL_ARB_shader_atomic_counters")) ARBShaderAtomicCounters = new(this, context);
				if (Extensions.Contains("GL_ARB_shader_image_load_store")) ARBShaderImageLoadStore = new(this, context);
				if (Extensions.Contains("GL_ARB_texture_storage")) ARBTextureStorage = new(this, context);
				if (Extensions.Contains("GL_ARB_transform_feedback_instanced")) ARBTransformFeedbackInstanced = new(this, context);
				if (Extensions.Contains("GL_ARB_shading_language_420pack")) ARBShadingLanguage420Pack = true;
				if (Extensions.Contains("GL_ARB_base_instance")) ARBBaseInstance = new(this, context);
				if (Extensions.Contains("GL_ARB_internalformat_query")) ARBInternalFormatQuery = new(this, context);
				if (Extensions.Contains("GL_ARB_compressed_texture_pixel_storage")) ARBCompressedTexturePixelStorage = true;
				if (Extensions.Contains("GL_ARB_shading_language_packing")) ARBShadingLanguagePacking = true;
				if (Extensions.Contains("GL_ARB_map_buffer_alignment")) ARBMapBufferAlignment = true;
				if (Extensions.Contains("GL_ARB_conservative_depth")) ARBConservativeDepth = true;
				if (Extensions.Contains("GL_ARB_texture_compression_bptc")) ARBTextureCompressionBPTC = true;
				hasGL42 = hasGL41 &&
					ARBShaderAtomicCounters != null &&
					ARBShaderImageLoadStore != null &&
					ARBTextureStorage != null &&
					ARBTransformFeedbackInstanced != null &&
					ARBShadingLanguage420Pack &&
					ARBBaseInstance != null &&
					ARBInternalFormatQuery != null &&
					ARBCompressedTexturePixelStorage &&
					ARBShadingLanguagePacking &&
					ARBMapBufferAlignment &&
					ARBConservativeDepth &&
					ARBTextureCompressionBPTC;
			}

			// GL 4.3
			if (hasGL43) {
				KHRDebug = new(this, context);
				ARBArraysOfArrays = true;
				ARBClearBufferObject = new(this, context);
				ARBComputeShader = new(this, context);
				ARBCopyImage = new(this, context);
				ARBES3Compatibility = true;
				ARBExplicitUniformLocation = true;
				ARBFragmentLayerViewport = true;
				ARBInternalFormatQuery2 = new(this, context);
				ARBInvalidateSubdata = new(this, context);
				ARBMultiDrawIndirect = new(this, context);
				ARBProgramInterfaceQuery = new(this, context);
				ARBShaderImageSize = true;
				ARBShaderStorageBufferObject = new(this, context);
				ARBStencilTexturing = true;
				ARBTextureBufferRange = new(this, context);
				ARBTextureQueryLevels = true;
				ARBTextureStorageMultisample = new(this, context);
				ARBTextureView = new(this, context);
				ARBVertexAttribBinding = new(this, context);
				ARBRobustBufferAccessBehavior = true;
			} else {
				if (Extensions.Contains("KHR_debug")) KHRDebug = new(this, context);
				if (Extensions.Contains("ARB_arrays_of_arrays")) ARBArraysOfArrays = true;
				if (Extensions.Contains("ARB_clear_buffer_object")) ARBClearBufferObject = new(this, context);
				if (Extensions.Contains("ARB_compute_shader")) ARBComputeShader = new(this, context);
				if (Extensions.Contains("ARB_copy_image")) ARBCopyImage = new(this, context);
				if (Extensions.Contains("ARB_ES3_compatibility")) ARBES3Compatibility = true;
				if (Extensions.Contains("ARB_explicit_uniform_location")) ARBExplicitUniformLocation = true;
				if (Extensions.Contains("ARB_fragment_layer_viewport")) ARBFragmentLayerViewport = true;
				if (Extensions.Contains("ARB_internalformat_query2")) ARBInternalFormatQuery2 = new(this, context);
				if (Extensions.Contains("ARB_invalidate_subdata")) ARBInvalidateSubdata = new(this, context);
				if (Extensions.Contains("ARB_multi_draw_indirect")) ARBMultiDrawIndirect = new(this, context);
				if (Extensions.Contains("ARB_program_interface_query")) ARBProgramInterfaceQuery = new(this, context);
				if (Extensions.Contains("ARB_shader_image_size")) ARBShaderImageSize = true;
				if (Extensions.Contains("ARB_shader_storage_buffer_object")) ARBShaderStorageBufferObject = new(this, context);
				if (Extensions.Contains("ARB_stencil_texture")) ARBStencilTexturing = true;
				if (Extensions.Contains("ARB_texture_buffer_range")) ARBTextureBufferRange = new(this, context);
				if (Extensions.Contains("ARB_texture_query_levels")) ARBTextureQueryLevels = true;
				if (Extensions.Contains("ARB_texture_storage_multisample")) ARBTextureStorageMultisample = new(this, context);
				if (Extensions.Contains("ARB_texture_view")) ARBTextureView = new(this, context);
				if (Extensions.Contains("ARB_vertex_attrib_binding")) ARBVertexAttribBinding = new(this, context);
				if (Extensions.Contains("ARB_robust_buffer_access_behavior")) ARBRobustBufferAccessBehavior = true;
				hasGL43 =
					KHRDebug != null &&
					ARBArraysOfArrays &&
					ARBClearBufferObject != null &&
					ARBComputeShader != null &&
					ARBCopyImage != null &&
					ARBES3Compatibility &&
					ARBExplicitUniformLocation &&
					ARBFragmentLayerViewport &&
					ARBInternalFormatQuery2 != null &&
					ARBInvalidateSubdata != null &&
					ARBMultiDrawIndirect != null &&
					ARBProgramInterfaceQuery != null &&
					ARBShaderImageSize &&
					ARBShaderStorageBufferObject != null &&
					ARBStencilTexturing &&
					ARBTextureBufferRange != null &&
					ARBTextureQueryLevels &&
					ARBTextureStorageMultisample != null &&
					ARBTextureView != null &&
					ARBVertexAttribBinding != null &&
					ARBRobustBufferAccessBehavior;
			}

			// GL 4.4
			if (hasGL44) {
				ARBBufferStorage = new(this, context);
				ARBClearTexture = new(this, context);
				ARBEnhancedLayouts = true;
				ARBMultiBind = new(this, context);
				ARBQueryBufferObject = true;
				ARBTextureMirrorClampToEdge = true;
				ARBTextureStencil8 = true;
				ARBVertexType10f11f11fRev = true;
			} else {
				if (Extensions.Contains("GL_ARB_buffer_storage")) ARBBufferStorage = new(this, context);
				if (Extensions.Contains("GL_ARB_clear_texture")) ARBClearTexture = new(this, context);
				if (Extensions.Contains("GL_ARB_enhanced_layouts")) ARBEnhancedLayouts = true;
				if (Extensions.Contains("GL_ARB_multi_bind")) ARBMultiBind = new(this, context);
				if (Extensions.Contains("GL_ARB_query_buffer_object")) ARBQueryBufferObject = true;
				if (Extensions.Contains("GL_ARB_texture_mirror_clamp_to_edge")) ARBTextureMirrorClampToEdge = true;
				if (Extensions.Contains("GL_ARB_texture_stencil8")) ARBTextureStencil8 = true;
				if (Extensions.Contains("GL_ARB_vertex_type_10f_11f_11f_rev")) ARBVertexType10f11f11fRev = true;
				hasGL44 =
					ARBBufferStorage != null &&
					ARBClearTexture != null &&
					ARBEnhancedLayouts &&
					ARBMultiBind != null &&
					ARBQueryBufferObject &&
					ARBTextureMirrorClampToEdge &&
					ARBTextureStencil8 &&
					ARBVertexType10f11f11fRev;
			}

			// GL 4.5
			if (hasGL45) {
				ARBClipControl = new(this, context);
				ARBCullDistance = true;
				ARBES31Compatibility = new(this, context);
				ARBConditionalRenderInverted = true;
				ARBDerivativeControl = true;
				KHRContextFlushControl = true;
				ARBDirectStateAccess = new(this, context);
				ARBGetTextureSubImage = new(this, context);
				KHRRobustness = new(this, context);
				ARBShaderTextureImageSamples = true;
				ARBTextureBarrier = new(this, context);
			} else {
				if (Extensions.Contains("GL_ARB_clip_control")) ARBClipControl = new(this, context);
				if (Extensions.Contains("GL_ARB_cull_distance")) ARBCullDistance = true;
				if (Extensions.Contains("GL_ARB_ES3_1_compatibility")) ARBES31Compatibility = new(this, context);
				if (Extensions.Contains("GL_ARB_conditional_render_inverted")) ARBConditionalRenderInverted = true;
				if (Extensions.Contains("GL_ARB_derivative_control")) ARBDerivativeControl = true;
				if (Extensions.Contains("GL_KHR_context_flush_control")) KHRContextFlushControl = true;
				if (Extensions.Contains("GL_ARB_direct_state_access")) ARBDirectStateAccess = new(this, context);
				if (Extensions.Contains("GL_ARB_get_texture_sub_image")) ARBGetTextureSubImage = new(this, context);
				if (Extensions.Contains("GL_KHR_robustness")) KHRRobustness = new(this, context);
				if (Extensions.Contains("GL_ARB_shader_texture_image_samples")) ARBShaderTextureImageSamples = true;
				if (Extensions.Contains("GL_ARB_texture_barrier")) ARBTextureBarrier = new(this, context);
				hasGL45 =
					ARBClipControl != null &&
					ARBCullDistance &&
					ARBES31Compatibility != null &&
					ARBConditionalRenderInverted &&
					ARBDerivativeControl &&
					KHRContextFlushControl &&
					ARBDirectStateAccess != null &&
					ARBGetTextureSubImage != null &&
					KHRRobustness != null &&
					ARBShaderTextureImageSamples &&
					ARBTextureBarrier != null;
			}

			// GL 4.6
			if (hasGL46) {
				ARBGLSPIRV = new(this, context);
				ARBSPIRVExtensions = true;
				ARBShaderDrawParameters = true;
				ARBIndirectParameters = new(this, context);
				ARBPipelineStatisticsQuery = true;
				ARBTransformFeedbackOverflowQuery = true;
				ARBTextureFilterAnisotropic = true;
				ARBPolygonOffsetClamp = new(this, context);
				KHRNoError = true;
				ARBShaderAtomicCounterOps = true;
				ARBShaderGroupVote = true;
			} else {
				if (Extensions.Contains("GL_ARB_gl_spirv")) ARBGLSPIRV = new(this, context);
				if (Extensions.Contains("GL_ARB_spirv_extensions")) ARBSPIRVExtensions = true;
				if (Extensions.Contains("GL_ARB_shader_draw_parameters")) ARBShaderDrawParameters = true;
				if (Extensions.Contains("GL_ARB_indirect_parameters")) ARBIndirectParameters = new(this, context);
				if (Extensions.Contains("GL_ARB_pipeline_statistics_query")) ARBPipelineStatisticsQuery = true;
				if (Extensions.Contains("GL_ARB_transform_feedback_overflow_query")) ARBTransformFeedbackOverflowQuery = true;
				if (Extensions.Contains("GL_ARB_texture_filter_anisotropic")) ARBTextureFilterAnisotropic = true;
				if (Extensions.Contains("GL_ARB_polygon_offset_clamp")) ARBPolygonOffsetClamp = new(this, context);
				if (Extensions.Contains("GL_KHR_no_error")) KHRNoError = true;
				if (Extensions.Contains("GL_ARB_shader_atomic_counter_ops")) ARBShaderAtomicCounterOps = true;
				if (Extensions.Contains("GL_ARB_shader_group_vote")) ARBShaderGroupVote = true;
				hasGL46 =
					ARBGLSPIRV != null &&
					ARBSPIRVExtensions &&
					ARBShaderDrawParameters &&
					ARBIndirectParameters != null &&
					ARBPipelineStatisticsQuery &&
					ARBTransformFeedbackOverflowQuery &&
					ARBTextureFilterAnisotropic &&
					ARBPolygonOffsetClamp != null &&
					KHRNoError &&
					ARBShaderAtomicCounterOps &&
					ARBShaderGroupVote;
			}

			// Initialize GL versions
			if (hasGL46) GL46 = new(this, context);
			if (hasGL45) GL45 = GL46 ?? new GL45(this, context);
			if (hasGL44) GL44 = GL45 ?? new GL44(this, context);
			if (hasGL43) GL43 = GL44 ?? new GL43(this, context);
			if (hasGL42) GL42 = GL43 ?? new GL42(this, context);
			if (hasGL41) GL41 = GL42 ?? new GL41(this, context);
			if (hasGL40) GL40 = GL41 ?? new GL40(this, context);
			if (hasGL33) GL33 = GL40 ?? new GL33(this, context);
			if (hasGL32) GL32 = GL33 ?? new GL32(this, context);
			if (hasGL31) GL31 = GL32 ?? new GL31(this, context);
			if (hasGL30) GL30 = GL31 ?? new GL30(this, context);
			if (hasGL20) GL20 = GL30 ?? new GL20(this, context);
			if (hasGL15) GL15 = GL20 ?? new GL15(this, context);
			if (hasGL14) GL14 = GL15 ?? new GL14(this, context);
			if (hasGL13) GL13 = GL14 ?? new GL13(this, context);
			if (hasGL12) GL12 = GL13 ?? new GL12(this, context);
			GL11 = GL12 ?? new GL11(this, context);

			EXTTextureFilterAnisotropic = Extensions.Contains("GL_EXT_texture_filter_anisotropic");
			if (Extensions.Contains("GL_EXT_depth_bounds_test")) EXTDepthBoundsTest = new(this, context);
			KHRTextureCompressionASTC_HDR = Extensions.Contains("GL_KHR_texture_compression_astc_hdr");
			KHRTextureCompressionASTC_LDR = Extensions.Contains("GL_KHR_texture_compression_astc_ldr");
			EXTTextureCompressionS3TC = Extensions.Contains("GL_EXT_texture_compression_s3tc");
			if (Extensions.Contains("GL_ARB_gpu_shader_int64")) ARBGPUShaderInt64 = new(this, context);
			if (Extensions.Contains("GL_ARB_bindless_texture")) ARBBindlessTexture = new(this, context);

			AMDGPUShaderInt16 = Extensions.Contains("GL_AMD_gpu_shader_int16");

			NVXGPUMemoryInfo = Extensions.Contains("GL_NVX_gpu_memory_info");

			if (Platform.CurrentPlatformType == PlatformType.Windows) {
				WGL = new WGL(this, context);
				if (Extensions.Contains("WGL_AMD_gpu_association")) WGLAMDGPUAssociation = new(this, context);
			}
		}

	}

}
