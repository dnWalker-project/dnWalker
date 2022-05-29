using dnlib.DotNet;

using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public class Model : IModel
    {
        private readonly Constraint _precondition;
        private readonly Dictionary<IRootVariable, IValue> _values;
        private readonly IHeapInfo _heapInfo;

        public Model(Constraint precondition)
        {
            _precondition = precondition ?? new Constraint();
            _values = new Dictionary<IRootVariable, IValue>();
            _heapInfo = new HeapInfo();
        }

        private Model(Model model)
        {
            _precondition = model._precondition.Clone();
            _values = new Dictionary<IRootVariable, IValue>(model._values);
            _heapInfo = model._heapInfo.Clone();
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

        public bool TryGetValue(IRootVariable variable, [NotNullWhen(true)]out IValue? value)
        {
            return _values.TryGetValue(variable, out value);
        }

        public void SetValue(IRootVariable variable, IValue value)
        {
            _values[variable] = value;
        }

        public IHeapInfo HeapInfo => _heapInfo;

        public IReadOnlyCollection<IRootVariable> Variables => _values.Keys;

        public Constraint Precondition => _precondition;
    }
}
