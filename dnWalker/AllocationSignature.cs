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

namespace MMC.State
{
    using System.Collections;
    using System.Collections.Generic;
    using dnlib.DotNet;
    using dnlib.DotNet.Emit;
    using MMC.Util;

    struct AllocationSignature
    {

        public MethodDef Method;
        public Instruction Instruction;
        public int Occurence;
        public int ThreadId;
    }

    class AllocSigHashHelper : IComparer<AllocationSignature>, IEqualityComparer<AllocationSignature>
    {
        int IEqualityComparer<AllocationSignature>.GetHashCode(AllocationSignature alsig)
        {
            int hashvalue = -1;
            hashvalue += (int)alsig.Instruction.Offset;
            hashvalue ^= HashMasks.MASK1;
            hashvalue += alsig.Method.Name.GetHashCode();
            hashvalue ^= HashMasks.MASK2;
            hashvalue += alsig.Method.DeclaringType.Name.GetHashCode();
            hashvalue ^= HashMasks.MASK1;
            hashvalue += alsig.Occurence;
            hashvalue += alsig.ThreadId * 71;
            return hashvalue;
        }

        bool IEqualityComparer<AllocationSignature>.Equals(AllocationSignature a, AllocationSignature b)
        {
            return Compare(a, b) == 0;
        }

        public int Compare(AllocationSignature aa, AllocationSignature bb)
        {
            int cmp = aa.ThreadId - bb.ThreadId;
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