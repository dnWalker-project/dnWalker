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
        private void WriteReturnValueAssert(ReturnValueSchema schema)
        {
            // two main cases
            // 1) the return parameter is a primitive value =>
            //    we can do the equality check
            // result.Should().Be(LITERAL);

            // 2) the return parameter is a reference type AND is a null =>
            //    we can do the null check
            // result.Should().BeNull();

            // 3) the return parameter is a reference type AND an input AND not null =>
            //    we can do the identity check
            // result.Should().BeSameAs(INPUT_VARIABLE_NAME);

            // 4) the return parameter is a reference type AND is not null AND is not input =>
            //    we can do not null check
            // result.Should().NotBeNull();
            //    + limited field value equality, i.a. identity with input parameters OR equality of primitive values
            //    - how deep should the check go?
            // result.GetPrivate("FIELDNAME").Should().BeSameAs(INPUT_VARIABLE_NAME);
            // result.GetPrivate("FIELDNAME").Should().Be(LITERAL);

            // 5) the return parameter is a value type =>
            //    we can do limited field value equality, i.a. identity with input parameters OR equality of primitive values
            //    how deep should the check go?
            // result.GetPrivate("FIELDNAME").Should().BeSameAs(INPUT_VARIABLE_NAME);
            // result.GetPrivate("FIELDNAME").Should().Be(LITERAL);

            IParameter retParam = schema.ReturnValue;
            ParameterRef reference = retParam.Reference;

            // 1)
            //if (retParam is IPrimitiveValueParameter ||
            //    retParam is IStringParameter ||
            //    retParam is IEnumParameter)
            if (retParam is IPrimitiveValueParameter)
            {
                WriteLine($"result.Should().Be({GetExpression(retParam)});");
                return;
            }

            // 2 - 4)
            if (retParam is IReferenceTypeParameter refParam)
            {
                // 2)
                if (refParam.GetIsNull())
                {
                    WriteLine("result.Should().BeNull();");
                    return;
                }
                else
                {
                    WriteLine("result.Should().NotBeNull();");
                }

                // 3)
                if (Context.BaseSet.Parameters.ContainsKey(reference))
                {
                    WriteLine($"result.Should().BeSameAs({GetExpression(reference.Resolve(Context.BaseSet)!)});");
                    return;
                }

                // 4) the execution set parameter should be arranged... we can do the Should().BeEquivalentTo()....
                WriteLine($"result.Should().BeEquivalentTo({GetExpression(reference.Resolve(Context.ExecutionSet)!)};");
                
                //if (refParam is IArrayParameter arrayParameter)
                //{
                //    WriteItemCheck(arrayParameter);
                //}
                //else if (refParam is IObjectParameter objectParameter)
                //{
                //    WriteFieldCheck(objectParameter);
                //}

            }

            // 5)
            if (retParam is IStructParameter structParameter)
            {
                WriteFieldCheck(structParameter);
            }


            void WriteFieldCheck(IFieldOwnerParameter fieldOwner)
            {
                IReadOnlyDictionary<string, ParameterRef> fields = fieldOwner.GetFields();

                IReadOnlyParameterSet set = fieldOwner.Set;
                TypeSignature type = fieldOwner.Type;

                foreach (KeyValuePair<string, ParameterRef> f in fields)
                {
                    string fieldName = f.Key;
                    TypeSignature fType = type.GetFieldType(fieldName);

                    ParameterRef pRef = f.Value;

                    //if (elementType.IsPrimitive ||
                    //    elementType.IsString ||
                    //    elementType.IsEnum)
                    if (fType.IsPrimitive)
                    {
                        if (pRef == ParameterRef.Empty)
                        {
                            WriteLine($"result.GetPrivate({fieldName}).Should().Be({TemplateHelpers.GetDefaultLiteral(fType)});");
                        }
                        else
                        {
                            IParameter p = pRef.Resolve(set) ?? throw new Exception("Could not resolve the parameter.");
                            WriteLine($"result.GetPrivate({fieldName}).Should().Be({GetExpression(p)});");
                        }
                    }
                    else if (!fType.IsValueType)
                    {
                        if (pRef == ParameterRef.Empty)
                        {
                            WriteLine($"result.GetPrivate({fieldName}).Should().BeNull();");
                        }
                        else
                        {
                            IReferenceTypeParameter p = pRef.Resolve<IReferenceTypeParameter>(set) ?? throw new Exception("Could not resolve the parameter.");
                            if (p.GetIsNull())
                            {
                                WriteLine($"result.GetPrivate({fieldName}).Should().BeNull();");
                            }
                            else if (pRef.TryResolve(Context.BaseSet, out IParameter? baseFParam))
                            {
                                WriteLine($"result.GetPrivate({fieldName}).Should().BeSameAs({GetExpression(baseFParam)});");
                            }
                            else // a value created during runtime && not null
                            {
                                WriteLine($"result.GetPrivate({fieldName}).Should().NotBeNull();");
                            }
                        }
                    }
                }
            }

            void WriteItemCheck(IItemOwnerParameter itemOwner)
            {
                int length = itemOwner.GetLength();

                WriteLine($"result.Should().HaveLength({length});");

                IReadOnlyParameterSet set = itemOwner.Set;

                TypeSignature elementType = itemOwner.ElementType;
                ParameterRef[] items = itemOwner.GetItems();
                string defaultLiteral = TemplateHelpers.GetDefaultLiteral(elementType);

                //if (elementType.IsPrimitive ||
                //    elementType.IsString ||
                //    elementType.IsEnum)
                if (elementType.IsPrimitive)
                {
                    // we can easily verify all elements within the array agains their literals.
                    for (int i = 0; i < length; ++i)
                    {
                        if (items[i] == ParameterRef.Empty)
                        {
                            WriteLine($"result[{i}].Should().Be({defaultLiteral});");
                        }
                        else
                        {
                            WriteLine($"result[{i}].Should().Be({GetExpression(items[i].Resolve(set) ?? throw new Exception("Could not resolve parameter."))});");
                        }
                    }
                }
                else if (!elementType.IsValueType)
                {
                    // we will verify only nulls and input parameters
                    for (int i = 0; i < length; ++i)
                    {
                        if (items[i] == ParameterRef.Empty)
                        {
                            WriteLine($"result[{i}].Should().BeNull();");
                        }
                        else if (items[i].TryResolve(Context.BaseSet, out IParameter? itemP))
                        {
                            WriteLine($"result[{i}].Should().BeSameAs({GetExpression(itemP)});");
                        }
                        else
                        {
                            WriteLine($"result[{i}].Should().NotBeNull();");
                        }
                    }
                }
                else
                {
                    // a composite value type - do we handle this?
                    // using some recursion?
                    // very complex stuff...
                }
            }
        }
    }
}
