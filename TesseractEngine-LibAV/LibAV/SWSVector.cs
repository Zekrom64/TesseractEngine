using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.LibAV {
	
	public class SWSVector : IDisposable {

		public UnmanagedPointer<SwsVector> Vector { get; }

		public UnmanagedPointer<double> Coeff {
			get {
				SwsVector v = Vector.Value;
				return new(v.Coeff, v.Length);
			}
		}

		public SWSVector(int length) {
			Vector = new(SWScale.Functions.sws_allocVec(length));
		}

		public SWSVector(UnmanagedPointer<SwsVector> vector) {
			Vector = vector;
		}

		public SWSVector(IntPtr vector) {
			Vector = new(vector);
		}

		public static SWSVector GetGaussian(double variance, int length) => new(SWScale.Functions.sws_getGaussianVec(variance, length));

		public static SWSVector GetConstant(double c, int length) => new(SWScale.Functions.sws_getConstVec(c, length));

		public static SWSVector GetIdentity() => new(SWScale.Functions.sws_getIdentityVec());

		public void Dispose() {
			GC.SuppressFinalize(this);
			SWScale.Functions.sws_freeVec(Vector);
		}

		public double this[int index] {
			get => Coeff[index];
			set {
				var coeff = Coeff;
				coeff[index] = value;
			}
		}

		public static implicit operator UnmanagedPointer<SwsVector>(SWSVector v) => v.Vector;

		public void Scale(double scalar) => SWScale.Functions.sws_scaleVec(Vector, scalar);

		public void Normalize(double height) => SWScale.Functions.sws_normalizeVec(Vector, height);

		public void Conv(SWSVector b) => SWScale.Functions.sws_convVec(Vector, b.Vector);

		public void Add(SWSVector b) => SWScale.Functions.sws_addVec(Vector, b.Vector);

		public void Sub(SWSVector b) => SWScale.Functions.sws_subVec(Vector, b.Vector);

		public void Shift(int shift) => SWScale.Functions.sws_shiftVec(Vector, shift);

		public SWSVector Clone() => new(SWScale.Functions.sws_cloneVec(Vector));

	}
}
