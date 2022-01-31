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
        protected void WriteArrangeComplesDepencency(ComplexDependency complexDependency)
        {
            // there should be only:
            // - struct types
            // - interface types
            // - class types
            // - arrays of struct, class, interface types

            // 1. create the instances
            foreach (IParameter p in complexDependency.GetParameters())
            {
                TypeSignature varType = GetVariableType(p);
                string varName = GetVariableName(p);

                if (p is IArrayParameter ap)
                {
                    WriteVariableDeclaration(varType, varName);
                    Write(" = new ");
                    WriteTypeName(ap.ElementType);
                    Write("[");
                    Write(ap.GetLength().ToString());
                    Write("]");
                    WriteLine(TemplateHelpers.Semicolon);
                }
                else if (p is IObjectParameter op)
                {
                    IReadOnlyDictionary<MethodSignature, ParameterRef[]>? methodResults = op.GetMethodResults();

                    if (varType.IsInterface ||
                        varType.IsAbstract ||
                        // at least one method result
                        (methodResults.Count > 0 && !methodResults.Values.All(parr => parr == null || parr.Length == 0)))
                    {
                        string mockVarName = "mock_" + varName;

                        // initialize the mock variable
                        WriteMockTypeName(varType);
                        Write(" ");
                        Write(mockVarName);
                        Write(" = new ");
                        WriteMockTypeName(varType);
                        Write("()"); // TODO: supply with arguments needed for non parameterless constructor
                        WriteLine(TemplateHelpers.Semicolon);

                        WriteVariableDeclaration(varType, varName);
                        Write(" = ");
                        Write(mockVarName);
                        Write(".Object");
                        WriteLine(TemplateHelpers.Semicolon);
                    }
                    else
                    {
                        WriteVariableDeclaration(varType, varName);
                        Write(" = new ");
                        WriteTypeName(varType);
                        Write("()");
                        WriteLine(TemplateHelpers.Semicolon);
                    }
                }
                else if (p is IStructParameter sp)
                {
                    throw new NotImplementedException("Implement the struct parameter");
                }
            }

            // 2. initialize the instances
            foreach (IParameter p in complexDependency.GetParameters())
            {
                TypeSignature varType = GetVariableType(p);
                string varName = GetVariableName(p);

                if (p is IArrayParameter ap)
                {
                    string defaultLiteral = TemplateHelpers.GetDefaultLiteral(ap.ElementType);
                    ParameterRef[] items = ap.GetItems();
                    for (int i = 0; i < items.Length; ++i)
                    {
                        ParameterRef pr = items[i];
                        if (pr == ParameterRef.Empty)
                        {
                            continue;
                        }

                        IParameter parameter = pr.Resolve(ap.Set) ?? throw new Exception("Could not resolve the item parameter.");
                        Write(varName);
                        Write("[");
                        Write(i.ToString());
                        Write("] = ");
                        Write(GetExpression(parameter));
                        WriteLine(TemplateHelpers.Semicolon);
                    }

                }
                else if (p is IObjectParameter op)
                {
                    IReadOnlyDictionary<MethodSignature, ParameterRef[]>? methodResults = op.GetMethodResults();
                    IReadOnlyDictionary<string, ParameterRef> fields = op.GetFields();

                    string mockVarName = "mock_" + varName;
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

                        WriteJoined(TemplateHelpers.Coma, method.Parameters,
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
                else if (p is IStructParameter sp)
                {
                    throw new NotImplementedException("Implement the struct parameter");
                }
            }
        }
    }
}
