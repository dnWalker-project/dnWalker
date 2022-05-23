using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace dnWalker.Tests.Examples
{
    public class ExamplesDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public ExamplesDiscoverer(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink ?? throw new ArgumentNullException(nameof(diagnosticMessageSink));
        }

        protected IMessageSink DiagnosticMessageSink => _diagnosticMessageSink;

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            string? skipReason = factAttribute.GetNamedArgument<string>("Skip");
            if (skipReason != null)
            {
                return CreateTestCasesForSkip(discoveryOptions, testMethod, factAttribute, skipReason);
            }

            IEnumerable<BuildInfo> builds = BuildInfo.GetBuildInfos();
            try
            {

                IAttributeInfo[] dataAttributes = testMethod.Method.GetCustomAttributes(typeof(DataAttribute)).ToArray();
                if (dataAttributes.Length > 0)
                {
                    // 1st case - the ExamplesTest attribute & a collection of ExamplesData attributes => generate for each data row on test case per build info
                    List<IXunitTestCase> results = new List<IXunitTestCase>();
                    foreach (IAttributeInfo dataAttribute in dataAttributes)
                    {
                        results.AddRange(CreateTestCasesForDataRow(dataAttribute, builds, discoveryOptions, testMethod, factAttribute));
                    }
                    return results;
                }
                else
                {
                    // 2nd case - only the ExamplesTest attribute => generate one test case per build info
                    return CreateTestCases(builds, discoveryOptions, testMethod, factAttribute);

                }

            }
            catch (Exception exception)
            {
                _diagnosticMessageSink.OnMessage(new DiagnosticMessage($"Exception thrown during examples test discovery on '{testMethod.TestClass.Class.Name}.{testMethod.Method.Name}'; falling back to single test case.{Environment.NewLine}{exception}"));
            }

            return CreateTestCases(builds, discoveryOptions, testMethod, factAttribute);
        }

        private IEnumerable<IXunitTestCase> CreateTestCases(IEnumerable<BuildInfo> builds, ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            List<IXunitTestCase> results = new List<IXunitTestCase>();
            foreach (BuildInfo build in builds)
            {
                results.Add(new XunitTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, new[] { build }));
            }
            return results;
        }

        private IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(IAttributeInfo dataAttribute, IEnumerable<BuildInfo> builds, ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            List<IXunitTestCase> results = new List<IXunitTestCase>();
            IReflectionAttributeInfo? reflectionDataAttribute = dataAttribute as IReflectionAttributeInfo;
            IReflectionMethodInfo? reflectionTestMethod = testMethod as IReflectionMethodInfo;

            if (reflectionDataAttribute != null && reflectionTestMethod != null)
            {
                IEnumerable<object[]> argArrays = ((DataAttribute)reflectionDataAttribute.Attribute).GetData(reflectionTestMethod.MethodInfo);

                foreach (object[] argArray in argArrays)
                {
                    foreach (BuildInfo build in builds)
                    {
                        object[] newArgArray = new object[argArray.Length + 1];
                        newArgArray[0] = build;
                        argArray.CopyTo(newArgArray, 1);
                        results.Add(new XunitTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, newArgArray));
                    }
                }

                return results;
            }

            _diagnosticMessageSink.OnMessage(new DiagnosticMessage("The dataAttribute is not reflectionAttributeInfo!"));
            return results;
        }

        private IEnumerable<IXunitTestCase> CreateTestCasesForSkip(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute, string skipReason)
        {
            throw new NotImplementedException();
        }
    }
}
