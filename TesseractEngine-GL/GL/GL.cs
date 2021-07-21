using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.GL.Native;

namespace Tesseract.GL {

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
		public GL GL { get; }

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
		public GL12 GL12 { get; }
		public GL13 GL13 { get; }
		public GL14 GL14 { get; }
		public GL15 GL15 { get; }
		public GL20 GL20 { get; }
		public GL30 GL30 { get; }
		public GL31 GL31 { get; }
		public GL32 GL32 { get; }
		public GL33 GL33 { get; }

		private readonly HashSet<string> extensions = new();
		public IReadOnlySet<string> Extensions => extensions;

		// OpenGL 3.0 Extensions
		// EXT_gpu_shader4
		public EXTGPUShader4 EXTGPUShader4 { get; }
		// NV_conditional_render
		public NVConditionalRender NVConditionalRender { get; }
		// ARB_map_buffer_range - Note: This borrows from APPLE_flush_mapped_buffer BUT unlike what the spec might lead you to believe this is not part of the core
		public ARBMapBufferRange ARBMapBufferRange { get; }
		// ARB_color_buffer_float
		public ARBColorBufferFloat ARBColorBufferFloat { get; }
		// ARB_depth_buffer_float
		public bool ARBDepthBufferFloat { get; }
		// ARB_texture_float
		public bool ARBTextureFloat { get; }
		// EXT_packed_float
		public bool EXTPackedFloat { get; }
		// EXT_texture_shared_exponent
		public bool EXTTextureSharedExponent { get; }
		// ARB_framebuffer_object - Note: This combines EXT_framebuffer_object, EXT_framebuffer_blit, EXT_framebuffer_multisample, and EXT_packed_depth_stencil, and EXT_texture_array
		public ARBFramebufferObject ARBFramebufferObject { get; }
		// ARB_half_float_pixel
		public bool ARBHalfFloatPixel { get; }
		// EXT_texture_integer
		public EXTTextureInteger EXTTextureInteger { get; }
		// EXT_draw_buffers2
		public EXTDrawBuffers2 EXTDrawBuffers2 { get; }
		// EXT_texture_compression_rgtc
		public bool EXTTextureCompressionRGTC { get; }
		// ARB_transform_feedback - Note: *Should* be the same as EXT_transform_feedback
		public ARBTransformFeedback ARBTransformFeedback { get; }
		// ARB_vertex_array_object
		public ARBVertexArrayObject ARBVertexArrayObject { get; }

		// OpenGL 3.1 Extensions
		// ARB_draw_instanced
		public ARBDrawInstanced ARBDrawInstanced { get; }
		// ARB_copy_buffer
		public ARBCopyBuffer ARBCopyBuffer { get; }
		// NV_primitive_restart
		public NVPrimitiveRestart NVPrimitiveRestart { get; }
		// ARB_texture_buffer_object
		public ARBTextureBufferObject ARBTextureBufferObject { get; }
		// ARB_texture_rectangle
		public bool ARBTextureRectangle { get; }
		// ARB_uniform_buffer_object
		public ARBUniformBufferObject ARBUniformBufferObject { get; }

		// OpenGL 3.2 Extensions
		// ARB_compatibility
		public bool ARBCompatibility { get; }
		// ARB_vertex_array_bgra
		public bool ARBVertexArrayBGRA { get; }
		// ARB_draw_elements_base_vertex
		public ARBDrawElementsBaseVertex ARBDrawElementsBaseVertex { get; }
		// ARB_fragment_coord_conventions
		public bool ARBFragmentCoordConventions { get; }
		// ARB_provoking_vertex
		public ARBProvokingVertex ARBProvokingVertex { get; }
		// ARB_seamless_cube_map
		public bool ARBSeamlessCubeMap { get; }
		// ARB_texture_multisample
		public ARBTextureMultisample ARBTextureMultisample { get; }
		// ARB_depth_clamp
		public bool ARBDepthClamp { get; }
		// ARB_geometry_shader4
		public ARBGeometryShader4 ARBGeometryShader4 { get; }
		// ARB_sync
		public ARBSync ARBSync { get; }

		// OpenGL 3.3 Extensions
		// ARB_shader_bit_encoding
		public bool ARBShaderBitEncoding { get; }
		// ARB_blend_func_extended
		public ARBBlendFuncExtended ARBBlendFuncExtended { get; }
		// ARB_explicit_attrib_location
		public bool ARBExplicitAttribLocation { get; }
		// ARB_occlusion_query2
		public bool ARBOcclusionQuery2 { get; }
		// ARB_sampler_objects
		public ARBSamplerObjects ARBSamplerObjects { get; }
		// ARB_texture_rgb10_a2ui
		public bool ARBTextureRGB10A2UI { get; }
		// ARB_texture_swizzle
		public bool ARBTextureSwizzle { get; }
		// ARB_timer_query
		public ARBTimerQuery ARBTimerQuery { get; }
		// ARB_instanced_arrays
		public ARBInstancedArrays ARBInstancedArrays { get; }
		// ARB_vertex_type_2_10_10_10_rev
		public bool ARBVertexType2_10_10_10Rev { get; }

		// OpenGL 4.0 Extensions
		// ARB_texture_query_lod
		// ARB_draw_buffers_blend
		// ARB_draw_indirect
		// ARB_gpu_shader5
		// ARB_gpu_shader_fp64
		// ARB_sample_shading
		// ARB_shader_subroutine
		// ARB_tessellation_shader
		public ARBTessellationShader ARBTessellationShader { get; }
		// ARB_texture_buffer_object_rgb32
		// ARB_texture_cube_map_array
		// ARB_texture_gather
		// ARB_transform_feedback2
		// ARB_transform_feedback3

		// OpenGL 4.1 Extensions
		// ARB_ES2_compatibility
		// ARB_get_program_binary
		// ARB_separate_shader_objects
		// ARB_shader_precision
		// ARB_vertex_attrib_64bit
		// ARB_viewport_array

		// OpenGL 4.2 Extensions
		// ARB_texture_compression_bptc
		// ARB_compressed_texture_pixel_storage
		// ARB_shader_atomic_counters
		// ARB_texture_storage
		// ARB_transform_feedback_instanced
		// ARB_base_instance
		// ARB_shader_image_load_store
		// ARB_conservative_depth
		// ARB_shading_language_420pack
		// ARB_internalformat_query
		// ARB_map_buffer_alignment

		// OpenGL 4.3 Extensions
		// ARB_arrays_of_arrays
		// ARB_ES3_compatibility
		// ARB_clear_buffer_object
		// ARB_compute_shader
		// ARB_copy_image
		// ARB_debug_output
		// ARB_explicit_uniform_location
		// ARB_fragment_layer_viewport
		// ARB_framebuffer_no_attachments
		// ARB_internalformat_query2
		// ARB_invalidate_subdata
		// ARB_multi_draw_indirect
		// ARB_program_interface_query
		// ARB_robust_buffer_access_behavior
		// ARB_shader_image_size
		// ARB_shader_storage_buffer_object
		// ARB_stencil_texturing
		// ARB_texture_buffer_range
		// ARB_texture_query_levels
		// ARB_texture_storage_multisample
		// ARB_texture_view
		// ARB_vertex_attrib_binding
		// KHR_debug

		// OpenGL 4.4 Extensions
		// ARB_buffer_storage
		// ARB_clear_texture
		// ARB_enhanced_layouts
		// ARB_multi_bind
		// ARB_query_buffer_object
		// ARB_texture_mirror_clamp_to_edge
		// ARB_texture_stencil8
		// ARB_vertex_type_10f_11f_11f_rev

		// OpenGL 4.5 Extensions
		// ARB_clip_control
		// ARB_cull_distance
		// ARB_ES3_1_compatibility
		// ARB_conditional_render_inverted
		// KHR_context_flush_control
		// ARB_derivative_control
		// ARB_direct_state_access
		// ARB_get_texture_sub_image
		// KHR_robustness
		// ARB_shader_texture_image_samples
		// ARB_texture_barrier

		// OpenGL 4.6 Extensions
		// ARB_indirect_parameters
		// ARB_pipeline_statistics_query
		// ARB_polygon_offset_clamp
		// KHR_no_error
		// ARB_shader_atomic_counter_ops
		// ARB_shader_draw_parameters
		// ARB_shader_group_vote
		// ARB_gl_spirv
		// ARB_spirv_extensions
		// ARB_texture_filter_anisotropic
		public bool ARBTextureFilterAnisotropic { get; }
		// ARB_transform_feedback_overflow_query

		// Common Extensions
		// EXT_texture_filter_anisotopic - Note: While anisotropic filtering was blessed with ARB status in GL 4.6, *many* vendors support the EXT form in earlier versions
		public bool EXTTextureFilterAnisotropic { get; }
		// EXT_direct_state_access - Note: May be more supported than ARB_direct_state_access on old hardware, but only supports DSA for GL 3.0 and older features 

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
				int nexts = GL11.GetInteger(GLEnums.GL_NUM_EXTENSIONS);
				for (int i = 0; i < nexts; i++) extensions.Add(GL30.GetString(GLEnums.GL_EXTENSIONS, (uint)i));
			} else {
				string extstr = GL11.GetString(GLEnums.GL_EXTENSIONS);
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
				if (Extensions.Contains("GL_EXT_texture_compression_rgtc")) EXTTextureCompressionRGTC = true;
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
					ARBSync != null;
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

			// Initialize GL versions
			if (hasGL33) GL33 = new(this, context);
			if (hasGL32) GL32 = GL33 ?? new GL32(this, context);
			if (hasGL31) GL31 = GL32 ?? new GL31(this, context);
			if (hasGL30) GL30 = GL31 ?? new GL30(this, context);
			if (hasGL20) GL20 = GL30 ?? new GL20(this, context);
			if (hasGL15) GL15 = GL20 ?? new GL15(this, context);
			if (hasGL14) GL14 = GL15 ?? new GL14(this, context);
			if (hasGL13) GL13 = GL14 ?? new GL13(this, context);
			if (hasGL12) GL12 = GL13 ?? new GL12(this, context);
			GL11 = GL12 ?? new GL11(this, context);
		}

	}

}
