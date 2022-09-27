using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;
using dnWalker.TypeSystem;

using FluentAssertions;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Interpreter.Symbolic
{
    public abstract class SymbolicTestBase : InterpreterTestBase
    {
        protected readonly struct SymbolicResult
        {
            public readonly string? PathConstraint;
            public readonly string? ResultExpression;

            public SymbolicResult(string? pathConstraint, string? resultExpression)
            {
                PathConstraint = pathConstraint;
                ResultExpression = resultExpression;
            }

            public void Deconstruct(out string? pathConstraint, out string? resultEpression)
            {
                pathConstraint = PathConstraint;
                resultEpression = ResultExpression;
            }

            public static SymbolicResult Build(string? pathConstraint, string? resultExpression)
            {
                return new SymbolicResult(pathConstraint, resultExpression);
            }
        }

        protected readonly struct SymbolicArgument
        {
            public readonly object Value;
            public readonly string? Name;

            public static SymbolicArgument[] Build(object[] values, string?[] names)
            {
                Assert.Equal(values.Length, names.Length);

                SymbolicArgument[] ret = new SymbolicArgument[values.Length];
                for (int i = 0; i < values.Length; ++i)
                {
                    ret[i] = new SymbolicArgument(values[i], names[i]);
                }
                return ret;
            }

            public SymbolicArgument(object value, string? name)
            {
                Value = value;
                Name = name;
            }

            public void Deconstruct(out object value, out string? name)
            {
                value = Value;
                name = Name;
            }
        }

        protected SymbolicTestBase(ITestOutputHelper output) : base(output)
        {
        }

        protected SymbolicResult Test(SymbolicArgument[] args, [CallerMemberName]string? methodName = null)
        {
            ArgumentNullException.ThrowIfNull(methodName);

            var explorer = Utils.GetModelChecker(GetFullMethodName(methodName), cur => CreateArguments(cur, args), DefinitionProvider);

            // setup symbolic execution & path constraint generation
            explorer.ActiveState.InitializeConcolicExploration(explorer.ActiveState.CurrentMethod.Definition, new FirstNPaths(1));

            explorer.Run();

            ConstraintTreeExplorer cte = explorer.ActiveState.Services.GetService<ConstraintTreeExplorer>();
            Constraint constraint = cte.Current.Constraint;

            IDataElement resultDE = explorer.ActiveState.CurrentThread.RetValue;

            if (resultDE != null && resultDE.TryGetExpression(explorer.ActiveState, out Expression expr))
            {
                return new SymbolicResult(constraint.ToString(), expr.ToString());
            }
            else
            {
                return new SymbolicResult(constraint.ToString(), null);
            }
        }

        protected void TestAndCompare(SymbolicArgument[] args, SymbolicResult expected, [CallerMemberName]string? methodName = null)
        {
            SymbolicResult actual = Test(args, methodName);
            actual.Should().BeEquivalentTo(expected);
        }

        private static DataElementList CreateArguments(ExplicitActiveState cur, SymbolicArgument[] args)
        {
            DataElementList argsDE = cur.StorageFactory.CreateList(args.Length);
            for (int i = 0; i < args.Length; ++i)
            {
                (object value, string? name) = args[i];

                argsDE[i] = DataElement.CreateDataElement(value, cur.DefinitionProvider);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Expression argExpr = CreateExpression(cur.DefinitionProvider, value.GetType(), name);
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
