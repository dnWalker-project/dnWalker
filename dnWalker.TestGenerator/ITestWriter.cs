using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    /// <summary>
    /// Represents a test suit context used to generate test project, test fixtures and test cases.
    /// </summary>
    public interface ITestWriter
    {
        /// <summary>
        /// Creates the class file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        CodeWriter CreateTestClassFile(string filename);

        /// <summary>
        /// Adds basic and concrete usings and declares the testing namespace.
        /// </summary>
        /// <param name="codeWriter"></param>
        /// <param name="testNamespaceName"></param>
        /// <returns></returns>
        IDisposable BeginTestNamespace(CodeWriter codeWriter, string testNamespaceName);

        /// <summary>
        /// Declares the test class with proper traits and other attributes.
        /// </summary>
        /// <param name="codeWriter"></param>
        /// <param name="testClassName"></param>
        /// <returns></returns>
        IDisposable BeginTestClass(CodeWriter codeWriter, string testClassName, IDictionary<string, string>? traits = null);

        /// <summary>
        /// Declares the test method.
        /// </summary>
        /// <param name="codeWriter"></param>
        /// <param name="testMethodName"></param>
        /// <param name="explorationData"></param>
        /// <param name="traits"></param>
        /// <returns></returns>
        IDisposable BeginTestMethod(CodeWriter codeWriter, string testMethodName, ExplorationData explorationData, IDictionary<string, string>? traits = null);

    }
}
