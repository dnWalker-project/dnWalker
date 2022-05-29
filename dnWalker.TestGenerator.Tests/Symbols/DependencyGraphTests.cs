using System;
using System.Collections.Generic;

using dnWalker.TypeSystem;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using dnWalker.TestGenerator.Symbols;

using dnlib.DotNet;

using FluentAssertions;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Symbols
{
    public class DependencyGraphTests
    {
        private readonly IDefinitionProvider _definitionProvider;
        private readonly TypeSig _int32;
        private readonly TypeSig _object;


        public DependencyGraphTests()
        {
            _definitionProvider = new DefinitionProvider(Domain.LoadFromAppDomain(typeof(DependencyGraphTests).Assembly));
            _int32 = _definitionProvider.BaseTypes.Int32;
            _object = _definitionProvider.BaseTypes.Object;
        }

        [Fact]
        public void IndependentSymbols()
        {
            IModel model = new Model(new Constraint());
            IHeapInfo heap = model.HeapInfo;

            IObjectHeapNode objNode1 = heap.InitializeObject(_object);
            IObjectHeapNode objNode2 = heap.InitializeObject(_object);

            TemplateSymbol[] symbols = new[]
            {
                new TemplateSymbol(model, objNode1.Location, objNode1.Type, "obj1"),
                new TemplateSymbol(model, objNode2.Location, objNode2.Type, "obj2"),
            };

            DependencyGraph dg = new DependencyGraph(symbols);
            IEnumerable<List<TemplateSymbol>> symbolGroups = dg.GetSortedSymbolGroups();

            symbolGroups.Should().HaveCount(2);
        }
    }
}

