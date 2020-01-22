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


    using MMC.Data;
    using MMC.Util;
    using MMC.Collections;
    using C5;
    using MethodDefinition = dnlib.DotNet.MethodDef;
    using TypeDefinition = dnlib.DotNet.TypeDef;
    using FieldDefinition = dnlib.DotNet.FieldDef;

    public interface IInitData { } // "tag"

    public class AllocatedClass : Allocation
    {

        DataElementList m_fields;
        int m_staticFieldCount;
        bool m_isDirty;
        InitDataContainer m_initData;

        // Initialization related.

        public DataElementList Fields { get { return m_fields; } set { m_fields = value; } }
        public int StaticFieldCount { get { return m_staticFieldCount; } }

        public override int InnerSize
        {
            get
            {
                return m_fields.Length;
            }
        }

        // --------------------------- Loadin' an' Lockin' --------------------------- 

        public bool Loaded
        {

            get { return m_initData.Loaded; }
            set
            {
                bool prev = m_initData.Loaded;
                m_isDirty |= (!prev && value) || (prev && !value);

                // If we're unloading this class, we need to do a little more
                // than simply reset the loaded flag, i.e. clear the fields and
                // initialization data.
                if (!value && prev)
                {
                    ClearFields();
                    m_initData.Clear();
                }

                m_initData.Loaded = value;
            }
        }


        public bool Initialized
        {

            get { return m_initData.Initialized; }
            set
            {
                bool prev = m_initData.Initialized;
                m_isDirty |= (!prev && value) || (prev && !value);
                m_initData.Initialized = value;
            }
        }

        public int InitializingThread
        {

            get { return m_initData.InitializingThread; }
            set
            {
                m_isDirty |= (m_initData.InitializingThread != value);
                m_initData.InitializingThread = value;
            }
        }

        public IInitData InitData
        {

            get { return m_initData; }
            set
            {
                InitDataContainer initData = value as InitDataContainer;
                if (initData == null)
                {
                    MonoModelChecker.Message("setting initialization data using some strange type {0}",
                            value.GetType());
                }
                else
                {
                    m_isDirty |= !m_initData.Equals(initData);
                    m_initData = initData;
                }
            }
        }

        public void AwakenWaitingThreads(ExplicitActiveState cur)
        {
            foreach (int sleepy_thread in m_initData.WaitingThreads)
                cur.ThreadPool.Threads[sleepy_thread].Awaken();

            if (!m_initData.WaitingThreads.IsEmpty)
                m_initData.Dirty = true;

            m_initData.WaitingThreads.Clear();
        }

        public void AddWaitingThread(int thread_id, ExplicitActiveState cur)
        {
            if (thread_id < 0)
            {
                throw new System.Exception("Negative thread id");
            }
            cur.ThreadPool.Threads[thread_id].WaitFor(m_initData.InitializingThread);
            m_initData.WaitingThreads.Add(thread_id);
            m_initData.Dirty = true;
        }

        // --------------------------------------------------------------------------- 

        public override bool IsDirty()
        {

            return m_isDirty || m_fields.IsDirty() || Lock.IsDirty() || m_initData.Dirty;
        }

        public override void Clean()
        {
            m_initData.Dirty = false;
            m_fields.Clean();
            Lock.Clean();
            m_isDirty = false;
        }

        public override void Dispose()
        {

            m_fields.Dispose();
        }

        public override string ToString()
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder("c:");
            sb.AppendFormat("{0} {1}", Type.Name, m_initData.ToString());

            TypeDefinition typeDef = DefinitionProvider.dp.GetTypeDefinition(Type);

            bool printed_a_field = false;
            sb.Append(" flds: {");
            for (int i = 0; i < m_fields.Length; ++i)
            {
                if (typeDef.Fields[i].IsStatic)
                {
                    sb.AppendFormat("{0}{1}={2}",
                            (printed_a_field ? ", " : ""),
                            typeDef.Fields[i].Name,
                            m_fields[i].ToString());
                    printed_a_field = true;
                }
            }
            sb.Append("}");

            return sb.ToString();
        }

        public override void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
            visitor.VisitAllocatedClass(this);
        }

        public void ClearFields()
        {

            m_staticFieldCount = 0;
            TypeDefinition typeDef = DefinitionProvider.dp.GetTypeDefinition(Type);
            for (int i = 0; i < m_fields.Length; ++i)
            {
                m_fields[i] = DefinitionProvider.dp.GetNullValue(typeDef.Fields[i].FieldType);
                if (typeDef.Fields[i].IsStatic)
                {
                    ++m_staticFieldCount;
                }
            }
        }

        public AllocatedClass(TypeDefinition typeDef, IConfig config)
            : base(typeDef, config)
        {
            m_fields = StorageFactory.sf.CreateList(typeDef.Fields.Count);
            m_initData = new InitDataContainer();
            m_isDirty = true;
            ClearFields();
        }

        private class InitDataContainer : IInitData, IStorable
        {
            const int LoadedFlag = 1;
            const int InitializedFlag = 2;
            const int ReadOnlyFlag = 4;

            int m_state;
            int m_initTrd;
            // TODO: maybe an idea here to build and use an ISet interface?
            // oh anyway, this is clear and it works great 
            HashSet<int> m_waitingThreads;
            bool m_isReadonly;

            public bool Dirty { get; set; }

            public bool Loaded
            {
                get { return (m_state & LoadedFlag) != 0; }
                set
                {
                    int old_mstate = m_state;
                    if (value)
                        m_state |= LoadedFlag;
                    else
                        m_state &= ~LoadedFlag;
                    Dirty |= old_mstate == m_state;
                }
            }

            public bool Initialized
            {
                get { return (m_state & InitializedFlag) != 0; }
                set
                {
                    int old_mstate = m_state;
                    if (value)
                        m_state |= InitializedFlag;
                    else
                        m_state &= ~InitializedFlag;
                    Dirty |= old_mstate == m_state;
                }
            }

            public int InitializingThread
            {
                get { return m_initTrd; }
                set
                {
                    m_initTrd = value;
                    Dirty |= m_initTrd != value;
                }
            }

            public HashSet<int> WaitingThreads
            {
                get { return m_waitingThreads; }
            }

            public bool ReadOnly
            {
                get { return m_isReadonly; }
                set
                {
                    if (m_isReadonly)
                        throw new System.InvalidOperationException("InitDataContainer is read-only.");
                    m_isReadonly = value;
                }
            }

            public IStorable StorageCopy()
            {
                InitDataContainer retval = new InitDataContainer();
                retval.Loaded = Loaded;
                retval.Initialized = Initialized;
                retval.WaitingThreads.AddAll(WaitingThreads);
                retval.InitializingThread = InitializingThread;
                return retval;
            }

            public override int GetHashCode()
            {
                int retval = (m_state ^ HashMasks.MASK1) + m_initTrd;
                retval ^= HashMasks.MASK2;
                if (m_waitingThreads.Count > 0)
                    retval += ArrayIntHasher.GetHashCodeIntArray(m_waitingThreads.ToArray());
                return retval;
            }

            public override bool Equals(object other)
            {
                InitDataContainer o = other as InitDataContainer;
                bool equal = o != null && o.Initialized == Initialized &&
                    o.Loaded == Loaded && o.InitializingThread == InitializingThread &&
                    o.WaitingThreads.Count == WaitingThreads.Count;
                if (equal && WaitingThreads.Count > 0)
                    equal = IntArrayHashHelper.CompareIntArrays(
                            WaitingThreads.ToArray(),
                            o.WaitingThreads.ToArray()) == 0;
                return equal;
            }

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("c:");
                sb.AppendFormat("{0}{1}[i:{2} w:{3}]",
                    Loaded ? "L" : "-",
                    Initialized ? "I" : "-",
                    InitializingThread >= 0 ? InitializingThread.ToString() : "-",
                    WaitingThreads.ToString());
                return sb.ToString();
            }

            public void Clear()
            {
                m_state = 0;
                m_initTrd = LockManager.NoThread;
                m_waitingThreads.Clear();
                Dirty |= m_state != 0 && m_initTrd != LockManager.NoThread && !m_waitingThreads.IsEmpty;
            }

            public InitDataContainer()
            {
                Dirty = false;
                m_waitingThreads = new HashSet<int>();
                Clear();
            }
        }
    }
}