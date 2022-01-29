using dnWalker.TypeSystem;

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
        public abstract string GetAccessString(IParameterSet context);

        public abstract ParameterAccessor Clone();

        public abstract void ChangeTarget(ParameterRef newTarget, IParameterSet context);

        public const string ThisName = "#__THIS__";
        public const string ReturnValueName = "#__RETURN_VALUE__";
    }

    public abstract class ParentChildParameterAccessor : ParameterAccessor
    {
        public ParameterRef ParentRef { get; }


        protected ParentChildParameterAccessor(ParameterRef parentRef)
        {
            ParentRef = parentRef;
        }

    }

    public class FieldParameterAccessor : ParentChildParameterAccessor
    {
        public string FieldName { get; }


        public FieldParameterAccessor(string fieldName, ParameterRef parent) : base(parent)
        {
            FieldName = fieldName;
        }

        public override string GetAccessString(IParameterSet context)
        {
            context.Parameters.TryGetValue(ParentRef, out IParameter? parent);
            return $"{parent?.GetAccessString() ?? string.Empty}.{FieldName}";
        }

        public override FieldParameterAccessor Clone()
        {
            return new FieldParameterAccessor(FieldName, ParentRef);
        }

        public override void ChangeTarget(ParameterRef newTarget, IParameterSet context)
        {
            ParentRef.Resolve<IFieldOwnerParameter>(context)!.SetField(FieldName, newTarget);
        }
    }

    public class ItemParameterAccessor : ParentChildParameterAccessor
    {
        public int Index { get; }

        public ItemParameterAccessor(int index, ParameterRef parent) : base(parent)
        {
            Index = index;
        }

        public override string GetAccessString(IParameterSet context)
        {
            context.Parameters.TryGetValue(ParentRef, out IParameter? parent);
            return $"{parent?.GetAccessString() ?? string.Empty}[{Index}]";
        }

        public override ItemParameterAccessor Clone()
        {
            return new ItemParameterAccessor(Index, ParentRef);
        }

        public override void ChangeTarget(ParameterRef newTarget, IParameterSet context)
        {
            ParentRef.Resolve<IItemOwnerParameter>(context)!.SetItem(Index, newTarget);
        }
    }

    public class MethodResultParameterAccessor : ParentChildParameterAccessor
    {
        public MethodSignature MethodSignature { get; }
        public int Invocation { get; }

        public MethodResultParameterAccessor(MethodSignature methodSignature, int invocation, ParameterRef parent) : base(parent)
        {
            MethodSignature = methodSignature;
            Invocation = invocation;
        }

        public override string GetAccessString(IParameterSet context)
        {
            context.Parameters.TryGetValue(ParentRef, out IParameter? parent);
            return $"{parent?.GetAccessString() ?? string.Empty}.{MethodSignature.Method.Name}({string.Join(',', MethodSignature.Method.MethodSig.Params.Select(p => p.FullName))})|{Invocation}|";

        }

        public override MethodResultParameterAccessor Clone()
        {
            return new MethodResultParameterAccessor(MethodSignature, Invocation, ParentRef);
        }

        public override void ChangeTarget(ParameterRef newTarget, IParameterSet context)
        {
            ParentRef.Resolve<IMethodResolverParameter>(context)!.SetMethodResult(MethodSignature, Invocation, newTarget);
        }
    }

    public abstract class RootParameterAccessor : ParameterAccessor
    {
        public virtual string Expression { get; }

        public RootParameterAccessor(string name)
        {
            Expression = name;
        }

        public override string GetAccessString(IParameterSet context)
        {
            return Expression;
        }

        public override void ChangeTarget(ParameterRef newTarget, IParameterSet context)
        {
            context.Roots[Expression] = newTarget;
            newTarget.Resolve(context)!.Accessors.Add(this);
        }
    }

    public class MethodArgumentParameterAccessor : RootParameterAccessor
    {
        public MethodArgumentParameterAccessor(string expression) : base(expression)
        {
        }

        public override MethodArgumentParameterAccessor Clone()
        {
            return new MethodArgumentParameterAccessor(Expression);
        }
    }

    public class StaticFieldParameterAccessor : RootParameterAccessor
    {
        public string ClassName { get; }
        public string FieldName { get; }

        public StaticFieldParameterAccessor(string className, string fieldName) : base($"{className}.{fieldName}")
        {
            ClassName = className;
            FieldName = fieldName;
        }

        public override StaticFieldParameterAccessor Clone()
        {
            return new StaticFieldParameterAccessor(ClassName, FieldName);
        }
    }

    public class ReturnValueParameterAccessor : RootParameterAccessor
    {
        public ReturnValueParameterAccessor() : base(ReturnValueName)
        {
        }

        public override ReturnValueParameterAccessor Clone()
        {
            return new ReturnValueParameterAccessor();
        }

        public override string Expression
        {
            get
            {
                return base.Expression;
            }
        }
    }
}
