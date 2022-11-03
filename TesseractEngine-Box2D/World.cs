using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {

	public class World {

		public IDestructionListener? DestructionListener { get; set; }

		public ContactFilter? ContactFilter { get; set; }

		public IContactListener? ContactListener { get; set; }

		public Body CreateBody(BodyDef def) {

		}

		public Joint CreateJoint(JointDef def) {

		}

		public void Step(float timeStep, int velocityIterations, int positionIterations) {

		}

		public void ClearForces() {

		}

		public void QueryAABB(QueryCallback callback, AABB aabb) {

		}

		public void Raycast(RaycastCallback callback, Vector2 point1, Vector2 point2) {

		}

		public Body? BodyList { get; private set; }



	}

}
