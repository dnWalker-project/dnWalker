using dnWalker.Symbolic;
using dnWalker.Symbolic.Variables;

using Microsoft.Z3;

using System.Collections.Generic;

using Z3Model = Microsoft.Z3.Model;
using IVariable = dnWalker.Symbolic.IVariable;
using Model = dnWalker.Symbolic.Model;
using System.Linq;
using System;
using dnlib.DotNet;

using System.Diagnostics;
using dnWalker.Symbolic.Heap;

namespace dnWalker.Z3
{
    public partial class Z3Solver
    {

        private static IModel BuildModel(Constraint constraint, ref SolverContext context)
        {
            Z3Model z3Model = context.Solver.Model;
            Model result = new Model(constraint);
            Dictionary<IVariable, Expr> varLookup = context.VariableMapping;
            Expr nullExpr = context.NullExpr;
            Expr stringNullExpr = context.StringNullExpr;
            Dictionary<Expr, Location> locationMapping = new Dictionary<Expr, Location>();

            // setup the variables into a list
            // sort the variables with regards to their depth
            // that way, root variables will be handled first, their member variables later
            // for example
            // variables: { x.a.b, x, y.z.r.t, x.a, y, y.z.r, y.z }
            // sorted:    { x, y, y.z, x.a, x.a.b, y.z.r, y.z.r.t }
            // ??can we assume that each "prefix" will be among the variables?? - does it matter???
            IVariable[] variables = varLookup.Keys.ToArray();
            Array.Sort(variables, static (v1, v2) => GetDepth(v1).CompareTo(GetDepth(v2)));

            foreach (IVariable variable in variables)
            {
                IValue value = GetValue(variable);

                if (variable is IRootVariable root)
                {
                    result.SetValue(root, value);
                }
                else if (variable is IMemberVariable member)
                {
                    // the parent should already be allocated
                    Location parentLocation = (Location)GetValue(member.Parent);
                    IHeapNode parentNode = result.HeapInfo.GetNode(parentLocation);

                    switch (member)
                    {
                        case InstanceFieldVariable instField:
                            ((IObjectHeapNode)parentNode).SetField(instField.Field, value);
                            break;

                        case MethodResultVariable methodResult:
                            ((IObjectHeapNode)parentNode).SetMethodResult(methodResult.Method, methodResult.Invocation, value);
                            break;

                        case ArrayElementVariable arrayElement:
                            ((IArrayHeapNode)parentNode).SetElement(arrayElement.Index, value);
                            break;

                        case ArrayLengthVariable arrayLength:
                            ((IArrayHeapNode)parentNode).Length = (int)(((PrimitiveValue<uint>)value).Value);
                            break;
                    }
                }
            }

            return result;

            IValue GetValue(IVariable variable)
            {
                Expr z3Var = varLookup[variable];
                Expr? z3Value = z3Model.ConstInterp(z3Var);

                if (z3Value == null)
                {
                    // no interpretation => default
                    return ValueFactory.GetDefault(variable.Type);
                }
                else
                {
                    TypeSig type = variable.Type;
                    if (type.IsString()) return z3Value.Equals(stringNullExpr) ? StringValue.Null : new StringValue(z3Value.String);

                    if (type.IsBoolean()) return new PrimitiveValue<bool>(z3Value.BoolValue == Z3_lbool.Z3_L_TRUE);


                    if (type.IsByte()) return z3Value switch 
                    {  
                        IntNum num => new PrimitiveValue<byte>((byte)num.Int), 
                        IntExpr => new PrimitiveValue<byte>(0), 
                        _ => throw new Exception("Unexpected z3 value.") 
                    };
                    if (type.IsUInt16()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<ushort>((ushort)num.Int),
                        IntExpr => new PrimitiveValue<ushort>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };
                    if (type.IsUInt32()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<uint>(num.UInt),
                        IntExpr => new PrimitiveValue<uint>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };
                    if (type.IsUInt64()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<ulong>(num.UInt64),
                        IntExpr => new PrimitiveValue<ulong>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };

                    if (type.IsSByte()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<sbyte>((sbyte)num.Int),
                        IntExpr => new PrimitiveValue<sbyte>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };
                    if (type.IsInt16()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<short>((short)num.Int),
                        IntExpr => new PrimitiveValue<short>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };
                    if (type.IsInt32()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<int>(num.Int),
                        IntExpr => new PrimitiveValue<int>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };
                    if (type.IsUInt64()) return z3Value switch
                    {
                        IntNum num => new PrimitiveValue<long>(num.Int64),
                        IntExpr => new PrimitiveValue<long>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };

                    if (type.IsSingle()) return z3Value switch
                    {
                        RatNum num => new PrimitiveValue<float>((float)num.Double),
                        RealExpr => new PrimitiveValue<float>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };
                    if (type.IsDouble()) return z3Value switch
                    {
                        RatNum num => new PrimitiveValue<double>(num.Double),
                        RealExpr => new PrimitiveValue<double>(0),
                        _ => throw new Exception("Unexpected z3 value.")
                    };


                    if (!type.IsPrimitive)
                    {
                        // a non primitive type => a location
                        if (!locationMapping.TryGetValue(z3Value, out Location location))
                        {
                            // this z3Value has not yet been allocated
                            // allocate it, we can ignore the heap node for now (some other parts will use it later)
                            // right now, we are only interested in its location
                            if (type.IsArray || type.IsSZArray || type.IsValueArray)
                            {
                                location = result.HeapInfo.InitializeArray(type.Next, 0).Location;
                            }
                            else
                            {
                                location = result.HeapInfo.InitializeObject(type).Location;
                            }
                            locationMapping[z3Value] = location;
                        }

                        return location;
                    }
                }

                throw new Exception("Unexpected type...");
            }


            static int GetDepth(IVariable v)
            {
                if (v is IRootVariable) return 0;
                return GetDepth(((IMemberVariable)v).Parent) + 1;
            }
        }
    }
}
