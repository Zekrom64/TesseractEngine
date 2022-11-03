using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {
	
	public enum JointType {
		Unknown,
		Revolute,
		Prismatic,
		Distance,
		Pulley,
		Mouse,
		Gear,
		Wheel,
		Weld,
		Friction,
		Motor
	}

	public struct Jacobian {
		
		public Vector2 Linear;
		public float AngularA;
		public float AngularB;

	}

	public class JointEdge {

		public Body Other =  null!;
		public Joint Joint = null!;
		public JointEdge? Prev = null;
		public JointEdge? Next = null;

	}

	public record class JointDef {

		public JointType Type { get; init; }

		public Body BodyA { get; init; } = null!;

		public Body BodyB { get; init; } = null!;

		public bool CollideConnected { get; init; }

	}

	public static partial class Box2D {

		public static (float Stiffness, float Damping) LinearStiffness(float frequencyHertz, float dampingRatio, Body bodyA, Body bodyB) {
			float massA = bodyA.Mass;
			float massB = bodyB.Mass;
			float mass;

			if (massA > 0 && massB > 0) {
				mass = massA * massB / (massA + massB);
			} else if (massA > 0) {
				mass = massA;
			} else {
				mass = massB;
			}

			float omega = 2 * MathF.PI * frequencyHertz;
			return (
				mass * omega * omega,
				2 * mass * dampingRatio * omega
			);
		}

		public static (float Stiffness, float Damping) AngularStiffness(float frequencyHertz, float dampingRatio, Body bodyA, Body bodyB) {
			float IA = bodyA.Inertia;
			float IB = bodyB.Inertia;
			float I;

			if (IA > 0 && IB > 0) {
				I = IA * IB / (IA + IB);
			} else if (IA > 0) {
				I = IA;
			} else {
				I = IB;
			}

			float omega = 2 * MathF.PI * frequencyHertz;
			return (
				I * omega * omega,
				2 * I * dampingRatio * omega
			);
		}

	}

	public abstract class Joint {

		public JointType Type { get; }

		public Body BodyA { get; }

		public Body BodyB { get; }

		public abstract Vector2 AnchorA { get; }

		public abstract Vector2 AnchorB { get; }

		public abstract Vector2 GetReactionForce(float invDt);

		public abstract float GetReactionTorque(float invDt);

		internal Joint? Prev;

		public Joint? Next { get; }

		public bool IsEnabled => BodyA.IsEnabled && BodyB.IsEnabled;

		public bool IsCollideConnected { get; }

		internal int Index;

		internal bool IslandFlag;

		internal JointEdge EdgeA, EdgeB;

		internal static Joint Create(JointDef def) {
			switch (def.Type) {
				case JointType.Revolute:
					break;
				case JointType.Prismatic:
					break;
				case JointType.Distance:
					break;
				case JointType.Pulley:
					break;
				case JointType.Mouse:
					break;
				case JointType.Gear:
					break;
				case JointType.Wheel:
					break;
				case JointType.Weld:
					break;
				case JointType.Friction:
					break;
				case JointType.Motor:
					break;
				default:
					throw new ArgumentException("Undefined joint type", nameof(def));
			}
		}

		protected Joint(JointDef def) {
			Type = def.Type;
			Prev = Next = null;
			BodyA = def.BodyA;
			BodyB = def.BodyB;
			Index = 0;
			IsCollideConnected = def.CollideConnected;
			IslandFlag = false;

			EdgeA = new();
			EdgeB = new();
		}

		public abstract void InitVelocityConstraints(SolverData data);

		public abstract void SolveVelocityConstraints(SolverData data);

		public abstract void SolvePositionConstraints(SolverData data);

	}
}
