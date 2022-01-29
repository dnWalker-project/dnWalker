using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteStaticMethodInvocation(MethodInvocationData data)
        {
            MethodInfo method = data.Method;
            Type declaringType = method.DeclaringType ?? throw new Exception("Cannot access the declaring type!");

            WriteTypeName(declaringType);

            Write(TemplateHelpers.Dot);

            if (method.IsGenericMethod)
            {
                Type[] genericParameters = method.GetGenericArguments();
                Write(method.Name);
                Write("<");

                WriteJoint(TemplateHelpers.Coma, genericParameters, WriteTypeName);

                Write(">");
            }
            else
            {
                Write(TemplateHelpers.WithoutGenerics(method.Name));
            }

            Write("(");

            if (data.Arguments.Length > 0)
            {
                WriteJoint(TemplateHelpers.Coma, data.Arguments, a => Write(a.Expression));
            }

            Write(")");
        }

        protected void WriteInstanceMethodInvocation(MethodInvocationData data)
        {
            MethodInfo method = data.Method;

            Write(data.Instance);

            Write(TemplateHelpers.Dot);

            if (method.IsGenericMethod)
            {
                Type[] genericParameters = method.GetGenericArguments();
                Write(method.Name);
                Write("<");

                WriteJoint(TemplateHelpers.Coma, genericParameters, WriteTypeName);

                Write(">");
            }
            else
            {
                Write(TemplateHelpers.WithoutGenerics(method.Name));
            }

            Write("(");

            if (data.Arguments.Length > 0)
            {
                WriteJoint(TemplateHelpers.Coma, data.Arguments, a => Write(a.Expression));
            }

            Write(")");
        }
    }
}
