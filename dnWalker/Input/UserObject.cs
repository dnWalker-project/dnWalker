using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public class UserObject : UserData
    {
        public TypeSig Type
        {
            get;
            set;
        }

        public IDictionary<FieldDef, UserData> Fields
        {
            get;
        } = new Dictionary<FieldDef, UserData>();

        public IDictionary<(MethodDef, int), UserData> MethodResults
        {
            get;
        } = new Dictionary<(MethodDef, int), UserData>();

        public override IValue Build(IModel model, TypeSig expectedType, IDictionary<string, IValue> references)
        {
            TypeSig type = Type ?? expectedType ?? throw new InvalidOperationException();

            // assert type is assignable to expectedType !!!!

            IObjectHeapNode objectNode = model.HeapInfo.InitializeObject(type);
            SetReference(objectNode.Location, references);

            foreach ((FieldDef fd, UserData ud) in Fields)
            {
                objectNode.SetField(fd, ud.Build(model, fd.FieldType, references));
            }

            foreach (((MethodDef md, int invocation), UserData ud) in MethodResults)
            {
                objectNode.SetMethodResult(md, invocation, ud.Build(model, md.ReturnType, references));
            }

            return objectNode.Location;
        }
    }
}
