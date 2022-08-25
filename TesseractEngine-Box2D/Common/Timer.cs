using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {
	
	public struct Timer {

		private readonly Stopwatch stopwatch = new();

		public Timer() {
			stopwatch.Start();
		}

		public void Reset() {
			stopwatch.Restart();
		}

		private static readonly double milliFactor = 1000.0 / Stopwatch.Frequency;

		public float Milliseconds => (float)(stopwatch.ElapsedTicks * milliFactor);

	}

}
