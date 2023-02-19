using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Collections;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Utilities;

namespace Tesseract.Vulkan.Services.Objects {

    /// <summary>
    /// Vulkan implementation for a pipeline layout.
    /// </summary>
    public class VulkanPipelineLayout : IPipelineLayout {

		/// <summary>
		/// The underlying Vulkan pipeline layout.
		/// </summary>
		public VKPipelineLayout Layout { get; }

		public VulkanPipelineLayout(VKPipelineLayout layout) {
			Layout = layout;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Layout.Dispose();
		}

	}

	/// <summary>
	/// Vulkan implementation for a pipeline.
	/// </summary>
	public class VulkanPipeline : IPipeline {

		/// <summary>
		/// The underlying Vulkan pipeline.
		/// </summary>
		public VKPipeline Pipeline { get; }

		/// <summary>
		/// The bind point for this pipeline.
		/// </summary>
		public VKPipelineBindPoint BindPoint { get; }

		public VulkanPipeline(VKPipeline pipeline, VKPipelineBindPoint bindPoint) {
			Pipeline = pipeline;
			BindPoint = bindPoint;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Pipeline.Dispose();
		}

	}

	/// <summary>
	/// Vulkan implementation of a pipeline cache.
	/// </summary>
	public class VulkanPipelineCache : IPipelineCache {

		/// <summary>
		/// The underlying Vulkan pipeline cache.
		/// </summary>
		public VKPipelineCache PipelineCache { get; }

		public byte[] Data => PipelineCache.Data;

		public VulkanPipelineCache(VKPipelineCache cache) {
			PipelineCache = cache;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			PipelineCache.Dispose();
		}

	}

	/// <summary>
	/// Vulkan implementation for a pipeline set.
	/// </summary>
	public class VulkanPipelineSet : IPipelineSet {

		// The threshold for variables above which 
		private const int HashingThreshold = 5;

		// A dynamic field is a variable used 
		private struct DynamicField {

			// A comparator function which determines if this field is the same between two dynamic infos
			public Func<PipelineDynamicCreateInfo, PipelineDynamicCreateInfo, bool> Comparator;

			// A merger function which merges just this field's value into a dynamic info
			public Func<PipelineDynamicCreateInfo, PipelineDynamicCreateInfo, PipelineDynamicCreateInfo> Merger;

		}

		// Converts a dynamic state to a dynamic field.
		private static DynamicField MakeField(PipelineDynamicState state) => state switch {
			PipelineDynamicState.Viewport => new() {
				Comparator = (p1, p2) => p1.Viewports == p2.Viewports,
				Merger = (pbase, pnew) => pbase with { Viewports = pnew.Viewports }
			},
			PipelineDynamicState.Scissor => new() {
				Comparator = (p1, p2) => p1.Scissors == p2.Scissors,
				Merger = (pbase, pnew) => pbase with { Scissors = pnew.Scissors }
			},
			PipelineDynamicState.LineWidth => new() {
				Comparator = (p1, p2) => p1.LineWidth == p2.LineWidth,
				Merger = (pbase, pnew) => pbase with { LineWidth = pnew.LineWidth }
			},
			PipelineDynamicState.DepthBias => new() {
				Comparator = (p1, p2) =>
					p1.DepthBiasClamp == p2.DepthBiasClamp &&
					p1.DepthBiasConstantFactor == p2.DepthBiasConstantFactor &&
					p1.DepthBiasSlopeFactor == p2.DepthBiasSlopeFactor,
				Merger = (pbase, pnew) => pbase with {
					DepthBiasClamp = pnew.DepthBiasClamp,
					DepthBiasConstantFactor = pnew.DepthBiasConstantFactor,
					DepthBiasSlopeFactor = pnew.DepthBiasSlopeFactor
				}
			},
			PipelineDynamicState.BlendConstants => new() {
				Comparator = (p1, p2) => p1.BlendConstant == p2.BlendConstant,
				Merger = (pbase, pnew) => pbase with { BlendConstant = pnew.BlendConstant }
			},
			PipelineDynamicState.DepthBounds => new() {
				Comparator = (p1, p2) => p1.Scissors == p2.Scissors,
				Merger = (pbase, pnew) => pbase with { Scissors = pnew.Scissors }
			},
			PipelineDynamicState.StencilCompareMask => new() {
				Comparator = (p1, p2) =>
					p1.FrontStencilState.CompareMask == p2.FrontStencilState.CompareMask &&
					p1.BackStencilState.CompareMask == p2.BackStencilState.CompareMask,
				Merger = (pbase, pnew) => pbase with {
					FrontStencilState = pbase.FrontStencilState with { CompareMask = pnew.FrontStencilState.CompareMask },
					BackStencilState = pbase.BackStencilState with { CompareMask = pnew.BackStencilState.CompareMask }
				}
			},
			PipelineDynamicState.StencilWriteMask => new() {
				Comparator = (p1, p2) =>
					p1.FrontStencilState.WriteMask == p2.FrontStencilState.WriteMask &&
					p1.BackStencilState.WriteMask == p2.BackStencilState.WriteMask,
				Merger = (pbase, pnew) => pbase with {
					FrontStencilState = pbase.FrontStencilState with { WriteMask = pnew.FrontStencilState.WriteMask },
					BackStencilState = pbase.BackStencilState with { WriteMask = pnew.BackStencilState.WriteMask }
				}
			},
			PipelineDynamicState.StencilReference => new() {
				Comparator = (p1, p2) =>
					p1.FrontStencilState.Reference == p2.FrontStencilState.Reference &&
					p1.BackStencilState.Reference == p2.BackStencilState.Reference,
				Merger = (pbase, pnew) => pbase with {
					FrontStencilState = pbase.FrontStencilState with { Reference = pnew.FrontStencilState.Reference },
					BackStencilState = pbase.BackStencilState with { Reference = pnew.BackStencilState.Reference }
				}
			},
			PipelineDynamicState.CullMode => new() {
				Comparator = (p1, p2) => p1.CullMode == p2.CullMode,
				Merger = (pbase, pnew) => pbase with { CullMode = pnew.CullMode }
			},
			PipelineDynamicState.FrontFace => new() {
				Comparator = (p1, p2) => p1.FrontFace == p2.FrontFace,
				Merger = (pbase, pnew) => pbase with { FrontFace = pnew.FrontFace }
			},
			PipelineDynamicState.DrawMode => new() {
				Comparator = (p1, p2) => p1.DrawMode == p2.DrawMode,
				Merger = (pbase, pnew) => pbase with { DrawMode = pnew.DrawMode }
			},
			PipelineDynamicState.DepthTestEnable => new() {
				Comparator = (p1, p2) => p1.Scissors == p2.Scissors,
				Merger = (pbase, pnew) => pbase with { Scissors = pnew.Scissors }
			},
			PipelineDynamicState.DepthWriteEnable => new() {
				Comparator = (p1, p2) => p1.DepthWriteEnable == p2.DepthWriteEnable,
				Merger = (pbase, pnew) => pbase with { DepthWriteEnable = pnew.DepthWriteEnable }
			},
			PipelineDynamicState.DepthCompareOp => new() {
				Comparator = (p1, p2) => p1.DepthCompareOp == p2.DepthCompareOp,
				Merger = (pbase, pnew) => pbase with { DepthCompareOp = pnew.DepthCompareOp }
			},
			PipelineDynamicState.DepthBoundsTestEnable => new() {
				Comparator = (p1, p2) => p1.DepthBoundsTestEnable == p2.DepthBoundsTestEnable,
				Merger = (pbase, pnew) => pbase with { DepthBoundsTestEnable = pnew.DepthBoundsTestEnable }
			},
			PipelineDynamicState.StencilTestEnable => new() {
				Comparator = (p1, p2) => p1.Scissors == p2.Scissors,
				Merger = (pbase, pnew) => pbase with { Scissors = pnew.Scissors }
			},
			PipelineDynamicState.StencilOp => new() {
				Comparator = (p1, p2) =>
					p1.FrontStencilState.CompareOp == p2.FrontStencilState.CompareOp &&
					p1.FrontStencilState.PassOp == p2.FrontStencilState.PassOp &&
					p1.FrontStencilState.FailOp == p2.FrontStencilState.FailOp &&
					p1.FrontStencilState.DepthFailOp == p2.FrontStencilState.DepthFailOp &&
					p1.BackStencilState.CompareOp == p2.BackStencilState.CompareOp &&
					p1.BackStencilState.PassOp == p2.BackStencilState.PassOp &&
					p1.BackStencilState.FailOp == p2.BackStencilState.FailOp &&
					p1.BackStencilState.DepthFailOp == p2.BackStencilState.DepthFailOp,
				Merger = (pbase, pnew) => pbase with { Scissors = pnew.Scissors }
			},
			PipelineDynamicState.PatchControlPoints => new() {
				Comparator = (p1, p2) => p1.PatchControlPoints == p2.PatchControlPoints,
				Merger = (pbase, pnew) => pbase with { PatchControlPoints = pnew.PatchControlPoints }
			},
			PipelineDynamicState.RasterizerDiscardEnable => new() {
				Comparator = (p1, p2) => p1.RasterizerDiscardEnable == p2.RasterizerDiscardEnable,
				Merger = (pbase, pnew) => pbase with { RasterizerDiscardEnable = pnew.RasterizerDiscardEnable }
			},
			PipelineDynamicState.DepthBiasEnable => new() {
				Comparator = (p1, p2) => p1.DepthBiasEnable == p2.DepthBiasEnable,
				Merger = (pbase, pnew) => pbase with { DepthBiasEnable = pnew.DepthBiasEnable }
			},
			PipelineDynamicState.LogicOp => new() {
				Comparator = (p1, p2) => p1.LogicOp == p2.LogicOp,
				Merger = (pbase, pnew) => pbase with { LogicOp = pnew.LogicOp }
			},
			PipelineDynamicState.PrimitiveRestartEnable => new() {
				Comparator = (p1, p2) => p1.PrimitiveRestartEnable == p2.PrimitiveRestartEnable,
				Merger = (pbase, pnew) => pbase with { PrimitiveRestartEnable = pnew.PrimitiveRestartEnable }
			},
			PipelineDynamicState.VertexFormat => new() {
				Comparator = (p1, p2) => p1.VertexFormat == p2.VertexFormat,
				Merger = (pbase, pnew) => pbase with { VertexFormat = pnew.VertexFormat }
			},
			PipelineDynamicState.ColorWrite => new() {
				Comparator = (p1, p2) => p1.ColorWriteEnable == p2.ColorWriteEnable,
				Merger = (pbase, pnew) => pbase with { ColorWriteEnable = pnew.ColorWriteEnable }
			},
			PipelineDynamicState.ViewportCount => new() {
				Comparator = (p1, p2) => p1.Viewports == p2.Viewports,
				Merger = (pbase, pnew) => pbase with { Viewports = pnew.Viewports }
			},
			PipelineDynamicState.ScissorCount => new() {
				Comparator = (p1, p2) => p1.Scissors == p2.Scissors,
				Merger = (pbase, pnew) => pbase with { Scissors = pnew.Scissors }
			},
			_ => default
		};

		// The list of pipeline fields to use
		private readonly List<DynamicField> fields = new();

		// Creates a new derived pipeline using the given dynamic info
		private (PipelineDynamicCreateInfo, VKPipeline) CreateDerivedPipeline(PipelineDynamicCreateInfo key) {
			PipelineDynamicCreateInfo newDynInfo = BaseInfo.GraphicsInfo!.DynamicInfo;
			foreach (var field in fields) newDynInfo = field.Merger(newDynInfo, key);
			PipelineCreateInfo createInfo = BaseInfo with {
				GraphicsInfo = BaseInfo.GraphicsInfo with {
					DynamicInfo = newDynInfo
				},
				BasePipeline = BasePipeline
			};
			return (newDynInfo, ((VulkanPipeline)Graphics.CreatePipeline(createInfo)).Pipeline);
		}

		// A pipeline set implementation that seeks to match pipelines from a list by their dynamic info
		private class MatchedPipelineSet : IReadOnlyIndexer<PipelineDynamicCreateInfo, VKPipeline>, IDisposable {

			// The pipeline set using this implementation
			private readonly VulkanPipelineSet pipelineSet;
			// The list of all created pipelines 
			private readonly List<(PipelineDynamicCreateInfo, VKPipeline)> pipelines = new();

			public MatchedPipelineSet(VulkanPipelineSet set) {
				pipelineSet = set;
				pipelines.Add((set.BaseInfo.GraphicsInfo!.DynamicInfo, set.BasePipeline.Pipeline));
			}

			private bool MatchPipelines(PipelineDynamicCreateInfo c1, PipelineDynamicCreateInfo c2) {
				// All dynamic fields must match
				foreach (DynamicField field in pipelineSet.fields) if (!field.Comparator(c1, c2)) return false;
				return true;
			}

			public VKPipeline this[PipelineDynamicCreateInfo key] {
				get {
					lock(pipelines) {
						// Try to find an existing matching pipeline
						foreach(var pipeline in pipelines) if (MatchPipelines(key, pipeline.Item1)) return pipeline.Item2;
						// Else create a new derived pipeline
						var vkpipeline = pipelineSet.CreateDerivedPipeline(key);
						pipelines.Add(vkpipeline);
						return vkpipeline.Item2;
					}
				}
			}

			public void Dispose() {
				GC.SuppressFinalize(this);
				foreach (var pipeline in pipelines) pipeline.Item2.Dispose();
			}

		}

		// A pipeline set implementation that seeks to map pipelines by their dynamic info's hash
		private class HashedPipelineSet : IReadOnlyIndexer<PipelineDynamicCreateInfo, VKPipeline>, IDisposable {

			private readonly VulkanPipelineSet pipelineSet;
			private readonly Dictionary<HashedValue<PipelineDynamicCreateInfo>, VKPipeline> pipelines = new();

			public HashedPipelineSet(VulkanPipelineSet set) {
				pipelineSet = set;
				pipelines[set.BaseInfo.GraphicsInfo!.DynamicInfo] = set.BasePipeline.Pipeline;
			}

			public VKPipeline this[PipelineDynamicCreateInfo key] {
				get {
					lock(pipelines) {
						// Try to find an existing matching pipeline
						if (pipelines.TryGetValue(key, out VKPipeline? pipeline)) return pipeline;
						// Else create a new derived pipeline
						var vkpipeline = pipelineSet.CreateDerivedPipeline(key);
						pipelines[vkpipeline.Item1] = vkpipeline.Item2;
						return vkpipeline.Item2;
					}
				}
			}

			public void Dispose() {
				GC.SuppressFinalize(this);
				foreach (VKPipeline pipeline in pipelines.Values) pipeline.Dispose();
			}

		}

		/// <summary>
		/// Indexer function from dynamic pipeline information to corresponding pipelines.
		/// </summary>
		public IReadOnlyIndexer<PipelineDynamicCreateInfo, VKPipeline> Pipelines;

		/// <summary>
		/// The graphics context this set was created from.
		/// </summary>
		public VulkanGraphics Graphics { get; }

		/// <summary>
		/// The base pipeline for this set's pipelines.
		/// </summary>
		public VulkanPipeline BasePipeline { get; }

		/// <summary>
		/// The base creation info for this set's pipelines.
		/// </summary>
		public PipelineCreateInfo BaseInfo { get; }

		/// <summary>
		/// The bind point for pipelines from this set.
		/// </summary>
		public VKPipelineBindPoint BindPoint { get; }

		public VulkanPipelineSet(VulkanGraphics graphics, PipelineSetCreateInfo createInfo) {
			Graphics = graphics;
			BasePipeline = (VulkanPipeline)graphics.CreatePipeline(createInfo.CreateInfo);
			BaseInfo = createInfo.CreateInfo;
			BindPoint = BaseInfo.GraphicsInfo != null ? VKPipelineBindPoint.Graphics : VKPipelineBindPoint.Compute;

			var dynStates = createInfo.CreateInfo.GraphicsInfo!.DynamicState;
			HashSet<PipelineDynamicState> varStates = new(createInfo.VariableStates);
			varStates.RemoveWhere(state => dynStates.Contains(state));
			foreach (var state in varStates) fields.Add(MakeField(state));

			if (varStates.Count > HashingThreshold) {
				Pipelines = new HashedPipelineSet(this);
			} else {
				Pipelines = new MatchedPipelineSet(this);
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			BasePipeline.Dispose();
			if (Pipelines is IDisposable d) d.Dispose();
		}

	}
}