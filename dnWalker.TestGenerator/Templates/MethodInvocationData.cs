using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public class MethodInvocationData
    {

        private MethodInvocationData(MethodInfo method, ICodeExpression? instance, ICodeExpression[]? positionalArguments = null, OptionalArgAssignment[]? optionalArguments = null)
        {
            Instance = instance;
            Method = method ?? throw new ArgumentNullException(nameof(method));
            PositionalArguments = positionalArguments ?? Array.Empty<ICodeExpression>();
            OptionalArguments = optionalArguments ?? Array.Empty<OptionalArgAssignment>();
        }

        /// <summary>
        /// Specifies the variable used for the instance method invocation. 
        /// If left null, the invocation is for a static method.
        /// </summary>
        public ICodeExpression? Instance { get; }

        public MethodInfo Method { get; }

        public ICodeExpression[] PositionalArguments { get; }

        public OptionalArgAssignment[] OptionalArguments { get; }

        public class Builder
        {
            private readonly MethodInfo _method;
            private readonly ICodeExpression? _instance;

            private List<ICodeExpression> _positional = new List<ICodeExpression>();
            private Dictionary<string, ICodeExpression> _optional = new Dictionary<string, ICodeExpression>();

            private Builder(MethodInfo method, ICodeExpression? instance)
            {
                _method = method ?? throw new ArgumentNullException(nameof(method));
                _instance = instance;
            }

            public static Builder GetStatic(MethodInfo method)
            {
                return new Builder(method, null);
            }

            public static Builder GetInstance(MethodInfo method, ICodeExpression instance)
            {
                if (method is null)
                {
                    throw new ArgumentNullException(nameof(method));
                }

                if (instance is null)
                {
                    throw new ArgumentNullException(nameof(instance));
                }

                return new Builder(method, instance);
            }

            public Builder Positional<TExpr>(TExpr argumentExpression)
                where TExpr : ICodeExpression
            {
                _positional.Add(argumentExpression);
                return this;
            }

            public Builder Optional<TExpr>(string argName, TExpr argumentExpression)
                where TExpr : ICodeExpression
            {
                _optional.Add(argName, argumentExpression);
                return this;
            }

            public MethodInvocationData Build()
            {
                return new MethodInvocationData(_method, _instance, _positional.ToArray(), _optional.Select(p => new OptionalArgAssignment(p.Key, p.Value )).ToArray() );
            }

            public static implicit operator MethodInvocationData(Builder b)
            {
                return b.Build();
            }
        }
    }
}
