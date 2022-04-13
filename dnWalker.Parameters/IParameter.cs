using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IParameter
    {

        IParameterSet Set { get; }

        ParameterRef Reference { get; }

        /// <summary>
        /// Clones data only. Result has no accessors.
        /// </summary>
        /// <param name="newContext"></param>
        /// <returns></returns>
        IParameter CloneData(IParameterSet newContext);

        /// <summary>
        /// Clones data and the accessors.
        /// </summary>
        /// <param name="newContext"></param>
        /// <returns></returns>
        public IParameter Clone(IParameterSet newContext)
        {
            IParameter clone = CloneData(newContext);
            foreach (var a in this.Accessors)
            {
                clone.Accessors.Add(a.Clone());
            }
            return clone;
        }

        IList<ParameterAccessor> Accessors { get; }

        TypeSignature Type { get; }
    }

    public static class ParameterExtensions
    {
        public static bool IsRoot(this IParameter parameter)
        {
            return parameter.Accessors.OfType<RootParameterAccessor>().Any();
        }

        public static bool IsRoot<TAccessor>(this IParameter parameter, out TAccessor[] rootAccessors)
            where TAccessor : ParameterAccessor
        {
            rootAccessors = parameter.Accessors.OfType<TAccessor>().ToArray();
            return rootAccessors.Length > 0;
        }

        public static bool HasAccessor<TAccessor>(this IParameter parameter, out TAccessor[] accessors)
        {
            accessors = parameter.Accessors.OfType<TAccessor>().ToArray(); 
            return accessors.Length > 0;
        }

        public static bool IsField(this IParameter parameter, out IFieldOwnerParameter[] fieldOwners, out string[] fieldNames)
        {
            HasAccessor(parameter, out FieldParameterAccessor[] fieldAccessors);
            
            fieldOwners = fieldAccessors.Select(fa => fa.ParentRef.Resolve<IFieldOwnerParameter>(parameter.Set)!).ToArray();
            fieldNames = fieldAccessors.Select(fa => fa.FieldName).ToArray();

            return fieldAccessors.Length > 0;

            //if (parameter.Accessor is FieldParameterAccessor fieldAccessor &&
            //    fieldAccessor.ParentRef.TryResolve(parameter.Context, out fieldOwner))
            //{
            //    fieldName = fieldAccessor.FieldName;
            //    return true;
            //}

            //fieldOwner = null;
            //fieldName = null;
            //return false;
        }

        public static bool IsItem(this IParameter parameter, out IItemOwnerParameter[] itemOwners, out int[] indeces)
        {
            HasAccessor(parameter, out ItemParameterAccessor[] itemAccessors);

            itemOwners = itemAccessors.Select(ia => ia.ParentRef.Resolve<IItemOwnerParameter>(parameter.Set)!).ToArray();
            indeces = itemAccessors.Select(ia => ia.Index).ToArray();

            return itemAccessors.Length > 0;



            //if (parameter.Accessor is ItemParameterAccessor itemAccessor &&
            //    itemAccessor.ParentRef.TryResolve(parameter.Context, out itemOwner))
            //{
            //    index = itemAccessor.Index;
            //    return true;
            //}

            //itemOwner = null;
            //index = 0;
            //return false;
        }

        public static bool IsMethodResult(this IParameter parameter, out IMethodResolverParameter[] methodResolvers, out MethodSignature[] methodSignatures, out int[] invocations)
        {
            HasAccessor(parameter, out MethodResultParameterAccessor[] methodResultAccessors);

            methodResolvers = methodResultAccessors.Select(mr => mr.ParentRef.Resolve<IMethodResolverParameter>(parameter.Set)!).ToArray();
            methodSignatures = methodResultAccessors.Select(mr => mr.MethodSignature).ToArray();
            invocations = methodResultAccessors.Select(mr => mr.Invocation).ToArray();

            return methodResultAccessors.Length > 0;


            //if (parameter.Accessor is MethodResultParameterAccessor methodResultAccessor &&
            //    methodResultAccessor.ParentRef.TryResolve(parameter.Context, out methodResolver))
            //{
            //    invocation = methodResultAccessor.Invocation;
            //    methodSignature = methodResultAccessor.MethodSignature;
            //    return true;
            //}

            //methodResolver = null;
            //methodSignature = MethodSignature.Empty;
            //invocation = 0;
            //return false;
        }

        [Obsolete("Doesn't work with circular dependencies!!!!")]
        public static string GetAccessString(this IParameter parameter, IDictionary<ParameterRef, string>? accessContext = null)
        {
            if (accessContext == null) accessContext = new Dictionary<ParameterRef,string>();

            if (accessContext.TryGetValue(parameter.Reference, out string? accessString)) return accessString;

            //if (parameter.Accessor == null) return string.Empty;

            //return parameter.Accessor.GetAccessString(parameter.Context);

            // build access string for all accessor and pick the shortest non empty one
            IEnumerable<string> accessStrings = parameter.Accessors.Select(a => a.GetAccessString(parameter.Set, accessContext)).Where(s => !string.IsNullOrWhiteSpace(s));

            accessString = "";
            int lenght = int.MaxValue;

            foreach (string acc in accessStrings)
            {
                if (acc.Length < lenght)
                {
                    accessString = acc;
                    lenght = acc.Length;
                }
            }

            accessContext[parameter.Reference] = accessString;

            return accessString;
        }
    }
}
