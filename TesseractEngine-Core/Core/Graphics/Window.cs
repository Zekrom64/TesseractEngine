using System;
using System.Collections;
using System.Collections.Generic;
using Tesseract.Core.Input;
using Tesseract.Core.Math;
using Tesseract.Core.Util;

namespace Tesseract.Core.Graphics {

	/// <summary>
	/// A window attribute is an aspect of a window that can be passed during or after window creation.
	/// </summary>
	public interface IWindowAttribute {

		/// <summary>
		/// Checks if the given value is valid for this attribute.
		/// </summary>
		/// <param name="val">Value to check</param>
		/// <returns>Checked value</returns>
		public object CheckValue(object val);

	}

	/// <summary>
	/// A window attribute is an aspect of a window that can be passed during or after window creation.
	/// <typeparam name="T">The type of the attribute value</typeparam>
	/// </summary>
	public interface IWindowAttribute<T> : IWindowAttribute { }

	/// <summary>
	/// A simple opaque window attribute that checks against a type parameter.
	/// </summary>
	/// <typeparam name="T">Window attribute type</typeparam>
	public class OpaqueWindowAttribute<T> : IWindowAttribute<T> {

		public object CheckValue(object val) => (T)val;

	}

	/// <summary>
	/// Standard window attributes.
	/// </summary>
	public static class WindowAttributes {

		/// <summary>
		/// The title of the window.
		/// </summary>
		public static readonly IWindowAttribute<string> Title = new OpaqueWindowAttribute<string>();

		/// <summary>
		/// The size of the window.
		/// </summary>
		public static readonly IWindowAttribute<Vector2i> Size = new OpaqueWindowAttribute<Vector2i>();

		/// <summary>
		/// The position of the window on the desktop. By default the windowing system may place the window anywhere.
		/// </summary>
		public static readonly IWindowAttribute<Vector2i> Position = new OpaqueWindowAttribute<Vector2i>();

		/// <summary>
		/// If the window is minimized. By default the window is not minimized.
		/// </summary>
		public static readonly IWindowAttribute<bool> Minimized = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// If the window is maximized. By default the window is not maximized.
		/// </summary>
		public static readonly IWindowAttribute<bool> Maximized = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// If the window is visible. By default the window is visible.
		/// </summary>
		public static readonly IWindowAttribute<bool> Visible = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// If the window has input focus. By default the window is not focused.
		/// </summary>
		public static readonly IWindowAttribute<bool> Focused = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// If the window is closing. By default the window is not closing.
		/// </summary>
		public static readonly IWindowAttribute<bool> Closing = new OpaqueWindowAttribute<bool>();

		/// <summary>
		/// The opacity of the window. By default the window opacity is 1.0.
		/// </summary>
		public static readonly IWindowAttribute<float> Opacity = new OpaqueWindowAttribute<float>();

		/// <summary>
		/// If the window is resizable. By default the window is not resizable.
		/// </summary>
		public static readonly IWindowAttribute<bool> Resizable = new OpaqueWindowAttribute<bool>();

	}

	/// <summary>
	/// A window attribute list stores a list of attributes and their values.
	/// </summary>
	public class WindowAttributeList : IEnumerable<KeyValuePair<IWindowAttribute, object>> {

		private readonly Dictionary<IWindowAttribute, object> attributes = new();

		/// <summary>
		/// Creates a new window attribute list.
		/// </summary>
		/// <param name="attribs">List of initial attributes and their values</param>
		public WindowAttributeList(params KeyValuePair<IWindowAttribute, object>[] attribs) {
			foreach (var attrib in attribs) {
				attributes[attrib.Key] = attrib.Value;
			}
		}

		/// <summary>
		/// Tests if the attribute list has the given attribute.
		/// </summary>
		/// <param name="attrib">Attribute to test for</param>
		/// <returns>If the list has the attribute</returns>
		public bool Has(IWindowAttribute attrib) => attributes.ContainsKey(attrib);

		/// <summary>
		/// Gets the value for the given attribute.
		/// </summary>
		/// <typeparam name="T">Type of attribute value</typeparam>
		/// <param name="attrib">Attribute to get</param>
		/// <returns>Attribute value</returns>
		public T Get<T>(IWindowAttribute<T> attrib) => (T)attributes[attrib];

		/// <summary>
		/// Attempts to get a value for the given attribute.
		/// </summary>
		/// <typeparam name="T">Type of attribute value</typeparam>
		/// <param name="attrib">Attribute to get</param>
		/// <param name="val">Attribute value</param>
		/// <returns>If the attribute list contained the value</returns>
		public bool TryGet<T>(IWindowAttribute<T> attrib, out T? val) {
			if (attributes.TryGetValue(attrib, out object? value)) {
				val = (T)value;
				return true;
			} else {
				val = default;
				return false;
			}
		}

		/// <summary>
		/// Sets the given attribute value in the list.
		/// </summary>
		/// <typeparam name="T">Type of attribute value</typeparam>
		/// <param name="attrib">Attribute to set</param>
		/// <param name="value">Attribute value</param>
		public void Set<T>(IWindowAttribute<T> attrib, T value) {
			if (value != null) attributes[attrib] = value;
		}

		public IEnumerator<KeyValuePair<IWindowAttribute, object>> GetEnumerator() => attributes.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => attributes.GetEnumerator();

		/// <summary>
		/// Adds an attribute to the list. Note that this is a convenience method
		/// for collection initialization, and <see cref="Set{T}(IWindowAttribute{T}, T)"/>
		/// should be preferred for manual modification.
		/// </summary>
		/// <param name="attrib">Attribute to set</param>
		/// <param name="value">Attribute value</param>
		public void Add(IWindowAttribute attrib, object value) => attributes.Add(attrib, value);

	}

	/// <summary>
	/// Enumeration of standard cursors.
	/// </summary>
	public enum StandardCursor {
		Arrow,
		IBeam,
		Hand,
		Crosshair,
		HResize,
		VResize
	}

	/// <summary>
	/// A cursor is an image that is drawn at the position of the mouse on screen.
	/// </summary>
	public interface ICursor : IDisposable { }

	/// <summary>
	/// A windowing system manages a desktop made up of one or more displays and
	/// user-created windows.
	/// </summary>
	[ThreadSafety(ThreadSafetyLevel.MainThread)]
	public interface IWindowSystem : Services.IServiceProvider {

		/// <summary>
		/// Creates a new window.
		/// </summary>
		/// <param name="title">The title of the window</param>
		/// <param name="w">The width of the window</param>
		/// <param name="h">The height of the window</param>
		/// <param name="attributes">The initial attributes of the window</param>
		/// <returns>The created window</returns>
		public IWindow CreateWindow(string title, int w, int h, WindowAttributeList? attributes = null);

		/// <summary>
		/// Gets the displays which make up the desktop.
		/// </summary>
		/// <returns>Desktop displays</returns>
		public IDisplay[] GetDisplays();

		/// <summary>
		/// If custom cursors are supported.
		/// </summary>
		public bool CustomCursorSupport { get; }

		/// <summary>
		/// Creates a custom cursor from an image. Cursors have a "hotspot" that determines how the cursor 
		/// image is positioned relative to where the mouse reports its position.
		/// </summary>
		/// <param name="image">The cursor image</param>
		/// <param name="hotspot">The hotspot position of the cursor</param>
		/// <returns>A custom cursor</returns>
		public ICursor CreateCursor(IImage image, Vector2i hotspot);

		/// <summary>
		/// Creates a standard cursor.
		/// </summary>
		/// <param name="std">The type of standard cursor to create</param>
		/// <returns>A standard cursor</returns>
		public ICursor CreateStandardCursor(StandardCursor std);

	}

	/// <summary>
	/// A display composes a section of a desktop in a windowing system and has
	/// a display mode 
	/// </summary>
	[ThreadSafety(ThreadSafetyLevel.MainThread)]
	public interface IDisplay : Services.IServiceProvider {

		/// <summary>
		/// The position of the display's area inside the desktop.
		/// </summary>
		public IReadOnlyTuple2<int> Position { get; }

		/// <summary>
		/// The name of the display. This may be specific to the windowing system.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The current display mode used by the display.
		/// </summary>
		public IDisplayMode CurrentMode { get; }

		/// <summary>
		/// Gets all of the display modes supported by the display.
		/// </summary>
		/// <returns>Display modes supported by the display</returns>
		public IDisplayMode[] GetDisplayModes();

	}

	/// <summary>
	/// A display mode describes how video is presented on a display.
	/// </summary>
	public interface IDisplayMode {

		/// <summary>
		/// The format of pixels sent to the display.
		/// </summary>
		public PixelFormat PixelFormat { get; }

		/// <summary>
		/// The size of the display in pixels.
		/// </summary>
		public IReadOnlyTuple2<int> Size { get; }

		/// <summary>
		/// The refresh/frame rate, or how many times per second the display's image is updated.
		/// </summary>
		public int RefreshRate { get; }

	}

	/// <summary>
	/// A window is a configurable surface created in a windowing system that can be used for presentation
	/// to a display. The window is only fully destroyed when explicitly disposed. Input for windows is
	/// "local"; positions will be relative to the client area and input may be disabled if the window
	/// is not focused.
	/// </summary>
	[ThreadSafety(ThreadSafetyLevel.MainThread)]
	public interface IWindow : IDisposable, Services.IServiceProvider, IKeyInput, ITextInput, IMouseInput {

		/// <summary>
		/// The title of the window.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The size of the window's content area. The actual size of the window
		/// with borders may be greater.
		/// </summary>
		public Vector2i Size { get; set; }

		/// <summary>
		/// The position of the window.
		/// </summary>
		public Vector2i Position { get; set; }

		/// <summary>
		/// If the window is minimized.
		/// </summary>
		public bool Minimized { get; set; }

		/// <summary>
		/// If the window is maximized.
		/// </summary>
		public bool Maximized { get; set; }

		/// <summary>
		/// If the window is visible.
		/// </summary>
		public bool Visible { get; set; }

		/// <summary>
		/// If the window is focused.
		/// </summary>
		public bool Focused { get; set; }

		/// <summary>
		/// If the window is closing.
		/// </summary>
		[ThreadSafety(ThreadSafetyLevel.Concurrent)]
		public bool Closing { get; set; }

		/// <summary>
		/// The opacity of the window.
		/// </summary>
		public float Opacity { get; set; }

		/// <summary>
		/// If the window is resizable.
		/// </summary>
		public bool Resizable { get; set; }

		/// <summary>
		/// If the window is fullscreen on a display.
		/// </summary>
		public bool Fullscreen { get; }

		/// <summary>
		/// The display this window is fullscreen on, or null.
		/// </summary>
		public IDisplay? FullscreenDisplay { get; }

		/// <summary>
		/// Event fired when the window is resized.
		/// </summary>
		public event Action<Vector2i> OnResize;

		/// <summary>
		/// Event fired when the window is moved.
		/// </summary>
		public event Action<Vector2i> OnMove;

		/// <summary>
		/// Event fired when the window is minimized.
		/// </summary>
		public event Action OnMinimized;

		/// <summary>
		/// Event fired when the window is maximized.
		/// </summary>
		public event Action OnMaximized;

		/// <summary>
		/// Event fired when the window is restored.
		/// </summary>
		public event Action OnRestored;

		/// <summary>
		/// Event fired when the window recieves input focus.
		/// </summary>
		public event Action OnFocused;

		/// <summary>
		/// Event fired when the window loses input focus.
		/// </summary>
		public event Action OnUnfocused;

		/// <summary>
		/// Event fired when the window is signaled to close.
		/// </summary>
		public event Action OnClosing;

		/// <summary>
		/// Restores this window to its regular windowed state, exiting any minimization, maximization, or fullscreen mode.
		/// </summary>
		public void Restore();

		/// <summary>
		/// Sets the fullscreen mode for this window.
		/// </summary>
		/// <param name="display">Display to make fullscreen on</param>
		/// <param name="mode">Display mode to use in fullscreen mode</param>
		public void SetFullscreen(IDisplay? display, IDisplayMode? mode);

		/// <summary>
		/// Sets the cursor displayed while inside the window's client area. If null
		/// is passed the cursor is reset to its default image.
		/// </summary>
		/// <param name="cursor">Cursor to set</param>
		public void SetCursor(ICursor? cursor);

		/// <summary>
		/// If the window captures the mouse when focused. When the mouse is captured the cursor is hidden
		/// and locked inside of the client area, allowing unlimited movement. When in this mode the the
		/// absolute position of mouse events is undefined and should be ignored.
		/// </summary>
		public bool CaptureMouse { get; set; }

		/// <summary>
		/// The window surface, if present.
		/// </summary>
		public IWindowSurface? Surface { get; }

	}

	/// <summary>
	/// <para>A gamma ramp stores a list of gamma-corrected red, green, and blue values.</para>
	/// <para>
	/// Gamma ramps may be of variable length, but most platforms require a certain size
	/// of gamma ramp. Compatible gamma ramps can be created using <see cref="IGammaRampObject.CreateCompatibleGammaRamp"/>.
	/// </para>
	/// </summary>
	public interface IGammaRamp {

		/// <summary>
		/// The number of entries in the gamma ramp.
		/// </summary>
		public int Length { get; }

		/// <summary>
		/// The list of red component values in the gamma ramp. Note that the field must be written to to set the values.
		/// </summary>
		public ushort[] Red { get; set; }

		/// <summary>
		/// The list of green component values in the gamma ramp. Note that the field must be written to to set the values.
		/// </summary>
		public ushort[] Green { get; set; }

		/// <summary>
		/// The list of blue component values in the gamma ramp. Note that the field must be written to to set the values.
		/// </summary>
		public ushort[] Blue { get; set; }

		/// <summary>
		/// Indexes this gamma ramp as a list of RGB tuples.
		/// </summary>
		/// <param name="index">Index in the gamma ramp</param>
		/// <returns>RGB tuple at index</returns>
		public Vector3us this[int index] { get; set; }

	}

	/// <summary>
	/// A gamma ramp object is an object that has its own gamma ramp.
	/// This is normally implemented by windows or displays.
	/// </summary>
	[ThreadSafety(ThreadSafetyLevel.MainThread)]
	public interface IGammaRampObject {

		/// <summary>
		/// The gamma ramp for this object.
		/// </summary>
		public IGammaRamp GammaRamp { get; set; }

		/// <summary>
		/// Creates a gamma ramp that is compatible with this object. The initial contents
		/// of the gamma ramp are undefined.
		/// </summary>
		/// <returns>Compatible gamma ramp</returns>
		public IGammaRamp CreateCompatibleGammaRamp();

	}

	/// <summary>
	/// A window surface is present on windows that support direct image copying to the window's screen area. Using
	/// the window surface directly is generally incompatible with accelerated graphics, which provide their own
	/// methods for presenting to the window surface.
	/// </summary>
	public interface IWindowSurface {

		/// <summary>
		/// Copies a region of an image to a point on the window surface.
		/// </summary>
		/// <param name="dstPos">The point to copy to on the window surface</param>
		/// <param name="srcImage">The image to copy from</param>
		/// <param name="srcArea">The area to copy from the image</param>
		public void BlitToSurface(IReadOnlyTuple2<int> dstPos, IImage srcImage, IReadOnlyRect<int> srcArea);

		/// <summary>
		/// Swaps the front and back buffers of the window surface, making changes visible.
		/// </summary>
		public void SwapSurface();

	}

}
