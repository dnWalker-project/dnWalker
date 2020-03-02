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

namespace MMC.Collections {

    //	using C5;
    using System.Text;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    /// <summary>
    /// Hashtable with a power of two size, allows the modulo
    /// to be performed by masking bits
    /// </summary>
    class FastHashtable<K, V>
    {
		private class Bucket
        {
			public K key;
			public V val;
			public Bucket next;
			public int hashcode;

            internal Bucket Clone()
            {
                return new Bucket
                {
                    key = key,
                    val = val,
                    hashcode = hashcode,
                    next = next?.Clone()
                };
            }
        }

		Bucket[] m_buckets;
		int m_power;
		int m_mask;

		int m_count;

        /*public FastHashtable()
            : this(20)
        {
        }*/

		public int Count
        {
            get { return m_count; }
        }

        public FastHashtable(int power)
        {
            m_buckets = new Bucket[1 << power];
            m_power = power;
            m_mask = (1 << power) - 1;
        }

		/*
		public V this[K o] {
			get {
				int hashcode = o.GetHashCode();
				int index = hashcode & m_mask;

				for (Bucket b = m_buckets[index]; b != null; b = b.next)
					if (b.hashcode == hashcode && b.key.Equals(o))
						return b.val;

				return default(V);
			}

			set {
				Bucket front = new Bucket();
				front.key = o;
				front.val = value;
				front.hashcode = o.GetHashCode();
				int index = front.hashcode & m_mask;
				front.next = m_buckets[index];
				m_buckets[index] = front;
				m_count++;
			}
		}*/

		public bool Remove(K key) {
			int hashcode = key.GetHashCode();
			int index = hashcode & m_mask;
			Bucket bprev = null;

			for (Bucket b = m_buckets[index]; b != null; bprev = b, b = b.next)
				if (b.hashcode == hashcode && b.key.Equals(key)) {
					// remove bucket or remove the chain
					if (bprev == null)
						m_buckets[index] = null;
					else
						bprev.next = b.next;

					m_count--;
					return true;
				}

			return false;
		}

        internal FastHashtable<K, V> Clone()
        {
            return new FastHashtable<K, V>(m_power)
            {
                m_count = m_count,
                m_buckets = m_buckets?.Select(b => b?.Clone()).ToArray()
            };
        }

        public bool Find(ref K o) {
			int hashcode = o.GetHashCode();
			int index = hashcode & m_mask;

			for (Bucket b = m_buckets[index]; b != null; b = b.next)
				if (b.hashcode == hashcode && b.key.Equals(o)) {
					o = b.key;
					return true;
				}

			return false;
		}

		public bool Find(K o, out V v) {
			int hashcode = o.GetHashCode();
			int index = hashcode & m_mask;

			for (Bucket b = m_buckets[index]; b != null; b = b.next)
				if (b.hashcode == hashcode && b.key.Equals(o)) {
					o = b.key;
					v = b.val;
					return true;
				}

			v = default(V);
			return false;
		}

		/// Careful with using this method!
		///
		/// It adds an element to the hashtable without
		/// checking whether it is already there
		///
		/// Should only be used icw Find
		public void UncheckedAdd(K key, V val) {
			int hashcode = key.GetHashCode();
			int index = hashcode & m_mask;

			Bucket front = new Bucket();
			front.key = key;
			front.val = val;
			front.hashcode = hashcode;
			front.next = m_buckets[index];
			m_buckets[index] = front;
			m_count++;
		}

		public bool FindOrAdd(ref K key, ref V val) {
			int hashcode = key.GetHashCode();
			int index = hashcode & m_mask;

			for (Bucket b = m_buckets[index]; b != null; b = b.next)
				if (b.hashcode == hashcode && b.key.Equals(key)) {
					key = b.key;
					val = b.val;
					return true;
				}

			Bucket front = new Bucket();
			front.key = key;
			front.val = val;
			front.hashcode = hashcode;
			front.next = m_buckets[index];
			m_buckets[index] = front;
			m_count++;
			return false;
		}

		public string CalculateDistribution() {
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < m_buckets.Length; i++) {
				if (m_buckets[i] != null)
					sb.AppendFormat("{0}: ", i);
				for (Bucket b = m_buckets[i]; b != null; b = b.next)
					sb.Append("*");
				if (m_buckets[i] != null)
					sb.AppendLine();
			}

			return sb.ToString();
		}
	}



}
