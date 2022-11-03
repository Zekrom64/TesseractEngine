using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {
	
	public partial class Box2D {

		public static float MixFriction(float f1, float f2) => MathF.Sqrt(f1 * f2);

		public static float MixRestitution(float r1, float r2) => Math.Max(r1, r2);

		public static float MixRestitutionThreshold(float r1, float r2) => Math.Min(r1, r2);

	}

	public delegate Contact ContactCreateFcn(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB);

	public class ContactEdge {

		public Body? Other;

		public Contact? Proxy;

		public ContactEdge? Prev;

		public ContactEdge? Next;

	}

	public abstract class Contact {

		protected Manifold manifold;
		public Manifold Manifold => manifold;

		public WorldManifold WorldManifold {
			get {
				Body bodyA = FixtureA.Body;
				Body bodyB = FixtureB.Body;
				IShape shapeA = FixtureA.Shape;
				IShape shapeB = FixtureB.Shape;
				WorldManifold wm = new();
				wm.Initialize(Manifold, bodyA.Transform, shapeA.Radius, bodyB.Transform, shapeB.Radius);
				return wm;
			}
		}

		public bool IsTouching {
			get => (Flags & ContactFlags.Touching) != 0;
			set {
				if (value) Flags |= ContactFlags.Touching;
				else Flags &= ~ContactFlags.Touching;
			}
		}
		

		public bool IsEnabled {
			get => (Flags & ContactFlags.Enabled) != 0;
			set {
				if (value) Flags |= ContactFlags.Enabled;
				else Flags &= ~ContactFlags.Enabled;
			}
		}

		protected Contact? Prev;

		public Contact? Next { get; }

		public Fixture FixtureA { get; }

		public int ChildIndexA { get; }

		public Fixture FixtureB { get; }

		public int ChildIndexB { get; }

		public float Friction { get; set; }

		public void ResetFriction() {
			Friction = Box2D.MixFriction(FixtureA.Friction, FixtureB.Friction);
		}

		public float Restitution { get; set; }

		public void ResetRestitution() {
			Restitution = Box2D.MixRestitution(FixtureA.Restitution, FixtureB.Restitution);
		}

		public float RestitutionThreshold { get; set; }

		public void ResetRestitutionThreshold() {
			RestitutionThreshold = Box2D.MixRestitutionThreshold(FixtureA.RestitutionThreshold, FixtureB.RestitutionThreshold);
		}

		public float TangentSpeed { get; set; }

		public abstract void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB);


		protected Contact(Fixture fixtureA, int childIndexA, Fixture fixtureB, int childIndexB) {
			FixtureA = fixtureA;
			ChildIndexA = childIndexA;
			FixtureB = fixtureB;
			ChildIndexB = childIndexB;

			ResetFriction();
			ResetRestitution();
			ResetRestitutionThreshold();
		}


		[Flags]
		protected enum ContactFlags {
			Island = 0x0001,
			Touching = 0x0002,
			Enabled = 0x0004,
			Filter = 0x0008,
			BulletHit = 0x0010,
			TOI = 0x0020
		}

		protected ContactFlags Flags = ContactFlags.Enabled;

		protected ContactEdge NodeA = new();
		protected ContactEdge NodeB = new();

		protected int TOICount = 0;
		protected float TOI;

		internal void Update(IContactListener listener) {
			Manifold oldManifold = Manifold;

			Flags |= ContactFlags.Enabled;

			bool touching;
			bool wasTouching = IsTouching;

			bool sensorA = FixtureA.IsSensor;
			bool sensorB = FixtureB.IsSensor;
			bool sensor = sensorA || sensorB;

			Body bodyA = FixtureA.Body;
			Body bodyB = FixtureB.Body;
			Transform xfA = bodyA.Transform;
			Transform xfB = bodyB.Transform;

			if (sensor) {
				IShape shapeA = FixtureA.Shape;
				IShape shapeB = FixtureB.Shape;
				touching = Box2D.TestOverlap(shapeA, ChildIndexA, shapeB, ChildIndexB, xfA, xfB);
				manifold = default;
			} else {
				Evaluate(ref manifold, xfA, xfB);
				touching = manifold.PointCount > 0;

				for(int i = 0; i < manifold.PointCount; i++) {
					ManifoldPoint mp2 = manifold.Points[i];
					mp2.NormalImpulse = 0;
					mp2.TangentImpulse = 0;
					ContactID id2 = mp2.ID;

					for(int j = 0; j < oldManifold.PointCount; j++) {
						ManifoldPoint mp1 = oldManifold.Points[j];

						if (mp1.ID.Key == id2.Key) {
							mp2.NormalImpulse = mp1.NormalImpulse;
							mp2.TangentImpulse = mp1.TangentImpulse;
						}
					}
				}

				if (touching ^ wasTouching) {
					bodyA.IsAwake = true;
					bodyB.IsAwake = true;
				}
			}

			IsTouching = touching;
			if (listener != null) {
				if (!wasTouching && touching) listener.BeginContact(this);
				if (wasTouching && !touching) listener.EndContact(this);
				if (!sensor && touching) listener.PreSolve(this, oldManifold);
			}
		}

		protected struct ContactRegister {

			public ContactCreateFcn? CreateFcn;
			public bool Primary;

		}

		protected static readonly ContactRegister[,] registers = new ContactRegister[(int)ShapeType.Count, (int)ShapeType.Count];

		private static void AddType(ContactCreateFcn createFcn, ShapeType type1, ShapeType type2) {
			registers[(int)type1, (int)type2] = new() {
				CreateFcn = createFcn,
				Primary = true
			};
			if (type1 != type2) {
				registers[(int)type2, (int)type1] = new() {
					CreateFcn = createFcn,
					Primary = false
				};
			}
		}

		static Contact() {
			AddType((fixA, indexA, fixB, indexB) => new CircleContact(fixA, indexA, fixB, indexB), ShapeType.Circle, ShapeType.Circle);
			AddType((fixA, indexA, fixB, indexB) => new PolygonAndCircleContact(fixA, indexA, fixB, indexB), ShapeType.Polygon, ShapeType.Circle);
			AddType((fixA, indexA, fixB, indexB) => new PolygonContact(fixA, indexA, fixB, indexB), ShapeType.Polygon, ShapeType.Polygon);
			AddType((fixA, indexA, fixB, indexB) => new EdgeAndCircleContact(fixA, indexA, fixB, indexB), ShapeType.Edge, ShapeType.Circle);
			AddType((fixA, indexA, fixB, indexB) => new EdgeAndPolygonContact(fixA, indexA, fixB, indexB), ShapeType.Edge, ShapeType.Polygon);
			AddType((fixA, indexA, fixB, indexB) => new ChainAndCircleContact(fixA, indexA, fixB, indexB), ShapeType.Chain, ShapeType.Circle);
			AddType((fixA, indexA, fixB, indexB) => new ChainAndPolygonContact(fixA, indexA, fixB, indexB), ShapeType.Chain, ShapeType.Polygon);
		}

		public static Contact? Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB) {
			ContactRegister reg = registers[(int)fixtureA.Type, (int)fixtureB.Type];
			if (reg.CreateFcn != null) {
				if (reg.Primary) return reg.CreateFcn(fixtureA, indexA, fixtureB, indexB);
				else return reg.CreateFcn(fixtureB, indexB, fixtureA, indexA);
			}
			return null;
		}

	}

	internal class ChainAndCircleContact : Contact {

		public ChainAndCircleContact(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB) : base(fixtureA, indexA, fixtureB, indexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Chain);
			Debug.Assert(fixtureB.Type == ShapeType.Circle);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			ChainShape chain = (ChainShape)FixtureA.Shape;
			EdgeShape edge = new();
			chain.GetChildEdge(edge, ChildIndexA);
			manifold = Box2D.CollideEdgeAndCircle(edge, xfA, (CircleShape)FixtureB.Shape, xfB);
		}

	}

	internal class ChainAndPolygonContact : Contact {

		public ChainAndPolygonContact(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB) : base(fixtureA, indexA, fixtureB, indexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Chain);
			Debug.Assert(fixtureB.Type == ShapeType.Polygon);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			ChainShape chain = (ChainShape)FixtureA.Shape;
			EdgeShape edge = new();
			chain.GetChildEdge(edge, ChildIndexA);
			manifold = Box2D.CollideEdgeAndPolygon(edge, xfA, (PolygonShape)FixtureB.Shape, xfB);
		}

	}

	internal class CircleContact : Contact {

		public CircleContact(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB) : base(fixtureA, indexA, fixtureB, indexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Circle);
			Debug.Assert(fixtureB.Type == ShapeType.Circle);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			manifold = Box2D.CollideCircles((CircleShape)FixtureA.Shape, xfA, (CircleShape)FixtureB.Shape, xfB);
		}

	}

	internal class EdgeAndCircleContact : Contact {
		
		public EdgeAndCircleContact(Fixture fixtureA, int childIndexA, Fixture fixtureB, int childIndexB) : base(fixtureA, childIndexA, fixtureB, childIndexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Edge);
			Debug.Assert(fixtureB.Type == ShapeType.Circle);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			manifold = Box2D.CollideEdgeAndCircle((EdgeShape)FixtureA.Shape, xfA, (CircleShape)FixtureB.Shape, xfB);
		}

	}

	internal class EdgeAndPolygonContact : Contact {

		public EdgeAndPolygonContact(Fixture fixtureA, int childIndexA, Fixture fixtureB, int childIndexB) : base(fixtureA, childIndexA, fixtureB, childIndexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Edge);
			Debug.Assert(fixtureB.Type == ShapeType.Polygon);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			manifold = Box2D.CollideEdgeAndPolygon((EdgeShape)FixtureA.Shape, xfA, (PolygonShape)FixtureB.Shape, xfB);
		}

	}

	internal class PolygonAndCircleContact : Contact {

		public PolygonAndCircleContact(Fixture fixtureA, int childIndexA, Fixture fixtureB, int childIndexB) : base(fixtureA, childIndexA, fixtureB, childIndexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Polygon);
			Debug.Assert(fixtureB.Type == ShapeType.Circle);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			manifold = Box2D.CollidePolygonAndCircle((PolygonShape)FixtureA.Shape, xfA, (CircleShape)FixtureB.Shape, xfB);
		}

	}

	internal class PolygonContact : Contact {

		public PolygonContact(Fixture fixtureA, int childIndexA, Fixture fixtureB, int childIndexB) : base(fixtureA, childIndexA, fixtureB, childIndexB) {
			Debug.Assert(fixtureA.Type == ShapeType.Polygon);
			Debug.Assert(fixtureB.Type == ShapeType.Polygon);
		}

		public override void Evaluate(ref Manifold manifold, Transform xfA, Transform xfB) {
			manifold = Box2D.CollidePolygons((PolygonShape)FixtureA.Shape, xfA, (PolygonShape)FixtureB.Shape, xfB);
		}

	}

}
