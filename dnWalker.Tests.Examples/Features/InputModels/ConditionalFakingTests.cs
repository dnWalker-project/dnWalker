using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Input;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples.Features.InputModels
{
    public class ConditionalFakingTests : ExamplesTestBase
    {
        public ConditionalFakingTests(ITestOutputHelper output) : base(output)
        {
        }

        [ExamplesTest]
        public void InvokeAbstractMethodOnAbstractClass(BuildInfo buildInfo)
        {
            IExplorer explorer = CreateExplorer(buildInfo);

            MethodDef entryPoint = DefinitionProvider.GetMethodDefinition("Examples.Concolic.Features.Interfaces.MethodsWithInterfaceParameter.InvokeInterfaceMethodWithArgs");
            
            TypeDef iMyInterfaceTD = DefinitionProvider.GetTypeDefinition("Examples.Concolic.Features.Interfaces.IMyInterface");
            MethodDef theMethod = iMyInterfaceTD.FindMethod("AbstractMethodWithArgs");

            Expression a1Expr = Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Int32, "a1"));

            UserModel model = new UserModel()
            {
                MethodArguments =
                {
                    [entryPoint.Parameters[0]] = new UserObject()
                    {
                        MethodBehaviors =
                        {
                            [theMethod] = new MethodBehavior()
                            {
                                Results =
                                {
                                    new ConditionalResult()
                                    {
                                        Condition = Expression.MakeLessThan(a1Expr, Expression.MakeConstant(DefinitionProvider.BaseTypes.Int32, 5l)),
                                        Result = new UserLiteral("5")
                                    },
                                    new ConditionalResult()
                                    {
                                        Condition = Expression.MakeGreaterThanOrEqual(a1Expr, Expression.MakeConstant(DefinitionProvider.BaseTypes.Int32, 5l)),
                                        Result = new UserLiteral("10")
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = explorer.Run(entryPoint, new UserModel[] { model });
            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("instance.AbstractMethodWithArgs == 5, because the argument is less than 5\r\ninstance.AbstractMethodWithArgs == 10, because the argument is greater than or equal to 5");
        }

        [ExamplesTest]
        public void InvokeAbstractMethodOnAbstractClassWithDynamicConstraint(BuildInfo buildInfo)
        {
            IExplorer explorer = CreateExplorer(buildInfo);

            MethodDef entryPoint = DefinitionProvider.GetMethodDefinition("Examples.Concolic.Features.Interfaces.MethodsWithInterfaceParameter.InvokeInterfaceMethodWithArgs");

            TypeDef iMyInterfaceTD = DefinitionProvider.GetTypeDefinition("Examples.Concolic.Features.Interfaces.IMyInterface");
            MethodDef theMethod = iMyInterfaceTD.FindMethod("AbstractMethodWithArgs");

            Expression a1Expr = Expression.MakeVariable(new NamedVariable(DefinitionProvider.BaseTypes.Int32, "a1"));

            UserModel model = new UserModel()
            {
                MethodArguments =
                {
                    [entryPoint.Parameters[0]] = new UserObject()
                    {
                        MethodBehaviors =
                        {
                            [theMethod] = new MethodBehavior()
                            {
                                Results =
                                {
                                    new ConditionalResult()
                                    {
                                        Condition = Expression.MakeLessThan(a1Expr, Expression.MakeConstant(DefinitionProvider.BaseTypes.Int32, 5l)),
                                        Result = new UserLiteral("5")
                                    },
                                    new ConditionalResult()
                                    {
                                        Condition = Expression.MakeGreaterThanOrEqual(a1Expr, Expression.MakeConstant(DefinitionProvider.BaseTypes.Int32, 5l)),
                                        Result = new UserLiteral("10")
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = explorer.Run(entryPoint, new UserModel[] { model });
            result.Iterations.Should().HaveCount(1);
            result.Iterations[0].Output.Trim().Should().Be("instance.AbstractMethodWithArgs == 5, because the argument is less than 5\r\ninstance.AbstractMethodWithArgs == 10, because the argument is greater than or equal to 5");
        }
    }
}
