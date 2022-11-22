using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Reflection {
	
	public enum MdSigCallingConvention : byte {
		Default = 0,
		C,
		StdCall,
		ThisCall,
		FastCall,
		Vararg,
		Field,
		LocalSig,
		Property,
		Unmanaged,
		GenericInstance,

		FlagGeneric = 0x10,
		FlagHasThis = 0x20,
		FlagExplicitThis = 0x40
	}

	public enum CorElementType : byte {
		End = 0,
		Void,
		Boolean,
		Char,
		I1,
		U1,
		I2,
		U2,
		I4,
		U4,
		I8,
		U8,
		R4,
		R8,
		String,
		Ptr,
		ByRef,
		ValueType,
		Class,
		Var,
		Array,
		GenericInst,
		TypedByRef,
		I,
		U,
		FnPtr = 0x1B,
		Object,
		SzArray,
		MVar,
		CModReqd,
		CModOpt,
		Internal,
		Max,
		Modifier = 0x40,
		Sentinel,
		Pinned = 0x45
	}

	public record class Signature {

		public MdSigCallingConvention CallConvention { get; init; } = MdSigCallingConvention.Default;

		public CallingConvention? NativeCallConvention {
			get {
				switch (CallConvention & (MdSigCallingConvention)0xF) {
					case MdSigCallingConvention.C:
						return CallingConvention.Cdecl;
					case MdSigCallingConvention.StdCall:
						return CallingConvention.StdCall;
					case MdSigCallingConvention.ThisCall:
						return CallingConvention.ThisCall;
					case MdSigCallingConvention.FastCall:
						return CallingConvention.FastCall;
					default:
						return null;
				}
			}
		}

		public IReadOnlyList<Type> ArgumentTypes { get; init; } = Array.Empty<Type>();

		public Type ReturnType { get; init; } = typeof(void);

		public static Signature Parse(in ReadOnlySpan<byte> sigdata) {
			MdSigCallingConvention cc = (MdSigCallingConvention)sigdata[0];
			int ptr = 1;

			int NextPackInt32(in ReadOnlySpan<byte> dat) {
				int i = dat[ptr++];
				if ((i & 0x80) != 0) {
					i <<= 8;
					i |= dat[ptr++];
					if ((i & 0x4000) != 0) {
						i <<= 16;
						i |= dat[ptr++] << 8;
						i |= dat[ptr++];
					}
				}
				return i;
			}

			Type NextCorType(in ReadOnlySpan<byte> dat) {
				CorElementType et = (CorElementType)dat[ptr++];
				switch(et) {
					case CorElementType.Void:
						return typeof(void);
					case CorElementType.Boolean:
						return typeof(bool);
					case CorElementType.Char:
						return typeof(char);
					case CorElementType.I1:
						return typeof(sbyte);
					case CorElementType.U1:
						return typeof(byte);
					case CorElementType.I2:
						return typeof(short);
					case CorElementType.U2:
						return typeof(ushort);
					case CorElementType.I4:
						return typeof(int);
					case CorElementType.U4:
						return typeof(uint);
					default:
						throw new InvalidOperationException($"No such CorElementType 0x{et:X}");
				}
			}

			int genericParamCount = 0;
			if ((cc & MdSigCallingConvention.FlagGeneric) != 0)
				genericParamCount = NextPackInt32(sigdata);
			
			return new Signature() {
				CallConvention = cc
			};
		}

		public override string ToString() {
			StringBuilder sb = new("{");
			bool reqComma = false;
			void BeginField() {
				if (reqComma) sb.Append(',');
				reqComma = false;
			}
			void EndField() => reqComma = true;

			if (NativeCallConvention!= null) {
				BeginField();
				sb.Append("CallConv=");
				sb.Append(NativeCallConvention);
				EndField();
			}

			MdSigCallingConvention cc = CallConvention & (MdSigCallingConvention)0xF;
			if (cc == MdSigCallingConvention.Vararg) {
				BeginField();
				sb.Append("Vararg");
				EndField();
			}
			if ((CallConvention & MdSigCallingConvention.FlagGeneric) != 0) {
				BeginField();
				sb.Append("Generic");
				EndField();
			}

			if (ArgumentTypes.Count > 0) {
				BeginField();
				sb.Append("Args=[");
				for(int i = 0; i < ArgumentTypes.Count; i++) {
					Type argt = ArgumentTypes[i];
					sb.Append(argt.FullName);
					sb.Append(i == ArgumentTypes.Count - 1 ? ']' : ','); 
				}
				EndField();
			}

			BeginField();
			sb.Append("Returns=");
			sb.Append(ReturnType.FullName);
			EndField();

			sb.Append('}');
			return sb.ToString();
		}

	}

}
