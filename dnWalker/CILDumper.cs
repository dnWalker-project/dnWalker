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

/// \author Niels Aan de Brugh <n.h.m.aandebrugh@student.utwente.nl>

namespace MMC {
    using dnlib.DotNet.Emit;
    using System.IO;
	using System.Text;
    using MethodDefinition = dnlib.DotNet.MethodDef;
    using TypeDefinition = dnlib.DotNet.TypeDef;

    /// This is a simple tool to dump the contents of a CIL image on stdout.
    ///
    /// It outputs the CIL data in a hierarchical way, like so:
    /// <pre>
    /// Assembly
    ///   Module
    ///     Class
    ///       Fields
    ///       Constructors
    ///         Parameters
    ///         Locals
    ///         Instructions
    ///       Methods
    ///         (same as for the constructors)
    /// </pre>
    ///
    /// Each individual method can be used to print less of the tree,
    /// but it will recursively go into the depth. If you only want to print
    /// one element, look at the file CILUtils.cs.
    ///
    /// \sa CILElementPrinter

    class CILDumper {

		int m_indent;
		TextWriter o;

		/// <summary>Print a formatted string to the output.</summary>
		///
		/// This method prefixes the string with 2*m_indent spaces.
		///
		/// <param name="str">Format string (cf. string.Format).</param>
		/// <param name="values">Values for {0}, {1}, etc.</param>
		public void Print(string str, params object[] values) {

			var sb = new StringBuilder();
			for (var i=m_indent; i > 0; --i)
				sb.Append("  ");
			sb.AppendFormat(str, values);
			o.WriteLine(sb.ToString());
		}

		/// <summary>Format and print instruction.</summary>
		///
		/// Example:
		/// <tt>0010 | ldstr "Apenkooi is leuk."</tt>
		///
		/// <param name="instr">The Instruction to print and format.</param>
		public void PrintInstruction(Instruction instr) {

			++m_indent;

			var operandstr = "";
			if (instr.Operand != null) {
				if (instr.Operand is Instruction)
					operandstr = ((Instruction)instr.Operand).Offset.ToString("D4");
				else
					operandstr = instr.Operand.ToString();
			}
			Print("{0:D4} | {1} {2}", instr.Offset, instr.OpCode.Name, operandstr);

			--m_indent;
		}

		/// <summary>Format and print method.</summary>
		///
		/// This prints:
		/// <nl>
		///   <li>method name, maxstack and code size</li>
		///   <li>parameters: sequence number, type and name</li>
		///   <li>locals: index number, type and name</li>
		///   <li>instructions: see PrintInstruction</li>
		/// </nl>
		///
		/// <param name="meth">The method to format and print.</param>
		/// \sa PrintInstruction
		public void PrintMethod(MethodDefinition meth) {

			Print("method: {0}::{1} .maxstack {2} .codesize {3} [{4}]",
					meth.DeclaringType.Name,
				   	meth.Name,
					meth.Body.MaxStack,
					-1,//meth.Body,
					meth.Attributes);
			++m_indent;

			Print("parameters (total {0}):", meth.Parameters.Count);
			++m_indent;
			foreach (var par in meth.Parameters)
				Print("{0}: {1} {2}", par.MethodSigIndex, par.Type.FullName, par.Name);
			--m_indent;
			
			Print("locals (total {0}):", meth.Body.Variables.Count);
			++m_indent;
            foreach (var var in meth.Body.Variables)
                Print("?");// {0}: {1} {2} ({3})", var.Index, var.VariableType.Name, var.Name, var.VariableType.IsValueType ? "val":"ref");
			--m_indent;

			Print("instructions (total {0}):", meth.Body.Instructions.Count);
			++m_indent;
			foreach (var instr in meth.Body.Instructions)
				PrintInstruction(instr);
			--m_indent;

			--m_indent;
		}

		/// <summary>Format and print a type (class, struct, etc.)</summary>
		///
		/// This prints:
		/// <nl>
		///   <li>type name, base class and implemented interfaces<li>
		///   <li>fields: type, name and attributes</li>
		///	  <li>constructors: see PrintMethod</li>
		///	  <li>methods: see constructors</li>
		/// </nl>
		///
		/// <param name="type">The type to format and print.</param>
		/// \sa PrintMethod
		public void PrintType(TypeDefinition type) {

			var sb = new StringBuilder();

			foreach (var iface in type.Interfaces)
				sb.AppendFormat("{0}{1}", 
						(sb.Length > 0 ? ", " : ""),
						iface.Interface.FullName);
			Print("type {0}  base {1}{2}{3}",
					type.Name,
					(type.BaseType != null ? type.BaseType.Name.String : "NONE"),
					(type.Interfaces.Count > 0 ? " implements " : ""),
					(type.Interfaces.Count > 0 ? sb.ToString() : ""));

			++m_indent;

			Print("fields (total {0})", type.Fields.Count);
			++m_indent;
			foreach (var fld in type.Fields)
				Print("{0} {1} [{2}]", fld.FieldType.FullName, fld.Name, fld.Attributes);
			--m_indent;

            Print("ctors (total {0})", -1);// type.FindConstructors().Count());
			++m_indent;
			foreach (var ctor in type.FindConstructors())
				PrintMethod(ctor);
			--m_indent;

			Print("methods (total {0})", type.Methods.Count);
			++m_indent;
			foreach (var meth in type.Methods)
				PrintMethod(meth);
			--m_indent;

			--m_indent;
		}

		/// <summary>Format and print a module.</summary>
		///
		/// This prints all types in the module.
		///
		/// <param name="mod">The module to format and print.</param>
		/// \sa PrintType
		public void PrintModule(dnlib.DotNet.ModuleDef mod) {

			Print("module {0}", mod.Name);
			++m_indent;
			
			Print("types (total {0})", mod.Types.Count);
			++m_indent;
			foreach (var type in mod.Types)
				PrintType(type);
			--m_indent;

			--m_indent;
		}

		/// <summary>Format and print an assembly.</summary>
		///
		/// This prints all modules in the assembly.
		///
		/// <param name="asm">The assembly format and to print.</param>
		/// \sa PrintModule
		public void PrintAssembly(dnlib.DotNet.AssemblyDef asm) {

			Print("assembly {0} kind {1}", asm.Name, asm.FullName);
			
			Print("modules (total {0})", asm.Modules.Count);
			++m_indent;
			foreach (var mod in asm.Modules)
				PrintModule(mod);
			--m_indent;
		}

		/// <summary>Initiate a new CILDumper.</summary>
		///
		/// <param name="output">Text output to write to.</param>
		public CILDumper(TextWriter output) {

			m_indent = 0;
			o = output;
		}

		/// <summary>Fatal error: print message and exit.</summary>
		///
		/// <param name="msg">Message to be printed</param>
		public static void Fatal(string msg) {

			System.Console.WriteLine("FATAL: " + msg);
			System.Environment.Exit(2);
		}


		/// <summary>Assembly entry point for cildump.exe.</summary>
		///
		/// This loads the assembly specified by name as the first argument.
		/// This assembly is then printed on stdout.
		///
		/// <param name="args">Command-line arguments.</param>
		/// \sa PrintAssembly
		public static void _Main(string[] args) {

			/*if (args.Length == 0)
				Fatal("please specify the path name of the assembly to print.");

			CILDumper d = new CILDumper(System.Console.Out);
			AssemblyDefinition asm = null;
			try {
				asm = AssemblyFactory.GetAssembly(args[0]);
				d.PrintAssembly(asm);
			}
			catch (ReflectionException) {
				Fatal("error loading assembly.");
			}*/
		}
	}
}
