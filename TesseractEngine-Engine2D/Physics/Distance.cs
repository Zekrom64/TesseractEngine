using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tesseract.Core.Numerics;
using Tesseract.Core.Utilities;

namespace Tesseract.Engine2D.Physics {
	
	public ref struct DistanceProxy {

		private Vector2[] buffer = Array.Empty<Vector2>();
		public ReadOnlySpan<Vector2> Vertices;
		public int Count;
		public float Radius;

		public DistanceProxy() {
			Vertices = Span<Vector2>.Empty;
			Count = 0;
			Radius = 0;
		}

		public void Set(IShape shape, int index) {
			switch(shape.Type) {
				case ShapeType.Circle:
					CircleShape circle = (CircleShape)shape;
					if (buffer.Length < 1) buffer = new Vector2[] { circle.Position };
					else buffer[0] = circle.Position;
					Vertices = buffer;
					Count = 1;
					Radius = circle.Radius;
					break;
				case ShapeType.Polygon:
					PolygonShape polygon = (PolygonShape)shape;
					Vertices = polygon.Vertices;
					Count = Vertices.Length;
					Radius = polygon.Radius;
					break;
				case ShapeType.Chain:
					ChainShape chain = (ChainShape)shape;
					if (buffer.Length < 2) buffer = new Vector2[2];
					buffer[0] = chain.Vertices[index];
					if (index + 1 < chain.Vertices.Count) buffer[1] = chain.Vertices[index + 1];
					else buffer[1] = chain.Vertices[0];
					Vertices = buffer;
					Count = 2;
					Radius = chain.Radius;
					break;
				case ShapeType.Edge:
					EdgeShape edge = (EdgeShape)shape;
					if (buffer.Length < 1) buffer = new Vector2[] { edge.Vertex1, edge.Vertex2 };
					else {
						buffer[0] = edge.Vertex1;
						buffer[1] = edge.Vertex2;
					}
					Count = 2;
					Radius = edge.Radius;
					break;
			}
		}

		public void Set(in ReadOnlySpan<Vector2> vertices, float radius) {
			Vertices = vertices;
			Count = vertices.Length;
			Radius = radius;
		}

		public int GetSupport(Vector2 d) {
			int bestIndex = 0;
			float bestValue = Vertices[0].Dot(d);
			for(int i = 1; i < Count; i++) {
				float value = Vertices[i].Dot(d);
				if (value > bestValue) {
					bestIndex = i;
					bestValue = value;
				}
			}
			return bestIndex;
		}

		public Vector2 GetSupportVertex(Vector2 d) => Vertices[GetSupport(d)];

	}

	public struct SimplexCache {

		public float Metric;

		public ushort Count;

		public unsafe fixed byte IndexA[3];

		public unsafe fixed byte IndexB[3];

	}

	public readonly ref struct DistanceInput {

		public DistanceProxy ProxyA { get; init; }

		public DistanceProxy ProxyB { get; init; }

		public Transform TransformA { get; init; }

		public Transform TransformB { get; init; }

		public bool UseRadii { get; init; }

	}

	public struct DistanceOutput {

		public Vector2 PointA;

		public Vector2 PointB;

		public float Distance;

		public int Iterations;

	}

	public ref struct ShapeCastInput {

		public DistanceProxy ProxyA { get; init; }

		public DistanceProxy ProxyB { get; init; }

		public Transform TransformA { get; init; }

		public Transform TransformB { get; init; }

		public Vector2 TranslationB { get; init; }

	}

	public struct ShapeCastOutput {

		public Vector2 Point;

		public Vector2 Normal;

		public float Lambda;

		public int Iterations;

	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct SimplexVertex {

		public Vector2 WA;

		public Vector2 WB;

		public Vector2 W;

		public float A;

		public int IndexA;

		public int IndexB;

	}

	[StructLayout(LayoutKind.Sequential)]
	internal ref struct Simplex {

		public void ReadCache(ref SimplexCache cache, DistanceProxy proxyA, Transform transformA, DistanceProxy proxyB, Transform transformB) {
			Count = cache.Count;
			
			unsafe {
				fixed (SimplexVertex* pV = &V1) {
					Span<SimplexVertex> vertices = new(pV, 3);
					for (int i = 0; i < Count; i++) {
						ref SimplexVertex v = ref vertices[i];
						v.IndexA = cache.IndexA[i];
						v.IndexB = cache.IndexB[i];
						Vector2 wALocal = proxyA.Vertices[v.IndexA];
						Vector2 wBLocal = proxyB.Vertices[v.IndexB];
						v.WA = transformA * wALocal;
						v.WB = transformB * wBLocal;
						v.W = v.WB - v.WA;
						v.A = 0;
					}
				}
			}

			if (Count > 1) {
				float metric1 = cache.Metric;
				float metric2 = GetMetric();
				if (metric2 < 0.5f * metric1 || 2 * metric1 < metric2 || metric2 < Box2D.Epsilon) Count = 0;
			}

			if (Count == 0) {
				Vector2 wALocal = proxyA.Vertices[0];
				Vector2 wBLocal = proxyB.Vertices[0];
				Vector2 wA = transformA * wALocal;
				Vector2 wB = transformB * wBLocal;
				V1 = new() {
					IndexA = 0,
					IndexB = 0,
					WA = wA,
					WB = wB,
					W = wB - wA,
					A = 1
				};
				Count = 1;
			}
		}

		public void WriteCache(ref SimplexCache cache) {
			cache.Metric = GetMetric();
			cache.Count = (ushort)Count;
			unsafe {
				fixed (SimplexVertex* pV = &V1) {
					Span<SimplexVertex> vertices = new(pV, 3);
					for(int i = 0; i < Count; i++) {
						cache.IndexA[i] = (byte)vertices[i].IndexA;
						cache.IndexB[i] = (byte)vertices[i].IndexB;
					}
				}
			}
		}

		public Vector2 GetSearchDirection() {
			switch(Count) {
				case 1:
					return -V1.W;
				case 2:
					Vector2 e12 = V2.W - V1.W;
					float sgn = e12.Cross(-V1.W);
					if (sgn > 0) return e12.CrossT(1);
					else return e12.Cross(1);
				default:
					throw new InvalidOperationException("Cannot get search direction for simplex with count <1 or >2");
			}
		}

		public Vector2 GetClosestPoint() => Count switch {
			1 => V1.W,
			2 => V1.A * V1.W + V2.A * V2.W,
			_ => throw new InvalidOperationException("Cannot get closest point for simplex with count <1 or >3"),
		};

		public (Vector2, Vector2) GetWitnessPoints() => Count switch {
			1 => (V1.WA, V1.WB),
			2 => (
				V1.A * V1.WA + V2.A * V2.WA,
				V1.A * V1.WB + V2.A * V2.WB
			),
			3 => Collections.TupleDup2(V1.A * V1.WA + V2.A * V2.WA + V3.A * V3.WA),
			_ => throw new InvalidOperationException("Cannot get witness points for simplex with count <1 or >3"),
		};

		public float GetMetric() => Count switch {
			1 => 0,
			2 => V1.W.Distance(V2.W),
			3 => (V2.W - V1.W).Cross(V3.W - V1.W),
			_ => throw new InvalidOperationException("Cannot get metric for simplex with count <1 or >3"),
		};

		public void Solve2() {
			Vector2 w1 = V1.W;
			Vector2 w2 = V2.W;
			Vector2 e12 = w2 - w1;

			float d12_2 = -w1.Dot(e12);
			if (d12_2 <= 0) {
				V1.A = 1;
				Count = 1;
				return;
			}

			float d12_1 = w2.Dot(e12);
			if (d12_1 <= 0) {
				V2.A = 1;
				Count = 1;
				V1 = V2;
				return;
			}

			float inv_d12 = 1 / (d12_1 + d12_2);
			V1.A = d12_1 * inv_d12;
			V2.A = d12_2 * inv_d12;
			Count = 2;
		}

		public void Solve3() {
			Vector2 w1 = V1.W;
			Vector2 w2 = V2.W;
			Vector2 w3 = V3.W;

			Vector2 e12 = w2 - w1;
			float w1e12 = w1.Dot(e12);
			float w2e12 = w2.Dot(e12);
			float d12_1 = w2e12;
			float d12_2 = -w1e12;

			Vector2 e13 = w3 - w1;
			float w1e13 = w1.Dot(e13);
			float w3e13 = w3.Dot(e13);
			float d13_1 = w3e13;
			float d13_2 = -w1e13;

			Vector2 e23 = w3 - w2;
			float w2e23 = w2.Dot(e23);
			float w3e23 = w3.Dot(e23);
			float d23_1 = w3e23;
			float d23_2 = -w2e23;

			float n123 = e12.Cross(e13);

			float d123_1 = n123 * w2.Cross(w3);
			float d123_2 = n123 * w3.Cross(w1);
			float d123_3 = n123 * w1.Cross(w2);

			if (d12_2 <= 0 && d13_2 <= 0) {
				V1.A = 1;
				Count = 1;
				return;
			}

			if (d12_1 > 0 && d12_2 > 0 && d123_3 <= 0) {
				float inv_d12 = 1 / (d12_1 + d12_2);
				V1.A = d12_1 * inv_d12;
				V2.A = d12_2 * inv_d12;
				Count = 2;
				return;
			}

			if (d13_1 > 0 && d13_2 > 0 && d123_2 <= 0) {
				float inv_d13 = 1 / (d13_1 + d13_2);
				V1.A = d13_1 * inv_d13;
				V3.A = d13_2 * inv_d13;
				Count = 2;
				V2 = V3;
				return;
			}

			if (d12_1 <= 0 && d23_2 <= 0) {
				V2.A = 1;
				Count = 1;
				V1 = V2;
				return;
			}

			if (d13_1 <= 0 && d23_1 <= 0) {
				V3.A = 1;
				Count = 1;
				V1 = V3;
				return;
			}

			if (d23_1 > 0 && d23_2 > 0 && d123_1 <= 0) {
				float inv_d23 = 1 / (d23_1 + d23_2);
				V2.A = d23_1 * inv_d23;
				V3.A = d23_2 * inv_d23;
				Count = 2;
				V1 = V3;
				return;
			}

			float inv_d123 = 1 / (d123_1 + d123_2 + d123_3);
			V1.A = d123_1 * inv_d123;
			V2.A = d123_2 * inv_d123;
			V3.A = d123_3 * inv_d123;
			Count = 3;
		}

		public SimplexVertex V1, V2, V3;
		public int Count;

	}

	public static partial class Box2D {

		internal static int GJKCalls, GJKIters, GJKMaxIters;

		internal static DistanceOutput Distance(ref SimplexCache cache, in DistanceInput input) {
			GJKCalls++;

			DistanceProxy proxyA = input.ProxyA;
			DistanceProxy proxyB = input.ProxyB;

			Transform transformA = input.TransformA;
			Transform transformB = input.TransformB;

			Simplex simplex = new();
			simplex.ReadCache(ref cache, proxyA, transformA, proxyB, transformB);

			const int MaxIters = 20;
			int iter = 0;
			unsafe {
				Span<SimplexVertex> vertices = new(&simplex.V1, 3);

				Span<int> saveA = stackalloc int[3], saveB = stackalloc int[3];
				int saveCount = 0;

				while(iter < MaxIters) {
					saveCount = simplex.Count;
					for(int i = 0; i < saveCount; i++) {
						saveA[i] = vertices[i].IndexA;
						saveB[i] = vertices[i].IndexB;
					}

					switch(simplex.Count) {
						case 1:
							break;
						case 2:
							simplex.Solve2();
							break;
						case 3:
							simplex.Solve3();
							break;
					}

					if (simplex.Count == 3) break;

					Vector2 d = simplex.GetSearchDirection();

					if (d.LengthSquared() < EpsilonSquared) break;

					ref SimplexVertex vertex = ref vertices[simplex.Count];
					vertex.IndexA = proxyA.GetSupport(transformA.Rotation.MulT(-d));
					vertex.WA = transformA * proxyA.Vertices[vertex.IndexA];
					vertex.IndexB = proxyB.GetSupport(transformB.Rotation.MulT(d));
					vertex.WB = transformB * proxyB.Vertices[vertex.IndexB];
					vertex.W = vertex.WB - vertex.WA;

					iter++;
					GJKIters++;

					bool duplicate = false;
					for(int i = 0; i < saveCount; i++) {
						if (vertex.IndexA == saveA[i] && vertex.IndexB == saveB[i]) {
							duplicate = true;
							break;
						}
					}

					if (duplicate) break;

					simplex.Count++;
				}
			}

			GJKMaxIters = Math.Max(GJKMaxIters, iter);

			DistanceOutput output = new();
			(output.PointA, output.PointB) = simplex.GetWitnessPoints();
			output.Distance = output.PointA.Distance(output.PointB);
			output.Iterations = iter;

			simplex.WriteCache(ref cache);

			if (input.UseRadii) {
				if (output.Distance < Epsilon) {
					Vector2 p = 0.5f * (output.PointA + output.PointB);
					output.PointA = p;
					output.PointB = p;
					output.Distance = 0;
				} else {
					float rA = proxyA.Radius;
					float rB = proxyB.Radius;
					Vector2 normal = (output.PointB - output.PointA).Normalize();
					output.Distance = Math.Max(0, output.Distance - rA - rB);
					output.PointA += rA * normal;
					output.PointB -= rB * normal;
				}
			}

			return output;
		}

		internal static bool ShapeCast(out ShapeCastOutput output, in ShapeCastInput input) {
			output = new() {
				Iterations = 0,
				Lambda = 1,
				Normal = Vector2.Zero,
				Point = Vector2.Zero
			};

			DistanceProxy proxyA = input.ProxyA;
			DistanceProxy proxyB = input.ProxyB;

			float radiusA = Math.Max(proxyA.Radius, PolygonRadius);
			float radiusB = Math.Max(proxyB.Radius, PolygonRadius);
			float radius = radiusA + radiusB;

			Transform xfA = input.TransformA;
			Transform xfB = input.TransformB;

			Vector2 r = input.TranslationB;
			Vector2 n = Vector2.Zero;
			float lambda = 0;

			Simplex simplex = new() { Count = 0 };

			unsafe {
				Span<SimplexVertex> vertices = new(&simplex.V1, 3);

				int indexA = proxyA.GetSupport(xfA.Rotation.MulT(-r));
				Vector2 wA = xfA * proxyA.Vertices[indexA];
				int indexB = proxyB.GetSupport(xfB.Rotation.MulT(r));
				Vector2 wB = xfB * proxyB.Vertices[indexB];
				Vector2 v = wA - wB;

				float sigma = Math.Max(PolygonRadius, radius - PolygonRadius);
				const float Tolerance = 0.5f * LinearSlop;

				const int MaxIters = 20;
				int iter = 0;
				while (iter < MaxIters && v.Length() - sigma > Tolerance) {
					output.Iterations += 1;

					indexA = proxyA.GetSupport(xfA.Rotation.MulT(-v));
					wA = xfA * proxyA.Vertices[indexA];
					indexB = proxyB.GetSupport(xfB.Rotation.MulT(v));
					wB = xfB * proxyB.Vertices[indexB];
					Vector2 p = wA - wB;

					v = v.Normalize();

					float vp = v.Dot(p);
					float vr = v.Dot(r);
					if (vp - sigma > lambda * vr) {
						if (vr <= 0) return false;
						lambda = (vp - sigma) / vr;
						if (lambda > 1) return false;
						n = -v;
						simplex.Count = 0;
					}

					ref SimplexVertex vertex = ref vertices[simplex.Count];
					vertex.IndexA = indexB;
					vertex.WA = wB + lambda * r;
					vertex.IndexB = indexA;
					vertex.WB = wA;
					vertex.W = vertex.WB - vertex.WA;
					vertex.A = 1;
					simplex.Count++;

					switch(simplex.Count) {
						case 1:
							break;
						case 2:
							simplex.Solve2();
							break;
						case 3:
							simplex.Solve3();
							break;
					}

					if (simplex.Count == 3) return false;

					v = simplex.GetClosestPoint();

					iter++;
				}

				if (iter == 0) return false;

				(Vector2 pointA, Vector2 _) = simplex.GetWitnessPoints();
				if (v.LengthSquared() > 0) {
					n = -v;
					n = n.Normalize();
				}

				output.Point = pointA + radiusA * n;
				output.Normal = n;
				output.Lambda = lambda;
				output.Iterations = iter;
			}

			return true;
		}

	}

}
