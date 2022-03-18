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
        private void WriteObjectFieldAssert(ObjectFieldSchema schema)
        {
            // we are given a list of changed fields within an object (TODO: extend to IFieldOwner)
            // we want to verify that after the method has run, the values are as expected
            // we will write assert for each element
            // !! we assume that the object is not null !!

            IObjectParameter startState = schema.InputState;
            IObjectParameter endState = schema.OutputState;

            IReadOnlyDictionary<string, ParameterRef> endFields = endState.GetFields();

            TypeSignature type = startState.Type;

            string objVar = GetVariableName(startState);

            IReadOnlyParameterSet set = endState.Set;

            foreach (KeyValuePair<string, ParameterRef> f in endFields)
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
                        WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().Be({TemplateHelpers.GetDefaultLiteral(fType)});");
                    }
                    else
                    {
                        IParameter p = pRef.Resolve(set) ?? throw new Exception("Could not resolve the parameter.");
                        WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().Be({GetExpression(p)});");
                    }
                }
                else if (!fType.IsValueType)
                {
                    if (pRef == ParameterRef.Empty)
                    {
                        WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().BeNull();");
                    }
                    else
                    {
                        IReferenceTypeParameter p = pRef.Resolve<IReferenceTypeParameter>(set) ?? throw new Exception("Could not resolve the parameter.");
                        if (p.GetIsNull())
                        {
                            WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().BeNull();");
                        }
                        else if (pRef.TryResolve(Context.BaseSet, out IParameter? baseFParam))
                        {
                            WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().BeSameAs({GetExpression(baseFParam)});");
                        }
                        else
                        {
                            // a value created during runtime && not null
                            WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().NotBeNull();");
                            WriteLine($"{objVar}.GetPrivate(\"{fieldName}\").Should().BeEquivalentTo({GetExpression(p)});");
                        }
                    }
                }
            }
        }
    }
}
