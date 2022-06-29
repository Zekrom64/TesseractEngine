using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Numerics;
using Tesseract.Core.Util;

namespace Tesseract.OpenGL.Graphics {

	public class GLPipelineLayout : IPipelineLayout {

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

	/// <summary>
	/// <para>
	/// Interface for objects that store an OpenGL shader program.
	/// </para>
	/// <para>
	/// Pipelines store a program object for the shader programs they use. When
	/// created with a pipeline cache the lifetime of this program is managed externally
	/// by the cache, otherwise it is a distinct object which is destroyed with the
	/// pipeline.
	/// </para>
	/// </summary>
	public interface IGLProgram : IDisposable {

		/// <summary>
		/// The ID of the OpenGL program object.
		/// </summary>
		public uint ProgramID { get; }
	
	}

	public class GLPipelineCache : IPipelineCache {

		// The GUID for this version of the pipeline cache binary data
		private static readonly Guid CacheVersionID = new("ba8382bc-a310-4138-aaa0-ac2965d0dd5f");

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		/// <summary>
		/// Cached program object.
		/// </summary>
		internal class CachedProgram : IGLProgram {

			/// <summary>
			/// The cache this program belongs to.
			/// </summary>
			public GLPipelineCache Cache { get; }

			public GL GL => Cache.GL;

			public uint ProgramID { get; private set; }

			/// <summary>
			/// The hash of the program's source code/binary.
			/// </summary>
			public ulong SourceHash { get; }

			// Attempts to make sure that binaryFormat and binary are defined, extracting them from the current program if possible
			private void CheckBinary() {
				var gpb = GL.ARBGetProgramBinary;
				if (binary == null && gpb != null) {
					binary = gpb.GetProgramBinary(ProgramID, out uint format);
					binaryFormat = format;
				}
			}

			private uint? binaryFormat = null;
			/// <summary>
			/// The binary format of the program, or null if unavailable.
			/// </summary>
			public uint? BinaryFormat {
				get {
					CheckBinary();
					return binaryFormat;
				}
			}

			private byte[]? binary = null;
			/// <summary>
			/// The binary of the program, or null if unavailable.
			/// </summary>
			public byte[]? Binary {
				get {
					CheckBinary();
					return binary;
				}
			}

			// Creates a cached program with a binary blob to load from
			internal CachedProgram(GLPipelineCache cache, ulong sourceHash, uint binaryFormat, byte[] binary) {
				Cache = cache;
				SourceHash = sourceHash;
				this.binaryFormat = binaryFormat;
				this.binary = binary;
			}

			// Creates a cached program from a set of shaders
			internal CachedProgram(GLPipelineCache cache, GL gl, IReadOnlyCollection<GLShader> shaders) {
				Cache = cache;
				var gl33 = gl.GL33!;
				ProgramID = gl33.CreateProgram();
				var gpb = gl.ARBGetProgramBinary;
				if (gpb != null)
					gpb.ProgramParameter(ProgramID, GLProgramParameter.BinaryRetrievableHint, 1);
				
				ulong srchash = 0;
				foreach(GLShader shader in shaders) {
					srchash ^= shader.SourceHash;
					gl33.AttachShader(ProgramID, shader.ID);
				}
				SourceHash = srchash;

				gl33.LinkProgram(ProgramID);
				if (gl33.GetProgram(ProgramID, GLGetProgram.LinkStatus) == 0) {
					string log = gl33.GetProgramInfoLog(ProgramID);
					gl33.DeleteProgram(ProgramID);
					throw new GLException("Failed to link shader program, info log: \n" + log);
				}
			}

			public void Dispose() { }

			/// <summary>
			/// Attempts to "checkout" this cached shader, ie. make sure the source hashes match and the program is linked.
			/// </summary>
			/// <param name="shaders">The source shaders to checkout against</param>
			/// <returns>If the checkout was successful</returns>
			public bool TryCheckout(IReadOnlyCollection<GLShader> shaders) {
				ulong srchash = 0;
				foreach (GLShader shader in shaders) srchash ^= shader.SourceHash;
				if (srchash != SourceHash) return false;
				if (ProgramID == 0) {
					var gl33 = GL.GL33!;
					ProgramID = gl33.CreateProgram();

					bool linked = false;

					// If we have GL_ARB_get_program_binary
					var gpb = GL.ARBGetProgramBinary;
					if (gpb != null) {
						// Set the hint that we may retrieve the binary
						gpb.ProgramParameter(ProgramID, GLProgramParameter.BinaryRetrievableHint, 1);
						// If there is a preinitialized binary, attempt to load it
						if (binary != null) {
							gpb.ProgramBinary(ProgramID, binaryFormat!.Value, binary);
							linked = gl33.GetProgram(ProgramID, GLGetProgram.LinkStatus) != 0;
						}
					}

					// If not linked, attempt to link from shader sources.
					if (!linked) {
						foreach (GLShader shader in shaders) gl33.AttachShader(ProgramID, shader.ID);
						gl33.LinkProgram(ProgramID);
						linked = gl33.GetProgram(ProgramID, GLGetProgram.LinkStatus) != 0;
					}

					// If still not linked, give up with an exception
					if (!linked) throw new GLException("Failed to link shader program, info log:\n" + gl33.GetProgramInfoLog(ProgramID));
				}
				return true;
			}

		}

		private readonly Dictionary<ulong, CachedProgram> cache = new();

		public byte[] Data {
			get {
				if (cache == null) return Array.Empty<byte>();
				MemoryStream ms = new();
				BinaryWriter bw = new(ms);
				bw.Write(CacheVersionID.ToByteArray());
				bw.Write(cache.Count);
				foreach(var entry in cache) {
					bw.Write(entry.Key);
					var pipeline = entry.Value;
					bw.Write(pipeline.SourceHash);
					bw.Write(pipeline.BinaryFormat!.Value);
					bw.Write(pipeline.Binary!.Length);
					bw.Write(pipeline.Binary);
				}
				return ms.ToArray();
			}
		}

		public GLPipelineCache(GLGraphics graphics, PipelineCacheCreateInfo createInfo) {
			Graphics = graphics;
			var gpb = GL.ARBGetProgramBinary;
			// We should only initialize the cache if we can actually load program binaries
			if (gpb != null && createInfo.InitialData != null) {
				try {
					MemoryStream ms = new(createInfo.InitialData);
					BinaryReader br = new(ms);
					Guid versionId = new(br.ReadBytes(16));
					if (versionId != CacheVersionID) throw new IOException("Pipeline cache version does not match");
					int n = br.ReadInt32();
					while(n-- > 0) {
						ulong key = br.ReadUInt64();
						uint format = br.ReadUInt32();
						int len = br.ReadInt32();
						byte[] binary = br.ReadBytes(len);
						cache[key] = new(this, key, format, binary);
					}
				} catch (Exception) {
					// We choose to silently fail if we can't load from the binary
					// Just in case, we clear the cache if there is an error
					cache.Clear();
				}
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			var gl33 = GL.GL33!;
			foreach(var entry in cache) {
				uint progr = entry.Value.ProgramID;
				if (progr != 0) gl33.DeleteProgram(progr);
			}
		}
	}

	public class GLPipeline : IPipeline {

		public GLGraphics Graphics { get; }

		public GL GL => Graphics.GL;

		//==============//
		// Static State //
		//==============//

		// Shader state

		public uint ShaderProgramID { get; }

		// Rasterization state

		public bool DepthClampEnable { get; }

		public GLPolygonMode PolygonMode { get; }

		// Color blend state

		public bool LogicOpEnable { get; }

		public struct ColorAttachmentState {

			public bool BlendEnable { get; init; }

			public GLBlendFactor SrcRGB { get; init; }

			public GLBlendFactor DstRGB { get; init; }

			public GLBlendFunction RGBOp { get; init; }

			public GLBlendFactor SrcAlpha { get; init; }

			public GLBlendFactor DstAlpha { get; init; }

			public GLBlendFunction AlphaOp { get; init; }

			public ColorComponent ColorWriteMask { get; init; }

		}

		public ColorAttachmentState[] Attachments { get; }

		//===============//
		// Dynmaic state //
		//===============//

		// Input assembly state

		public GLDrawMode? DrawMode { get; }

		public bool? PrimitiveRestartEnable { get; }

		// Tessellation state

		public uint? PatchControlPoints { get; }

		// Viewport state

		public Viewport[]? Viewports { get; }

		public Recti[]? Scissors { get; }

		// Rasterization state

		public bool? RasterizerDiscardEnable { get; }

		public record struct Culling {

			public bool Enable;

			public GLFace CullFace;

		}

		public Culling? CullMode { get; }
		
		public GLCullFace? FrontFace { get; }

		public float? LineWidth { get; }

		public bool? DepthBiasEnable { get; }

		public record struct DepthBiasFactors {

			public float ConstantFactor { get; init; }

			public float Clamp { get; init; }

			public float SlopeFactor { get; init; }

		}

		public DepthBiasFactors? DepthBias { get; }

		// Depth/stencil state

		public bool? DepthTestEnable { get; }

		public bool? DepthWriteEnable { get; }

		public GLCompareFunc? DepthCompareOp { get; }

		public bool? DepthBoundsTestEnable { get; }

		public bool? StencilTestEnable { get; }

		public record struct StencilOpState {

			public GLStencilOp FailOp { get; init; }

			public GLStencilOp PassOp { get; init; }

			public GLStencilOp DepthFailOp { get; init; }

			public GLStencilFunc CompareOp { get; init; }

		}

		public (StencilOpState, StencilOpState)? StencilOp { get; }

		public (uint, uint)? StencilCompareMask { get; }

		public (uint, uint)? StencilWriteMask { get; }

		public (int, int)? StencilReference { get; }

		public (float, float)? DepthBounds { get; }

		public GLLogicOp? LogicOp { get; }

		public Vector4? BlendConstant { get; }

		public bool[]? ColorWriteEnable { get; }

		public GLPipeline(GLGraphics graphics, PipelineCreateInfo createInfo) {
			Graphics = graphics;
			GL33 gl33 = GL.GL33!;

			ShaderProgramID = gl33.CreateProgram();
			bool hasTess = false;
			foreach(var shaderInfo in createInfo.GraphicsInfo!.Shaders) {
				GLShader shader = (GLShader)shaderInfo.Shader;
				switch(shader.Type) {
					case GLShaderType.TessellationControl:
					case GLShaderType.TessellationEvaluation:
						hasTess = true;
						break;
				}
				gl33.AttachShader(ShaderProgramID, shader.ID);
			}
			gl33.LinkProgram(ShaderProgramID);
			if (gl33.GetProgram(ShaderProgramID, GLGetProgram.LinkStatus) != Native.GLEnums.GL_TRUE) {
				string log = gl33.GetProgramInfoLog(ShaderProgramID);
				throw new GLException("Failed to link shader program:\n" + log);
			}

			var gfxInfo = createInfo.GraphicsInfo!;
			DepthClampEnable = gfxInfo.DepthClampEnable;
			PolygonMode = GLEnums.Convert(gfxInfo.PolygonMode);

			LogicOpEnable = gfxInfo.LogicOpEnable;
			Attachments = gfxInfo.Attachments.ToArray().ConvertAll(attachment => new ColorAttachmentState() {
				BlendEnable = attachment.BlendEnable,
				SrcRGB = GLEnums.Convert(attachment.BlendEquation.SrcRGB),
				SrcAlpha = GLEnums.Convert(attachment.BlendEquation.SrcAlpha),
				DstRGB = GLEnums.Convert(attachment.BlendEquation.DstRGB),
				DstAlpha = GLEnums.Convert(attachment.BlendEquation.DstAlpha),
				RGBOp = GLEnums.Convert(attachment.BlendEquation.RGBOp),
				AlphaOp = GLEnums.Convert(attachment.BlendEquation.AlphaOp),
				ColorWriteMask = attachment.ColorWriteMask
			});

			var dynInfo = gfxInfo.DynamicInfo;
			var dynState = gfxInfo.DynamicState;

			DrawMode = dynState.Contains(PipelineDynamicState.DrawMode) ? null : GLEnums.Convert(dynInfo.DrawMode);
			PrimitiveRestartEnable = dynState.Contains(PipelineDynamicState.PrimitiveRestartEnable) ? null : dynInfo.PrimitiveRestartEnable;

			PatchControlPoints = !hasTess || dynState.Contains(PipelineDynamicState.PatchControlPoints) ? null : dynInfo.PatchControlPoints;

			Viewports = dynState.Contains(PipelineDynamicState.Viewport) ? null : dynInfo.Viewports.ToArray();
			Scissors = dynState.Contains(PipelineDynamicState.Scissor) ? null : dynInfo.Scissors.ToArray();

			RasterizerDiscardEnable = dynState.Contains(PipelineDynamicState.RasterizerDiscardEnable) ? null : dynInfo.RasterizerDiscardEnable;
			if (dynState.Contains(PipelineDynamicState.CullMode)) CullMode = null;
			else {
				var face = GLEnums.Convert(dynInfo.CullMode, out bool cullModeEnable);
				CullMode = new Culling() { Enable = cullModeEnable, CullFace = face };
			}
			FrontFace = dynState.Contains(PipelineDynamicState.FrontFace) ? null : GLEnums.Convert(dynInfo.FrontFace);
			LineWidth = dynState.Contains(PipelineDynamicState.LineWidth) ? null : dynInfo.LineWidth;
			DepthBiasEnable = dynState.Contains(PipelineDynamicState.DepthBiasEnable) ? null : dynInfo.DepthBiasEnable;
			if (!dynState.Contains(PipelineDynamicState.DepthBias)) {
				DepthBias = new() {
					ConstantFactor = dynInfo.DepthBiasConstantFactor,
					SlopeFactor = dynInfo.DepthBiasSlopeFactor,
					Clamp = dynInfo.DepthBiasClamp
				};
			} else DepthBias = null;

			DepthTestEnable = dynState.Contains(PipelineDynamicState.DepthTestEnable) ? null : dynInfo.DepthTestEnable;
			DepthWriteEnable = dynState.Contains(PipelineDynamicState.DepthWriteEnable) ? null : dynInfo.DepthWriteEnable;
			DepthCompareOp = dynState.Contains(PipelineDynamicState.DepthCompareOp) ? null : GLEnums.Convert(dynInfo.DepthCompareOp);
			DepthBoundsTestEnable = dynState.Contains(PipelineDynamicState.DepthBoundsTestEnable) ? null : dynInfo.DepthBoundsTestEnable;
			StencilTestEnable = dynState.Contains(PipelineDynamicState.StencilTestEnable) ? null : dynInfo.StencilTestEnable;
			if (!dynState.Contains(PipelineDynamicState.StencilOp)) {
				StencilOp = (
					new StencilOpState() {
						PassOp = GLEnums.Convert(dynInfo.FrontStencilState.PassOp),
						FailOp = GLEnums.Convert(dynInfo.FrontStencilState.FailOp),
						DepthFailOp = GLEnums.Convert(dynInfo.FrontStencilState.DepthFailOp),
						CompareOp = GLEnums.ConvertStencilFunc(dynInfo.FrontStencilState.CompareOp)
					},
					new StencilOpState() {
						PassOp = GLEnums.Convert(dynInfo.BackStencilState.PassOp),
						FailOp = GLEnums.Convert(dynInfo.BackStencilState.FailOp),
						DepthFailOp = GLEnums.Convert(dynInfo.BackStencilState.DepthFailOp),
						CompareOp = GLEnums.ConvertStencilFunc(dynInfo.BackStencilState.CompareOp)
					}
				);
			} else StencilOp = null;
			StencilCompareMask = dynState.Contains(PipelineDynamicState.StencilCompareMask) ? null : (dynInfo.FrontStencilState.CompareMask, dynInfo.BackStencilState.CompareMask);
			StencilWriteMask = dynState.Contains(PipelineDynamicState.StencilWriteMask) ? null : (dynInfo.FrontStencilState.WriteMask, dynInfo.BackStencilState.WriteMask);
			StencilReference = dynState.Contains(PipelineDynamicState.StencilReference) ? null : ((int)dynInfo.FrontStencilState.Reference, (int)dynInfo.BackStencilState.Reference);
			DepthBounds = dynState.Contains(PipelineDynamicState.DepthBounds) ? null : dynInfo.DepthBounds;
			LogicOp = dynState.Contains(PipelineDynamicState.LogicOp) ? null : GLEnums.Convert(dynInfo.LogicOp);
			BlendConstant = dynState.Contains(PipelineDynamicState.BlendConstants) ? null : dynInfo.BlendConstant;
			ColorWriteEnable = dynState.Contains(PipelineDynamicState.ColorWrite) ? null : dynInfo.ColorWriteEnable.ToArray();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			GL.GL33!.DeleteProgram(ShaderProgramID);
		}

	}

	public class GLPipelineSet : IPipelineSet {

		public GLGraphics Graphics { get; }

		public GLPipeline BasePipeline { get; }

		private readonly bool[] varStates = new bool[EnumUtils.GetMaxValue<PipelineDynamicState>()];

		public GLPipelineSet(GLGraphics graphics, PipelineSetCreateInfo createInfo) {
			Graphics = graphics;
			PipelineCreateInfo graphicsInfo = createInfo.CreateInfo;
			// We combine all dynamic states since OpenGL's pipeline is inherently dynamic
			// When binding with this set we simply bind the base pipeline and then set
			//   dynamic state based on the set variables
			HashSet<PipelineDynamicState> dynStates = new();
			dynStates.AddAll(createInfo.CreateInfo.GraphicsInfo!.DynamicState);
			dynStates.AddAll(createInfo.VariableStates);
			foreach (PipelineDynamicState state in createInfo.VariableStates) varStates[(int)state] = true;
			graphicsInfo = graphicsInfo with {
				GraphicsInfo = graphicsInfo.GraphicsInfo! with {
					DynamicState = dynStates
				}
			};
			BasePipeline = new GLPipeline(graphics, graphicsInfo);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsVariable(PipelineDynamicState state) => varStates[(int)state];

		public void Dispose() {
			GC.SuppressFinalize(this);
		}

	}

}
