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

namespace MMC
{
	/// <summary>Short-hands for System.Threading.ThreadState enumeration.</summary>
	/// Basically, this struct just contains short-hands for the different
	/// thread states.
	internal struct ThreadStatus
    {
		/// <summary>Unused.</summary>
		public const int Unstarted		= (int)System.Threading.ThreadState.Unstarted; // 8

		/// <summary>Thread is running. Note that in MMC the state is never</summary>
		/// guarenteed to be fully up to date, so make sure you double check.
		public const int Running		= (int)System.Threading.ThreadState.Running; // 0

		/// <summary>Thread is waiting to acquire a lock, sleeping, or waiting</summary>
		/// for another thread to terminate (join).
		public const int WaitSleepJoin	= (int)System.Threading.ThreadState.WaitSleepJoin; // 32

        /// <summary>Thread has stopped. This means it has been terminated.</summary>
		public const int Stopped		= (int)System.Threading.ThreadState.Stopped; // 16

        /// <summary>Unused at the moment.</summary>
		public const int AbortRequested	= (int)System.Threading.ThreadState.AbortRequested; // 128

        /// <summary>Unused at the moment.</summary>
		public const int Aborted		= (int)System.Threading.ThreadState.Aborted; // 256

		/// <summary>Check if a thread state means a thread is not terminated.</summary>
		/// <param name="val">The thread state to check.</param>
		/// <returns>True iff the thread state means a living thread.</returns>
		public static bool IsAlive(int val)
        {
			return (val == Unstarted) || (val == Running) || (val == WaitSleepJoin);
		}

		/// <summary>Format a thread state to a readable format.</summary>
		///
		/// This basically is the reverse of using the constants, i.e. it gives
		/// the name with the number. This exploits the standard enum print
		/// facility of Mono (CLI?).
		///
		/// <param name="s">The thread state to format.</param>
		/// <returns>A formatted thread state.</returns>
		public static string ToString(int s)
        {
			return ((System.Threading.ThreadState)s).ToString();
		}
	}

    /// <summary>A bunch of random numbers used in hashing. </summary>
    /// <remarks>Its values really should not matter at all.</remarks>
    internal struct HashMasks
    {
		public const int MASK1 = 0x0473DE6F;
		public const int MASK2 = 0x0ABCDEF1;
		public const int MASK3 = 0x043672AF;
		public const int MASK4 = 0x063612E5;
		public const int MASK5 = 0x0B267382;
        public const int MASK6 = 0x01234567;
        public const int MASK7 = 0x09876543;
		public const int MASKC = 7;
	}

	/// <summary>
	/// Constants to signal special events to the dot writer.
	/// </summary>
	internal struct DotGraph
    {
		public const int BacktrackEdge	= -1;
	}

	/// <summary>
	/// Offsets of collapsed method state parts.
	/// </summary>
	internal struct MethodPartsOffsets
    {
		public const int Definition 		= 0;
		public const int ProgramCounter		= 1;
		public const int IsExceptionSource	= 2;
		public const int OnDispose			= 3;
		public const int Arguments			= 4;
		public const int Locals				= 5;
		public const int EvalStack			= 6;
		public const int Count				= 7;
	}

	/// <summary>
	/// Offsets of collapsed class state parts.
	/// </summary>
	internal struct ClassPartsOffsets
    {
//		public const int Definition		= 0; // definition is implicit by position.
		public const int InitData		= 0;
		public const int Lock			= 1;
		public const int Fields			= 2;
		public const int Count			= 3;
	}

	/// <summary>
	/// Offsets of collapsed thread state parts.
	/// </summary>
	internal struct ThreadPartOffsets
    {
		public const int State				= 0;
		public const int WaitingFor			= 1;
		public const int CallStack			= 2;
		public const int ExceptionReference	= 3;
		public const int Count				= 4;
	}

    /// <summary>Offsets of collapsed allocation parts.</summary>
    /// <remarks>Each specific allocation should not use these fields for something else.
    /// There is no way to enforce this by means of inheritence without
    /// throwing out of the constant folding optimization, making accessing
    /// these constants expensive (virtual call).</remarks>
    internal struct AllocationPartsOffsets
    {
		public const int AllocationType = 0;
		public const int Lock			= 1;
	}

	/// <summary>
	/// Offsets of collapsed object parts.
	/// </summary>
	internal struct ObjectPartsOffsets
    {
		public const int AllocationType = 0;
		public const int Lock			= 1;
		public const int Definition		= 2;
		public const int Fields			= 3;
		public const int Count			= 4;
	}

	/// <summary>
	/// Offsets of collapsed array parts.
	/// </summary>
	internal struct ArrayPartsOffsets
    {
		public const int AllocationType = 0;
		public const int Lock			= 1;
		public const int Definition		= 2;
		public const int Elements		= 3;
		public const int Count			= 4;
	}

	/// <summary>
	/// Offsets of collapsed delegate parts.
	/// </summary>
	internal struct DelegatePartsOffsets
    {
		public const int AllocationType = 0;
		public const int Lock			= 1;
		public const int Object			= 2;
		public const int MethodPointer	= 3;
		public const int Count			= 4;
	}

    /// <summary>Constants used for sets and lists throughout MMC.</summary>
    /// <remarks>Values are not relevant, but should be less than zero.</remarks>
    internal struct CollectionConstants
    {
		public const int NotSet = 0; // change by VY: this exceptional case made things difficult
 		public const int Deleted		= -20;
	}
}
