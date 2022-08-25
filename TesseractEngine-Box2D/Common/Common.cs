using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Box2D.NET {
	
	public static partial class Box2D {

		public const float MaxFloat = float.MaxValue;

		public const float Epsilon = 1.19209e-07f;

		public const float EpsilonSquared = Epsilon * Epsilon;

		// Collision

		public const int MaxManifoldPoints = 2;

		public const float AABBExtension = 0.1f * LengthUnitsPerMeter;

		public const float AABBMultiplier = 4.0f;

		public const float LinearSlop = 0.005f * LengthUnitsPerMeter;

		public const float AngularSlop = 2.0f / 180.0f * MathF.PI;

		public const float PolygonRadius = 2.0f * LinearSlop;

		public const int MaxSubSteps = 8;

		// Dynamics

		public const int MaxTOIContacts = 32;

		public const float MaxLinearCorrection = 0.2f * LengthUnitsPerMeter;

		public const float MaxAngularCorrection = 8.0f / 180.0f * MathF.PI;

		public const float MaxTranslation = 2 * LengthUnitsPerMeter;

		public const float MaxTranslationSquared = MaxTranslation * MaxTranslation;

		public const float MaxRotation = 0.5f * MathF.PI;

		public const float MaxRotationSquared = MaxRotation * MaxRotation;

		public const float Baumgarte = 0.2f;

		public const float TOIBaumgarte = 0.75f;

		// Sleep

		public const float TimeToSleep = 0.5f;

		public const float LinearSleepTolerance = 0.01f * LengthUnitsPerMeter;

		public const float AngularSleepTolerance = 2.0f / 180.0f * MathF.PI;

	}

}
