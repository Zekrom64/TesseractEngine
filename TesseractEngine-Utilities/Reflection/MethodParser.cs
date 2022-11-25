using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Tesseract.Core.Utilities;

namespace Tesseract.Reflection {

	/// <summary>
	/// A method parser can parse the IL of a method, generating calls to a visitor
	/// for every instruction parsed.
	/// </summary>
	public class MethodParser {

		/// <summary>
		/// The method the parser targets.
		/// </summary>
		public MethodBase Method { get; }

		/// <summary>
		/// The body of the method.
		/// </summary>
		public MethodBody Body { get; }

		private readonly byte[] bytecode;

		/// <summary>
		/// The raw bytecode of the method.
		/// </summary>
		public IReadOnlyList<byte> Bytecode => bytecode;

		/// <summary>
		/// Creates a method parser that will parse the specified method.
		/// </summary>
		/// <param name="method">The method to parse</param>
		/// <exception cref="ArgumentException">If the specified method cannot be parsed</exception>
		/// <exception cref="InvalidOperationException">If an error occurs attempting to set up the method for parsing</exception>
		public MethodParser(MethodBase method) {
			Method = method;
			Body = method.GetMethodBody() ?? throw new ArgumentException("Cannot parse abstract method", nameof(method));
			bytecode = Body.GetILAsByteArray() ?? throw new InvalidOperationException("Could not get method IL");
		}

		/// <summary>
		/// Visits the target method using the given visitor.
		/// </summary>
		/// <param name="visitor">Visitor which will visit the method</param>
		/// <exception cref="InvalidOperationException">If an error occurs parsing the method's IL code</exception>
		public void Visit(IMethodVisitor visitor) {
			Module module = Method.Module;
			// ip - Pointer to current instruction, ip2 - Pointer within current instruction
			int ip = 0, ip2 = ip;

			// Fetches the next opcode
			OpCodeValue NextOp() {
				int op = bytecode[ip2++];
				if (op >= 0xF8) {
					op <<= 8;
					op |= bytecode[ip2++];
				}
				return (OpCodeValue)op;
			}

			// Fetches the next byte
			byte NextU1() => bytecode[ip2++];

			// Fetches the next ushort
			ushort NextU2() {
				ushort u = bytecode[ip2++];
				u |= (ushort)(bytecode[ip2++] << 8);
				return u;
			}

			// Fetches the next int
			int NextI4() {
				int i = bytecode[ip2++];
				i |= bytecode[ip2++] << 8;
				i |= bytecode[ip2++] << 16;
				i |= bytecode[ip2++] << 24;
				return i;
			}

			// Fetches the next long
			long NextI8() {
				long l = NextI4();
				l |= (long)NextI4() << 32;
				return l;
			}

			// Fetches the next type
			Type NextType() => module.ResolveType(NextI4());
			// Fetches the next method
			MethodBase NextMethod() => module.ResolveMethod(NextI4())!;
			// Fetches the next field
			FieldInfo NextField() => module.ResolveField(NextI4())!;

			// While the instruction pointer is below the length keep parsing opcodes
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
						visitor.VisitWithImmediate(ip, OpCodes.Ldc_I4_S, (sbyte)NextU1());
						break;
					case OpCodeValue.Ldc_I4:
						visitor.VisitWithImmediate(ip, OpCodes.Ldc_I4, NextI4());
						break;
					case OpCodeValue.Ldc_I8:
						visitor.VisitWithImmediate(ip, OpCodes.Ldc_I8, NextI8());
						break;
					case OpCodeValue.Ldc_R4:
						visitor.VisitWithImmediate(ip, OpCodes.Ldc_R4, (decimal)BitConverter.Int32BitsToSingle(NextI4()));
						break;
					case OpCodeValue.Ldc_R8:
						visitor.VisitWithImmediate(ip, OpCodes.Ldc_R4, (decimal)BitConverter.Int64BitsToDouble(NextI8()));
						break;
					case OpCodeValue.Dup:
						visitor.Visit(ip, OpCodes.Dup);
						break;
					case OpCodeValue.Pop:
						visitor.Visit(ip, OpCodes.Pop);
						break;
					case OpCodeValue.Jmp:
						visitor.VisitWithMethodBase(ip, OpCodes.Jmp, NextMethod());
						break;
					case OpCodeValue.Call:
						visitor.VisitWithMethodBase(ip, OpCodes.Call, NextMethod());
						break;
					case OpCodeValue.Calli:
						visitor.VisitCalli(ip, Signature.Parse(module, module.ResolveSignature(NextI4())));
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
						visitor.Visit(ip, OpCodes.Ldind_I1);
						break;
					case OpCodeValue.Ldind_U1:
						visitor.Visit(ip, OpCodes.Ldind_U1);
						break;
					case OpCodeValue.Ldind_I2:
						visitor.Visit(ip, OpCodes.Ldind_I2);
						break;
					case OpCodeValue.Ldind_U2:
						visitor.Visit(ip, OpCodes.Ldind_U2);
						break;
					case OpCodeValue.Ldind_I4:
						visitor.Visit(ip, OpCodes.Ldind_I4);
						break;
					case OpCodeValue.Ldind_U4:
						visitor.Visit(ip, OpCodes.Ldind_U4);
						break;
					case OpCodeValue.Ldind_I8:
						visitor.Visit(ip, OpCodes.Ldind_I8);
						break;
					case OpCodeValue.Ldind_I:
						visitor.Visit(ip, OpCodes.Ldind_I);
						break;
					case OpCodeValue.Ldind_R4:
						visitor.Visit(ip, OpCodes.Ldind_R4);
						break;
					case OpCodeValue.Ldind_R8:
						visitor.Visit(ip, OpCodes.Ldind_R8);
						break;
					case OpCodeValue.Ldind_Ref:
						visitor.Visit(ip, OpCodes.Ldind_Ref);
						break;
					case OpCodeValue.Stind_Ref:
						visitor.Visit(ip, OpCodes.Stind_Ref);
						break;
					case OpCodeValue.Stind_I1:
						visitor.Visit(ip, OpCodes.Stind_I1);
						break;
					case OpCodeValue.Stind_I2:
						visitor.Visit(ip, OpCodes.Stind_I2);
						break;
					case OpCodeValue.Stind_I4:
						visitor.Visit(ip, OpCodes.Stind_I4);
						break;
					case OpCodeValue.Stind_I8:
						visitor.Visit(ip, OpCodes.Stind_I8);
						break;
					case OpCodeValue.Stind_R4:
						visitor.Visit(ip, OpCodes.Stind_R4);
						break;
					case OpCodeValue.Stind_R8:
						visitor.Visit(ip, OpCodes.Stind_R8);
						break;
					case OpCodeValue.Add:
						visitor.Visit(ip, OpCodes.Add);
						break;
					case OpCodeValue.Sub:
						visitor.Visit(ip, OpCodes.Sub);
						break;
					case OpCodeValue.Mul:
						visitor.Visit(ip, OpCodes.Mul);
						break;
					case OpCodeValue.Div:
						visitor.Visit(ip, OpCodes.Div);
						break;
					case OpCodeValue.Div_Un:
						visitor.Visit(ip, OpCodes.Div_Un);
						break;
					case OpCodeValue.Rem:
						visitor.Visit(ip, OpCodes.Rem);
						break;
					case OpCodeValue.Rem_Un:
						visitor.Visit(ip, OpCodes.Rem_Un);
						break;
					case OpCodeValue.And:
						visitor.Visit(ip, OpCodes.And);
						break;
					case OpCodeValue.Or:
						visitor.Visit(ip, OpCodes.Or);
						break;
					case OpCodeValue.Xor:
						visitor.Visit(ip, OpCodes.Xor);
						break;
					case OpCodeValue.Shl:
						visitor.Visit(ip, OpCodes.Shl);
						break;
					case OpCodeValue.Shr:
						visitor.Visit(ip, OpCodes.Shr);
						break;
					case OpCodeValue.Shr_Un:
						visitor.Visit(ip, OpCodes.Shr_Un);
						break;
					case OpCodeValue.Neg:
						visitor.Visit(ip, OpCodes.Neg);
						break;
					case OpCodeValue.Not:
						visitor.Visit(ip, OpCodes.Not);
						break;
					case OpCodeValue.Conv_I1:
						visitor.Visit(ip, OpCodes.Conv_I1);
						break;
					case OpCodeValue.Conv_I2:
						visitor.Visit(ip, OpCodes.Conv_I2);
						break;
					case OpCodeValue.Conv_I4:
						visitor.Visit(ip, OpCodes.Conv_I4);
						break;
					case OpCodeValue.Conv_I8:
						visitor.Visit(ip, OpCodes.Conv_I8);
						break;
					case OpCodeValue.Conv_R4:
						visitor.Visit(ip, OpCodes.Conv_R4);
						break;
					case OpCodeValue.Conv_R8:
						visitor.Visit(ip, OpCodes.Conv_R8);
						break;
					case OpCodeValue.Conv_U4:
						visitor.Visit(ip, OpCodes.Conv_U4);
						break;
					case OpCodeValue.Conv_U8:
						visitor.Visit(ip, OpCodes.Conv_U8);
						break;
					case OpCodeValue.Callvirt:
						visitor.VisitWithMethodBase(ip, OpCodes.Callvirt, NextMethod());
						break;
					case OpCodeValue.Cpobj:
						visitor.VisitWithType(ip, OpCodes.Cpobj, NextType());
						break;
					case OpCodeValue.Ldobj:
						visitor.VisitWithType(ip, OpCodes.Ldobj, NextType());
						break;
					case OpCodeValue.Ldstr:
						visitor.VisitLdstr(ip, module.ResolveString(NextI4()));
						break;
					case OpCodeValue.Newobj:
						visitor.VisitWithMethodBase(ip, OpCodes.Newobj, NextMethod());
						break;
					case OpCodeValue.Castclass:
						visitor.VisitWithType(ip, OpCodes.Castclass, NextType());
						break;
					case OpCodeValue.Isinst:
						visitor.VisitWithType(ip, OpCodes.Isinst, NextType());
						break;
					case OpCodeValue.Conv_R_Un:
						visitor.Visit(ip, OpCodes.Conv_R_Un);
						break;
					case OpCodeValue.Unbox:
						visitor.VisitWithType(ip, OpCodes.Unbox, NextType());
						break;
					case OpCodeValue.Throw:
						visitor.Visit(ip, OpCodes.Throw);
						break;
					case OpCodeValue.Ldfld:
						visitor.VisitWithFieldInfo(ip, OpCodes.Ldfld, NextField());
						break;
					case OpCodeValue.Ldflda:
						visitor.VisitWithFieldInfo(ip, OpCodes.Ldflda, NextField());
						break;
					case OpCodeValue.Stfld:
						visitor.VisitWithFieldInfo(ip, OpCodes.Stfld, NextField());
						break;
					case OpCodeValue.Ldsfld:
						visitor.VisitWithFieldInfo(ip, OpCodes.Ldsfld, NextField());
						break;
					case OpCodeValue.Ldsflda:
						visitor.VisitWithFieldInfo(ip, OpCodes.Ldsflda, NextField());
						break;
					case OpCodeValue.Stsfld:
						visitor.VisitWithFieldInfo(ip, OpCodes.Stsfld, NextField());
						break;
					case OpCodeValue.Stobj:
						visitor.VisitWithType(ip, OpCodes.Stobj, NextType());
						break;
					case OpCodeValue.Conv_Ovf_I1_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I1_Un);
						break;
					case OpCodeValue.Conv_Ovf_I2_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I2_Un);
						break;
					case OpCodeValue.Conv_Ovf_I4_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I4_Un);
						break;
					case OpCodeValue.Conv_Ovf_I8_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I8_Un);
						break;
					case OpCodeValue.Conv_Ovf_U1_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U1_Un);
						break;
					case OpCodeValue.Conv_Ovf_U2_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U2_Un);
						break;
					case OpCodeValue.Conv_Ovf_U4_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U4_Un);
						break;
					case OpCodeValue.Conv_Ovf_U8_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U8_Un);
						break;
					case OpCodeValue.Conv_Ovf_I_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I_Un);
						break;
					case OpCodeValue.Conv_Ovf_U_Un:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U_Un);
						break;
					case OpCodeValue.Box:
						visitor.VisitWithType(ip, OpCodes.Box, NextType());
						break;
					case OpCodeValue.Newarr:
						visitor.VisitWithType(ip, OpCodes.Newarr, NextType());
						break;
					case OpCodeValue.Ldlen:
						visitor.Visit(ip, OpCodes.Ldlen);
						break;
					case OpCodeValue.Ldelema:
						visitor.VisitWithType(ip, OpCodes.Ldelema, NextType());
						break;
					case OpCodeValue.Ldelem_I1:
						visitor.Visit(ip, OpCodes.Ldelem_I1);
						break;
					case OpCodeValue.Ldelem_U1:
						visitor.Visit(ip, OpCodes.Ldelem_U1);
						break;
					case OpCodeValue.Ldelem_I2:
						visitor.Visit(ip, OpCodes.Ldelem_I2);
						break;
					case OpCodeValue.Ldelem_U2:
						visitor.Visit(ip, OpCodes.Ldelem_U2);
						break;
					case OpCodeValue.Ldelem_I4:
						visitor.Visit(ip, OpCodes.Ldelem_I4);
						break;
					case OpCodeValue.Ldelem_U4:
						visitor.Visit(ip, OpCodes.Ldelem_U4);
						break;
					case OpCodeValue.Ldelem_I8:
						visitor.Visit(ip, OpCodes.Ldelem_I8);
						break;
					case OpCodeValue.Ldelem_I:
						visitor.Visit(ip, OpCodes.Ldelem_I);
						break;
					case OpCodeValue.Ldelem_R4:
						visitor.Visit(ip, OpCodes.Ldelem_R4);
						break;
					case OpCodeValue.Ldelem_R8:
						visitor.Visit(ip, OpCodes.Ldelem_R8);
						break;
					case OpCodeValue.Ldelem_Ref:
						visitor.Visit(ip, OpCodes.Ldelem_Ref);
						break;
					case OpCodeValue.Stelem_I:
						visitor.Visit(ip, OpCodes.Stelem_I);
						break;
					case OpCodeValue.Stelem_I1:
						visitor.Visit(ip, OpCodes.Stelem_I1);
						break;
					case OpCodeValue.Stelem_I2:
						visitor.Visit(ip, OpCodes.Stelem_I2);
						break;
					case OpCodeValue.Stelem_I4:
						visitor.Visit(ip, OpCodes.Stelem_I4);
						break;
					case OpCodeValue.Stelem_I8:
						visitor.Visit(ip, OpCodes.Stelem_I8);
						break;
					case OpCodeValue.Stelem_R4:
						visitor.Visit(ip, OpCodes.Stelem_R4);
						break;
					case OpCodeValue.Stelem_R8:
						visitor.Visit(ip, OpCodes.Stelem_R8);
						break;
					case OpCodeValue.Stelem_Ref:
						visitor.Visit(ip, OpCodes.Stelem_Ref);
						break;
					case OpCodeValue.Ldelem:
						visitor.VisitWithType(ip, OpCodes.Ldelem, NextType());
						break;
					case OpCodeValue.Stelem:
						visitor.VisitWithType(ip, OpCodes.Stelem, NextType());
						break;
					case OpCodeValue.Unbox_Any:
						visitor.VisitWithType(ip, OpCodes.Unbox_Any, NextType());
						break;
					case OpCodeValue.Conv_Ovf_I1:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I1);
						break;
					case OpCodeValue.Conv_Ovf_U1:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U1);
						break;
					case OpCodeValue.Conv_Ovf_I2:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I2);
						break;
					case OpCodeValue.Conv_Ovf_U2:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U2);
						break;
					case OpCodeValue.Conv_Ovf_I4:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I4);
						break;
					case OpCodeValue.Conv_Ovf_U4:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U4);
						break;
					case OpCodeValue.Conv_Ovf_I8:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I8);
						break;
					case OpCodeValue.Conv_Ovf_U8:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U8);
						break;
					case OpCodeValue.Refanyval:
						visitor.Visit(ip, OpCodes.Refanyval);
						break;
					case OpCodeValue.Ckfinite:
						visitor.Visit(ip, OpCodes.Ckfinite);
						break;
					case OpCodeValue.Mkrefany:
						visitor.VisitWithType(ip, OpCodes.Mkrefany, NextType());
						break;
					case OpCodeValue.Ldtoken:
						visitor.VisitWithImmediate(ip, OpCodes.Ldtoken, NextI4());
						break;
					case OpCodeValue.Conv_U2:
						visitor.Visit(ip, OpCodes.Conv_U2);
						break;
					case OpCodeValue.Conv_U1:
						visitor.Visit(ip, OpCodes.Conv_U1);
						break;
					case OpCodeValue.Conv_I:
						visitor.Visit(ip, OpCodes.Conv_I);
						break;
					case OpCodeValue.Conv_Ovf_I:
						visitor.Visit(ip, OpCodes.Conv_Ovf_I);
						break;
					case OpCodeValue.Conv_Ovf_U:
						visitor.Visit(ip, OpCodes.Conv_Ovf_U);
						break;
					case OpCodeValue.Add_Ovf:
						visitor.Visit(ip, OpCodes.Add_Ovf);
						break;
					case OpCodeValue.Add_Ovf_Un:
						visitor.Visit(ip, OpCodes.Add_Ovf_Un);
						break;
					case OpCodeValue.Mul_Ovf:
						visitor.Visit(ip, OpCodes.Mul_Ovf);
						break;
					case OpCodeValue.Mul_Ovf_Un:
						visitor.Visit(ip, OpCodes.Mul_Ovf_Un);
						break;
					case OpCodeValue.Sub_Ovf:
						visitor.Visit(ip, OpCodes.Sub_Ovf);
						break;
					case OpCodeValue.Sub_Ovf_Un:
						visitor.Visit(ip, OpCodes.Sub_Ovf_Un);
						break;
					case OpCodeValue.Endfinally:
						visitor.Visit(ip, OpCodes.Endfinally);
						break;
					case OpCodeValue.Leave:
						visitor.VisitBranch(ip, OpCodes.Leave, NextI4() + ip2);
						break;
					case OpCodeValue.Leave_S:
						visitor.VisitBranch(ip, OpCodes.Leave_S, (sbyte)NextU1() + ip2);
						break;
					case OpCodeValue.Stind_I:
						visitor.Visit(ip, OpCodes.Stind_I);
						break;
					case OpCodeValue.Conv_U:
						visitor.Visit(ip, OpCodes.Conv_U);
						break;
					case OpCodeValue.Arglist:
						visitor.Visit(ip, OpCodes.Arglist);
						break;
					case OpCodeValue.Ceq:
						visitor.Visit(ip, OpCodes.Ceq);
						break;
					case OpCodeValue.Cgt:
						visitor.Visit(ip, OpCodes.Cgt);
						break;
					case OpCodeValue.Cgt_Un:
						visitor.Visit(ip, OpCodes.Cgt_Un);
						break;
					case OpCodeValue.Clt:
						visitor.Visit(ip, OpCodes.Clt);
						break;
					case OpCodeValue.Clt_Un:
						visitor.Visit(ip, OpCodes.Clt_Un);
						break;
					case OpCodeValue.Ldftn:
						visitor.VisitWithMethodBase(ip, OpCodes.Ldftn, NextMethod());
						break;
					case OpCodeValue.Ldvirtfn:
						visitor.VisitWithMethodBase(ip, OpCodes.Ldvirtftn, NextMethod());
						break;
					case OpCodeValue.Ldarg:
						visitor.VisitWithIndex(ip, OpCodes.Ldarg, NextU2());
						break;
					case OpCodeValue.Ldarga:
						visitor.VisitWithIndex(ip, OpCodes.Ldarga, NextU2());
						break;
					case OpCodeValue.Starg:
						visitor.VisitWithIndex(ip, OpCodes.Starg, NextU2());
						break;
					case OpCodeValue.Ldloc:
						visitor.VisitWithIndex(ip, OpCodes.Ldloc, NextU2());
						break;
					case OpCodeValue.Ldloca:
						visitor.VisitWithIndex(ip, OpCodes.Ldloca, NextU2());
						break;
					case OpCodeValue.Stloc:
						visitor.VisitWithIndex(ip, OpCodes.Stloc, NextU2());
						break;
					case OpCodeValue.Localloc:
						visitor.Visit(ip, OpCodes.Localloc);
						break;
					case OpCodeValue.Endfilter:
						visitor.Visit(ip, OpCodes.Endfilter);
						break;
					case OpCodeValue.Unaligned:
						visitor.VisitWithImmediate(ip, OpCodes.Unaligned, NextU1());
						break;
					case OpCodeValue.Volatile:
						visitor.Visit(ip, OpCodes.Volatile);
						break;
					case OpCodeValue.Tailcall:
						visitor.Visit(ip, OpCodes.Tailcall);
						break;
					case OpCodeValue.Initobj:
						visitor.VisitWithType(ip, OpCodes.Initobj, NextType());
						break;
					case OpCodeValue.Constrained:
						visitor.VisitWithType(ip, OpCodes.Constrained, NextType());
						break;
					case OpCodeValue.Cpblk:
						visitor.Visit(ip, OpCodes.Cpblk);
						break;
					case OpCodeValue.Initblk:
						visitor.Visit(ip, OpCodes.Initblk);
						break;
					case OpCodeValue.Rethrow:
						visitor.Visit(ip, OpCodes.Rethrow);
						break;
					case OpCodeValue.Sizeof:
						visitor.VisitWithType(ip, OpCodes.Sizeof, NextType());
						break;
					case OpCodeValue.Refanytype:
						visitor.Visit(ip, OpCodes.Refanytype);
						break;
					case OpCodeValue.Readonly:
						visitor.Visit(ip, OpCodes.Readonly);
						break;
					default:
						throw new InvalidOperationException($"Unexpected opcode 0x{(int)op:X}@0x{ip:X}");
				}

				// Update the current instruction pointer
				ip = ip2;
			}
		}

	}

}
