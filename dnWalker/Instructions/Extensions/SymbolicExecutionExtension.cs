//using dnlib.DotNet.Emit;

//using dnWalker.Symbolic;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Instructions.Extensions
//{
//    /// <summary>
//    /// This extensions handles CLT CGT and CEQ symbolic execution.
//    /// </summary>
//    public abstract class SymbolicExecutionExtension : IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
//    {
//        public abstract bool CanExecute(Code code);

//        /// <summary>
//        /// Gets the expression type which is created by this extension.
//        /// </summary>
//        public abstract ExpressionType ExpressionType { get; }

//        IDataElement _lhs;
//        IDataElement _rhs;

//        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
//        {
//            _rhs = cur.EvalStack[cur.EvalStack.Length - 1];
//            _lhs = cur.EvalStack[cur.EvalStack.Length - 2];
//        }


//        public void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
//        {
//            bool isRhsSymbolic = _rhs.TryGetExpression(cur, out Expression rhsExpr);
//            bool isLhsSymbolic = _lhs.TryGetExpression(cur, out Expression lhsExpr);
            
//            if (isRhsSymbolic || isLhsSymbolic)
//            {
//                IDataElement result = cur.EvalStack.Peek();

//                result.SetExpression(Expression.MakeBinary(ExpressionType, lhsExpr ?? _lhs.AsExpression(), rhsExpr ?? _rhs.AsExpression()), cur);
//            }
//        }
//    }

//    public class CGT_SymbolicExecutionExtension : SymbolicExecutionExtension
//    {
//        public override bool CanExecute(Code code)
//        {
//            return code == Code.Cgt || code == Code.Cgt_Un;
//        }

//        public override ExpressionType ExpressionType
//        {
//            get { return ExpressionType.GreaterThan; }
//        }
//    }

//    public class CLT_SymbolicExecutionExtension : SymbolicExecutionExtension
//    {
//        public override bool CanExecute(Code code)
//        {
//            return code == Code.Clt || code == Code.Clt_Un;
//        }

//        public override ExpressionType ExpressionType
//        {
//            get { return ExpressionType.LessThan; }
//        }
//    }

//    public class CEQ_SymbolicExecutionExtension : SymbolicExecutionExtension
//    {
//        public override bool CanExecute(Code code)
//        {
//            return code == Code.Ceq;
//        }

//        public override ExpressionType ExpressionType
//        {
//            get { return ExpressionType.Equal; }
//        }
//    }
//}
