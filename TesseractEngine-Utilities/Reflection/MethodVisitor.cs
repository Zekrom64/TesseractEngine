using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Reflection {

	public interface IMethodVisitor {

		public void Visit(int ip, OpCode opcode);

		public void VisitBranch(int ip, OpCode opcode, int target);

		public void VisitWithType(int ip, OpCode opcode, Type type);

		public void VisitWithMethodBase(int ip, OpCode opcode, MethodBase method) {
			if (method is MethodInfo mi) VisitWithMethodInfo(ip, opcode, mi);
			else if (method is ConstructorInfo ci) VisitWithConstructorInfo(ip, opcode, ci);
		}

		public void VisitWithMethodInfo(int ip, OpCode opcode, MethodInfo methodInfo);

		public void VisitWithConstructorInfo(int ip, OpCode opcode, ConstructorInfo constructorInfo);

		public void VisitWithPropertyInfo(int ip, OpCode opcode, PropertyInfo propertyInfo);

		public void VisitWithFieldInfo(int ip, OpCode opcode, FieldInfo fieldInfo);

		public void VisitWithEventInfo(int ip, OpCode opcode, EventInfo eventInfo);

		public void VisitWithIndex(int ip, OpCode opcode, ushort index);

		public void VisitLdcWithImmediate(int ip, OpCode opcode, decimal value);

		public void VisitSwitch(int ip, int[] targets);

		public void VisitCalli(int ip, Signature signature);

	}

	public class MethodVisitor : IMethodVisitor {

		public void Visit(int ip, OpCode opcode) { }

		public void VisitBranch(int ip, OpCode opcode, int target) { }

		public void VisitCalli(int ip, Signature signature) { }

		public void VisitLdcWithImmediate(int ip, OpCode opcode, decimal value) { }

		public void VisitSwitch(int ip, int[] targets) { }

		public void VisitWithConstructorInfo(int ip, OpCode opcode, ConstructorInfo constructorInfo) { }

		public void VisitWithEventInfo(int ip, OpCode opcode, EventInfo eventInfo) { }

		public void VisitWithFieldInfo(int ip, OpCode opcode, FieldInfo fieldInfo) { }

		public void VisitWithIndex(int ip, OpCode opcode, ushort index) { }

		public void VisitWithMethodInfo(int ip, OpCode opcode, MethodInfo methodInfo) { }

		public void VisitWithPropertyInfo(int ip, OpCode opcode, PropertyInfo propertyInfo) { }

		public void VisitWithType(int ip, OpCode opcode, Type type) { }

	}

}
