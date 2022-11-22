using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Tesseract.Reflection {
	internal class Test {

		public static void Main(string[] args) {
			MethodParser parser = new MethodParser(typeof(Test).GetMethod("TestFunc")!);
			parser.Visit(new Visitor());
		}

		public static unsafe void TestFunc() {
			delegate*<int,void> fn = null;
			fn(1);
		}

		private class Visitor : IMethodVisitor {

			public void Visit(int ip, OpCode opcode) {
				Console.WriteLine($"0x{ip:X8}: {opcode}");
			}

			public void VisitBranch(int ip, OpCode opcode, int target) {
				throw new NotImplementedException();
			}

			public void VisitCalli(int ip, Signature signature) {
				Console.WriteLine($"0x{ip:X8}: calli {signature}");
			}

			public void VisitLdcWithImmediate(int ip, OpCode opcode, decimal value) {
				throw new NotImplementedException();
			}

			public void VisitSwitch(int ip, int[] targets) {
				throw new NotImplementedException();
			}

			public void VisitWithConstructorInfo(int ip, OpCode opcode, ConstructorInfo constructorInfo) {
				throw new NotImplementedException();
			}

			public void VisitWithEventInfo(int ip, OpCode opcode, EventInfo eventInfo) {
				throw new NotImplementedException();
			}

			public void VisitWithFieldInfo(int ip, OpCode opcode, FieldInfo fieldInfo) {
				throw new NotImplementedException();
			}

			public void VisitWithIndex(int ip, OpCode opcode, ushort index) {
				throw new NotImplementedException();
			}

			public void VisitWithMethodInfo(int ip, OpCode opcode, MethodInfo methodInfo) {
				throw new NotImplementedException();
			}

			public void VisitWithPropertyInfo(int ip, OpCode opcode, PropertyInfo propertyInfo) {
				throw new NotImplementedException();
			}

			public void VisitWithType(int ip, OpCode opcode, Type type) {
				throw new NotImplementedException();
			}

		}

	}
}
