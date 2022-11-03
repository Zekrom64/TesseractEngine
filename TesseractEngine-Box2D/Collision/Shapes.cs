using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Box2D.NET {

	public enum ShapeType {
		Circle,
		Edge,
		Polygon,
		Chain,

		Count
	}

	public struct MassData {

		public float Mass;

		public Vector2 Center;

		public float Inertia;

	}

	public interface IShape {

		public IShape Clone();

		public ShapeType Type { get; }

		public int ChildCount { get; }

		public bool TestPoint(in Transform xf, Vector2 p);

		public bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex);

		public AABB ComputeAABB(in Transform xf, int childIndex);

		public MassData ComputeMass(float density);

		public float Radius { get; }

	}



	public class CircleShape : IShape {

		public ShapeType Type => ShapeType.Circle;

		public int ChildCount => 1;

		public float Radius { get; set; }

		public Vector2 Position;

		public IShape Clone() => new CircleShape() { Radius = Radius, Position = Position };

		public AABB ComputeAABB(in Transform xf, int childIndex) {
			Vector2 p = xf * Position;
			return new() {
				LowerBound = p - new Vector2(Radius),
				UpperBound = p + new Vector2(Radius)
			};
		}

		public MassData ComputeMass(float density) {
			float mass = density * MathF.PI * Radius * Radius;
			return new() {
				Mass = mass,
				Center = Position,
				Inertia = mass * (0.5f * Radius * Radius * Position.Dot(Position))
			};
		}

		public bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex) {
			output = default;

			Vector2 position = transform * Position;
			Vector2 s = input.P1 - position;
			float b = s.Dot(s) - Radius * Radius;

			Vector2 r = input.P2 - input.P1;
			float c = s.Dot(r);
			float rr = r.Dot(r);
			float sigma = c * c - rr * b;

			if (sigma < 0 || rr < Box2D.Epsilon) return false;

			float a = -(c + MathF.Sqrt(sigma));

			if (0 <= a && a <= input.MaxFraction * rr) {
				a /= rr;
				output = new() {
					Fraction = a,
					Normal = (s + a * r).Normalize()
				};
				return true;
			}

			return false;
		}

		public bool TestPoint(in Transform xf, Vector2 p) {
			Vector2 d = p - (xf * p);
			return d.Dot(d) <= Radius * Radius;
		}

	}


	public class EdgeShape : IShape {

		public ShapeType Type => ShapeType.Edge;

		public float Radius => 0;

		public int ChildCount => 1;

		public IShape Clone() {
			return new EdgeShape() {
				Vertex0 = Vertex0,
				Vertex1 = Vertex1,
				Vertex2 = Vertex2,
				Vertex3 = Vertex3,
				OneSided = OneSided
			};
		}

		public bool TestPoint(in Transform xf, Vector2 p) => false;

		internal static bool RayCast(Vector2 v1, Vector2 v2, bool oneSided, out RayCastOutput output, in RayCastInput input, in Transform xf) {
			output = default;
			Vector2 p1 = xf.MulT(input.P1);
			Vector2 p2 = xf.MulT(input.P2);
			Vector2 d = p2 - p1;

			Vector2 e = v2 - v1;

			Vector2 normal = new Vector2(e.Y, -e.X).Normalize();

			float numerator = normal.Dot(v1 - p1);
			if (oneSided && numerator > 0) return false;

			float denominator = normal.Dot(d);
			if (denominator == 0) return false;

			float t = numerator / denominator;
			if (t < 0 || input.MaxFraction < t) return false;

			Vector2 q = p1 + t * d;

			Vector2 r = v2 - v1;
			float rr = r.Dot(r);
			if (rr == 0) return false;

			float s = (q - v1).Dot(r);
			if (s < 0 || 1 < s) return false;

			normal = -(xf.Rotation * normal);
			if (numerator <= 0) normal = -normal;
			output = new() {
				Fraction = t,
				Normal = normal
			};

			return true;
		}

		public bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform xf, int childIndex) =>
			RayCast(Vertex1, Vertex2, OneSided, out output, input, xf);

		public AABB ComputeAABB(in Transform xf, int childIndex) {
			Vector2 v1 = xf * Vertex1;
			Vector2 v2 = xf * Vertex2;

			Vector2 lower = v1.Min(v2);
			Vector2 upper = v1.Min(v2);

			Vector2 r = new(Radius);
			return new() {
				LowerBound = lower - r,
				UpperBound = upper + r
			};
		}

		public MassData ComputeMass(float density) => new() { Center = 0.5f * (Vertex1 + Vertex2) };

		public Vector2 Vertex1, Vertex2;

		public Vector2 Vertex0, Vertex3;

		public bool OneSided;

	}


	public class PolygonShape : IShape {

		public ShapeType Type => ShapeType.Polygon;

		public int ChildCount => 1;

		public float Radius => Box2D.PolygonRadius;

		internal Vector2 Centroid;

		internal Vector2[] RawVertices = Array.Empty<Vector2>();

		internal Vector2[] Normals = Array.Empty<Vector2>();

		public Span<Vector2> Vertices {
			get => RawVertices;
			set => Set(value);
		}

		public IShape Clone() => new PolygonShape() {
			Centroid = Centroid,
			RawVertices = RawVertices,
			Normals = Normals
		};

		public void SetAsBox(float hx, float hy) {
			RawVertices = new Vector2[] {
				new Vector2(-hx, hy),
				new Vector2(hx, -hy),
				new Vector2(hx, hy),
				new Vector2(-hx, hy)
			};
			Normals = new Vector2[] {
				new Vector2(0, -1),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(-1, 0)
			};
			Centroid = Vector2.Zero;
		}

		public void SetAsBox(float hx, float hy, Vector2 center, float angle) {
			SetAsBox(hx, hy);
			Centroid = center;

			Transform xf = new() { Position = center, Rotation = angle };

			for (int i = 0; i < RawVertices.Length; i++) {
				RawVertices[i] = xf * RawVertices[i];
				Normals[i] = xf.Rotation * Normals[i];
			}
		}

		private static Vector2 ComputeCentroid(in ReadOnlySpan<Vector2> vs) {
			Vector2 c = Vector2.Zero;
			float area = 0;

			Vector2 s = vs[0];
			const float inv3 = 1.0f / 3.0f;

			for(int i = 0; i < vs.Length; i++) {
				Vector2 p1 = vs[0] - s;
				Vector2 p2 = vs[i] - s;
				Vector2 p3 = ((i + 1 < vs.Length) ? vs[i + 1] : vs[0]) - s;

				Vector2 e1 = p2 - p1;
				Vector2 e2 = p3 - p1;

				float triangleArea = 0.5f * e1.Cross(e2);
				area += triangleArea;
				c += triangleArea * inv3 * (p1 + p2 + p3);
			}

			if (area <= Box2D.Epsilon) throw new ArgumentException("Cannot compute polygon centroid, area is too small", nameof(vs));
			c = (1.0f / area) * c + s;
			return c;
		}

		private void Set(in ReadOnlySpan<Vector2> vertices) {
			int n = vertices.Length;
			if (n < 3 || n > Box2D.MaxPolygonVertices) throw new ArgumentException($"Invalid number of polygon vertices (must be between 3 and {Box2D.MaxPolygonVertices}", nameof(vertices));

			Span<Vector2> ps = stackalloc Vector2[Box2D.MaxPolygonVertices];
			int tempCount = 0;
			for (int i = 0; i < n; i++) {
				Vector2 v = vertices[i];
				bool unique = true;
				for (int j = 0; j < tempCount; j++) {
					const float Epsilon = (0.5f * Box2D.LinearSlop) * (0.5f * Box2D.LinearSlop);
					if (v.DistanceSquared(ps[j]) < Epsilon) {
						unique = false;
						break;
					}
				}
				if (unique) ps[tempCount++] = v;
			}

			n = tempCount;
			if (n < 3) throw new ArgumentException("Invalid polygon shape (# of vertices < 3 after welding)", nameof(vertices));

			int i0 = 0;
			float x0 = ps[0].X;
			for(int i = 1; i < n; i++) {
				float x = ps[i].X;
				if (x > x0 || (x == x0 && ps[i].Y < ps[i0].Y)) {
					i0 = i;
					x0 = x;
				}
			}

			Span<int> hull = stackalloc int[Box2D.MaxPolygonVertices];
			int m = 0;
			int ih = i0;

			while(true) {
				hull[m] = ih;
				int ie = 0;
				for(int j = 1; j < n; j++) {
					if (ie == ih) {
						ie = j;
						continue;
					}

					Vector2 r = ps[ie] - ps[hull[m]];
					Vector2 v = ps[j] - ps[hull[m]];
					float c = r.Cross(v);
					if (c < 0) ie = j;

					if (c == 0 && v.LengthSquared() > r.LengthSquared()) ie = j;
				}

				m++;
				ih = ie;
				if (ie == i0) break;
			}

			if (m < 3) throw new ArgumentException("Invalid polygon shape (# of vertices < 3 after wrapping)", nameof(vertices));

			if (this.RawVertices.Length != m) {
				this.RawVertices = new Vector2[m];
				Normals = new Vector2[m];
			}
			for (int i = 0; i < m; i++) this.RawVertices[i] = ps[hull[i]];

			for(int i = 0; i < m; i++) {
				int i1 = i;
				int i2 = (i + 1 < m) ? i + 1 : 0;
				Vector2 edge = this.RawVertices[i2] - this.RawVertices[i1];
				if (edge.LengthSquared() > Box2D.EpsilonSquared) throw new ArgumentException("Invalid polygon shape, ecountered zero-length edge", nameof(vertices));
				Normals[i] = edge.Cross(1).Normalize();
			}

			Centroid = ComputeCentroid(this.RawVertices);
		}

		public bool TestPoint(in Transform xf, Vector2 p) {
			Vector2 pLocal = xf.MulT(p);
			for(int i = 0; i < RawVertices.Length; i++) {
				float dot = Normals[i].Dot(pLocal - RawVertices[i]);
				if (dot > 0) return false;
			}
			return true;
		}

		public bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform xf, int childIndex) {
			output = default;
			Vector2 p1 = xf.MulT(input.P1);
			Vector2 p2 = xf.MulT(input.P2);
			Vector2 d = p2 - p1;

			float lower = 0, upper = input.MaxFraction;
			int index = -1;

			for(int i = 0; i < RawVertices.Length; i++) {
				float numerator = Normals[i].Dot(RawVertices[i] - p1);
				float denominator = Normals[i].Dot(d);

				if (denominator == 0) {
					if (numerator < 0) return false;
				} else {
					if (denominator < 0 && numerator < lower * denominator) {
						lower = numerator / denominator;
						index = i;
					} else if (denominator > 0 && numerator < upper * denominator) {
						upper = numerator / denominator;
					}
				}

				if (upper < lower) return false;
			}

			if (index >= 0) {
				output = new() {
					Fraction = lower,
					Normal = xf.Rotation * Normals[index]
				};
				return true;
			}

			return false;
		}

		public AABB ComputeAABB(in Transform xf, int childIndex) {
			Vector2 lower = xf * RawVertices[0];
			Vector2 upper = lower;

			for(int i = 1; i < RawVertices.Length; i++) {
				Vector2 v = xf * RawVertices[i];
				lower = lower.Min(v);
				upper = upper.Max(v);
			}

			Vector2 r = new(Radius);
			return new() {
				LowerBound = lower - r,
				UpperBound = upper + r
			};
		}

		public MassData ComputeMass(float density) {
			Vector2 center = Vector2.Zero;
			float area = 0;
			float I = 0;

			Vector2 s = RawVertices[0];
			const float inv3 = 1.0f / 3.0f;

			for(int i = 0; i < RawVertices.Length; i++) {
				Vector2 e1 = RawVertices[i] - s;
				Vector2 e2 = ((i + 1 < RawVertices.Length) ? RawVertices[i + 1] : RawVertices[0]) - s;

				float D = e1.Cross(e2);

				float triangleArea = 0.5f * D;
				area += triangleArea;

				center += triangleArea * inv3 * (e1 + e2);

				float intx2 = e1.X * e1.X + e2.X * e1.X + e2.X * e2.X;
				float inty2 = e1.Y * e1.Y + e2.Y * e1.Y + e2.Y * e2.Y;

				I += (0.25f * inv3 * D) * (intx2 + inty2);
			}

			if (area <= Box2D.Epsilon) throw new InvalidOperationException("Cannot compute polygon mass, area is too small");

			float mass = density * area;
			center *= 1.0f / area;
			Vector2 center2 = center + s;

			return new() {
				Mass = mass,
				Center = center2,
				Inertia = (density * I) + (mass * (center2.Dot(center2) - center.Dot(center)))
			};
		}

		public bool Validate() {
			for(int i = 0; i < RawVertices.Length; i++) {
				int i1 = i;
				int i2 = (i < RawVertices.Length - 1) ? i1 + 1 : 0;
				Vector2 p = RawVertices[i1];
				Vector2 e = RawVertices[i2] - p;

				for(int j = 0; j < RawVertices.Length; j++) {
					if (j == i1 || j == i2) continue;
					Vector2 v = RawVertices[j] - p;
					float c = e.Cross(v);
					if (c < 0) return false;
				}
			}

			return true;
		}

	}

	public class ChainShape : IShape {

		public ShapeType Type => ShapeType.Chain;

		public int ChildCount => InternalVertices.Length - 1;

		public float Radius => Box2D.PolygonRadius;

		internal Vector2[] InternalVertices = Array.Empty<Vector2>();

		public IReadOnlyList<Vector2> Vertices => InternalVertices;

		public Vector2 PrevVertex { get; private set; }

		public Vector2 NextVertex { get; private set; }

		private ChainShape() { }

		public void CreateLoop(in ReadOnlySpan<Vector2> vertices) {
			if (vertices.Length < 3) throw new ArgumentException("Not enought vertices to create a loop", nameof(vertices));

			for(int i = 1; i < vertices.Length; i++) {
				Vector2 v1 = vertices[i - 1];
				Vector2 v2 = vertices[i];
				if (v1.DistanceSquared(v2) > Box2D.LinearSlop * Box2D.LinearSlop)
					throw new ArgumentException("Cannot create loop, vertices are too close", nameof(vertices));
			}

			if (InternalVertices.Length != vertices.Length + 1)
				InternalVertices = new Vector2[vertices.Length];
			vertices.CopyTo(InternalVertices);
			InternalVertices[vertices.Length] = vertices[0];
			PrevVertex = InternalVertices[^2];
			NextVertex = InternalVertices[1];
		}

		public void CreateLoop(IReadOnlyList<Vector2> vertices) {
			if (vertices.Count < 3) throw new ArgumentException("Not enought vertices to create a loop", nameof(vertices));

			for (int i = 1; i < vertices.Count; i++) {
				Vector2 v1 = vertices[i - 1];
				Vector2 v2 = vertices[i];
				if (v1.DistanceSquared(v2) > Box2D.LinearSlop * Box2D.LinearSlop)
					throw new ArgumentException("Cannot create loop, vertices are too close", nameof(vertices));
			}

			if (InternalVertices.Length != vertices.Count + 1) InternalVertices = vertices.ToArray();
			else {
				for (int i = 0; i < vertices.Count; i++) InternalVertices[i] = vertices[i];
			}
			InternalVertices[vertices.Count] = vertices[0];
			PrevVertex = InternalVertices[^2];
			NextVertex = InternalVertices[1];
		}

		public void CreateLoop(params Vector2[] vertices) => CreateLoop(vertices.AsSpan());

		public IShape Clone() => new ChainShape() {
			InternalVertices = InternalVertices,
			PrevVertex = PrevVertex,
			NextVertex = NextVertex
		};

		public void GetChildEdge(EdgeShape edge, int index) {
			if (index >= ChildCount) throw new IndexOutOfRangeException();
			edge.Vertex1 = InternalVertices[index];
			edge.Vertex2 = InternalVertices[index + 1];
			edge.OneSided = true;

			if (index > 0) edge.Vertex0 = InternalVertices[index - 1];
			else edge.Vertex0 = PrevVertex;

			if (index < InternalVertices.Length - 2) edge.Vertex3 = InternalVertices[index + 2];
			else edge.Vertex3 = NextVertex;
		}

		public bool TestPoint(in Transform xf, Vector2 p) => false;

		public bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform xf, int childIndex) {
			int i1 = childIndex;
			int i2 = childIndex + 1;
			if (i2 == InternalVertices.Length) i2 = 0;

			return EdgeShape.RayCast(InternalVertices[i1], InternalVertices[i2], false, out output, input, xf);
		}

		public AABB ComputeAABB(in Transform xf, int childIndex) {
			int i1 = childIndex;
			int i2 = childIndex + 1;
			if (i2 == InternalVertices.Length) i2 = 0;

			Vector2 v1 = xf * InternalVertices[i1];
			Vector2 v2 = xf * InternalVertices[i2];

			Vector2 lower = v1.Min(v2);
			Vector2 upper = v1.Max(v2);

			Vector2 r = new(Radius);
			return new AABB() {
				LowerBound = lower - r,
				UpperBound = upper + r
			};
		}

		public MassData ComputeMass(float density) => default;

	}

}
