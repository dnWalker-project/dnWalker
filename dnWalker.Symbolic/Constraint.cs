using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public class Constraint : ICloneable
    {

        private readonly List<PureTerm> _pureTerms;
        private readonly List<HeapTerm> _heapTerms;

        public Constraint()
        {
            _pureTerms = new List<PureTerm>();
            _heapTerms = new List<HeapTerm>();
        }

        public Constraint(IEnumerable<PureTerm> pureTerms, IEnumerable<HeapTerm> heapTerms)
        {
            _pureTerms = new List<PureTerm>(pureTerms);
            _heapTerms = new List<HeapTerm>(heapTerms);
        }

        public IReadOnlyList<PureTerm> PureTerms => _pureTerms;
        public IReadOnlyList<HeapTerm> HeapTerms => _heapTerms;

        public void AddExpressionConstraint(Expression expression) => _pureTerms.Add(new BooleanExpressionTerm(expression));

        public void AddEqualConstraint(Expression left, Expression right) => AddExpressionConstraint(ExpressionFactory.Default.MakeEqual(left, right));
        public void AddNotEqualConstraint(Expression left, Expression right) => AddExpressionConstraint(ExpressionFactory.Default.MakeNotEqual(left, right));
        public void AddLessThanConstraint(Expression left, Expression right) => AddExpressionConstraint(ExpressionFactory.Default.MakeLessThan(left, right));
        public void AddLessThanOrEqualConstraint(Expression left, Expression right) => AddExpressionConstraint(ExpressionFactory.Default.MakeLessThanOrEqual(left, right));
        public void AddGreaterThanConstraint(Expression left, Expression right) => AddExpressionConstraint(ExpressionFactory.Default.MakeGreaterThan(left, right));
        public void AddGreaterThanOrEqualConstraint(Expression left, Expression right) => AddExpressionConstraint(ExpressionFactory.Default.MakeGreaterThanOrEqual(left, right));

        public void AddPointToConstriant(IVariable source, params KeyValuePair<object, IVariable>[] attributes)
        {
            _heapTerms.Add(new PointToTerm(source, attributes));
        }


        object ICloneable.Clone()
        {
            return Clone();
        }

        public Constraint Clone()
        {
            return new Constraint(_pureTerms, _heapTerms);
        }

        public static Constraint Merge(IEnumerable<Constraint> constraints)
        {
            IEnumerable<PureTerm> pure = constraints.SelectMany(static c => c.PureTerms);
            IEnumerable<HeapTerm> heap = constraints.SelectMany(static c => c.HeapTerms);

            return new Constraint(pure, heap);
        }

        public override string ToString()
        {
            if (_pureTerms.Count > 0 && _heapTerms.Count > 0)
            {
                return $"({string.Join(" && ", _pureTerms)}) && ({string.Join(" * ", _heapTerms)})";
            }
            else if (_pureTerms.Count > 0 && _heapTerms.Count == 0)
            {
                return $"{string.Join(" && ", _pureTerms)}";
            }
            else if (_pureTerms.Count == 0 && _heapTerms.Count > 0)
            {
                return $"{string.Join(" * ", _heapTerms)}";
            }
            else // if (_pureTerms.Count == 0 && _heapTerms.Count == 0)
            {
                return "True";
            }
        }
    }
}
