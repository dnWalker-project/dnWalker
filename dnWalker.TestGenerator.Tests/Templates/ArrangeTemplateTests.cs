using dnWalker.Parameters;
using dnWalker.TestGenerator.Parameters;
using dnWalker.TestGenerator.Templates;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class ArrangeTemplateTests
    {
        private class SimpleDependencyArrangeTemplate : TemplateBase
        {
            private readonly SimpleDependency _dependency;

            public SimpleDependencyArrangeTemplate(SimpleDependency dependency)
            {
                _dependency = dependency;
            }

            public override string TransformText()
            {
                WriteSimpleDepencencyArrange(_dependency);

                return base.TransformText().Trim();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData(10)]
        [InlineData(-1)]
        [InlineData(0)]
        public void Test_PrimitiveValueParameter(int? value)
        {
            IParameterContext context = new ParameterContext();
            var i1 = context.CreateInt32Parameter();
            i1.Value = value;

            string expected = $"int var_{i1.Reference} = {value ?? 0}";

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(i1)).TransformText();
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(new double[0])]
        [InlineData(new double[] { -5, 0, 10 })]
        public void Test_ArrayOfPrimitiveValueParameters(double[] srcArray)
        {
            IParameterContext context = new ParameterContext();
            var ap = context.CreateArrayParameter("System.Double", srcArray == null, srcArray?.Length);
            if (srcArray != null)
            {
                for (int i = 0; i < srcArray.Length; ++i)
                {
                    var dp = context.CreateDoubleParameter();
                    dp.Value = srcArray[i];
                    ap.SetItem(i, dp);
                }
            }

            string expected = $"double[] var_{ap.Reference} = " + (srcArray == null ? "null" : $"new double[{srcArray.Length}]" + ( srcArray.Length > 0 ? $" {{ {string.Join(", ", srcArray)} }}" : string.Empty));

            string result = new SimpleDependencyArrangeTemplate(new SimpleDependency(ap)).TransformText();
            result.Should().Be(expected);
        }
    }
}
