using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public interface IRuntimeTemplate
    {
        IDictionary<string, object> Session { get; set; } 
        CompilerErrorCollection Errors { get; } 

        IGenerationEnvironment GenerationEnvironment { get; set; }

        string TransformText();

        void Initialize();

        void Warning(string message);
        void Error(string message);

        string CurrentIndent { get; }

        void PushIndent(string indent);
        string PopIndent();
        void ClearIndent();

        void Write(string textToAppend);

        void WriteLine(string textToAppend);

        void Write(string format, params object[] args);

        void WriteLine(string format, params object[] args);

        ToStringInstanceHelper ToStringHelper { get; }
    }

    public static class RuntimeTemplateExtensions
    {
        public static string GetText(this IRuntimeTemplate template)
        {
            // we want just to get the text => use the string builder generation environment
            var environment = ObjectPool<StringBuilderGenerationEnvironment>.Rent();

            string value = template.TransformText();

            environment.Clear();

            ObjectPool<StringBuilderGenerationEnvironment>.Return(environment);

            return value;
        }

        public static void WriteTo(this IRuntimeTemplate template, Stream output)
        {

        }
    }
}
