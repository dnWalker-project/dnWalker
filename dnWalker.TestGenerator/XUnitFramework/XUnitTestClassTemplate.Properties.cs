using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.XUnitFramework
{
    internal partial class XUnitTestClassTemplate
    {
        private ExplorationIterationData? _iterationData;
        private ExplorationData? _explorationData;

        public ExplorationIterationData IterationData
        {
            get
            {
                return _iterationData ?? throw new InvalidOperationException("The template is not initialized.");
            }
        }

        public ExplorationData ExplorationData
        {
            get
            {
                return _explorationData ?? throw new InvalidOperationException("The template is not initialized.");
            }
        }


        public string GenerateContent(ExplorationData explorationData, ExplorationIterationData iterationData)
        {
            _explorationData = explorationData ?? throw new ArgumentNullException(nameof(explorationData));
            _iterationData = iterationData ?? throw new ArgumentNullException(nameof(iterationData));

            return TransformText();
        }
    }
}
