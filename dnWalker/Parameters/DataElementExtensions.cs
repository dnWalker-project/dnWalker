using dnWalker.Symbolic;
using dnWalker.TypeSystem;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        public static void SetParameterStore(this ExplicitActiveState cur, ParameterStore store)
        {
            cur.PathStore.CurrentPath.SetPathAttribute("parameter_store", store);
        }

        public static bool TryGetParameterStore(this ExplicitActiveState cur, out ParameterStore store)
        {
            return cur.PathStore.CurrentPath.TryGetPathAttribute("parameter_store", out store);
        }


        public static void SetParameter(this IDataElement dataElement, IParameter parameter, ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, "parameter", parameter);

            if (parameter is IPrimitiveValueParameter primitiveValueParameter)
            {
                // set value parameter expression as well
                //Expression e = primitiveValueParameter.GetValueExpression(cur);
                //dataElement.SetExpression(e, cur);
            }
        }


        public static bool TryGetParameter(this IDataElement dataElement, ExplicitActiveState cur, [NotNullWhen(true)] out IParameter parameter)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute(dataElement, "parameter", out parameter);
        }

        public static bool TryGetParameter<TParameter>(this IDataElement dataElement, ExplicitActiveState cur, [NotNullWhen(true)] out TParameter parameter)
            where TParameter : class, IParameter
        {
            TryGetParameter(dataElement, cur, out IParameter p);
            parameter = p as TParameter;
            return parameter != null;
        }

        public static Int4 GetIsNotNullDataElement(this ExplicitActiveState cur, IReferenceTypeParameter referenceTypeParameter)
        {
            bool isNotNull = !referenceTypeParameter.GetIsNull();
            Int4 de = new Int4(isNotNull ? 1 : 0);
            //de.SetExpression(Expression.Not(referenceTypeParameter.GetIsNullExpression(cur)), cur);
            return de;
        }

        public static Int4 GetIsNullDataElement(this ExplicitActiveState cur, IReferenceTypeParameter referenceTypeParameter)
        {
            bool isNull = referenceTypeParameter.GetIsNull();
            Int4 de = new Int4(isNull ? 1 : 0);
            //de.SetExpression(referenceTypeParameter.GetIsNullExpression(cur), cur);
            return de;
        }

        public static Int4 GetRefsEqualDataElement(this ExplicitActiveState cur, IReferenceTypeParameter lhs, IReferenceTypeParameter rhs)
        {
            if (!cur.TryGetParameterStore(out ParameterStore store))
            {
                throw new Exception("No IParameterContext is registered with this ExplicitActiveState");
            }

            Int4 de = new Int4(lhs.Reference == rhs.Reference ? 1 : 0);
            //de.SetExpression(lhs.GetReferenceEqualsExpression(rhs, cur), cur);
            return de;
        }

        public static UnsignedInt4 GetLengthDataElement(this ExplicitActiveState cur, IArrayParameter array)
        {
            UnsignedInt4 de = new UnsignedInt4((uint)array.GetLength());

            Expression lengthExpression = array.GetLengthExpression(cur);

            //de.SetExpression(lengthExpression, cur);

            // removed - do this magic in the Z3Solver...
            //// Z3 can't work with unsigned integers => all int parameters are signed and amended by GreaterThanOrEqual constraint
            //Expression nonNegativeLength = Expression.GreaterThanOrEqual(lengthExpression, Expression.Constant(0));
            //cur.PathStore.AddPathConstraint(nonNegativeLength, de);

            return de;
        }

        private const string Method2Invocation = "MET2INV";

        public static IDictionary<MethodSignature, int> GetInvocationLookup(this ObjectReference objectReference, ExplicitActiveState cur)
        {
            if (!cur.PathStore.CurrentPath.TryGetPathAttribute(Method2Invocation, out IDictionary<MethodSignature, int> lookup))
            {
                lookup = new Dictionary<MethodSignature, int>();
                cur.PathStore.CurrentPath.SetPathAttribute(Method2Invocation, lookup);
            }

            return lookup;
        }

        public static int IncreaseInvocationCount(this ExplicitActiveState cur, ObjectReference objectReference, MethodSignature methodSignature)
        {
            IDictionary<MethodSignature,int> invocationLookup = objectReference.GetInvocationLookup(cur);
            if (!invocationLookup.TryGetValue(methodSignature, out int invocationCount))
            {
                invocationCount = 0;
                invocationLookup[methodSignature] = invocationCount;
            }
            else
            {
                invocationCount++;
                invocationLookup[methodSignature] = invocationCount;
            }
            return invocationCount;
        }
    }
}
