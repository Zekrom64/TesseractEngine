using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Reflection {

	/// <summary>
	/// A method visitor is an object which visits the IL code of a method instruction by instruction.
	/// </summary>
	public interface IMethodVisitor {

		/// <summary>
		/// Visits an instruction with no operands.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		public void Visit(int ip, OpCode opcode);

		/// <summary>
		/// Visits a branching instruction with a target address.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="target">The target adress of the branch</param>
		public void VisitBranch(int ip, OpCode opcode, int target);

		/// <summary>
		/// Visits an instruction referencing a type.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="type"></param>
		public void VisitWithType(int ip, OpCode opcode, Type type);

		/// <summary>
		/// Visits an instruction referencing a method which may be a member function or constructor.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="method">The base method being visited</param>
		public void VisitWithMethodBase(int ip, OpCode opcode, MethodBase method) {
			if (method is MethodInfo mi) VisitWithMethodInfo(ip, opcode, mi);
			else if (method is ConstructorInfo ci) VisitWithConstructorInfo(ip, opcode, ci);
		}

		/// <summary>
		/// Visits an instruction referencing a static or instance method.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="methodInfo">The method being visited</param>
		public void VisitWithMethodInfo(int ip, OpCode opcode, MethodInfo methodInfo);

		/// <summary>
		/// Visits an instruction referencing a constructor method.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="constructorInfo">The constructor being visited</param>
		public void VisitWithConstructorInfo(int ip, OpCode opcode, ConstructorInfo constructorInfo);

		/// <summary>
		/// Visits an instruction referencing a property.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="propertyInfo">The property being visited</param>
		public void VisitWithPropertyInfo(int ip, OpCode opcode, PropertyInfo propertyInfo);

		/// <summary>
		/// Visits an instruction referencing a field.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="fieldInfo">The field being visited</param>
		public void VisitWithFieldInfo(int ip, OpCode opcode, FieldInfo fieldInfo);

		/// <summary>
		/// Visits an instruction referencing an event.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="eventInfo">The event being visited</param>
		public void VisitWithEventInfo(int ip, OpCode opcode, EventInfo eventInfo);

		/// <summary>
		/// Visits an instruction referencing a value using an associated index value.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="index">The index associated with the instruction</param>
		public void VisitWithIndex(int ip, OpCode opcode, ushort index);

		/// <summary>
		/// Vists an instruction referencing an immediate value.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="opcode">The opcode of the instruction</param>
		/// <param name="value">The immediate value associated with the instruction</param>
		public void VisitWithImmediate(int ip, OpCode opcode, decimal value);

		/// <summary>
		/// Visits a <see cref="OpCodes.Ldstr"/> instruction with the given value.
		/// </summary>
		/// <param name="ip">Offset of the isntruction</param>
		/// <param name="value">String value associated with instruction</param>
		public void VisitLdstr(int ip, string value);

		/// <summary>
		/// Visits a <see cref="OpCodes.Switch"/> instruction with the given list of targets.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="targets">List of switch target addresses</param>
		public void VisitSwitch(int ip, int[] targets);

		/// <summary>
		/// Visits a <see cref="OpCodes.Calli"/> instruction with the given signature.
		/// </summary>
		/// <param name="ip">Offset of the instruction</param>
		/// <param name="signature"></param>
		public void VisitCalli(int ip, Signature signature);

	}

	/// <summary>
	/// Base class implementation of <see cref="IMethodVisitor"/> with no-op virtual methods.
	/// </summary>
	public class MethodVisitor : IMethodVisitor {

		public virtual void Visit(int ip, OpCode opcode) { }

		public virtual void VisitBranch(int ip, OpCode opcode, int target) { }

		public virtual void VisitCalli(int ip, Signature signature) { }

		public virtual void VisitWithImmediate(int ip, OpCode opcode, decimal value) { }

		public virtual void VisitLdstr(int ip, string value) { }

		public virtual void VisitSwitch(int ip, int[] targets) { }

		public virtual void VisitWithConstructorInfo(int ip, OpCode opcode, ConstructorInfo constructorInfo) { }

		public virtual void VisitWithEventInfo(int ip, OpCode opcode, EventInfo eventInfo) { }

		public virtual void VisitWithFieldInfo(int ip, OpCode opcode, FieldInfo fieldInfo) { }

		public virtual void VisitWithIndex(int ip, OpCode opcode, ushort index) { }

		public virtual void VisitWithMethodInfo(int ip, OpCode opcode, MethodInfo methodInfo) { }

		public virtual void VisitWithPropertyInfo(int ip, OpCode opcode, PropertyInfo propertyInfo) { }

		public virtual void VisitWithType(int ip, OpCode opcode, Type type) { }

	}

}
