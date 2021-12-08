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

	using System.Diagnostics;
	using MMC.State;
	using MMC.Util;
	using MMC.Collections;

    public class DataElementStack : IDataElementContainer {

		protected IDataElement[] m_elements;
		protected int m_stackptr;
		protected bool m_isDirty;
		protected bool m_isReadonly;
        private readonly ExplicitActiveState cur;

        public int StackPointer
        {
            get { return m_stackptr; }
            set { m_stackptr = value; } // do not use
        }

		public int	Length { get { return StackPointer; } }
		public int	Capacity { get { return m_elements.Length; } }
		public bool	IsDirty() { return m_isDirty; }
		public void	Clean() { m_isDirty = false; }
		public bool	IsEmpty() {	return m_stackptr == 0; }
		// The following member functions are RC-related.
		public virtual void Dispose() { }
		public virtual void PopAll() { m_stackptr = 0; }

		public IDataElement this[int index] {

			get { return m_elements[index]; }
			set { 
				if (m_isReadonly)
					throw new System.InvalidOperationException("Changing read-only data element stack.");
				m_elements[index] = value;
			}
		}

		public bool ReadOnly {

			get { return m_isReadonly; }
			set { 
				if (m_isReadonly)
					throw new System.InvalidOperationException("Changing read-only data element stack.");
				m_isReadonly = value;
			}
		}

        public void Push(object o)
        {
			Push(cur.DefinitionProvider.CreateDataElement(o));
        }

        public virtual void Push(IDataElement e) {

			if (m_isReadonly)
				throw new System.InvalidOperationException("Changing read-only data element stack.");
			m_isDirty = true;
			m_elements[m_stackptr++] = e;

			ThreadObjectWatcher.Increment(cur.ThreadPool.CurrentThreadId, e, cur);
		}

		public virtual IDataElement Pop()
        {
            if (m_isReadonly)
            {
                throw new System.InvalidOperationException("Changing read-only data element stack.");
            }

            if (m_stackptr - 1 < 0 || m_stackptr > m_elements.Length)// || m_stackptr > m_elements.Length)
            {
                throw new System.InvalidOperationException("Cannot pop " + (m_stackptr - 1));
            }

			m_isDirty = true;

			var popped = m_elements[--m_stackptr];

			ThreadObjectWatcher.Decrement(cur.ThreadPool.CurrentThreadId, popped, cur);

			//if (popped is ObjectReference && !popped.Equals(ObjectReference.Null)) 
			//	Explorer.ActivateGC = true;
			return popped;
		}

        public IDataElement Peek(int depth = 0)
        {
            return m_elements[m_stackptr - depth - 1];
        }

        internal IDataElement Top()
        {
            if (m_stackptr < 1)
            {
                return null;
            }

            return m_elements[m_stackptr - 1];
        }

        public override string ToString() {

//			return string.Format("{0} ({1}, c:{2})",
//					ListToString.Format(m_elements, 0, m_stackptr),
//					(m_isReadonly ? "ro":"rw"),
//					m_elements.Length);
			return ListToString.Format(m_elements, 0, m_stackptr);
		}

		public virtual IStorable StorageCopy()
        {
			return new DataElementStack(this, cur);
		}

		public override int GetHashCode()
        {
			// Note that both the capacity and the stack pointer are taken into
			// account here. This is done on purpose!		
			return ArrayIntHasher.GetHashCodeDataElementContainer(this, m_stackptr);
		}

        public override bool Equals(object other)
        {
            var o = other as DataElementStack;
            var equal = o != null
                && o.Capacity == Capacity
                && o.StackPointer == StackPointer;
            for (var i = 0; equal && i < m_stackptr; ++i)
            {
                equal = o[i].Equals(m_elements[i]);
            }
            return equal;
        }

		public DataElementStack(int maxStack, ExplicitActiveState cur)
        {
            this.cur = cur;
			m_elements = new IDataElement[maxStack];
			m_stackptr = 0;
			m_isDirty = true;
			m_isReadonly = false;
		}

		protected DataElementStack(DataElementStack copy, ExplicitActiveState cur)
        {
            this.cur = cur;
            m_elements = new IDataElement[copy.Capacity];
			m_stackptr = copy.StackPointer;
			m_isDirty = true;
			m_isReadonly = false;
            for (var i = 0; i < m_stackptr; ++i)
            {
                m_elements[i] = copy[i];
            }
		}
	}

	/*
	class DataElementStackRC : DataElementStack {

		public override void Push(IDataElement e) {

			base.Push(e);
			if (e is ObjectReference)
				ActiveState.cur.DynamicArea.IncRefCount((ObjectReference)e);
		}

		public override IDataElement Pop() {

			IDataElement retval = base.Pop();
			if (retval is ObjectReference)
				ActiveState.cur.DynamicArea.DecRefCount((ObjectReference)retval);
			return retval;
		}

		public override void PopAll() {

			while (m_stackptr > 0)
				Pop();
		}

		public override void Dispose() {

			// We just want to decrement the reference count for all referred objects,
			// not to kill the entire stack. For example, the RET::Execute method still
			// wants to get a return value from the stack after Pop()ing the method state.
			for (int i = m_stackptr-1; i >= 0; --i)
				if (m_elements[m_stackptr] is ObjectReference)
					ActiveState.cur.DynamicArea.DecRefCount((ObjectReference)m_elements[i]);
		}

		public override IStorable StorageCopy() {

			return new DataElementStackRC(this);
		}

		public DataElementStackRC(int maxStack) : base(maxStack) {}
		protected DataElementStackRC(IDataElementStack copy) : base(copy) {}
	}*/
}
