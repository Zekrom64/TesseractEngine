using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Input;

namespace Tesseract.Core.Engine.Input {

	/// <summary>
	/// An input mapping connects one or more input sources to a 
	/// </summary>
	public interface IInputMapping : IDisposable {

		public string Name { get; }

		public string ID { get; }

		public void Reset();

	}

	/// <summary>
	/// A boolean input mapping that is intended to map a keyboard or joystick key to an input action.
	/// </summary>
	public class ButtonInputMapping : IInputMapping {

		/// <summary>
		/// The current state of the button input.
		/// </summary>
		public bool State { get; private set; }

		private IInputSource<bool> trigger = NullInputSource<bool>.Instance;
		/// <summary>
		/// The input source that will trigger this mapping.
		/// </summary>
		public IInputSource<bool> TriggerSource {
			get => trigger;
			set {
				trigger.OnChange -= Triggered;
				trigger = value;
				Triggered(trigger.CurrentValue);
				trigger.OnChange += Triggered;
			}
		}

		/// <summary>
		/// The list of input sources that must be active for this mapping to trigger.
		/// </summary>
		public IList<IInputSource<bool>> ModifierSources { get; set; } = new List<IInputSource<bool>>();

		/// <summary>
		/// A "gate" function which must be active for this mapping to trigger.
		/// </summary>
		public Func<bool> GateFunction { get; set; } = () => true;

		public required string Name { get; init; }

		public required string ID { get; init; }

		/// <summary>
		/// Event fired when the mapping is triggered.
		/// </summary>
		public event Action? OnPress;

		/// <summary>
		/// Event fired hen the trigger source is released.
		/// </summary>
		public event Action? OnRelease;

		private void Triggered(bool value) {
			if (value) {
				if (!GateFunction()) return;
				foreach (var mod in ModifierSources)
					if (!mod.CurrentValue) return;
				State = true;
				OnPress?.Invoke();
			} else {
				State = false;
				OnRelease?.Invoke();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			trigger.OnChange -= Triggered;
		}

		public void Reset() {
			TriggerSource = NullInputSource<bool>.Instance;
			ModifierSources.Clear();
			State = false;
		}

	}

	/// <summary>
	/// A linear input mapping maps to a one-axis input such as a joystick trigger.
	/// </summary>
	public class LinearInputMapping : IInputMapping {

		/// <summary>
		/// The most recent change in the state.
		/// </summary>
		public float Delta { get; private set; }

		/// <summary>
		/// The current state of the linear input.
		/// </summary>
		public float State { get; private set; }

		private IInputSource<float> source = NullInputSource<float>.Instance;
		public IInputSource<float> Source {
			get => source;
			set {
				source.OnChange -= Changed;
				source = value;
				Changed(source.CurrentValue);
				source.OnChange += Changed;
			}
		}

		public required string Name { get; init; }

		public required string ID { get; init; }

		public event Action? OnChange;

		private void Changed(float value) {
			if (value != State) {
				Delta = value - State;
				State = value;
				OnChange?.Invoke();
			}
		}

		public void Reset() {
			Source = NullInputSource<float>.Instance;
			State = Source.CurrentValue;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Source.OnChange -= Changed;
		}
	}

	/// <summary>
	/// A biaxial input mapping maps to a two-axis input such as a mouse or thumbstick.
	/// </summary>
	public class BiaxialInputMapping : IInputMapping {

		public Vector2 Delta { get; private set; }

		public Vector2 State { get; private set; }

		private IInputSource<Vector2> source = NullInputSource<Vector2>.Instance;
		public IInputSource<Vector2> Source {
			get => source;
			set {
				source.OnChange -= Changed;
				source = value;
				Changed(source.CurrentValue);
				source.OnChange += Changed;
			}
		}

		public required string Name { get; init; }

		public required string ID { get; init; }

		public event Action? OnChange;

		private void Changed(Vector2 value) {
			if (value != State) {
				Delta = value - State;
				State = value;
				OnChange?.Invoke();
			}
		}

		public void Reset() {
			Source = NullInputSource<Vector2>.Instance;
			State = Source.CurrentValue;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			Source.OnChange -= Changed;
		}

	}

}
