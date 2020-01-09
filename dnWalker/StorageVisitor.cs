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

	interface IStorageVisitable {

		void Accept(IStorageVisitor visitor);
	}

	interface IStorageVisitor {

		object Result { get; }

		void VisitActiveState(ExplicitActiveState act);

		void VisitDynamicArea(DynamicArea dyn);
		void VisitAllocatedObject(AllocatedObject ao);
		void VisitAllocatedArray(AllocatedArray arr);
		void VisitAllocatedDelegate(AllocatedDelegate del);

		void VisitStaticArea(IStaticArea stat);
		void VisitAllocatedClass(AllocatedClass cls);

		void VisitThreadPool(ThreadPool tp);
		void VisitThreadState(ThreadState trd);
		void VisitMethodState(MethodState meth);
	}

	abstract class BaseStorageVisitor {

		public virtual object Result {

			get { return null; }
		}

		public virtual void VisitActiveState(ExplicitActiveState act) {}

		public virtual void VisitDynamicArea(DynamicArea dyn) {}
		public virtual void VisitAllocatedObject(AllocatedObject ao) {}
		public virtual void VisitAllocatedArray(AllocatedArray arr) {}
		public virtual void VisitAllocatedDelegate(AllocatedDelegate del) {}

		public virtual void VisitStaticArea(IStaticArea stat) {}
		public virtual void VisitAllocatedClass(AllocatedClass cls) {}

		public virtual void VisitThreadPool(ThreadPool tp) {}
		public virtual void VisitThreadState(ThreadState trd) {}
		public virtual void VisitMethodState(MethodState meth) {}
	}
}
