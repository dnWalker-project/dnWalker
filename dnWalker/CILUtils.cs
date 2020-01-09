/*
 *   Copyright 2007 University of Twente, Formal Methods and Tools group
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 */

namespace MMC.Util {

	using System.Diagnostics;
	using Mono.Cecil;
	using Mono.Cecil.Cil;

	struct CILLocation : System.IComparable {

		Mono.Cecil.Cil.Instruction m_instr;
		Mono.Cecil.MethodDefinition m_meth;

		public Mono.Cecil.Cil.Instruction Instruction {

			get { return m_instr; }
		}

		public Mono.Cecil.MethodDefinition Method {

			get { return m_meth; }
		}

		public int CompareTo(object other) {

			CILLocation o = (CILLocation)other;
			int retval = CILMethodDefinitionComparer.CompareMethodDefinitions(m_meth, o.Method);
			if (retval == 0)
				retval = CILInstructionComparer.CompareInstructions(m_instr, o.Instruction);
			return retval;
		}

		// TODO: use better hashing
		public override int GetHashCode() {

			int retval = CILElementHashCodeProvider.CalcCILHashCode(m_instr);
			retval ^= MMC.HashMasks.MASK2;
			retval += CILElementHashCodeProvider.CalcCILHashCode(m_meth);
			return retval;
		}

		public override bool Equals(object other) {

			return CompareTo(other) == 0;
		}

		public override string ToString() {

			return string.Format("{0}:{1}", m_meth.Name, m_instr.Offset);
		}

		public CILLocation(Mono.Cecil.Cil.Instruction instr,
				Mono.Cecil.MethodDefinition meth) {

			m_instr = instr;
			m_meth = meth;
		}
	}

	class CILInstructionComparer : System.Collections.IComparer {

		public static int CompareInstructions(Instruction a, Instruction b) {

			int retval = 0;
			if (a != b) {
				retval = a.Offset - b.Offset;

				if (retval == 0)
					retval = a.OpCode.Value - b.OpCode.Value;

				if (retval == 0)
					retval = a.Operand.GetHashCode() - b.Operand.GetHashCode();
			}
			return retval;
		}

		public int Compare(object a, object b) {

			return CompareInstructions(a as Instruction, b as Instruction);
		}
	}

	class CILMethodDefinitionComparer : System.Collections.IComparer {

		public static int CompareMethodDefinitions(MethodDefinition a, MethodDefinition b) {

			int retval = 0;
			if (a != b) {
				retval = string.Compare(a.Name, b.Name);
				if (retval == 0) {
					retval = CILTypeReferenceComparer.CompareTypeReferences(
							a.DeclaringType,
							b.DeclaringType);
				}
			}
			return retval;
		}

		public int Compare(object a, object b) {

			return CompareMethodDefinitions(a as MethodDefinition, b as MethodDefinition);
		}
	}

	class CILTypeReferenceComparer : System.Collections.IComparer {

		public static int CompareTypeReferences(TypeReference a, TypeReference b) {

			int retval;
			if (a == null)
				retval = -1;
			else if (b == null)
				retval = 1;
			else
				retval = string.Compare(a.FullName, b.FullName);
			return retval;
		}

		public int Compare(object a, object b) {

			return CompareTypeReferences(a as TypeReference, b as TypeReference);
		}
	}

	class CILElementComparer : System.Collections.IComparer {

		public int Compare(object a, object b) {

			int retval = 1;
			if (a.GetType() != b.GetType()) {
				// When comparing apples to oranges issue a warning. Not very
				// useful for the user, but this is an indication the developer
				// should use separate hash tables, or improve the hashing
				// functions.
				MonoModelChecker.Message("Comparing two elements of inequal types: {0} and {1}",
						a.GetType(), b.GetType());
			} else if (a is TypeReference)
				retval = CILTypeReferenceComparer.CompareTypeReferences(
						a as TypeReference, b as TypeReference);
			else if (a is MethodDefinition)
				retval = CILMethodDefinitionComparer.CompareMethodDefinitions(
						a as MethodDefinition, b as MethodDefinition);
			else if (a is Instruction)
				retval = CILInstructionComparer.CompareInstructions(
						a as Instruction, b as Instruction);
			else if (a is System.Delegate) {
				// Can't really do better here. No way to compare method pointers safely.
				// GetHashCode() and Equals() are overloaded in System.Delegate.
				retval = a.GetHashCode() - b.GetHashCode();
			} else {
				// We did not implement a comparison function for operands of
				// this type. Issue a warning.
				MonoModelChecker.Message(
						"Comparer for objects of type {0} not implemented.", a.GetType());
			}

			return retval;
		}
	}

	class CILElementHashCodeProvider : System.Collections.IHashCodeProvider {

		// TODO : use better hashing
		public int GetHashCode(object o) {

			return CalcCILHashCode(o);
		}

		public static int CalcCILHashCode(object o) {

			int retval = 0;
			if (o is Instruction) {
				//retval = CalcInstructionHashCode(o as Instruction);
								return o.GetHashCode();
			} else if (o is MethodDefinition) {
				retval = CalcMethodDefinitionHashCode(o as MethodDefinition);
			} else if (o is TypeReference) {
				retval = CalcTypeReferenceHashCode(o as TypeReference);
			} else {
				MMC.MonoModelChecker.Message(
						"Hash code calculation for objects of type {0} not implemented.", o.GetType());
				retval = o.GetHashCode();
			}
			return retval;
		}

		public static int CalcInstructionHashCode(Mono.Cecil.Cil.Instruction instr) {

			int retval = 1;
			retval += instr.Offset;
			retval ^= HashMasks.MASK1;
			retval += instr.OpCode.Value;
			retval ^= HashMasks.MASK2;
			retval += (instr.Previous != null ? instr.Previous.Offset : -1);
			return retval;
		}

		public static int CalcMethodDefinitionHashCode(Mono.Cecil.MethodDefinition meth) {

			int retval = 13;
			retval += meth.Name.GetHashCode();
			retval ^= MMC.HashMasks.MASK3;
			Debug.Assert(meth.DeclaringType != null, "method definition " +
					meth.Name + " without a declaring type.",
				   "check for incomplete cloning of CIL elements.");
			retval += CalcTypeReferenceHashCode(meth.DeclaringType);
			return retval;
		}

		public static int CalcTypeReferenceHashCode(Mono.Cecil.TypeReference type) {

			return (type.FullName.GetHashCode() ^ HashMasks.MASK5) + 29;
		}
	}

	class CILElementPrinter : IToStringConvertor {

		public static string FormatCILElement(object o) {

			string retval;
			if (o == null) {
				retval = "null";
			} else if (o is System.String) {
				retval = string.Format("\"{0}\"", o as string);
			} else if (o is TypeReference) {
				retval = o.ToString();
			} else if (o is MethodReference) {
				MethodReference m = o as MethodReference;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.AppendFormat("{0}::{1}(",
						(m.DeclaringType != null ? m.DeclaringType.Name : "null"),
						m.Name);
				for (int i = 0; i < m.Parameters.Count; ++i) {
					if (i > 0)
						sb.Append(", ");
					sb.Append(m.Parameters[i].ParameterType.Name);
				}
				sb.Append(")");
				retval = sb.ToString();
			} else if (o is FieldReference) {
				FieldReference f = o as FieldReference;
				retval = string.Format("{0}::{1}", f.DeclaringType.Name, f.Name);
			} else if (o is VariableReference) {
				VariableReference v = o as VariableReference;
				retval = v.Name;
			} else if (o is Instruction) {
				Instruction i = o as Instruction;
				//retval = string.Format("[{0:D4}]{1}", i.Offset, i.OpCode.Name);
				retval = string.Format("{0:D4}", i.Offset);
			} else {
				MMC.MonoModelChecker.Message("Printer for objects of type {0} not implemented.", o.GetType());
				retval = o.ToString();
			}
			return retval;
		}

		public string ToString(object o) {

			return FormatCILElement(o);
		}
	}
}
