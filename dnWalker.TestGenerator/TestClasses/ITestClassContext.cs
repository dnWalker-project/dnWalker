using dnWalker.Parameters;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;

namespace dnWalker.TestGenerator.TestClasses
{
    public interface ITestClassContext
    {
        string AssemblyFileName { get; }
        string AssemblyName { get; }
        IReadOnlyParameterSet BaseSet { get; }
        IDefinitionProvider DefinitionProvider { get; }
        string ErrorOutput { get; }
        TypeSignature Exception { get; }
        IReadOnlyParameterSet ExecutionSet { get; }
        bool IsFaulted { get; }
        int IterationNumber { get; }
        MethodSignature MethodSignature { get; }
        IParameterContext ParameterContext { get; }
        string PathConstraint { get; }
        string StandardOutput { get; }
        string TestClassName { get; }
        string TestNamespaceName { get; }

        IList<string> Usings { get; }
    }

    public static class TestClassContextExtensions
    {

        public static IEnumerable<string> GetNamespaces(this ITestClassContext ctx)
        {
            HashSet<string> nsSet = new HashSet<string>();

            foreach (IParameter p in ctx.ExecutionSet.Parameters.Values)
            {
                nsSet.Add(p.Type.Namespace);
            }

            if (ctx.MethodSignature.IsStatic)
            {
                nsSet.Add(ctx.MethodSignature.DeclaringType.Namespace);
            }

            if (ctx.IsFaulted)
            {
                nsSet.Add(ctx.Exception.Namespace);
            }

            foreach (string u in ctx.Usings)
            {
                nsSet.Add(u);
            }

            string[] ns = nsSet.ToArray();
            Array.Sort(ns);
            return ns;
        }

        public static IEnumerable<AssertionSchema> GetSchemas(this ITestClassContext ctx)
        {
            List<AssertionSchema> schemas = new List<AssertionSchema>();
            if (ctx.IsFaulted)
            {
                schemas.Add(new ExceptionSchema(ctx.Exception));

                // a faulted execution - we do not care for any other schemas
                return schemas;
            }
            else
            {
                schemas.Add(ExceptionSchema.NoException);
            }

            IReadOnlyParameterSet inSet = ctx.BaseSet;
            IReadOnlyParameterSet outSet = ctx.ExecutionSet;

            // check the return type
            if (!ctx.MethodSignature.ReturnType.IsVoid && !ctx.IsFaulted)
            {
                if (!ctx.ExecutionSet.TryGetReturnValue(out IParameter? rv))
                {
                    throw new Exception("Could not find the return value.");
                }

                // there is a return value
                schemas.Add(new ReturnValueSchema(rv, inSet.Parameters.ContainsKey(rv.Reference)));

            }

            foreach (ParameterRef r in inSet.Parameters.Keys)
            {
                if (r.TryResolve(inSet, out IObjectParameter? inObj) &&
                    r.TryResolve(outSet, out IObjectParameter? outObj))
                {
                    string[] changedFields = GetChangedFields(inObj, outObj).ToArray();
                    if (changedFields.Length > 0)
                    {
                        ObjectFieldSchema schema = new ObjectFieldSchema(r, inSet, outSet, changedFields);
                        schemas.Add(schema);
                    }
                }

                else if (r.TryResolve(inSet, out IArrayParameter? inArr) &&
                         r.TryResolve(outSet, out IArrayParameter? outArr))
                {
                    int[] changedPositions = GetChangedPositions(inArr, outArr).ToArray();
                    if (changedPositions.Length > 0)
                    {
                        ArrayElementSchema schema = new ArrayElementSchema(r, inSet, outSet, changedPositions);
                        schemas.Add(schema);
                    }
                }
            }

            return schemas;
        }


        private static IEnumerable<string> GetChangedFields(IObjectParameter input, IObjectParameter output)
        {
            List<string> changedFields = new List<string>();

            IReadOnlyDictionary<string, ParameterRef> inFields = input.GetFields();
            IReadOnlyDictionary<string, ParameterRef> outFields = output.GetFields();

            // we know that any field that is in the inFields set must be in the outFields set
            // - we need to find all fields which are in the outFields and not in the inFields (i.a. some new fields were assigned)
            // - we need to find all fields which have different value in outFields than in the inFields (i.a. some fields were overwritten)

            foreach (KeyValuePair<string, ParameterRef> fi in outFields)
            {
                string fieldName = fi.Key;
                if (!inFields.ContainsKey(fieldName))
                {
                    // a newly assigned field
                    changedFields.Add(fieldName);
                }
                else if (fi.Value != inFields[fieldName])
                {
                    // a changed field
                    changedFields.Add(fieldName);
                }
            }

            return changedFields;
        }

        private static IEnumerable<int> GetChangedPositions(IArrayParameter input, IArrayParameter output)
        {
            List<int> changedPositions = new List<int>();

            ParameterRef[] inItems = input.GetItems();
            ParameterRef[] outItems = output.GetItems();

            for (int i = 0; i < inItems.Length; ++i)
            {
                if (inItems[i] != outItems[i])
                {
                    changedPositions.Add(i);
                }
            }

            return changedPositions;
        }

    }

}