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

namespace MMC.Data {

    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using MMC.Util;
    using MMC.Collections;
    using System;


    /// This pool clones the objects stored and retrieved from it
    class CloningFastPool<T> : FastPool<T> where T : class {

		public CloningFastPool(int size) : base(size) {
		}

		public CloningFastPool()
			: base() {
	}

		public new int GetInt(T o) {

			// ---------------------------------------------------------------
			// Quick and dirty check. Multiple returns are evil, but like most
			// evil things, sometimes they're better than the so-called right
			// way, in this case adding a level of indentation.
			if (o == null)
				return CollectionConstants.NotSet;
			// ---------------------------------------------------------------

			var retval = m_intToElem.Count;

			if (!m_elemToInt.Find(ref o)) {
				var toStore = o;

				if (o is IStorable) {
					var copy = ((IStorable)o).StorageCopy();
					copy.ReadOnly = true;
					toStore = (T)copy;
				}

				m_elemToInt.FindOrAdd(ref toStore, ref retval);
				m_intToElem.Add(toStore);	

			} else 
				m_elemToInt.FindOrAdd(ref o, ref retval);

			return retval;
		}

        internal new CloningFastPool<T> Clone()
        {
            return new CloningFastPool<T>
            {
                m_elemToInt = m_elemToInt.Clone(),
                m_intToElem = m_intToElem == null ? null : new List<T>(m_intToElem),
            };
        }

        public new T GetObject(int i) {
			// ---------------------------------------------------------------
			if (i == CollectionConstants.NotSet)
				return null;
			// ---------------------------------------------------------------

			T retval = null;

			if (i >= 0 && i < m_intToElem.Count)
				retval = m_intToElem[i];

			if (retval is IStorable) {
				var copy = ((IStorable)retval).StorageCopy();
				copy.ReadOnly = false;
				retval = (T)copy;
			} 
				//throw new System.InvalidOperationException();
			
			return retval;
		}
	}


	/// FastPool is "fast" because it is based on FastHashtable;
	class FastPool<T> where T : class {

		public FastHashtable<T, int> m_elemToInt;
		public List<T> m_intToElem;

		public FastPool(int size) {
			m_elemToInt = new FastHashtable<T, int>(size);
			m_intToElem = new List<T>(100);

			/*
			 * To ensure that all the integers that a Pool returns start with a 1
			 */
			m_intToElem.Add(null);
		}

		public FastPool() : this(20) {
		}

		public int GetInt(T o) {

			// ---------------------------------------------------------------
			// Quick and dirty check. Multiple returns are evil, but like most
			// evil things, sometimes they're better than the so-called right
			// way, in this case adding a level of indentation.
			if (o == null)
				return CollectionConstants.NotSet;
			// ---------------------------------------------------------------

			var retval = m_intToElem.Count;

			if (!m_elemToInt.FindOrAdd(ref o, ref retval)) 
				m_intToElem.Add(o);			

			return retval; 
		}

		public T GetObject(int i) {
			return m_intToElem[i];
		}

        internal virtual FastPool<T> Clone()
        {
            return new FastPool<T>
            {
                m_elemToInt = m_elemToInt?.Clone(),
                m_intToElem = m_intToElem == null ? null : new List<T>(m_intToElem)
            };
        }

        /*public static implicit operator FastPool<T>(FastPool<WrappedIntArray> v)
        {
            throw new NotImplementedException();
        }*/
    }

	class Pool  {

		Hashtable m_elemToInt;
		ArrayList m_intToElem;
		IToStringConvertor m_tsc;
		bool m_clone;

		public Pool(IHashCodeProvider hcp, IComparer cmp, IToStringConvertor tsc, bool clone) {

			m_elemToInt = new Hashtable(hcp, cmp);
			m_intToElem = new ArrayList();
			m_tsc = tsc;
			m_clone = clone;

			/*
			 * To ensure that all the integers that a Pool returns start with a 1
			 */
			m_intToElem.Add(null);
			//GetInt(null);
		}
		public Pool() : this(null, null, null, true) { }

		public int GetInt(object o) {

			// ---------------------------------------------------------------
			// Quick and dirty check. Multiple returns are evil, but like most
			// evil things, sometimes they're better than the so-called right
			// way, in this case adding a level of indentation.
			if (o == null)
				return CollectionConstants.NotSet;
			// ---------------------------------------------------------------

			int retval;
			var wrapped = m_elemToInt[o];
			if (wrapped == null) {
				// Make a copy of the object to store if it implements the
				// Clone() method. Else, assume it's either an immutable
				// object, or the caller knows the difference between passing
				// by value and by reference. :-)
				var key = o;
				if (m_clone && (o is IStorable)) {
					var copy = ((IStorable)o).StorageCopy();
					copy.ReadOnly = true;
					key = copy;
				} else if (m_clone && (o is System.ICloneable)) {
					//MonoModelChecker.Message("plain cloning of object {0}.", o.ToString());
					key = ((System.ICloneable)o).Clone();
				}

				retval = m_intToElem.Add(key);
				m_elemToInt[key] = retval;
			} else
				retval = (int)wrapped;

			return retval;
		}

		public object GetObject(int i) {

			// ---------------------------------------------------------------
			if (i == CollectionConstants.NotSet)
				return null;
			// ---------------------------------------------------------------

			object retval = null;
			if (i >= 0 && i < m_intToElem.Count)
				retval = m_intToElem[i];
			if (m_clone && retval is IStorable) {
				var copy = ((IStorable)retval).StorageCopy();
				copy.ReadOnly = false;
				retval = copy;
			} else if (m_clone && (retval is System.ICloneable)) {
				retval = ((System.ICloneable)retval).Clone();
			}
			return retval;
		}

		public override string ToString() {

			var sb = new StringBuilder();
			for (var i = 0; i < m_intToElem.Count; ++i)
				sb.AppendFormat("\t{0} -> {1}\n", i,
						(m_tsc != null ? m_tsc.ToString(m_intToElem[i]) : m_intToElem[i].ToString()));
			if (sb.Length == 0)
				sb.Append("Empty.");
			return sb.ToString();
		}

        internal Pool Clone()
        {
            return new Pool
            {
                m_clone = m_clone,
                m_elemToInt = m_elemToInt == null ? null : new Hashtable(m_elemToInt),
                m_intToElem = m_intToElem == null ? null : new ArrayList(m_intToElem),
                m_tsc = m_tsc
            };
        }
    }
}
