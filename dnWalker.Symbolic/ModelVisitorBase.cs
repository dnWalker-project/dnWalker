using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public class ModelVisitorBase : IModelVisitor
    {
        private IReadOnlyModel? _model;
        private readonly Stack<IVariable> _variables = new Stack<IVariable>();

        public virtual void Visit(IModelVisitable visitable)
        {
            visitable.Accept(this);
        }

        public virtual void VisitModel(IReadOnlyModel model)
        {
            if (_model != null) throw new InvalidOperationException("Cannot explore more than one model at the time!");
            _variables.Clear();

            try
            {
                _model = model;

                foreach (IVariable variable in model.Variables)
                {
                    Visit(variable);
                }
            }
            finally
            {
                _model = null;
            }
        }

        protected Boolean TryGetHeapNode<TNode>(Location location, [NotNullWhen(true)] out TNode? node)
            where TNode : IReadOnlyHeapNode
        {
            if (_model!.HeapInfo.TryGetNode(location, out IReadOnlyHeapNode? n) && n is TNode nd)
            {
                node = nd;
                return true;
            }
            node = default(TNode);
            return false;
        }

        protected Boolean TryGetHeapNode<TNode>(IVariable variable, [NotNullWhen(true)] out TNode? node)
            where TNode : IReadOnlyHeapNode
        {
            if (TryGetValue(variable, out Location location) && TryGetHeapNode(location, out node))
            {
                return true;
            }
            node = default(TNode);
            return false;
        }

        protected Boolean TryGetValue<TValue>(IVariable variable, [NotNullWhen(true)] out TValue? value)
            where TValue : IValue
        {
            if (_model!.TryGetValue(variable, out IValue? v) && v is TValue val)
            {
                value = val;
                return true;
            }
            value = default(TValue);
            return false;

        }

        protected Stack<IVariable> Variables
        {
            get
            {
                return _variables;
            }
        }

        protected IReadOnlyModel? Model
        {
            get
            {
                return _model;
            }

            set
            {
                _model = value;
            }
        }

        private IVariable GetCurrentVariable()
        {
            return _variables.Peek();
        }

        public virtual void VisitVariable(IVariable variable)
        {
            _variables.Push(variable);

            switch (variable)
            {
                case ArrayElementVariable v: VisitArrayElementVariable(v); break;
                case ArrayLengthVariable v: VisitArrayLengthVariable(v); break;
                case InstanceFieldVariable v: VisitInstanceFieldVariable(v); break;
                case MethodArgumentVariable v: VisitMethodArgumentVariable(v); break;
                case MethodResultVariable v: VisitMethodResultVariable(v); break;
                case StaticFieldVariable v: VisitStaticFieldVariable(v); break;
                case NamedVariable v: VisitNamedVariable(v); break;
                case ReturnValueVariable v: VisitReturnValueVariable(v); break;
                case ConditionalMethodResultVariable v: VisitConditionalMethodResultVaraible(v); break;
            }

            _variables.Pop();
        }

        protected virtual void VisitConditionalMethodResultVaraible(ConditionalMethodResultVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitArrayElementVariable(ArrayElementVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitArrayLengthVariable(ArrayLengthVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitInstanceFieldVariable(InstanceFieldVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitMethodArgumentVariable(MethodArgumentVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitMethodResultVariable(MethodResultVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitStaticFieldVariable(StaticFieldVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitNamedVariable(NamedVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }

        protected virtual void VisitReturnValueVariable(ReturnValueVariable variable)
        {
            if (TryGetHeapNode(variable, out IReadOnlyHeapNode? node))
            {
                Visit(node);
            }
        }



        public virtual void VisitHeapNode(IReadOnlyHeapNode heapNode)
        {
            switch (heapNode)
            {
                case IReadOnlyObjectHeapNode n: VisitObjectHeapNode(n); break;
                case IReadOnlyArrayHeapNode n: VisitArrayHeapNode(n); break;
            }
        }

        protected virtual void VisitArrayHeapNode(IReadOnlyArrayHeapNode node)
        {
            IVariable currentVar = GetCurrentVariable();
            foreach (int index in node.Indeces)
            {
                IVariable itemVariable = Variable.ArrayElement(currentVar, index);
                Visit(itemVariable);
            }
        }

        protected virtual void VisitObjectHeapNode(IReadOnlyObjectHeapNode node)
        {
            IVariable currentVar = GetCurrentVariable();
            foreach (IField field in node.Fields)
            {
                IVariable fieldVariable = Variable.InstanceField(currentVar, field);
                Visit(fieldVariable);
            }

            foreach ((IMethod method, int invocation) in node.MethodInvocations)
            {
                IVariable methodResultVariable = Variable.MethodResult(currentVar, method, invocation);
                Visit(methodResultVariable);
            }

            foreach ((IMethod method, Expression condition) in node.MethodConditions) 
            {
                IVariable conditionalVariable = new ConditionalMethodResultVariable(currentVar, method, condition);
                Visit(conditionalVariable);
            }
        }
    }
}
