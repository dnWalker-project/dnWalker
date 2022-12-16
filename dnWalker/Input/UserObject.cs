using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
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

        public void AddMethodResult(MethodDef method, int invocation, UserData result)
        {
            MethodResults[(method, invocation)] = result;
        }

        public IDictionary<MethodDef, MethodBehavior> MethodBehaviors
        {
            get;
        } = new Dictionary<MethodDef, MethodBehavior>(MethodEqualityComparer.CompareDeclaringTypes);


        public void AddMethodBehavior(MethodDef method, Expression expression, UserData result)
        {
            ConditionalResult conditionalResult = new ConditionalResult() { Condition = expression, Result = result };
            if (!MethodBehaviors.TryGetValue(method, out MethodBehavior beh))
            {
                beh = new MethodBehavior();
                MethodBehaviors.Add(method, beh);
            }

            beh.Results.Add(conditionalResult);
        }


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


            foreach (((MethodDef md, int invocation), UserData ud) in MethodResults
                // if there is conditional behavior for the method, use that!!!
                .Where(p => !MethodBehaviors.ContainsKey(p.Key.Item1)))
            {
                objectNode.SetMethodResult(md, invocation, ud.Build(model, md.ReturnType, references));
            }

            foreach ((MethodDef md, MethodBehavior beh) in MethodBehaviors)
            {
                foreach (ConditionalResult r in beh.Results)
                {
                    objectNode.SetConstrainedMethodResult(md, r.Condition, r.Result.Build(model, md.ReturnType, references));
                }
            }

            return objectNode.Location;
        }
    }
}
