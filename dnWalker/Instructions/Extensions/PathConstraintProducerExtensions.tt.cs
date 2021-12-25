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
        public abstract IEnumerable<Type> SupportedInstructions { get; }

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
            cur.PathStore.AddPathConstraint(expression, _nextInstruction, cur);
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


    public static partial class Extensions
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

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BRFALSE),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BRTRUE_PathConstraintProducerInstructionExtension : UnaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression valueExpression, bool willBranch)
        {
            return willBranch ? valueExpression : Expression.MakeUnary(ExpressionType.Not, valueExpression, typeof(bool));
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BRTRUE),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BEQ_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.Equal : ExpressionType.NotEqual, lhsExpression, rhsExpression);
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BEQ),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BGE_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.GreaterThanOrEqual : ExpressionType.LessThan, lhsExpression, rhsExpression);
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BGE),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BGT_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.GreaterThan : ExpressionType.LessThanOrEqual, lhsExpression, rhsExpression);
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BGT),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BLE_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.LessThanOrEqual : ExpressionType.GreaterThan, lhsExpression, rhsExpression);
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BLE),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BLT_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.LessThan : ExpressionType.GreaterThanOrEqual, lhsExpression, rhsExpression);
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BLT),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
    public class BNE_PathConstraintProducerInstructionExtension : BinaryPathConstraintProducerInstructionExtension
    {

        protected override Expression CreatePathConstraintExpression(Expression lhsExpression, Expression rhsExpression, bool willBranch)
        {
            return Expression.MakeBinary(willBranch ? ExpressionType.NotEqual : ExpressionType.Equal, lhsExpression, rhsExpression);
        }

        private static readonly Type[] _instructions = new Type[]
        {
                typeof(BNE),
        };

        public override IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }
    }
}

