
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{

    public partial class ExpressionEvaluator : ExpressionVisitor<IReadOnlyModel>
    {
        private IValue? _result;
        private IReadOnlyModel? _model;

        [ThreadStatic]
        private static readonly ExpressionEvaluator _instance = new ExpressionEvaluator();

        public static IValue Evaluate(Expression expression, IReadOnlyModel model) 
        {
            _instance.Visit(expression, model);
            return _instance._result!;
        }

        protected internal override Expression VisitBinary(BinaryExpression binary, IReadOnlyModel context)
        {
            Visit(binary.Left, context);
            IValue left = _result!;
            Visit(binary.Right, context);
            IValue right = _result!;

            switch (binary.Operator)
            {
                case Operator.And:
                    _result = ValueFactory.GetValue<bool>(((PrimitiveValue<bool>)left).Value &&
                                                          ((PrimitiveValue<bool>)right).Value);
                    break;
                case Operator.Or:
                    _result = ValueFactory.GetValue<bool>(((PrimitiveValue<bool>)left).Value ||
                                                          ((PrimitiveValue<bool>)right).Value);
                    break;
                case Operator.Xor:
                    _result = ValueFactory.GetValue<bool>(((PrimitiveValue<bool>)left).Value ^
                                                          ((PrimitiveValue<bool>)right).Value);
                    break;
                case Operator.Add:
                    _result = BinaryOperations.Add(left, right);
                    break;
                case Operator.Subtract:
                    _result = BinaryOperations.Subtract(left, right);
                    break;
                case Operator.Multiply:
                    _result = BinaryOperations.Multiply(left, right);
                    break;
                case Operator.Divide:
                    _result = BinaryOperations.Divide(left, right);
                    break;
                case Operator.Modulo:
                    _result = BinaryOperations.Modulo(left, right);
                    break;
                case Operator.Equal:
                    _result = BinaryOperations.Equal(left, right);
                    break;
                case Operator.NotEqual:
                    _result = BinaryOperations.NotEqual(left, right);
                    break;
                case Operator.GreaterThan:
                    _result = BinaryOperations.GreaterThan(left, right);
                    break;
                case Operator.GreaterThanOrEqual:
                    _result = BinaryOperations.GreaterThanOrEqual(left, right);
                    break;
                case Operator.LessThan:
                    _result = BinaryOperations.LessThan(left, right);
                    break;
                case Operator.LessThanOrEqual:
                    _result = BinaryOperations.LessThanOrEqual(left, right);
                    break;

                default:
                    throw new NotSupportedException($"Not supported operator for evaluation: {binary.Operator}");
            }

            return binary;
        }

        protected internal override Expression VisitUnary(UnaryExpression unary, IReadOnlyModel context)
        {
            Visit(unary.Inner, context);

            switch (unary.Operator)
            {
                case Operator.Not:
                    _result = ValueFactory.GetValue<bool>(!((PrimitiveValue<bool>)_result!).Value);
                    break;

                case Operator.Negate:
                    _result = UnaryOperations.Negate(_result!);
                    break;

                default:
                    throw new NotSupportedException($"Not supported operator for evaluation: {unary.Operator}");
            }

            return unary;
        }

        protected internal override Expression VisitConstant(ConstantExpression constant, IReadOnlyModel context)
        {
            _result = Constants.HandleConstant(constant.Value);

            return constant;
        }


        protected internal override Expression VisitVariable(VariableExpression variable, IReadOnlyModel context)
        {
            _result = context.GetValueOrDefault(variable.Variable);

            return variable;
        }
    }
}
