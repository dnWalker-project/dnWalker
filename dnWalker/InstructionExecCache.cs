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

namespace MMC.InstructionExec
{
    using dnlib.DotNet.Emit;
    using dnWalker.Factories;
    using MMC.Collections;

	/// <summary>
	/// An interface to types that return an instruction executor for a given instruction.
	/// </summary>
	public interface IInstructionExecProvider
    {
		/// <summary>
		/// Get an instruction executor for the given instruction.
		/// </summary>
		InstructionExecBase GetExecFor(Instruction instr);
	}

    /// <summary>
    /// Singleton accessor for instruction executor provider.
    /// </summary>
    internal static class InstructionExecProvider
    {
        /// <summary>
        /// The singleton instruction executor provider.
        /// </summary>
        public static IInstructionExecProvider Get(IConfig config, IInstructionFactory instructionFactory)
        {
            if (config.UseInstructionCache)
            {
                return new HashedIEC(instructionFactory);
            }
            else
            {
                return new NoStorageIEC(instructionFactory);
            }
        }
	}

    /// <summary>
    /// Provide a fresh instruction executor every time. No storage.
    /// </summary>
    internal class NoStorageIEC : IInstructionExecProvider
    {
        private readonly IInstructionFactory _instructionFactory;

        /// <summary>
        /// Get an instruction executor for the given instruction.
        /// </summary>
        /// <remarks>
        /// This always creates a new one.
        /// </remarks>
        /// <param name="instr">The instruction to create a new IE for.</param>
        /// <returns>An IE for the given instruction.</returns>
        public InstructionExecBase GetExecFor(Instruction instr)
        {
            return _instructionFactory.CreateInstructionExec(instr);
        }

        public NoStorageIEC(IInstructionFactory instructionFactory)
        {
            _instructionFactory = instructionFactory;
        }
    }

    /// <summary>
    /// A instruction executor provider that caches the IEs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A test that re-used code a lot (calculate the same thing 10000 times)
    /// showed a speed-up of about 30 times when using HashedIEC instead of the NoStorageIEC.
    /// </para>
    /// <para>
    /// However, calculate that same thing 100 times, and other factors become
    /// much more important, and this ratio drops to 1.5--2.
    /// </para>
    /// <para>
    /// Calculate it just once, and the two perform the same. Other factors outweigh the small hashing overhead.
    /// </para>
    /// <para>
    /// Note that this structure grows indefinitely. This is not a really big
    /// problem. If the code is really big, this is still a relatively small
    /// part of the consumed memory.
    /// </para>
    /// </remarks>
    internal class HashedIEC : IInstructionExecProvider
    {
        private readonly IInstructionFactory _instructionFactory;
        private readonly FastHashtable<Instruction, InstructionExecBase> m_instrExecCache;

        /// <summary>
        /// Get an instruction executor for the given instruction.
        /// </summary>
        /// <remarks>
        /// This re-uses previously allocated IEs to avoid a 'new' operation
        /// and parsing operands, etc.
        /// </remarks>
        /// <param name="instr">The instruction to create a new IE for.</param>
        /// <returns>An IE for the given instruction.</returns>
        public InstructionExecBase GetExecFor(Instruction instr)
        {
            if (!m_instrExecCache.Find(instr, out var retval))
            {
                retval = _instructionFactory.CreateInstructionExec(instr);
                m_instrExecCache.UncheckedAdd(instr, retval);
            }

            return retval;
		}

		public HashedIEC(IInstructionFactory instructionFactory)
        {
            _instructionFactory = instructionFactory;
            m_instrExecCache = new FastHashtable<Instruction, InstructionExecBase>(14);
		}
	}
}
