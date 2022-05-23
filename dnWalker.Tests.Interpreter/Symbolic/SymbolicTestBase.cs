using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Interpreter.Symbolic
{
    public abstract class SymbolicTestBase : InterpreterTestBase
    {
        protected struct SymbolicResult
        {
            public Constraint? PathConstraint;
            public Expression? ResultExpression;
        }

        protected SymbolicTestBase(ITestOutputHelper output) : base(output)
        {
        }

        protected SymbolicResult Test(string methodName, object[] args, string[] argsNames)
        {
            var explorer = Utils.GetModelChecker(GetFullMethodName(methodName), cur => CreateArguments(cur, args, argsNames), DefinitionProvider);

            // setup symbolic execution & path constraint generation
            explorer.ActiveState.InitializeConcolicExploration(explorer.ActiveState.CurrentMethod.Definition, new FirstNPaths(1));

            explorer.Run();

            SymbolicResult result = new SymbolicResult();

            return result;
        }

        private static DataElementList CreateArguments(ExplicitActiveState cur, object[] args, string[] names)
        {
            DataElementList argsDE = cur.StorageFactory.CreateList(args.Length);
            for (int i = 0; i < args.Length; ++i)
            {
                argsDE[i] = DataElement.CreateDataElement(args[i], cur.DefinitionProvider);
                if (!string.IsNullOrWhiteSpace(names[i]))
                {
                    Expression argExpr = CreateExpression(cur.DefinitionProvider, args[i].GetType(), names[i]);
                    argsDE[i].SetExpression(cur, argExpr);
                }
            }
            return argsDE;
        }

        private static Expression CreateExpression(IDefinitionProvider definitionProvider, Type type, string name)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Double: return NamedDouble(definitionProvider, name);
                case TypeCode.Single: return NamedSingle(definitionProvider, name);
                case TypeCode.Boolean: return NamedBoolean(definitionProvider, name);

                case TypeCode.SByte: return NamedByte(definitionProvider, name);
                case TypeCode.Byte: return NamedSByte(definitionProvider, name);
                case TypeCode.Int16: return NamedShort(definitionProvider, name);
                case TypeCode.UInt16: return NamedUShort(definitionProvider, name);
                case TypeCode.Int32: return NamedInt(definitionProvider, name);
                case TypeCode.UInt32: return NamedUInt(definitionProvider, name);
                case TypeCode.Int64: return NamedLong(definitionProvider, name);
                case TypeCode.UInt64: return NamedULong(definitionProvider, name);
            }
            throw new NotSupportedException("Unsupported type.");
        }

        private static Expression NamedUInt(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.UInt32, name));
        }

        private static Expression NamedUShort(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.UInt16, name));
        }

        private static Expression NamedShort(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Int16, name));
        }

        private static Expression NamedSByte(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.SByte, name));
        }

        private static Expression NamedByte(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Byte, name));
        }

        private static Expression NamedSingle(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Single, name));
        }

        private static Expression NamedULong(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.UInt64, name));
        }

        private static Expression NamedLong(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Int64, name));
        }

        private static Expression NamedBoolean(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Boolean, name));
        }
        private static Expression NamedDouble(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Double, name));
        }
        private static Expression NamedInt(IDefinitionProvider definitionProvider, string name)
        {
            return Expression.MakeVariable(new NamedVariable(definitionProvider.BaseTypes.Int32, name));
        }
    }
}
