using dnlib.DotNet;

using dnWalker.DataElements;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DnParameter = dnlib.DotNet.Parameter;

namespace dnWalker.Parameters
{
    public static partial class ParameterStoreExtensions
    {
        public static void EnsureMethodParameters(this ParameterStore store, MethodDef method)
        {
            if (method.HasThis)
            {
                if (!store.TryGetRootParameter(ParameterStore.ThisName, out IParameter parameter))
                {
                    // it is not specified => make it default
                    parameter = ParameterFactory.CreateParameter(method.DeclaringType.ToTypeSig());
                    store.AddRootParameter(ParameterStore.ThisName, parameter);
                }
            }

            foreach (DnParameter p in method.Parameters)
            {
                if (!store.TryGetRootParameter(p.Name, out IParameter parameter))
                {
                    // it is not specified => make it default
                    parameter = ParameterFactory.CreateParameter(p.Type);
                    store.AddRootParameter(p.Name, parameter);
                }
            }
        }

        public static DataElementList CreateMethodArguments(this ParameterStore store, MethodDef method, ExplicitActiveState cur)
        {
            int parameterCount = method.GetParamCount() + (method.HasThis ? 1 : 0);
            DataElementList arguments = cur.StorageFactory.CreateList(parameterCount);

            int idx = 0;

            if (method.HasThis)
            {
                if (store.TryGetRootParameter(ParameterStore.ThisName, out IParameter parameter))
                {
                    arguments[idx++] = parameter.AsDataElement(cur);
                }
                else
                {
                    throw new Exception("Hidden THIS parameter is not defined in the store.");
                }
            }

            foreach (DnParameter p in method.Parameters)
            {
                if (store.TryGetRootParameter(p.Name, out IParameter parameter))
                {
                    arguments[idx++] = parameter.AsDataElement(cur);
                }
                else
                {
                    throw new Exception($"Parameter {p.Name} is not defined in the store.");
                }
            }

            return arguments;
        }

        public static void SetReturnValue(this ParameterStore store, ReturnValue retValue, ExplicitActiveState cur)
        {
            // TODO
        }

        public static void NextGeneration(this ParameterStore store, IDictionary<string, object> data)
        {
            // data is in format:
            // <KIND><ID 1><ID 2>...<ID N>
            // V - value of, single id - primitive value only
            // N - is null, single id - reference types only
            // L - length, single id - array only
            // E - reference equals, two ids  - refernce types only

            const char ValueOf = 'V';
            const char Null = 'N';
            const char LengthOf = 'L';
            const char RefEquals = 'E';

            HashSet<int> toPrune = new HashSet<int>(store.GetAllParameters().Select(p => p.Id));
                
            static int GetId(string key)
            {
                return Convert.ToInt32(key, 16);
            }

            void MarkToKeep(IParameter p)
            {
                // walk from the parameter through the accessors until the roots
                // remove them from the toPrune hashset
                int id = p.Id;
                if (!toPrune.Contains(id))
                {
                    // already marked to keep
                    return;
                }

                toPrune.Remove(id);

                p = p.Accessor?.Parent;
                if (p != null)
                {
                    MarkToKeep(p);
                }

                // if it is null, it is either because
                // the accessor was not set (should not be possible)
                // or the parameter is root, so no need to climb further
            }


            bool IsValueOf(string key, out IPrimitiveValueParameter p)
            {
                if (key[0] == ValueOf)
                {
                    int id = GetId(key.Substring(1));
                    store.TryGetParameter(id, out IParameter parameter);
                    p = parameter as IPrimitiveValueParameter;

                    return p != null;
                }
                p = null;
                return false;
            }
            bool IsNull(string key, out IReferenceTypeParameter p)
            {

                if (key[0] == Null)
                {
                    int id = GetId(key.Substring(1));
                    store.TryGetParameter(id, out IParameter parameter);
                    p = parameter as IReferenceTypeParameter;

                    return p != null;
                }
                p = null;
                return false;
            }
            bool IsLengthOf(string key, out IArrayParameter p)
            {
                if (key[0] == LengthOf)
                {
                    int id = GetId(key.Substring(1));
                    store.TryGetParameter(id, out IParameter parameter);
                    p = parameter as IArrayParameter;

                    return p != null;
                }
                p = null;
                return false;
            }
            bool IsReferenceEquals(string key, out IReferenceTypeParameter lhs, out IReferenceTypeParameter rhs)
            {

                if (key[0] == RefEquals)
                {
                    int id1 = GetId(key.Substring(1, 4));
                    int id2 = GetId(key.Substring(5));
                    store.TryGetParameter(id1, out IParameter p1);
                    store.TryGetParameter(id2, out IParameter p2);
                    lhs = p1 as IReferenceTypeParameter;
                    rhs = p2 as IReferenceTypeParameter;

                    return lhs != null && rhs != null;
                }
                lhs = null;
                rhs = null;
                return false;
            }



            foreach (KeyValuePair<string, object> pair in data)
            {
                string key = pair.Key;

                if (IsValueOf(key, out IPrimitiveValueParameter primitiveValueParameter))
                {
                    primitiveValueParameter.Value = pair.Value;
                    MarkToKeep(primitiveValueParameter);
                }

                else if (IsNull(key, out IReferenceTypeParameter referenceTypeParameter))
                {
                    bool value = (bool)pair.Value;
                    referenceTypeParameter.IsNull = value;
                    MarkToKeep(referenceTypeParameter);
                }

                else if (IsLengthOf(key, out IArrayParameter arrayParameter))
                {
                    int length = (int)pair.Value;
                    arrayParameter.Length = length;
                    MarkToKeep(arrayParameter);
                }

                else if (IsReferenceEquals(key, out IReferenceTypeParameter lhs, out IReferenceTypeParameter rhs))
                {
                    bool value = (bool)pair.Value;

                    if (value)
                    {
                        lhs.SetReferenceEquals(rhs);
                        rhs.SetReferenceEquals(lhs);
                    }
                    else
                    {
                        lhs.ClearReferenceEquals(rhs);
                        rhs.ClearReferenceEquals(lhs);
                    }
                    MarkToKeep(lhs);
                    MarkToKeep(rhs);
                }

                else
                {
                    throw new Exception($"Unexpected expression evaluation: {key}");
                }
            }

            foreach (IParameter parameter in toPrune
                .Select(i => { store.TryGetParameter(i, out IParameter parameter); return parameter; })
                .Where(parameter => parameter != null))
            {
                store.PruneParameter(parameter);
            }
        }
    }
}
