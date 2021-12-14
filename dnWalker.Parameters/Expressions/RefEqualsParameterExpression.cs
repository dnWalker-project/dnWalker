using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace dnWalker.Parameters.Expressions
{
    public class RefEqualsParameterExpression : BinaryParameterExpression
    {
        public RefEqualsParameterExpression(ParameterReference lhs, ParameterReference rhs) : base(lhs, rhs)
        {
        }

        internal static readonly char IdChar = 'R';

        protected override char Identifier
        {
            get { return IdChar; }
        }

        public override bool TryApplyTo(ParameterStore store, object value)
        {
            static bool BothOfType<T>(IParameter p1, IParameter p2, [NotNullWhen(true)] out T? tp1, [NotNullWhen(true)] out T? tp2)
                where T : class, IParameter
            {
                tp1 = p1 as T;
                tp2 = p2 as T;
                return tp1 != null && tp2 != null;
            }

            static bool AtLeastOneOfType<T>(IParameter p1, IParameter p2)
                where T : class, IParameter
            {
                T? tp1 = p1 as T;
                T? tp2 = p2 as T;
                return tp1 != null || tp2 != null;
            }

            static void ReplaceParameter(ParameterStore store, IParameter oldParameter, IParameter newParameter)
            {
                store.RemoveParameter(oldParameter);
                if (oldParameter.IsRoot())
                {
                    store.AddRootParameter(((RootParameterAccessor)oldParameter.Accessor!).Name, newParameter);
                }
                else
                {
                    oldParameter.Accessor!.ChangeTarget(newParameter);
                    store.AddParameter(newParameter);
                }
                oldParameter.Accessor = null;
            }


            if (Lhs.TryResolve(store, out IParameter? lhsP) &&
                Rhs.TryResolve(store, out IParameter? rhsP) &&
                lhsP is IReferenceTypeParameter refTypeLhs &&
                rhsP is IReferenceTypeParameter refTypeRhs &&
                value is bool boolValue)
            {
                if (boolValue)
                {
                    // lhs must be an alias of rhs or rhs must be an alias of lhs

                    if (refTypeLhs.ParameterReferenceEquals(refTypeRhs))
                    {
                        // already is => do nothing && return true
                        return true;
                    }
                    else
                    {
                        IAliasParameter? aliasLhs = lhsP as IAliasParameter;
                        IAliasParameter? aliasRhs = rhsP as IAliasParameter;

                        if (aliasLhs != null &&
                            aliasRhs == null)
                        {
                            // lhs is an alias, rhs is not => just switch referenced parameter in lhs
                            aliasLhs.ReferencedParameter = refTypeRhs;
                        }
                        else if (aliasLhs == null &&
                                 aliasRhs != null)
                        {
                            // same as before, just switch sides
                            aliasRhs.ReferencedParameter = refTypeLhs;
                        }
                        else if (aliasLhs != null &&
                                 aliasRhs != null)
                        {
                            // both are aliases => make LHS reference RHS target
                            aliasLhs.ReferencedParameter = aliasRhs.GetReferencedParameter();
                        }
                        else if (aliasLhs == null &&
                                 aliasRhs == null)
                        {
                            // neither is an alias
                            // we will make lhs an alias of rhs
                            // => all lhs traits must be merged into rhs
                            // in order to not do an equivalence check (at cost of more concolic execution iterations), we let rhs override lhs traits

                            // we do not care about IsNull, because lhs will never override the rhs traits anyway

                            if (BothOfType(lhsP, rhsP, out IItemOwnerParameter? itemOwnerLhs, out IItemOwnerParameter? itemOwnerRhs))
                            {
                                itemOwnerLhs.TransferTraitsInto(itemOwnerRhs);
                            }
                            else if (AtLeastOneOfType<IItemOwnerParameter>(lhsP, rhsP))
                            {
                                return false;
                            }

                            if (BothOfType(lhsP, rhsP, out IMethodResolverParameter? methodResolverLhs, out IMethodResolverParameter? methodResolverRhs))
                            {
                                methodResolverLhs.TransferTraitsInto(methodResolverRhs);
                            }
                            else if (AtLeastOneOfType<IMethodResolverParameter>(lhsP, rhsP))
                            {
                                return false;
                            }

                            if (BothOfType(lhsP, rhsP, out IFieldOwnerParameter? fieldResolverLhs, out IFieldOwnerParameter? fieldResolverRhs))
                            {
                                fieldResolverLhs.TransferTraitsInto(fieldResolverRhs);
                            }
                            else if (AtLeastOneOfType<IFieldOwnerParameter>(lhsP, rhsP))
                            {
                                return false;
                            }

                            // remove lhs from the store
                            // create an alias of rhs with the same id
                            IParameter newLhs = refTypeRhs.CreateAlias(lhsP.Id);
                            ReplaceParameter(store, lhsP, newLhs);


                            // we must change targets of all lhs aliases
                            foreach (IAliasParameter lhsAlias in store.GetAllParameters()
                                .OfType<IAliasParameter>()
                                .Where(a => a.GetReferencedParameter().Id == lhsP.Id))
                            {
                                lhsAlias.ReferencedParameter = refTypeRhs;
                            }
                        }

                        return true;
                    }
                }
                else
                {
                    // lhs must not be an alias of rhs and rhs must not be an alias of lhs
                    if (!refTypeLhs.ParameterReferenceEquals(refTypeRhs))
                    {
                        // already is => do nothing && return true
                        return true;
                    }
                    else
                    {
                        // at least each of them must be an alias

                        IAliasParameter? aliasLhs = lhsP as IAliasParameter;
                        IAliasParameter? aliasRhs = rhsP as IAliasParameter;

                        if (aliasLhs != null &&
                            aliasRhs == null)
                        {
                            // because the parameters do ref equals, aliasLhs.GetReferencedParameter() will return the rhsP
                            // create a copy of rhsP with the same id as aliasLhs
                            IParameter newLhs = rhsP.ShallowCopy(lhsP.Id);
                            ReplaceParameter(store, lhsP, newLhs);

                        }
                        else if (aliasRhs == null &&
                                 aliasLhs != null)
                        {
                            // same as before with switched sides
                            IParameter newRhs = rhsP.ShallowCopy(rhsP.Id);
                            ReplaceParameter(store, rhsP, newRhs);
                        }
                        else if (aliasLhs != null &&
                                 aliasRhs != null)
                        {
                            // both are aliases => both reference the same parameter => do almost the same thing as with the 1st case
                            IParameter newLhs = aliasRhs.GetReferencedParameter().ShallowCopy(lhsP.Id);
                            ReplaceParameter(store, lhsP, newLhs);
                        }
                    }

                    return true;
                }

            }
            return false;
        }
    }
}

