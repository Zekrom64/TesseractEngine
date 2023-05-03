using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.ImGui.NET {

	public class ImGuiNETIO : IImGuiIO, IDisposable {

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Cannot be readonly to set clipboard user data")]
		private ImGuiIOPtr io;

		private GCHandle self;

		internal ImGuiNETIO(ImGuiIOPtr io) {
			this.io = io;
			self = GCHandle.Alloc(this);

			// Shouldn't be null if we have IO...
			Fonts = new ImGuiNETFontAtlas(io.Fonts);

			pfnSetPlatformImeData = Marshal.GetFunctionPointerForDelegate(SetPlatformImeData);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			self.Free();

			iniFilename.Dispose();
			logFilename.Dispose();

			backendPlatformName.Dispose();
			backendRendererName.Dispose();

			clipboardText.Dispose();
		}


		public ImGuiConfigFlags ConfigFlags { get => (ImGuiConfigFlags)io.ConfigFlags; set => io.ConfigFlags = (ImGuiNET.ImGuiConfigFlags)value; }

		public ImGuiBackendFlags BackendFlags { get => (ImGuiBackendFlags)io.BackendFlags; set => io.BackendFlags = (ImGuiNET.ImGuiBackendFlags)value; }

		public Vector2 DisplaySize { get => io.DisplaySize; set => io.DisplaySize = value; }

		public float DeltaTime { get => io.DeltaTime; set => io.DeltaTime = value; }

		public float IniSavingRate { get => io.IniSavingRate; set => io.IniSavingRate = value; }


		private ManagedPointer<byte> iniFilename;

		public string? IniFilename {
			get => io.IniFilename;
			set {
				iniFilename.Dispose();
				iniFilename = MemoryUtil.AllocUTF8(value);
				unsafe {
					io.NativePtr->IniFilename = (byte*)iniFilename.Ptr;
				}
			}
		}

		private ManagedPointer<byte> logFilename;

		public string LogFilename {
			get => io.LogFilename;
			set {
				logFilename.Dispose();
				logFilename = MemoryUtil.AllocUTF8(value);
				unsafe {
					io.NativePtr->LogFilename = (byte*)logFilename.Ptr;
				}
			}
		}


		public float MouseDoubleClickTime { get => io.MouseDoubleClickTime; set => io.MouseDoubleClickTime = value; }

		public float MouseDoubleClickMaxDist { get => io.MouseDoubleClickMaxDist; set => io.MouseDoubleClickMaxDist = value; }

		public float MouseDragThreshold { get => io.MouseDragThreshold; set => io.MouseDragThreshold = value; }

		public float KeyRepeatDelay { get => io.KeyRepeatDelay; set => io.KeyRepeatDelay = value; }

		public float KeyRepeatRate { get => io.KeyRepeatRate; set => io.KeyRepeatRate = value; }

		public IImFontAtlas Fonts { get; }

		public float FontGlobalScale { get => io.FontGlobalScale; set => io.FontGlobalScale = value; }

		public bool FontAllowUserScaling { get => io.FontAllowUserScaling; set => io.FontAllowUserScaling = value; }

		public IImFont? FontDefault {
			get {
				var ptr = io.FontDefault;
				unsafe {
					if (ptr.NativePtr == null) return null;
					else return ((ImGuiNETFontAtlas)Fonts).GetFont(ptr);
				}
			}
			set {
				unsafe {
					io.NativePtr->FontDefault = value == null ? null : ((ImGuiNETFont)value).font;
				}
			}
		} // TODO

		public Vector2 DisplayFramebufferScale { get => io.DisplayFramebufferScale; set => io.DisplayFramebufferScale = value; }

		public bool MouseDrawCursor { get => io.MouseDrawCursor; set => io.MouseDrawCursor = value; }

		public bool ConfigMacOSXBehaviors { get => io.ConfigMacOSXBehaviors; set => io.ConfigMacOSXBehaviors = value; }

		public bool ConfigInputTrickleEventQueue { get => io.ConfigInputTrickleEventQueue; set => io.ConfigInputTrickleEventQueue = value; }

		public bool ConfigInputTextCursorBlink { get => io.ConfigInputTextCursorBlink; set => io.ConfigInputTextCursorBlink = value; }

		public bool ConfigDragClickToInputText { get => io.ConfigDragClickToInputText; set => io.ConfigDragClickToInputText = value; }

		public bool ConfigWindowsResizeFromEdges { get => io.ConfigWindowsResizeFromEdges; set => io.ConfigWindowsResizeFromEdges = value; }

		public bool ConfigWindowsMoveFromTitleBarOnly { get => io.ConfigWindowsMoveFromTitleBarOnly; set => io.ConfigWindowsMoveFromTitleBarOnly = value; }

		public float ConfigMemoryCompactTimer { get => io.ConfigMemoryCompactTimer; set => io.ConfigMemoryCompactTimer = value; }


		private ManagedPointer<byte> backendPlatformName;

		public string? BackendPlatformName {
			get => io.BackendPlatformName;
			set {
				backendPlatformName.Dispose();
				backendPlatformName = MemoryUtil.AllocUTF8(value);
				unsafe {
					io.NativePtr->BackendPlatformName = (byte*)backendPlatformName.Ptr;
				}
				
			}
		}

		private ManagedPointer<byte> backendRendererName;

		public string? BackendRendererName {
			get => io.BackendRendererName;
			set {
				backendRendererName.Dispose();
				backendRendererName = MemoryUtil.AllocUTF8(value);
				unsafe {
					io.NativePtr->BackendRendererName = (byte*)backendRendererName.Ptr;
				}
			}
		}


		private ManagedPointer<byte> clipboardText;

		[UnmanagedCallersOnly]
		private static unsafe IntPtr GetClipboardText(IntPtr userdata) {
			ImGuiNETIO io = (ImGuiNETIO)GCHandle.FromIntPtr(userdata).Target!;
			string text = io.getClipboardTextFn?.Invoke() ?? string.Empty;
			io.clipboardText.Dispose();
			io.clipboardText = MemoryUtil.AllocUTF8(text);
			return io.clipboardText.Ptr;
		}

		[UnmanagedCallersOnly]
		private static unsafe void SetClipboardText(IntPtr userdata, IntPtr text) {
			ImGuiNETIO io = (ImGuiNETIO)GCHandle.FromIntPtr(userdata).Target!;
			io.setClipboardTextFn?.Invoke(MemoryUtil.GetUTF8(text)!);
		}

		private Func<string>? getClipboardTextFn;

		public Func<string>? GetClipboardTextFn {
			set {
				getClipboardTextFn = value;
				unsafe {
					delegate* unmanaged<IntPtr, IntPtr> cbk = &GetClipboardText;
					io.GetClipboardTextFn = (nint)cbk;
					io.ClipboardUserData = (nint)self;
				}
			}
		}

		private Action<string>? setClipboardTextFn;

		public Action<string>? SetClipboardTextFn {
			set {
				setClipboardTextFn = value;
				unsafe {
					delegate* unmanaged<IntPtr, IntPtr, void> cbk = &SetClipboardText;
					io.SetClipboardTextFn = (nint)cbk;
					io.ClipboardUserData = (nint)self;
				}
			}
		}


		private void SetPlatformImeData(IntPtr viewport, IntPtr data) => setPlaformImeDataFn?.Invoke(MemoryUtil.ReadUnmanaged<ImGuiViewport>(viewport), MemoryUtil.ReadUnmanaged<ImGuiPlatformImeData>(data));

		private readonly IntPtr pfnSetPlatformImeData;

		private Action<ImGuiViewport, ImGuiPlatformImeData>? setPlaformImeDataFn;

		public Action<ImGuiViewport, ImGuiPlatformImeData>? SetPlatformImeDataFn {
			set {
				setPlaformImeDataFn = value;
				unsafe {
					io.NativePtr->SetPlatformImeDataFn = pfnSetPlatformImeData;
				}
			}
		}


		public bool WantCaptureMouse => io.WantCaptureMouse;

		public bool WantCaptureKeyboard => io.WantCaptureKeyboard;

		public bool WantTextInput => io.WantTextInput;

		public bool WantSetMousePos => io.WantSetMousePos;

		public bool WantSaveIniSettings { get => io.WantSaveIniSettings; set => io.WantSaveIniSettings = value; }

		public bool NavActive => io.NavActive;

		public bool NavVisible => io.NavVisible;

		public float Framerate => io.Framerate;

		public int MetricsRenderVertices => io.MetricsRenderVertices;

		public int MetricsRenderIndices => io.MetricsRenderIndices;

		public int MetricsRenderWindows => io.MetricsRenderWindows;

		public int MetricsActiveWindows => io.MetricsActiveWindows;

		public int MetricsActiveAllocations => io.MetricsActiveAllocations;

		public Vector2 MouseDelta => io.MouseDelta;

		public Vector2 MousePos => io.MousePos;

		public void AddFocusEvent(bool focused) => io.AddFocusEvent(focused);

		public void AddInputCharacter(int c) => io.AddInputCharacter((uint)c);

		public void AddInputCharacters(string str) => io.AddInputCharactersUTF8(str);

		public void AddInputCharactersUTF8(ReadOnlySpan<byte> str) {
			unsafe {
				fixed(byte* pStr = ImGuiNETCore.CheckStr(str)) {
					ImGuiNative.ImGuiIO_AddInputCharactersUTF8(io.NativePtr, pStr);
				}
			}
		}

		public void AddInputCharacterUTF16(char c) => io.AddInputCharacterUTF16(c);

		public void AddKeyAnalogEvent(ImGuiKey key, bool down, float v) => io.AddKeyAnalogEvent((ImGuiNET.ImGuiKey)key, down, v);

		public void AddKeyEvent(ImGuiKey key, bool down) => io.AddKeyEvent((ImGuiNET.ImGuiKey)key, down);

		public void AddMouseButtonEvent(ImGuiMouseButton button, bool down) => io.AddMouseButtonEvent((int)button, down);

		public void AddMousePosEvent(float x, float y) => io.AddMousePosEvent(x, y);

		public void AddMouseWheelEvent(float x, float y) => io.AddMouseWheelEvent(x, y);

	}

}
