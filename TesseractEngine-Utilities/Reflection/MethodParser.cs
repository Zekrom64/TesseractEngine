using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Reflection {

	public class MethodParser {

		public MethodBase Method { get; }

		public MethodBody Body { get; }

		private readonly byte[] bytecode;
		public IReadOnlyList<byte> Bytecode => bytecode;

		public MethodParser(MethodBase method) {
			Method = method;
			Body = method.GetMethodBody() ?? throw new ArgumentException("Cannot parse abstract method", nameof(method));
			bytecode = Body.GetILAsByteArray() ?? throw new InvalidOperationException("Could not get method IL");
		}

		public void Visit(IMethodVisitor visitor) {
			Module module = Method.Module;
			int ip = 0, ip2 = ip;

			OpCodeValue NextOp() {
				int op = bytecode[ip2++];
				if (op >= 0xF8) op |= bytecode[ip2++] << 8;
				return (OpCodeValue)op;
			}

			byte NextU1() => bytecode[ip2++];

			ushort NextU2() {
				ushort u = bytecode[ip2++];
				u |= (ushort)(bytecode[ip2++] << 8);
				return u;
			}

			int NextI4() {
				int i = bytecode[ip2++];
				i |= bytecode[ip2++] << 8;
				i |= bytecode[ip2++] << 16;
				i |= bytecode[ip2++] << 24;
				return i;
			}

			long NextI8() {
				long l = NextI4();
				l |= (long)NextI4() << 32;
				return l;
			}

			while (ip < bytecode.Length) {
				OpCodeValue op = NextOp();
				switch (op) {
					case OpCodeValue.Nop:
						visitor.Visit(ip, OpCodes.Nop);
						break;
					case OpCodeValue.Break:
						visitor.Visit(ip, OpCodes.Break);
						break;
					case OpCodeValue.Ldarg_0:
						visitor.Visit(ip, OpCodes.Ldarg_0);
						break;
					case OpCodeValue.Ldarg_1:
						visitor.Visit(ip, OpCodes.Ldarg_1);
						break;
					case OpCodeValue.Ldarg_2:
						visitor.Visit(ip, OpCodes.Ldarg_2);
						break;
					case OpCodeValue.Ldarg_3:
						visitor.Visit(ip, OpCodes.Ldarg_3);
						break;
					case OpCodeValue.Ldloc_0:
						visitor.Visit(ip, OpCodes.Ldloc_0);
						break;
					case OpCodeValue.Ldloc_1:
						visitor.Visit(ip, OpCodes.Ldloc_1);
						break;
					case OpCodeValue.Ldloc_2:
						visitor.Visit(ip, OpCodes.Ldloc_2);
						break;
					case OpCodeValue.Ldloc_3:
						visitor.Visit(ip, OpCodes.Ldloc_3);
						break;
					case OpCodeValue.Stloc_0:
						visitor.Visit(ip, OpCodes.Stloc_0);
						break;
					case OpCodeValue.Stloc_1:
						visitor.Visit(ip, OpCodes.Stloc_1);
						break;
					case OpCodeValue.Stloc_2:
						visitor.Visit(ip, OpCodes.Stloc_2);
						break;
					case OpCodeValue.Stloc_3:
						visitor.Visit(ip, OpCodes.Stloc_3);
						break;
					case OpCodeValue.Ldarg_S:
						visitor.VisitWithIndex(ip, OpCodes.Ldarg_S, NextU1());
						break;
					case OpCodeValue.Ldarga_S:
						visitor.VisitWithIndex(ip, OpCodes.Ldarga_S, NextU1());
						break;
					case OpCodeValue.Starg_S:
						visitor.VisitWithIndex(ip, OpCodes.Starg, NextU1());
						break;
					case OpCodeValue.Ldloc_S:
						visitor.VisitWithIndex(ip, OpCodes.Ldloc_S, NextU1());
						break;
					case OpCodeValue.Ldloca_S:
						visitor.VisitWithIndex(ip, OpCodes.Ldloca_S, NextU1());
						break;
					case OpCodeValue.Stloc_S:
						visitor.VisitWithIndex(ip, OpCodes.Stloc_S, NextU1());
						break;
					case OpCodeValue.Ldnull:
						visitor.Visit(ip, OpCodes.Ldnull);
						break;
					case OpCodeValue.Ldc_I4_M1:
						visitor.Visit(ip, OpCodes.Ldc_I4_M1);
						break;
					case OpCodeValue.Ldc_I4_0:
						visitor.Visit(ip, OpCodes.Ldc_I4_0);
						break;
					case OpCodeValue.Ldc_I4_1:
						visitor.Visit(ip, OpCodes.Ldc_I4_1);
						break;
					case OpCodeValue.Ldc_I4_2:
						visitor.Visit(ip, OpCodes.Ldc_I4_2);
						break;
					case OpCodeValue.Ldc_I4_3:
						visitor.Visit(ip, OpCodes.Ldc_I4_3);
						break;
					case OpCodeValue.Ldc_I4_4:
						visitor.Visit(ip, OpCodes.Ldc_I4_4);
						break;
					case OpCodeValue.Ldc_I4_5:
						visitor.Visit(ip, OpCodes.Ldc_I4_5);
						break;
					case OpCodeValue.Ldc_I4_6:
						visitor.Visit(ip, OpCodes.Ldc_I4_6);
						break;
					case OpCodeValue.Ldc_I4_7:
						visitor.Visit(ip, OpCodes.Ldc_I4_7);
						break;
					case OpCodeValue.Ldc_I4_8:
						visitor.Visit(ip, OpCodes.Ldc_I4_8);
						break;
					case OpCodeValue.Ldc_I4_S:
						visitor.VisitLdcWithImmediate(ip, OpCodes.Ldc_I4_S, (sbyte)NextU1());
						break;
					case OpCodeValue.Ldc_I4:
						visitor.VisitLdcWithImmediate(ip, OpCodes.Ldc_I4, NextI4());
						break;
					case OpCodeValue.Ldc_I8:
						visitor.VisitLdcWithImmediate(ip, OpCodes.Ldc_I8, NextI8());
						break;
					case OpCodeValue.Ldc_R4:
						visitor.VisitLdcWithImmediate(ip, OpCodes.Ldc_R4, (decimal)BitConverter.Int32BitsToSingle(NextI4()));
						break;
					case OpCodeValue.Ldc_R8:
						visitor.VisitLdcWithImmediate(ip, OpCodes.Ldc_R4, (decimal)BitConverter.Int64BitsToDouble(NextI8()));
						break;
					case OpCodeValue.Dup:
						visitor.Visit(ip, OpCodes.Dup);
						break;
					case OpCodeValue.Pop:
						visitor.Visit(ip, OpCodes.Pop);
						break;
					case OpCodeValue.Jmp: {
							MethodBase mb = module.ResolveMethod(NextI4()) ?? throw new InvalidOperationException("Could not lookup method token");
							visitor.VisitWithMethodBase(ip, OpCodes.Jmp, mb);
						}
						break;
					case OpCodeValue.Call: {
							MethodBase mb = module.ResolveMethod(NextI4()) ?? throw new InvalidOperationException("Could not lookup method token");
							visitor.VisitWithMethodBase(ip, OpCodes.Call, mb);
						}
						break;
					case OpCodeValue.Calli: {
							byte[] sigdata = module.ResolveSignature(NextI4());
							visitor.VisitCalli(ip, Signature.Parse(sigdata));
						}
						break;
					case OpCodeValue.Ret:
						visitor.Visit(ip, OpCodes.Ret);
						break;
					case OpCodeValue.Br_S:
						visitor.VisitBranch(ip, OpCodes.Br_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Brfalse_S:
						visitor.VisitBranch(ip, OpCodes.Brfalse_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Brtrue_S:
						visitor.VisitBranch(ip, OpCodes.Brtrue_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Beq_S:
						visitor.VisitBranch(ip, OpCodes.Beq_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Bge_S:
						visitor.VisitBranch(ip, OpCodes.Bge_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Bgt_S:
						visitor.VisitBranch(ip, OpCodes.Bgt_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Ble_S:
						visitor.VisitBranch(ip, OpCodes.Ble_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Blt_S:
						visitor.VisitBranch(ip, OpCodes.Blt_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Bne_Un_S:
						visitor.VisitBranch(ip, OpCodes.Bne_Un_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Bge_Un_S:
						visitor.VisitBranch(ip, OpCodes.Bge_Un_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Bgt_Un_S:
						visitor.VisitBranch(ip, OpCodes.Bgt_Un_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Ble_Un_S:
						visitor.VisitBranch(ip, OpCodes.Ble_Un_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Blt_Un_S:
						visitor.VisitBranch(ip, OpCodes.Blt_Un_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Br:
						visitor.VisitBranch(ip, OpCodes.Br, NextI4() + ip2);
						break;
					case OpCodeValue.Brfalse:
						visitor.VisitBranch(ip, OpCodes.Brfalse, NextI4() + ip2);
						break;
					case OpCodeValue.Brtrue:
						visitor.VisitBranch(ip, OpCodes.Brtrue, NextI4() + ip2);
						break;
					case OpCodeValue.Beq:
						visitor.VisitBranch(ip, OpCodes.Beq, NextI4() + ip2);
						break;
					case OpCodeValue.Bge:
						visitor.VisitBranch(ip, OpCodes.Bge, NextI4() + ip2);
						break;
					case OpCodeValue.Bgt:
						visitor.VisitBranch(ip, OpCodes.Bgt, NextI4() + ip2);
						break;
					case OpCodeValue.Ble:
						visitor.VisitBranch(ip, OpCodes.Ble, NextI4() + ip2);
						break;
					case OpCodeValue.Blt:
						visitor.VisitBranch(ip, OpCodes.Blt, NextI4() + ip2);
						break;
					case OpCodeValue.Bne_Un:
						visitor.VisitBranch(ip, OpCodes.Bne_Un, NextI4() + ip2);
						break;
					case OpCodeValue.Bge_Un:
						visitor.VisitBranch(ip, OpCodes.Bge_Un, NextI4() + ip2);
						break;
					case OpCodeValue.Bgt_Un:
						visitor.VisitBranch(ip, OpCodes.Bgt_Un, NextI4() + ip2);
						break;
					case OpCodeValue.Ble_Un:
						visitor.VisitBranch(ip, OpCodes.Ble_Un, NextI4() + ip2);
						break;
					case OpCodeValue.Blt_Un:
						visitor.VisitBranch(ip, OpCodes.Blt_Un, NextI4() + ip2);
						break;
					case OpCodeValue.Switch: {
							int count = NextI4();
							if (count < 0) throw new InvalidOperationException("Cannot parse switch statement with >2^31 targets");
							int[] targets = new int[count];
							for(int i = 0; i < count; i++) targets[i] = NextI4();
							for (int i = 0; i < count; i++) targets[i] += ip2;
							visitor.VisitSwitch(ip, targets);
						}
						break;
					case OpCodeValue.Ldind_I1:
						break;
					case OpCodeValue.Ldind_U1:
						break;
					case OpCodeValue.Ldind_I2:
						break;
					case OpCodeValue.Ldind_U2:
						break;
					case OpCodeValue.Ldind_I4:
						break;
					case OpCodeValue.Ldind_U4:
						break;
					case OpCodeValue.Ldind_I8:
						break;
					case OpCodeValue.Ldind_I:
						break;
					case OpCodeValue.Ldind_R4:
						break;
					case OpCodeValue.Ldind_R8:
						break;
					case OpCodeValue.Ldind_Ref:
						break;
					case OpCodeValue.Stind_Ref:
						break;
					case OpCodeValue.Stind_I1:
						break;
					case OpCodeValue.Stind_I2:
						break;
					case OpCodeValue.Stind_I4:
						break;
					case OpCodeValue.Stind_I8:
						break;
					case OpCodeValue.Stind_R4:
						break;
					case OpCodeValue.Stind_R8:
						break;
					case OpCodeValue.Add:
						break;
					case OpCodeValue.Sub:
						break;
					case OpCodeValue.Mul:
						break;
					case OpCodeValue.Div:
						break;
					case OpCodeValue.Div_Un:
						break;
					case OpCodeValue.Rem:
						break;
					case OpCodeValue.Rem_Un:
						break;
					case OpCodeValue.And:
						break;
					case OpCodeValue.Or:
						break;
					case OpCodeValue.Xor:
						break;
					case OpCodeValue.Shl:
						break;
					case OpCodeValue.Shr:
						break;
					case OpCodeValue.Shr_Un:
						break;
					case OpCodeValue.Neg:
						break;
					case OpCodeValue.Not:
						break;
					case OpCodeValue.Conv_I1:
						break;
					case OpCodeValue.Conv_I2:
						break;
					case OpCodeValue.Conv_I4:
						break;
					case OpCodeValue.Conv_I8:
						break;
					case OpCodeValue.Conv_R4:
						break;
					case OpCodeValue.Conv_R8:
						break;
					case OpCodeValue.Conv_U4:
						break;
					case OpCodeValue.Conv_U8:
						break;
					case OpCodeValue.Callvirt:
						break;
					case OpCodeValue.Cpobj:
						break;
					case OpCodeValue.Ldobj:
						break;
					case OpCodeValue.Ldstr:
						break;
					case OpCodeValue.Newobj:
						break;
					case OpCodeValue.Castclass:
						break;
					case OpCodeValue.Isinst:
						break;
					case OpCodeValue.Conv_R_Un:
						break;
					case OpCodeValue.Unbox:
						break;
					case OpCodeValue.Throw:
						break;
					case OpCodeValue.Ldfld:
						break;
					case OpCodeValue.Ldflda:
						break;
					case OpCodeValue.Stfld:
						break;
					case OpCodeValue.Ldsfld:
						break;
					case OpCodeValue.Ldsflda:
						break;
					case OpCodeValue.Stsfld:
						break;
					case OpCodeValue.Stobj:
						break;
					case OpCodeValue.Conv_Ovf_I1_Un:
						break;
					case OpCodeValue.Conv_Ovf_I2_Un:
						break;
					case OpCodeValue.Conv_Ovf_I4_Un:
						break;
					case OpCodeValue.Conv_Ovf_I8_Un:
						break;
					case OpCodeValue.Conv_Ovf_U1_Un:
						break;
					case OpCodeValue.Conv_Ovf_U2_Un:
						break;
					case OpCodeValue.Conv_Ovf_U4_Un:
						break;
					case OpCodeValue.Conv_Ovf_U8_Un:
						break;
					case OpCodeValue.Conv_Ovf_I_Un:
						break;
					case OpCodeValue.Conv_Ovf_U_Un:
						break;
					case OpCodeValue.Box:
						break;
					case OpCodeValue.Newarr:
						break;
					case OpCodeValue.Ldlen:
						break;
					case OpCodeValue.Ldelema:
						break;
					case OpCodeValue.Ldelem_I1:
						break;
					case OpCodeValue.Ldelem_U1:
						break;
					case OpCodeValue.Ldelem_I2:
						break;
					case OpCodeValue.Ldelem_U2:
						break;
					case OpCodeValue.Ldelem_I4:
						break;
					case OpCodeValue.Ldelem_U4:
						break;
					case OpCodeValue.Ldelem_I8:
						break;
					case OpCodeValue.Ldelem_I:
						break;
					case OpCodeValue.Ldelem_R4:
						break;
					case OpCodeValue.Ldelem_R8:
						break;
					case OpCodeValue.Ldelem_Ref:
						break;
					case OpCodeValue.Stelem_I:
						break;
					case OpCodeValue.Stelem_I1:
						break;
					case OpCodeValue.Stelem_I2:
						break;
					case OpCodeValue.Stelem_I4:
						break;
					case OpCodeValue.Stelem_I8:
						break;
					case OpCodeValue.Stelem_R4:
						break;
					case OpCodeValue.Stelem_R8:
						break;
					case OpCodeValue.Stelem_Ref:
						break;
					case OpCodeValue.Ldelem:
						break;
					case OpCodeValue.Stelem:
						break;
					case OpCodeValue.Unbox_Any:
						break;
					case OpCodeValue.Conv_Ovf_I1:
						break;
					case OpCodeValue.Conv_Ovf_U1:
						break;
					case OpCodeValue.Conv_Ovf_I2:
						break;
					case OpCodeValue.Conv_Ovf_U2:
						break;
					case OpCodeValue.Conv_Ovf_I4:
						break;
					case OpCodeValue.Conv_Ovf_U4:
						break;
					case OpCodeValue.Conv_Ovf_I8:
						break;
					case OpCodeValue.Conv_Ovf_U8:
						break;
					case OpCodeValue.Refanyval:
						break;
					case OpCodeValue.Ckfinite:
						break;
					case OpCodeValue.Mkrefany:
						break;
					case OpCodeValue.Ldtoken:
						break;
					case OpCodeValue.Conv_U2:
						break;
					case OpCodeValue.Conv_U1:
						break;
					case OpCodeValue.Conv_I:
						break;
					case OpCodeValue.Conv_Ovf_I:
						break;
					case OpCodeValue.Conv_Ovf_U:
						break;
					case OpCodeValue.Add_Ovf:
						break;
					case OpCodeValue.Add_Ovf_Un:
						break;
					case OpCodeValue.Mul_Ovf:
						break;
					case OpCodeValue.Mul_Ovf_Un:
						break;
					case OpCodeValue.Sub_Ovf:
						break;
					case OpCodeValue.Sub_Ovf_Un:
						break;
					case OpCodeValue.Endfinally:
						break;
					case OpCodeValue.Leave:
						break;
					case OpCodeValue.Leave_S:
						break;
					case OpCodeValue.Stind_I:
						break;
					case OpCodeValue.Conv_U:
						break;
					case OpCodeValue.Arglist:
						break;
					case OpCodeValue.Ceq:
						break;
					case OpCodeValue.Cgt:
						break;
					case OpCodeValue.Cgt_Un:
						break;
					case OpCodeValue.Clt:
						break;
					case OpCodeValue.Clt_Un:
						break;
					case OpCodeValue.Ldftn:
						break;
					case OpCodeValue.Ldvirtfn:
						break;
					case OpCodeValue.Ldarg:
						break;
					case OpCodeValue.Ldarga:
						break;
					case OpCodeValue.Starg:
						break;
					case OpCodeValue.Ldloc:
						break;
					case OpCodeValue.Ldloca:
						break;
					case OpCodeValue.Stloc:
						break;
					case OpCodeValue.Localloc:
						break;
					case OpCodeValue.Endfilter:
						break;
					case OpCodeValue.Unaligned:
						break;
					case OpCodeValue.Volatile:
						break;
					case OpCodeValue.Tail:
						break;
					case OpCodeValue.Initobj:
						break;
					case OpCodeValue.Constrained:
						break;
					case OpCodeValue.Cpblock:
						break;
					case OpCodeValue.Initblk:
						break;
					case OpCodeValue.Rethrow:
						break;
					case OpCodeValue.Sizeof:
						break;
					case OpCodeValue.Refanytype:
						visitor.Visit(ip, OpCodes.Refanytype);
						break;
					case OpCodeValue.Readonly:
						visitor.Visit(ip, OpCodes.Readonly);
						break;
					default:
						throw new InvalidOperationException($"Unexpected opcode 0x{(int)op:X}");
				}
				ip = ip2;
			}
		}

	}

}
