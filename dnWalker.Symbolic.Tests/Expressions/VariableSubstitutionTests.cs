using dnlib.DotNet;

using dnWalker.Symbolic.Variables;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Symbolic.Tests.Expressions
{
    public class VariableSubstitutionTests
    {
        public class TestClass
        {
            public static int StaticField;
            public int Method() { return 1; }
            public int Field;
        }

        private readonly ICorLibTypes _types;

        private readonly IMethod _theMethod;
        private readonly IField _theField;
        private readonly IField _theStaticField;
        private readonly TypeSig _theType;

        public VariableSubstitutionTests()
        {
            ModuleDef md = ModuleDefMD.Load(typeof(PrintingTests).Assembly.Modules.First());

            _types = md.CorLibTypes;
            TypeDef td = md.Find("dnWalker.Symbolic.Tests.Expressions.VariableSubstitutionTests/TestClass", false);
            _theType = td.ToTypeSig();
            _theMethod = td.FindMethod("Method");
            _theField = td.FindField("Field");
            _theStaticField = td.FindField("StaticField");
        }

        [Fact]
        public void SubstituteNamedVariable()
        {
            IVariable x = new NamedVariable(_theType, "x");
            IVariable y = new NamedVariable(_theType, "y");

            IVariable xSubs = x.Substitute(x, y);
            xSubs.Should().Be(y);
        }

        [Fact]
        public void SubstituteFieldVariable()
        {
            IVariable x = new NamedVariable(_theType, "x");
            IVariable y = new NamedVariable(_theType, "y");

            IVariable xField = new InstanceFieldVariable(x, _theField);
            IVariable yField = new InstanceFieldVariable(y, _theField);

            IVariable xFieldSubs = xField.Substitute(x, y);
            xFieldSubs.Should().Be(yField);
        }

        [Fact]
        public void SubstituteMethodResultVariable()
        {
            IVariable x = new NamedVariable(_theType, "x");
            IVariable y = new NamedVariable(_theType, "y");

            IVariable xMethod = new MethodResultVariable(x, _theMethod, 0);
            IVariable yMethod = new MethodResultVariable(y, _theMethod, 0);

            IVariable xMethodSubs = xMethod.Substitute(x, y);
            xMethodSubs.Should().Be(yMethod);
        }

        [Fact]
        public void SubstituteArrayElementVariable()
        {
            IVariable x = new NamedVariable(new SZArraySig(_theType), "x");
            IVariable y = new NamedVariable(new SZArraySig(_theType), "y");

            IVariable xElem = new ArrayElementVariable(x, 0);
            IVariable yElem = new ArrayElementVariable(y, 0);

            IVariable xElemSubs = xElem.Substitute(x, y);
            xElemSubs.Should().Be(yElem);
        }

        [Fact]
        public void SubstituteArrayLengthVariable()
        {
            IVariable x = new NamedVariable(new SZArraySig(_theType), "x");
            IVariable y = new NamedVariable(new SZArraySig(_theType), "y");

            IVariable xLength = new ArrayLengthVariable(x);
            IVariable yLength = new ArrayLengthVariable(y);

            IVariable xLengthSubs = xLength.Substitute(x, y);
            xLengthSubs.Should().Be(yLength);
        }
    }
}
