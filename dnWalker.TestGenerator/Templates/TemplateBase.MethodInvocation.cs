using dnlib.DotNet;

using dnWalker.TypeSystem;

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
        protected void WriteMethodInvocation(MethodInvocationData data)
        {
            if (data.Instance == null) WriteStaticMethodInvocation(data);
            else WriteInstanceMethodInvocation(data);
        }

        protected void WriteStaticMethodInvocation(MethodInvocationData data)
        {
            MethodSignature method = data.Method;

            WriteTypeName(method.DeclaringType);

            Write(TemplateHelpers.Dot);

            if (method.IsGenericInstance)
            {
                TypeSignature[] genericParameters = method.GetGenericParameters();
                Write(method.Name);
                Write("<");

                WriteJoined(TemplateHelpers.Coma, genericParameters, WriteTypeName);

                Write(">");
            }
            else
            {
                Write(TemplateHelpers.WithoutGenerics(method.Name));
            }

            Write("(");

            if (data.Arguments.Length > 0)
            {
                WriteJoined(TemplateHelpers.Coma, data.Arguments, a => Write(a.Expression));
            }

            Write(")");
        }

        protected void WriteInstanceMethodInvocation(MethodInvocationData data)
        {
            MethodSignature method = data.Method;

            Write(data.Instance);

            Write(TemplateHelpers.Dot);

            if (method.IsGenericInstance)
            {
                TypeSignature[] genericParameters = method.GetGenericParameters();
                Write(method.Name);
                Write("<");

                WriteJoined(TemplateHelpers.Coma, genericParameters, WriteTypeName);

                Write(">");
            }
            else
            {
                Write(TemplateHelpers.WithoutGenerics(method.Name));
            }

            Write("(");

            if (data.Arguments.Length > 0)
            {
                WriteJoined(TemplateHelpers.Coma, data.Arguments, a => Write(a.Expression));
            }

            Write(")");
        }
    }
}
