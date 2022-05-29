using dnlib.DotNet;

using dnWalker.Symbolic;

using System;

namespace dnWalker.TestGenerator.Symbols
{
    public record TemplateSymbol
    {
        public IReadOnlyModel Model { get; }
        public IValue Value { get; }

        public TypeSig Type { get; }
        public string Name { get; }

        public TemplateSymbol(IReadOnlyModel model, IValue value, TypeSig type, string name)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}

