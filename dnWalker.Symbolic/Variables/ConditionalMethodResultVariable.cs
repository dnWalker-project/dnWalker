using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Variables
{
    public readonly struct ConditionalMethodResultVariable : IMemberVariable, IEquatable<ConditionalMethodResultVariable>
    {
        private readonly IVariable _parent;
        private readonly IMethod _method;
        private readonly Expression _condition;

        public ConditionalMethodResultVariable(IVariable parent, IMethod method, Expression condition)
        {
            _parent = parent;
            _method = method;
            _condition = condition;
        }

        public bool Equals(ConditionalMethodResultVariable other)
        {
            return other._parent.Equals(_parent) &&
                   MethodEqualityComparer.CompareDeclaringTypes.Equals(other._method, _method) &&
                   other._condition.Equals(_condition);
        }

        public IVariable Parent
        {
            get
            {
                return _parent;
            }
        }

        public bool IsSameMemberAs(IMemberVariable other)
        {
            throw new NotImplementedException();
        }

        public IVariable Substitute(IVariable from, IVariable to)
        {
            if (from is ConditionalMethodResultVariable met &&
                MethodEqualityComparer.CompareDeclaringTypes.Equals(_method, met._method) &&
                _condition.Equals(met._condition) &&
                met.Parent.Equals(_parent))
            {
                // we are substituting me
                return to;
            }

            return new ConditionalMethodResultVariable(_parent.Substitute(from, to), _method, _condition);
        }

        public string Name => $"{_parent.Name}.{_method.Name}({_condition})";
        public TypeSig Type => _method.MethodSig.RetType;

        public IMethod Method
        {
            get
            {
                return _method;
            }
        }

        public Expression Condition
        {
            get
            {
                return _condition;
            }
        }

        public bool Equals(IVariable? other)
        {
            return other is ConditionalMethodResultVariable c && Equals(c);
        }

        public override bool Equals(object obj)
        {
            return obj is ConditionalMethodResultVariable && Equals((ConditionalMethodResultVariable)obj);
        }

        public static bool operator ==(ConditionalMethodResultVariable left, ConditionalMethodResultVariable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConditionalMethodResultVariable left, ConditionalMethodResultVariable right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MethodEqualityComparer.CompareDeclaringTypes.GetHashCode(_method), _parent, _condition);
        }
    }
}
