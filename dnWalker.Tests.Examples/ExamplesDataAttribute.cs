using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xunit.Sdk;

namespace dnWalker.Tests.Examples
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class ExamplesDataAttribute : DataAttribute
    {
        public ExamplesDataAttribute()
        {
        }

        protected abstract IEnumerable<object[]> GetDataCore(MethodInfo testMethod);

        public sealed override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            IReadOnlyList<BuildInfo> configurations = BuildInfo.GetBuildInfos();

            List<object[]> rows = new List<object[]>();

            foreach (object[] row in GetDataCore(testMethod))
            {
                object[] newRow = new object[row.Length + 1];

                foreach (BuildInfo info in configurations)
                {
                    newRow[0] = info;
                    Array.Copy(row, 0, newRow, 1, row.Length);
                    rows.Add(newRow);
                }
            }

            return rows;
        }
    }
}
