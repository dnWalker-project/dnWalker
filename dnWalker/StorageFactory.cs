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

	using System.Collections.Generic;
	using MMC.State;

	interface IStorageFactory {

		DataElementList CreateList(int size);		
		DataElementList CreateSingleton(IDataElement elem);		

		DataElementStack CreateStack(int size);

		CallStack CreateCallStack();
		CallStack CreateCallStack(ICollection<MethodState> col);
	}

	class StorageFactory : IStorageFactory {

		static IStorageFactory instance = new StorageFactory();

		public static IStorageFactory sf {

			get { return instance; }
		}

		/*
		public static void UseRefCounting(bool useRC) {

			if (!useRC)
				instance = new StorageFactory();
			else
				instance = new RCStorageFactory();
		}*/

		public virtual DataElementList CreateList(int size) {

			return new DataElementList(size);
		}

		public DataElementList CreateSingleton(IDataElement elem) {

			DataElementList retval = CreateList(1);
			retval[0] = elem;
			return retval;
		}

		public virtual DataElementStack CreateStack(int size) {

			return new DataElementStack(size);
		}

		public virtual CallStack CreateCallStack() {

			return new CallStack();
		}

		public virtual CallStack CreateCallStack(ICollection<MethodState> col) {

			return new CallStack(col);
		}

		protected /* singleton */ StorageFactory() {

		}
	}
	
//    class RCStorageFactory : StorageFactory {

//        public override IDataElementStack CreateStack(int size) {

//            return new DataElementStackRC(size);
//        }

//        public override DataElementList CreateList(int size) {

//            return new DataElementListRC(size);
//        }

////		public override ICallStack CreateCallStack() {
////
////			return new CallStackRC();
////		}
////
////		public override ICallStack CreateCallStack(ICollection col) {
////
////			return new CallStackRC(col);
////		}

//        public /* singleton */ RCStorageFactory() : base() {

//            // do not call me.
//        }
//    }
}
