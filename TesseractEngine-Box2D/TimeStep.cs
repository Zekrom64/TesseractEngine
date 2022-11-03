using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {
	
	public struct Profile {

		public float Step;
		public float Collide;
		public float Solve;
		public float SolveInit;
		public float SolveVelocity;
		public float SolvePosition;
		public float Broadphase;
		public float SolveTOI;

	}

	internal struct TimeStep {

		public float DT;
		public float InvDT;
		public float DTRatio;
		public int VelocityIterations;
		public int PositionIterations;
		public bool WarmStarting;

	}

	internal struct Position {

		public Vector2 C;
		public float A;

	}

	internal struct Velocity {

		public Vector2 V;
		public float W;

	}

	internal ref struct SolverData {

		public TimeStep Step;
		public Span<Position> Positions;
		public Span<Velocity> Velocities;

	}

}
