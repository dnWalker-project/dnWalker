using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Examples
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExamplesInlineDataAttribute : ExamplesDataAttribute
    {
        private readonly object[] _data;

        public ExamplesInlineDataAttribute(params object[] data)
        {
            _data = data;
        }

        protected override IEnumerable<object[]> GetDataCore(MethodInfo testMethod)
        {
            return new[] { _data };
        }
    }
}
