using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Box2D.NET {
	
	public ref struct TOIInput {

		public DistanceProxy ProxyA;
		public DistanceProxy ProxyB;
		public Sweep SweepA;
		public Sweep SweepB;
		public float TMax;

	}

	public enum TOIState {
		Unknown,
		Failed,
		Overlapped,
		Touching,
		Separated
	}

	public static partial class Box2D {

		public enum SeparationType {
			Points,
			FaceA,
			FaceB
		}

		private ref struct SeparationFunction {
			
			private DistanceProxy proxyA;
			private DistanceProxy proxyB;
			private Sweep sweepA, sweepB;
			private SeparationType type;
			private Vector2 localPoint;
			private Vector2 axis;

			public float Initialize(ref SimplexCache cache, DistanceProxy proxyA, DistanceProxy proxyB, Sweep sweepA, Sweep sweepB, float t1) {
				this.proxyA = proxyA;
				this.proxyB = proxyB;
				int count = cache.Count;
				Debug.Assert(0 < count && count < 3);

				this.sweepA = sweepA;
				this.sweepB = sweepB;

				Transform xfA = sweepA.GetTransform(t1);
				Transform xfB = sweepB.GetTransform(t1);

				unsafe {
					if (count == 1) {
						type = SeparationType.Points;
						Vector2 localPointA = proxyA.Vertices[cache.IndexA[0]];
						Vector2 localPointB = proxyB.Vertices[cache.IndexB[0]];
						Vector2 pointA = xfA * localPointA;
						Vector2 pointB = xfB * localPointB;
						axis = pointB - pointA;
						float length = axis.Length();
						axis *= 1.0f / length;
						return length;
					} else if (cache.IndexA[0] == cache.IndexA[1]) {
						type = SeparationType.FaceB;
						Vector2 localPointB1 = proxyB.Vertices[cache.IndexB[0]];
						Vector2 localPointB2 = proxyB.Vertices[cache.IndexB[1]];
						
						axis = (localPointB2 - localPointB1).Cross(1);
						axis = axis.Normalize();
						Vector2 normal = xfB.Rotation * axis;

						localPoint = 0.5f * (localPointB1 + localPointB2);
						Vector2 pointB = xfB * localPoint;

						Vector2 localPointA = proxyA.Vertices[cache.IndexA[0]];
						Vector2 pointA = xfA * localPointA;

						float s = (pointA - pointB).Dot(normal);
						if (s < 0) {
							axis = -axis;
							s = -s;
						}
						return s;
					} else {
						type = SeparationType.FaceA;
						Vector2 localPointA1 = proxyA.Vertices[cache.IndexA[0]];
						Vector2 localPointA2 = proxyA.Vertices[cache.IndexA[1]];

						axis = (localPointA2 - localPointA1).Cross(1);
						axis = axis.Normalize();
						Vector2 normal = xfA.Rotation * axis;

						localPoint = 0.5f * (localPointA1 + localPointA2);
						Vector2 pointA = xfA * localPoint;

						Vector2 localPointB = proxyB.Vertices[cache.IndexB[0]];
						Vector2 pointB = xfB * localPointB;

						float s = (pointB - pointA).Dot(normal);
						if (s < 0) {
							axis = -axis;
							s = -s;
						}
						return s;
					}
				}
			}

			public float FindMinSeparation(out int indexA, out int indexB, float t) {
				Transform xfA = sweepA.GetTransform(t);
				Transform xfB = sweepB.GetTransform(t);

				switch(type) {
					case SeparationType.Points:
						{
							Vector2 axisA = xfA.Rotation * axis;
							Vector2 axisB = xfB.Rotation * -axis;

							indexA = proxyA.GetSupport(axisA);
							indexB = proxyB.GetSupport(axisB);

							Vector2 localPointA = proxyA.Vertices[indexA];
							Vector2 localPointB = proxyB.Vertices[indexB];

							Vector2 pointA = xfA * localPointA;
							Vector2 pointB = xfB * localPointB;

							return (pointB - pointA).Dot(axis);
						}
					case SeparationType.FaceA:
						{
							Vector2 normal = xfA.Rotation * axis;
							Vector2 pointA = xfA * localPoint;

							Vector2 axisB = xfB.Rotation * -normal;

							indexA = -1;
							indexB = proxyB.GetSupport(axisB);

							Vector2 localPointB = proxyB.Vertices[indexB];
							Vector2 pointB = xfB * localPointB;

							return (pointB - pointA).Dot(normal);
						}
					case SeparationType.FaceB:
						{
							Vector2 normal = xfB.Rotation * axis;
							Vector2 pointB = xfB * localPoint;

							Vector2 axisA = xfA.Rotation * -normal;

							indexB = -1;
							indexA = proxyA.GetSupport(axisA);

							Vector2 localPointA = proxyA.Vertices[indexA];
							Vector2 pointA = xfA * localPointA;

							return (pointA - pointB).Dot(normal);
						}
					default:
						throw new InvalidOperationException();
				}
			}

			public float Evaluate(int indexA, int indexB, float t) {
				Transform xfA = sweepA.GetTransform(t);
				Transform xfB = sweepB.GetTransform(t);

				switch(type) {
					case SeparationType.Points:
						{
							Vector2 localPointA = proxyA.Vertices[indexA];
							Vector2 localPointB = proxyB.Vertices[indexB];

							Vector2 pointA = xfA * localPointA;
							Vector2 pointB = xfB * localPointB;
							return (pointB - pointA).Dot(axis);
						}
					case SeparationType.FaceA:
						{
							Vector2 normal = xfA.Rotation * axis;
							Vector2 pointA = xfA * localPoint;

							Vector2 localPointB = proxyB.Vertices[indexB];
							Vector2 pointB = xfB * localPointB;

							return (pointB - pointA).Dot(normal);
						}
					case SeparationType.FaceB: {
							Vector2 normal = xfB.Rotation * axis;
							Vector2 pointB = xfB * localPoint;

							Vector2 localPointA = proxyA.Vertices[indexA];
							Vector2 pointA = xfA * localPointA;

							return (pointA - pointB).Dot(normal);
						}
					default:
						throw new InvalidOperationException();
				}
			}
			
		}

		private static object lockTOIMaxRootIters = new(), lockTOIMaxIters = new(), lockTOITime = new();

		internal static float TOITime, TOIMaxTime;
		internal static int TOICalls, TOIIters, TOIMaxIters;
		internal static int TOIRootIters, TOIMaxRootIters;

		public static TOIState TimeOfImpact(TOIInput input, out float tOut) {
			Timer timer = new();
			Interlocked.Increment(ref TOICalls);

			TOIState state = TOIState.Unknown;
			tOut = input.TMax;

			DistanceProxy proxyA = input.ProxyA;
			DistanceProxy proxyB = input.ProxyB;

			Sweep sweepA = input.SweepA;
			Sweep sweepB = input.SweepB;

			sweepA.Normalize();
			sweepB.Normalize();

			float tMax = input.TMax;

			float totalRadius = proxyA.Radius + proxyB.Radius;
			float target = Math.Max(LinearSlop, totalRadius - 3 * LinearSlop);
			float tolerance = 0.25f * LinearSlop;
			Debug.Assert(target > tolerance);

			float t1 = 0;
			const int MaxIterations = 20;
			int iter = 0;

			SimplexCache cache = new();
			DistanceInput distanceInput = new() {
				ProxyA = proxyA,
				ProxyB = proxyB,
				UseRadii = false
			};

			while(true) {
				Transform xfA = sweepA.GetTransform(t1);
				Transform xfB = sweepB.GetTransform(t1);

				distanceInput = distanceInput with {
					TransformA = xfA,
					TransformB = xfB
				};
				DistanceOutput distanceOutput = Distance(ref cache, distanceInput);

				if (distanceOutput.Distance <= 0) {
					tOut = 0;
					return TOIState.Overlapped;
				}

				if (distanceOutput.Distance < target + tolerance) {
					tOut = t1;
					return TOIState.Touching;
				}

				SeparationFunction fcn = new();
				fcn.Initialize(ref cache, proxyA, proxyB, sweepA, sweepB, t1);

				bool done = false;
				float t2 = tMax;
				int pushBackIter = 0;
				while(true) {
					float s2 = fcn.FindMinSeparation(out int indexA, out int indexB, t2);

					if (s2 > target + tolerance) {
						state = TOIState.Separated;
						tOut = tMax;
						done = true;
						break;
					}

					if (s2 > target - tolerance) {
						t1 = t2;
						break;
					}

					float s1 = fcn.Evaluate(indexA, indexB, t1);

					if (s1 < target - tolerance) {
						state = TOIState.Failed;
						tOut = t1;
						done = true;
						break;
					}

					if (s1 <= target + tolerance) {
						state = TOIState.Touching;
						tOut = t1;
						done = true;
						break;
					}

					int rootIterCount = 0;
					float a1 = t1, a2 = t2;
					while(true) {
						float t;
						if ((rootIterCount & 1) != 0) {
							t = a1 + (target - s1) * (a2 - a1) / (s2 - s1);
						} else {
							t = 0.5f * (a1 + a2);
						}

						rootIterCount++;
						Interlocked.Increment(ref TOIRootIters);

						float s = fcn.Evaluate(indexA, indexB, t);

						if (Math.Abs(s - target) < tolerance) {
							t2 = t;
							break;
						}

						if (s > target) {
							a1 = t;
							s1 = s;
						} else {
							a2 = t;
							s2 = s;
						}

						if (rootIterCount == 50) break;
					}

					lock(lockTOIMaxRootIters) {
						TOIMaxRootIters = Math.Max(TOIMaxRootIters, rootIterCount);
					}

					if (++pushBackIter == MaxPolygonVertices) break;
				}

				iter++;
				Interlocked.Increment(ref TOIIters);

				if (done) break;

				if (iter == MaxIterations) {
					state = TOIState.Failed;
					tOut = t1;
					break;
				}
			}

			lock(lockTOIMaxIters) {
				TOIMaxIters = Math.Max(TOIMaxIters, iter);
			}

			float time = timer.Milliseconds;
			lock(lockTOITime) {
				TOIMaxTime = Math.Max(TOIMaxTime, time);
				TOITime += time;
			}

			return state;
		}

	}

}
