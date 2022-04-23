using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public class Model : IModel
    {
        private readonly Dictionary<IVariable, IValue> _values;
        private readonly IHeapInfo _heapInfo;

        public Model()
        {
            _values = new Dictionary<IVariable, IValue>();
            _heapInfo = new HeapInfo();
        }

        public Model(Model model)
        {
            _values = new Dictionary<IVariable, IValue>(model._values.Select(p => KeyValuePair.Create(p.Key, p.Value.Clone())));
            _heapInfo = model._heapInfo;
        }

        IModel IModel.Clone()
        {
            return Clone();
        }

        IReadOnlyModel IReadOnlyModel.Clone()
        {
            return Clone();
        }

        public Model Clone()
        {
            return new Model(this);
        }

        public bool TryGetValue(IVariable variable, out IValue value)
        {
            return _values.TryGetValue(variable, out value);

            //if (!_values.TryGetValue(variable, out IValue value))
            //{
            //    value = ValueFactory.GetDefault(variable.Type);
            //    _values.Add(variable, value);
            //}
            //return value;
        }

        public void SetValue(IVariable variable, IValue value)
        {
            _values[variable] = value;
        }

        public IHeapInfo HeapInfo => _heapInfo;

        public IReadOnlyCollection<IVariable> Variables => _values.Keys;
    }
}
