using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public abstract class ExpressionVisitor
    {
        public virtual Expression Visit(Expression expression) => expression.Accept(this);
        public IEnumerable<Expression> Visit(IEnumerable<Expression> expressions) => expressions.Select(e => Visit(e));

        internal protected virtual Expression VisitUnary(UnaryExpression unary)
        {
            Expression inner = Visit(unary.Inner);
            if (!ReferenceEquals(inner, unary.Inner))
            {
                return new UnaryExpression(unary.Operator, inner);
            }
            return unary;
        }
        internal protected virtual Expression VisitBinary(BinaryExpression binary)
        {
            Expression left = Visit(binary.Left);
            Expression right = Visit(binary.Right);
            if (!ReferenceEquals(left, binary.Left) ||
                !ReferenceEquals(right, binary.Right))
            {
                return new BinaryExpression(binary.Operator, left, right);
            }
            return binary;
        }
        internal protected virtual Expression VisitConstant(ConstantExpression constant)
        {
            return constant;
        }
        internal protected virtual Expression VisitVariable(VariableExpression variable)
        {
            return variable;
        }

        internal protected virtual Expression VisitStringOperation(StringOperationExpression stringOperation)
        {
            Expression[] oldOperands = stringOperation.Operands;
            Expression[] newOperands = new Expression[oldOperands.Length];

            bool changed = false;
            for (int i = 0; i < oldOperands.Length; ++i)
            {
                newOperands[i] = Visit(stringOperation.Operands[i]);
                changed |= ReferenceEquals(oldOperands[i], newOperands[i]);
            }

            if (changed)
            {
                return new StringOperationExpression(stringOperation.Operator, newOperands);
            }
            else
            {
                return stringOperation;
            }
        }

        internal protected virtual Expression VisitConvert(ConvertExpression convertExpression)
        {
            Expression inner = Visit(convertExpression.Inner);
            
            if (!ReferenceEquals(inner, convertExpression.Inner))
            {
                return new ConvertExpression(convertExpression.Type, inner);
            }
            return convertExpression;
        }

        internal protected virtual Expression VisitGeneric(GenericExpression genericExpression)
        {
            bool changed = false;
            Expression[] operands = new Expression[genericExpression.Operands.Count];
            for (int i = 0; i < operands.Length; ++i)
            {
                Expression old = genericExpression.Operands[i];
                operands[i] = Visit(old);
                changed |= !ReferenceEquals(old, operands[i]);
            }
            
            return changed ? new GenericExpression(genericExpression.Type, genericExpression.Operation, operands) : genericExpression;
        }
    }

    public abstract class ExpressionVisitor<T>
    {


        public virtual Expression Visit(Expression expression, T context) => expression.Accept(this, context);

        public IEnumerable<Expression> Visit(IEnumerable<Expression> expressions, T context) => expressions.Select(e => Visit(e, context));

        internal protected virtual Expression VisitUnary(UnaryExpression unary, T context)
        {
            Expression inner = Visit(unary.Inner, context);
            if (!ReferenceEquals(inner, unary.Inner))
            {
                return new UnaryExpression(unary.Operator, inner);
            }
            return unary;
        }
        internal protected virtual Expression VisitBinary(BinaryExpression binary, T context)
        {
            Expression left = Visit(binary.Left, context);
            Expression right = Visit(binary.Right, context);
            if (!ReferenceEquals(left, binary.Left) ||
                !ReferenceEquals(right, binary.Right))
            {
                return new BinaryExpression(binary.Operator, left, right);
            }
            return binary;
        }
        internal protected virtual Expression VisitConstant(ConstantExpression constant, T context)
        {
            return constant;
        }
        internal protected virtual Expression VisitVariable(VariableExpression variable, T context)
        {
            return variable;
        }

        internal protected virtual Expression VisitStringOperation(StringOperationExpression stringOperation, T context)
        {
            Expression[] oldOperands = stringOperation.Operands;
            Expression[] newOperands = new Expression[oldOperands.Length];

            bool changed = false;
            for (int i = 0; i < oldOperands.Length; ++i)
            {
                newOperands[i] = Visit(stringOperation.Operands[i], context);
                changed |= ReferenceEquals(oldOperands[i], newOperands[i]);
            }

            if (changed)
            {
                return new StringOperationExpression(stringOperation.Operator, newOperands);
            }
            else
            {
                return stringOperation;
            }
        }

        internal protected virtual Expression VisitConvert(ConvertExpression convertExpression, T context)
        {
            Expression inner = Visit(convertExpression.Inner, context);

            if (!ReferenceEquals(inner, convertExpression.Inner))
            {
                return new ConvertExpression(convertExpression.Type, inner);
            }
            return convertExpression;
        }


        internal protected virtual Expression VisitGeneric(GenericExpression genericExpression, T context)
        {
            bool changed = false;
            Expression[] operands = new Expression[genericExpression.Operands.Count];
            for (int i = 0; i < operands.Length; ++i)
            {
                Expression old = genericExpression.Operands[i];
                operands[i] = Visit(old, context);
                changed |= !ReferenceEquals(old, operands[i]);
            }

            return changed ? new GenericExpression(genericExpression.Type, genericExpression.Operation, operands) : genericExpression;
        }
    }
}
