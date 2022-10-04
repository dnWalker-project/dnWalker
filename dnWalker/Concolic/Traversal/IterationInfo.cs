using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class IterationInfo
    {
        public static readonly IterationInfo InvalidEmpty = new IterationInfo(null, null, Array.Empty<ConstraintNode>(), Array.Empty<ConstraintNode>());

        private readonly ConstraintNode _selectedNode;
        private readonly IModel _model;


        private readonly IReadOnlyCollection<ConstraintNode> _unsatNodes;
        private readonly IReadOnlyCollection<ConstraintNode> _skippedNodes;

        public IterationInfo(ConstraintNode selectedNode, IModel model, IReadOnlyCollection<ConstraintNode> unsatNodes, IReadOnlyCollection<ConstraintNode> skippedNodes)
        {
            _selectedNode = selectedNode;
            _model = model;
            _unsatNodes = unsatNodes ?? throw new ArgumentNullException(nameof(unsatNodes));
            _skippedNodes = skippedNodes ?? throw new ArgumentNullException(nameof(skippedNodes));
        }

        public ConstraintNode SelectedNode
        {
            get
            {
                return _selectedNode;
            }
        }

        public IModel Model
        {
            get
            {
                return _model;
            }
        }

        public bool IsValid
        {
            get
            {
                return _model != null;
            }
        }

        public IReadOnlyCollection<ConstraintNode> UnsatNodes
        {
            get
            {
                return _unsatNodes;
            }
        }

        public IReadOnlyCollection<ConstraintNode> SkippedNodes
        {
            get
            {
                return _skippedNodes;
            }
        }
    }
}
