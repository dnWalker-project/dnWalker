using dnlib.DotNet;

using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
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
        private protected void WriteArrangeSimpleDepencency(SimpleDependency simpleDependency)
        {
            IParameter p = simpleDependency.Parameter;

            string varName = GetVariableName(p);
            WriteLine($"// Arrange variable: {varName}");


            if (p is IPrimitiveValueParameter pp)
            {
                WriteArrangePrimitiveValueParameter(pp);
            }
            else if (p is IArrayParameter ap)
            {
                WriteArrangeArrayParameter(ap);
            }
            else if (p is IObjectParameter op)
            {
                WriteArrangeObjectParameter(op);
            }
        }

        private void WriteArrangePrimitiveValueParameter(IPrimitiveValueParameter pp)
        {
            string varName = GetVariableName(pp);
            TypeSignature varType = GetVariableType(pp);

            WriteVariableDeclaration(varType, varName);
            Write(" = ");

            Write(GetExpression(pp));
            WriteLine(TemplateHelpers.Semicolon);
        }

        private void WriteArrangeArrayParameter(IArrayParameter ap)
        {
            if (ap.GetIsNull())
            {
                WriteArrangeNull(ap);
            }
            else
            {
                string varName = GetVariableName(ap);
                TypeSignature varType = GetVariableType(ap);

                TypeSignature elementType = ap.ElementType;

                WriteVariableDeclaration(varType, varName);
                Write(" = ");

                // array of primitive values => initialization using constants
                Write("new ");
                WriteTypeName(elementType);
                Write("[");
                Write((ap.GetLength()).ToString());
                Write("]");
                if (ap.GetLength() > 0)
                {
                    string defaultLiteral = TemplateHelpers.GetDefaultLiteral(elementType);

                    Write(" { ");
                    WriteJoined(TemplateHelpers.Coma, ap.GetItems().Select(r => r == ParameterRef.Empty ?
                     defaultLiteral : 
                     GetExpression(r.Resolve(ap.Set) ?? throw new Exception("Could not resolve the parameter."))), Write);
                    Write(" }");
                }
                WriteLine(TemplateHelpers.Semicolon);
            }
        }

        private void WriteArrangeObjectParameter(IObjectParameter op)
        {
            if (op.GetIsNull())
            {
                WriteArrangeNull(op);
                return;
            }

            string varName = GetVariableName(op);
            TypeSignature varType = GetVariableType(op);

            IReadOnlyDictionary<string, ParameterRef> fields = op.GetFields();
            IReadOnlyDictionary<MethodSignature, ParameterRef[]>? methodResults = op.GetMethodResults();

            if (varType.IsInterface || 
                varType.IsAbstract || 
                // at least one method result
                (methodResults.Count > 0 && !methodResults.Values.All(parr => parr == null || parr.Length == 0)))
            {
                WriteArrangeInterfaceAbstractOrMethodsOnly(op);
            }
            else
            {
                WriteArrangeFieldsOnlyAndNotAbstract(op);
            }
        }

        private void WriteArrangeNull(IReferenceTypeParameter rp)
        {
            string varName = GetVariableName(rp);
            TypeSignature varType = GetVariableType(rp);

            WriteVariableDeclaration(varType, varName);
            Write(" = ");

            Write(TemplateHelpers.Null);
            WriteLine(TemplateHelpers.Semicolon);
        }

        private void WriteArrangeInterfaceAbstractOrMethodsOnly(IObjectParameter op)
        {
            string varName = GetVariableName(op);
            TypeSignature varType = GetVariableType(op);

            IReadOnlyDictionary<string, ParameterRef> fields = op.GetFields();
            IReadOnlyDictionary<MethodSignature, ParameterRef[]>? methodResults = op.GetMethodResults();

            string mockVarName = "mock_" + varName;

            // initialize the mock variable
            WriteMockTypeName(varType);
            Write(" ");
            Write(mockVarName);
            Write(" = new ");
            WriteMockTypeName(varType);
            Write("()"); // TODO: supply with arguments needed for non parameterless constructor
            WriteLine(TemplateHelpers.Semicolon);

            // set-up the method results
            foreach (KeyValuePair<MethodSignature, ParameterRef[]> resultInfo in methodResults)
            {
                MethodSignature method = resultInfo.Key;

                //TypeSignature[] genericParameters = method.GetGenericParameters();

                string methodName = method.Name;

                TypeSignature returnType = method.ReturnType;
                string defaultExpr = TemplateHelpers.GetDefaultLiteral(returnType);

                string[] resultExprs = resultInfo.Value
                        .Select(r =>
                        {
                            if (r == ParameterRef.Empty)
                            {
                                    // not specified => return default expression
                                    return defaultExpr;
                            }
                            else
                            {
                                IParameter rp = r.Resolve(op.Set) ?? throw new Exception($"Could not resolve the parameter.");

                                if (rp is IPrimitiveValueParameter primitiveValue)
                                {
                                    return primitiveValue.Value?.ToString() ?? defaultExpr;
                                }
                                else
                                {
                                    return GetVariableName(rp);
                                }
                            }
                        })
                    .ToArray();

                if (resultExprs.Length == 0) continue;

                Write(mockVarName);
                Write(".SetupSequence(o => o.");
                Write(methodName);

                if (method.IsGenericInstance)
                {
                    Write("<");
                    WriteJoined(TemplateHelpers.Coma, method.GetGenericParameters(), WriteTypeName);
                    Write(">");
                }

                Write("(");

                WriteJoined(TemplateHelpers.Coma, method.ParameterTypes,
                    t =>
                    {
                        Write("It.Any<");
                        WriteTypeName(t);
                        Write(">()");
                    });

                Write("))");

                PushIndent("    ");
                foreach (string resultExpr in resultExprs)
                {
                    WriteLine(string.Empty);
                    Write(".Returns(");
                    Write(resultExpr);
                    Write(")");
                }
                PopIndent();
                WriteLine(TemplateHelpers.Semicolon);
            }

            WriteVariableDeclaration(varType, varName);
            Write(" = ");
            Write(mockVarName);
            Write(".Object");
            WriteLine(TemplateHelpers.Semicolon);

            // set-up the fields
            foreach (KeyValuePair<string, ParameterRef> field in fields)
            {
                if (field.Value == ParameterRef.Empty)
                {
                    continue;
                }

                IParameter fieldParameter = field.Value.Resolve(op.Set) ?? throw new Exception("Could not resolve the field parameter");

                Write(varName);
                Write(".SetPrivate(");
                Write("\"");
                Write(field.Key);
                Write("\"");
                Write(TemplateHelpers.Coma);
                Write(GetExpression(fieldParameter));
                Write(")");
                WriteLine(TemplateHelpers.Semicolon);
            }
        }
        private void WriteArrangeFieldsOnlyAndNotAbstract(IObjectParameter op)
        {
            string varName = GetVariableName(op);
            TypeSignature varType = GetVariableType(op);

            IReadOnlyDictionary<string, ParameterRef> fields = op.GetFields();
            IReadOnlyDictionary<MethodSignature, ParameterRef[]>? methodResults = op.GetMethodResults();

            // TODO: use some method to guess the constructor or allow user to inject factory method
            WriteVariableDeclaration(varType, varName);
            Write(" = new ");
            WriteTypeName(varType);
            Write("()");
            WriteLine(TemplateHelpers.Semicolon);

            // set-up the fields
            foreach (KeyValuePair<string, ParameterRef> field in fields)
            {
                if (field.Value == ParameterRef.Empty)
                {
                    continue;
                }

                IParameter fieldParameter = field.Value.Resolve(op.Set) ?? throw new Exception("Could not resolve the field parameter");

                Write(varName);
                Write(".SetPrivate(");
                Write("\"");
                Write(field.Key);
                Write("\"");
                Write(TemplateHelpers.Coma);
                Write(GetExpression(fieldParameter));
                Write(")");
                WriteLine(TemplateHelpers.Semicolon);
            }
        }
    }
}
