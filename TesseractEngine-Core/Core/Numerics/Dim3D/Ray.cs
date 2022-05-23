using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Numerics.Dim3D {
	
	/// <summary>
	/// A ray of infinite length, with an origin point and a direction vector.
	/// </summary>
	public struct Ray {

		/// <summary>
		/// The origin point of the ray.
		/// </summary>
		public Vector3 Origin;

		/// <summary>
		/// A direction vector describing the direction the ray travels.
		/// </summary>
		public Vector3 Direction;

		public Ray(Vector3 origin, Vector3 direction) {
			Origin = origin;
			Direction = direction;
		}

	}

}
