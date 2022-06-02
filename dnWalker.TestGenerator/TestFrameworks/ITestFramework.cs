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
    /// A top level object which creates <see cref="ITestProjectWriter"> and <see cref="ITestClassWriter"/>.
    /// </summary>
    public interface ITestFramework
    {

        /// <summary>
        /// Creates an instance of <see cref="ITestClassWriter"/>.
        /// </summary>
        /// <returns></returns>
        ITestClassWriter CreateClassWriter();

        /// <summary>
        /// Initializes an instance of <see cref="ITestClassContext"/>.
        /// </summary>
        /// <returns></returns>
        void InitializeClassContext(ITestClassContext classContext);

        /// <summary>
        /// Initializes an instance of <see cref="ITestProjectContext"/>.
        /// </summary>
        /// <returns></returns>
        void InitializeProjectContext(ITestProjectContext projectContext);
    }
}
