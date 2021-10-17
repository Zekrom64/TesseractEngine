using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Graphics.Accelerated;
using Tesseract.Core.Native;
using Tesseract.Core.Util;

namespace Tesseract.Vulkan.Graphics.Impl {

	public class VulkanPipelineLayout : IPipelineLayout {

		public VKPipelineLayout Layout { get; }

		public VulkanPipelineLayout(VKPipelineLayout layout) {
			Layout = layout;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Layout.Dispose();
		}

	}

	public class VulkanPipeline : IPipeline {

		public VKPipeline Pipeline { get; }

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

	public class VulkanPipelineCache : IPipelineCache {

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

	public class VulkanPipelineSet : IPipelineSet {

		public const int HashingThreshold = 5;

		private struct DynamicField {

			public Func<PipelineDynamicCreateInfo, PipelineDynamicCreateInfo, bool> Comparator;

			public Func<PipelineDynamicCreateInfo, PipelineDynamicCreateInfo, PipelineDynamicCreateInfo> Merger;

		}

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
			PipelineDynamicState.DepthTest => new() {
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

		private readonly List<DynamicField> fields = new();

		private (PipelineDynamicCreateInfo, VKPipeline) CreateDerivedPipeline(PipelineDynamicCreateInfo key) {
			PipelineDynamicCreateInfo newDynInfo = BaseInfo.GraphicsInfo.DynamicInfo;
			foreach (var field in fields) newDynInfo = field.Merger(newDynInfo, key);
			PipelineCreateInfo createInfo = BaseInfo with {
				GraphicsInfo = BaseInfo.GraphicsInfo with {
					DynamicInfo = newDynInfo
				},
				BasePipeline = BasePipeline
			};
			return (newDynInfo, ((VulkanPipeline)Graphics.CreatePipeline(createInfo)).Pipeline);
		}

		private class MatchedPipelineSet : IReadOnlyIndexer<PipelineDynamicCreateInfo, VKPipeline>, IDisposable {

			private readonly VulkanPipelineSet pipelineSet;
			private readonly List<(PipelineDynamicCreateInfo, VKPipeline)> pipelines = new();

			public MatchedPipelineSet(VulkanPipelineSet set) {
				pipelineSet = set;
				pipelines.Add((set.BaseInfo.GraphicsInfo.DynamicInfo, set.BasePipeline.Pipeline));
			}

			private bool MatchPipelines(PipelineDynamicCreateInfo c1, PipelineDynamicCreateInfo c2) {
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

		private class HashedPipelineSet : IReadOnlyIndexer<PipelineDynamicCreateInfo, VKPipeline>, IDisposable {

			private readonly VulkanPipelineSet pipelineSet;
			private readonly Dictionary<HashedValue<PipelineDynamicCreateInfo>, VKPipeline> pipelines = new();

			public HashedPipelineSet(VulkanPipelineSet set) {
				pipelineSet = set;
				pipelines[set.BaseInfo.GraphicsInfo.DynamicInfo] = set.BasePipeline.Pipeline;
			}

			public VKPipeline this[PipelineDynamicCreateInfo key] {
				get {
					lock(pipelines) {
						// Try to find an existing matching pipeline
						if (pipelines.TryGetValue(key, out VKPipeline pipeline)) return pipeline;
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

		public IReadOnlyIndexer<PipelineDynamicCreateInfo, VKPipeline> Pipelines;

		public VulkanGraphics Graphics { get; }

		public VulkanPipeline BasePipeline { get; }

		public PipelineCreateInfo BaseInfo { get; }

		public VKPipelineBindPoint BindPoint { get; }

		public VulkanPipelineSet(VulkanGraphics graphics, PipelineSetCreateInfo createInfo) {
			Graphics = graphics;
			BasePipeline = (VulkanPipeline)graphics.CreatePipeline(createInfo.CreateInfo);

			var dynStates = createInfo.CreateInfo.GraphicsInfo.DynamicState;
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