﻿using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public abstract class SymbolicBinaryExecutionExtension : IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
    {
        public abstract bool CanExecute(Code code);

        /// <summary>
        /// Gets the expression type which is created by this extension.
        /// </summary>
        public abstract ExpressionType ExpressionType { get; }

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

        public virtual void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            _rhs = cur.EvalStack[cur.EvalStack.Length - 1];
            _lhs = cur.EvalStack[cur.EvalStack.Length - 2];
        }


        public virtual void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            if (IsSymbolic(cur, out Expression lhsExpr, out Expression rhsExpr))
            {
                IDataElement result = cur.EvalStack.Peek();

                result.SetExpression(Expression.MakeBinary(ExpressionType, lhsExpr, rhsExpr), cur);
            }
        }

        protected virtual bool IsSymbolic(ExplicitActiveState cur, out Expression lhsExpr, out Expression rhsExpr)
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
    }
    
    public abstract class SymbolicUnaryExecutionExtension : IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
    {
        public abstract bool CanExecute(Code code);

        /// <summary>
        /// Gets the expression type which is created by this extension.
        /// </summary>
        public abstract ExpressionType ExpressionType { get; }

        IDataElement _value;

        public virtual void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            _value = cur.EvalStack[cur.EvalStack.Length - 1];
        }


        public virtual void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            if (_value.TryGetExpression(cur, out Expression valueExpr))
            {
                IDataElement result = cur.EvalStack.Peek();

                result.SetExpression(Expression.MakeUnary(ExpressionType, valueExpr, valueExpr.Type), cur);
            }
        }
    }

    public static class SymbolicExecutionFactoryExtensions
    {
        public static ExtendableInstructionFactory AddSymbolicExecution(this ExtendableInstructionFactory factory)
        {
                factory.RegisterExtension(new CGT_SymbolicExecutionExtension());
                factory.RegisterExtension(new CLT_SymbolicExecutionExtension());
                factory.RegisterExtension(new ADD_SymbolicExecutionExtension());
                factory.RegisterExtension(new DIV_SymbolicExecutionExtension());
                factory.RegisterExtension(new MUL_SymbolicExecutionExtension());
                factory.RegisterExtension(new REM_SymbolicExecutionExtension());
                factory.RegisterExtension(new SUB_SymbolicExecutionExtension());
                factory.RegisterExtension(new NEG_SymbolicExecutionExtension());
                factory.RegisterExtension(new AND_SymbolicExecutionExtension());
                factory.RegisterExtension(new NOT_SymbolicExecutionExtension());
                factory.RegisterExtension(new OR_SymbolicExecutionExtension());
                factory.RegisterExtension(new XOR_SymbolicExecutionExtension());
                factory.RegisterExtension(new SHL_SymbolicExecutionExtension());
                factory.RegisterExtension(new SHR_SymbolicExecutionExtension());
                factory.RegisterExtension(new CEQ_SymbolicExecutionExtension());
     
            return factory;
        }
    
    }
    

    public class CEQ_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Ceq;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Equal; }
        }

        public override void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            // two posibilities
            // 1. (else branch) we are comparing two normal data elements => just do the default magic
            // 2. (if branch)   we are comparing a boolean value with zero (or possibly one if compiler is weird)
            //                  in other word we are doing negation / nothing over the iput value
            //                  only way to verify, which mode of execution is desired is if 
            //                    1) LHS is symbolic and its type is boolean
            //                    2) RHS is not symbolic
            //                  (this method is a bit dirty, but works thus far)
            if (Lhs.TryGetExpression(cur, out Expression lhsExpr) && !Rhs.TryGetExpression(cur, out Expression _) && lhsExpr.Type == typeof(bool))
            {
                IDataElement result = cur.EvalStack[cur.EvalStack.Length - 1];
                if (!Rhs.ToBool())
                {
                    result.SetExpression(Expression.MakeUnary(ExpressionType.Not, lhsExpr, typeof(bool)), cur);
                }
                else
                {
                    // weird compiler
                    result.SetExpression(lhsExpr, cur);
                }
            }
            else
            {
                base.PostExecute(instruction, cur, retValue);
            }
        }
    }

    public class CGT_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Cgt;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.GreaterThan; }
        }
    }
    public class CLT_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Clt;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.LessThan; }
        }
    }
    public class ADD_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Add || code == Code.Add_Ovf_Un;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Add; }
        }
    }
    public class DIV_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Div;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Divide; }
        }
    }
    public class MUL_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Mul;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Multiply; }
        }
    }
    public class REM_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Rem;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Modulo; }
        }
    }
    public class SUB_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Sub;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Subtract; }
        }
    }
    public class NEG_SymbolicExecutionExtension : SymbolicUnaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Neg;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Negate; }
        }
    }
    public class AND_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.And;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.And; }
        }
    }
    public class NOT_SymbolicExecutionExtension : SymbolicUnaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Not;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Not; }
        }
    }
    public class OR_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Or;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.Or; }
        }
    }
    public class XOR_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Xor;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.ExclusiveOr; }
        }
    }
    public class SHL_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Shl;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.LeftShift; }
        }
    }
    public class SHR_SymbolicExecutionExtension : SymbolicBinaryExecutionExtension
    {
        public override bool CanExecute(Code code)
        {
            return code == Code.Shr || code == Code.Shr_Un;
        }

        public override ExpressionType ExpressionType
        {
            get { return ExpressionType.RightShift; }
        }
    }
}
