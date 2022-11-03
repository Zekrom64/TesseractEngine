using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {

	public struct Filter {

		public ushort CategoryBits = 0x0001;

		public ushort MaskBits = 0xFFFF;

		public int GroupIndex = 0;

		public Filter() { }

	}

	public record class FixtureDef {

		public IShape Shape { get; init; } = default!;

		public float Friction { get; init; }

		public float Restitution { get; init; }

		public float RestitutionThreshold { get; init; }

		public float Density { get; init; }

		public bool IsSensor { get; init; }

		public Filter Filter { get; init; }

	}

	public struct FixtureProxy {

		public AABB AABB;

		public Fixture? Fixture;

		public int ChildIndex;

		public int ProxyID;

	}

	public class Fixture : IDisposable {

		public ShapeType Type => Shape.Type;

		public IShape Shape { get; }

		private bool isSensor;
		public bool IsSensor {
			get => isSensor;
			set {
				if (value ^ isSensor) {
					Body.IsAwake = true;
					isSensor = value;
				}
			}
		}

		private Filter filter;
		public Filter Filter {
			get => filter;
			set {
				filter = value;
				Refilter();
			}
		}

		public void Refilter() {

		}

		public Body Body { get; }

		public Fixture? Next { get; }

		public bool TestPoint(Vector2 p) => Shape.TestPoint(Body.Transform, p);

		public bool RayCast(out RayCastOutput output, in RayCastInput input, int childIndex) => Shape.RayCast(out output, input, Body.Transform, childIndex);

		public MassData GetMassData() => Shape.ComputeMass(Density);

		private float density;
		public float Density {
			get => density;
			set {
				if (value < 0 || !float.IsFinite(value)) throw new ArgumentException("Invalid density value", nameof(value));
				density = value;
			}
		}

		public float Friction { get; set; }

		public float Restitution { get; set; }

		public float RestitutionThreshold { get; set; }

		public AABB GetAABB(int childIndex) => Proxies[childIndex].AABB;


		internal FixtureProxy[] Proxies = Array.Empty<FixtureProxy>();
		internal int ProxyCount = 0;

		internal Fixture(Body body, FixtureDef def) {
			Friction = def.Friction;
			Restitution = def.Restitution;
			RestitutionThreshold = def.RestitutionThreshold;

			Body = body;
			Next = null;

			Filter = def.Filter;

			IsSensor = def.IsSensor;

			Shape = def.Shape.Clone();

			int childCount = Shape.ChildCount;
			if (childCount > 0) {
				Proxies = new FixtureProxy[childCount];
				for(int i = 0; i < childCount; i++) {
					Proxies[i] = new() {
						Fixture = null,
						ProxyID = BroadPhase.NullProxy
					};
				}
			}

			Density = def.Density;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			// TODO
		}

	}

}
