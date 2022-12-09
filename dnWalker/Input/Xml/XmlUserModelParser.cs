using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Expressions.Parsing;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace dnWalker.Input.Xml
{
    public class XmlUserModelParser
    {
        private readonly IDefinitionProvider _definitionProvider;
        private readonly Dictionary<MethodDef, ExpressionParser> _methodConditionParsers = new Dictionary<MethodDef, ExpressionParser>(MethodEqualityComparer.CompareDeclaringTypes);
        private readonly ExpressionFactory _expressionFactory;

        private ExpressionParser GetParser(MethodDef methodDef)
        {
            if (!_methodConditionParsers.TryGetValue(methodDef, out ExpressionParser parser))
            {
                Dictionary<string, TypeSig> vars = new Dictionary<string, TypeSig>(
                    methodDef.Parameters
                        .Where(p => !p.IsReturnTypeParameter)
                        .Select(p =>
                        {
                            if (p.IsHiddenThisParameter)
                            {
                                return KeyValuePair.Create("this", p.Type);
                            }
                            else //if (p.IsNormalMethodParameter)
                            {
                                return KeyValuePair.Create(p.Name, p.Type);
                            }
                        }));

                parser = new ExpressionParser(vars, _expressionFactory);
                _methodConditionParsers.Add(methodDef, parser);
            }
            return parser;
        }

        public XmlUserModelParser(IDefinitionProvider definitionProvider, ExpressionFactory expressionFactory = null)
        {
            _expressionFactory = expressionFactory ?? new CustomModuleExpressionFactory(definitionProvider.Context.MainModule);
            _definitionProvider = definitionProvider;
        }

        public IList<UserModel> ParseModelCollection(XElement xml)
        {
            Dictionary<string, UserData> sharedData = new Dictionary<string, UserData>();

            XElement sharedDataXml = xml.Element(XmlTokens.SharedData);
            if (sharedDataXml != null)
            {
                foreach (XElement sharedXml in sharedDataXml.Elements())
                {
                    UserData userData = ParseUserData(sharedXml, sharedData);

                    Debug.Assert(userData != null && !String.IsNullOrWhiteSpace(userData.Id));

                    //sharedData.Add(userData.Id, userData);
                }
            }

            List<UserModel> userModels = new List<UserModel>();
            foreach (XElement modelXml in xml.Elements(XmlTokens.UserModel))
            {
                UserModel model = ParseModel(modelXml, sharedData);

                Debug.Assert(model != null);

                userModels.Add(model);
            }

            return userModels;
        }

        public UserModel ParseModel(XElement xml, IReadOnlyDictionary<string, UserData> sharedData = null)
        {
            UserModel userModel = sharedData == null ? new UserModel() : new UserModel(sharedData);

            IDictionary<string, UserData> references = userModel.Data;

            string methodName = xml.Attribute(XmlTokens.EntryPoint)?.Value ?? throw new Exception("UserModel XML must have attribute EntryPoint");
            
            MethodDef method = _definitionProvider.GetMethodDefinition(methodName);
            userModel.Method = method;


            //foreach (XElement argXml in xml.Elements(XmlTokens.Argument))
            //{
            //    string name = argXml.Attribute(XmlTokens.Name)?.Value ?? throw new Exception("Argument XMl must have attribute name.");
            //    Parameter parameter = method.Parameters.First(p => p.Name == name);

            //    userModel.MethodArguments[parameter] = ParseUserData(argXml.Elements().First(), userModel.Data);
            //}

            // TODO handle all kinds of static data...

            foreach (IGrouping<string, XElement> varGroup in xml.Elements().GroupBy(varXml => varXml.Name.LocalName))
            {
                string varName = varGroup.Key;
                int nextInvocation = 0;
                foreach (XElement varXml in varGroup)
                {
                    if (!TrySetupAsMethodArgument(varName, varXml) &&
                        !TrySetupAsStaticField(varName, varXml))
                    // !TrySetupAsStaticMethod(varName, varXml) &&
                    // !TrySetupAsStaticProperty(varName, varXml))
                    {
                        throw new Exception($"Could not match any variable for '{varName}'");
                    }
                }
            }

            return userModel;

            bool TrySetupAsMethodArgument(string varName, XElement varXml)
            {
                if (varName == "m-this")
                {
                    if (!method.HasThis)
                    {
                        throw new Exception("Trying to set 'this' parameter for method without hidden 'this' parameter.");
                    }

                    Parameter thisParameter = method.Parameters[0];

                    userModel.MethodArguments[thisParameter] = ParseUserDataFromValue(varXml, references);

                    return true;
                }
                else
                {
                    Parameter parameter = method.Parameters.FirstOrDefault(p => p.Name == varName);
                    if (parameter != null)
                    {
                        userModel.MethodArguments[parameter] = ParseUserDataFromValue(varXml, references, parameter.Type);
                        return true;
                    }
                }
                return false;
            }

            bool TrySetupAsStaticField(string varName, XElement varXml)
            {
                string fullFieldName = varName.Replace('-', '/');

                int lastDot = fullFieldName.LastIndexOf(".");
                string fieldTypeName = fullFieldName.Substring(0, lastDot);

                TypeDef theType = _definitionProvider.GetTypeDefinition(fieldTypeName);
                if (theType != null)
                {
                    string fieldName = fullFieldName.Substring(lastDot + 1);
                    FieldDef fd = theType.FindField(fieldName);
                    if (fd != null)
                    {
                        userModel.StaticFields[fd] = ParseUserDataFromValue(varXml, references);
                        return true;
                    }
                }

                return false;
            }
        }


        private UserData ParseUserData(XElement xml, IDictionary<string, UserData> references, TypeSig expectedType = null)
        {
            UserData userData = xml.Name.LocalName switch
            {
                XmlTokens.Reference => ParseReference(xml, references, expectedType),
                XmlTokens.Object => ParseObject(xml, references, expectedType),
                XmlTokens.Array => ParseArray(xml, references, expectedType),
                XmlTokens.Literal => ParseLiteral(xml, references, expectedType),
                _ => null
            };

            if (userData != null)
            {
                string id = xml.Attribute(XmlTokens.Id)?.Value;
                if (!string.IsNullOrWhiteSpace(id))
                {
                    userData.Id = id;
                    references[id] = userData;
                }

                return userData;
            }
            throw new NotSupportedException($"Unsupported xml: {xml}");
        }

        private UserData ParseUserDataFromValue(XElement xml, IDictionary<string, UserData> references, TypeSig expectedType = null)
        {
            if (xml.HasElements)
            {
                return ParseUserData(xml.Elements().First(), references, expectedType);
            }
            else
            {
                return new UserLiteral() { Value = xml.Value.Trim() };
            }
        }

        private UserReference ParseReference(XElement xml, IDictionary<string, UserData> references, TypeSig expectedType = null)
        {
            return new UserReference() { Reference = xml.Value.Trim() };
        }

        private UserObject ParseObject(XElement xml, IDictionary<string, UserData> references, TypeSig expectedType = null)
        {
            TypeSig type = GetType(xml, _definitionProvider) ?? expectedType ?? throw new InvalidOperationException("Object must have defined type.");

            UserObject uObject = new UserObject()
            {
                Type = type
            };

            TypeDef td = type.ToTypeDefOrRef().ResolveTypeDefThrow();

            //foreach(IGrouping<string, XElement> memberGroup in xml.Elements(XmlTokens.Member).GroupBy(mXml => (string)mXml.Attribute(XmlTokens.Name)))
            foreach(IGrouping<string, XElement> memberGroup in xml.Elements().GroupBy(mXml => mXml.Name.LocalName))
            {
                string memberName = memberGroup.Key;
                int nextInvocation = 0;

                foreach (XElement memberXml in memberGroup)
                {
                    if (!TrySetupAsField(memberName, memberXml) &&
                        !TrySetupAsMethod(memberName, memberXml) &&
                        !TrySetupAsProperty(memberName, memberXml))
                    {
                        throw new Exception($"Member '{memberName}' could not have been matched up to any field, method or property of '{type}'.");
                    }
                }



                bool TrySetupAsField(string memberName, XElement memberXml)
                {
                    FieldDef fd = td.FindField(memberName);
                    if (fd != null)
                    {
                        uObject.Fields[fd] = ParseUserDataFromValue(memberXml, references);
                        return true;
                    }
                    return false;
                }

                bool TrySetupAsProperty(string memberName, XElement memberXml)
                {
                    PropertyDef pd = td.FindProperty(memberName);
                    if (pd != null)
                    {
                        // ignore inner working && ignore setter
                        int invocation = GetAndUpdateCounter(ref nextInvocation, GetInvocation(memberXml));
                        UserData memberValue = ParseUserDataFromValue(memberXml, references);
                        uObject.MethodResults[(pd.GetMethod, invocation)] = memberValue;
                        return true;
                    }
                    return false;
                }

                bool TrySetupAsMethod(string memberName, XElement memberXml)
                {
                    MethodDef md = td.FindMethod(memberName);
                    if (md != null)
                    {
                        UserData memberValue = ParseUserDataFromValue(memberXml, references);

                        int invocation = GetInvocation(memberXml);
                        if (invocation < 0)
                        {
                            string conditionString = GetConditionString(memberXml);
                            if (conditionString != null)
                            {
                                ExpressionParser parser = GetParser(md);
                                Expression condition = parser.Parse(conditionString) ?? _expressionFactory.MakeBooleanConstant(true);

                                uObject.AddMethodBehavior(md, condition, memberValue);
                                return true;
                            }
                        }

                        invocation = GetAndUpdateCounter(ref nextInvocation, invocation);
                        uObject.AddMethodResult(md, invocation, memberValue);
                        return true;
                    }
                    return false;
                }
            }


            return uObject;
        }


        private UserArray ParseArray(XElement xml, IDictionary<string, UserData> references, TypeSig expectedType = null)
        {
            UserArray array = new UserArray()
            {
                ElementType = GetElementType(xml, _definitionProvider) ?? expectedType?.Next,
            };

            int nextIndex = 0;
            foreach (XElement elementXml in xml.Elements())
            {
                int index = GetAndUpdateCounter(ref nextIndex, GetIndex(elementXml));

                UserData elementData = ParseUserData(elementXml, references);
                array.Elements[index] = elementData;
            }

            int length = GetLength(xml);
            if (length == -1)
            {
                length = nextIndex;
            }

            array.Length = length;
            return array;
        }

        private UserLiteral ParseLiteral(XElement xml, IDictionary<string, UserData> references, TypeSig expectedType = null)
        {
            TypeSig type = GetType(xml, _definitionProvider);
            return new UserLiteral() { Value = xml.Value.Trim(), Type = type };
        }

        private static int GetLength(XElement xml)
        {
            string str = xml.Attribute(XmlTokens.Length)?.Value;
            if (str != null) return int.Parse(str);
            return -1;
        }
        private static int GetIndex(XElement xml)
        {
            string str = xml.Attribute(XmlTokens.Index)?.Value;
            if (str != null) return int.Parse(str);
            return -1;
        }

        private static int GetAndUpdateCounter(ref int currentCounter, int counterValue)
        {
            if (counterValue == -1)
            {
                counterValue = currentCounter++;
            }
            else
            {
                if (counterValue <= currentCounter)
                {
                    throw new Exception("Invalid XML, invocations must be rising sequence");
                }
                currentCounter = counterValue + 1;
            }
            return counterValue;
        }

        private static int GetInvocation(XElement xml)
        {
            string str = xml.Attribute(XmlTokens.Invocation)?.Value;
            if (str != null) return int.Parse(str);
            return -1;
        }

        private static string GetConditionString(XElement xml)
        {
            string str = xml.Attribute(XmlTokens.Condition)?.Value;
            return str;
        }


        private static TypeSig GetElementType(XElement xml, IDefinitionProvider definitionProvider)
        {
            string str = xml.Attribute(XmlTokens.ElementType)?.Value;
            if (str != null) return definitionProvider.GetTypeDefinition(str).ToTypeSig();
            return null;
        }
        private static TypeSig GetType(XElement xml, IDefinitionProvider definitionProvider)
        {
            string str = xml.Attribute(XmlTokens.Type)?.Value;
            if (str != null) return definitionProvider.GetTypeDefinition(str).ToTypeSig();
            return null;
        }
    }
}
