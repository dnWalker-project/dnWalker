using System;
using System.IO;

using dnWalker.TestGenerator.Symbols;

namespace dnWalker.TestGenerator.Templates
{
    public interface IAssertTemplate
    {
        void WriteAssertNull(TextWriter output, TemplateSymbol symbol);
        void WriteAssertNotNull(TextWriter output, TemplateSymbol symbol);

        void WriteAssertEqual(TextWriter output, TemplateSymbol left, TemplateSymbol right);
        void WriteAssertNotEqual(TextWriter output, TemplateSymbol left, TemplateSymbol right);

        void WriteAssertSame(TextWriter output, TemplateSymbol left, TemplateSymbol right);
        void WriteAssertNotSame(TextWriter output, TemplateSymbol left, TemplateSymbol right);


    }
}

