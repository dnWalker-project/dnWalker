using dnlib.DotNet;

namespace dnWalker.Symbolic
{
    public abstract class HeapTerm : Term
    {
    }

    public class PointToTerm : HeapTerm
    {
        public PointToTerm(IVariable source, IReadOnlyCollection<KeyValuePair<object, IVariable>> attributes)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
        }

        public IVariable Source { get; }
        public IReadOnlyCollection<KeyValuePair<object, IVariable>> Attributes { get; }

        public override void GetVariables(ICollection<IVariable> variables)
        {
            variables.Add(Source);
            foreach (KeyValuePair<object, IVariable> attribute in Attributes)
            {
                switch (attribute.Key)
                {
                    case IField fld: variables.Add(Variable.InstanceField(Source, fld)); break;
                    case int index: variables.Add(Variable.ArrayElement(Source, index)); break;
                    //case (IMethod method, int invocation): variables.Add(Variable.MethodResult(Source, method, invocation)); break;
                    default: throw new InvalidOperationException($"Provided attribute has invalid type: {attribute.Key.GetType()}");
                }

                variables.Add(attribute.Value);
            }
        }

        public override string ToString()
        {
            return $"{Source.Name} -> {Source.Type}({string.Join(", ", Attributes.Select(p => $"{p.Key} := {p.Value.Name}"))})";
        }
    }
}