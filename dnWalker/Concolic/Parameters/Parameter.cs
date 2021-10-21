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
        private String _name;

        public String Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

                if (_name != value)
                {
                    _name = value;
                    OnNameChanged(value);
                }
            }
        }

        public Boolean HasName()
        {
            return !String.IsNullOrWhiteSpace(_name);
        }

        public String TypeName { get; }

        //public IList<ParameterTrait> Traits { get; }

        protected virtual void OnNameChanged(String newName)
        {

        }

        protected Parameter(String typeName)
        {
            TypeName = typeName;
        }

        protected Parameter(String typeName, String name)
        {
            if (String.IsNullOrWhiteSpace(typeName)) throw new ArgumentNullException(nameof(typeName));
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            TypeName = typeName;
            Name = name;
            //Traits = new List<ParameterTrait>();
        }

        //public Boolean TryGetTrait<TTrait>(out TTrait trait) where TTrait : ParameterTrait
        //{
        //    trait = Traits.OfType<TTrait>().FirstOrDefault();
        //    return trait != null;
        //}
        //public Boolean TryGetTrait<TTrait>(Func<TTrait, Boolean> predicate, out TTrait trait) where TTrait : ParameterTrait
        //{
        //    trait = Traits.OfType<TTrait>().FirstOrDefault(predicate);
        //    return trait != null;
        //}

        //public void AddTrait<TTrait>(TTrait newValue) where TTrait : ParameterTrait
        //{
        //    Traits.Add(newValue);
        //}


        public abstract IDataElement CreateDataElement(ExplicitActiveState cur);



        //protected abstract Type GetFrameworkType();

        //private ParameterExpression _expression = null;

        //public ParameterExpression GetExpression()
        //{
        //    if (_expression != null)
        //    {
        //        return _expression;
        //    }

        //    Type frameworkType = GetFrameworkType();
        //    if (frameworkType != null)
        //    {
        //        _expression = Expression.Parameter(frameworkType, Name);
        //    }
        //    return _expression;
        //}

        public abstract IEnumerable<ParameterExpression> GetParameterExpressions();
        public abstract Boolean HasSingleExpression { get; }
        public abstract ParameterExpression GetSingleParameterExpression();

        public override String ToString()
        {
            return $"Parameter: {Name}, Type: {TypeName}";
        }

        public abstract Boolean TryGetChildParameter(String name, out Parameter childParameter);
    }
}
