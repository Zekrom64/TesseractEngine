using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Graphics.Accelerated {

	/// <summary>
	/// The command mode determines how commands are forwareded from a command sink.
	/// </summary>
	public enum CommandMode {
		/// <summary>
		/// The effect of the commands is immediate.
		/// </summary>
		Immediate,
		/// <summary>
		/// The commands are stored in a buffer for later submission.
		/// </summary>
		Buffered
	}
	
	/// <summary>
	/// A command sink is an object that can receive commands for accelerated graphics.
	/// </summary>
	public interface ICommandSink {

		public CommandMode Mode { get; }

		// Pipeline state

		/// <summary>
		/// Binds a pipeline to the current 
		/// </summary>
		/// <param name="pipeline"></param>
		public void BindPipeline(IPipeline pipeline);

		/// <summary>
		/// Binds a pipeline from a pipeline set with the given dynamic state.
		/// </summary>
		/// <param name="set">Pipeline set</param>
		/// <param name="state">Dynamic state to bind the pipeline with</param>
		public void BindPipelineWithState(IPipelineSet set, in PipelineDynamicState state);

	}

}
