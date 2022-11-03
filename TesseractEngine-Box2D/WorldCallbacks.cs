using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {
	
	public interface IDestructionListener {

		public void SayGoodbye(Joint joint);

		public void SayGoodbye(Fixture fixture);

	}

	public delegate bool ContactFilter(Fixture fixtureA, Fixture fixtureB);

	public struct ContactImpulse {

		private readonly float[] normalImpulses = new float[Box2D.MaxManifoldPoints];
		private readonly float[] tangentImpulses = new float[Box2D.MaxManifoldPoints];

		public Span<float> NormalImpulses => normalImpulses.AsSpan()[..Count];

		public Span<float> TangentImpulses => tangentImpulses.AsSpan()[..Count];

		public int Count;

		public ContactImpulse() {
			Count = 0;
		}

	}

	public interface IContactListener {

		public void BeginContact(Contact contact);

		public void EndContact(Contact contact);

		public void PreSolve(Contact contact, Manifold manifold);

		public void PostSolve(Contact contact, ContactImpulse impulse);

	}

	public delegate bool QueryCallback(Fixture fixture);

	public delegate float RaycastCallback(Fixture fixture, Vector2 point, Vector2 normal, float fraction);

}
