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
        public virtual Expression VisitLength(LengthExpression lengthExpression) => lengthExpression;
        public virtual Expression VisitLocation(LocationExpression locationExpression) => locationExpression;
        public virtual Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression) => binaryOperationExpression;
        public virtual Expression VisitUnaryOperation(UnaryOperationExpression unaryOperationExpression) => unaryOperationExpression;
    }


    public abstract class ExpressionVisitor<TState>
    {
        public virtual Expression Visit(Expression expression, TState state) => expression.Accept(this, state);
        public virtual Expression VisitLength(LengthExpression lengthExpression, TState state) => lengthExpression;
        public virtual Expression VisitLocation(LocationExpression locationExpression, TState state) => locationExpression;
        public virtual Expression VisitBinaryOperation(BinaryOperationExpression binaryOperationExpression, TState state) => binaryOperationExpression;
        public virtual Expression VisitNull(NullExpression nullExpression, TState state) => nullExpression;
        public virtual Expression VisitBooleanConstant(BooleanConstantExpression booleanConstantExpression, TState state) => booleanConstantExpression;
        public virtual Expression VisitRealConstant(RealConstantExpression realConstantExpression, TState state) => realConstantExpression;
        public virtual Expression VisitIntegerConstant(IntegerConstantExpression integerConstantExpression, TState state) => integerConstantExpression;
        public virtual Expression VisitUnaryOperation(UnaryOperationExpression unaryOperationExpression, TState state) => unaryOperationExpression;
        public virtual Expression VisitVariable(VariableExpression variableExpression, TState state) => variableExpression;
        public virtual Expression VisitCharConstant(CharConstantExpression charConstantExpression, TState state) => charConstantExpression;
        public virtual Expression VisitStringConstant(StringConstantExpression stringConstantExpression, TState state) => stringConstantExpression;
    }
}
