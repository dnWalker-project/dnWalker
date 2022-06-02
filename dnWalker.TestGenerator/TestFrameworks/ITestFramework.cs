using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.TestProjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestFrameworks
{
    /// <summary>
    /// A top level object which creates <see cref="ITestProjectGenerator"> and <see cref="ITestClassGenerator"/>.
    /// </summary>
    public interface ITestFramework
    {
        /// <summary>
        /// Initializes project context.
        /// </summary>
        /// <remarks>
        /// Use this method in order to add package, services references and other project file settings.
        /// </remarks>
        /// <param name="context"></param>
        void InitializeProjectContext(ITestProjectContext context);

        /// <summary>
        /// Initializes class context.
        /// </summary>
        /// <remarks>
        /// Use this method in order to add namespaces or some other parts of the test class.
        /// </remarks>
        /// <param name="context"></param>
        void InitializeClassContext(ITestClassContext context);

        /// <summary>
        /// Creates an instance of <see cref="ITestClassGenerator"/>.
        /// </summary>
        /// <returns></returns>
        ITestClassGenerator CreateClassGenerator();

        /// <summary>
        /// Creates an instance of <see cref="ITestProjectGenerator"/>.
        /// </summary>
        /// <returns></returns>
        ITestProjectGenerator CreateProjectGenerator();
    }
}
