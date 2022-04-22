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

        public virtual Expression VisitBooleanConstant(BooleanConstantExpression booleanConstantExpression) => booleanConstantExpression;
        public virtual Expression VisitStringConstant(StringConstantExpression stringConstantExpression) => stringConstantExpression;
        public virtual Expression VisitIntegerConstant(IntegerConstantExpression integerConstantExpression) => integerConstantExpression;
        public virtual Expression VisitCharConstant(CharConstantExpression charConstantExpression) => charConstantExpression;
        public virtual Expression VisitRealConstant(RealConstantExpression realConstantExpression) => realConstantExpression;
        
        public virtual Expression VisitVariable(VariableExpression variableExpression) => variableExpression;

        public virtual Expression VisitNull(NullExpression nullExpression) => nullExpression;

        public virtual Expression VisitRealToInteger(RealToIntegerExpression realToIntegerExpression)
        {
            Expression inner = Visit(realToIntegerExpression.Inner);
            if (ReferenceEquals(inner, realToIntegerExpression.Inner))
            {
                return realToIntegerExpression;
            }
            else
            {
                return new RealToIntegerExpression(inner);
            }
        }
        public virtual Expression VisitIntegerToReal(IntegerToRealExpression integerToRealExpression)
        {
            Expression inner = Visit(integerToRealExpression.Inner);
            if (ReferenceEquals(inner, integerToRealExpression.Inner))
            {
                return integerToRealExpression;
            }
            else
            {
                return new IntegerToRealExpression(inner);
            }
        }
        public virtual Expression VisitLength(LengthExpression lengthExpression)
        {
            Expression expression = Visit(lengthExpression.Expression);
            if (ReferenceEquals(expression, lengthExpression.Expression))
            {
                return lengthExpression;
            }
            else
            {
                return new LengthExpression(expression);
            }
        }
        public virtual Expression VisitLocation(LocationExpression locationExpression)
        {
            Expression expression = Visit(locationExpression.Expression);
            if (ReferenceEquals(expression, locationExpression.Expression))
            {
                return locationExpression;
            }
            else
            {
                return new LocationExpression(expression);
            }
        }
        public virtual Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression)
        {
            Expression left = Visit(binaryOperationExpression.Left);
            Expression right = Visit(binaryOperationExpression.Right);
            if (!ReferenceEquals(left, binaryOperationExpression.Left) ||
                !ReferenceEquals(right, binaryOperationExpression.Right))
            {
                return new BinaryOperationExpression(binaryOperationExpression.Operator, left, right);
            }
            else
            {
                return binaryOperationExpression;
            }
        }
        public virtual Expression VisitUnaryOperation(UnaryOperationExpression unaryOperationExpression)
        {
            Expression operand = Visit(unaryOperationExpression.Operand);
            if (!ReferenceEquals(operand, unaryOperationExpression.Operand))
            {
                return new UnaryOperationExpression(unaryOperationExpression.Operator, operand);
            }
            else
            {
                return unaryOperationExpression;
            }
        }
    }


    public abstract class ExpressionVisitor<TState>
    {
        public virtual Expression Visit(Expression expression, TState state) => expression.Accept(this, state);

        public virtual Expression VisitBooleanConstant(BooleanConstantExpression booleanConstantExpression, TState state) => booleanConstantExpression;
        public virtual Expression VisitRealConstant(RealConstantExpression realConstantExpression, TState state) => realConstantExpression;
        public virtual Expression VisitIntegerConstant(IntegerConstantExpression integerConstantExpression, TState state) => integerConstantExpression;
        public virtual Expression VisitCharConstant(CharConstantExpression charConstantExpression, TState state) => charConstantExpression;
        public virtual Expression VisitStringConstant(StringConstantExpression stringConstantExpression, TState state) => stringConstantExpression;

        public virtual Expression VisitNull(NullExpression nullExpression, TState state) => nullExpression;

        public virtual Expression VisitVariable(VariableExpression variableExpression, TState state) => variableExpression;

        public virtual Expression VisitRealToInteger(RealToIntegerExpression realToIntegerExpression, TState state)
        {
            Expression inner = Visit(realToIntegerExpression.Inner, state);
            if (ReferenceEquals(inner, realToIntegerExpression.Inner))
            {
                return realToIntegerExpression;
            }
            else
            {
                return new RealToIntegerExpression(inner);
            }
        }
        public virtual Expression VisitIntegerToReal(IntegerToRealExpression integerToRealExpression, TState state)
        {
            Expression inner = Visit(integerToRealExpression.Inner, state);
            if (ReferenceEquals(inner, integerToRealExpression.Inner))
            {
                return integerToRealExpression;
            }
            else
            {
                return new IntegerToRealExpression(inner);
            }
        }

        public virtual Expression VisitLength(LengthExpression lengthExpression, TState state)
        {
            Expression expression = Visit(lengthExpression.Expression, state);
            if (ReferenceEquals(expression, lengthExpression.Expression))
            {
                return lengthExpression;
            }
            else
            {
                return new LengthExpression(expression);
            }
        }
        public virtual Expression VisitLocation(LocationExpression locationExpression, TState state)
        {
            Expression expression = Visit(locationExpression.Expression, state);
            if (ReferenceEquals(expression, locationExpression.Expression))
            {
                return locationExpression;
            }
            else
            {
                return new LocationExpression(expression);
            }
        }
        public virtual Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression, TState state)
        {
            Expression left = Visit(binaryOperationExpression.Left, state);
            Expression right = Visit(binaryOperationExpression.Right, state);
            if (!ReferenceEquals(left, binaryOperationExpression.Left) ||
                !ReferenceEquals(right, binaryOperationExpression.Right))
            {
                return new BinaryOperationExpression(binaryOperationExpression.Operator, left, right);
            }
            else
            {
                return binaryOperationExpression;
            }
        }
        public virtual Expression VisitUnaryOperation(UnaryOperationExpression unaryOperationExpression, TState state)
        {
            Expression operand = Visit(unaryOperationExpression.Operand, state);
            if (!ReferenceEquals(operand, unaryOperationExpression.Operand))
            {
                return new UnaryOperationExpression(unaryOperationExpression.Operator, operand);
            }
            else
            {
                return unaryOperationExpression;
            }
        }
    }
}
