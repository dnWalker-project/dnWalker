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

namespace MMC.InstructionExec {

	using System.Collections;
	using Mono.Cecil.Cil;
	using MMC.Collections;

	/// \brief An interface to types that return an instruction executor for a
	/// given instruction.	
	interface IInstructionExecProvider {

		/// Get an instruction executor for the given instruction.
		InstructionExecBase GetExecFor(Instruction instr);
	}

	/// Singleton accessor for instruction executor provider.
	static class InstructionExecProvider {

		static IInstructionExecProvider instance = null;

		/// The singleton instruction executor provider.
		public static IInstructionExecProvider iep {

			get {
				if (instance == null) {
					if (Config.UseInstructionCache)
						instance = new HashedIEC();
					else
						instance = new NoStorageIEC();
				}
				return instance;
			}
		}
	}

	/// Provide a fresh instruction executor every time. No storage.
	class NoStorageIEC : IInstructionExecProvider {

		/// Get an instruction executor for the given instruction.
		///
		/// This always creates a new one.
		///
		/// \param instr The instruction to create a new IE for.
		/// \return An IE for the given instruction.
		public InstructionExecBase GetExecFor(Instruction instr) {

			return InstructionExecBase.CreateInstructionExec(instr);
		}
	}

	/// A instruction executor provider that caches the IEs.
	///
	/// A test that re-used code a lot (calculate the same thing 10000 times)
	/// showed a speed-up of about 30 times when using HashedIEC instead of the
	/// NoStorageIEC.
	///
	/// However, calculate that same thing 100 times, and other factors become
	/// much more important, and this ratio drops to 1.5--2.
	///
	/// Calculate it just once, and the two perform the same. Other factors
	/// outweigh the small hashing overhead.
	///
	/// Note that this structure grows indefinitely. This is not a really big
	/// problem. If the code is really big, this is still a relatively small
	/// part of the consumed memory.
	class HashedIEC : IInstructionExecProvider {

		//IDictionary m_instrExecCache;
		public FastHashtable<Instruction, InstructionExecBase> m_instrExecCache;

		/// Get an instruction executor for the given instruction.
		///
		/// This re-uses previously allocated IEs to avoid a 'new' operation
		/// and parsing operands, etc.
		///
		/// \param instr The instruction to create a new IE for.
		/// \return An IE for the given instruction.
		public InstructionExecBase GetExecFor(Instruction instr) {

			/*
			InstructionExecBase retval = m_instrExecCache[instr] as InstructionExecBase;
			if (retval == null) {
				retval = InstructionExecBase.CreateInstructionExec(instr);
				m_instrExecCache.Add(instr, retval);
			}
			return retval;*/

			InstructionExecBase retval;
			if (!m_instrExecCache.Find(instr, out retval)) {
				retval = InstructionExecBase.CreateInstructionExec(instr);
				m_instrExecCache.UncheckedAdd(instr, retval);
			}

			return retval;

		}

		public HashedIEC() {

			//m_instrExecCache = new Hashtable();
			m_instrExecCache = new FastHashtable<Instruction, InstructionExecBase>(14);
		}
	}
}
