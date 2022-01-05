using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Core.Math.Dim3D {
	
	/// <summary>
	/// Interface for simple 3D shapes.
	/// </summary>
	public interface IShape3D {

		/// <summary>
		/// The center position of the shape.
		/// </summary>
		public Vector3 Center { get; }

		/// <summary>
		/// The volume of the shape.
		/// </summary>
		public float Volume { get; }


		/// <summary>
		/// Tests if the shape contains the given point.
		/// </summary>
		/// <param name="point">Point to test</param>
		/// <returns>If the point is contained inside this shape</returns>
		public bool Contains(Vector3 point);

		/// <summary>
		/// Tests if this shape intersects the given ray. This follows the same behavior
		/// as described in <see cref="Intersects(Ray, out Vector3)"/>, but skips the
		/// hit position computation.
		/// </summary>
		/// <param name="ray">Ray to test</param>
		/// <returns>If the ray intersects the shape</returns>
		public bool Intersects(Ray ray);

		/// <summary>
		/// Computes the intersection between this shape and a ray, returning the hit position
		/// if there is intersection. If the ray originates inside this shape, a hit is registered
		/// as the ray exits the shape.
		/// </summary>
		/// <param name="ray">Ray to test</param>
		/// <param name="hit">Hit position</param>
		/// <returns>If the ray intersects the shape</returns>
		public bool Intersects(Ray ray, out Vector3 hit);

	}

	/// <summary>
	/// An Axis-Aligned Bounding-Box (AABB), represented by two points in space.
	/// </summary>
	public struct AABB : IShape3D {

		/// <summary>
		/// First point of the bounding box.
		/// </summary>
		public Vector3 P1;

		/// <summary>
		/// Second point of the bounding box.
		/// </summary>
		public Vector3 P2;

		/// <summary>
		/// The size of the bounding box.
		/// </summary>
		public Vector3 Size => (P1 - P2).Abs();


		public Vector3 Center => ((P2 - P1) * 0.5f) + P1;

		public float Volume {
			get {
				Vector3 size = Size;
				return size.X * size.Y * size.Z;
			}
		}

		/// <summary>
		/// The minimum (closest to origin) point of the bounding box.
		/// </summary>
		public Vector3 PMin => P1.Min(P2);

		/// <summary>
		/// The maximum (furthest from origin) point of the bounding box.
		/// </summary>
		public Vector3 PMax => P1.Max(P2);


		public AABB(Vector3 p1, Vector3 p2) {
			P1 = p1;
			P2 = p2;
		}


		public bool Contains(Vector3 point) {
			Vector3 min = PMin, max = PMax;
			return point.X >= min.X && point.X <= max.X &&
				point.Y >= min.Y && point.Y <= max.Y &&
				point.Z >= min.Z && point.Z <= max.Z;
		}

		// Computes the time of intersection of a ray through this bounding box
		// This does consider intersection for rays which originate inside this bounding box
		private float TimeOfIntersection(Ray ray) {
			Vector3 dirinv = Vector3.One / ray.Direction;

			Vector3 t1 = (PMin - ray.Origin) * dirinv;
			Vector3 t2 = (PMax - ray.Origin) * dirinv;

			float tmin = ExMath.Min(t1.X, t2.X);
			float tmax = ExMath.Max(t1.X, t2.X);

			tmin = ExMath.Max(tmin, MathF.Min(t1.Y, t2.Y));
			tmax = ExMath.Min(tmax, MathF.Max(t1.Y, t2.Y));

			tmin = ExMath.Max(tmin, MathF.Min(t1.Z, t2.Z));
			tmax = ExMath.Min(tmax, MathF.Max(t1.Z, t2.Z));

			tmin = MathF.Max(tmin, 0.0f);
			return tmax > tmin ? tmin : -1.0f;
		}

		public bool Intersects(Ray ray) => TimeOfIntersection(ray) >= 0;

		public bool Intersects(Ray ray, out Vector3 hit) {
			hit = default;
			float toi = TimeOfIntersection(ray);
			if (toi >= 0) {
				hit = ray.Origin + ray.Direction * toi;
				return true;
			} else return false;
		}


		/// <summary>
		/// Tests if this and another bounding box overlap.
		/// </summary>
		/// <param name="bb2">Bounding box to test</param>
		/// <returns>If the bounding boxes overlap</returns>
		public bool Overlaps(AABB bb2) {
			Vector3 pmax1 = PMax, pmin1 = PMin, pmax2 = bb2.PMax, pmin2 = bb2.PMin;
			return !(
				pmin2.X > pmax1.X ||
				pmax2.X < pmin1.X ||
				pmin2.Y > pmax1.Y ||
				pmax2.Y < pmax1.Y ||
				pmin2.Z > pmax1.Z ||
				pmax2.Z < pmax1.Z
			);
		}

		/// <summary>
		/// tests if this and another bounding box overlap, computing the bounding box of the overlap.
		/// </summary>
		/// <param name="bb2">Bounding box to test</param>
		/// <param name="overlap">The overlapping bounding box</param>
		/// <returns>If the bounding boxes overlap</returns>
		public bool Overlaps(AABB bb2, out AABB overlap) {
			Vector3 pmax1 = PMax, pmin1 = PMin, pmax2 = bb2.PMax, pmin2 = bb2.PMin;
			if (!(
				pmin2.X > pmax1.X ||
				pmax2.X < pmin1.X ||
				pmin2.Y > pmax1.Y ||
				pmax2.Y < pmax1.Y ||
				pmin2.Z > pmax1.Z ||
				pmax2.Z < pmax1.Z
			)) {
				overlap = new AABB() {
					P1 = pmin1.Max(pmin2),
					P2 = pmax1.Min(pmax2)
				};
				return true;
			} else {
				overlap = default;
				return false;
			}
		}

		/// <summary>
		/// Computes the minimum bounding box which will contain both this bounding
		/// box and the supplied bounding box.
		/// </summary>
		/// <param name="bb2">Second bounding box</param>
		/// <returns>Union of bounding boxes</returns>
		public AABB Union(AABB bb2) => new(PMin.Min(bb2.PMin), PMax.Max(bb2.PMax));

	}

	/// <summary>
	/// A sphere, defined by its center position and radius.
	/// </summary>
	public struct Sphere : IShape3D {

		/// <summary>
		/// The center point of the sphere.
		/// </summary>
		public Vector3 Center;

		/// <summary>
		/// The radius of the sphere.
		/// </summary>
		public float Radius;

		public Sphere(Vector3 center, float radius) {
			Center = center;
			Radius = radius;
		}


		private const float FourThirdsPi = (4.0f / 3.0f) * MathF.PI;

		public float Volume => FourThirdsPi * Radius * Radius * Radius;

		Vector3 IShape3D.Center => Center;


		public bool Contains(Vector3 point) => (point - Center).LengthSquared() <= (Radius * Radius); // Faster to square radius than take root of length
		
		private float TimeOfIntersection(Ray ray) {
			Vector3 oc = ray.Origin - Center;
			float a = ray.Direction.Dot(ray.Direction);
			float b = 2.0f * oc.Dot(ray.Direction);
			float c = oc.Dot(oc) - Radius * Radius;
			float discriminant = b * b - 4 * a * c;
			if (discriminant < 0) return -1.0f;
			else return (-b - MathF.Sqrt(discriminant)) / (2.0f * a);
		}

		public bool Intersects(Ray ray) => TimeOfIntersection(ray) >= 0;
		
		public bool Intersects(Ray ray, out Vector3 hit) {
			hit = default;
			float toi = TimeOfIntersection(ray);
			if (toi >= 0) {
				hit = ray.Origin + ray.Direction * toi;
				return true;
			} else return false;
		}


		/// <summary>
		/// Tests if this and another sphere overlap.
		/// </summary>
		/// <param name="sphere">Sphere to test</param>
		/// <returns>If the two spheres overlap</returns>
		public bool Overlaps(Sphere sphere) => (Center - sphere.Center).Length() < (Radius + sphere.Radius);

	}

}
