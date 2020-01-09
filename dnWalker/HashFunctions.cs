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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MMC.Data;

namespace MMC.Collections {
	sealed class SingleIntHasher {
	
		/// Wang's hash
		public static int Hash(int key) {
			key = ~key + (key << 15); // key = (key << 15) - key - 1;
			key = key ^ (key >> 12);
			key = key + (key << 2);
			key = key ^ (key >> 4);
			key = key * 2057; // key = (key + (key << 3)) + (key << 11);
			key = key ^ (key >> 16);
			return key;
		}
	}

	class ArrayIntHasher  {



		public static int GetHashCodeDataElementContainer(IDataElementContainer val, int length) {
			// Jenkins' LOOKUP3 hash  (May 2006), gracefully copied from JPF
			uint a = 0x510fb60d;
			uint b = 0xa4cb30d9 + ((uint)length);
			uint c = 0x9e3779b9;

			int i;
			for (i = 0; i < length - 2; i += 3) {
				a += (uint)val[i].GetHashCode();
				b += (uint)val[i + 1].GetHashCode();
				c += (uint)val[i + 2].GetHashCode();
				a -= c;
				a ^= (c << 4) ^ (c >> 28);
				c += b;
				b -= a;
				b ^= (a << 6) ^ (a >> 26);
				a += c;
				c -= b;
				c ^= (b << 8) ^ (b >> 24);
				b += a;
				a -= c;
				a ^= (c << 16) ^ (c >> 16);
				c += b;
				b -= a;
				b ^= (a << 19) ^ (a >> 13);
				a += c;
				c -= b;
				c ^= (b << 4) ^ (b >> 28);
				b += a;
			}

			switch (length - i) {
				case 2:
					c += (uint)val[length - 2].GetHashCode();
					b += (uint)val[length - 1].GetHashCode();
					break;
				case 1:
					b += (uint)val[length - 1].GetHashCode();
					break;
			}
			c ^= b;
			c -= (b << 14) ^ (b >> 18);
			a ^= c;
			a -= (c << 11) ^ (c >> 21);
			b ^= a;
			b -= (a << 25) ^ (a >> 7);
			c ^= b;
			c -= (b << 16) ^ (b >> 16);
			a ^= c;
			a -= (c << 4) ^ (c >> 28);
			b ^= a;
			b -= (a << 14) ^ (a >> 18);
			c ^= b;
			c -= (b << 24) ^ (b >> 8);

			return (int)(c ^ b ^ a);
		}

		public static int GetHashCodeEnumerable(IEnumerable ie, int length) {
			// Jenkins' LOOKUP3 hash  (May 2006), gracefully copied from JPF
			uint a = 0x510fb60d;
			uint b = 0xa4cb30d9 + ((uint)length);
			uint c = 0x9e3779b9;

			IEnumerator enumerator = ie.GetEnumerator();
			enumerator.MoveNext();

			int i;
			for (i = 0; i < length - 2; i += 3) {
				a += (uint)((int)enumerator.Current);
				enumerator.MoveNext();
				b += (uint)((int)enumerator.Current);
				enumerator.MoveNext();
				c += (uint)((int)enumerator.Current);
				enumerator.MoveNext();
				a -= c;
				a ^= (c << 4) ^ (c >> 28);
				c += b;
				b -= a;
				b ^= (a << 6) ^ (a >> 26);
				a += c;
				c -= b;
				c ^= (b << 8) ^ (b >> 24);
				b += a;
				a -= c;
				a ^= (c << 16) ^ (c >> 16);
				c += b;
				b -= a;
				b ^= (a << 19) ^ (a >> 13);
				a += c;
				c -= b;
				c ^= (b << 4) ^ (b >> 28);
				b += a;
			}

			switch (length - i) {
				case 2:
					c += (uint)((int)enumerator.Current);
					enumerator.MoveNext();
					b += (uint)((int)enumerator.Current);
					break;
				case 1:
					b += (uint)((int)enumerator.Current);
					break;
			}
			c ^= b;
			c -= (b << 14) ^ (b >> 18);
			a ^= c;
			a -= (c << 11) ^ (c >> 21);
			b ^= a;
			b -= (a << 25) ^ (a >> 7);
			c ^= b;
			c -= (b << 16) ^ (b >> 16);
			a ^= c;
			a -= (c << 4) ^ (c >> 28);
			b ^= a;
			b -= (a << 14) ^ (a >> 18);
			c ^= b;
			c -= (b << 24) ^ (b >> 8);

			return (int)(c ^ b ^ a);
		}

		public static int GetHashCodeIntArray(int[] val) {

			// Jenkins' LOOKUP3 hash  (May 2006), gracefully copied from JPF
			uint a = 0x510fb60d;
			uint b = 0xa4cb30d9 + ((uint)val.Length);
			uint c = 0x9e3779b9;

			int i;
			for (i = 0; i < val.Length - 2; i += 3) {
				a += (uint)val[i];
				b += (uint)val[i + 1];
				c += (uint)val[i + 2];
				a -= c;
				a ^= (c << 4) ^ (c >> 28);
				c += b;
				b -= a;
				b ^= (a << 6) ^ (a >> 26);
				a += c;
				c -= b;
				c ^= (b << 8) ^ (b >> 24);
				b += a;
				a -= c;
				a ^= (c << 16) ^ (c >> 16);
				c += b;
				b -= a;
				b ^= (a << 19) ^ (a >> 13);
				a += c;
				c -= b;
				c ^= (b << 4) ^ (b >> 28);
				b += a;
			}

			switch (val.Length - i) {
				case 2:
					c += (uint)val[val.Length - 2];
					b += (uint)val[val.Length - 1];
					break;
				case 1:
					b += (uint)val[val.Length - 1];
					break;
			}
			c ^= b;
			c -= (b << 14) ^ (b >> 18);
			a ^= c;
			a -= (c << 11) ^ (c >> 21);
			b ^= a;
			b -= (a << 25) ^ (a >> 7);
			c ^= b;
			c -= (b << 16) ^ (b >> 16);
			a ^= c;
			a -= (c << 4) ^ (c >> 28);
			b ^= a;
			b -= (a << 14) ^ (a >> 18);
			c ^= b;
			c -= (b << 24) ^ (b >> 8);

			return (int)(c ^ b ^ a);
			//return ((long)c << (long)32) ^ (long)b ^ (long)a;
		}
	}
}
