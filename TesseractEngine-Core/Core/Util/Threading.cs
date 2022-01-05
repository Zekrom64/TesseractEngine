using System;

namespace Tesseract.Core.Util {

	/// <summary>
	/// Enumeration of levels for thread safety.
	/// </summary>
	public enum ThreadSafetyLevel {
		/// <summary>
		/// The target must only be used within the "main" thread of a program.
		/// </summary>
		MainThread,
		/// <summary>
		/// The target must only be used in a single-threaded manner, but otherwise has no restrictions.
		/// </summary>
		SingleThread,
		/// <summary>
		/// The target has mixed support for different levels of thread safety.
		/// </summary>
		Mixed,
		/// <summary>
		/// The target can safely be used concurrently by multiple threads.
		/// </summary>
		Concurrent
	}

	/// <summary>
	/// Indicates how the target should be used safely in a multi-threaded context.
	/// </summary>
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	public sealed class ThreadSafetyAttribute : Attribute {

		/// <summary>
		/// The level of thread safety required.
		/// </summary>
		public ThreadSafetyLevel SafetyLevel { get; }

		public ThreadSafetyAttribute(ThreadSafetyLevel safetyLevel) {
			SafetyLevel = safetyLevel;
		}

	}

}
