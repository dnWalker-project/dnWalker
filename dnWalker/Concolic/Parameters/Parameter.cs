﻿
using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Expressions = System.Linq.Expressions;

namespace dnWalker.Concolic.Parameters
{
    public abstract class Parameter
    {
        public String Name { get; set; }
        public String TypeName { get; }

        public IList<ParameterTrait> Traits { get; }

        protected Parameter(String typeName) : this(typeName, Enumerable.Empty<ParameterTrait>())
        { }

        protected Parameter(String typeName, IEnumerable<ParameterTrait> traits)
        {
            Traits = new List<ParameterTrait>(traits);
            TypeName = typeName;
        }

        public Boolean TryGetTrait<TTrait>(out TTrait trait) where TTrait : ParameterTrait
        {
            trait = Traits.OfType<TTrait>().FirstOrDefault();
            return trait != null;
        }
        public Boolean TryGetTrait<TTrait>(Func<TTrait, Boolean> predicate, out TTrait trait) where TTrait : ParameterTrait
        {
            trait = Traits.OfType<TTrait>().FirstOrDefault(predicate);
            return trait != null;
        }

        public void AddTrait<TTrait>(TTrait newValue) where TTrait : ParameterTrait
        {
            Traits.Add(newValue);
        }


        public abstract IDataElement CreateDataElement(ExplicitActiveState cur);



        protected abstract Type GetFrameworkType();

        private ParameterExpression _expression = null;

        public ParameterExpression GetExpression()
        {
            if (_expression != null)
            {
                return _expression;
            }

            Type frameworkType = GetFrameworkType();
            if (frameworkType != null)
            {
                _expression = Expression.Parameter(frameworkType, Name);
            }
            return _expression;
        }


        public override String ToString()
        {
            return $"Parameter: {Name}, Type: {TypeName}, Traits: {Traits.Count}";
        }


    }
}
