using dnlib.DotNet;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public abstract class Parameter
    {
        public abstract IDataElement AsDataElement(ExplicitActiveState cur);

        public String TypeName { get; }

        public IList<ParameterTrait> Traits { get; }

        protected Parameter(String typeName) : this(typeName, Enumerable.Empty<ParameterTrait>())
        { }

        protected Parameter(String typeName, IEnumerable<ParameterTrait> traits)
        {
            Traits = new List<ParameterTrait>(traits);
            TypeName = typeName;
        }

        public Boolean TryGetTrait<TTrait>(out TTrait trait) where TTrait : ParameterTrait
        {
            trait = Traits.OfType<TTrait>().FirstOrDefault();
            return trait != null;
        }
        public Boolean TryGetTrait<TTrait>(Func<TTrait, Boolean> predicate, out TTrait trait) where TTrait : ParameterTrait
        {
            trait = Traits.OfType<TTrait>().FirstOrDefault(predicate);
            return trait != null;
        }

        public void AddTrait<TTrait>(TTrait newValue) where TTrait : ParameterTrait
        {
            Traits.Add(newValue);
        }


        public virtual void SetTraits(IDictionary<String, Object> data, ParameterStore parameterStore)
        {
        }

        public virtual IEnumerable<Expressions.ParameterExpression> GetParameterExpressions()
        {
            return Enumerable.Empty<Expressions.ParameterExpression>();
        }

        public static Parameter CreateParameter(ITypeDefOrRef parameterType)
        {
            if (parameterType.IsPrimitive) return CreatePrimitiveValueParameter(parameterType);


            if (parameterType.IsTypeDef)
            {
                TypeDef td = parameterType.ResolveTypeDefThrow();

                if (td.IsClass)
                {
                    return CreateObjectParameter(td);
                }
                else if (td.IsInterface)
                {
                    return CreateInterfaceParameter(td);
                }
                else if (td.IsValueType)
                {
                    throw new NotSupportedException("Not yet supported custom value types...");
                }
                else
                {
                    throw new Exception("Unexpected type, supports classes and interfaces");
                }
            }

            throw new Exception("Could not resolve provided parameter type: " + parameterType.FullName);
        }

        public static InterfaceParameter CreateInterfaceParameter(ITypeDefOrRef typeDefOrRef)
        {
            return new InterfaceParameter(typeDefOrRef);
        }

        public static ObjectParameter CreateObjectParameter(ITypeDefOrRef typeDefOrRef)
        {
            return new ObjectParameter(typeDefOrRef);
        }

        public static PrimitiveValueParameter CreatePrimitiveValueParameter(ITypeDefOrRef typeDefOrRef)
        {
            if (typeDefOrRef.IsPrimitive)
            {
                switch (typeDefOrRef.FullName)
                {
                    //case TypeNames.StringTypeName: return new Int32Parameter();
                    //case TypeNames.ObjectTypeName: return new Int32Parameter();

                    case TypeNames.BooleanTypeName: return new BooleanParameter();
                    case TypeNames.CharTypeName: return new CharParameter();
                    case TypeNames.ByteTypeName: return new ByteParameter();
                    case TypeNames.SByteTypeName: return new SByteParameter();
                    case TypeNames.Int16TypeName: return new Int16Parameter();
                    case TypeNames.Int32TypeName: return new Int32Parameter();
                    case TypeNames.Int64TypeName: return new Int64Parameter();
                    case TypeNames.UInt16TypeName: return new UInt16Parameter();
                    case TypeNames.UInt32TypeName: return new UInt32Parameter();
                    case TypeNames.UInt64TypeName: return new UInt64Parameter();
                    case TypeNames.SingleTypeName: return new SingleParameter();
                    case TypeNames.DoubleTypeName: return new DoubleParameter();

                    default: throw new Exception("Unexpected primitive value parameter.");
                }
            }
            else
            {
                throw new Exception("Provided type is NOT primitive!");
            }
        }
    }
}
