using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Util {
	
	/// <summary>
	/// Enumeration of simple logging levels. Higher-priority levels are numerically
	/// greather than lower-priority levels. The base level is <see cref="Info"/>.
	/// </summary>
	public enum LogLevel {
		/// <summary>
		/// Debugging logging level.
		/// </summary>
		Debug = -100,
		/// <summary>
		/// General information logging level.
		/// </summary>
		Info = 0,
		/// <summary>
		/// Configuration logging level.
		/// </summary>
		Config = 100,
		/// <summary>
		/// Warning logging level.
		/// </summary>
		Warning = 300,
		/// <summary>
		/// Error logging level.
		/// </summary>
		Error = 500
	}

	/// <summary>
	/// A simple logger provides a way of logging messages without requiring any specific complex
	/// logging system (although messages from this may be forwarded to one).
	/// </summary>
	public interface ISimpleLogger {

		/// <summary>
		/// Logs a message.
		/// </summary>
		/// <param name="level">The logging level of the message</param>
		/// <param name="message">The message to be logged</param>
		public void Log(LogLevel level, string message);

	}

}
