using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static dnWalker.TestGenerator.Templates.TemplateHelpers;

namespace dnWalker.TestGenerator.Templates
{
    public readonly struct StaticMethodInvocation : ICodeExpression
    {
        private readonly MethodInvocationData _data;

        public StaticMethodInvocation(MethodInvocationData data)
        {
            _data = data;
        }

        public void WriteTo(StringBuilder output)
        {
            // lets say we want to write following invocations:
            // MyClass.MyMethod(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
            // MyClass<T1>.MyMethod(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
            // MyClass<T1>.MyMethod<T2>(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
            // MyClass.MyMethod<T2>(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)

            MethodInfo method = _data.Method;
            Type declaringType = method.DeclaringType ?? throw new Exception("Cannot access the declaring type!");

            new TypeName(declaringType).WriteTo(output);
            output.Append(".");

            //if (declaringType.IsGenericType)
            //{
            //    // cases 2) and 3)

            //    Type[] genericParameters = declaringType.GetGenericArguments();
            //    // we assume that all used namespaces are imported
            //    output.AppendFormat("{0}<{1}>.", 
            //        WithoutGenerics(GetTypeNameOrAlias(declaringType)), 
            //        string.Join(Coma, genericParameters.Select(gp => WithoutGenerics(GetTypeNameOrAlias(gp)))));
            //}
            //else
            //{
            //    output.AppendFormat("{0}.", GetTypeNameOrAlias(declaringType));
            //}

            // now we have written:
            // MyClass.
            // MyClass<T>.
            // MyClass<T>.
            // MyClass.
            if (method.IsGenericMethod)
            {
                // cases 3) and 4)

                Type[] genericParameters = method.GetGenericArguments();
                // we assume that all used namespaces are imported
                output.AppendFormat("{0}<", method.Name);

                JoinAndWriteExpressions(output, Coma, genericParameters.Select(t => new TypeName(t)));

                //new TypeName(genericParameters[0]).WriteTo(output);
                //for (int i = 1; i < genericParameters.Length; ++i)
                //{
                //    output.Append(Coma);
                //    new TypeName(genericParameters[i]).WriteTo(output);
                //}

                output.Append(">(");
            }
            else
            {
                // cases 1) and 2)
                output.AppendFormat("{0}(", WithoutGenerics(method.Name));
            }

            // now we have written:
            // MyClass.MyMethod(
            // MyClass<T>.MyMethod(
            // MyClass<T>.MyMethod<T2>(
            // MyClass.MyMethod<T2>(
            if (_data.PositionalArguments.Length > 0)
            {
                JoinAndWriteExpressions(output, Coma, _data.PositionalArguments);
            }

            if (_data.PositionalArguments.Length > 0 &&
                _data.OptionalArguments.Length > 0)
            {
                output.Append(Coma);
            }

            // now we have written:
            // MyClass.MyMethod(arg1, arg2
            // MyClass<T>.MyMethod(arg1, arg2
            // MyClass<T>.MyMethod<T2>(arg1, arg2
            // MyClass.MyMethod<T2>(arg1, arg2
            if (_data.OptionalArguments.Length > 0)
            {
                JoinAndWriteExpressions(output, Coma, _data.OptionalArguments);
            }

            output.Append(")");

            // now we have written:
            // MyClass.MyMethod(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
            // MyClass<T1>.MyMethod(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
            // MyClass<T1>.MyMethod<T2>(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
            // MyClass.MyMethod<T2>(arg1, arg2, optionalArg1: arg3, optionalArg2: arg4)
        }
    }
}
