using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

	public enum MetadataTokenType {
		Module = 0x00000000,
		TypeRef = 0x01000000,
		TypeDef = 0x02000000,
		FieldDef = 0x04000000,
		MethodDef = 0x06000000,
		ParamDef = 0x08000000,
		InterfaceImpl = 0x09000000,
		MemberRef = 0x0a000000,
		CustomAttribute = 0x0c000000,
		Permission = 0x0e000000,
		Signature = 0x11000000,
		Event = 0x14000000,
		Property = 0x17000000,
		ModuleRef = 0x1a000000,
		TypeSpec = 0x1b000000,
		Assembly = 0x20000000,
		AssemblyRef = 0x23000000,
		File = 0x26000000,
		ExportedType = 0x27000000,
		ManifestResource = 0x28000000,
		GenericPar = 0x2a000000,
		MethodSpec = 0x2b000000,
		String = 0x70000000,
		Name = 0x71000000,
		BaseType = 0x72000000
	}

	public record class Signature {

		// https://www.debugthings.com/2015/10/13/rewriting-il-remotely-part3/
		// Use this to help decipher the convoluted mess of how signatures are encoded

		public MdSigCallingConvention CallConvention { get; init; } = MdSigCallingConvention.Default;

		public CallingConvention? NativeCallConvention => 
			(CallConvention & (MdSigCallingConvention)0xF) switch {
				MdSigCallingConvention.C => (CallingConvention?)CallingConvention.Cdecl,
				MdSigCallingConvention.StdCall => (CallingConvention?)CallingConvention.StdCall,
				MdSigCallingConvention.ThisCall => (CallingConvention?)CallingConvention.ThisCall,
				MdSigCallingConvention.FastCall => (CallingConvention?)CallingConvention.FastCall,
				_ => null,
			};

		public IReadOnlyList<Type> ArgumentTypes { get; init; } = Array.Empty<Type>();

		public Type ReturnType { get; init; } = typeof(void);

		public static Signature Parse(Module module, in ReadOnlySpan<byte> sigdata) {
			MdSigCallingConvention cc = (MdSigCallingConvention)sigdata[0];
			int ptr = 1;

			int NextPackInt32(in ReadOnlySpan<byte> dat) {
				int i = dat[ptr++];
				if ((i & 0x80) != 0) {
					if ((i & 0x40) != 0) {
						i <<= 24;
						i |= dat[ptr++] << 16;
						i |= dat[ptr++] << 8;
						i &= 0x1FFFFF00;
					} else {
						i <<= 8;
						i &= 0x3F00;
					}
					i |= dat[ptr++];
				}
				return i;
			}

			Type NextTypeToken(in ReadOnlySpan<byte> dat) {
				int rid = NextPackInt32(dat);
				int mode = rid & 0x3;
				rid >>= 2;
				switch(mode) {
					case 0b00:
						rid |= (int)MetadataTokenType.TypeDef;
						break;
					case 0b01:
						rid |= (int)MetadataTokenType.TypeRef;
						break;
					case 0b10:
						rid |= (int)MetadataTokenType.TypeSpec;
						break;
					case 0b11:
						rid |= (int)MetadataTokenType.BaseType;
						break;
				}
				return module.ResolveType(rid);
			}

			Type NextCorType(in ReadOnlySpan<byte> dat) {
				CorElementType et = (CorElementType)dat[ptr++];
				switch (et) {
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
					case CorElementType.I8:
						return typeof(long);
					case CorElementType.U8:
						return typeof(ulong);
					case CorElementType.R4:
						return typeof(float);
					case CorElementType.R8:
						return typeof(double);
					case CorElementType.String:
						return typeof(string);
					case CorElementType.Ptr:
						return NextCorType(dat).MakePointerType();
					case CorElementType.ByRef:
						return NextCorType(dat).MakeByRefType();
					case CorElementType.ValueType:
					case CorElementType.Class:
						return NextTypeToken(dat);
					case CorElementType.Var:
						// TODO: Class variable type slot ????
						break;
					case CorElementType.Array: {
							var elemt = NextCorType(dat);
							int rank = NextPackInt32(dat);
							int nsizes = NextPackInt32(dat);
							for (int i = 0; i < nsizes; i++) NextPackInt32(dat);
							int nbases = NextPackInt32(dat);
							for (int i = 0; i < nbases; i++) NextPackInt32(dat);
							return elemt.MakeArrayType(rank);
						}
					case CorElementType.GenericInst: {
							Type baset = NextCorType(dat);
							int genericCount = NextPackInt32(dat);
							Type[] generics = new Type[genericCount];
							for (int i = 0; i < genericCount; i++)
								generics[i] = NextCorType(dat);
							return baset.MakeGenericType(generics);
						}
					case CorElementType.I:
						return typeof(nint);
					case CorElementType.U:
						return typeof(nuint);
					case CorElementType.FnPtr:
						// Note: For the purposes of C#, function pointers are just IntPtr types apparently
						return typeof(IntPtr);
					case CorElementType.Object:
						return typeof(object);
					case CorElementType.SzArray:
						return NextCorType(dat).MakeArrayType();
					case CorElementType.MVar:
						// TODO: Method variable type modifier ????
						break;
					case CorElementType.TypedByRef:
					case CorElementType.CModReqd:
					case CorElementType.CModOpt:
					case CorElementType.Sentinel:
						return NextCorType(dat);
					default:
						break;
				}
				throw new InvalidOperationException($"Unrecognized CorElementType 0x{et:X}");
			}

			int genericParamCount = 0;
			if ((cc & MdSigCallingConvention.FlagGeneric) != 0)
				genericParamCount = NextPackInt32(sigdata);

			int paramCount = NextPackInt32(sigdata);

			Type returnType = NextCorType(sigdata);

			List<Type> paramTypes = new(paramCount);
			for (int i = 0; i < paramCount; i++) paramTypes.Add(NextCorType(sigdata));
			
			return new Signature() {
				CallConvention = cc,
				ArgumentTypes = paramTypes,
				ReturnType = returnType
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
