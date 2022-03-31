using dnWalker.Parameters;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        private void WriteArrayElementAssert(ArrayElementSchema schema)
        {
            // we are given list of changed position within an array (TODO: extend to IItemsOwner)
            // we want to verify that after the method has run, the values are as expected
            // we will write assert for each element
            // !! we assume that the array is not null and has non zero length !!

            IArrayParameter startState = schema.InputState;
            IArrayParameter endState = schema.OutputState;

            //ParameterRef[] startItems = startState.GetItems();
            ParameterRef[] endItems = endState.GetItems();

            TypeSignature elementType = startState.ElementType;

            string arrVar = GetVariableName(startState);
            string defaultLiteral = TemplateHelpers.GetDefaultLiteral(elementType);

            int length = startState.GetLength();

            //if (elementType.IsPrimitive ||
            //    elementType.IsString ||
            //    elementType.IsEnum)
            if (elementType.IsPrimitive)
            {
                for (int i = 0; i < length; ++i)
                {
                    if (endItems[i] == ParameterRef.Empty)
                    {
                        WriteLine($"{arrVar}[{i}].Should().Be({defaultLiteral});");
                    }
                    else
                    {
                        IParameter itemParam = endItems[i].Resolve(endState.Set) ?? throw new Exception("Could not resolve the parameter.");
                        WriteLine($"{arrVar}[{i}].Should().Be({GetExpression(itemParam)});");
                    }
                }
            }
            else if (!elementType.IsValueType)
            {
                for (int i = 0; i < length; ++i)
                {
                    if (endItems[i] == ParameterRef.Empty)
                    {
                        WriteLine($"{arrVar}[{i}].Should().BeNull();");
                    }
                    else
                    {
                        IReferenceTypeParameter itemParam = endItems[i].Resolve<IReferenceTypeParameter>(endState.Set) ?? throw new Exception("Could not resolve the parameter.");
                        if (itemParam.GetIsNull())
                        {
                            // the value is null - check is null
                            WriteLine($"{arrVar}[{i}].Should().BeNull();");
                        }
                        else if (endItems[i].TryResolve(Context.BaseSet, out IParameter? inItemParam))
                        {
                            // the value is a reference to some input parameter - check identity
                            WriteLine($"{arrVar}[{i}].Should().BeSameAs({GetExpression(inItemParam)});");
                        }
                        else
                        {
                            // set during execution, we do not do deep recursive value checks => assert not null
                            WriteLine($"{arrVar}[{i}].Should().NotBeNull();");
                            WriteLine($"{arrVar}[{i}].Should().BeEquivalentTo({GetExpression(itemParam)});");
                        }
                    }
                }
            }
            else
            {
                // composite value type, i.a. struct parameters
            }
        }
    }
}
