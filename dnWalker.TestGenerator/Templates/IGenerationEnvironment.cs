using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    /// <summary>
    /// Represents an output for text generation.
    /// </summary>
    public interface IGenerationEnvironment
    {
        IGenerationEnvironment Append<T>(T value) where T : struct;

        IGenerationEnvironment Append(string value);
        IGenerationEnvironment AppendLine(string value);

        IGenerationEnvironment AppendFormat(IFormatProvider? provider, string format, params object[] args);

        //IGenerationEnvironment Clear();

        IGenerationEnvironment Append(char value, int repeatCount);
        IGenerationEnvironment Append(ReadOnlySpan<char> value);
        IGenerationEnvironment Append(ReadOnlyMemory<char> value);

        //int Length { get; }
    }

    public static class GenerationEnvironmentExtensions
    {
        public static TEnv AppendFormat<TEnv>(this TEnv env, string format, params object[] args)
            where TEnv : IGenerationEnvironment
        {
            env.AppendFormat(null, format, args);
            return env;
        }
        public static TEnv AppendLine<TEnv>(this TEnv env)
            where TEnv : IGenerationEnvironment
        {
            env.AppendLine(string.Empty);
            return env;
        }
    }
}
