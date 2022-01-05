using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Native;

namespace Tesseract.OpenAL {

	[SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Effect properties use overlapping indices for different types of effects")]
	public enum ALEffectParameter : int {
		ReverbDensity = 0x0001,
		ReverbDiffusion = 0x0002,
		ReverbGain = 0x0003,
		ReverbGainHF = 0x0004,
		ReverbDecayTime = 0x0005,
		ReverbDecayHFRatio = 0x0006,
		ReverbReflectionsGain = 0x0007,
		ReverbReflectionsDelay = 0x0008,
		ReverbLateReverbGain = 0x0009,
		ReverbLateReverbDelay = 0x000A,
		ReverbAirAbsorptionGainHF = 0x000B,
		ReverbRoomRolloffFactor = 0x000C,
		ReverbDecayHFLimit = 0x000D,

		EAXReverbDensity = 0x0001,
		EAXReverbDiffusion = 0x0002,
		EAXReverbGain = 0x0003,
		EAXReverbGainHF = 0x0004,
		EAXReverbGainLF = 0x0005,
		EAXReverbDecayTime = 0x0006,
		EAXReverbDecayHFRatio = 0x0007,
		EAXReverbDecayLFRatio = 0x0008,
		EAXReverbReflectionsGain = 0x0009,
		EAXReverbReflectionsDelay = 0x000A,
		EAXReverbReflectionsPan = 0x000E,
		EAXReverbEchoTime = 0x000F,
		EAXReverbEchoDepth = 0x0010,
		EAXReverbModulationTime = 0x0011,
		EAXReverbModulationDepth = 0x0012,
		EAXReverbAirAbsorptionGainHF = 0x0013,
		EAXReverbHFReference = 0x0014,
		EAXReverbLFReference = 0x0015,
		EAXReverbRoomRolloffFactor = 0x0016,
		EAXReverbDecayHFLimit = 0x0017,

		ChorusWaveform = 0x0001,
		ChorusPhase = 0x0002,
		ChorusRate = 0x0003,
		ChorusDepth = 0x0004,
		ChorusFeedback = 0x0005,
		ChorusDelay = 0x0006,

		DistortionEdge = 0x0001,
		DistortionGain = 0x0002,
		DistortionLowpassCutoff = 0x0003,
		DistortionEqCenter = 0x0004,
		DistortionEqBandwidth = 0x0005,

		EchoDelay = 0x0001,
		EchoLRDelay = 0x0002,
		EchoDamping = 0x0003,
		EchoFeedback = 0x0004,
		EchoSpread = 0x0005,

		FlangerWaveform = 0x0001,
		FlangerPhase = 0x0002,
		FlangerRate = 0x0003,
		FlangerDepth = 0x0004,
		FlangerFeedback = 0x0005,
		FlangerDelay = 0x0006,

		FrequencyShifterFrequency = 0x0001,
		FrequencyShifterLeftDirection = 0x0002,
		FrequencyShifterRightDirection = 0x0003,

		VocalMorpherPhonemeA = 0x0001,
		VocalMorpherPhonemeACoarseTuning = 0x0002,
		VocalMorpherPhonemeB = 0x0003,
		VocalMorpherPhonemeBCoarseTuning = 0x0004,
		VocalMorpherWaveform = 0x0005,
		VocalMorpherRate = 0x0006,

		PitchShifterCoarseTune = 0x0001,
		PitchShifterFineTune = 0x0002,

		RingModulatorFrequency = 0x0001,
		RingModulatorHighpassCutoff = 0x0002,
		RingModulatorWaveform = 0x0003,

		AutowahAttackTime = 0x0001,
		AutowahReleaseTime = 0x0002,
		AutowahResonance = 0x0003,
		AutowahPeakGain = 0x0004,

		CompressorOnOff = 0x0001,

		EqualizerLowGain = 0x0001,
		EqualizerLowCutoff = 0x0002,
		EqualizerMid1Gain = 0x0003,
		EqualizerMid1Center = 0x0004,
		EqualizerMid1Width = 0x0005,
		EqualizerMid2Gain = 0x0006,
		EqualizerMid2Center = 0x0007,
		EqualizerMid2Width = 0x0008,
		HighGain = 0x0009,
		HighCutoff = 0x000A
	}

	public enum ALEffectAttrib : int {
		FirstParameter = 0x0000,
		LastParameter = 0x8000,
		Type = 0x8001
	}

	public enum ALEffectType : int {
		Null = 0,
		Reverb = 0x0001,
		Chorus = 0x0002,
		Distortion = 0x0003,
		Echo = 0x0004,
		Flanger = 0x0005,
		FrequencyChanger = 0x0006,
		VocalMorpher = 0x0007,
		PitchShifter = 0x0008,
		RingModulator = 0x0009,
		Autowah = 0x000A,
		Compressor = 0x000B,
		Equalizer = 0x000C,
		EAXReverb = 0x8000
	}

	public enum ALEffectSlotAttrib : int {
		Effect = 0x0001,
		Gain = 0x0002,
		AuxiliarySendAuto = 0x0003
	}

	[SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "Filter properties use overlapping indices for different types of filters")]
	public enum ALFilterParameter : int {
		LowpassGain = 0x0001,
		LowpassGainHF = 0x0002,

		HighpassGain = 0x0001,
		HighpassGainLF = 0x0002,

		BandpassGain = 0x0001,
		BandpassGainLF = 0x0002,
		BandpassGainHF = 0x0003
	}

	public enum ALFilterAttrib : int {
		FirstParameter = 0x0000,
		LastParameter = 0x8000,
		Type = 0x8001
	}

	public enum ALFilterType : int {
		Null = 0,
		Lowpass = 0x0001,
		Highpass = 0x0002,
		Bandpass = 0x0003
	}

	public enum ALChorusEffectWaveform : int {
		Sinusoid = 0,
		Triangle = 1
	}

	public enum ALFlangerEffectWaveform : int {
		Sinusoid = 0,
		Triangle = 1
	}

	public enum ALFrequencyShifterEffectDirection : int {
		Down = 0,
		Up = 1,
		Off = 2
	}

	public enum ALVocalMorpherEffectPhoneme : int {
		A = 0, E, I, O, U, AA, AE, AH, AO, EH, ER, IH, IY, UH, UW, B, D, F, G, J, K, L, M, N, P, R, S, T, V, Z
	}

	public enum ALVocalMorpherEffectWaveform : int {
		Sinusoid = 0,
		Triangle = 1,
		Sawtooth = 2
	}

	public enum ALRingModulatorEffectWaveform : int {
		Sinusoid = 0,
		Sawtooth = 1,
		Square = 2
	}

#nullable disable
	public class EXTEFXFunctions {

		public delegate void PFN_alGenEffects(int n, [NativeType("ALuint*")] IntPtr effects);
		public delegate void PFN_alDeleteEffects(int n, [NativeType("const ALuint*")] IntPtr effects);
		public delegate byte PFN_alIsEffect(uint effect);
		public delegate void PFN_alEffecti(uint effect, int param, int value);
		public delegate void PFN_alEffectiv(uint effect, int param, [NativeType("const ALint*")] IntPtr values);
		public delegate void PFN_alEffectf(uint effect, int param, float value);
		public delegate void PFN_alEffectfv(uint effect, int param, [NativeType("const ALfloat*")] IntPtr values);
		public delegate void PFN_alGetEffecti(uint effect, int param, out int value);
		public delegate void PFN_alGetEffectiv(uint effect, int param, [NativeType("ALint*")] IntPtr values);
		public delegate void PFN_alGetEffectf(uint effect, int param, out float value);
		public delegate void PFN_alGetEffectfv(uint effect, int param, [NativeType("ALfloat*")] IntPtr values);

		public PFN_alGenEffects alGenEffects;
		public PFN_alDeleteEffects alDeleteEffects;
		public PFN_alIsEffect alIsEffect;
		public PFN_alEffecti alEffecti;
		public PFN_alEffectiv alEffectiv;
		public PFN_alEffectf alEffectf;
		public PFN_alEffectfv alEffectfv;
		public PFN_alGetEffecti alGetEffecti;
		public PFN_alGetEffectiv alGetEffectiv;
		public PFN_alGetEffectf alGetEffectf;
		public PFN_alGetEffectfv alGetEffectfv;

		public delegate void PFN_alGenFilters(int n, [NativeType("ALuint*")] IntPtr effects);
		public delegate void PFN_alDeleteFilters(int n, [NativeType("const ALuint*")] IntPtr effects);
		public delegate byte PFN_alIsFilter(uint effect);
		public delegate void PFN_alFilteri(uint effect, int param, int value);
		public delegate void PFN_alFilteriv(uint effect, int param, [NativeType("const ALint*")] IntPtr values);
		public delegate void PFN_alFilterf(uint effect, int param, float value);
		public delegate void PFN_alFilterfv(uint effect, int param, [NativeType("const ALfloat*")] IntPtr values);
		public delegate void PFN_alGetFilteri(uint effect, int param, out int value);
		public delegate void PFN_alGetFilteriv(uint effect, int param, [NativeType("ALint*")] IntPtr values);
		public delegate void PFN_alGetFilterf(uint effect, int param, out float value);
		public delegate void PFN_alGetFilterfv(uint effect, int param, [NativeType("ALfloat*")] IntPtr values);

		public PFN_alGenFilters alGenFilters;
		public PFN_alDeleteFilters alDeleteFilters;
		public PFN_alIsFilter alIsFilter;
		public PFN_alFilteri alFilteri;
		public PFN_alFilteriv alFilteriv;
		public PFN_alFilterf alFilterf;
		public PFN_alFilterfv alFilterfv;
		public PFN_alGetFilteri alGetFilteri;
		public PFN_alGetFilteriv alGetFilteriv;
		public PFN_alGetFilterf alGetFilterf;
		public PFN_alGetFilterfv alGetFilterfv;

		public delegate void PFN_alGenAuxiliaryEffectSlots(int n, [NativeType("ALuint*")] IntPtr effects);
		public delegate void PFN_alDeleteAuxiliaryEffectSlots(int n, [NativeType("const ALuint*")] IntPtr effects);
		public delegate byte PFN_alIsAuxiliaryEffectSlot(uint effect);
		public delegate void PFN_alAuxiliaryEffectSloti(uint effect, ALEffectSlotAttrib param, int value);
		public delegate void PFN_alAuxiliaryEffectSlotiv(uint effect, ALEffectSlotAttrib param, [NativeType("const ALint*")] IntPtr values);
		public delegate void PFN_alAuxiliaryEffectSlotf(uint effect, ALEffectSlotAttrib param, float value);
		public delegate void PFN_alAuxiliaryEffectSlotfv(uint effect, ALEffectSlotAttrib param, [NativeType("const ALfloat*")] IntPtr values);
		public delegate void PFN_alGetAuxiliaryEffectSloti(uint effect, ALEffectSlotAttrib param, out int value);
		public delegate void PFN_alGetAuxiliaryEffectSlotiv(uint effect, ALEffectSlotAttrib param, [NativeType("ALint*")] IntPtr values);
		public delegate void PFN_alGetAuxiliaryEffectSlotf(uint effect, ALEffectSlotAttrib param, out float value);
		public delegate void PFN_alGetAuxiliaryEffectSlotfv(uint effect, ALEffectSlotAttrib param, [NativeType("ALfloat*")] IntPtr values);

		public PFN_alGenAuxiliaryEffectSlots alGenAuxiliaryEffectSlots;
		public PFN_alDeleteAuxiliaryEffectSlots alDeleteAuxiliaryEffectSlots;
		public PFN_alIsAuxiliaryEffectSlot alIsAuxiliaryEffectSlot;
		public PFN_alAuxiliaryEffectSloti alAuxiliaryEffectSloti;
		public PFN_alAuxiliaryEffectSlotiv alAuxiliaryEffectSlotiv;
		public PFN_alAuxiliaryEffectSlotf alAuxiliaryEffectSlotf;
		public PFN_alAuxiliaryEffectSlotfv alAuxiliaryEffectSlotfv;
		public PFN_alGetAuxiliaryEffectSloti alGetAuxiliaryEffectSloti;
		public PFN_alGetAuxiliaryEffectSlotiv alGetAuxiliaryEffectSlotiv;
		public PFN_alGetAuxiliaryEffectSlotf alGetAuxiliaryEffectSlotf;
		public PFN_alGetAuxiliaryEffectSlotfv alGetAuxiliaryEffectSlotfv;

	}
#nullable restore

	public class EXTEFX {

		public const string ExtensionName = "ALC_EXT_EFX";

		public EXTEFXFunctions Functions { get; } = new();

		public EXTEFX(AL al) {
			Library.LoadFunctions(al.GetProcAddress, Functions);
		}

		// Unless otherwise defined, min-max range is 0.0 to 1.0, and default value is 1.0. Enum defaults are unspecified

		public const float ReverbDefaultGain = 0.32f;
		public const float ReverbDefaultGainHF = 0.89f;
		public const float ReverbMinDecayTime = 0.1f;
		public const float ReverbMaxDecayTime = 20.0f;
		public const float ReverbDefaultDecayTime = 1.49f;
		public const float ReverbMinDecayHFRatio = 0.1f;
		public const float ReverbMaxDecayHFRatio = 2.0f;
		public const float ReverbDefaultDecayHFRatio = 0.83f;
		public const float ReverbMaxReflectionsGain = 3.16f;
		public const float ReverbDefaultReflectionsGain = 0.05f;
		public const float ReverbMaxReflectionsDelay = 0.3f;
		public const float ReverbDefaultReflectionsDelay = 0.007f;
		public const float ReverbMaxLateReverbGain = 10.0f;
		public const float ReverbDefaultLateReverbGain = 1.26f;
		public const float ReverbMaxLateReverbDelay = 0.1f;
		public const float ReverbDefaultLateReverbDelay = 0.011f;
		public const float ReverbMinAirAbsorptionGainHF = 0.892f;
		public const float ReverbDefaultAirAbsorptionGainHF = 0.994f;
		public const float ReverbMaxRoomRolloffFactor = 10.0f;
		public const float ReverbDefaultRoomRolloffFactor = 0.0f;

		public const float EAXReverbDefaultGain = 0.32f;
		public const float EAXReverbDefaultGainHF = 0.89f;
		public const float EAXReverbMaxDecayTime = 20.0f;
		public const float EAXReverbDefaultDecayTime = 1.49f;
		public const float EAXReverbMinDecayHFRatio = 0.1f;
		public const float EAXReverbMaxDecayHFRatio = 2.0f;
		public const float EAXReverbDefaultDecayHFRatio = 0.83f;
		public const float EAXReverbMinDecayLFRatio = 0.1f;
		public const float EAXReverbMaxDecayLFRatio = 2.0f;
		public const float EAXReverbMaxReflectionsGain = 3.16f;
		public const float EAXReverbDefaultReflectionsGain = 0.05f;
		public const float EAXReverbMaxReflectionsDelay = 0.3f;
		public const float EAXReverbDefaultReflectionsDelay = 0.007f;
		public const float EAXReverbMaxLateReverbGain = 10.0f;
		public const float EAXReverbDefaultLateReverbGain = 1.26f;
		public const float EAXReverbMaxLateReverbDelay = 0.1f;
		public const float EAXReverbDefaultLateReverbDelay = 0.011f;
		public const float EAXReverbMinEchoTime = 0.075f;
		public const float EAXReverbMaxEchoTime = 0.25f;
		public const float EAXReverbDefaultEchoTime = 0.25f;
		public const float EAXReverbDefaultEchoDepth = 0.0f;
		public const float EAXReverbMinModulationTime = 0.04f;
		public const float EAXReverbMaxModulationTIme = 4.0f;
		public const float EAXReverbDefaultModulationTime = 0.25f;
		public const float EAXReverbDefaultModulationDepth = 0.0f;
		public const float EAXReverbMinAirAbsorptionGainHF = 0.892f;
		public const float EAXReverbMaxAirAbsorptionGainHF = 1.0f;
		public const float EAXReverbDefaultAirAbsorptionGainHF = 0.994f;
		public const float EAXReverbMinHFReference = 1000.0f;
		public const float EAXReverbMaxHFReference = 20000.0f;
		public const float EAXReverbDefaultHFReference = 5000.0f;
		public const float EAXReverbMinLFReference = 20.0f;
		public const float EAXReverbMaxLFReference = 1000.0f;
		public const float EAXReverbDefaultLFReference = 250.0f;
		public const float EAXReverbMaxRoomRolloffFactor = 10.0f;
		public const float EAXReverbDefaultRoomRolloffFactor = 0.0f;

		public const float ChorusMinPhase = -180.0f;
		public const float ChorusMaxPhase = 180.0f;
		public const float ChorusDefualtPhase = 90.0f;
		public const float ChorusMaxRate = 10.0f;
		public const float ChorusDefaultRate = 1.1f;
		public const float ChorusDefaultDepth = 0.1f;
		public const float ChorusMinFeedback = -1.0f;
		public const float ChorusDefaultFeedback = 0.25f;
		public const float ChorusMaxDelay = 0.016f;
		public const float ChorusDefaultDelay = 0.016f;

		public const float DistortionDefaultEdge = 0.2f;
		public const float DistortionMinGain = 0.01f;
		public const float DistortionDefaultGain = 0.05f;
		public const float DistortionMinLowpassCutoff = 80.0f;
		public const float DistortionMaxLowpassCutoff = 24000.0f;
		public const float DistortionDefaultLowpassCutoff = 8000.0f;
		public const float DistortionMinEqCenter = 80.0f;
		public const float DistortionMaxEqCenter = 24000.0f;
		public const float DistortionDefaultEqCenter = 3600.0f;
		public const float DistortionMinEqBandwidth = 80.0f;
		public const float DistortionMaxEqBandwidth = 24000.0f;
		public const float DistortionDefaultEqBandwidth = 3600.0f;

		public const float EchoMaxDelay = 0.207f;
		public const float EchoDefaultDelay = 0.1f;
		public const float EchoMaxLRDelay = 0.404f;
		public const float EchoDefaultLRDelay = 0.1f;
		public const float EchoMaxDamping = 0.99f;
		public const float EchoDefaultDamping = 0.5f;
		public const float EchoDefaultFeedback = 0.5f;
		public const float EchoMinSpread = -1.0f;
		public const float EchoDefaultSpread = -1.0f;

		public const float FlangerMinPhase = -180.0f;
		public const float FlangerMaxPhase = 180.0f;
		public const float FlangerDefaultPhase = 0;
		public const float FlangerMaxRate = 10.0f;
		public const float FlangerDefaultRate = 0.27f;
		public const float FlangerMinFeedback = -1.0f;
		public const float FlangerDefaultFeedback = -0.5f;
		public const float FlangerMaxDelay = 0.004f;
		public const float FlangerDefaultDelay = 0.002f;

		public const float FrequencyShifterMaxFrequency = 24000.0f;
		public const float FrequencyShifterDefaultFrequency = 0.0f;

		public const float VocalMorpherMinPhonemeACoarseTuning = -24;
		public const float VocalMorpherMaxPhonemeACoarseTuning = 24;
		public const float VocalMorpherDefaultPhonemeACoarseTuning = 0;
		public const float VocalMorpherMinPhonemeBCoarseTuning = -24;
		public const float VocalMorpherMaxPhonemeBCoarseTuning = 24;
		public const float VocalMorpherDefaultPhonemeBCoarseTuning = 0;
		public const float VocalMorpherMaxRate = 10.0f;
		public const float VocalMorpherDefaultRate = 1.41f;

		public const float PitchShifterMinCoarseTune = -12;
		public const float PitchShifterMaxCoarseTune = 12;
		public const float PitchShifterDefaultCoarseTune = 12;
		public const float PitchShifterMinFineTune = -50;
		public const float PitchShifterMaxFineTune = 50;
		public const float PitchShifterDefaultFineTune = 0;

		public const float RingModulatorMaxFrequency = 8000.0f;
		public const float RingModulatorDefaultFrequency = 440.0f;
		public const float RingModulatorMaxHighpassCutoff = 24000.0f;
		public const float RingModulatorDefaultHighpassCutoff = 800.0f;

		public const float AutowahMinAttackTime = 0.0001f;
		public const float AutowahDefaultAttackTime = 0.06f;
		public const float AutowahMinReleaseTime = 0.0001f;
		public const float AutowahDefaultReleaseTime = 0.06f;
		public const float AutowahMinResonance = 2.0f;
		public const float AutowahMaxResonance = 1000.0f;
		public const float AutowahDefaultResonance = 1000.0f;
		public const float AutowahMinPeakGain = 0.00003f;
		public const float AutowahMaxPeakGain = 31621.0f;
		public const float AutowahDefaultPeakGain = 11.22f;

		public const float EqualizerMinLowGain = 0.126f;
		public const float EqualizerMaxLowGain = 7.943f;
		public const float EqualizerMinLowCutoff = 50.0f;
		public const float EqualizerMaxLowCutoff = 800.0f;
		public const float EqualizerDefaultLowCutoff = 200.0f;
		public const float EqualizerMinMid1Gain = 0.126f;
		public const float EqualizerMaxMid1Gain = 7.943f;
		public const float EqualizerMinMid1Cutoff = 200.0f;
		public const float EqualizerMaxMid1Cutoff = 3000.0f;
		public const float EqualizerDefaultMid1Cutoff = 500.0f;
		public const float EqualizerMinMid1Width = 0.01f;
		public const float EqualizerMinMid2Gain = 0.126f;
		public const float EqualizerMaxMid2Gain = 7.943f;
		public const float EqualizerMinMid2Cutoff = 1000.0f;
		public const float EqualizerMaxMid2Cutoff = 8000.0f;
		public const float EqualizerDefaultMid2Cutoff = 3000.0f;
		public const float EqualizerMinMid2Width = 0.01f;
		public const float EqualizerMinHighGain = 0.126f;
		public const float EqualizerMaxHighGain = 7.943f;
		public const float EqualizerMinHighCutoff = 4000.0f;
		public const float EqualizerMaxHighCutoff = 16000.0f;
		public const float EqualizerDefaultHighCutoff = 6000.0f;

		public const float MaxAirAbsorptionFactor = 10.0f;
		public const float DefaultAirAbsorptionFactor = 0.0f;
		public const float MaxRoomRolloffFactor = 10.0f;
		public const float DefaultRoomRolloffFactor = 0.0f;

		public const float MinMetersPerUnit = float.Epsilon;
		public const float MaxMetersPerUnit = float.MaxValue;

	}

	public class ALEffect : IDisposable, IALObject {

		public AL AL { get; }
		public uint Effect { get; }

#nullable disable
		public ALEffectType Type {
			get {
				AL.EXTEFX.Functions.alGetEffecti(Effect, (int)ALEffectAttrib.Type, out int value);
				return (ALEffectType)value;
			}
			set => AL.EXTEFX.Functions.alEffecti(Effect, (int)ALEffectAttrib.Type, (int)value);
		}

		public ALEffect(AL al) {
			AL = al;
			uint effect = 0;
			unsafe {
				AL.EXTEFX.Functions.alGenEffects(1, (IntPtr)(&effect));
			}
			Effect = effect;
		}

		public ALEffect(AL al, uint effect) {
			AL = al;
			Effect = effect;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			uint effect = Effect;
			unsafe {
				AL.EXTEFX.Functions.alDeleteEffects(1, (IntPtr)(&effect));
			}
		}

		public void SetParam(ALEffectParameter param, float value) {
			AL.EXTEFX.Functions.alEffectf(Effect, (int)ALEffectAttrib.FirstParameter + (int)param, value);
		}

		public void SetParam<E>(ALEffectParameter param, E value) where E : Enum {
			AL.EXTEFX.Functions.alEffectf(Effect, (int)ALEffectAttrib.FirstParameter + (int)param, (int)(object)value);
		}

		public float GetParam(ALEffectParameter param) {
			AL.EXTEFX.Functions.alGetEffectf(Effect, (int)ALEffectAttrib.FirstParameter + (int)param, out float value);
			return value;
		}

		public E GetParam<E>(ALEffectParameter param) where E : Enum {
			AL.EXTEFX.Functions.alGetEffecti(Effect, (int)ALEffectAttrib.FirstParameter + (int)param, out int value);
			return (E)(object)value;
		}

		public void SetParam(ALEffectParameter param, bool value) {
			AL.EXTEFX.Functions.alEffecti(Effect, (int)ALEffectAttrib.FirstParameter + (int)param, value ? 1 : 0);
		}
#nullable restore

	}

	public class ALFilter : IDisposable, IALObject {

		public AL AL { get; }
		public uint Filter { get; }

#nullable disable
		public ALFilterType Type {
			get {
				AL.EXTEFX.Functions.alGetFilteri(Filter, (int)ALFilterAttrib.Type, out int value);
				return (ALFilterType)value;
			}
			set => AL.EXTEFX.Functions.alFilteri(Filter, (int)ALFilterAttrib.Type, (int)value);
		}

		public ALFilter(AL al) {
			AL = al;
			uint filter = 0;
			unsafe {
				AL.EXTEFX.Functions.alGenFilters(1, (IntPtr)(&filter));
			}
			Filter = filter;
		}

		public ALFilter(AL al, uint filter) {
			AL = al;
			Filter = filter;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			uint effect = Filter;
			unsafe {
				AL.EXTEFX.Functions.alDeleteFilters(1, (IntPtr)(&effect));
			}
		}

		public void SetParam(ALFilterParameter param, float value) {
			AL.EXTEFX.Functions.alFilterf(Filter, (int)ALFilterAttrib.FirstParameter + (int)param, value);
		}

		public void SetParam<E>(ALFilterParameter param, E value) where E : Enum {
			AL.EXTEFX.Functions.alFilterf(Filter, (int)ALFilterAttrib.FirstParameter + (int)param, (int)(object)value);
		}

		public float GetParam(ALFilterParameter param) {
			AL.EXTEFX.Functions.alGetFilterf(Filter, (int)ALFilterAttrib.FirstParameter + (int)param, out float value);
			return value;
		}

		public E GetParam<E>(ALFilterParameter param) where E : Enum {
			AL.EXTEFX.Functions.alGetFilteri(Filter, (int)ALFilterAttrib.FirstParameter + (int)param, out int value);
			return (E)(object)value;
		}
#nullable restore

	}

	public class ALAuxiliaryEffectSlot : IDisposable, IALObject {

		public AL AL { get; }
		public uint AuxiliaryEffectSlot { get; }

#nullable disable
		public ALEffect Effect {
			get {
				AL.EXTEFX.Functions.alGetAuxiliaryEffectSloti(AuxiliaryEffectSlot, ALEffectSlotAttrib.Effect, out int value);
				return value != 0 ? new ALEffect(AL, (uint)value) : null;
			}
			set => AL.EXTEFX.Functions.alAuxiliaryEffectSloti(AuxiliaryEffectSlot, ALEffectSlotAttrib.Effect, value != null ? (int)value.Effect : 0);
		}

		public float Gain {
			get {
				AL.EXTEFX.Functions.alGetAuxiliaryEffectSlotf(AuxiliaryEffectSlot, ALEffectSlotAttrib.Gain, out float value);
				return value;
			}
			set => AL.EXTEFX.Functions.alAuxiliaryEffectSlotf(AuxiliaryEffectSlot, ALEffectSlotAttrib.Gain, value);
		}

		public bool AuxiliarySendAuto {
			get {
				AL.EXTEFX.Functions.alGetAuxiliaryEffectSloti(AuxiliaryEffectSlot, ALEffectSlotAttrib.AuxiliarySendAuto, out int value);
				return value != 0;
			}
			set => AL.EXTEFX.Functions.alAuxiliaryEffectSloti(AuxiliaryEffectSlot, ALEffectSlotAttrib.AuxiliarySendAuto, value ? 1 : 0);
		}

		public ALAuxiliaryEffectSlot(AL al) {
			AL = al;
			uint filter = 0;
			unsafe {
				AL.EXTEFX.Functions.alGenAuxiliaryEffectSlots(1, (IntPtr)(&filter));
			}
			AuxiliaryEffectSlot = filter;
		}

		public ALAuxiliaryEffectSlot(AL al, uint filter) {
			AL = al;
			AuxiliaryEffectSlot = filter;
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
			uint effect = AuxiliaryEffectSlot;
			unsafe {
				AL.EXTEFX.Functions.alDeleteAuxiliaryEffectSlots(1, (IntPtr)(&effect));
			}
		}
#nullable restore

	}

}
