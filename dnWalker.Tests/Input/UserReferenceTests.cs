using dnWalker.Input;
using dnWalker.Symbolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Input
{
    public class UserReferenceTests : DnlibTestBase
    {
        public UserReferenceTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        [InlineData("\r\n  \t  ")]
        public void BuildNotInitializedReference(string invalidReference)
        {
            Action act = () => new UserReference() { Reference = invalidReference }.Build(new Model(), null, new Dictionary<string, IValue>());
            act.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("myObject")]
        public void BuildExistingValue(string referenceName)
        {
            IValue referencedValue = new StringValue("Hello world");
            IValue resolvedValue = new UserReference() { Reference = referenceName }.Build(new Model(), null, new Dictionary<string, IValue>() { [referenceName] = referencedValue });

            resolvedValue.Should().Be(referencedValue);
        }

        [Theory]
        [InlineData("myObject")]
        public void BuildNonExistingValue(string referenceName)
        {
            Action act = () => new UserReference() { Reference = referenceName }.Build(new Model(), null, new Dictionary<string, IValue>());
            act.Should().Throw<Exception>();
        }
    }
}
