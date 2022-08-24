using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Engine2D.Physics {
	
	public enum ContactFeatureType : byte {
		Vertex,
		Face
	}

	public readonly record struct ContactFeature {

		public byte IndexA { get; init; }

		public byte IndexB { get; init; }

		public ContactFeatureType TypeA { get; init; }

		public ContactFeatureType TypeB { get; init; }

	}

	public readonly record struct ContactID {

		public ContactFeature Feature { get; init; }

		public uint Key { get; init; }

	}

	public readonly record struct ManifoldPoint {

		public Vector2 LocalPoint { get; init; }

		public float NormalImpulse { get; init; }

		public float TangentImpulse { get; init; }

		public ContactID ID { get; init; }

	}

	public enum ManifoldType {
		Circles,
		FaceA,
		FaceB
	}

	public readonly record struct Manifold {

		public ManifoldPoint[] Points { get; init; } = new ManifoldPoint[Box2D.MaxManifoldPoints];

		public Vector2 LocalNormal { get; init; } = default;

		public Vector2 LocalPoint { get; init; } = default;

		public ManifoldType Type { get; init; } = default;

		public int PointCount { get; init; } = default;

		public Manifold() { }

	}

	public struct WorldManifold {

		public void Initialize(in Manifold manifold, in Transform xfA, float radiusA, in Transform xfB, float radiusB) {
			if (manifold.PointCount == 0) return;

			switch(manifold.Type) {
				case ManifoldType.Circles: {
						Normal = new(1, 0);
						Vector2 pointA = xfA * manifold.LocalPoint;
						Vector2 pointB = xfB * manifold.Points[0].LocalPoint;
						Vector2 diff = pointB - pointA;
						float distSqr = diff.LengthSquared();
						if (distSqr > Box2D.EpsilonSquared)
							Normal = diff / MathF.Sqrt(distSqr);
						Vector2 cA = pointA + radiusA * Normal;
						Vector2 cB = pointB - radiusB * Normal;
						Points[0] = 0.5f * (cA + cB);
						Separations[0] = (cB - cA).Dot(Normal);
					}
					break;
				case ManifoldType.FaceA: {
						Normal = xfA.Rotation * manifold.LocalNormal;
						Vector2 planePoint = xfA * manifold.LocalPoint;
						for(int i = 0; i < manifold.PointCount; i++) {
							Vector2 clipPoint = xfB * manifold.Points[i].LocalPoint;
							Vector2 cA = clipPoint + (radiusA - (clipPoint - planePoint).Dot(Normal)) * Normal;
							Vector2 cB = clipPoint - radiusB * Normal;
							Points[i] = 0.5f * (cA + cB);
							Separations[i] = (cB - cA).Dot(Normal);
						}
					}
					break;
				case ManifoldType.FaceB: {
						Normal = xfB.Rotation * manifold.LocalNormal;
						Vector2 planePoint = xfB * manifold.LocalPoint;
						for (int i = 0; i < manifold.PointCount; i++) {
							Vector2 clipPoint = xfA * manifold.Points[i].LocalPoint;
							Vector2 cA = clipPoint + (radiusB - (clipPoint - planePoint).Dot(Normal)) * Normal;
							Vector2 cB = clipPoint - radiusA * Normal;
							Points[i] = 0.5f * (cA + cB);
							Separations[i] = (cA - cB).Dot(Normal);
						}
						Normal = -Normal;
					}
					break;
			}
		}

		public Vector2 Normal = default;

		public readonly Vector2[] Points = new Vector2[Box2D.MaxManifoldPoints];
		public readonly float[] Separations = new float[Box2D.MaxManifoldPoints];

		public WorldManifold() { }

	}

	public enum PointState {
		Null,
		Add,
		Persist,
		Remove
	}

	public struct ClipVertex {
		
		public Vector2 Vertex;

		public ContactID ID;

	}

	public readonly record struct RayCastInput {

		public Vector2 P1 { get; init; }

		public Vector2 P2 { get; init; }

		public float MaxFraction { get; init; }

	}

	public readonly record struct RayCastOutput {

		public Vector2 Normal { get; init; }

		public float Fraction { get; init; }

	}

	public struct AABB {

		public bool IsValid {
			get {
				Vector2 d = UpperBound - LowerBound;
				bool valid = d.X >= 0 && d.Y >= 0;
				valid = valid && LowerBound.IsValid() && UpperBound.IsValid();
				return valid;
			}
		}

		public Vector2 Center => 0.5f * (LowerBound + UpperBound);

		public Vector2 Extents => 0.5f * (UpperBound - LowerBound);

		public float Perimeter {
			get {
				Vector2 w = UpperBound - LowerBound;
				return 2 * (w.X + w.Y);
			}
		}

		public void Combine(AABB aabb) {
			LowerBound = LowerBound.Min(aabb.LowerBound);
			UpperBound = UpperBound.Max(aabb.UpperBound);
		}

		public void Combine(AABB aabb1, AABB aabb2) {
			LowerBound = aabb1.LowerBound.Min(aabb2.LowerBound);
			UpperBound = aabb1.UpperBound.Max(aabb2.UpperBound);
		}

		public bool Contains(AABB aabb) {
			bool result = true;
			result &= LowerBound.X <= aabb.LowerBound.X;
			result &= LowerBound.Y <= aabb.LowerBound.Y;
			result &= UpperBound.X >= aabb.UpperBound.X;
			result &= UpperBound.Y >= aabb.UpperBound.Y;
			return result;
		}

		public bool Raycast(out RayCastOutput output, RayCastInput input) {
			output = default;

			float tmin = -Box2D.MaxFloat;
			float tmax = Box2D.MaxFloat;

			Vector2 p = input.P1;
			Vector2 d = input.P2 - input.P1;
			Vector2 absD = d.Abs();
			Vector2 normal = default;

			for(int i = 0; i < 2; i++) {
				if (absD.Get(i) < Box2D.Epsilon) {
					if (p.Get(i) < LowerBound.Get(i) || UpperBound.Get(i) < p.Get(i)) return false;
				} else {
					float invD = 1.0f / d.Get(i);
					float t1 = (LowerBound.Get(i) - p.Get(i)) * invD;
					float t2 = (UpperBound.Get(i) - p.Get(i)) * invD;
					float s = -1;
					if (t1 > t2) {
						(t1, t2) = (t2, t1);
						s = 1;
					}
					if (t1 > tmin) {
						normal = Vector2.Zero;
						normal.Set(i, s);
						tmin = t1;
					}
					tmax = MathF.Min(tmax, t2);
					if (tmin > tmax) return false;
				}
			}

			if (tmin < 0 || input.MaxFraction < tmin) return false;

			output = new() {
				Fraction = tmin,
				Normal = normal
			};
			return true;
		}

		public Vector2 LowerBound;

		public Vector2 UpperBound;

	}

	public static partial class Box2D {

		internal static void GetPointStates(PointState[] state1, PointState[] state2, in Manifold manifold1, in Manifold manifold2) {
			for (int i = 0; i < MaxManifoldPoints; i++)
				state1[i] = state2[i] = PointState.Null;

			for(int i = 0; i < manifold1.PointCount; i++) {
				ContactID id = manifold1.Points[i].ID;
				state1[i] = PointState.Remove;
				for(int j = 0; j < manifold2.PointCount; j++) {
					if (manifold2.Points[j].ID.Key == id.Key) {
						state1[i] = PointState.Persist;
						break;
					}
				}
			}

			for (int i = 0; i < manifold1.PointCount; i++) {
				ContactID id = manifold2.Points[i].ID;
				state2[i] = PointState.Add;
				for (int j = 0; j < manifold1.PointCount; j++) {
					if (manifold1.Points[j].ID.Key == id.Key) {
						state1[i] = PointState.Persist;
						break;
					}
				}
			}
		}

		internal static Manifold CollideCircles(CircleShape circleA, in Transform xfA, CircleShape circleB, in Transform xfB) {
			Vector2 pA = xfA * circleA.Position;
			Vector2 pB = xfB * circleB.Position;

			Vector2 d = pB - pA;
			float distSqr = d.Dot(d);
			float rA = circleA.Radius, rB = circleB.Radius;
			float radius = rA + rB;
			if (distSqr > radius * radius) return default;

			return new Manifold() {
				Type = ManifoldType.Circles,
				LocalPoint = circleB.Position,
				LocalNormal = Vector2.Zero,
				PointCount = 1,
				Points = new ManifoldPoint[] {
					new() {
						LocalPoint = circleB.Position,
						ID = default
					}
				}
			};
		}

		internal static Manifold CollidePolygonAndCircle(PolygonShape polygonA, in Transform xfA, CircleShape circleB, in Transform xfB) {
			Vector2 c = xfB * circleB.Position;
			Vector2 cLocal = xfA.MulT(c);

			int normalIndex = 0;
			float separation = -MaxFloat;
			float radius = polygonA.Radius + circleB.Radius;
			int vertexCount = polygonA.Vertices.Length;
			var vertices = polygonA.Vertices;
			var normals = polygonA.Normals;

			for(int i = 0; i < vertexCount; i++) {
				float s = normals[i].Dot(cLocal - vertices[i]);
				if (s > radius) return default;
				if (s > separation) {
					separation = s;
					normalIndex = i;
				}
			}

			int vertIndex1 = normalIndex;
			int vertIndex2 = vertIndex1 + 1 < vertexCount ? vertIndex1 + 1 : 0;
			Vector2 v1 = vertices[vertIndex1];
			Vector2 v2 = vertices[vertIndex2];

			if (separation < Epsilon) {
				return new Manifold() {
					PointCount = 1,
					Type = ManifoldType.FaceA,
					LocalNormal = normals[normalIndex],
					LocalPoint = 0.5f * (v1 + v2),
					Points = new ManifoldPoint[] {
						new() {
							LocalPoint = circleB.Position,
							ID = default
						}
					}
				};
			}

			float u1 = (cLocal - v1).Dot(v2 - v1);
			float u2 = (cLocal - v2).Dot(v1 - v2);

			if (u1 <= 0) {
				if (cLocal.DistanceSquared(v1) > radius * radius) return default;
				return new Manifold() {
					PointCount = 1,
					Type = ManifoldType.FaceA,
					LocalNormal = (cLocal - v1).Normalize(),
					LocalPoint = v1,
					Points = new ManifoldPoint[] {
						new() {
							LocalPoint = circleB.Position,
							ID = default
						}
					}
				};
			} else if (u2 <= 0) {
				if (cLocal.DistanceSquared(v2) > radius * radius) return default;
				return new Manifold() {
					PointCount = 1,
					Type = ManifoldType.FaceA,
					LocalNormal = (cLocal - v2).Normalize(),
					LocalPoint = v2,
					Points = new ManifoldPoint[] {
						new() {
							LocalPoint = circleB.Position,
							ID = default
						}
					}
				};
			} else {
				Vector2 faceCenter = 0.5f * (v1 + v2);
				float s = (cLocal - faceCenter).Dot(normals[vertIndex1]);
				if (s > radius) return default;

				return new Manifold() {
					PointCount = 1,
					Type = ManifoldType.FaceA,
					LocalNormal = normals[vertIndex1],
					LocalPoint = faceCenter,
					Points = new ManifoldPoint[] {
						new() {
							LocalPoint = circleB.Position,
							ID = default
						}
					}
				};
			}
		}

		private static float FindMaxSeparation(out int edgeIndex, PolygonShape poly1, in Transform xf1, PolygonShape poly2, in Transform xf2) {
			int count1 = poly1.Vertices.Length;
			int count2 = poly2.Vertices.Length;
			var n1s = poly1.Normals;
			var v1s = poly1.Vertices;
			var v2s = poly2.Vertices;
			Transform xf = xf2.MulT(xf1);

			int bestIndex = 0;
			float maxSeparation = -MaxFloat;
			for(int i = 0; i < count1; i++) {
				Vector2 n = xf.Rotation * n1s[i];
				Vector2 v1 = xf * v1s[i];

				float si = MaxFloat;
				for(int j = 0; j < count2; j++) {
					float sij = n.Dot(v2s[j] - v1);
					if (sij < si) si = sij;
				}

				if (si > maxSeparation) {
					maxSeparation = si;
					bestIndex = i;
				}
			}

			edgeIndex = bestIndex;
			return maxSeparation;
		}

		private static void FindIncidentEdge(Span<ClipVertex> c, PolygonShape poly1, in Transform xf1, int edge1, PolygonShape poly2, in Transform xf2) {
			var normals1 = poly1.Normals;
			int count2 = poly2.Normals.Length;
			var vertices2 = poly2.Vertices;
			var normals2 = poly2.Normals;

			Vector2 normal1 = xf2.Rotation.MulT(xf1.Rotation * normals1[edge1]);

			int index = 0;
			float minDot = MaxFloat;
			for(int i = 0; i < count2; i++) {
				float dot = normal1.Dot(normals2[i]);
				if (dot < minDot) {
					minDot = dot;
					index = i;
				}
			}

			int i1 = index;
			int i2 = i1 + 1 < count2 ? i1 + 1 : 0;

			c[0] = new() {
				Vertex = xf2 * vertices2[i1],
				ID = new() {
					Feature = new() {
						IndexA = (byte)edge1,
						IndexB = (byte)i1,
						TypeA = ContactFeatureType.Face,
						TypeB = ContactFeatureType.Vertex
					}
				}
			};

			c[1] = new() {
				Vertex = xf2 * vertices2[i2],
				ID = new() {
					Feature = new() {
						IndexA = (byte)edge1,
						IndexB = (byte)i2,
						TypeA = ContactFeatureType.Face,
						TypeB = ContactFeatureType.Vertex
					}
				}
			};
		}

		internal static Manifold CollidePolygons(PolygonShape polyA, in Transform xfA, PolygonShape polyB, in Transform xfB) {
			float totalRadius = polyA.Radius + polyB.Radius;

			float separationA = FindMaxSeparation(out int edgeA, polyA, xfA, polyB, xfB);
			if (separationA > totalRadius)
				return default;

			float separationB = FindMaxSeparation(out int edgeB, polyB, xfB, polyA, xfA);
			if (separationB > totalRadius)
				return default;

			PolygonShape poly1;
			PolygonShape poly2;
			Transform xf1, xf2;
			int edge1;
			bool flip;
			const float Tol = 0.1f * LinearSlop;
			ManifoldType manifoldType;

			if (separationB > separationA + Tol) {
				poly1 = polyB;
				poly2 = polyA;
				xf1 = xfB;
				xf2 = xfA;
				edge1 = edgeB;
				manifoldType = ManifoldType.FaceB;
				flip = true;
			} else {
				poly1 = polyA;
				poly2 = polyB;
				xf1 = xfA;
				xf2 = xfB;
				edge1 = edgeA;
				manifoldType = ManifoldType.FaceA;
				flip = false;
			}

			Span<ClipVertex> incidentEdge = stackalloc ClipVertex[2];
			FindIncidentEdge(incidentEdge, poly1, xf1, edge1, poly2, xf2);

			int count1 = poly1.Vertices.Length;
			var vertices1 = poly1.Vertices;

			int iv1 = edge1;
			int iv2 = edge1 + 1 < count1 ? edge1 + 1 : 0;

			Vector2 v11 = vertices1[iv1];
			Vector2 v12 = vertices1[iv2];

			Vector2 localTangent = (v12 - v11).Normalize();

			Vector2 localNormal = localTangent.Cross(1.0f);
			Vector2 planePoint = 0.5f * (v11 + v12);

			Vector2 tangent = xf1.Rotation * localTangent;
			Vector2 normal = tangent.Cross(1.0f);

			v11 = xf1 * v11;
			v12 = xf1 * v12;

			float frontOffset = normal.Dot(v11);

			float sideOffset1 = -tangent.Dot(v11) + totalRadius;
			float sideOffset2 = tangent.Dot(v12) + totalRadius;

			Span<ClipVertex> clipPoints1 = stackalloc ClipVertex[2];
			Span<ClipVertex> clipPoints2 = stackalloc ClipVertex[2];
			int np;

			np = ClipSegmentToLine(clipPoints1, incidentEdge, -tangent, sideOffset1, iv1);
			if (np < 2)
				return default;

			np = ClipSegmentToLine(clipPoints2, clipPoints1, tangent, sideOffset2, iv2);
			if (np < 2)
				return default;

			Span<ManifoldPoint> points = stackalloc ManifoldPoint[MaxManifoldPoints];
			int pointCount = 0;
			for(int i = 0; i < MaxManifoldPoints; i++) {
				float separation = normal.Dot(clipPoints2[i].Vertex) - frontOffset;
				if (separation <= totalRadius) {
					ref ManifoldPoint cp = ref points[pointCount];
					cp = new() {
						LocalPoint = xf2.MulT(clipPoints2[i].Vertex),
						ID = clipPoints2[i].ID
					};
					if (flip) {
						ContactFeature cf = cp.ID.Feature;
						cp = cp with {
							ID = new() {
								Feature = new() {
									IndexA = cf.IndexB,
									IndexB = cf.IndexA,
									TypeA = cf.TypeB,
									TypeB = cf.TypeA
								}
							}
						};
					}
					pointCount++;
				}
			}

			return new Manifold() {
				Type = manifoldType,
				LocalNormal = localNormal,
				LocalPoint = planePoint,
				PointCount = pointCount,
				Points = points[..pointCount].ToArray()
			};
		}

		internal static Manifold CollideEdgeAndCircle(EdgeShape edgeA, in Transform xfA, CircleShape circleB, in Transform xfB) {
			Vector2 Q = xfA.MulT(xfB * circleB.Position);
			Vector2 A = edgeA.Vertex1, B = edgeA.Vertex2;
			Vector2 e = B - A;

			Vector2 n = new(e.Y, -e.X);
			float offset = n.Dot(Q - A);

			bool oneSided = edgeA.OneSided;
			if (oneSided && offset < 0) return default;

			float u = e.Dot(B - Q);
			float v = e.Dot(Q - A);

			float radius = edgeA.Radius - circleB.Radius;

			ContactFeature cf = new() { IndexB = 0, TypeB = ContactFeatureType.Vertex };

			Vector2 P, d;
			float dd;
			if (v <= 0) {
				P = A;
				d = Q - P;
				dd = d.Dot(d);
				if (dd > radius * radius) return default;

				if (edgeA.OneSided) {
					Vector2 A1 = edgeA.Vertex0;
					Vector2 B1 = A;
					Vector2 e1 = B1 - A1;
					float u1 = e1.Dot(B1 - Q);
					if (u1 > 0) return default;
				}

				cf = cf with {
					IndexA = 0,
					TypeA = ContactFeatureType.Vertex
				};
				return new Manifold() {
					PointCount = 1,
					Type = ManifoldType.Circles,
					LocalNormal = Vector2.Zero,
					LocalPoint = P,
					Points = new ManifoldPoint[] {
						new() {
							ID = new() {
								Key = 0,
								Feature = cf
							},
							LocalPoint = circleB.Position
						}
					}
				};
			}

			float den = e.Dot(e);
			P = (1 / den) * (u * A + v * B);
			d = Q - P;
			dd = d.Dot(d);
			if (dd > radius * radius) return default;

			if (offset < 0) n = -n;
			n = n.Normalize();

			cf = cf with {
				IndexA = 0,
				TypeA = ContactFeatureType.Face
			};
			return new Manifold() {
				PointCount = 1,
				Type = ManifoldType.FaceA,
				LocalNormal = n,
				LocalPoint = A,
				Points = new ManifoldPoint[] {
					new() {
						ID = new() {
							Key = 0,
							Feature = cf
						},
						LocalPoint = circleB.Position
					}
				}
			};
		}

		public enum EPAxisType {
			Unknown,
			EdgeA,
			EdgeB
		}

		private struct EPAxis {

			public Vector2 Normal;
			public EPAxisType Type;
			public int Index;
			public float Separation;

		}

		private ref struct TempPolygon {

			public Span<Vector2> Vertices;
			public Span<Vector2> Normals;
			public int Count;

		}

		private struct ReferenceFace {

			public int I1, I2;
			public Vector2 V1, V2;
			public Vector2 Normal;

			public Vector2 SideNormal1;
			public float SideOffset1;

			public Vector2 SideNormal2;
			public float SideOffset2;

		}

		private static EPAxis ComputeEdgeSeparation(in TempPolygon polygonB, Vector2 v1, Vector2 normal1) {
			EPAxis axis = new() {
				Type = EPAxisType.EdgeA,
				Index = -1,
				Separation = -MaxFloat,
				Normal = Vector2.Zero
			};

			Span<Vector2> axes = stackalloc Vector2[] { normal1, -normal1 };

			for(int j = 0; j < 2; j++) {
				float sj = MaxFloat;
				for(int i = 0; i < polygonB.Count; i++) {
					float si = axes[j].Dot(polygonB.Vertices[i] - v1);
					if (si < sj) sj = si;
				}
				if (sj > axis.Separation) {
					axis.Index = j;
					axis.Separation = sj;
					axis.Normal = axes[j];
				}
			}

			return axis;
		}

		private static EPAxis ComputePolygonSeparation(in TempPolygon polygonB, Vector2 v1, Vector2 v2) {
			EPAxis axis = new() {
				Type = EPAxisType.Unknown,
				Index = -1,
				Separation = -MaxFloat,
				Normal = Vector2.Zero
			};

			for(int i = 0; i < polygonB.Count; i++) {
				Vector2 n = -polygonB.Normals[i];

				float s1 = n.Dot(polygonB.Vertices[i] - v1);
				float s2 = n.Dot(polygonB.Vertices[i] - v2);
				float s = Math.Min(s1, s2);

				if (s > axis.Separation) {
					axis = new() {
						Type = EPAxisType.EdgeB,
						Index = i,
						Separation = s,
						Normal = n
					};
				}
			}

			return axis;
		}

		internal static Manifold CollideEdgeAndPolygon(EdgeShape edgeA, in Transform xfA, PolygonShape polygonB, in Transform xfB) {
			Transform xf = xfA.MulT(xfB);
			
			Vector2 centroidB = xf * polygonB.Centroid;

			Vector2 v1 = edgeA.Vertex1;
			Vector2 v2 = edgeA.Vertex2;

			Vector2 edge1 = (v2 - v1).Normalize();

			Vector2 normal1 = new(edge1.Y, -edge1.X);
			float offset1 = normal1.Dot(centroidB - v1);

			bool oneSided = edgeA.OneSided;
			if (oneSided && offset1 < 0) return default;

			TempPolygon tempPolygonB = new() {
				Count = polygonB.Vertices.Length,
				Vertices = polygonB.Vertices,
				Normals = polygonB.Normals
			};

			float radius = polygonB.Radius + edgeA.Radius;

			EPAxis edgeAxis = ComputeEdgeSeparation(tempPolygonB, v1, normal1);
			if (edgeAxis.Separation > radius) return default;

			EPAxis polygonAxis = ComputePolygonSeparation(tempPolygonB, v1, v2);
			if (polygonAxis.Separation > radius) return default;

			const float RelativeTol = 0.98f;
			const float AbsoluteTol = 0.001f;

			EPAxis primaryAxis;
			if (polygonAxis.Separation - radius > RelativeTol * (edgeAxis.Separation - radius) + AbsoluteTol) primaryAxis = polygonAxis;
			else primaryAxis = edgeAxis;

			if (oneSided) {
				Vector2 edge0 = (v1 - edgeA.Vertex0).Normalize();
				Vector2 normal0 = new(edge0.Y, -edge0.X);
				bool convex1 = edge0.Cross(edge1) >= 0;

				Vector2 edge2 = (edgeA.Vertex3 - v2).Normalize();
				Vector2 normal2 = new(edge2.Y, -edge2.X);
				bool convex2 = edge1.Cross(edge2) >= 0;

				const float SinTol = 0.1f;
				bool side1 = primaryAxis.Normal.Dot(edge1) <= 0;

				if (side1) {
					if (convex1) {
						if (primaryAxis.Normal.Cross(normal0) > SinTol) return default;
						else primaryAxis = edgeAxis;
					} else {
						if (convex2) {
							if (normal2.Cross(primaryAxis.Normal) > SinTol) return default;
						} else primaryAxis = edgeAxis;
					}
				}
			}

			Span<ClipVertex> clipPoints = stackalloc ClipVertex[2];
			ReferenceFace rface;

			Manifold manifold = new();
			if (primaryAxis.Type == EPAxisType.EdgeA) {
				manifold = manifold with { Type = ManifoldType.FaceA };

				int bestIndex = 0;
				float bestValue = primaryAxis.Normal.Dot(tempPolygonB.Normals[0]);
				for(int i = 1; i < tempPolygonB.Count; i++) {
					float value = primaryAxis.Normal.Dot(tempPolygonB.Normals[i]);
					if (value < bestValue) {
						bestValue = value;
						bestIndex = i;
					}
				}

				int i1 = bestIndex;
				int i2 = i1 + 1 < tempPolygonB.Count ? i1 + 1 : 0;

				clipPoints[0] = new() {
					Vertex = tempPolygonB.Vertices[i1],
					ID = new() {
						Feature = new() {
							IndexA = 0,
							IndexB = (byte)i1,
							TypeA = ContactFeatureType.Face,
							TypeB = ContactFeatureType.Vertex
						}
					}
				};

				clipPoints[1] = new() {
					Vertex = tempPolygonB.Vertices[i2],
					ID = new() {
						Feature = new() {
							IndexA = 0,
							IndexB = (byte)i2,
							TypeA = ContactFeatureType.Face,
							TypeB = ContactFeatureType.Vertex
						}
					}
				};

				rface = new() {
					I1 = 0,
					I2 = 1,
					V1 = v1,
					V2 = v2,
					Normal = primaryAxis.Normal,
					SideNormal1 = -edge1,
					SideNormal2 = edge1
				};
			} else {
				manifold = manifold with { Type = ManifoldType.FaceB };

				clipPoints[0] = new() {
					Vertex = v2,
					ID = new() {
						Feature = new() {
							IndexA = 0,
							IndexB = (byte)primaryAxis.Index,
							TypeA = ContactFeatureType.Vertex,
							TypeB = ContactFeatureType.Face
						}
					}
				};

				clipPoints[1] = new() {
					Vertex = v1,
					ID = new() {
						Feature = new() {
							IndexA = 0,
							IndexB = (byte)primaryAxis.Index,
							TypeA = ContactFeatureType.Vertex,
							TypeB = ContactFeatureType.Face
						}
					}
				};

				rface = new() { I1 = primaryAxis.Index };
				rface.I2 = rface.I1 + 1 < tempPolygonB.Count ? rface.I1 + 1 : 0;
				rface.V1 = tempPolygonB.Vertices[rface.I1];
				rface.V2 = tempPolygonB.Vertices[rface.I2];
				rface.Normal = tempPolygonB.Normals[rface.I1];
				rface.SideNormal1 = new(rface.Normal.Y, -rface.Normal.X);
				rface.SideNormal2 = -rface.SideNormal1;
			}

			rface.SideOffset1 = rface.SideNormal1.Dot(rface.V1);
			rface.SideOffset2 = rface.SideNormal2.Dot(rface.V2);

			Span<ClipVertex> clipPoints1 = stackalloc ClipVertex[2];
			Span<ClipVertex> clipPoints2 = stackalloc ClipVertex[2];
			int np; 

			np = ClipSegmentToLine(clipPoints1, clipPoints, rface.SideNormal1, rface.SideOffset1, rface.I1);
			if (np < MaxManifoldPoints) return default;

			np = ClipSegmentToLine(clipPoints2, clipPoints1, rface.SideNormal2, rface.SideOffset2, rface.I2);
			if (np < MaxManifoldPoints) return default;

			if (primaryAxis.Type == EPAxisType.EdgeA) {
				manifold = manifold with {
					LocalNormal = rface.Normal,
					LocalPoint = rface.V1
				};
			} else {
				manifold = manifold with {
					LocalNormal = polygonB.Normals[rface.I1],
					LocalPoint = polygonB.Vertices[rface.I1]
				};
			}

			Span<ManifoldPoint> points = stackalloc ManifoldPoint[MaxManifoldPoints];

			int pointCount = 0;
			for(int i = 0; i < MaxManifoldPoints; i++) {
				float separation = rface.Normal.Dot(clipPoints2[i].Vertex - rface.V1);
				if (separation <= radius) {
					ref ManifoldPoint cp = ref points[pointCount];
					if (primaryAxis.Type == EPAxisType.EdgeA) {
						cp = new() {
							LocalPoint = xf.MulT(clipPoints2[i].Vertex),
							ID = clipPoints2[i].ID
						};
					} else {
						cp = new() {
							LocalPoint = clipPoints2[i].Vertex,
							ID = clipPoints2[i].ID
						};
					}
				}
			}

			return manifold with {
				PointCount = pointCount,
				Points = points[..pointCount].ToArray()
			};
		}

		internal static int ClipSegmentToLine(Span<ClipVertex> vOut, in ReadOnlySpan<ClipVertex> vIn, Vector2 normal, float offset, int vertexIndexA) {
			int count = 0;

			float distance0 = normal.Dot(vIn[0].Vertex) - offset;
			float distance1 = normal.Dot(vIn[1].Vertex) - offset;

			if (distance0 <= 0) vOut[count++] = vIn[0];
			if (distance1 <= 0) vOut[count++] = vIn[1];

			if (distance0 * distance1 < 0) {
				float interp = distance0 / (distance0 - distance1);
				vOut[count] = new ClipVertex() {
					Vertex = vIn[0].Vertex + interp * (vIn[1].Vertex - vIn[0].Vertex),
					ID = new ContactID() {
						Feature = new ContactFeature() {
							IndexA = (byte)vertexIndexA,
							IndexB = vIn[0].ID.Feature.IndexB,
							TypeA = ContactFeatureType.Vertex,
							TypeB = ContactFeatureType.Face
						}
					}
				};
				count++;
			}

			return count;

		}

		public static bool TestOverlap(IShape shapeA, int indexA, IShape shapeB, int indexB, in Transform xfA, in Transform xfB) {
			DistanceInput input = new() {
				ProxyA = new(shapeA, indexA),
				ProxyB = new(shapeB, indexB),
				TransformA = xfA,
				TransformB = xfB,
				UseRadii = true
			};

			SimplexCache cache = new();

			DistanceOutput output = Distance(ref cache, input);

			return output.Distance < 10.0f * Epsilon;
		}

		public static bool TestOverlap(AABB a, AABB b) {
			Vector2 d1, d2;
			d1 = b.LowerBound - a.UpperBound;
			d2 = a.LowerBound - b.UpperBound;

			if (d1.X > 0 || d1.Y > 0)
				return false;

			if (d2.X > 0 || d2.Y > 0)
				return false;

			return true;
		}

	}

}
