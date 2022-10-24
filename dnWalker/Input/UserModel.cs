using dnlib.DotNet;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public class UserModel
    {
        public UserModel()
        {
            Data = new Dictionary<string, UserData>();
        }

        public UserModel(IReadOnlyDictionary<string, UserData> sharedData)
        {
            Data = new Dictionary<string, UserData>(sharedData);
        }

        public IDictionary<string, UserData> Data { get; }

        public MethodDef Method { get; set; }

        public IDictionary<Parameter, UserData> MethodArguments { get; } = new Dictionary<Parameter, UserData>();
        public IDictionary<FieldDef, UserData> StaticFields { get; } = new Dictionary<FieldDef, UserData>();

        public IModel Build()
        {
            Model model = new Model();
            Dictionary<string, IValue> references = new Dictionary<string, IValue>();

            // build the shared data
            foreach ((string id, UserData userData) in Data)
            {
                IValue value = userData.Build(model, null, references);
                references[id] = value;
            }

            // build static fields
            foreach ((FieldDef fd, UserData userData) in StaticFields)
            {
                IValue fieldValue = userData.Build(model, fd.FieldType, references);
                model.SetValue((IRootVariable)Variable.StaticField(fd), fieldValue);
            }

            // build method arguments
            foreach ((Parameter p, UserData userData) in MethodArguments)
            {
                IValue argValue = userData.Build(model, p.Type, references);
                model.SetValue((IRootVariable)Variable.MethodArgument(p), argValue);
            }

            return model;
        }
    }
}
