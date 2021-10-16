using dnlib.DotNet;

using dnWalker.DataElements;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public class ParameterStore
    {
        private readonly List<Parameter> _parameters = new List<Parameter>();

        public IList<Parameter> Parameters
        {
            get 
            {
                return _parameters; 
            }
        }

        public TParameter AddNamedParameter<TParameter>(String name, TParameter parameter) where TParameter : Parameter
        {
            _parameters.Add(parameter);

            if (parameter.TryGetTrait(out NamedParameterTrait t))
            {
                t.Name = name;
            }
            else
            {
                parameter.AddTrait(new NamedParameterTrait(name));
            }

            return parameter;
        }

        public Parameter AddNamedParameter(String name, ITypeDefOrRef parameterType)
        {
            Parameter p = Parameter.CreateParameter(parameterType);

            return AddNamedParameter(name, p);
        }

        public ObjectParameter AddNamedObjectParameter(String name, ITypeDefOrRef parameterType)
        {
            ObjectParameter p = Parameter.CreateObjectParameter(parameterType);

            return AddNamedParameter(name, p);
        }
        public InterfaceParameter AddNamedInterfaceParameter(String name, ITypeDefOrRef parameterType)
        {
            InterfaceParameter p = Parameter.CreateInterfaceParameter(parameterType);

            return AddNamedParameter(name, p);
        }
        public BooleanParameter AddNamedBooleanParameter(String name)
        {
            BooleanParameter p = new BooleanParameter();

            return AddNamedParameter(name, p);
        }

        public CharParameter AddNamedCharParameter(String name)
        {
            CharParameter p = new CharParameter();

            return AddNamedParameter(name, p);
        }
        public ByteParameter AddNamedByteParameter(String name)
        {
            ByteParameter p = new ByteParameter();

            return AddNamedParameter(name, p);
        }
        public SByteParameter AddNamedSByteParameter(String name)
        {
            SByteParameter p = new SByteParameter();

            return AddNamedParameter(name, p);
        }
        public Int16Parameter AddNamedInt16Parameter(String name)
        {
            Int16Parameter p = new Int16Parameter();

            return AddNamedParameter(name, p);
        }
        public Int32Parameter AddNamedInt32Parameter(String name)
        {
            Int32Parameter p = new Int32Parameter();

            return AddNamedParameter(name, p);
        }
        public Int64Parameter AddNamedInt64Parameter(String name)
        {
            Int64Parameter p = new Int64Parameter();

            return AddNamedParameter(name, p);
        }
        public UInt16Parameter AddNamedUInt16Parameter(String name)
        {
            UInt16Parameter p = new UInt16Parameter();

            return AddNamedParameter(name, p);
        }
        public UInt32Parameter AddNamedUInt32Parameter(String name)
        {
            UInt32Parameter p = new UInt32Parameter();

            return AddNamedParameter(name, p);
        }
        public UInt64Parameter AddNamedUInt64Parameter(String name)
        {
            UInt64Parameter p = new UInt64Parameter();

            return AddNamedParameter(name, p);
        }
        public SingleParameter AddNamedSingleParameter(String name)
        {
            SingleParameter p = new SingleParameter();

            return AddNamedParameter(name, p);
        }
        public DoubleParameter AddNamedDoubleParameter(String name)
        {
            DoubleParameter p = new DoubleParameter();

            return AddNamedParameter(name, p);
        }
    }
}
