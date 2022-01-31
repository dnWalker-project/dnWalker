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
        protected void WriteArrangeSimpleDepencency(SimpleDependency simpleDependency)
        {
            string varName = GetVariableName(simpleDependency.Parameter);
            TypeSignature varType = GetVariableType(simpleDependency.Parameter);

            IParameter p = simpleDependency.Parameter;

            if (p is IPrimitiveValueParameter pp)
            {
                WriteVariableDeclaration(varType, varName);
                Write(" = ");

                Write(GetExpression(pp));
                WriteLine(TemplateHelpers.Semicolon);
            }
            else if (p is IArrayParameter ap)
            {
                if (ap.GetIsNull())
                {
                    WriteVariableDeclaration(varType, varName);
                    Write(" = ");

                    Write(TemplateHelpers.Null);
                }
                else
                {
                    TypeSignature elementType = ap.ElementType.ElementType;

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
                        Write(" { ");
                        WriteJoined(TemplateHelpers.Coma, ap.GetItems().Select(r => GetExpression(r.Resolve(ap.Set) ?? throw new Exception("Could not resolve the parameter."))), Write);
                        Write(" }");
                    }
                }
                WriteLine(TemplateHelpers.Semicolon);
            }
            else if (p is IObjectParameter op)
            {
                if (op.GetIsNull())
                {
                    WriteVariableDeclaration(varType, varName);
                    Write(" = ");

                    Write(TemplateHelpers.Null);
                    WriteLine(TemplateHelpers.Semicolon);
                    return;
                }

                // check 3 possibilities
                // 1) type is an interface || parameter has no fields defined
                //    => create instance using mock, setup using mock.setup
                // 2) type is a class and parameter has some fields && some method results defined
                //    => create instance using mock, setup using mock.setup && private object
                // 3) type is a class and parameter has only fields defined
                //    => create the instance directly, setup using private object

                IReadOnlyDictionary<string, ParameterRef> fields = op.GetFields();
                IReadOnlyDictionary<MethodSignature, ParameterRef[]>? methodResults = op.GetMethodResults();

                if (varType.IsInterface || op.GetFields().Count == 0)
                {
                    // 1) choice
                    string mockVarName = "mock_" + varName;

                    // initialize the mock variable
                    WriteMockTypeName(varType);
                    Write(" ");
                    Write(mockVarName);
                    Write(" = new ");
                    WriteMockTypeName(varType);
                    Write("()"); // TODO: supply with arguments needed for non parameterless constructor
                    WriteLine(TemplateHelpers.Semicolon);

                    // do the set-up
                    foreach (KeyValuePair<MethodSignature, ParameterRef[]> resultInfo in methodResults)
                    {
                        MethodSignature method = resultInfo.Key;

                        TypeSignature[] genericParameters = method.GetGenericParameters();

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
                                    return GetExpression(rp);
                                }
                            })
                            .ToArray();

                        if (resultExprs.Length == 0) continue;

                        Write(mockVarName);
                        Write(".SetupSequence(o => o.");
                        Write(methodName);

                        if (genericParameters.Count() > 0)
                        {
                            Write("<");
                            WriteJoined(TemplateHelpers.Coma, genericParameters, WriteTypeName);
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
                        foreach(string resultExpr in resultExprs)
                        {
                            WriteLine(string.Empty);
                            Write(".Returns(");
                            Write(resultExpr);
                            Write(")");
                        }
                        PopIndent();
                        WriteLine(TemplateHelpers.Semicolon);
                    }


                    // initialize the object variable
                    WriteVariableDeclaration(varType, varName);
                    Write(" = ");
                    Write(mockVarName);
                    Write(".Object");
                    WriteLine(TemplateHelpers.Semicolon);
                }
                else if (varType.IsAbstract || 
                         (fields.Count > 0 &&                                                    // any fields
                          methodResults.Count() > 0 &&                                           // any methods
                          !methodResults.Values.All(parr => parr == null || parr.Length == 0)))  // any method results
                {
                    // 2) choice
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

                        TypeSignature[] genericParameters = method.GetGenericParameters();

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

                    WriteVariableDeclaration(varType, varName);
                    Write(" = ");
                    Write(mockVarName);
                    Write(".Object");
                    WriteLine(TemplateHelpers.Semicolon);


                    // set-up the fields
                    foreach (KeyValuePair<string, ParameterRef> field in fields)
                    {
                        IParameter fieldParameter = field.Value.Resolve(op.Set) ?? throw new Exception("Could not resolve the field parameter");

                        Write(varName);
                        Write(".SetPrivate(");
                        Write(field.Key);
                        Write(TemplateHelpers.Coma);
                        Write(GetExpression(fieldParameter));
                        WriteLine(");");
                    }
                }
                else
                {
                    // 3) choice
                }
            }
        }
    }
}
