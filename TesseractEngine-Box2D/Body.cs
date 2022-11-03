using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {

	public enum BodyType {
		Static,
		Kinematic,
		Dynamic
	}

	public record class BodyDef {

		public BodyType Type { get; init; }

		public Vector2 Position { get; init; }

		public float Angle { get; init; }

		public Vector2 LinearVelocity { get; init; }

		public float AngularVelocity { get; init; }

		public float LinearDampening { get; init; }

		public float AngularDampening { get; init; }

		public bool AllowSleep { get; init; }

		public bool Awake { get; init; }

		public bool FixedRotation { get; init; }

		public bool Bullet { get; init; }

		public bool Enabled { get; init; }

		public float GravityScale { get; init; }

	}

	public class Body {

		public Fixture CreateFixture(FixtureDef def);

		public Fixture CreateFixture(IShape shape, float density);

		public Transform Transform { get; set; }

		public Vector2 Position { get; }

		public float Angle { get; }

		public Vector2 WorldCenter { get; }

		public Vector2 LocalCenter { get; }

		public Vector2 LinearVelocity { get; set; }

		public float AngularVelocity { get; set; }

		public void ApplyForce(Vector2 force, Vector2 point, bool wake) {

		}

		public void ApplyForceToCenter(Vector2 force, bool wake) {

		}

		public void ApplyTorque(float torque, bool wake) {

		}

		public void ApplyLinearImpulse(Vector2 impulse, Vector2 point, bool wake) {

		}

		public void ApplyLinearImpulseToCenter(Vector2 impulse, bool wake) {

		}

		public void ApplyAngularImpulse(float impulse, bool wake) {

		}

		public float Mass { get; }

		public float Inertia { get; }

		public MassData MassData { get; set; }

		public void ResetMassData() {

		}

		public Vector2 GetWorldPoint(Vector2 localPoint) {

		}

		public Vector2 GetWorldVector(Vector2 localVector) {

		}

		public Vector2 GetLocalPoint(Vector2 worldPoint) {

		}

		public Vector2 GetLocalVector(Vector2 worldVector) {

		}

		public Vector2 GetLinearVelocityFromWorldPoint(Vector2 worldPoint) {

		}

		public Vector2 GetLinearVelocityFromLocalPoint(Vector2 localPoint) {

		}

		public float LinearDampening { get; set; }

		public float AngularDampening { get; set; }

		public float GravityScale { get; set; }

		private BodyType type;
		public BodyType Type {
			get => type;
			set {
				if (world)
			}
		}

		public bool IsBullet { get; set; }

		public bool IsSleepingAllowed { get; set; }

		public bool IsAwake { get; set; }

		public bool IsEnabled { get; set; }

		public bool IsFixedRotation { get; set; }

		public Fixture? FixtureList { get; }

		public JointEdge? JointList { get; } = null;

		public ContactEdge? ContactList { get; } = null;

		public Body? Next { get; } = null;

		public World World { get; }

		[Flags]
		internal enum BodyFlags : ushort {
			Island = 0x0001,
			Awake = 0x0002,
			AutoSleep = 0x0004,
			Bullet = 0x0008,
			FixedRotation= 0x0010,
			Enabled = 0x0020,
			TOI = 0x0040
		}

		internal BodyFlags Flags = 0;
		internal Sweep Sweep = default;
		internal Body? Prev = null;

		internal Body(BodyDef def, World world) {
			Debug.Assert(def.Position.IsValid());
			Debug.Assert(def.LinearVelocity.IsValid());
			Debug.Assert(Box2D.IsValid(def.Angle));
			Debug.Assert(Box2D.IsValid(def.AngularVelocity));
			Debug.Assert(Box2D.IsValid(def.AngularDampening) && def.AngularDampening >= 0);
			Debug.Assert(Box2D.IsValid(def.LinearDampening) && def.LinearDampening >= 0);

			if (def.Bullet) Flags |= BodyFlags.Bullet;
			if (def.FixedRotation) Flags |= BodyFlags.FixedRotation;
			if (def.AllowSleep) Flags |= BodyFlags.AutoSleep;
			if (def.Awake && def.Type != BodyType.Static) Flags |= BodyFlags.Awake;
			if (def.Enabled) Flags |= BodyFlags.Enabled;

			World = world;

			Transform = new Transform(def.Position, def.Angle);
			Sweep = new() { 
				LocalCenter = Vector2.Zero,
				StartCenter = Transform.Position,
				EndCenter = Transform.Position,
				StartAngle = Transform.Rotation.Angle,
				EndAngle = Transform.Rotation.Angle,
				StartAlpha = 0
			};
			
		}

		internal void SynchronizeFixtures() {

		}

		internal void SynchronizeTransform() {

		}

		internal bool ShouldCollide(Body other) {

		}

		internal void Advance(float t) {

		}

		internal int IslandIndex;

		internal float InvMass;
		internal float InvI;

	}

}
