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

        public abstract string GetAccessString();

        public virtual void ChangeTarget(IParameter newTarget)
        {
            newTarget.Accessor = this;
        }
    }

    public class FieldParameterAccessor : ParameterAccessor
    {
        public string FieldName { get; }

        public new IFieldOwnerParameter Parent
        {
            get { return (IFieldOwnerParameter?)base.Parent ?? throw new Exception("Parent must not be null!"); }
        }

        public override void ChangeTarget(IParameter newTarget)
        {
            base.ChangeTarget(newTarget);

            Parent.ClearField(FieldName);
            Parent.SetField(FieldName, newTarget);
        }

        public FieldParameterAccessor(string fieldName, IFieldOwnerParameter parent) : base(parent)
        {
            FieldName = fieldName;
        }

        public override string GetAccessString()
        {
            return $".{FieldName}";
        }
    }

    public class ItemParameterAccessor : ParameterAccessor
    {
        public int Index { get; }

        public new IItemOwnerParameter Parent
        {
            get { return (IItemOwnerParameter?)base.Parent ?? throw new Exception("Parent must not be null!"); }
        }

        public override void ChangeTarget(IParameter newTarget)
        {
            base.ChangeTarget(newTarget);

            Parent.ClearItem(Index);
            Parent.SetItem(Index, newTarget);
        }

        public ItemParameterAccessor(int index, IItemOwnerParameter parent) : base(parent)
        {
            Index = index;
        }

        public override string GetAccessString()
        {
            return $"[{Index}]";
        }
    }

    public class MethodResultParameterAccessor : ParameterAccessor
    {
        public MethodSignature MethodSignature { get; }
        public int Invocation { get; }

        public new IMethodResolverParameter Parent
        {
            get { return (IMethodResolverParameter?)base.Parent ?? throw new Exception("Parent must not be null!"); }
        }

        public override void ChangeTarget(IParameter newTarget)
        {
            base.ChangeTarget(newTarget);

            Parent.ClearMethodResult(MethodSignature, Invocation);
            Parent.SetMethodResult(MethodSignature, Invocation, newTarget);
        }

        public MethodResultParameterAccessor(MethodSignature methodSignature, int invocation, IMethodResolverParameter parent) : base(parent)
        {
            MethodSignature = methodSignature;
            Invocation = invocation;
        }

        public override string GetAccessString()
        {
            return $".{MethodSignature.MethodName}({string.Join(',', MethodSignature.ArgumentTypeFullNames)})[{Invocation}]";
        }
    }

    public class RootParameterAccessor : ParameterAccessor
    {
        public virtual string Name { get; }

        public RootParameterAccessor(string name) : base(null)
        {
            Name = name;
        }

        public override string GetAccessString()
        {
            return Name;
        }
    }

    public class MethodArgumentParameterAccessor : RootParameterAccessor
    {
        public MethodArgumentParameterAccessor(string name) : base(name)
        {
        }
    }

    public class StaticFieldParameterAccessor : RootParameterAccessor
    {
        public StaticFieldParameterAccessor(string className, string fieldName) : base($"{className}.{fieldName}")
        {

        }
    }
}
