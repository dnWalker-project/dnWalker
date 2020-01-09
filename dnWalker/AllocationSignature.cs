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

namespace MMC.State {

	using Mono.Cecil;
	using Mono.Cecil.Cil;
#if NET_2_0
	using System.Collections.Generic;
#else
	using System.Collections;
#endif
	using MMC.Util;

	struct AllocationSignature {

		public MethodDefinition Method;
		public Instruction Instruction;
		public int Occurence;
		public int ThreadId;
	}

#if NET_2_0
	class AllocSigHashHelper : IEqualityComparer {
#else
	class AllocSigHashHelper : IComparer, IHashCodeProvider {
#endif
		
		public int GetHashCode(object o) {

			int hashvalue = -1;
			AllocationSignature alsig = (AllocationSignature)o;
			hashvalue += alsig.Instruction.Offset;
			hashvalue ^= HashMasks.MASK1;
			hashvalue += alsig.Method.Name.GetHashCode();
			hashvalue ^= HashMasks.MASK2;
			hashvalue += alsig.Method.DeclaringType.Name.GetHashCode();
			hashvalue ^= HashMasks.MASK1;
			hashvalue += alsig.Occurence;
			hashvalue += alsig.ThreadId * 71;
			return hashvalue;
		}

		public new bool Equals(object a, object b) {

			return Compare(a, b) == 0;
		}

		public int Compare(object a, object b) {

			int cmp;
			AllocationSignature aa = (AllocationSignature)a;
			AllocationSignature bb = (AllocationSignature)b;

			cmp = aa.ThreadId - bb.ThreadId;
			if (cmp != 0)
				cmp = aa.Occurence - bb.Occurence;
			else if (cmp != 0)
				cmp = aa.Instruction.GetHashCode() - bb.Instruction.GetHashCode();
			else if (cmp != 0)
				cmp = aa.Method.GetHashCode() - bb.Method.GetHashCode();

			return cmp;
		}
	}
}
