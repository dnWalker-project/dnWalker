using dnlib.DotNet;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public readonly struct MethodArgument
    {
        public string Expression { get; }

        private MethodArgument(string expression)
        {
            Expression = expression;
        }

        public static MethodArgument Positional(string expression)
        {
            return new MethodArgument(expression);
        }

        public static MethodArgument Optional(string argument, string expression)
        {
            return new MethodArgument($"{argument}: {expression}");
        }
    }


    public class MethodInvocationData
    {

        private MethodInvocationData(MethodSignature method, string? instance, MethodArgument[]? arguments = null)
        {
            Instance = instance;
            Method = method;
            Arguments = arguments ?? Array.Empty<MethodArgument>();
        }

        /// <summary>
        /// Specifies the variable used for the instance method invocation. 
        /// If left null, the invocation is for a static method.
        /// </summary>
        public string? Instance { get; }

        public MethodSignature Method { get; }

        public MethodArgument[] Arguments { get; }


        public class Builder
        {
            private readonly MethodSignature _method;
            private readonly string? _instance;

            private readonly List<string> _positional = new List<string>();
            private readonly Dictionary<string, string> _optional = new Dictionary<string, string>();

            private Builder(MethodSignature method, string? instance)
            {
                _method = method;
                _instance = instance;
            }

            public static Builder GetStatic(MethodSignature method)
            {
                return new Builder(method, null);
            }

            public static Builder GetInstance(MethodSignature method, string instance)
            {
                if (string.IsNullOrWhiteSpace(instance))
                {
                    throw new ArgumentException($"'{nameof(instance)}' cannot be null or whitespace.", nameof(instance));
                }

                return new Builder(method, instance);
            }

            public Builder Positional(string expression)
            {
                _positional.Add(expression);
                return this;
            }

            public Builder Optional(string argName, string expression)
            {
                _optional.TryAdd(argName, expression);
                return this;
            }

            public MethodInvocationData Build()
            {
                IEnumerable<MethodArgument> args =
                    Enumerable.Concat
                    (
                        _positional.Select(e => MethodArgument.Positional(e)),
                        _optional.Select(p => MethodArgument.Optional(p.Key, p.Value))
                    );

                return new MethodInvocationData(_method, _instance, args.ToArray());
            }

            public static implicit operator MethodInvocationData(Builder b)
            {
                return b.Build();
            }
        }
    }
}
