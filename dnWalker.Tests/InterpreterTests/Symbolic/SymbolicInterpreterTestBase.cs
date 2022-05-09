using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using FluentAssertions;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

using PathStore = dnWalker.Traversal.PathStore;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class SymbolicInterpreterTestBase : TestBase
    {
        private const string AssemblyFilename = @"..\..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";

        private static Lazy<IDefinitionProvider> Lazy = new Lazy<IDefinitionProvider>(() => new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilename)));

        public SymbolicInterpreterTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, Lazy.Value)
        {
            //_config.StateStorageSize = 5;
            OverrideModelCheckerExplorerBuilderInitialization(c =>
            {
                c.SetStateStorageSize(5);
                ConstraintTreeExplorer tree = new ConstraintTreeExplorer(new AllPathsCoverage());
                tree.GetNextConstraintNode();
                c.AddService(tree);
            });
        }

        private Expression CreateExpression(Type type, string name)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Double: return NamedDouble(name);
                case TypeCode.Single: return NamedSingle(name);
                case TypeCode.Boolean: return NamedBoolean(name);

                case TypeCode.SByte: return NamedByte(name);
                case TypeCode.Byte: return NamedSByte(name);
                case TypeCode.Int16: return NamedShort(name);
                case TypeCode.UInt16: return NamedUShort(name);
                case TypeCode.Int32: return NamedInt(name);
                case TypeCode.UInt32: return NamedUInt(name);
                case TypeCode.Int64: return NamedLong(name);
                case TypeCode.UInt64: return NamedULong(name);
            }
            throw new NotSupportedException("Unsupported type.");
        }

        private Expression NamedUInt(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.UInt32, name));
        }

        private Expression NamedUShort(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.UInt16, name));
        }

        private Expression NamedShort(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Int16, name));
        }

        private Expression NamedSByte(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.SByte, name));
        }

        private Expression NamedByte(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Byte, name));
        }

        private Expression NamedSingle(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Single, name));
        }

        private Expression NamedULong(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.UInt64, name));
        }

        private Expression NamedLong(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Int64, name));
        }

        private Expression NamedBoolean(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Boolean, name));
        }
        private Expression NamedDouble(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Double, name));
        }

        private Expression NamedInt(string name)
        {
            return Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Int32, name));
        }

        protected void Test(string methodName, (object arg, string name)[] args , string pathCondition, string retValueExpression)
        {
            methodName = "dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + methodName;

            IModelCheckerExplorerBuilder builder = GetModelCheckerBuilder();


            builder.SetArgs(cur =>
            {
                IDataElement[] argDE = new IDataElement[args.Length];
                for (int i = 0; i < args.Length; ++i)
                {
                    argDE[i] = DataElement.CreateDataElement(args[i].arg, cur.DefinitionProvider);
                    if (!string.IsNullOrWhiteSpace(args[i].name))
                    {
                        Expression argExpr = CreateExpression(args[i].arg.GetType(), args[i].name);
                        argDE[i].SetExpression(cur, argExpr);
                    }
                }
                return argDE;
            });
            builder.SetMethod(methodName);

            Explorer explorer = builder.Build();
            explorer.Run();

            ExplicitActiveState cur = explorer.cur;
            PathStore pathStore = cur.PathStore;
            IDataElement retValue = cur.CurrentThread.RetValue;


            if (pathCondition != null)
            {
                Constraint currentPathConstraint = cur.Services.GetService<ConstraintTreeExplorer>().Current.GetPrecondition();
                currentPathConstraint.ToString().Should().Be(pathCondition);
            }

            if (retValueExpression != null)
            {
                retValue.TryGetExpression(cur, out Expression retExpression).Should().BeTrue();
                retExpression.ToString().Should().Be(retValueExpression);
            }
            else
            {
                retValue.TryGetExpression(cur, out _).Should().BeFalse();
            }
        }
    }
}
