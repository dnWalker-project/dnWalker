using dnlib.DotNet;

using dnWalker.Symbolic.Utils;

namespace dnWalker.Symbolic.Expressions
{
    public abstract partial class Expression
    {
        public abstract TypeSig Type { get; }

        internal protected abstract Expression Accept(ExpressionVisitor visitor);
        internal protected abstract Expression Accept<T>(ExpressionVisitor<T> visitor, T context);


        public sealed override string ToString() => Utils.ExpressionWriter.ToString(this);
        public sealed override int GetHashCode() => Utils.ExpressionComparer.GetHashCode(this);
        public override bool Equals(object? obj) => obj is Expression other && Utils.ExpressionComparer.Equals(this, other);

        public virtual Expression AsBoolean()
        {
            if (Type.IsBoolean()) return this;
            if (Type.IsNumber()) return new BinaryExpression(Operator.GreaterThan, this, new ConstantExpression(Type.Module.CorLibTypes.Int32, 0));
            if (Type.IsString()) return new BinaryExpression(Operator.NotEqual, this, new ConstantExpression(Type.Module.CorLibTypes.String, null));
            if (!Type.IsPrimitive) return new BinaryExpression(Operator.NotEqual, this, new ConstantExpression(Type.Module.CorLibTypes.Object, null));

            throw new Exception("Unexpected expression type.");
        }
    }
}
