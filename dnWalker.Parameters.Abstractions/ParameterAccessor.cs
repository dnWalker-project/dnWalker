using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    /// <summary>
    /// Describes how the parameter is referenced.
    /// </summary>
    public abstract class ParameterAccessor
    {
        public IParameter? Parent { get; }

        protected ParameterAccessor(IParameter? parent)
        {
            Parent = parent;
        }

        public abstract void OnChildSet(IParameter? child);
        public abstract bool TryGetChild([NotNullWhen(true)]out IParameter? child);
        public abstract void ClearChild();
    }

    public class ParameterFieldAccessor : ParameterAccessor
    {
        public string FieldName { get; }

        public new IFieldOwnerParameter Parent
        {
            get { return (IFieldOwnerParameter?)base.Parent ?? throw new Exception("Parent must not be null!"); }
        }

        public ParameterFieldAccessor(string fieldName, IFieldOwnerParameter parent) : base(parent)
        {
            FieldName = fieldName;
        }

        public override void OnChildSet(IParameter? child)
        {
            if (child == null)
            {
                Parent.ClearField(FieldName);
            }
            else
            {
                Parent.SetField(FieldName, child);
            }
        }

        public override bool TryGetChild([NotNullWhen(true)] out IParameter? child)
        {
            return Parent.TryGetField(FieldName, out child);
        }

        public override void ClearChild()
        {
            Parent.ClearField(FieldName);
        }
    }

    public class ParameterItemAccessor : ParameterAccessor
    {
        public int Index { get; }

        public new IItemOwnerParameter Parent
        {
            get { return (IItemOwnerParameter?)base.Parent ?? throw new Exception("Parent must not be null!"); }
        }

        public ParameterItemAccessor(int index, IItemOwnerParameter parent) : base(parent)
        {
            Index = index;
        }

        public override void OnChildSet(IParameter? child)
        {
            if (child == null)
            {
                Parent.ClearItem(Index);
            }
            else
            {
                Parent.SetItem(Index, child);
            }
        }

        public override bool TryGetChild([NotNullWhen(true)] out IParameter? child)
        {
            return Parent.TryGetItem(Index, out child);
        }

        public override void ClearChild()
        {
            Parent.ClearItem(Index);
        }
    }

    public class ParameterMethodResultAccessor : ParameterAccessor
    {
        public MethodSignature MethodSignature { get; }
        public int Invocation { get; }

        public new IMethodResolverParameter Parent
        {
            get { return (IMethodResolverParameter?)base.Parent ?? throw new Exception("Parent must not be null!"); }
        }

        public ParameterMethodResultAccessor(MethodSignature methodSignature, int invocation, IMethodResolverParameter parent) : base (parent)
        {
            MethodSignature = methodSignature;
            Invocation = invocation;
        }

        public override void OnChildSet(IParameter? child)
        {
            if (child == null)
            {
                Parent.ClearMethodResult(MethodSignature, Invocation);
            }
            else
            {
                Parent.SetMethodResult(MethodSignature, Invocation, child);
            }
        }

        public override bool TryGetChild([NotNullWhen(true)] out IParameter? child)
        {
            return Parent.TryGetMethodResult(MethodSignature, Invocation, out child);
        }

        public override void ClearChild()
        {
            Parent.ClearMethodResult(MethodSignature, Invocation);
        }
    }

    public class ParameterRootAccessor : ParameterAccessor
    {
        public string Name { get; }

        public ParameterRootAccessor(string name) : base(null)
        {
            Name = name;
        }

        public override void OnChildSet(IParameter? child)
        { }

        public override bool TryGetChild([NotNullWhen(true)] out IParameter? child)
        {
            child = null;
            return false;
        }
        public override void ClearChild()
        { }
    }
}
