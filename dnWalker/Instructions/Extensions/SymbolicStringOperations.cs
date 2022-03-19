using dnlib.DotNet;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public static partial class Extensions
    {
        public static ExtendableInstructionFactory AddSymbolicStringOperations(this ExtendableInstructionFactory factory)
        {
            factory.RegisterExtension(new SymbolicStringOperations());
            return factory;
        }
    }


    public class SymbolicStringOperations : IInstructionExtension, IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
    {
        private interface ISymbolicMethodHandler
        {
            void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur);
            void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue);
        }

        private static readonly Type[] _supportedInstructions = new Type[] { typeof(CALL), typeof(CALLVIRT) };

        private readonly Dictionary<string, ISymbolicMethodHandler> _methodHandlers = new Dictionary<string, ISymbolicMethodHandler>()
        {
            ["op_Equality"] = new StringEqualityHandler(),
            ["get_Length"] = new LengthHandler(),
            ["Concat"] = new ConcatHandler(),
            ["StartsWith"] = new StartsWithHandler(),
            ["EndsWith"] = new EndsWithHandler(),
            ["Substring"] = new SubstringHandler()
        };

        private ISymbolicMethodHandler _currentHandler;

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _supportedInstructions;
            }
        }

        public void PreExecute(MMC.InstructionExec.InstructionExecBase instruction, ExplicitActiveState cur)
        {
            if (instruction is CallInstructionExec callInstruction)
            {
                IMethod method = (IMethod)callInstruction.Operand;

                if (TypeEqualityComparer.Instance.Equals(method.DeclaringType, cur.DefinitionProvider.BaseTypes.String) && 
                    _methodHandlers.TryGetValue(method.Name, out _currentHandler))
                {
                    _currentHandler.PreExecute(callInstruction, cur);
                }
            }
        }

        public void PostExecute(MMC.InstructionExec.InstructionExecBase instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
        {
            if (instruction is CallInstructionExec callInstruction)
            {
                IMethod method = (IMethod)callInstruction.Operand;

                if (TypeEqualityComparer.Instance.Equals(method.DeclaringType, cur.DefinitionProvider.BaseTypes.String) &&
                    _methodHandlers.TryGetValue(method.Name, out var handler))
                {
                    if (!ReferenceEquals(handler, _currentHandler))
                    {
                        throw new Exception("PreExecute and PostExecute handlers do not match.");
                    }

                    _currentHandler.PostExecute(callInstruction, cur, retValue);
                }
            }
        }

        private class StringEqualityHandler : ISymbolicMethodHandler
        {
            ConstantString _lhs;
            ConstantString _rhs;

            public void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur)
            {
                _lhs = (ConstantString)cur.EvalStack.Peek(1);
                _rhs = (ConstantString)cur.EvalStack.Peek(0);
            }

            public void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
            {
                IDataElement result = cur.EvalStack.Peek();

                bool lhsSymbolic = _lhs.TryGetExpression(cur, out Expression lhsExpression);
                bool rhsSymbolic = _rhs.TryGetExpression(cur, out Expression rhsExpression);

                if (lhsSymbolic || rhsSymbolic)
                {
                    result.SetExpression(Expression.Equal(lhsExpression ?? _lhs.AsExpression(), rhsExpression ?? _rhs.AsExpression()), cur);
                }
            }
        }

        private class LengthHandler : ISymbolicMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _get_Length = typeof(string).GetMethod("get_Length");

            ConstantString _theString;

            public void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur)
            {
                _theString = (ConstantString)cur.EvalStack.Peek();
            }

            public void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
            {
                IDataElement result = cur.EvalStack.Peek();

                if (_theString.TryGetExpression(cur, out Expression stringExpression))
                {
                    Expression lengthExpression = Expression.Call(stringExpression, _get_Length);
                    result.SetExpression(lengthExpression, cur);
                }
            }
        }

        private class ConcatHandler : ISymbolicMethodHandler
        {
            private static readonly System.Reflection.MethodInfo _concat = typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) });

            ConstantString _str1;
            ConstantString _str2;

            public void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur)
            {
                // many concat options:
                // - each string as separate argument
                // - strings within an IEnumerable<string>
                //      - this may be veery nasty...
                // - strings within an string[]
                // ...

                // handle only 2 strings for now...

                _str1 = (ConstantString)cur.EvalStack.Peek(1);
                _str2 = (ConstantString)cur.EvalStack.Peek(0);
            }

            public void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
            {
                IDataElement result = cur.EvalStack.Peek();

                bool str1Symbolic = _str1.TryGetExpression(cur, out Expression str1Expression);
                bool str2Symbolic = _str2.TryGetExpression(cur, out Expression str2Expression);

                if (str1Symbolic || str2Symbolic)
                {
                    Expression concat = Expression.Call(null, _concat, str1Expression ?? _str1.AsExpression(), str2Expression ?? _str2.AsExpression());
                    result.SetExpression(concat, cur);
                }
            }
        }

        private class StartsWithHandler : ISymbolicMethodHandler
        {
            // defined for string & char, handle the string only
            private static readonly System.Reflection.MethodInfo _startsWith = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });

            ConstantString _theString;
            ConstantString _prefix;

            public void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur)
            {
                _theString = (ConstantString)cur.EvalStack.Peek(0);
                _prefix = (ConstantString)cur.EvalStack.Peek(1);
            }

            public void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
            {
                bool stringSymbolic = _theString.TryGetExpression(cur, out Expression stringExpression);
                bool prefixSymbolic = _prefix.TryGetExpression(cur, out Expression prefixExpression);

                if (stringSymbolic || prefixSymbolic)
                {
                    Expression startsWith = Expression.Call(stringExpression ?? _theString.AsExpression(), _startsWith, prefixExpression ?? _prefix.AsExpression());
                    cur.EvalStack.Peek(0).SetExpression(startsWith, cur);
                }
            }
        }

        private class EndsWithHandler : ISymbolicMethodHandler
        {
            // defined for string & char, handle the string only
            private static readonly System.Reflection.MethodInfo _endsWith = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

            ConstantString _theString;
            ConstantString _suffix;

            public void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur)
            {
                _theString = (ConstantString)cur.EvalStack.Peek(0);
                _suffix = (ConstantString)cur.EvalStack.Peek(1);
            }

            public void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
            {
                bool stringSymbolic = _theString.TryGetExpression(cur, out Expression stringExpression);
                bool prefixSymbolic = _suffix.TryGetExpression(cur, out Expression suffixExpression);

                if (stringSymbolic || prefixSymbolic)
                {
                    Expression endsWith = Expression.Call(stringExpression ?? _theString.AsExpression(), _endsWith, suffixExpression ?? _suffix.AsExpression());
                    cur.EvalStack.Peek(0).SetExpression(endsWith, cur);
                }
            }
        }

        private class SubstringHandler : ISymbolicMethodHandler
        {
            // defined for offset only & offset + length, handle the latter only
            private static readonly System.Reflection.MethodInfo _substring = typeof(string).GetMethod("Substrins", new Type[] { typeof(int), typeof(int) });

            ConstantString _theString;
            IDataElement _offset;
            IDataElement _length;

            public void PreExecute(CallInstructionExec instruction, ExplicitActiveState cur)
            {
                _theString = (ConstantString)cur.EvalStack.Peek(0);
                _offset = cur.EvalStack.Peek(1);
                _length = cur.EvalStack.Peek(2);
            }

            public void PostExecute(CallInstructionExec instruction, ExplicitActiveState cur, MMC.InstructionExec.IIEReturnValue retValue)
            {
                bool stringSymbolic = _theString.TryGetExpression(cur, out Expression stringExpression);
                bool offsetSymbolic = _offset.TryGetExpression(cur, out Expression offsetExpression);
                bool lengthSymbolic = _length.TryGetExpression(cur, out Expression lengthExpression);

                if (stringSymbolic || offsetSymbolic || lengthSymbolic)
                {
                    stringExpression = stringExpression ?? _theString.AsExpression();
                    offsetExpression = offsetExpression ?? _offset.AsExpression();
                    lengthExpression = lengthExpression ?? _length.AsExpression();

                    Expression substring = Expression.Call(stringExpression, _substring, offsetExpression, lengthExpression);
                    cur.EvalStack.Peek().SetExpression(substring, cur);
                }
            }
        }
    }
}
