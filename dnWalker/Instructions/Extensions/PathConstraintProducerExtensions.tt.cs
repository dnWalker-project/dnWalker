using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{

    public abstract class PathConstraintProducerInstructionExtension : IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
    {
        public abstract bool CanExecute(Code code);

        private Instruction _nextInstruction;

        public abstract void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur);

        public virtual void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            if (WillBranch(retValue))
            {
                // we do jump;
                _nextInstruction = (Instruction)instruction.Operand;
            }
            else
            {
                // we do NOT jump
                _nextInstruction = null;
            }
        }

        [Pure]
        protected static bool WillBranch(IIEReturnValue retValue)
        {
            return retValue is not NextReturnValue;
        }

        protected void AddPathConstraint(ExplicitActiveState cur, Expression expression)
        {
            cur.PathStore.CurrentPath.AddPathConstraint(expression, _nextInstruction, cur);
        }

    }

    public abstract class UnaryPathConstraintProducerInstructionExtension : PathConstraintProducerInstructionExtension
    {
        private IDataElement _value;

        protected IDataElement Value
        {
            get { return _value; }
        }

        public override void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            _value = cur.EvalStack.Peek();
        }

        public override void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            // initialize 
            base.PostExecute(instruction, cur, retValue);

            if (IsSymbolic(cur, out Expression valueExpression))
            {
                // we may branch based on a symbolic value => add the path constraint
                Expression constraintExpression = CreatePathConstraintExpression(valueExpression, WillBranch(retValue));
                AddPathConstraint(cur, constraintExpression);
            }
        }

        private bool IsSymbolic(ExplicitActiveState cur, out Expression expression)
        {
            return _value.TryGetExpression(cur, out expression);
        }
        protected abstract Expression CreatePathConstraintExpression(Expression valueExpression, bool willBranch);
    }

    public abstract class BinaryPathConstraintProducerInstructionExtension : PathConstraintProducerInstructionExtension
    {
        private IDataElement _lhs;
        private IDataElement _rhs;

        protected IDataElement Lhs
        {
            get { return _lhs; }
        }
        protected IDataElement Rhs
        {
            get { return _rhs; }
        }


        public override void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            _rhs = cur.EvalStack[cur.EvalStack.Length - 1];
            _lhs = cur.EvalStack[cur.EvalStack.Length - 2];
        }

        public override void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            // initialize 
            base.PostExecute(instruction, cur, retValue);

            if (IsSymbolic(cur, out Expression lhsExpr, out Expression rhsExpr))
            {
                // we may branch based on a symbolic value => add the path constraint
                Expression constraintExpression = CreatePathConstraintExpression(lhsExpr, rhsExpr, WillBranch(retValue));
                AddPathConstraint(cur, constraintExpression);
            }
        }

        private bool IsSymbolic(ExplicitActiveState cur, out Expression lhsExpr, out Expression rhsExpr)
        {
            bool isLhsSymbolic = _lhs.TryGetExpression(cur, out lhsExpr);
            bool isRhsSymbolic = _rhs.TryGetExpression(cur, out rhsExpr);

            if (isRhsSymbolic || isLhsSymbolic)
            {
                lhsExpr ??= _lhs.AsExpression();
                rhsExpr ??= _rhs.AsExpression();

                return true;
            }

            return false;
        }

        protected abstract Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch);
    }

    
    public static class PathConstraintProducerFactoryExtensions
    {
        public static ExtendableInstructionFactory AddPathConstraintProducers(this ExtendableInstructionFactory factory)
        {
            factory.RegisterExtension(new BRFALSE_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BRTRUE_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BEQ_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BGE_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BGT_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BLE_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BLT_PathConstraintProducerInstructionExtension());
            factory.RegisterExtension(new BNE_PathConstraintProducerInstructionExtension());
            return factory;
        }
    }

    public class BRFALSE_PathConstraintProducerInstructionExtension : UnaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression valueExpression, bool willBranch)
        {
            return willBranch ? Expression.MakeUnary(ExpressionType.Not, valueExpression, typeof(bool)) : valueExpression;
        }

        public override bool CanExecute(Code code)
        {
            return code == Code.Brfalse || code == Code.Brfalse_S;
        }
    }
    public class BRTRUE_PathConstraintProducerInstructionExtension : UnaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression valueExpression, bool willBranch)
        {
            return willBranch ? valueExpression : Expression.MakeUnary(ExpressionType.Not, valueExpression, typeof(bool));
        }

        public override bool CanExecute(Code code)
        {
            return code == Code.Brtrue || code == Code.Brtrue_S;
        }
    }
    public class BEQ_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.Equal : ExpressionType.NotEqual, lhsExpression, rhsExpression);
        }

        public override bool CanExecute(Code code)
        {
            return code == Code.Beq || code == Code.Beq_S;
        }
    }
    public class BGE_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.GreaterThanOrEqual : ExpressionType.LessThan, lhsExpression, rhsExpression);
        }

        public override bool CanExecute(Code code)
        {
            switch(code)
            {
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                    return true;
                default:
                    return false;
            }
        }
    }
    public class BGT_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.GreaterThan : ExpressionType.LessThanOrEqual, lhsExpression, rhsExpression);
        }

        public override bool CanExecute(Code code)
        {
            switch(code)
            {
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                    return true;
                default:
                    return false;
            }
        }
    }
    public class BLE_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.LessThanOrEqual : ExpressionType.GreaterThan, lhsExpression, rhsExpression);
        }

        public override bool CanExecute(Code code)
        {
            switch(code)
            {
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                    return true;
                default:
                    return false;
            }
        }
    }
    public class BLT_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.LessThan : ExpressionType.GreaterThanOrEqual, lhsExpression, rhsExpression);
        }

        public override bool CanExecute(Code code)
        {
            switch(code)
            {
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                    return true;
                default:
                    return false;
            }
        }
    }
    public class BNE_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.NotEqual : ExpressionType.Equal, lhsExpression, rhsExpression);
        }

        public override bool CanExecute(Code code)
        {
            return code == Code.Bne_Un || code == Code.Bne_Un_S;
        }
    }
}

