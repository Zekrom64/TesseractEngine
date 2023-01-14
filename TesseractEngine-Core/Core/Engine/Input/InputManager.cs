using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;

namespace Tesseract.Core.Engine.Input {

	/// <summary>
	/// The input manager controls how inputs are received and mapped.
	/// </summary>
	public class InputManager : IEngineObject {

		public TesseractEngine Engine { get; }

		// The list of registered mappings
		private readonly List<IInputMapping> mappings = new();

		/// <summary>
		/// The handler for boolean inputs.
		/// </summary>
		public BoolInputHandler BoolInputs { get; }

		// Generic input handler for float inputs.
		private readonly InputHandler<float> floatInputs = new();

		/// <summary>
		/// The handler for 2-component float vector inputs.
		/// </summary>
		public Vector2InputHandler Vector2Inputs { get; }

		// List of controller handlers by player index
		internal readonly List<ControllerHandler> Controllers = new();

		internal InputManager(TesseractEngine engine) {
			Engine = engine;
			BoolInputs = new BoolInputHandler(engine);
			Vector2Inputs = new Vector2InputHandler(engine);
		}

		/// <summary>
		/// Adds an input mapping to the manager.
		/// </summary>
		/// <param name="mapping"></param>
		/// <returns></returns>
		public InputManager AddMapping(IInputMapping mapping) {
			mappings.Add(mapping);
			return this;
		}

		/// <summary>
		/// Gets the controller at the given player index.
		/// </summary>
		/// <param name="index">Player index to get</param>
		/// <returns>The controller at the given player index</returns>
		public ControllerHandler GetController(int index) {
			lock(Controllers) {
				if (Controllers.Count <= index) {
					for (int i = Controllers.Count; i <= index; i++) Controllers.Add(new ControllerHandler(this, i));
				}
				return Controllers[index];
			}
		}

		/// <summary>
		/// Gets the associated handler for a specific type of input.
		/// </summary>
		/// <typeparam name="T">The input value type</typeparam>
		/// <returns>The handler of inputs of the given type</returns>
		public InputHandler<T> GetHandler<T>() where T : struct {
			if (typeof(T) == typeof(bool)) return (InputHandler<T>)(object)BoolInputs;
			if (typeof(T) == typeof(float)) return (InputHandler<T>)(object)floatInputs;
			if (typeof(T) == typeof(Vector2)) return (InputHandler<T>)(object)Vector2Inputs;
			else return InputHandler<T>.Empty;
		}

		/// <summary>
		/// Gets the available input sources for the given type of input (ie. <tt>bool</tt>, <tt>float</tt>, etc.). This list may
		/// not be entirely accurate for some input sources which cannot be fully enumerated until an input is provided for them.
		/// The returned list may be modified if new sources are added.
		/// </summary>
		/// <typeparam name="T">The input value type</typeparam>
		/// <returns>Enumeration of input sources</returns>
		public IReadOnlyList<IInputSource<T>> GetSources<T>() where T : struct => GetHandler<T>().Sources;

		/// <summary>
		/// Awaits the next input from a source that provides the given type of input.
		/// </summary>
		/// <typeparam name="T">The input value type</typeparam>
		/// <returns>Task that will complete when an input of the given type is fired</returns>
		public Task<IInputSource<T>> AwaitSource<T>() where T : struct {
			var handler = GetHandler<T>();
			TaskCompletionSource<IInputSource<T>> tcs = new();
			Action<IInputSource<T>> callback = null!;
			callback = source => {
				tcs.SetResult(source);
				handler.SourceFired -= callback;
			};
			handler.SourceFired += callback;
			return tcs.Task;
		}

	}

}
