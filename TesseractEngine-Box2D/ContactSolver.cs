using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Numerics;

namespace Tesseract.Box2D.NET {

	internal struct ContactPositionConstraint {

		public readonly Vector2[] LocalPoints = new Vector2[Box2D.MaxManifoldPoints];
		public Vector2 LocalNormal = default;
		public Vector2 LocalPoint = default;
		public int IndexA = default;
		public int IndexB = default;
		public float InvMassA = default;
		public float InvMassB = default;
		public Vector2 LocalCenterA = default, LocalCenterB = default;
		public float InvIA = default, InvIB = default;
		public ManifoldType Type = default;
		public float RadiusA = default, RadiusB = default;
		public int PointCount = default;

		public ContactPositionConstraint() { }

	}

	internal struct VelocityConstraintPoint {

		public Vector2 RA;
		public Vector2 RB;

		public float NormalImpulse;
		public float TangentImpulse;
		public float NormalMass;
		public float TangentMass;
		public float VelocityBias;

	}

	internal struct ContactVelocityConstraint {

		public readonly VelocityConstraintPoint[] Points = new VelocityConstraintPoint[Box2D.MaxManifoldPoints];

		public Vector2 Normal = default;
		public Matrix2x2 NormalMass = default;
		public Matrix2x2 K = default;
		public int IndexA = default;
		public int IndexB = default;
		public float InvMassA = default, InvMassB = default;
		public float InvIA = default, InvIB = default;
		public float Friction = default;
		public float Restitution = default;
		public float Threshold = default;
		public float TangentSpeed = default;
		public int PointCount = 0;
		public int ContactIndex = default;

		public ContactVelocityConstraint() { }

	}

	internal struct ContactSolverDef {

		public TimeStep Step = default;
		public Contact[] Contacts = Array.Empty<Contact>();
		public int Count = 0;
		public Position[] Positions = Array.Empty<Position>();
		public Velocity[] Velocities = Array.Empty<Velocity>();

		public ContactSolverDef() { }

	}

	internal class ContactSolver {

		private static bool blockSolve = true;

		private TimeStep step;
		private Position[] positions;
		private Velocity[] velocities;
		private ContactPositionConstraint[] positionConstraints;
		private ContactVelocityConstraint[] velocityConstraints;
		private Contact[] contacts;
		private int count;

		public ContactSolver(ContactSolverDef def) {
			step = def.Step;
			count = def.Count;
			positionConstraints = new ContactPositionConstraint[count];
			velocityConstraints = new ContactVelocityConstraint[count];
			positions = def.Positions;
			velocities = def.Velocities;
			contacts = def.Contacts;

			for(int i = 0; i < count; i++) {
				Contact contact = contacts[i];
				Fixture fixtureA = contact.FixtureA;
				Fixture fixtureB = contact.FixtureB;
				IShape shapeA = fixtureA.Shape;
				IShape shapeB = fixtureB.Shape;
				float radiusA = shapeA.Radius;
				float radiusB = shapeB.Radius;
				Body bodyA = fixtureA.Body;
				Body bodyB = fixtureB.Body;
				Manifold manifold = contact.Manifold;

				int pointCount = manifold.PointCount;
				Debug.Assert(pointCount > 0);

				ref ContactVelocityConstraint vc = ref velocityConstraints[i];
				vc.Friction = contact.Friction;
				vc.Restitution = contact.Restitution;
				vc.Threshold = contact.RestitutionThreshold;
				vc.TangentSpeed = contact.TangentSpeed;
				vc.IndexA = bodyA.IslandIndex;
				vc.IndexB = bodyB.IslandIndex;
				vc.InvMassA = bodyA.InvMass;
				vc.InvMassB = bodyB.InvMass;
				vc.InvIA = bodyA.InvI;
				vc.InvIB = bodyB.InvI;
				vc.ContactIndex = i;
				vc.PointCount = pointCount;
				vc.K = Matrix2x2.Zero;
				vc.NormalMass = Matrix2x2.Zero;

				ref ContactPositionConstraint pc = ref positionConstraints[i];
				pc.IndexA = bodyA.IslandIndex;
				pc.IndexB = bodyB.IslandIndex;
				pc.InvMassA = bodyA.InvMass;
				pc.InvMassB = bodyB.InvMass;
				pc.LocalCenterA = bodyA.Sweep.LocalCenter;
				pc.LocalCenterB = bodyB.Sweep.LocalCenter;
				pc.InvIA = bodyA.InvI;
				pc.InvIB = bodyB.InvI;
				pc.LocalNormal = manifold.LocalNormal;
				pc.LocalPoint = manifold.LocalPoint;
				pc.PointCount = pointCount;
				pc.RadiusA = radiusA;
				pc.RadiusB = radiusB;
				pc.Type = manifold.Type;

				for(int j = 0; j < pointCount; j++) {
					ref ManifoldPoint cp = ref manifold.Points[j];
					ref VelocityConstraintPoint vcp = ref vc.Points[j];

					if (step.WarmStarting) {
						vcp.NormalImpulse = step.DTRatio * cp.NormalImpulse;
						vcp.TangentImpulse = step.DTRatio * cp.TangentImpulse;
					} else {
						vcp.NormalImpulse = 0;
						vcp.TangentImpulse = 0;
					}

					vcp.RA = Vector2.Zero;
					vcp.RB = Vector2.Zero;
					vcp.NormalMass = 0;
					vcp.TangentMass = 0;
					vcp.VelocityBias = 0;

					pc.LocalPoints[j] = pc.LocalPoint;
				}
			}
		}

		internal void InitializeVelocityConstraints() {
			for(int i = 0; i < count; i++) {
				ref ContactVelocityConstraint vc = ref velocityConstraints[i];
				ref ContactPositionConstraint pc = ref positionConstraints[i];

				float radiusA = pc.RadiusA;
				float radiusB = pc.RadiusB;
				Manifold manifold = contacts[i].Manifold;

				int indexA = vc.IndexA;
				int indexB = vc.IndexB;

				float mA = vc.InvMassA;
				float mB = vc.InvMassA;
				float iA = vc.InvIA;
				float iB = vc.InvIA;
				Vector2 localCenterA = pc.LocalCenterA;
				Vector2 localCenterB = pc.LocalCenterB;

				Vector2 cA = positions[indexA].C;
				float aA = positions[indexA].A;
				Vector2 vA = velocities[indexA].V;
				float wA = velocities[indexA].W;

				Vector2 cB = positions[indexB].C;
				float aB = positions[indexB].A;
				Vector2 vB = velocities[indexB].V;
				float wB = velocities[indexB].W;

				Debug.Assert(manifold.PointCount > 0);

				Transform xfA = new() {
					Rotation = aA
				}, xfB = new() {
					Rotation = aB
				};
				xfA.Position = cA - (xfA.Rotation * localCenterA);
				xfB.Position = cB - (xfB.Rotation * localCenterB);

				WorldManifold worldManifold = new();
				worldManifold.Initialize(manifold, xfA, radiusA, xfB, radiusB);

				vc.Normal = worldManifold.Normal;

				int pointCount = vc.PointCount;
				for(int j = 0; j < pointCount; j++) {
					ref VelocityConstraintPoint vcp = ref vc.Points[j];

					vcp.RA = worldManifold.Points[j] - cA;
					vcp.RB = worldManifold.Points[j] - cB;

					float rnA = vcp.RA.Cross(vc.Normal);
					float rnB = vcp.RB.Cross(vc.Normal);

					float kNormal = mA + mB + iA * rnA * rnA + iB * rnB * rnB;

					vcp.NormalMass = kNormal > 0 ? 1 / kNormal : 0;

					Vector2 tangent = vc.Normal.Cross(1);

					float rtA = vcp.RA.Cross(tangent);
					float rtB = vcp.RB.Cross(tangent);

					float kTangent = mA + mB + iA * rtA * rtA + iB * rtB * rtB;

					vcp.TangentMass = kTangent > 0 ? 1 / kTangent : 0;

					vcp.VelocityBias = 0;
					float vRel = vc.Normal.Dot(vB + vcp.RB.CrossT(wB) - vA - vcp.RA.CrossT(wA));
					if (vRel < -vc.Threshold) {
						vcp.VelocityBias = -vc.Restitution * vRel;
					}
				}

				if (vc.PointCount == 2 && blockSolve) {
					VelocityConstraintPoint vcp1 = vc.Points[0];
					VelocityConstraintPoint vcp2 = vc.Points[1];

					float rn1A = vcp1.RA.Cross(vc.Normal);
					float rn1B = vcp1.RB.Cross(vc.Normal);
					float rn2A = vcp2.RA.Cross(vc.Normal);
					float rn2B = vcp2.RB.Cross(vc.Normal);

					float k11 = mA + mB + iA * rn1A * rn1A + iB * rn1B * rn1B;
					float k22 = mA + mB + iA * rn2A * rn2A + iB * rn2B * rn2B;
					float k12 = mA + mB + iA * rn1A * rn1A + iB * rn2B * rn2B;

					const float MaxConditionNumber = 1000;
					if (k11 * k11 < MaxConditionNumber * (k11 * k22 - k12 * k12)) {
						vc.K = new Matrix2x2(k11, k12, k12, k22);
						vc.NormalMass = vc.K.Inverse;
					} else {
						vc.PointCount = 1;
					}
				}
			}
		}

		internal void WarmStart() {
			for(int i = 0; i < count; i++) {
				ContactVelocityConstraint vc = velocityConstraints[i];

				int indexA = vc.IndexA;
				int indexB = vc.IndexB;
				float mA = vc.InvMassA;
				float iA = vc.InvIA;
				float mB = vc.InvMassB;
				float iB = vc.InvIB;
				int pointCount = vc.PointCount;

				Vector2 vA = velocities[indexA].V;
				float wA = velocities[indexA].W;
				Vector2 vB = velocities[indexB].V;
				float wB = velocities[indexB].W;

				Vector2 normal = vc.Normal;
				Vector2 tangent = normal.Cross(1);

				for(int j = 0; j < pointCount; j++) {
					VelocityConstraintPoint vcp = vc.Points[j];
				}
			}
		}

		internal void SolveVelocityConstraints() {

		}

		internal void StoreImpulses() {

		}

		internal bool SolvePositionConstraints() {

		}

		internal bool SolveTOIPositionConstraints(int toiIndexA, int toiIndexB) {

		}

	}
}
