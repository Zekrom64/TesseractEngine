#include "imspinner.h"
#include "imgui_cli.h"
#include <cmath>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;
using namespace System::Runtime::InteropServices;

constexpr float PI_DIV_4 = IM_PI / 4.f;
constexpr float PI_DIV_2 = IM_PI / 2.f;
constexpr float PI_2 = IM_PI * 2.f;

// Preprocessor hackery to make declaring these easier

inline ImColor IM_GET_COLOR(Vector4 v) { return { v.X, v.Y, v.Z, v.W }; }

#define OPTIONAL(X) [OptionalAttribute] Nullable<X>
#define DEFAULTVAL(X,DEFAULT) (X.HasValue ? X.Value : (DEFAULT))

#define LIST(...) __VA_ARGS__

#define DECLSPINNER(NAME, ARGLIST, CALLLIST) \
	static void NAME(ReadOnlySpan<uint8_t> label, ARGLIST) { \
		IM_SPAN_TO_STR(pLabel, label); \
		::ImSpinner::NAME(pLabel, CALLLIST); \
	} \
	static void NAME(System::String^ label, ARGLIST) { \
		StringParam pLabel(label); \
		::ImSpinner::NAME(pLabel.c_str(), CALLLIST); \
	}

#define DECLVAR(TYPE, NAME, INIT) \
	private: \
		static TYPE _##NAME = INIT; \
	public: \
		static property TYPE NAME { TYPE get() { return _##NAME; } void set(TYPE value) { _##NAME = value; } }

namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

	public delegate Vector4 LeafColorFn(int index);

	/// <summary>
	/// Contains methods for idle spinner widgets.
	/// </summary>
	public ref class ImSpinner abstract sealed {
	public:
		DECLVAR(float, DefaultRadius, 6)
		DECLVAR(float, DefaultThickness, 2)
		DECLVAR(Vector4, DefaultColor, Vector4::One)
		DECLVAR(float, DefaultSpeed, 2.8f)
		DECLVAR(Vector4, DefaultBg, Vector4(1, 1, 1, 0.5f))

		DECLSPINNER(
			SpinnerRainbow,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(float) angMin, OPTIONAL(float) angMax, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angMin, 0), DEFAULTVAL(angMax, IM_PI*2), DEFAULTVAL(arcs, 1))
		)

		DECLSPINNER(
			SpinnerRotatingHeart,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(float) angMin),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angMin, 0))
		)

		DECLSPINNER(
			SpinnerAng,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerLoadingRing,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) segments),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(segments, 8))
		)

		DECLSPINNER(
			SpinnerClock,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, Vector4::One)), IM_GET_COLOR(DEFAULTVAL(bg, Vector4(1, 1, 1, 0.5f))), DEFAULTVAL(speed, 2.8f))
		)

		DECLSPINNER(
			SpinnerPulsar,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(bool) sequence),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(sequence, true))
		)

		DECLSPINNER(
			SpinnerDoubleFadePulsar,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) bg, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerTwinPulsar,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) rings),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(rings, 2))
		)

		DECLSPINNER(
			SpinnerFadePulsar,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) rings),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(rings, 2))
		)

		DECLSPINNER(
			SpinnerCircularLines,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lines),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lines, 8))
		)

		static void SpinnerDots(ReadOnlySpan<uint8_t> label, float% nextDot, OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots, OPTIONAL(float) minth) {
			IM_SPAN_TO_STR(pLabel, label);
			pin_ptr<float> pNextDot = &nextDot;
			::ImSpinner::SpinnerDots(pLabel, pNextDot, DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 12), DEFAULTVAL(minth, -1));
		}

		static void SpinnerDots(String^ label, float% nextDot, OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots, OPTIONAL(float) minth) {
			StringParam pLabel(label);
			pin_ptr<float> pNextDot = &nextDot;
			::ImSpinner::SpinnerDots(pLabel.c_str(), pNextDot, DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 12), DEFAULTVAL(minth, -1));
		}

		DECLSPINNER(
			SpinnerVDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) dots, OPTIONAL(int) mdots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 12), DEFAULTVAL(mdots, 6))
		)

		DECLSPINNER(
			SpinnerBounceDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 3))
		)

		DECLSPINNER(
			SpinnerZipDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 5))
		)

		DECLSPINNER(
			SpinnerDotsToBar,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(float) offsetK, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), DEFAULTVAL(offsetK, 2), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 5))
		)

		DECLSPINNER(
			SpinnerWaveDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lt),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lt, 8))
		)

		DECLSPINNER(
			SpinnerFadeDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lt),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lt, 8))
		)

		DECLSPINNER(
			SpinnerMultiFadeDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lt),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lt, 8))
		)

		DECLSPINNER(
			SpinnerScaleDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lt),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lt, 8))
		)

		DECLSPINNER(
			SpinnerMovingDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 3))
		)

		DECLSPINNER(
			SpinnerRotateDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 2))
		)

		DECLSPINNER(
			SpinnerOrionDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerGalaxyDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerTwinAng,
			LIST(float radius1, float radius2, OPTIONAL(float) thickness, OPTIONAL(Vector4) color1, OPTIONAL(Vector4) color2, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(radius1, radius2, DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color1, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(color2, Vector4(1, 0, 0, 1))), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerFilling,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color1, OPTIONAL(Vector4) color2, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color1, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(color2, Vector4(1, 0, 0, 1))), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerTopup,
			LIST(float radius1, float radius2, OPTIONAL(Vector4) color, OPTIONAL(Vector4) fg, OPTIONAL(Vector4) bg, OPTIONAL(float) speed),
			LIST(radius1, radius2, IM_GET_COLOR(DEFAULTVAL(color, Vector4(1, 0, 0, 1))), IM_GET_COLOR(DEFAULTVAL(fg, Vector4::One)), IM_GET_COLOR(DEFAULTVAL(bg, Vector4::One)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerTwinAng180,
			LIST(float radius1, float radius2, OPTIONAL(float) thickness, OPTIONAL(Vector4) color1, OPTIONAL(Vector4) color2, OPTIONAL(float) speed),
			LIST(radius1, radius2, DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color1, Vector4::One)), IM_GET_COLOR(DEFAULTVAL(color2, Vector4(1, 0, 0, 1))), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerTwinAng360,
			LIST(float radius1, float radius2, OPTIONAL(float) thickness, OPTIONAL(Vector4) color1, OPTIONAL(Vector4) color2, OPTIONAL(float) speed1, OPTIONAL(float) speed2),
			LIST(radius1, radius2, DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color1, Vector4::One)), IM_GET_COLOR(DEFAULTVAL(color2, Vector4(1, 0, 0, 1))), DEFAULTVAL(speed1, DefaultSpeed), DEFAULTVAL(speed2, 2.5f))
		)

		DECLSPINNER(
			SpinnerIncDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 6))
		)

		DECLSPINNER(
			SpinnerIncFullDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 4))
		)

		DECLSPINNER(
			SpinnerFadeBars,
			LIST(float w, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) bars, OPTIONAL(bool) scale),
			LIST(w, IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 3), DEFAULTVAL(scale, false))
		)

		DECLSPINNER(
			SpinnerBarsRotateFade,
			LIST(float rmin, float rmax, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(rmin, rmax, DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 6))
		)

		DECLSPINNER(
			SpinnerBarsScaleMiddle,
			LIST(float w, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(w, IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 3))
		)

		DECLSPINNER(
			SpinnerAngTwin,
			LIST(float radius1, float radius2, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(float) angle, OPTIONAL(int) arcs),
			LIST(radius1, radius2, DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI), DEFAULTVAL(arcs, 1))
		)

		DECLSPINNER(
			SpinnerArcRotation,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerArcFade,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerSquareStrokeFade,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		static void SpinnerAsciiSymbolPoints(ReadOnlySpan<uint8_t> label, ReadOnlySpan<uint8_t> text, OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR(pText, text);
			::ImSpinner::SpinnerAsciiSymbolPoints(pLabel, pText, DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed));
		}

		static void SpinnerAsciiSymbolPoints(String^ label, String^ text, OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed) {
			StringParam pLabel(label), pText(text);
			::ImSpinner::SpinnerAsciiSymbolPoints(pLabel.c_str(), pText.c_str(), DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed));
		}

		static void SpinnerSevenSegments(ReadOnlySpan<uint8_t> label, ReadOnlySpan<uint8_t> text, OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed) {
			IM_SPAN_TO_STR(pLabel, label);
			IM_SPAN_TO_STR(pText, text);
			::ImSpinner::SpinnerSevenSegments(pLabel, pText, DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed));
		}

		static void SpinnerSevenSegments(String^ label, String^ text, OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed) {
			StringParam pLabel(label), pText(text);
			::ImSpinner::SpinnerSevenSegments(pLabel.c_str(), pText.c_str(), DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed));
		}

		DECLSPINNER(
			SpinnerSquareStrokeFill,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerSquareStrokeLoading,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerFilledArcFade,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerFilledArcColor,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerFilledArcRing,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerArcWedges,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerTwinBall,
			LIST(float radius1, float radius2, float thickness, float bThickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(radius1, radius2, thickness, bThickness, IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 2))
		)

		DECLSPINNER(
			SpinnerSolarBalls,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 4))
		)

		DECLSPINNER(
			SpinnerSolarScaleBalls,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 4))
		)

		DECLSPINNER(
			SpinnerSolarArcs,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 4))
		)

		DECLSPINNER(
			SpinnerRainbowCircle,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs, OPTIONAL(int) mode),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4), DEFAULTVAL(mode, 1))
		)

		DECLSPINNER(
			SpinnerBounceBall,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots, OPTIONAL(bool) shadow),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 1), DEFAULTVAL(shadow, false))
		)

		DECLSPINNER(
			SpinnerIncScaleDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 6))
		) 

		DECLSPINNER(
			SpinnerAngTriple,
			LIST(float radius1, float radius2, float radius3, OPTIONAL(float) thickness, OPTIONAL(Vector4) c1, OPTIONAL(Vector4) c2, OPTIONAL(Vector4) c3, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(radius1, radius2, radius3, DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(c1, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(c2, DefaultBg)), IM_GET_COLOR(DEFAULTVAL(c3, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerAngEclipse,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerIngYang,
			LIST(float radius, float thickness, bool reverse, float yangDeltaR, OPTIONAL(Vector4) colorI, OPTIONAL(Vector4) colorY, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(radius, thickness, reverse, yangDeltaR, IM_GET_COLOR(DEFAULTVAL(colorI, Vector4::One)), IM_GET_COLOR(DEFAULTVAL(colorY, Vector4::One)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI * 0.7f))
		)

		DECLSPINNER(
			SpinnerGooeyBalls,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerRotateGooeyBalls,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 3))
		)

		DECLSPINNER(
			SpinnerRotateTriangles,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) tris),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(tris, 3))
		)

		DECLSPINNER(
			SpinnerRotateShapes,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) shapes, OPTIONAL(int) points),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(shapes, 3), DEFAULTVAL(points, 4))
		)

		DECLSPINNER(
			SpinnerSinSquares,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerMoonLine,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerCircleDrop,
			LIST(float radius, float thickness, float thicknessDrop, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(radius, thickness, thicknessDrop, IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerSurroundedIndicator,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerWifiIndicator,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(float) cangle, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(cangle, 0), DEFAULTVAL(dots, 3))
		)
		
		DECLSPINNER(
			SpinnerTrianglesSelector,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 8))
		)

		DECLSPINNER(
			SpinnerCamera,
			LIST(float radius, float thickness, LeafColorFn^ color, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(radius, thickness, (::ImSpinner::LeafColor*)(void*)Marshal::GetFunctionPointerForDelegate(color), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 8))
		)

		DECLSPINNER(
			SpinnerFlowingGradient,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, Vector4(1, 0, 0, 1))), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI))
		)

		DECLSPINNER(
			SpinnerRotateSegments,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs, OPTIONAL(int) layers),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4), DEFAULTVAL(layers, 1))
		)

		DECLSPINNER(
			SpinnerLemniscate,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(float) angle),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(angle, IM_PI / 2.0f))
		)

		DECLSPINNER(
			SpinnerRotateGear,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) pins),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(pins, 12))
		)

		DECLSPINNER(
			SpinnerRotateWheel,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) bg, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) pins),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(bg, Vector4::One)), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(pins, 12))
		)

		DECLSPINNER(
			SpinnerAtom,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3))
		)

		DECLSPINNER(
			SpinnerPatternRings,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3))
		)

		DECLSPINNER(
			SpinnerPatternEclipse,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses, OPTIONAL(float) deltaA, OPTIONAL(float) deltaY),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3), DEFAULTVAL(deltaA, 2), DEFAULTVAL(deltaY, 0))
		)

		DECLSPINNER(
			SpinnerPatternSphere,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3))
		)

		DECLSPINNER(
			SpinnerRingSynchronous,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3))
		)

		DECLSPINNER(
			SpinnerRingWatermarks,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3))
		)

		DECLSPINNER(
			SpinnerRotatedAtom,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) elipses),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(elipses, 3))
		)

		DECLSPINNER(
			SpinnerRainbowBalls,
			LIST(float radius, float thickness, Vector4 color, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(radius, thickness, IM_GET_COLOR(color), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 5))
		)

		DECLSPINNER(
			SpinnerRainbowShot,
			LIST(float radius, float thickness, Vector4 color, OPTIONAL(float) speed, OPTIONAL(int) balls),
			LIST(radius, thickness, IM_GET_COLOR(color), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(balls, 5))
		)

		DECLSPINNER(
			SpinnerSpiral,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerSpiralEye,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerBarChartSine,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) bars, OPTIONAL(int) mode),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 5), DEFAULTVAL(mode, 0))
		)

		DECLSPINNER(
			SpinnerBarChartRainbow,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 5))
		)

		DECLSPINNER(
			SpinnerBlocks,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) bg, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(bg, DefaultBg)), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerScaleBlocks,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerScaleSquares,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerFluid,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 3))
		)

		DECLSPINNER(
			SpinnerArcPolarFade,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerArcPolarRadius,
			LIST(OPTIONAL(float) radius, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs),
			LIST(DEFAULTVAL(radius, DefaultRadius), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4))
		)

		DECLSPINNER(
			SpinnerCaleidoscope,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) arcs, OPTIONAL(int) mode),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(arcs, 4), DEFAULTVAL(mode, 0))
		)

		DECLSPINNER(
			SpinnerHboDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) minFade, OPTIONAL(float) ryk, OPTIONAL(float) speed, OPTIONAL(int) dots),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(minFade, 0), DEFAULTVAL(ryk, 0), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(dots, 6))
		)

		DECLSPINNER(
			SpinnerSineArcs,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerTrianglesShift,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, Vector4(1, 1, 1, 0.5f))), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 8))
		)

		DECLSPINNER(
			SpinnerPointsShift,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(Vector4) bg, OPTIONAL(float) speed, OPTIONAL(int) bars),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), IM_GET_COLOR(DEFAULTVAL(bg, Vector4(1, 1, 1, 0.5f))), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(bars, 8))
		)

		DECLSPINNER(
			SpinnerSwingDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerCircularPoints,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lines),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lines, 8))
		)

		DECLSPINNER(
			SpinnerCurvedCircle,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) circles),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(circles, 1))
		)

		DECLSPINNER(
			SpinnerModCircle,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) angMin, OPTIONAL(float) angMax, OPTIONAL(float) speed),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(angMin, 0), DEFAULTVAL(angMax, 1), DEFAULTVAL(speed, DefaultSpeed))
		)

		DECLSPINNER(
			SpinnerDnaDots,
			LIST(OPTIONAL(float) radius, OPTIONAL(float) thickness, OPTIONAL(Vector4) color, OPTIONAL(float) speed, OPTIONAL(int) lt, OPTIONAL(float) delta, OPTIONAL(bool) mode),
			LIST(DEFAULTVAL(radius, DefaultRadius), DEFAULTVAL(thickness, DefaultThickness), IM_GET_COLOR(DEFAULTVAL(color, DefaultColor)), DEFAULTVAL(speed, DefaultSpeed), DEFAULTVAL(lt, 8), DEFAULTVAL(delta, 0.5f), DEFAULTVAL(mode, false))
		)
	};

}}}}