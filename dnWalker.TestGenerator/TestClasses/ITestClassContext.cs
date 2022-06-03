using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestGenerator.Symbolic;
using dnWalker.TestGenerator.Templates;
using dnWalker.TestGenerator.TestClasses.Schemas;
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
        IReadOnlyModel InputModel { get; }
        string ErrorOutput { get; }
        TypeSig Exception { get; }
        IReadOnlyModel OutputModel { get; }
        bool IsFaulted { get; }
        int IterationNumber { get; }
        IMethod Method { get; }
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

            foreach (string ns in ctx.OutputModel.GetNamespaces())
            {
                nsSet.Add(ns);
            }

            if (ctx.Method.ResolveMethodDefThrow().IsStatic)
            {
                nsSet.Add(ctx.Method.DeclaringType.Namespace);
            }

            if (ctx.IsFaulted)
            {
                nsSet.Add(ctx.Exception.Namespace);
            }

            foreach (string u in ctx.Usings)
            {
                nsSet.Add(u);
            }

            string[] nsArr = nsSet.ToArray();
            Array.Sort(nsArr);
            return nsArr;
        }

        public static IEnumerable<TestSchema> GetSchemas(this ITestClassContext ctx)
        {
            List<TestSchema> schemas = new List<TestSchema>();
            if (ctx.IsFaulted)
            {
                schemas.Add(new ExceptionSchema(ctx));

                // a faulted execution - we do not care for any other schemas
                return schemas;
            }
            else
            {
                // add only if we want to explicitly check for it...
                schemas.Add(new ExceptionSchema(ctx));
            }

            IReadOnlyModel inSet = ctx.InputModel;
            IReadOnlyModel outSet = ctx.OutputModel;

            // check the return type
            if (!TypeEqualityComparer.Instance.Equals(ctx.Method.MethodSig.RetType, ctx.Method.Module.CorLibTypes.Void))
            {
                // there is a return value
                schemas.Add(new ReturnValueSchema(ctx));

            }

            // check for state changes - compare
            foreach (IReadOnlyObjectHeapNode changedObject in GetChangedObjects(ctx.InputModel, ctx.OutputModel))
            {
                schemas.Add(new ChangedObjectSchema(changedObject.Location, ctx));
            }
            foreach (IReadOnlyArrayHeapNode changedArray in GetChangedArrays(ctx.InputModel, ctx.OutputModel))
            {
                schemas.Add(new ChangedArraySchema(changedArray.Location, ctx));
            }

            return schemas;
        }


        private static IEnumerable<IReadOnlyHeapNode> GetChangedObjects(IReadOnlyModel inputModel, IReadOnlyModel outputModel)
        {
            return outputModel.HeapInfo.Nodes.OfType<IReadOnlyObjectHeapNode>().Where(static n => n.IsDirty);
        }

        private static IEnumerable<IReadOnlyArrayHeapNode> GetChangedArrays(IReadOnlyModel inputModel, IReadOnlyModel outputModel)
        {
            return outputModel.HeapInfo.Nodes.OfType<IReadOnlyArrayHeapNode>().Where(static n => n.IsDirty);
        }

    }

}