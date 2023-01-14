using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Engine.Input {

	/// <summary>
	/// An input source provides a value which can be read from a certain input (button, joystick, etc.). The input source
	/// can either be polled or fire a event when the input changes.
	/// </summary>
	/// <typeparam name="T">The type of input value provided by the source</typeparam>
	public interface IInputSource<T> where T : struct {

		/// <summary>
		/// The human-readable name of the input source.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The unique ID of the input source.
		/// </summary>
		public string ID { get; }

		/// <summary>
		/// The current value of the input source. This may not reveal all changes in the value if
		/// polled infrequently, the <see cref="OnChange"/> event will be fired every time the value
		/// changes.
		/// </summary>
		public T CurrentValue { get; }

		/// <summary>
		/// Event fired when the value of the source changes, passing the new value.
		/// </summary>
		public event Action<T>? OnChange;

	}

	/// <summary>
	/// A null input source provides a constant 'null' value for an input.
	/// </summary>
	/// <typeparam name="T">Input value type</typeparam>
	public class NullInputSource<T> : IInputSource<T> where T : struct {

		/// <summary>
		/// The default null source instance.
		/// </summary>
		public static NullInputSource<T> Instance { get; } = new();

		public string Name => "None";

		public string ID => "null";

		public T CurrentValue { get; }

		public NullInputSource(T value = default) {
			CurrentValue = value;
		}

		public event Action<T>? OnChange;

	}

	/*
	/// <summary>
	/// Source which converts a boolean value to a linear value. This can be used
	/// as a way of substituting button inputs for a linear trigger or axial input.
	/// </summary>
	public class BoolToLinearInputSource : IInputSource<float>, IDisposable {

		/// <summary>
		/// The parent source used for the conversion.
		/// </summary>
		public IInputSource<bool> ParentSource { get; }

		public string Name => ParentSource.Name;

		public string ID => ParentSource.ID;

		public float CurrentValue => ParentSource.CurrentValue ? 1 : 0;

		public BoolToLinearInputSource(IInputSource<bool> parentSource) {
			ParentSource = parentSource;
			parentSource.OnChange += ParentChanged;
		}

		public event Action<float>? OnChange;

		public void Dispose() {
			GC.SuppressFinalize(this);
			ParentSource.OnChange -= ParentChanged;
		}

		private void ParentChanged(bool val) => OnChange?.Invoke(val ? 1 : 0);

	}
	*/

}
