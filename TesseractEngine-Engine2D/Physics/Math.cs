using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Engine2D.Physics {
	
	public readonly record struct Rotation {

		public static readonly Rotation Identity = new(0);

		public float Sine { get; } = 0;

		public float Cosine { get; } = 1;

		public float Angle => MathF.Atan2(Sine, Cosine);

		public Vector2 XAxis => new(Cosine, Sine);

		public Vector2 YAxis => new(-Sine, Cosine);

		public Rotation(float angle) {
			Sine = MathF.Sin(angle);
			Cosine = MathF.Cos(angle);
		}

		private Rotation(float s, float c) {
			Sine = s;
			Cosine = c;
		}

		public static implicit operator Rotation(float angle) => new(angle);

		public static Rotation operator *(in Rotation r1, in Rotation r2) => new(
			r1.Sine * r1.Cosine + r1.Cosine * r2.Sine,
			r1.Cosine * r2.Cosine - r1.Sine * r2.Sine
		);

		public Rotation MulT(in Rotation r2) => new(
			Cosine * r2.Sine - Sine * r2.Cosine,
			Cosine * r2.Cosine + Sine * r2.Sine
		);

		public static Vector2 operator *(in Rotation q, Vector2 v) => new(
			q.Cosine * v.X - q.Sine * v.Y,
			q.Sine * v.X + q.Cosine * v.Y
		);

		public Vector2 MulT(Vector2 v) => new(
			Cosine * v.X + Sine * v.Y,
			-Sine * v.X + Cosine * v.Y
		);

	}

	public readonly record struct Transform {

		public readonly Vector2 Position { get; init; } = Vector2.Zero;

		public Rotation Rotation { get; init; } = default;

		public Transform() { }

		public Transform(Vector2 position, float angle) {
			Position = position;
			Rotation = angle;
		}

		public static Transform operator *(in Transform xf1, in Transform xf2) => new() {
			Rotation = xf1.Rotation * xf2.Rotation,
			Position = xf1.Rotation * xf1.Position + xf1.Position
		};

		public Transform MulT(in Transform xf2) => new() {
			Rotation = Rotation.MulT(xf2.Rotation),
			Position = Rotation.MulT(xf2.Position - Position)
		};

		public static Vector2 operator *(in Transform xf, Vector2 v) => xf.Rotation * v + xf.Position;

		public Vector2 MulT(Vector2 v) => Rotation.MulT(v - Position);

	}

	public struct Sweep {

		public Vector2 LocalCenter;

		public Vector2 StartCenter, EndCenter;

		public float StartAngle, EndAngle;

		public float StartAlpha;

		public Transform GetTransform(float beta) => new() {
			Position = (1 - beta) * StartCenter + beta * EndCenter,
			Rotation = (1 - beta) * StartAngle + beta * EndAngle
		};

		public void Advance(float alpha) {
			if (StartAlpha < 1) throw new InvalidOperationException("Sweep start alpha must be <1");
			float beta = (alpha - StartAlpha) / (1 - StartAlpha);
			StartCenter += beta * (EndCenter - StartCenter);
			StartAngle += beta * (EndAngle - StartAngle);
			StartAlpha = alpha;
		}

		public void Normalize() {
			const float twoPi = 2 * MathF.PI;
			float d = twoPi * MathF.Floor(StartAngle / twoPi);
			StartAngle -= d;
			EndAngle -= d;
		}

	}

	public static partial class Box2D {

		public static bool IsValid(float x) => float.IsFinite(x);

		public static bool IsValid(this Vector2 v) => IsValid(v.X) && IsValid(v.Y);

		public static float Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

		public static Vector2 Cross(this Vector2 v, float s) => new(s * v.Y, -s * v.X);

		public static Vector2 CrossT(this Vector2 a, float s) => new(-s * a.Y, s * a.X);

	}

}
